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
                    cb.WriteLine("=> RemoveCore(startIndex, length, zeroBeyondPosition: false);");
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
                        cb.WriteLine("RemoveCore(startIndex, count, zeroBeyondPosition: false);");
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
                cb.WriteLine("/// characters in the substring ar removed and then the specified ");
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
                cb.WriteLine("/// characters in the substring ar removed and then the specified ");
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
                cb.WriteLine("private void RemoveCore(int startIndex, int length, bool zeroBeyondPosition)");
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
                        cb.WriteLine("if (zeroBeyondPosition)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("_chars.Slice(_pos).Fill('\0'); // Zero out the remaining chars");
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
                cb.WriteLine("private void ReplaceCore(int startIndex, int count, scoped ReadOnlySpan<char> newValue)");
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
                    cb.WriteLine("int end = startIndex + count;");
                    cb.WriteLine("if (end > _pos)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("end = _pos;");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine("if (end > startIndex)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("int stringLength = newValue.Length;");
                        cb.WriteLine("int diff = end - startIndex - stringLength;");
                        cb.WriteLine("if (diff > 0)");
                        cb.WriteLine("{ // replacing with fewer characters");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("RemoveCore(startIndex, diff, zeroBeyondPosition: false);");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine("else if (diff < 0)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("// replacing with more characters...need some room");
                            cb.WriteLine("MakeRoom(startIndex, -diff);");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine("// copy the chars based on the new length");
                        cb.WriteLine("int index = startIndex; // Need a copy in case it is modified so it doesn't affect the below insert.");
                        cb.WriteLine("ReplaceInPlace(ref index, ref MemoryMarshal.GetReference(newValue), stringLength);");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine("if (startIndex == end)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Insert(startIndex, ref MemoryMarshal.GetReference(newValue), newValue.Length);");
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
