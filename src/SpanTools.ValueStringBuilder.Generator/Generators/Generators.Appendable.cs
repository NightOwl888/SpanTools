// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE.txt file or at https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.Text;

namespace SpanTools
{
    internal static partial class Generators
    {
        public static string GenerateValueStringBuilderAppendable(ValueStringBuilderOptions options)
        {
            var sb = new StringBuilder();
            var cb = new CodeBuilder(sb);

            cb.WriteLineIf(options.IncludesJ2N_2_0_Or_Greater, "using J2N.Text;");
            cb.WriteLine("using System;");
            cb.WriteLine("using System.Diagnostics;");
            cb.WriteLine("using System.Text;");
            cb.WriteLine("#nullable enable");
            cb.WriteLine();
            WriteNamespace(cb, options);
            cb.WriteLine();
            cb.WriteLine("internal ref partial struct ValueStringBuilder");
            cb.WriteLine("{");

            if (options.IncludesJ2N_2_0_Or_Greater)
            {

                cb.IndentBlock(() =>
                {
                    cb.WriteLine("#region Append ICharSequence");
                });
                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Appends the string representation of the Unicode characters in a specified sequence to this instance.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// NOTE: Unlike the Java implementation, this method does not add the word <c>\"null\"</c> to the <see cref=\"ValueStringBuilder\"/>");
                    cb.WriteLine("/// if <paramref name=\"value\"/> is <c>null</c>. Instead, no operation is performed.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The sequence of characters to append.</param>");
                    cb.WriteLine("/// <remarks>The capacity of this instance is adjusted as needed.</remarks>");
                    cb.WriteLine("public void Append(ICharSequence? value) => Append(value, 0, value?.Length ?? 0); // JDK Method (requires J2N)");
                });
                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Appends the string representation of a specified subarray of Unicode characters to this instance.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The sequence of characters to append.</param>");
                    cb.WriteLine("/// <param name=\"startIndex\">The starting position in <paramref name=\"value\"/>.</param>");
                    cb.WriteLine("/// <param name=\"count\">The number of characters to append.</param>");
                    cb.WriteLine("/// <remarks>");
                    cb.WriteLine("/// <paramref name=\"startIndex\"/> and <paramref name=\"count\"/> range checks are performed");
                    cb.WriteLine("/// using <see cref=\"Debug.Assert(bool)\"/>.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// The capacity of this instance is adjusted as needed.");
                    cb.WriteLine("/// </remarks>");
                    cb.WriteLine("public void Append(ICharSequence? value, int startIndex, int count) // JDK Method (requires J2N)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Debug.Assert(startIndex >= 0);");
                        cb.WriteLine("Debug.Assert(count >= 0);");
                        cb.WriteLine("Debug.Assert(value is null || value?.Length - startIndex >= count);");
                    });
                });
                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("if (value == null || !value.HasValue)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("return;");
                        });
                        cb.WriteLine("}");
                    });
                });
                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Debug.Assert(value.Length - startIndex >= count);");
                    });
                });
                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("if (value != null && value.HasValue)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("if (value is StringCharSequence str)");
                            cb.WriteLine("{");
                            cb.IndentBlock(() =>
                            {
                                cb.WriteLine("Append(str.Value, startIndex, count);");
                                cb.WriteLine("return;");
                            });
                            cb.WriteLine("}");
                            cb.WriteLine("if (value is CharArrayCharSequence chars)");
                            cb.WriteLine("{");
                            cb.IndentBlock(() =>
                            {
                                cb.WriteLine("Append(chars.Value, startIndex, count);");
                                cb.WriteLine("return;");
                            });
                            cb.WriteLine("}");
                            cb.WriteLine("if (value is StringBuilderCharSequence sb)");
                            cb.WriteLine("{");
                            cb.IndentBlock(() =>
                            {
                                cb.WriteLine("Append(sb.Value, startIndex, count);");
                                cb.WriteLine("return;");
                            });
                            cb.WriteLine("}");
                        });
                    });
                });
                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("int pos = _pos;");
                            cb.WriteLine("if (pos > _chars.Length - count)");
                            cb.WriteLine("{");
                            cb.IndentBlock(() =>
                            {
                                cb.WriteLine("Grow(count);");
                            });
                            cb.WriteLine("}");
                        });
                    });
                });
                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("for (int i = 0; i < count; i++)");
                            cb.WriteLine("{");
                            cb.IndentBlock(() =>
                            {
                                cb.WriteLine("_chars[pos++] = value[i + startIndex];");
                            });
                            cb.WriteLine("}");
                            cb.WriteLine("_pos += count;");
                            cb.WriteLineIf(options.IncludeMaxLengthTracking, "UpdateMaxLength();");
                        });
                        cb.WriteLine("}");
                    });
                    cb.WriteLine("}");
                });
                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("#endregion Append ICharSequence");
                });
                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("#region Insert ICharSequence");
                });
                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Inserts the string representation of a specified subarray of Unicode characters into");
                    cb.WriteLine("/// this instance at the specified character position.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"index\">The position in this instance where insertion begins.</param>");
                    cb.WriteLine("/// <param name=\"value\">The character sequence to insert.</param>");
                    cb.WriteLine("/// <remarks>");
                    cb.WriteLine("/// <paramref name=\"index\"/> range checks are performed using <see cref=\"Debug.Assert(bool)\"/>.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// Existing characters are shifted to make room for the new text. The capacity is adjusted as needed.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// This instance of <see cref=\"ValueStringBuilder\"/> is not changed if <paramref name=\"value\"/> is <c>null</c>,");
                    cb.WriteLine("/// or <paramref name=\"value\"/> is not <c>null</c> but its length is zero.");
                    cb.WriteLine("/// </remarks>");
                    cb.WriteLine("public void Insert(int index, ICharSequence? value) // JDK Method (requires J2N)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Debug.Assert((uint)index <= (uint)_pos);");
                    });
                });
                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("if (value == null || !value.HasValue)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("return;");
                        });
                        cb.WriteLine("}");
                    });
                });
                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("if (value is StringCharSequence str)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("Insert(index, str.Value);");
                            cb.WriteLine("return;");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine("if (value is CharArrayCharSequence chars)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("Insert(index, chars.Value);");
                            cb.WriteLine("return;");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine("if (value is StringBuilderCharSequence sb)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("Insert(index, sb.Value);");
                            cb.WriteLine("return;");
                        });
                        cb.WriteLine("}");
                    });
                });
                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("int count = value.Length;");
                    });
                });
                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("if (count > 0)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("MakeRoom(index, count);");
                            cb.WriteLine("for (int i = 0; i < count; i++)");
                            cb.WriteLine("{");
                            cb.IndentBlock(() =>
                            {
                                cb.WriteLine("_chars[index++] = value[i];");
                            });
                            cb.WriteLine("}");
                        });
                        cb.WriteLine("}");
                    });
                    cb.WriteLine("}");
                });
                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("#endregion Insert ICharSequence");
                });
                cb.WriteLine();
            }

            cb.IndentBlock(() =>
            {
                cb.WriteLine("#region  IAppendable Members");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Appends the string representation of a specified subarray of Unicode characters to this instance.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">The string to copy characters from.</param>");
                cb.WriteLine("/// <param name=\"startIndex\">The starting position in <paramref name=\"value\"/>.</param>");
                cb.WriteLine("/// <param name=\"count\">The number of characters to append.</param>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// <paramref name=\"startIndex\"/> and <paramref name=\"count\"/> range checks are performed");
                cb.WriteLine("/// using <see cref=\"Debug.Assert(bool)\"/>.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// The capacity of this instance is adjusted as needed.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("public void Append(string? value, int startIndex, int count)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Debug.Assert(startIndex >= 0);");
                    cb.WriteLine("Debug.Assert(count >= 0);");
                    cb.WriteLine("Debug.Assert(value is null || value?.Length - startIndex >= count);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("if (value is null || count == 0) return;");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("int pos = _pos;");
                    cb.WriteLine("if (pos > _chars.Length - count)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Grow(count);");
                    });
                    cb.WriteLine("}");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("value.AsSpan(startIndex, count).CopyTo(_chars.Slice(pos, count));");
                    cb.WriteLine("_pos += count;");
                    cb.WriteLineIf(options.IncludeMaxLengthTracking, "UpdateMaxLength();");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Appends the string representation of a specified subarray of Unicode characters to this instance.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">A character array.</param>");
                cb.WriteLine("/// <param name=\"startIndex\">The starting position in <paramref name=\"value\"/>.</param>");
                cb.WriteLine("/// <param name=\"count\">The number of characters to append.</param>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// <paramref name=\"startIndex\"/> and <paramref name=\"count\"/> range checks are performed");
                cb.WriteLine("/// using <see cref=\"Debug.Assert(bool)\"/>.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// The capacity of this instance is adjusted as needed.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("public void Append(char[]? value, int startIndex, int count)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Debug.Assert(startIndex >= 0);");
                    cb.WriteLine("Debug.Assert(count >= 0);");
                    cb.WriteLine("Debug.Assert(value is null || value?.Length - startIndex >= count);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("if (value is null || count == 0) return;");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("int pos = _pos;");
                    cb.WriteLine("if (pos > _chars.Length - count)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Grow(count);");
                    });
                    cb.WriteLine("}");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("value.AsSpan(startIndex, count).CopyTo(_chars.Slice(pos, count));");
                    cb.WriteLine("_pos += count;");
                    cb.WriteLineIf(options.IncludeMaxLengthTracking, "UpdateMaxLength();");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Appends the string representation of the Unicode characters in a specified sequence to this instance.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// NOTE: Unlike the Java implementation, this method does not add the word <c>\"null\"</c> to the <see cref=\"ValueStringBuilder\"/>");
                cb.WriteLine("/// if <paramref name=\"value\"/> is <c>null</c>. Instead, no operation is performed.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">The <see cref=\"StringBuilder\"/> to append.</param>");
                cb.WriteLine("/// <remarks>The capacity of this instance is adjusted as needed.</remarks>");
                cb.WriteLine("public void Append(StringBuilder? value) => Append(value, 0, value?.Length ?? 0); // Requires J2N (or a shim of CopyTo(Span))");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Appends the string representation of a specified subarray of Unicode characters to this instance.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">The <see cref=\"StringBuilder\"/> to copy characters from.</param>");
                cb.WriteLine("/// <param name=\"startIndex\">The starting position in <paramref name=\"value\"/>.</param>");
                cb.WriteLine("/// <param name=\"count\">The number of characters to append.</param>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// <paramref name=\"startIndex\"/> and <paramref name=\"count\"/> range checks are performed");
                cb.WriteLine("/// using <see cref=\"Debug.Assert(bool)\"/>.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// The capacity of this instance is adjusted as needed.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("public void Append(StringBuilder? value, int startIndex, int count) // Requires J2N (or a shim of CopyTo(Span))");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Debug.Assert(startIndex >= 0);");
                    cb.WriteLine("Debug.Assert(count >= 0);");
                    cb.WriteLine("Debug.Assert(value is null || value?.Length - startIndex >= count);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("if (value is null || count == 0) return;");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("int pos = _pos;");
                    cb.WriteLine("if (pos > _chars.Length - count)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Grow(count);");
                    });
                    cb.WriteLine("}");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("if (_arrayToReturnToPool is null)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("// Only call our extension method (which may allocate a buffer) if we have to.");
                        cb.WriteLine("value.CopyTo(startIndex, _chars.Slice(pos), count);");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine("else");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("// If we are already on the heap, we can use CopyTo(int, char[], int, int) for better efficiency");
                        cb.WriteLine("value.CopyTo(startIndex, _arrayToReturnToPool, pos, count);");
                    });
                    cb.WriteLine("}");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("_pos += count;");
                    cb.WriteLineIf(options.IncludeMaxLengthTracking, "UpdateMaxLength();");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("#endregion IAppendable Members");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("#region Insert StringBuilder");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Inserts the string representation of a specified subarray of Unicode characters into");
                cb.WriteLine("/// this instance at the specified character position.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"index\">The position in this instance where insertion begins.</param>");
                cb.WriteLine("/// <param name=\"value\">The <see cref=\"StringBuilder\"/> to insert.</param>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// <paramref name=\"index\"/> range checks are performed using <see cref=\"Debug.Assert(bool)\"/>.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// Existing characters are shifted to make room for the new text. The capacity is adjusted as needed.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// This instance of <see cref=\"ValueStringBuilder\"/> is not changed if <paramref name=\"value\"/> is <c>null</c>,");
                cb.WriteLine("/// or <paramref name=\"value\"/> is not <c>null</c> but its length is zero.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("public void Insert(int index, StringBuilder? value) // Requires J2N (or a shim of CopyTo(Span))");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Debug.Assert((uint)index <= (uint)_pos);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("if (value == null)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("return;");
                    });
                    cb.WriteLine("}");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("int count = value.Length;");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("if (count == 0)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("return;");
                    });
                    cb.WriteLine("}");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("MakeRoom(index, count);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("if (_arrayToReturnToPool is null)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("// Only call our extension method (which may allocate a buffer) if we have to.");
                        cb.WriteLine("value.CopyTo(0, _chars.Slice(index), count);");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine("else");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("// If we are already on the heap, we can use CopyTo(int, char[], int, int) for better efficiency");
                        cb.WriteLine("value.CopyTo(0, _arrayToReturnToPool, index, count);");
                    });
                    cb.WriteLine("}");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("#endregion Insert StringBuilder");
            });
            cb.WriteLine("}");


            return sb.ToString();
        }
    }
}
