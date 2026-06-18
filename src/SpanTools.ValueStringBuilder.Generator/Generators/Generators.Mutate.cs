// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE.txt file or at https://opensource.org/licenses/MIT.

using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpanTools
{
    internal static partial class Generators
    {
        public static string GenerateValueStringBuilderMutate(ValueStringBuilderOptions options)
        {
            var sb = new StringBuilder();
            var cb = new CodeBuilder(sb);

            cb.WriteLineIf(options.IncludesJ2N_2_1_Or_Greater, "using J2N;");
            cb.WriteLine("using System;");
            cb.WriteLine("using System.Buffers;");
            cb.WriteLine("using System.Diagnostics;");
            cb.WriteLine("using System.Runtime.InteropServices;");
            cb.WriteLine("#nullable enable");
            cb.WriteLine();
            WriteNamespace(cb, options);
            cb.WriteLine();
            cb.WriteLine("internal ref partial struct ValueStringBuilder");
            cb.WriteLine("{");
            if (options.IncludesJ2N_2_1_Or_Greater)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Causes this character sequence to be replaced by the reverse of");
                    cb.WriteLine("/// the sequence. If there are any surrogate pairs included in the");
                    cb.WriteLine("/// sequence, these are treated as single characters for the");
                    cb.WriteLine("/// reverse operation. Thus, the order of the high-low surrogates");
                    cb.WriteLine("/// is never reversed.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// This operation is done in-place.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// Let <c>n</c> be the character length of this character sequence");
                    cb.WriteLine("/// (not the length in <see cref=\"char\"/> values) just prior to");
                    cb.WriteLine("/// execution of the <see cref=\"Reverse()\"/> method. Then the");
                    cb.WriteLine("/// character at index <c>k</c> in the new character sequence is");
                    cb.WriteLine("/// equal to the character at index <c>n-k-1</c> in the old");
                    cb.WriteLine("/// character sequence.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// Note that the reverse operation may result in producing");
                    cb.WriteLine("/// surrogate pairs that were unpaired low-surrogates and");
                    cb.WriteLine("/// high-surrogates before the operation. For example, reversing");
                    cb.WriteLine("/// \"&#92;uDC00&#92;uD800\" produces \"&#92;uD800&#92;uDC00\" which is");
                    cb.WriteLine("/// a valid surrogate pair.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("public void Reverse() // Requires J2N");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("_chars.Slice(0, _pos).ReverseText();");
                    });
                    cb.WriteLine("}");
                });
                cb.WriteLine();
            }
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Removes the specified range of characters from this instance.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"startIndex\">The zero-based position in this instance where removal begins.</param>");
                cb.WriteLine("/// <param name=\"length\">The number of characters to remove.</param>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// <paramref name=\"startIndex\"/> and <paramref name=\"length\"/> range checks are performed using <see cref=\"Debug.Assert(bool)\"/>.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// The current method removes the specified range of characters from the current instance. The characters at");
                cb.WriteLine("/// (<paramref name=\"startIndex\"/> + <paramref name=\"length\"/>) are moved to <paramref name=\"startIndex\"/>, and");
                cb.WriteLine("/// the string value of the current instance is shortened by <paramref name=\"length\"/>. The capacity of the");
                cb.WriteLine("/// current instance is unaffected.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("public void Remove(int startIndex, int length) // BCL method");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("=> RemoveCore(startIndex, length);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Removes the specified range of characters from this instance.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"startIndex\">The zero-based position in this instance where removal begins.</param>");
                cb.WriteLine("/// <param name=\"count\">The number of characters to remove. If <paramref name=\"startIndex\"/> + <paramref name=\"count\"/>");
                cb.WriteLine("/// is greater than <see cref=\"Length\"/>, the effect will be to remove the remainder of the chars after");
                cb.WriteLine("/// <paramref name=\"startIndex\"/>.</param>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// <paramref name=\"startIndex\"/> and <paramref name=\"count\"/> range checks are performed using <see cref=\"Debug.Assert(bool)\"/>.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("public void Delete(int startIndex, int count) // JDK method");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Debug.Assert(startIndex >= 0 || startIndex <= _pos);");
                    cb.WriteLine("Debug.Assert(count >= 0);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("int pos = _pos;");
                    cb.WriteLine("if (startIndex + count > pos)");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("count = pos - startIndex;");
                    });
                    cb.WriteLine("if (count > 0)");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("RemoveCore(startIndex, count);");
                    });
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.WriteLine("#if !NET5_0_OR_GREATER");
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Replaces the specified subsequence in this builder with the specified");
                cb.WriteLine("/// string, <paramref name=\"newValue\"/>. The substring begins at the specified");
                cb.WriteLine("/// <paramref name=\"startIndex\"/> and ends to the character at ");
                cb.WriteLine("/// <c><paramref name=\"count\"/> - <paramref name=\"startIndex\"/></c> or");
                cb.WriteLine("/// to the end of the sequence if no such character exists. First the");
                cb.WriteLine("/// characters in the substring are removed and then the specified ");
                cb.WriteLine("/// <paramref name=\"newValue\"/> is inserted at <paramref name=\"startIndex\"/>.");
                cb.WriteLine("/// This <see cref=\"ValueStringBuilder\"/> will be lengthened to accommodate the");
                cb.WriteLine("/// specified <paramref name=\"newValue\"/> if necessary.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"startIndex\">The inclusive begin index in this builder.</param>");
                cb.WriteLine("/// <param name=\"count\">The number of characters to replace.</param>");
                cb.WriteLine("/// <param name=\"newValue\">The replacement string.</param>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// <paramref name=\"startIndex\"/> and <paramref name=\"count\"/> range checks are performed using <see cref=\"Debug.Assert(bool)\"/>.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("public void Replace(int startIndex, int count, string newValue) // JDK method");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Debug.Assert(newValue != null);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("ReplaceCore(startIndex, count, newValue.AsSpan());");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine("#endif");
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Replaces the specified subsequence in this builder with the specified");
                cb.WriteLine("/// string, <paramref name=\"newValue\"/>. The substring begins at the specified");
                cb.WriteLine("/// <paramref name=\"startIndex\"/> and ends to the character at ");
                cb.WriteLine("/// <c><paramref name=\"count\"/> - <paramref name=\"startIndex\"/></c> or");
                cb.WriteLine("/// to the end of the sequence if no such character exists. First the");
                cb.WriteLine("/// characters in the substring are removed and then the specified ");
                cb.WriteLine("/// <paramref name=\"newValue\"/> is inserted at <paramref name=\"startIndex\"/>.");
                cb.WriteLine("/// This <see cref=\"ValueStringBuilder\"/> will be lengthened to accommodate the");
                cb.WriteLine("/// specified <paramref name=\"newValue\"/> if necessary.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"startIndex\">The inclusive begin index in this builder.</param>");
                cb.WriteLine("/// <param name=\"count\">The number of characters to replace.</param>");
                cb.WriteLine("/// <param name=\"newValue\">The replacement string.</param>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// <paramref name=\"startIndex\"/> and <paramref name=\"count\"/> range checks are performed using <see cref=\"Debug.Assert(bool)\"/>.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// This method allows <paramref name=\"newValue\"/> to be this instance or a slice of this instance.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("public void Replace(int startIndex, int count, scoped ReadOnlySpan<char> newValue) // JDK method");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Debug.Assert(startIndex >= 0 || startIndex <= _pos);");
                    cb.WriteLine("Debug.Assert(count >= 0);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("ReplaceCore(startIndex, count, newValue);");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("private void RemoveCore(int startIndex, int length)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Debug.Assert(length >= 0);");
                    cb.WriteLine("Debug.Assert(startIndex >= 0);");
                    cb.WriteLine("Debug.Assert(length <= _pos - startIndex);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("if (_pos == length && startIndex == 0)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("_pos = 0;");
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
                    cb.WriteLine("if (length > 0)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("int endIndex = startIndex + length;");
                        cb.WriteLine("_chars.Slice(endIndex).CopyTo(_chars.Slice(startIndex));");
                        cb.WriteLine("_pos -= length;");
                    });
                    cb.WriteLine("}");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();

            cb.IndentBlock(() =>
            {
                cb.WriteLine("private void ReplaceCore(int startIndex, int count, scoped ReadOnlySpan<char> newValue)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("int pos = _pos;");
                    cb.WriteLine("Debug.Assert(startIndex >= 0 && startIndex <= pos);");
                    cb.WriteLine("Debug.Assert(count >= 0);");
                    cb.WriteLine();
                    cb.WriteLine("// Clamp to end of buffer (Harmony/JDK behavior)");
                    cb.WriteLine("int end = count > pos - startIndex");
                    cb.WriteLine("? pos");
                    cb.WriteLine(": startIndex + count; // Overflow not possible here");
                    cb.WriteLine();
                    cb.WriteLine("int replacedLength = end - startIndex;");
                    cb.WriteLine();
                    cb.WriteLine("if (_chars.Overlaps(newValue, out int sourceOffset))");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("ReplaceCoreOverlapping(startIndex, count, sourceOffset, newValue.Length, replacedLength, end);");
                        cb.WriteLine("return;");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine();
                    cb.WriteLine("int delta = newValue.Length - replacedLength;");
                    cb.WriteLine("if (delta > 0)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("// Need more space.");
                        cb.WriteLine("//");
                        cb.WriteLine("// Insert the additional space immediately after the replaced region.");
                        cb.WriteLine("// This preserves the replacement area while shifting only the tail.");
                        cb.WriteLine("MakeRoom(end, delta);");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine("else if (delta < 0)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("// Need less space.");
                        cb.WriteLine("//");
                        cb.WriteLine("// Remove only the excess characters after the replacement area.");
                        cb.WriteLine("RemoveCore(startIndex + newValue.Length, -delta);");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine();
                    cb.WriteLine("// Overwrite the replacement area.");
                    cb.WriteLine("if (!newValue.IsEmpty)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("newValue.CopyTo(_chars.Slice(startIndex));");
                    });
                    cb.WriteLine("}");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("private void ReplaceCoreOverlapping(int startIndex, int count, int sourceOffset, int sourceLength, int replacedLength, int end)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Debug.Assert(startIndex <= int.MaxValue - sourceLength);");
                    cb.WriteLine();
                    cb.WriteLine("// Fast path: Exact self-replacement (no-op)");
                    cb.WriteLine("if (sourceOffset == startIndex && sourceLength == replacedLength)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("return;");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine();
                    cb.WriteLine("// Common case: Source entirely before the replacement region");
                    cb.WriteLine("if ((uint)sourceOffset + (uint)sourceLength <= startIndex)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("ReadOnlySpan<char> source = _chars.Slice(sourceOffset, sourceLength);");
                        cb.WriteLine();
                        cb.WriteLine("int delta = sourceLength - replacedLength;");
                        cb.WriteLine();
                        cb.WriteLine("if (delta > 0)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("MakeRoom(end, delta);");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine("else if (delta < 0)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("RemoveCore(startIndex + sourceLength, -delta);");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine();
                        cb.WriteLine("if (sourceLength > 0)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("source.CopyTo(_chars.Slice(startIndex));");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine("return;");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine();
                    cb.WriteLine("char[]? arrayToReturn = null;");
                    cb.WriteLine("try");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Span<char> temp = sourceLength <= CharStackBufferSize");
                        cb.WriteLine("? stackalloc char[sourceLength]");
                        cb.WriteLine(": (arrayToReturn = ArrayPool<char>.Shared.Rent(sourceLength)).AsSpan(0, sourceLength);");
                        cb.WriteLine();
                        cb.WriteLine("_chars.Slice(sourceOffset, sourceLength)");
                        cb.WriteLine(".CopyTo(temp);");
                        cb.WriteLine();
                        cb.WriteLine("int delta = sourceLength - replacedLength;");
                        cb.WriteLine();
                        cb.WriteLine("if (delta > 0)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("MakeRoom(end, delta);");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine("else if (delta < 0)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("RemoveCore(startIndex + sourceLength, -delta);");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine();
                        cb.WriteLine("temp.CopyTo(_chars.Slice(startIndex));");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine("finally");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("if (arrayToReturn is not null)");
                        cb.WriteLine("ArrayPool<char>.Shared.Return(arrayToReturn);");
                    });
                    cb.WriteLine("}");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine("}");

            return sb.ToString();

        }
    }
}
