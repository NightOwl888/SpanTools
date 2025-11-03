using System;
using System.Collections.Generic;
using System.Text;

namespace SpanTools
{
    internal static partial class Generators
    {
        public static void WriteNamespace(CodeBuilder cb, ValueStringBuilderOptions options)
        {
            cb.WriteLineIf(!string.IsNullOrWhiteSpace(options.Namespace) && !options.Namespace!.Equals("global::", StringComparison.OrdinalIgnoreCase), $"namespace {options.Namespace};");
        }
    }
}
