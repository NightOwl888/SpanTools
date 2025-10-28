// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE.txt file or at https://opensource.org/licenses/MIT.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using SpanTools.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SpanTools
{
    [Generator]
    public sealed class ValueStringBuilderGenerator : IIncrementalGenerator
    {
        private static readonly Version Version_2_0 = new Version(2, 0);
        private static readonly Version Version_2_1 = new Version(2, 1);
        private static readonly Version Version_2_2 = new Version(2, 2);

        void IIncrementalGenerator.Initialize(IncrementalGeneratorInitializationContext context)
        {
            // ---------- props provider (single ValueStringBuilderOptions) ----------
            // Get any AdditionalText named ValueStringBuilder.Generator.props (0..1)
            var propsArrayProvider = context.AdditionalTextsProvider
                .Where(at => Path.GetFileName(at.Path)
                    .Equals("ValueStringBuilder.Generator.props", StringComparison.OrdinalIgnoreCase))
                .Collect(); // now we have IncrementalValueProvider<ImmutableArray<AdditionalText>>

            // Convert the (possible) array into a single ValueStringBuilderOptions instance
            var propsProvider = propsArrayProvider
                .Select((arr, _) =>
                {
                    if (arr.IsDefaultOrEmpty)
                    {
                        return new ValueStringBuilderOptions(); // defaults
                    }

                    // Use first file if multiple present
                    var first = arr[0];
                    return ParsePropsFile(first);
                })
                // prevent regeneration when options haven’t changed
                .WithComparer(EqualityComparer<ValueStringBuilderOptions>.Default);

            // ---------- 2RootNamespace provider (from MSBuild globals) ----------
            var rootNsProvider = context.AnalyzerConfigOptionsProvider
                .Select((provider, _) =>
                {
                    provider.GlobalOptions.TryGetValue("build_property.RootNamespace", out var rootNs);
                    return rootNs ?? string.Empty;
                })
                .WithComparer(StringComparer.Ordinal);

            // ---------- combine compilation + options ----------
            // propsProvider is IncrementalValueProvider<ValueStringBuilderOptions>
            // context.CompilationProvider is IncrementalValueProvider<Compilation>
            // rootNsProvider is IncrementalValueProvider<string>
            // Combined type: (Compilation, (RootNamespace, ValueStringBuilderOptions))
            var combined = context.CompilationProvider.Combine(rootNsProvider.Combine(propsProvider));

            // ---------- register generation ----------
            context.RegisterSourceOutput(combined, (spc, pair) =>
            {
                //var (compilation, options) = pair;
                var compilation = pair.Left;
                var (rootNamespace, options) = pair.Right;

                options.AllowUnsafeBlocks = ((CSharpCompilationOptions)compilation.Options).AllowUnsafe;
                options.Namespace ??= rootNamespace;

                spc.AddSource("ValueStringBuilder.g.cs", SourceText.From(Generators.GenerateValueStringBuilder(options), Encoding.UTF8));
                spc.AddSource("ValueStringBuilder.Appendable.g.cs", SourceText.From(Generators.GenerateValueStringBuilderAppendable(options), Encoding.UTF8));
                spc.AddSource("ValueStringBuilder.Casing.g.cs", SourceText.From(Generators.GenerateValueStringBuilderCasing(options), Encoding.UTF8));
                if (options.IncludesJ2N_2_2_Or_Greater && options.IncludeCodepointSupport)
                {
                    spc.AddSource("ValueStringBuilder.Codepoints.g.cs", SourceText.From(Generators.GenerateValueStringBuilderCodepoints(options), Encoding.UTF8));
                }
                spc.AddSource("ValueStringBuilder.IndexOf.g.cs", SourceText.From(Generators.GenerateValueStringBuilderIndexOf(options), Encoding.UTF8));
                spc.AddSource("ValueStringBuilder.Mutate.g.cs", SourceText.From(Generators.GenerateValueStringBuilderMutate(options), Encoding.UTF8));
                spc.AddSource("ValueStringBuilder.Numbers.g.cs", SourceText.From(Generators.GenerateValueStringBuilderNumbers(options), Encoding.UTF8));
            });
        }

        // ---------- helper to parse AdditionalText into options ----------
        private static ValueStringBuilderOptions ParsePropsFile(AdditionalText additionalText)
        {
            var options = new ValueStringBuilderOptions();

            SourceText? sourceText = additionalText.GetText();
            if (sourceText is null) return options;

            string text = sourceText.ToString();

            foreach (var line in text.SplitLines())
            {
                ReadOnlySpan<char> s = line.Text.Trim();
                // Allow comments to start with #
                if (s.IsEmpty || s.StartsWith("#".AsSpan())) continue;

                // support both "key=value" and "key = value"
                int index = s.IndexOf('=');
                if (index < 0) continue;

                ReadOnlySpan<char> key = s.Slice(0, index).Trim();
                ReadOnlySpan<char> value = s.Slice(index + 1).Trim();

                if (key.Equals("ValueStringBuilder_Namespace".AsSpan(), StringComparison.OrdinalIgnoreCase))
                {
                    if (!value.IsEmpty)
                    {
                        options.Namespace = value.ToString();
                    }
                }
                else if (key.Equals("ValueStringBuilder_IncludeMaxLengthTracking".AsSpan(), StringComparison.OrdinalIgnoreCase))
                {
                    options.IncludeMaxLengthTracking = IsTrue(value, options.IncludeMaxLengthTracking);
                }
                else if (key.Equals("ValueStringBuilder_IncludeAsMemoryMethods".AsSpan(), StringComparison.OrdinalIgnoreCase))
                {
                    options.IncludeAsMemory = IsTrue(value, options.IncludeAsMemory);
                }
                else if (key.Equals("ValueStringBuilder_IncludesJ2N".AsSpan(), StringComparison.OrdinalIgnoreCase))
                {
                    options.IncludesJ2N = IsTrue(value, defaultValue: false);
                }
                else if (key.Equals("ValueStringBuilder_J2NPackageVersion".AsSpan(), StringComparison.Ordinal))
                {
                    if (!value.IsEmpty)
                    {
                        int preIndex = value.IndexOf('-');
                        ReadOnlySpan<char> numeric = preIndex < 0 ? value : value.Slice(0, preIndex);
                        if (Version.TryParse(numeric.ToString(), out var version))
                        {
                            options.IncludesJ2N_2_0_Or_Greater = (version >= Version_2_0);
                            options.IncludesJ2N_2_1_Or_Greater = (version >= Version_2_1);
                            options.IncludesJ2N_2_2_Or_Greater = (version >= Version_2_2);
                        }
                    }
                }
                else if (key.Equals("ValueStringBuilder_IncludeCodepointSupport".AsSpan(), StringComparison.OrdinalIgnoreCase))
                {
                    options.IncludeCodepointSupport = IsTrue(value, options.IncludeCodepointSupport);
                }
                else if (key.Equals("ValueStringBuilder_UseJavaStyleFormatting".AsSpan(), StringComparison.OrdinalIgnoreCase))
                {
                    options.UseJavaStyleFormatting = IsTrue(value, options.UseJavaStyleFormatting);
                }
            }

            return options;
        }

        private static bool IsTrue(ReadOnlySpan<char> value, bool defaultValue)
        {
            bool result = value.Equals("true".AsSpan(), StringComparison.OrdinalIgnoreCase);
            if (result)
                return result;
            else
                return defaultValue;
        }
    }
}
