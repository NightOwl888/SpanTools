// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE.txt file or at https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.Text;

namespace SpanTools
{
    internal static partial class Generators
    {
        public static string GenerateValueStringBuilderCasing(ValueStringBuilderOptions options)
        {
            var sb = new StringBuilder();
            var cb = new CodeBuilder(sb);

            cb.WriteLine("using System;");
            cb.WriteLine("using System.Diagnostics;");
            cb.WriteLine("using System.Globalization;");
            cb.WriteLine("#nullable enable");
            cb.WriteLine();
            cb.WriteLineIf(!string.IsNullOrWhiteSpace(options.Namespace), $"namespace {options.Namespace};");
            cb.WriteLine();
            cb.WriteLine("internal ref partial struct ValueStringBuilder");
            cb.WriteLine("{");
            cb.WriteLine("#if !NET5_0_OR_GREATER");
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Appends the upper case string representation of a specified string");
                cb.WriteLine("/// to this instance using the casing rules from the specified culture.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">The string to append.</param>");
                cb.WriteLine("/// <param name=\"culture\">An object that supplies culture-specific casing rules.</param>");
                cb.WriteLine("/// <remarks>The capacity of this instance is adjusted as needed.</remarks>");
                cb.WriteLine("public void AppendUpper(string value, CultureInfo culture)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Debug.Assert(value != null);");
                    cb.WriteLine("Debug.Assert(culture != null);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("AppendUpper(value.AsSpan(), culture!);");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine("#endif");
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Appends the upper case string representation of a specified read-only character");
                cb.WriteLine("/// span to this instance using the casing rules from the specified culture.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">The read-only character span to append.</param>");
                cb.WriteLine("/// <param name=\"culture\">An object that supplies culture-specific casing rules.</param>");
                cb.WriteLine("/// <remarks>The capacity of this instance is adjusted as needed.</remarks>");
                cb.WriteLine("public void AppendUpper(ReadOnlySpan<char> value, CultureInfo culture)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Debug.Assert(culture != null);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("int valueLength = value.Length;");
                    cb.WriteLine("int pos = _pos;");
                    cb.WriteLine("if (pos > _chars.Length - valueLength)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Grow(valueLength);");
                    });
                    cb.WriteLine("}");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("int length = value.ToUpper(_chars.Slice(_pos), culture);");
                    cb.WriteLine("while (length < 0) // rare");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Grow(valueLength);");
                        cb.WriteLine("length = value.ToUpper(_chars.Slice(_pos), culture);");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine("_pos += length;");
                    cb.WriteLineIf(options.IncludeMaxLengthTracking, "UpdateMaxLength();");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.WriteLine("#if !NET5_0_OR_GREATER");
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Appends the lower case string representation of a specified string");
                cb.WriteLine("/// to this instance using the casing rules from the specified culture.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">The string to append.</param>");
                cb.WriteLine("/// <param name=\"culture\">An object that supplies culture-specific casing rules.</param>");
                cb.WriteLine("/// <remarks>The capacity of this instance is adjusted as needed.</remarks>");
                cb.WriteLine("public void AppendLower(string value, CultureInfo culture)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Debug.Assert(value != null);");
                    cb.WriteLine("Debug.Assert(culture != null);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("AppendLower(value.AsSpan(), culture!);");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine("#endif");
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Appends the lower case string representation of a specified read-only character");
                cb.WriteLine("/// span to this instance using the casing rules from the specified culture.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">The read-only character span to append.</param>");
                cb.WriteLine("/// <param name=\"culture\">An object that supplies culture-specific casing rules.</param>");
                cb.WriteLine("/// <remarks>The capacity of this instance is adjusted as needed.</remarks>");
                cb.WriteLine("public void AppendLower(ReadOnlySpan<char> value, CultureInfo culture)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Debug.Assert(culture != null);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("int valueLength = value.Length;");
                    cb.WriteLine("int pos = _pos;");
                    cb.WriteLine("if (pos > _chars.Length - valueLength)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Grow(valueLength);");
                    });
                    cb.WriteLine("}");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("int length = value.ToLower(_chars.Slice(_pos), culture);");
                    cb.WriteLine("while (length < 0) // rare");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Grow(valueLength);");
                        cb.WriteLine("length = value.ToLower(_chars.Slice(_pos), culture);");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine("_pos += length;");
                    cb.WriteLineIf(options.IncludeMaxLengthTracking, "UpdateMaxLength();");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.WriteLine();
            cb.WriteLine("#if !NET5_0_OR_GREATER");
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Appends the upper case string representation of a specified string");
                cb.WriteLine("/// to this instance using the casing rules from the invariant culture.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">The string to append.</param>");
                cb.WriteLine("/// <remarks>The capacity of this instance is adjusted as needed.</remarks>");
                cb.WriteLine("public void AppendUpperInvariant(string value)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Debug.Assert(value != null);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("AppendUpperInvariant(value.AsSpan());");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine("#endif");
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Appends the upper case string representation of a specified read-only character");
                cb.WriteLine("/// span to this instance using the casing rules from the invariant culture.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">The read-only character span to append.</param>");
                cb.WriteLine("/// <remarks>The capacity of this instance is adjusted as needed.</remarks>");
                cb.WriteLine("public void AppendUpperInvariant(ReadOnlySpan<char> value)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("int valueLength = value.Length;");
                    cb.WriteLine("int pos = _pos;");
                    cb.WriteLine("if (pos > _chars.Length - valueLength)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Grow(valueLength);");
                    });
                    cb.WriteLine("}");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("int length = value.ToUpperInvariant(_chars.Slice(_pos));");
                    cb.WriteLine("Debug.Assert(length >= 0, \"The invariant culture should never require expanding the buffer to more characters than the original value\");");
                    cb.WriteLine("_pos += length;");
                    cb.WriteLineIf(options.IncludeMaxLengthTracking, "UpdateMaxLength();");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.WriteLine("#if !NET5_0_OR_GREATER");
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Appends the lower case string representation of a specified string");
                cb.WriteLine("/// to this instance using the casing rules from the invariant culture.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">The string to append.</param>");
                cb.WriteLine("/// <remarks>The capacity of this instance is adjusted as needed.</remarks>");
                cb.WriteLine("public void AppendLowerInvariant(string value)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Debug.Assert(value != null);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("AppendLowerInvariant(value.AsSpan());");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine("#endif");
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Appends the lower case string representation of a specified read-only character");
                cb.WriteLine("/// span to this instance using the casing rules from the invariant culture.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">The read-only character span to append.</param>");
                cb.WriteLine("/// <remarks>The capacity of this instance is adjusted as needed.</remarks>");
                cb.WriteLine("public void AppendLowerInvariant(ReadOnlySpan<char> value)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("int valueLength = value.Length;");
                    cb.WriteLine("int pos = _pos;");
                    cb.WriteLine("if (pos > _chars.Length - valueLength)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Grow(valueLength);");
                    });
                    cb.WriteLine("}");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("int length = value.ToLowerInvariant(_chars.Slice(_pos));");
                    cb.WriteLine("Debug.Assert(length >= 0, \"The invariant culture should never require expanding the buffer to more characters than the original value\");");
                    cb.WriteLine("_pos += length;");
                    cb.WriteLineIf(options.IncludeMaxLengthTracking, "UpdateMaxLength();");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine("}");


            return sb.ToString();
        }
    }
}
