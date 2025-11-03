// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE.txt file or at https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.Text;

namespace SpanTools
{
    internal static partial class Generators
    {

        public static string GenerateValueStringBuilderNumbers(ValueStringBuilderOptions options)
        {
            var sb = new StringBuilder();
            var cb = new CodeBuilder(sb);

            cb.WriteLine("using System;");
            cb.WriteLine("using System.Buffers;");
            cb.WriteLine("using System.Collections.Generic;");
            cb.WriteLine("using System.Globalization;");
            cb.WriteLine("using System.Linq;");
            cb.WriteLine("using System.Text;");
            cb.WriteLine("using System.Threading.Tasks;");
            cb.WriteLine("#nullable enable");
            cb.WriteLine();
            WriteNamespace(cb, options);
            cb.WriteLine();
            cb.WriteLine("internal ref partial struct ValueStringBuilder");
            cb.WriteLine("{");
            if (!options.UseJavaStyleFormatting && options.IncludesJ2N_2_2_Or_Greater)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("private const string GeneralFormat = \"G\"; // For .NET-style formatting");
                    cb.WriteLine();
                });
            }
            cb.WriteLine("#if NET6_0_OR_GREATER");
            cb.IndentBlock(() =>
            {
                cb.WriteLine("internal void AppendSpanFormattable<T>(T value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null) where T : ISpanFormattable");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("int charsWritten = 0;");
                    cb.WriteLine("while (!value.TryFormat(_chars.Slice(_pos), out charsWritten, format, provider))");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Grow(16);");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine();
                    cb.WriteLine("_pos += charsWritten;");
                    cb.WriteLineIf(options.IncludeMaxLengthTracking, "UpdateMaxLength();");
                });
                cb.WriteLine("}");
                cb.WriteLine();
                cb.WriteLine("internal void InsertSpanFormattable<T>(int index, T value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null) where T : ISpanFormattable");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("char[]? arrayToReturnToPool = null;");
                    cb.WriteLine("Span<char> buffer = stackalloc char[CharStackBufferSize];");
                    cb.WriteLine("int charsWritten = 0;");
                    cb.WriteLine("try");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("while (!value.TryFormat(buffer, out charsWritten, format, provider))");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("int newLength = buffer.Length * 2;");
                            cb.WriteLine("buffer = (arrayToReturnToPool = ArrayPool<char>.Shared.Rent(newLength));");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine();
                        cb.WriteLine("Insert(index, buffer.Slice(0, charsWritten), 1);");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine("finally");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("if (arrayToReturnToPool != null)");
                        cb.WriteLine("ArrayPool<char>.Shared.Return(arrayToReturnToPool);");
                    });
                    cb.WriteLine("}");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine("#endif");
            cb.WriteLine();
            cb.WriteLine();

            WriteNumericDoc(cb, "Append", "sbyte", options);
            if (options.IncludesJ2N_2_2_Or_Greater)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("//[CLSCompliant(false)]");
                    cb.WriteLine("public void Append(sbyte value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLineIf(options.UseJavaStyleFormatting, "provider ??= NumberFormatInfo.InvariantInfo; // For JDK-style formatting");
                        cb.WriteLine("int charsWritten = 0;");
                        cb.WriteLine();
                        cb.WriteLine("if (_pos > _chars.Length - 4)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("Grow(4);");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine();
                        cb.WriteLine("while (!J2N.Numerics.SByte.TryFormat(value, _chars.Slice(_pos), out charsWritten, format, provider))");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("Grow(8); // rare");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine();
                        cb.WriteLine("_pos += charsWritten;");
                        cb.WriteLineIf(options.IncludeMaxLengthTracking, "UpdateMaxLength();");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("public void Append(sbyte value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                    cb.WriteLine("{");
                });
                cb.WriteLine("#if NET6_0_OR_GREATER");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("AppendSpanFormattable(value, format, provider);");
                    });
                });
                cb.WriteLine("#else");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("string formatStr = format.IsEmpty");
                        cb.WriteLine("    ? string.Empty");
                        cb.WriteLine("    : format.ToString();");
                        cb.WriteLine();
                        cb.WriteLine("Append(value.ToString(formatStr, provider));");
                    });
                });
                cb.WriteLine("#endif");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }

            WriteNumericDoc(cb, "Append", "byte", options);
            if (options.IncludesJ2N_2_2_Or_Greater)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("public void Append(byte value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLineIf(options.UseJavaStyleFormatting, "provider ??= NumberFormatInfo.InvariantInfo; // For JDK-style formatting");
                        cb.WriteLine("int charsWritten = 0;");
                        cb.WriteLine();
                        cb.WriteLine("if (_pos > _chars.Length - 4)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("Grow(4);");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine();
                        cb.WriteLine("while (!J2N.Numerics.Byte.TryFormat(value, _chars.Slice(_pos), out charsWritten, format, provider))");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("Grow(8); // rare");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine();
                        cb.WriteLine("_pos += charsWritten;");
                        cb.WriteLineIf(options.IncludeMaxLengthTracking, "UpdateMaxLength();");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("public void Append(byte value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                    cb.WriteLine("{");
                });
                cb.WriteLine("#if NET6_0_OR_GREATER");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("AppendSpanFormattable(value, format, provider);");
                    });
                });
                cb.WriteLine("#else");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("string formatStr = format.IsEmpty");
                        cb.WriteLine("    ? string.Empty");
                        cb.WriteLine("    : format.ToString();");
                        cb.WriteLine();
                        cb.WriteLine("Append(value.ToString(formatStr, provider));");
                    });
                });
                cb.WriteLine("#endif");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }

            WriteNumericDoc(cb, "Append", "short", options);
            if (options.IncludesJ2N_2_2_Or_Greater)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("public void Append(short value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLineIf(options.UseJavaStyleFormatting, "provider ??= NumberFormatInfo.InvariantInfo; // For JDK-style formatting");
                        cb.WriteLine("int charsWritten = 0;");
                        cb.WriteLine();
                        cb.WriteLine("if (_pos > _chars.Length - 6)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("Grow(6);");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine();
                        cb.WriteLine("while (!J2N.Numerics.Int16.TryFormat(value, _chars.Slice(_pos), out charsWritten, format, provider))");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("Grow(16); // rare");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine();
                        cb.WriteLine("_pos += charsWritten;");
                        cb.WriteLineIf(options.IncludeMaxLengthTracking, "UpdateMaxLength();");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("public void Append(short value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                    cb.WriteLine("{");
                });
                cb.WriteLine("#if NET6_0_OR_GREATER");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("AppendSpanFormattable(value, format, provider);");
                    });
                });
                cb.WriteLine("#else");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("string formatStr = format.IsEmpty");
                        cb.WriteLine("    ? string.Empty");
                        cb.WriteLine("    : format.ToString();");
                        cb.WriteLine();
                        cb.WriteLine("Append(value.ToString(formatStr, provider));");
                    });
                });
                cb.WriteLine("#endif");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }

            WriteNumericDoc(cb, "Append", "int", options);
            if (options.IncludesJ2N_2_2_Or_Greater)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("public void Append(int value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLineIf(options.UseJavaStyleFormatting, "provider ??= NumberFormatInfo.InvariantInfo; // For JDK-style formatting");
                        cb.WriteLine("int charsWritten = 0;");
                        cb.WriteLine();
                        cb.WriteLine("if (_pos > _chars.Length - 11)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("Grow(11);");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine();
                        cb.WriteLine("while (!J2N.Numerics.Int32.TryFormat(value, _chars.Slice(_pos), out charsWritten, format, provider))");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("Grow(16); // rare");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine();
                        cb.WriteLine("_pos += charsWritten;");
                        cb.WriteLineIf(options.IncludeMaxLengthTracking, "UpdateMaxLength();");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("public void Append(int value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                    cb.WriteLine("{");
                });
                cb.WriteLine("#if NET6_0_OR_GREATER");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("AppendSpanFormattable(value, format, provider);");
                    });
                });
                cb.WriteLine("#else");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("string formatStr = format.IsEmpty");
                        cb.WriteLine("    ? string.Empty");
                        cb.WriteLine("    : format.ToString();");
                        cb.WriteLine();
                        cb.WriteLine("Append(value.ToString(formatStr, provider));");
                    });
                });
                cb.WriteLine("#endif");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }

            WriteNumericDoc(cb, "Append", "long", options);
            if (options.IncludesJ2N_2_2_Or_Greater)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("public void Append(long value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLineIf(options.UseJavaStyleFormatting, "provider ??= NumberFormatInfo.InvariantInfo; // For JDK-style formatting");
                        cb.WriteLine("int charsWritten = 0;");
                        cb.WriteLine();
                        cb.WriteLine("if (_pos > _chars.Length - 20)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("Grow(20);");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine();
                        cb.WriteLine("while (!J2N.Numerics.Int64.TryFormat(value, _chars.Slice(_pos), out charsWritten, format, provider))");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("Grow(32); // rare");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine();
                        cb.WriteLine("_pos += charsWritten;");
                        cb.WriteLineIf(options.IncludeMaxLengthTracking, "UpdateMaxLength();");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("public void Append(long value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                    cb.WriteLine("{");
                });
                cb.WriteLine("#if NET6_0_OR_GREATER");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("AppendSpanFormattable(value, format, provider);");
                    });
                });
                cb.WriteLine("#else");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("string formatStr = format.IsEmpty");
                        cb.WriteLine("    ? string.Empty");
                        cb.WriteLine("    : format.ToString();");
                        cb.WriteLine();
                        cb.WriteLine("Append(value.ToString(formatStr, provider));");
                    });
                });
                cb.WriteLine("#endif");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }

            WriteNumericDoc(cb, "Append", "ushort", options);
            cb.IndentBlock(() =>
            {
                cb.WriteLine("//[CLSCompliant(false)]");
                cb.WriteLine("public void Append(ushort value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLineIf(options.UseJavaStyleFormatting && options.IncludesJ2N_2_2_Or_Greater, "provider ??= NumberFormatInfo.InvariantInfo; // For JDK-style formatting");
                });
            });
            cb.WriteLine("#if NET6_0_OR_GREATER");
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("AppendSpanFormattable(value, format, provider);");
                });
            });
            cb.WriteLine("#else");
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("string formatStr = format.IsEmpty");
                    cb.WriteLine("    ? string.Empty");
                    cb.WriteLine("    : format.ToString();");
                    cb.WriteLine();
                    cb.WriteLine("Append(value.ToString(formatStr, provider));");
                });
            });
            cb.WriteLine("#endif");
            cb.IndentBlock(() =>
            {
                cb.WriteLine("}");
                cb.WriteLine();
            });

            WriteNumericDoc(cb, "Append", "uint", options);
            cb.IndentBlock(() =>
            {
                cb.WriteLine("//[CLSCompliant(false)]");
                cb.WriteLine("public void Append(uint value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLineIf(options.UseJavaStyleFormatting && options.IncludesJ2N_2_2_Or_Greater, "provider ??= NumberFormatInfo.InvariantInfo; // For JDK-style formatting");
                });
            });
            cb.WriteLine("#if NET6_0_OR_GREATER");
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("AppendSpanFormattable(value, format, provider);");
                });
            });
            cb.WriteLine("#else");
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("string formatStr = format.IsEmpty");
                    cb.WriteLine("    ? string.Empty");
                    cb.WriteLine("    : format.ToString();");
                    cb.WriteLine();
                    cb.WriteLine("Append(value.ToString(formatStr, provider));");
                });
            });
            cb.WriteLine("#endif");
            cb.IndentBlock(() =>
            {
                cb.WriteLine("}");
                cb.WriteLine();
            });

            WriteNumericDoc(cb, "Append", "ulong", options);
            cb.IndentBlock(() =>
            {
                cb.WriteLine("//[CLSCompliant(false)]");
                cb.WriteLine("public void Append(ulong value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLineIf(options.UseJavaStyleFormatting && options.IncludesJ2N_2_2_Or_Greater, "provider ??= NumberFormatInfo.InvariantInfo; // For JDK-style formatting");
                });
            });
            cb.WriteLine("#if NET6_0_OR_GREATER");
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("AppendSpanFormattable(value, format, provider);");
                });
            });
            cb.WriteLine("#else");
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("string formatStr = format.IsEmpty");
                    cb.WriteLine("    ? string.Empty");
                    cb.WriteLine("    : format.ToString();");
                    cb.WriteLine();
                    cb.WriteLine("Append(value.ToString(formatStr, provider));");
                });
            });
            cb.WriteLine("#endif");
            cb.IndentBlock(() =>
            {
                cb.WriteLine("}");
                cb.WriteLine();
                cb.WriteLine("// Floating Point");
                cb.WriteLine();
            });


            cb.IndentBlock(() =>
            {
                cb.WriteLine("// Floating Point");
                cb.WriteLine();
            });

            WriteNumericDoc(cb, "Append", "float", options);
            if (options.IncludesJ2N_2_2_Or_Greater)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("public void Append(float value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null) // Requires J2N");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLineIf(options.UseJavaStyleFormatting, "provider ??= NumberFormatInfo.InvariantInfo; // For JDK-style formatting");
                        if (!options.UseJavaStyleFormatting)
                        {
                            cb.WriteLine("if (format.IsEmpty)");
                            cb.WriteLine("{");
                            cb.WriteLine("    format = GeneralFormat.AsSpan(); // For .NET-style formatting");
                            cb.WriteLine("}");
                            cb.WriteLine();
                        }
                        cb.WriteLine("if (_pos > _chars.Length - 14)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("Grow(14);");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine();
                        cb.WriteLine("int charsWritten = 0;");
                        cb.WriteLine("while (!J2N.Numerics.Single.TryFormat(value, _chars.Slice(_pos), out charsWritten, format, provider))");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("Grow(16);");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine();
                        cb.WriteLine("_pos += charsWritten;");
                        cb.WriteLineIf(options.IncludeMaxLengthTracking, "UpdateMaxLength();");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("public void Append(float value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                    cb.WriteLine("{");
                });
                cb.WriteLine("#if NET6_0_OR_GREATER");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("AppendSpanFormattable(value, format, provider);");
                    });
                });
                cb.WriteLine("#else");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("string formatStr = format.IsEmpty");
                        cb.WriteLine("    ? string.Empty");
                        cb.WriteLine("    : format.ToString();");
                        cb.WriteLine();
                        cb.WriteLine("Append(value.ToString(formatStr, provider));");
                    });
                });
                cb.WriteLine("#endif");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }


            WriteNumericDoc(cb, "Append", "double", options);
            if (options.IncludesJ2N_2_2_Or_Greater)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("public void Append(double value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null) // Requires J2N");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLineIf(options.UseJavaStyleFormatting, "provider ??= NumberFormatInfo.InvariantInfo; // For JDK-style formatting");
                        if (!options.UseJavaStyleFormatting)
                        {
                            cb.WriteLine("if (format.IsEmpty)");
                            cb.WriteLine("{");
                            cb.WriteLine("    format = GeneralFormat.AsSpan(); // For .NET-style formatting");
                            cb.WriteLine("}");
                        }
                        cb.WriteLine();
                        cb.WriteLine("if (_pos > _chars.Length - 24)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("Grow(24);");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine();
                        cb.WriteLine("int charsWritten = 0;");
                        cb.WriteLine("while (!J2N.Numerics.Double.TryFormat(value, _chars.Slice(_pos), out charsWritten, format, provider))");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("Grow(16);");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine();
                        cb.WriteLine("_pos += charsWritten;");
                        cb.WriteLineIf(options.IncludeMaxLengthTracking, "UpdateMaxLength();");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("public void Append(double value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                    cb.WriteLine("{");
                });
                cb.WriteLine("#if NET6_0_OR_GREATER");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("AppendSpanFormattable(value, format, provider);");
                    });
                });
                cb.WriteLine("#else");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("string formatStr = format.IsEmpty");
                        cb.WriteLine("    ? string.Empty");
                        cb.WriteLine("    : format.ToString();");
                        cb.WriteLine();
                        cb.WriteLine("Append(value.ToString(formatStr, provider));");
                    });
                });
                cb.WriteLine("#endif");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }

            WriteNumericDoc(cb, "Append", "decimal", options);
            cb.IndentBlock(() =>
            {
                cb.WriteLine("public void Append(decimal value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLineIf(options.UseJavaStyleFormatting && options.IncludesJ2N_2_2_Or_Greater, "provider ??= NumberFormatInfo.InvariantInfo; // For JDK-style formatting");
                });
            });
            cb.WriteLine("#if NET6_0_OR_GREATER");
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("AppendSpanFormattable(value, format, provider);");
                });
            });
            cb.WriteLine("#else");
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("string formatStr = format.IsEmpty");
                    cb.WriteLine("    ? string.Empty");
                    cb.WriteLine("    : format.ToString();");
                    cb.WriteLine();
                    cb.WriteLine("Append(value.ToString(formatStr, provider));");
                });
            });
            cb.WriteLine("#endif");
            cb.IndentBlock(() =>
            {
                cb.WriteLine("}");
                cb.WriteLine();
                cb.WriteLine("// Insert");
                cb.WriteLine();
            });

            WriteNumericDoc(cb, "Insert", "sbyte", options);
            if (options.IncludesJ2N_2_2_Or_Greater)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("//[CLSCompliant(false)]");
                    cb.WriteLine("public void Insert(int index, sbyte value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLineIf(options.UseJavaStyleFormatting, "provider ??= NumberFormatInfo.InvariantInfo; // For JDK-style formatting");
                        cb.WriteLine("char[]? arrayToReturnToPool = null;");
                        cb.WriteLine("Span<char> buffer = stackalloc char[CharStackBufferSize];");
                        cb.WriteLine("int charsWritten = 0;");
                        cb.WriteLine("try");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("while (!J2N.Numerics.SByte.TryFormat(value, buffer, out charsWritten, format, provider))");
                            cb.WriteLine("{");
                            cb.IndentBlock(() =>
                            {
                                cb.WriteLine("int newLength = buffer.Length * 2;");
                                cb.WriteLine("buffer = (arrayToReturnToPool = ArrayPool<char>.Shared.Rent(newLength));");
                            });
                            cb.WriteLine("}");
                            cb.WriteLine();
                            cb.WriteLine("Insert(index, buffer.Slice(0, charsWritten), 1);");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine("finally");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("if (arrayToReturnToPool != null)");
                            cb.WriteLine("ArrayPool<char>.Shared.Return(arrayToReturnToPool);");
                        });
                        cb.WriteLine("}");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("public void Insert(int index, sbyte value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                    cb.WriteLine("{");
                });
                cb.WriteLine("#if NET6_0_OR_GREATER");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("InsertSpanFormattable(index, value, format, provider);");
                    });
                });
                cb.WriteLine("#else");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("string formatStr = format.IsEmpty");
                        cb.WriteLine("    ? string.Empty");
                        cb.WriteLine("    : format.ToString();");
                        cb.WriteLine();
                        cb.WriteLine("Insert(index, value.ToString(formatStr, provider), 1);");
                    });
                });
                cb.WriteLine("#endif");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }

            WriteNumericDoc(cb, "Insert", "byte", options);
            if (options.IncludesJ2N_2_2_Or_Greater)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("public void Insert(int index, byte value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLineIf(options.UseJavaStyleFormatting, "provider ??= NumberFormatInfo.InvariantInfo; // For JDK-style formatting");
                        cb.WriteLine("char[]? arrayToReturnToPool = null;");
                        cb.WriteLine("Span<char> buffer = stackalloc char[CharStackBufferSize];");
                        cb.WriteLine("int charsWritten = 0;");
                        cb.WriteLine("try");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("while (!J2N.Numerics.Byte.TryFormat(value, buffer, out charsWritten, format, provider))");
                            cb.WriteLine("{");
                            cb.IndentBlock(() =>
                            {
                                cb.WriteLine("int newLength = buffer.Length * 2;");
                                cb.WriteLine("buffer = (arrayToReturnToPool = ArrayPool<char>.Shared.Rent(newLength));");
                            });
                            cb.WriteLine("}");
                            cb.WriteLine();
                            cb.WriteLine("Insert(index, buffer.Slice(0, charsWritten), 1);");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine("finally");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("if (arrayToReturnToPool != null)");
                            cb.WriteLine("ArrayPool<char>.Shared.Return(arrayToReturnToPool);");
                        });
                        cb.WriteLine("}");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("public void Insert(int index, byte value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                    cb.WriteLine("{");
                });
                cb.WriteLine("#if NET6_0_OR_GREATER");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("InsertSpanFormattable(index, value, format, provider);");
                    });
                });
                cb.WriteLine("#else");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("string formatStr = format.IsEmpty");
                        cb.WriteLine("    ? string.Empty");
                        cb.WriteLine("    : format.ToString();");
                        cb.WriteLine();
                        cb.WriteLine("Insert(index, value.ToString(formatStr, provider), 1);");
                    });
                });
                cb.WriteLine("#endif");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }

            WriteNumericDoc(cb, "Insert", "short", options);
            if (options.IncludesJ2N_2_2_Or_Greater)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("public void Insert(int index, short value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLineIf(options.UseJavaStyleFormatting, "provider ??= NumberFormatInfo.InvariantInfo; // For JDK-style formatting");
                        cb.WriteLine("char[]? arrayToReturnToPool = null;");
                        cb.WriteLine("Span<char> buffer = stackalloc char[CharStackBufferSize];");
                        cb.WriteLine("int charsWritten = 0;");
                        cb.WriteLine("try");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("while (!J2N.Numerics.Int16.TryFormat(value, buffer, out charsWritten, format, provider))");
                            cb.WriteLine("{");
                            cb.IndentBlock(() =>
                            {
                                cb.WriteLine("int newLength = buffer.Length * 2;");
                                cb.WriteLine("buffer = (arrayToReturnToPool = ArrayPool<char>.Shared.Rent(newLength));");
                            });
                            cb.WriteLine("}");
                            cb.WriteLine();
                            cb.WriteLine("Insert(index, buffer.Slice(0, charsWritten), 1);");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine("finally");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("if (arrayToReturnToPool != null)");
                            cb.WriteLine("ArrayPool<char>.Shared.Return(arrayToReturnToPool);");
                        });
                        cb.WriteLine("}");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("public void Insert(int index, short value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                    cb.WriteLine("{");
                });
                cb.WriteLine("#if NET6_0_OR_GREATER");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("InsertSpanFormattable(index, value, format, provider);");
                    });
                });
                cb.WriteLine("#else");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("string formatStr = format.IsEmpty");
                        cb.WriteLine("    ? string.Empty");
                        cb.WriteLine("    : format.ToString();");
                        cb.WriteLine();
                        cb.WriteLine("Insert(index, value.ToString(formatStr, provider), 1);");
                    });
                });
                cb.WriteLine("#endif");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }

            WriteNumericDoc(cb, "Insert", "int", options);
            if (options.IncludesJ2N_2_2_Or_Greater)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("public void Insert(int index, int value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLineIf(options.UseJavaStyleFormatting, "provider ??= NumberFormatInfo.InvariantInfo; // For JDK-style formatting");
                        cb.WriteLine("char[]? arrayToReturnToPool = null;");
                        cb.WriteLine("Span<char> buffer = stackalloc char[CharStackBufferSize];");
                        cb.WriteLine("int charsWritten = 0;");
                        cb.WriteLine("try");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("while (!J2N.Numerics.Int32.TryFormat(value, buffer, out charsWritten, format, provider))");
                            cb.WriteLine("{");
                            cb.IndentBlock(() =>
                            {
                                cb.WriteLine("int newLength = buffer.Length * 2;");
                                cb.WriteLine("buffer = (arrayToReturnToPool = ArrayPool<char>.Shared.Rent(newLength));");
                            });
                            cb.WriteLine("}");
                            cb.WriteLine();
                            cb.WriteLine("Insert(index, buffer.Slice(0, charsWritten), 1);");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine("finally");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("if (arrayToReturnToPool != null)");
                            cb.WriteLine("ArrayPool<char>.Shared.Return(arrayToReturnToPool);");
                        });
                        cb.WriteLine("}");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("public void Insert(int index, int value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                    cb.WriteLine("{");
                });
                cb.WriteLine("#if NET6_0_OR_GREATER");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("InsertSpanFormattable(index, value, format, provider);");
                    });
                });
                cb.WriteLine("#else");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("string formatStr = format.IsEmpty");
                        cb.WriteLine("    ? string.Empty");
                        cb.WriteLine("    : format.ToString();");
                        cb.WriteLine();
                        cb.WriteLine("Insert(index, value.ToString(formatStr, provider), 1);");
                    });
                });
                cb.WriteLine("#endif");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }

            WriteNumericDoc(cb, "Insert", "long", options);
            if (options.IncludesJ2N_2_2_Or_Greater)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("public void Insert(int index, long value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLineIf(options.UseJavaStyleFormatting, "provider ??= NumberFormatInfo.InvariantInfo; // For JDK-style formatting");
                        cb.WriteLine("char[]? arrayToReturnToPool = null;");
                        cb.WriteLine("Span<char> buffer = stackalloc char[CharStackBufferSize];");
                        cb.WriteLine("int charsWritten = 0;");
                        cb.WriteLine("try");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("while (!J2N.Numerics.Int64.TryFormat(value, buffer, out charsWritten, format, provider))");
                            cb.WriteLine("{");
                            cb.IndentBlock(() =>
                            {
                                cb.WriteLine("int newLength = buffer.Length * 2;");
                                cb.WriteLine("buffer = (arrayToReturnToPool = ArrayPool<char>.Shared.Rent(newLength));");
                            });
                            cb.WriteLine("}");
                            cb.WriteLine();
                            cb.WriteLine("Insert(index, buffer.Slice(0, charsWritten), 1);");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine("finally");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("if (arrayToReturnToPool != null)");
                            cb.WriteLine("ArrayPool<char>.Shared.Return(arrayToReturnToPool);");
                        });
                        cb.WriteLine("}");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("public void Insert(int index, long value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                    cb.WriteLine("{");
                });
                cb.WriteLine("#if NET6_0_OR_GREATER");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("InsertSpanFormattable(index, value, format, provider);");
                    });
                });
                cb.WriteLine("#else");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("string formatStr = format.IsEmpty");
                        cb.WriteLine("    ? string.Empty");
                        cb.WriteLine("    : format.ToString();");
                        cb.WriteLine();
                        cb.WriteLine("Insert(index, value.ToString(formatStr, provider), 1);");
                    });
                });
                cb.WriteLine("#endif");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }

            WriteNumericDoc(cb, "Insert", "ushort", options);
            cb.IndentBlock(() =>
            {
                cb.WriteLine("//[CLSCompliant(false)]");
                cb.WriteLine("public void Insert(int index, ushort value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLineIf(options.UseJavaStyleFormatting && options.IncludesJ2N_2_2_Or_Greater, "provider ??= NumberFormatInfo.InvariantInfo; // For JDK-style formatting");
                });
            });
            cb.WriteLine("#if NET6_0_OR_GREATER");
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("InsertSpanFormattable(index, value, format, provider);");
                });
            });
            cb.WriteLine("#else");
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("string formatStr = format.IsEmpty");
                    cb.WriteLine("    ? string.Empty");
                    cb.WriteLine("    : format.ToString();");
                    cb.WriteLine();
                    cb.WriteLine("Insert(index, value.ToString(formatStr, provider), 1);");
                });
            });
            cb.WriteLine("#endif");
            cb.IndentBlock(() =>
            {
                cb.WriteLine("}");
                cb.WriteLine();
            });

            WriteNumericDoc(cb, "Insert", "uint", options);
            cb.IndentBlock(() =>
            {
                cb.WriteLine("//[CLSCompliant(false)]");
                cb.WriteLine("public void Insert(int index, uint value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLineIf(options.UseJavaStyleFormatting && options.IncludesJ2N_2_2_Or_Greater, "provider ??= NumberFormatInfo.InvariantInfo; // For JDK-style formatting");
                });
            });
            cb.WriteLine("#if NET6_0_OR_GREATER");
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("InsertSpanFormattable(index, value, format, provider);");
                });
            });
            cb.WriteLine("#else");
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("string formatStr = format.IsEmpty");
                    cb.WriteLine("    ? string.Empty");
                    cb.WriteLine("    : format.ToString();");
                    cb.WriteLine();
                    cb.WriteLine("Insert(index, value.ToString(formatStr, provider), 1);");
                });
            });
            cb.WriteLine("#endif");
            cb.IndentBlock(() =>
            {
                cb.WriteLine("}");
                cb.WriteLine();
            });

            WriteNumericDoc(cb, "Insert", "ulong", options);
            cb.IndentBlock(() =>
            {
                cb.WriteLine("//[CLSCompliant(false)]");
                cb.WriteLine("public void Insert(int index, ulong value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLineIf(options.UseJavaStyleFormatting && options.IncludesJ2N_2_2_Or_Greater, "provider ??= NumberFormatInfo.InvariantInfo; // For JDK-style formatting");
                });
            });
            cb.WriteLine("#if NET6_0_OR_GREATER");
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("InsertSpanFormattable(index, value, format, provider);");
                });
            });
            cb.WriteLine("#else");
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("string formatStr = format.IsEmpty");
                    cb.WriteLine("    ? string.Empty");
                    cb.WriteLine("    : format.ToString();");
                    cb.WriteLine();
                    cb.WriteLine("Insert(index, value.ToString(formatStr, provider), 1);");
                });
            });
            cb.WriteLine("#endif");
            cb.IndentBlock(() =>
            {
                cb.WriteLine("}");
                cb.WriteLine();
                cb.WriteLine("// Floating Point");
                cb.WriteLine();
            });

            WriteNumericDoc(cb, "Insert", "float", options);
            if (options.IncludesJ2N_2_2_Or_Greater)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("public void Insert(int index, float value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null) // Requires J2N");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLineIf(options.UseJavaStyleFormatting, "provider ??= NumberFormatInfo.InvariantInfo; // For JDK-style formatting");
                        if (!options.UseJavaStyleFormatting)
                        {
                            cb.WriteLine("if (format.IsEmpty)");
                            cb.WriteLine("{");
                            cb.WriteLine("    format = GeneralFormat.AsSpan(); // For .NET-style formatting");
                            cb.WriteLine("}");
                        }
                        cb.WriteLine("char[]? arrayToReturnToPool = null;");
                        cb.WriteLine("Span<char> buffer = stackalloc char[CharStackBufferSize];");
                        cb.WriteLine("int charsWritten = 0;");
                        cb.WriteLine("try");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("while (!J2N.Numerics.Single.TryFormat(value, buffer, out charsWritten, format, provider))");
                            cb.WriteLine("{");
                            cb.IndentBlock(() =>
                            {
                                cb.WriteLine("int newLength = buffer.Length * 2;");
                                cb.WriteLine("buffer = (arrayToReturnToPool = ArrayPool<char>.Shared.Rent(newLength));");
                            });
                            cb.WriteLine("}");
                            cb.WriteLine();
                            cb.WriteLine("Insert(index, buffer.Slice(0, charsWritten), 1);");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine("finally");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("if (arrayToReturnToPool != null)");
                            cb.WriteLine("ArrayPool<char>.Shared.Return(arrayToReturnToPool);");
                        });
                        cb.WriteLine("}");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("public void Insert(int index, float value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                    cb.WriteLine("{");
                });
                cb.WriteLine("#if NET6_0_OR_GREATER");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("InsertSpanFormattable(index, value, format, provider);");
                    });
                });
                cb.WriteLine("#else");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("string formatStr = format.IsEmpty");
                        cb.WriteLine("    ? string.Empty");
                        cb.WriteLine("    : format.ToString();");
                        cb.WriteLine();
                        cb.WriteLine("Insert(index, value.ToString(formatStr, provider), 1);");
                    });
                });
                cb.WriteLine("#endif");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }

            WriteNumericDoc(cb, "Insert", "double", options);
            if (options.IncludesJ2N_2_2_Or_Greater)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("public void Insert(int index, double value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLineIf(options.UseJavaStyleFormatting, "provider ??= NumberFormatInfo.InvariantInfo; // For JDK-style formatting");
                        if (!options.UseJavaStyleFormatting)
                        {
                            cb.WriteLine("if (format.IsEmpty)");
                            cb.WriteLine("{");
                            cb.WriteLine("    format = GeneralFormat.AsSpan(); // For .NET-style formatting");
                            cb.WriteLine("}");
                        }
                        cb.WriteLine("char[]? arrayToReturnToPool = null;");
                        cb.WriteLine("Span<char> buffer = stackalloc char[CharStackBufferSize];");
                        cb.WriteLine("int charsWritten = 0;");
                        cb.WriteLine("try");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("while (!J2N.Numerics.Double.TryFormat(value, buffer, out charsWritten, format, provider))");
                            cb.WriteLine("{");
                            cb.IndentBlock(() =>
                            {
                                cb.WriteLine("int newLength = buffer.Length * 2;");
                                cb.WriteLine("buffer = (arrayToReturnToPool = ArrayPool<char>.Shared.Rent(newLength));");
                            });
                            cb.WriteLine("}");
                            cb.WriteLine();
                            cb.WriteLine("Insert(index, buffer.Slice(0, charsWritten), 1);");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine("finally");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("if (arrayToReturnToPool != null)");
                            cb.WriteLine("ArrayPool<char>.Shared.Return(arrayToReturnToPool);");
                        });
                        cb.WriteLine("}");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("public void Insert(int index, double value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                    cb.WriteLine("{");
                });
                cb.WriteLine("#if NET6_0_OR_GREATER");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("InsertSpanFormattable(index, value, format, provider);");
                    });
                });
                cb.WriteLine("#else");
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("string formatStr = format.IsEmpty");
                        cb.WriteLine("    ? string.Empty");
                        cb.WriteLine("    : format.ToString();");
                        cb.WriteLine();
                        cb.WriteLine("Insert(index, value.ToString(formatStr, provider), 1);");
                    });
                });
                cb.WriteLine("#endif");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("}");
                    cb.WriteLine();
                });
            }

            WriteNumericDoc(cb, "Insert", "decimal", options);
            cb.IndentBlock(() =>
            {
                cb.WriteLine("public void Insert(int index, decimal value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLineIf(options.UseJavaStyleFormatting && options.IncludesJ2N_2_2_Or_Greater, "provider ??= NumberFormatInfo.InvariantInfo; // For JDK-style formatting");
                });
            });
            cb.WriteLine("#if NET6_0_OR_GREATER");
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("InsertSpanFormattable(index, value, format, provider);");
                });
            });
            cb.WriteLine("#else");
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("string formatStr = format.IsEmpty");
                    cb.WriteLine("    ? string.Empty");
                    cb.WriteLine("    : format.ToString();");
                    cb.WriteLine();
                    cb.WriteLine("Insert(index, value.ToString(formatStr, provider), 1);");
                });
            });
            cb.WriteLine("#endif");
            cb.IndentBlock(() =>
            {
                cb.WriteLine("}");
                cb.WriteLine();
            });
            cb.WriteLine("}");


            return sb.ToString();

        }


        // Types that J2N supports for TryFormat (per your list)
        private static readonly HashSet<string> J2nSupportedTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            "byte",
            "sbyte",
            "short",
            "int",
            "long",
            "float",
            "double"
        };

        /// <summary>
        /// Writes documentation comments for numeric Append/Insert methods according to the rules:
        /// - If J2N 2.2+ is referenced AND the type is supported by J2N => emit J2N docs (no #if).
        /// - Otherwise => emit "stock" docs with #if NET6_0_OR_GREATER ... #else ... #endif that use
        ///   TryFormat in the NET6 branch and ToString in the else branch.
        /// </summary>
        /// <param name="cb">Your CodeBuilder instance (writes lines verbatim).</param>
        /// <param name="methodKind">"Append" or "Insert".</param>
        /// <param name="csharpType">C# short type name: "double", "decimal", etc.</param>
        /// <param name="options">ValueStringBuilderOptions instance.</param>
        public static void WriteNumericDoc(
            CodeBuilder cb,
            string methodKind,
            string csharpType,
            ValueStringBuilderOptions options)
        {
            if (cb == null) throw new ArgumentNullException(nameof(cb));
            if (string.IsNullOrWhiteSpace(methodKind)) throw new ArgumentNullException(nameof(methodKind));
            if (string.IsNullOrWhiteSpace(csharpType)) throw new ArgumentNullException(nameof(csharpType));
            methodKind = methodKind.Trim();

            bool j2nAvailable = options.IncludesJ2N_2_2_Or_Greater;
            bool typeSupportedByJ2n = J2nSupportedTypes.Contains(csharpType);
            bool useJ2NDocs = j2nAvailable;
            bool useJavaStyle = useJ2NDocs && options.UseJavaStyleFormatting;
            bool isInsert = string.Equals(methodKind, "Insert", StringComparison.OrdinalIgnoreCase);
            bool isFloatingPoint = string.Equals(csharpType, "float", StringComparison.OrdinalIgnoreCase)
                || string.Equals(csharpType, "double", StringComparison.OrdinalIgnoreCase);

            // friendly description of the type (as in your examples)
            string typeDescription = csharpType switch
            {
                "float" => "single-precision floating-point number",
                "double" => "double-precision floating-point number",
                "decimal" => "decimal",
                "sbyte" => "8-bit signed integer",
                "byte" => "8-bit unsigned integer",
                "short" => "16-bit signed integer",
                "ushort" => "16-bit unsigned integer",
                "int" => "32-bit signed integer",
                "uint" => "32-bit unsigned integer",
                "long" => "64-bit signed integer",
                "ulong" => "64-bit unsigned integer",
                _ => csharpType
            };

            // Helpers for cref target types (Integer names used in your docs for method cref)
            string crefTypeName = ToClrTypeForDoc(csharpType); // e.g. "Double", "Int32", etc.

            // ------------- Emit -------------
            if (!useJ2NDocs || !typeSupportedByJ2n)
            {
                // Stock docs - emit the conditional block with NET6 TryFormat vs ToString in else
                cb.WriteLine("#if NET6_0_OR_GREATER");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine($"/// {methodKind}s the string representation of a specified {typeDescription} to this instance");
                    cb.WriteLine("/// with the specified numeric format and culture-specific format information.");
                    cb.WriteLine("/// <para/>");
                    if (useJavaStyle)
                    {
                        cb.WriteLineIf(isFloatingPoint, "/// Unless otherwise specified, formatting is performed in the invariant culture using the \"J\" format, which");
                        cb.WriteLineIf(!isFloatingPoint, "/// Unless otherwise specified, formatting is performed in the invariant culture, which");
                        cb.WriteLine("/// is similar to how the JDK formats numbers.");
                    }
                    else
                    {
                        cb.WriteLine("/// Unless otherwise specified, formatting is performed using the current culture using the \"G\" format, which");
                        cb.WriteLine("/// is similar to how the BCL formats numbers.");
                    }
                    cb.WriteLine("/// </summary>");
                    cb.WriteLineIf(isInsert, "/// <param name=\"index\">The position in this instance where insertion begins.</param>");
                    cb.WriteLine("/// <param name=\"value\">The value to format and append.</param>");
                    cb.WriteLine("/// <param name=\"format\">A standard or custom numeric format string.</param>");
                    cb.WriteLine("/// <param name=\"provider\">An object that supplies culture-specific formatting information.</param>");
                    cb.WriteLine("/// <exception cref=\"FormatException\"><paramref name=\"format\"/> is invalid.</exception>");
                    cb.WriteLine("/// <remarks>");
                    if (!isInsert)
                    {
                        cb.WriteLine("/// This method allows similar options as the <c>AppendFormat</c> methods, but has better performance because");
                        cb.WriteLine("/// the value being formatted is not boxed.");
                        cb.WriteLine("/// <para/>");
                    }
                    cb.WriteLine($"/// This method calls the <see cref=\"{crefTypeName}.TryFormat(Span{{char}}, out int, ReadOnlySpan{{char}}, IFormatProvider?)\"/> method");
                    cb.WriteLine("/// to get the string representation of <paramref name=\"value\"/> for the provided <paramref name=\"format\"/> and <paramref name=\"provider\"/>.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// The capacity of this instance is adjusted as needed.");
                    cb.WriteLine("/// </remarks>");
                });
                cb.WriteLine("#else");
                cb.IndentBlock(() =>
                {
                    // The else variant: ToString(string,IFormatProvider)
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine($"/// {methodKind}s the string representation of a specified {typeDescription} to this instance");
                    cb.WriteLine("/// with the specified numeric format and culture-specific format information.");
                    cb.WriteLine("/// <para/>");
                    if (useJavaStyle)
                    {
                        cb.WriteLineIf(isFloatingPoint, "/// Unless otherwise specified, formatting is performed in the invariant culture using the \"J\" format, which");
                        cb.WriteLineIf(!isFloatingPoint, "/// Unless otherwise specified, formatting is performed in the invariant culture, which");
                        cb.WriteLine("/// is similar to how the JDK formats numbers.");
                    }
                    else
                    {
                        cb.WriteLine("/// Unless otherwise specified, formatting is performed using the current culture using the \"G\" format, which");
                        cb.WriteLine("/// is similar to how the BCL formats numbers.");
                    }
                    cb.WriteLine("/// </summary>");
                    cb.WriteLineIf(isInsert, "/// <param name=\"index\">The position in this instance where insertion begins.</param>");
                    cb.WriteLine("/// <param name=\"value\">The value to format and append.</param>");
                    cb.WriteLine("/// <param name=\"format\">A standard or custom numeric format string.</param>");
                    cb.WriteLine("/// <param name=\"provider\">An object that supplies culture-specific formatting information.</param>");
                    cb.WriteLine("/// <exception cref=\"FormatException\"><paramref name=\"format\"/> is invalid.</exception>");
                    cb.WriteLine("/// <remarks>");
                    cb.WriteLine($"/// This method calls the <see cref=\"{crefTypeName}.ToString(string, IFormatProvider)\"/> method");
                    cb.WriteLine("/// to get the string representation of <paramref name=\"value\"/> for the provided <paramref name=\"format\"/> and <paramref name=\"provider\"/>.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// The capacity of this instance is adjusted as needed.");
                    cb.WriteLine("/// </remarks>");
                });
                cb.WriteLine("#endif");
            }
            else
            {
                // J2N docs for supported types when J2N 2.2+ is available.
                // Paragraph uses "J" + invariant if UseJavaStyleFormatting true, otherwise current culture + "G".
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine($"/// {methodKind}s the string representation of a specified {typeDescription} to this instance");
                    cb.WriteLine("/// with the specified numeric format and culture-specific format information.");
                    cb.WriteLine("/// <para/>");
                    if (useJavaStyle)
                    {
                        cb.WriteLineIf(isFloatingPoint, "/// Unless otherwise specified, formatting is performed in the invariant culture using the \"J\" format, which");
                        cb.WriteLineIf(!isFloatingPoint, "/// Unless otherwise specified, formatting is performed in the invariant culture, which");
                        cb.WriteLine("/// is similar to how the JDK formats numbers.");
                    }
                    else
                    {
                        cb.WriteLine("/// Unless otherwise specified, formatting is performed using the current culture using the \"G\" format, which");
                        cb.WriteLine("/// is similar to how the BCL formats numbers.");
                    }
                    cb.WriteLine("/// </summary>");
                    cb.WriteLineIf(isInsert, "/// <param name=\"index\">The position in this instance where insertion begins.</param>");
                    cb.WriteLine("/// <param name=\"value\">The value to format and append.</param>");
                    cb.WriteLine("/// <param name=\"format\">A standard or custom numeric format string.</param>");
                    cb.WriteLine("/// <param name=\"provider\">An object that supplies culture-specific formatting information.</param>");
                    cb.WriteLine("/// <exception cref=\"FormatException\"><paramref name=\"format\"/> is invalid.</exception>");
                    cb.WriteLine("/// <remarks>");
                    if (!isInsert)
                    {
                        cb.WriteLine("/// This method allows similar options as the <c>AppendFormat</c> methods, but has better performance because");
                        cb.WriteLine("/// the value being formatted is not boxed.");
                        cb.WriteLine("/// <para/>");
                    }
                    // Use J2N.Numerics.<Type>.TryFormat(...)
                    cb.WriteLine($"/// This method calls the <see cref=\"J2N.Numerics.{crefTypeName}.TryFormat({csharpType}, Span{{char}}, out int, ReadOnlySpan{{char}}, IFormatProvider?)\"/> method");
                    cb.WriteLine("/// to get the string representation of <paramref name=\"value\"/> for the provided <paramref name=\"format\"/> and <paramref name=\"provider\"/>.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// The capacity of this instance is adjusted as needed.");
                    cb.WriteLine("/// </remarks>");
                });
            }
        }

        private static string ToClrTypeForDoc(string csharpName) => csharpName switch
        {
            "sbyte" => "SByte",
            "byte" => "Byte",
            "short" => "Int16",
            "ushort" => "UInt16",
            "int" => "Int32",
            "uint" => "UInt32",
            "long" => "Int64",
            "ulong" => "UInt64",
            "float" => "Single",
            "double" => "Double",
            "decimal" => "Decimal",
            _ => char.ToUpperInvariant(csharpName[0]) + csharpName.Substring(1)
        };


        //// Table describing the numeric types we care about.
        //private sealed class NumericTypeInfo
        //{
        //    public string CSharpName { get; }
        //    public string DisplayBitsOrName { get; } // "32-bit" or "decimal"
        //    public bool J2NSupported { get; }

        //    public NumericTypeInfo(string csharpName, string displayBitsOrName, bool j2nSupported)
        //    {
        //        CSharpName = csharpName;
        //        DisplayBitsOrName = displayBitsOrName;
        //        J2NSupported = j2nSupported;
        //    }
        //}

        //private static readonly IReadOnlyList<NumericTypeInfo> NumericTypes = new[]
        //{
        //    // J2N supported types (Byte, SByte, Int16, Int32, Int64, Single, Double)
        //    new NumericTypeInfo("byte",   "8-bit unsigned integer",  true),
        //    new NumericTypeInfo("sbyte",  "8-bit signed integer",    true),
        //    new NumericTypeInfo("short",  "16-bit signed integer",   true),
        //    new NumericTypeInfo("int",    "32-bit signed integer",   true),
        //    new NumericTypeInfo("long",   "64-bit signed integer",   true),
        //    new NumericTypeInfo("float",  "single-precision floating-point number", true),
        //    new NumericTypeInfo("double", "double-precision floating-point number", true),

        //    // J2N unsupported types (Decimal, UInt16, UInt32, UInt64)
        //    new NumericTypeInfo("decimal", "decimal", false),
        //    new NumericTypeInfo("ushort",  "16-bit unsigned integer", false),
        //    new NumericTypeInfo("uint",    "32-bit unsigned integer", false),
        //    new NumericTypeInfo("ulong",   "64-bit unsigned integer", false),
        //};

        ///// <summary>
        ///// Generates the XML documentation block for a numeric Append/Insert overload.
        ///// </summary>
        ///// <param name="cb">CodeBuilder to write into.</param>
        ///// <param name="methodKind">"Append" or "Insert".</param>
        ///// <param name="csharpType">short C# type name (e.g. "int", "double", "ushort").</param>
        ///// <param name="includesJ2N_2_2">true if J2N 2.2 is referenced.</param>
        ///// <param name="useJavaStyleFormatting">true if the user's option sets Java-style defaults.</param>
        //public static void GenerateNumericDoc(
        //    CodeBuilder cb,
        //    string methodKind,
        //    string csharpType,
        //    bool includesJ2N_2_2,
        //    bool useJavaStyleFormatting)
        //{
        //    if (cb is null) throw new ArgumentNullException(nameof(cb));
        //    if (string.IsNullOrWhiteSpace(methodKind)) throw new ArgumentException("methodKind required", nameof(methodKind));
        //    if (string.IsNullOrWhiteSpace(csharpType)) throw new ArgumentException("csharpType required", nameof(csharpType));

        //    // lookup type info
        //    NumericTypeInfo typeInfo = null!;
        //    foreach (var t in NumericTypes)
        //    {
        //        if (string.Equals(t.CSharpName, csharpType, StringComparison.OrdinalIgnoreCase))
        //        {
        //            typeInfo = t;
        //            break;
        //        }
        //    }
        //    if (typeInfo == null)
        //    {
        //        // Fallback: unknown type -> describe generically
        //        typeInfo = new NumericTypeInfo(csharpType, csharpType, false);
        //    }

        //    bool isFloating = string.Equals(csharpType, "float", StringComparison.OrdinalIgnoreCase)
        //                      || string.Equals(csharpType, "double", StringComparison.OrdinalIgnoreCase)
        //                      || string.Equals(csharpType, "decimal", StringComparison.OrdinalIgnoreCase);

        //    bool j2nSupportedForThisType = includesJ2N_2_2 && typeInfo.J2NSupported;

        //    // --- Summary ---
        //    cb.WriteLine("/// <summary>");
        //    if (string.Equals(methodKind, "Append", StringComparison.OrdinalIgnoreCase))
        //    {
        //        cb.WriteLine($"/// Appends the string representation of a specified {typeInfo.DisplayBitsOrName} to this instance");
        //        cb.WriteLine("/// with the specified numeric format and culture-specific format information.");
        //    }
        //    else // Insert
        //    {
        //        cb.WriteLine($"/// Inserts the string representation of a specified {typeInfo.DisplayBitsOrName} to this instance");
        //        cb.WriteLine("/// with the specified numeric format and culture-specific format information.");
        //    }
        //    cb.WriteLine("/// <para/>");

        //    // default formatting paragraph is different for floating point vs integers only in wording (you asked floating types mention 'J' explicitly).
        //    if (useJavaStyleFormatting && includesJ2N_2_2)
        //    {
        //        cb.WriteLine("/// Unless otherwise specified, formatting is performed in the invariant culture using the \"J\" format, which");
        //        cb.WriteLine("/// is similar to how the JDK formats numbers.");
        //    }
        //    else
        //    {
        //        cb.WriteLine("/// Unless otherwise specified, formatting is performed using the current culture using the \"G\" format, which");
        //        cb.WriteLine("/// is similar to how the BCL formats numbers.");
        //    }
        //    cb.WriteLine("/// </summary>");

        //    // --- Parameters ---
        //    if (string.Equals(methodKind, "Insert", StringComparison.OrdinalIgnoreCase))
        //    {
        //        cb.WriteLine("/// <param name=\"index\">The position in this instance where insertion begins.</param>");
        //    }

        //    cb.WriteLine($"/// <param name=\"value\">The value to format and {(string.Equals(methodKind, "Append", StringComparison.OrdinalIgnoreCase) ? "append" : "insert")}.</param>");
        //    cb.WriteLine("/// <param name=\"format\">A standard or custom numeric format string.</param>");
        //    cb.WriteLine("/// <param name=\"provider\">An object that supplies culture-specific formatting information.</param>");

        //    // --- Remarks ---
        //    cb.WriteLine("/// <remarks>");

        //    // Behavior paragraphs:
        //    // 1) J2N not referenced -> stock .NET behavior (ToString fallback or Decimal.ToString)
        //    if (!includesJ2N_2_2)
        //    {
        //        // Use exact text you specified: "stock implementation" behavior
        //        // For supported types (in this mode it doesn't matter), we describe fallback behavior.
        //        if (isFloating && string.Equals(csharpType, "decimal", StringComparison.OrdinalIgnoreCase))
        //        {
        //            // decimal: ToString
        //            cb.WriteLine("/// This method calls the <see cref=\"Decimal.ToString(string, IFormatProvider)\"/> method");
        //            cb.WriteLine("/// to get the string representation of <paramref name=\"value\"/> for the provided <paramref name=\"format\"/> and <paramref name=\"provider\"/>.");
        //        }
        //        else if (isFloating)
        //        {
        //            // float/double: describe ToString (or TryFormat if using NET6 code paths will be conditionally compiled elsewhere)
        //            cb.WriteLine("/// This method calls the <see cref=\"{0}.ToString(string, IFormatProvider)\"/> method".Replace("{0}", ToClrTypeForDoc(csharpType)));
        //            cb.WriteLine("/// to get the string representation of <paramref name=\"value\"/> for the provided <paramref name=\"format\"/> and <paramref name=\"provider\"/>.");
        //        }
        //        else
        //        {
        //            // integer-like types
        //            cb.WriteLine($"/// This method calls the <see cref=\"{ToClrTypeForDoc(csharpType)}.ToString(string, IFormatProvider)\"/> method");
        //            cb.WriteLine("/// to get the string representation of <paramref name=\"value\"/> for the provided <paramref name=\"format\"/> and <paramref name=\"provider\"/>.");
        //        }

        //        cb.WriteLine("/// <para/>");
        //        cb.WriteLine("/// The capacity of this instance is adjusted as needed.");
        //    }
        //    else
        //    {
        //        // 2) J2N referenced.
        //        if (j2nSupportedForThisType)
        //        {
        //            // For J2N-supported types: call J2N.Numerics.<Type>.TryFormat in the implementation (that is what you wanted).
        //            string j2nTypeName = J2NTypeName(csharpType); // e.g. "Int32" or "Single"
        //            cb.WriteLine($"/// This method calls the <see cref=\"J2N.Numerics.{j2nTypeName}.TryFormat({ToJ2NTryFormatSignature(csharpType)})\"/> method");
        //            cb.WriteLine("/// to get the string representation of <paramref name=\"value\"/> for the provided <paramref name=\"format\"/> and <paramref name=\"provider\"/>.");
        //            cb.WriteLine("/// <para/>");
        //            cb.WriteLine("/// The capacity of this instance is adjusted as needed.");
        //            // Add note about defaults depending on useJavaStyleFormatting
        //            if (useJavaStyleFormatting)
        //            {
        //                cb.WriteLine("/// <para/>");
        //                cb.WriteLine("/// When using Java-style formatting the implementation will default to the invariant culture (NumberFormatInfo.InvariantInfo)");
        //                cb.WriteLine("/// and will use the \"J\" format unless another format is explicitly provided.");
        //            }
        //            else
        //            {
        //                cb.WriteLine("/// <para/>");
        //                cb.WriteLine("/// By default the implementation defaults to the .NET formatting behavior (culture-sensitive, \"G\" format).");
        //            }
        //        }
        //        else
        //        {
        //            // J2N referenced but unsupported type -> fallback to ToString semantics
        //            if (string.Equals(csharpType, "decimal", StringComparison.OrdinalIgnoreCase))
        //            {
        //                cb.WriteLine("/// This method calls the <see cref=\"Decimal.ToString(string, IFormatProvider)\"/> method");
        //                cb.WriteLine("/// to get the string representation of <paramref name=\"value\"/> for the provided <paramref name=\"format\"/> and <paramref name=\"provider\"/>.");
        //            }
        //            else
        //            {
        //                cb.WriteLine($"/// This method falls back to the standard .NET formatting path (e.g. <see cref=\"{ToClrTypeForDoc(csharpType)}.ToString(string, IFormatProvider)\"/>).");
        //            }
        //            cb.WriteLine("/// <para/>");
        //            cb.WriteLine("/// The capacity of this instance is adjusted as needed.");

        //            // If J2N referenced but not supporting the type, still mention defaults:
        //            if (useJavaStyleFormatting)
        //            {
        //                cb.WriteLine("/// <para/>");
        //                cb.WriteLine("/// Note: J2N is referenced but this numeric type is not extended by J2N, so Java-style defaults are not applied;");
        //                cb.WriteLine("/// formatting will follow the .NET behavior described above.");
        //            }
        //        }
        //    }

        //    cb.WriteLine("/// </remarks>");
        //}

        //// --- Helpers used by the generator above ---

        //// Map from C# short name to a CLR-ish doc-name for ToString references (Int32 vs int)
        //private static string ToClrTypeForDoc(string csharpName)
        //{
        //    return csharpName switch
        //    {
        //        "sbyte" => "SByte",
        //        "byte" => "Byte",
        //        "short" => "Int16",
        //        "ushort" => "UInt16",
        //        "int" => "Int32",
        //        "uint" => "UInt32",
        //        "long" => "Int64",
        //        "ulong" => "UInt64",
        //        "float" => "Single",
        //        "double" => "Double",
        //        "decimal" => "Decimal",
        //        _ => csharpName
        //    };
        //}

        //// Map to the J2N numeric struct/class name used in your examples
        //private static string J2NTypeName(string csharpName)
        //{
        //    return ToClrTypeForDoc(csharpName); // J2N uses names like Int32, Single, Double, Decimal etc.
        //}

        //// Build the TryFormat signature stub used in the XML cref so it looks like the examples you provided.
        //// We won't produce a fully accurate method signature string but a representative one that matches your docs.
        //private static string ToJ2NTryFormatSignature(string csharpName)
        //{
        //    // Example from your docs:
        //    // J2N.Numerics.Single.TryFormat(float, Span{char}, out int, ReadOnlySpan{char}, IFormatProvider?)
        //    // We'll return a string that fits that pattern.
        //    var clrName = ToClrTypeForDoc(csharpName);
        //    var argType = csharpName; // the parameter type is the short name e.g. float
        //    return $"{argType}, Span{{char}}, out int, ReadOnlySpan{{char}}, IFormatProvider?";
        //}
    }
}
