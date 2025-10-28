// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE.txt file or at https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.Text;

namespace SpanTools
{
    internal static partial class Generators
    {
        public static string GenerateValueStringBuilderCodepoints(ValueStringBuilderOptions options)
        {
            var sb = new StringBuilder();
            var cb = new CodeBuilder(sb);

            cb.WriteLine("using J2N;");
            cb.WriteLine("using System;");
            cb.WriteLine("using System.Diagnostics;");
            cb.WriteLine("using System.Runtime.InteropServices;");
            cb.WriteLine("#nullable enable");
            cb.WriteLine();
            cb.WriteLineIf(!string.IsNullOrWhiteSpace(options.Namespace), $"namespace {options.Namespace};");
            cb.WriteLine();
            cb.WriteLine("internal ref partial struct ValueStringBuilder");
            cb.WriteLine("{");

            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Appends the string representation of the <paramref name=\"codePoint\"/>");
                cb.WriteLine("/// argument to this sequence.");
                cb.WriteLine("/// <para>");
                cb.WriteLine("/// The argument is appended to the contents of this sequence.");
                cb.WriteLine("/// The length of this sequence increases by <see cref=\"Character.CharCount(int)\"/>.");
                cb.WriteLine("/// </para>");
                cb.WriteLine("/// <para>");
                cb.WriteLine("/// The overall effect is exactly as if the argument were");
                cb.WriteLine("/// converted to a <see cref=\"char\"/> array by the method");
                cb.WriteLine("/// <see cref=\"Character.ToChars(int)\"/> and the character in that array");
                cb.WriteLine("/// were then appended to this <see cref=\"ValueStringBuilder\"/> instance.");
                cb.WriteLine("/// </para>");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"codePoint\">A Unicode code point.</param>");
                cb.WriteLine("/// <returns>The character length of the code point that was appended, either 1 or 2.</returns>");
                cb.WriteLine("/// <exception cref=\"ArgumentException\"><paramref name=\"codePoint\"/> is not a valid Unicode code point.</exception>");
                cb.WriteLine("public int AppendCodePoint(int codePoint) // JDK Method (requires J2N or a shim)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("int count = Character.ToChars(codePoint, out char high, out char low);");
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
                    cb.WriteLine("_chars[pos++] = high;");
                    cb.WriteLine("if (count == 2)");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("_chars[pos++] = low;");
                    });
                    cb.WriteLine("_pos += count;");
                    cb.WriteLineIf(options.IncludeMaxLengthTracking, "UpdateMaxLength();");
                    cb.WriteLine("return count;");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Insert the string representation of the <paramref name=\"codePoint\"/>");
                cb.WriteLine("/// argument to this sequence at <paramref name=\"index\"/>.");
                cb.WriteLine("/// <para>");
                cb.WriteLine("/// The argument is inserted into to the contents of this sequence.");
                cb.WriteLine("/// The length of this sequence increases by <see cref=\"Character.CharCount(int)\"/>.");
                cb.WriteLine("/// </para>");
                cb.WriteLine("/// <para>");
                cb.WriteLine("/// The overall effect is exactly as if the argument were");
                cb.WriteLine("/// converted to a <see cref=\"char\"/> array by the method");
                cb.WriteLine("/// <see cref=\"Character.ToChars(int)\"/> and the character in that array");
                cb.WriteLine("/// were then inserted into this <see cref=\"ValueStringBuilder\"/> instance.");
                cb.WriteLine("/// </para>");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"index\">The position in this instance where insertion begins.</param>");
                cb.WriteLine("/// <param name=\"codePoint\">A Unicode code point.</param>");
                cb.WriteLine("/// <returns>The character length of the code point that was appended, either 1 or 2.</returns>");
                cb.WriteLine("/// <exception cref=\"ArgumentException\"><paramref name=\"codePoint\"/> is not a valid Unicode code point.</exception>");
                cb.WriteLine("/// <remarks><paramref name=\"index\"/> range checks are performed using <see cref=\"Debug.Assert(bool)\"/>.</remarks>");
                cb.WriteLine("public int InsertCodePoint(int index, int codePoint) // Requires J2N or a shim");
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
                    cb.WriteLine("ReadOnlySpan<char> value = Character.ToChars(codePoint, stackalloc char[2]);");
                    cb.WriteLine("int count = value.Length;");
                    cb.WriteLine("Insert(index, ref MemoryMarshal.GetReference(value), count);");
                    cb.WriteLine("return count;");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();

            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Returns the code point at <paramref name=\"index\"/> in the specified sequence of");
                cb.WriteLine("/// character units. If the unit at <paramref name=\"index\"/> is a high-surrogate unit,");
                cb.WriteLine("/// <c><paramref name=\"index\"/> + 1</c> is less than the length of the sequence and the unit at");
                cb.WriteLine("/// <c><paramref name=\"index\"/> + 1</c> is a low-surrogate unit, then the supplementary code");
                cb.WriteLine("/// point represented by the pair is returned; otherwise the <see cref=\"char\"/>");
                cb.WriteLine("/// value at <paramref name=\"index\"/> is returned.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"index\">The position in this <see cref=\"ValueStringBuilder\"/> from which to retrieve the code");
                cb.WriteLine("/// point.</param>");
                cb.WriteLine("/// <returns>The Unicode code point or <see cref=\"char\"/> value at <paramref name=\"index\"/> in");
                cb.WriteLine("/// this <see cref=\"ValueStringBuilder\"/>.</returns>");
                cb.WriteLine("/// <exception cref=\"ArgumentOutOfRangeException\">");
                cb.WriteLine("/// <paramref name=\"index\"/> is greater than or equal to <see cref=\"Length\"/>.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// -or-");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// <paramref name=\"index\"/> is less than zero.");
                cb.WriteLine("/// </exception>");
                cb.WriteLine("public int CodePointAt(int index) => ((ReadOnlySpan<char>)_chars.Slice(0, _pos)).CodePointAt(index); // JDK Method (requires J2N or a shim)");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Returns the code point that precedes <paramref name=\"index\"/> in the specified");
                cb.WriteLine("/// sequence of character units. If the unit at <c><paramref name=\"index\"/> - 1</c> is a");
                cb.WriteLine("/// low-surrogate unit, <c><paramref name=\"index\"/> - 2</c> is not negative and the unit at");
                cb.WriteLine("/// <c><paramref name=\"index\"/> - 2</c> is a high-surrogate unit, then the supplementary code");
                cb.WriteLine("/// point represented by the pair is returned; otherwise the <see cref=\"char\"/>");
                cb.WriteLine("/// value at <c><paramref name=\"index\"/> - 1</c> is returned.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"index\">The position in this <see cref=\"ValueStringBuilder\"/> following the code");
                cb.WriteLine("/// point that should be returned.</param>");
                cb.WriteLine("/// <returns>The Unicode code point or <see cref=\"char\"/> value before <paramref name=\"index\"/>");
                cb.WriteLine("/// in this <see cref=\"ValueStringBuilder\"/>.</returns>");
                cb.WriteLine("/// <exception cref=\"ArgumentOutOfRangeException\">If the <paramref name=\"index\"/> is less than");
                cb.WriteLine("/// 1 or greater than <see cref=\"Length\"/>.</exception>");
                cb.WriteLine("public int CodePointBefore(int index) => ((ReadOnlySpan<char>)_chars.Slice(0, _pos)).CodePointBefore(index); // JDK Method (requires J2N or a shim)");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Returns the number of Unicode code points in the text range of the specified char sequence.");
                cb.WriteLine("/// The text range begins at the specified <paramref name=\"startIndex\"/> and extends for the number");
                cb.WriteLine("/// of characters specified in <paramref name=\"length\"/>. ");
                cb.WriteLine("/// Unpaired surrogates within the text range count as one code point each.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// IMPORTANT: This method has .NET semantics. That is, the <paramref name=\"length\"/> parameter");
                cb.WriteLine("/// is a length rather than an exclusive end index. To convert from");
                cb.WriteLine("/// Java, use <c>endIndex - startIndex</c> to obtain the length.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"startIndex\">The index to the first char of the text range.</param>");
                cb.WriteLine("/// <param name=\"length\">The number of characters to consider in this <see cref=\"ValueStringBuilder\"/>.</param>");
                cb.WriteLine("/// <returns>The number of Unicode code points in the specified text range.</returns>");
                cb.WriteLine("/// <exception cref=\"ArgumentOutOfRangeException\">");
                cb.WriteLine("/// <paramref name=\"startIndex\"/> plus <paramref name=\"length\"/> indicates a position not within");
                cb.WriteLine("/// this <see cref=\"ValueStringBuilder\"/>.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// -or-");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// <paramref name=\"startIndex\"/> or <paramref name=\"length\"/> is less than zero.");
                cb.WriteLine("/// </exception>");
                cb.WriteLine("public int CodePointCount(int startIndex, int length) => ((ReadOnlySpan<char>)_chars.Slice(startIndex, length)).CodePointCount(); // JDK Method (requires J2N or a shim)");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Returns the index within this instance that is offset from the given <paramref name=\"index\"/> by");
                cb.WriteLine("/// <paramref name=\"codePointOffset\"/> code points. Unpaired surrogates within the text range given by ");
                cb.WriteLine("/// <paramref name=\"index\"/> and <paramref name=\"codePointOffset\"/> count as one code point each.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"index\">The index to be offset.</param>");
                cb.WriteLine("/// <param name=\"codePointOffset\">The number of code points to look backwards or forwards; may");
                cb.WriteLine("/// be a negative or positive value.</param>");
                cb.WriteLine("/// <returns>The index within the char sequence, offset by <paramref name=\"codePointOffset\"/> code points.</returns>");
                cb.WriteLine("/// <exception cref=\"ArgumentOutOfRangeException\">");
                cb.WriteLine("/// <paramref name=\"index\"/> is less than zero or greater than <see cref=\"Length\"/>.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// -or-");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// <paramref name=\"codePointOffset\"/> is positive and the subsequence starting with");
                cb.WriteLine("/// <paramref name=\"index\"/> has fewer than <paramref name=\"codePointOffset\"/> code points.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// -or-");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// <paramref name=\"codePointOffset\"/> is negative and the subsequence before <paramref name=\"index\"/>");
                cb.WriteLine("/// has fewer than the absolute value of <paramref name=\"codePointOffset\"/> code points.");
                cb.WriteLine("/// </exception>");
                cb.WriteLine("public int OffsetByCodePoints(int index, int codePointOffset) => ((ReadOnlySpan<char>)_chars.Slice(0, _pos)).OffsetByCodePoints(index, codePointOffset); // JDK Method (requires J2N or a shim)");
            });
            cb.WriteLine("}");

            return sb.ToString();
        }
    }
}
