// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE.txt file or at https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.Text;

namespace SpanTools
{
    internal static partial class Generators
    {
        public static string GenerateValueStringBuilderIndexOf(ValueStringBuilderOptions options)
        {
            var sb = new StringBuilder();
            var cb = new CodeBuilder(sb);

            cb.WriteLine("using System;");
            cb.WriteLine("using System.Diagnostics;");
            cb.WriteLine("#nullable enable");
            cb.WriteLine();
            WriteNamespace(cb, options);
            cb.WriteLine();
            cb.WriteLine("internal ref partial struct ValueStringBuilder");
            cb.WriteLine("{");
            cb.IndentBlock(() =>
            {
                cb.WriteLine("#region IndexOf");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Reports the zero-based index of the first occurrence of the specified Unicode character");
                cb.WriteLine("/// in this builder.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">A Unicode character to seek.</param>");
                cb.WriteLine("/// <returns>The zero-based index position of <paramref name=\"value\"/> if that character");
                cb.WriteLine("/// is found, or -1 if it is not.</returns>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// Index numbering starts from zero.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// This method performs an ordinal (culture-insensitive) search, where a character is considered");
                cb.WriteLine("/// equivalent to another character only if their Unicode scalar values are the same.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("public int IndexOf(char value) => _chars.Slice(0, _pos).IndexOf(value);");
            });
            cb.WriteLine();
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Searches for the index of the specified character. The search for the");
                cb.WriteLine("/// character starts at the specified offset and moves towards the end.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                cb.WriteLine("/// <returns>The index of the specified character, or <c>-1</c> if the character isn't found.</returns>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// Index numbering starts from zero.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// This method performs an ordinal (culture-insensitive) search, where a character is considered");
                cb.WriteLine("/// equivalent to another character only if their Unicode scalar values are the same.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("public int IndexOf(ReadOnlySpan<char> value)");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("=> ((ReadOnlySpan<char>)_chars.Slice(0, _pos)).IndexOf(value);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Searches for the index of the specified character. The search for the");
                cb.WriteLine("/// character starts at the specified offset and moves towards the end.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                cb.WriteLine("/// <param name=\"startIndex\">The search starting position.</param>");
                cb.WriteLine("/// <returns>The index of the specified character, or <c>-1</c> if the character isn't found.</returns>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// <paramref name=\"startIndex\"/> range checks are performed with <see cref=\"Debug.Assert(bool)\"/>.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// Index numbering starts from zero.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// This method performs an ordinal (culture-insensitive) search, where a character is considered");
                cb.WriteLine("/// equivalent to another character only if their Unicode scalar values are the same.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("public int IndexOf(ReadOnlySpan<char> value, int startIndex)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Debug.Assert((uint)startIndex <= (uint)_pos);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("return ((ReadOnlySpan<char>)_chars.Slice(startIndex, _pos - startIndex)).IndexOf(value) + startIndex;");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Searches for the index of the specified character. The search for the");
                cb.WriteLine("/// character starts at the specified offset and moves towards the end.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                cb.WriteLine("/// <param name=\"comparisonType\">One of the enumeration values that specifies the rules for the search.</param>");
                cb.WriteLine("/// <returns>The index of the specified character, or <c>-1</c> if the character isn't found.</returns>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// Index numbering starts from zero.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("public int IndexOf(ReadOnlySpan<char> value, StringComparison comparisonType)");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("=> ((ReadOnlySpan<char>)_chars.Slice(0, _pos)).IndexOf(value, comparisonType);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Searches for the index of the specified character. The search for the");
                cb.WriteLine("/// character starts at the specified offset and moves towards the end.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                cb.WriteLine("/// <param name=\"startIndex\">The search starting position.</param>");
                cb.WriteLine("/// <param name=\"comparisonType\">One of the enumeration values that specifies the rules for the search.</param>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// <paramref name=\"startIndex\"/> range checks are performed with <see cref=\"Debug.Assert(bool)\"/>.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// Index numbering starts from zero.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("public int IndexOf(ReadOnlySpan<char> value, int startIndex, StringComparison comparisonType)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Debug.Assert((uint)startIndex <= (uint)_pos);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("return ((ReadOnlySpan<char>)_chars.Slice(startIndex, _pos - startIndex)).IndexOf(value, comparisonType) + startIndex;");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.WriteLine("#if !NET5_0_OR_GREATER");
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Searches for the index of the specified character. The search for the");
                cb.WriteLine("/// character starts at the specified offset and moves towards the end.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                cb.WriteLine("/// <param name=\"comparisonType\">One of the enumeration values that specifies the rules for the search.</param>");
                cb.WriteLine("/// <returns>The index of the specified character, or <c>-1</c> if the character isn't found.</returns>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// Index numbering starts from zero.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("public int IndexOf(string value, StringComparison comparisonType)");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("=> ((ReadOnlySpan<char>)_chars.Slice(0, _pos)).IndexOf(value.AsSpan(), comparisonType);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Searches for the index of the specified character. The search for the");
                cb.WriteLine("/// character starts at the specified offset and moves towards the end.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                cb.WriteLine("/// <param name=\"startIndex\">The search starting position.</param>");
                cb.WriteLine("/// <param name=\"comparisonType\">One of the enumeration values that specifies the rules for the search.</param>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// <paramref name=\"startIndex\"/> range checks are performed with <see cref=\"Debug.Assert(bool)\"/>.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// Index numbering starts from zero.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("public int IndexOf(string value, int startIndex, StringComparison comparisonType)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Debug.Assert((uint)startIndex <= (uint)_pos);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("return ((ReadOnlySpan<char>)_chars.Slice(startIndex, _pos - startIndex)).IndexOf(value.AsSpan(), comparisonType) + startIndex;");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine("#endif");
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("#endregion IndexOf");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("#region LastIndexOf");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Reports the zero-based index position of the last occurrence of a specified Unicode character");
                cb.WriteLine("/// within this instance.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">The Unicode character to seek.</param>");
                cb.WriteLine("/// <returns>The zero-based index of the last occurrence of the value in the");
                cb.WriteLine("/// span. If not found, returns -1.</returns>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// Index numbering starts from zero. That is, the first character in the string");
                cb.WriteLine("/// is at index zero and the last is at <see cref=\"Length\"/> - 1.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// This method begins searching at the last character position of this instance");
                cb.WriteLine("/// and proceeds backward toward the beginning until either <paramref name=\"value\"/>");
                cb.WriteLine("/// is found or the first character position has been examined. The search is case-sensitive.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// This method performs an ordinal (culture-insensitive) search, where a character is");
                cb.WriteLine("/// considered equivalent to another character only if their Unicode scalar values are");
                cb.WriteLine("/// the same.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("public int LastIndexOf(char value) => System.MemoryExtensions.LastIndexOf(_chars.Slice(0, _pos), value);");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Searches for the index of the specified character. The search for the");
                cb.WriteLine("/// character starts at the specified offset and moves towards the beginning.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                cb.WriteLine("/// <returns>The index of the specified character, <c>-1</c> if the character isn't found.</returns>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// Index numbering starts from zero. That is, the first character in the string");
                cb.WriteLine("/// is at index zero and the last is at <see cref=\"Length\"/> - 1.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// This method begins searching at the last character position of this instance");
                cb.WriteLine("/// and proceeds backward toward the beginning until either <paramref name=\"value\"/>");
                cb.WriteLine("/// is found or the first character position has been examined. The search is case-sensitive.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// This method performs an ordinal (culture-insensitive) search, where a character is");
                cb.WriteLine("/// considered equivalent to another character only if their Unicode scalar values are");
                cb.WriteLine("/// the same.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("public int LastIndexOf(ReadOnlySpan<char> value)");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("=> ((ReadOnlySpan<char>)_chars.Slice(0, _pos)).LastIndexOf(value);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Searches for the index of the specified character. The search for the");
                cb.WriteLine("/// character starts at the specified offset and moves towards the beginning.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                cb.WriteLine("/// <param name=\"startIndex\">The search starting position. The search proceeds from");
                cb.WriteLine("/// <paramref name=\"startIndex\"/> toward the beginning of this instance.</param>");
                cb.WriteLine("/// <returns>The index of the specified character, <c>-1</c> if the character isn't found.</returns>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// <paramref name=\"startIndex\"/> range checks are performed with <see cref=\"Debug.Assert(bool)\"/>.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// Index numbering starts from zero. That is, the first character in the string");
                cb.WriteLine("/// is at index zero and the last is at <see cref=\"Length\"/> - 1.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// This method begins searching at the last character position of this instance");
                cb.WriteLine("/// and proceeds backward toward the beginning until either <paramref name=\"value\"/>");
                cb.WriteLine("/// is found or the first character position has been examined. The search is case-sensitive.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// This method performs an ordinal (culture-insensitive) search, where a character is");
                cb.WriteLine("/// considered equivalent to another character only if their Unicode scalar values are");
                cb.WriteLine("/// the same.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("public int LastIndexOf(ReadOnlySpan<char> value, int startIndex)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("if ((uint)startIndex >= (uint)_pos)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("// For consistency with string.LastIndexOf:");
                        cb.WriteLine("// if startIndex >= length, use full length; if empty, just return -1.");
                        cb.WriteLine("if (_pos == 0)");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("return -1;");
                        });
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
                        cb.WriteLine("startIndex = _pos - 1;");
                    });
                    cb.WriteLine("}");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("// Equivalent of searching within the substring that ends at startIndex (inclusive)");
                    cb.WriteLine("ReadOnlySpan<char> searchSpan = _chars.Slice(0, startIndex + 1);");
                    cb.WriteLine("return searchSpan.LastIndexOf(value);");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.WriteLine("#if NET5_0_OR_GREATER");
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Searches for the index of the specified character. The search for the");
                cb.WriteLine("/// character starts at the specified offset and moves towards the beginning.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                cb.WriteLine("/// <param name=\"comparisonType\">One of the enumeration values that specifies the rules for the search.</param>");
                cb.WriteLine("/// <returns>The index of the specified character, <c>-1</c> if the character isn't found.</returns>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// Index numbering starts from zero. That is, the first character in the string");
                cb.WriteLine("/// is at index zero and the last is at <see cref=\"Length\"/> - 1.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// The <paramref name=\"comparisonType\"/> parameter specifies to search for the <paramref name=\"value\"/>");
                cb.WriteLine("/// parameter using:");
                cb.WriteLine("/// <list type=\"bullet\">");
                cb.WriteLine("///     <item><description>The current or invariant culture.</description></item>");
                cb.WriteLine("///     <item><description>A case-sensitive or case-insensitive search.</description></item>");
                cb.WriteLine("///     <item><description>Word or ordinal comparison rules.</description></item>");
                cb.WriteLine("/// </list>");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// This method begins searching at the last character position of this instance");
                cb.WriteLine("/// and proceeds backward toward the beginning until either <paramref name=\"value\"/>");
                cb.WriteLine("/// is found or the first character position has been examined.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("public int LastIndexOf(ReadOnlySpan<char> value, StringComparison comparisonType)");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("=> ((ReadOnlySpan<char>)_chars.Slice(0, _pos)).LastIndexOf(value, comparisonType);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Searches for the index of the specified character. The search for the");
                cb.WriteLine("/// character starts at the specified offset and moves towards the beginning.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                cb.WriteLine("/// <param name=\"startIndex\">The search starting position. The search proceeds from");
                cb.WriteLine("/// <paramref name=\"startIndex\"/> toward the beginning of this instance.</param>");
                cb.WriteLine("/// <param name=\"comparisonType\">One of the enumeration values that specifies the rules for the search.</param>");
                cb.WriteLine("/// <returns>The index of the specified character, <c>-1</c> if the character isn't found.</returns>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// Index numbering starts from zero. That is, the first character in the string");
                cb.WriteLine("/// is at index zero and the last is at <see cref=\"Length\"/> - 1.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// The <paramref name=\"comparisonType\"/> parameter specifies to search for the <paramref name=\"value\"/>");
                cb.WriteLine("/// parameter using:");
                cb.WriteLine("/// <list type=\"bullet\">");
                cb.WriteLine("///     <item><description>The current or invariant culture.</description></item>");
                cb.WriteLine("///     <item><description>A case-sensitive or case-insensitive search.</description></item>");
                cb.WriteLine("///     <item><description>Word or ordinal comparison rules.</description></item>");
                cb.WriteLine("/// </list>");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// This method begins searching at the last character position of this instance");
                cb.WriteLine("/// and proceeds backward toward the beginning until either <paramref name=\"value\"/>");
                cb.WriteLine("/// is found or the first character position has been examined.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("public int LastIndexOf(ReadOnlySpan<char> value, int startIndex, StringComparison comparisonType)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("if ((uint)startIndex >= (uint)_pos)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("// For consistency with string.LastIndexOf:");
                        cb.WriteLine("// if startIndex >= length, use full length; if empty, just return -1.");
                        cb.WriteLine("if (_pos == 0)");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("return -1;");
                        });
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
                        cb.WriteLine("startIndex = _pos - 1;");
                    });
                    cb.WriteLine("}");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("// Equivalent of searching within the substring that ends at startIndex (inclusive)");
                    cb.WriteLine("ReadOnlySpan<char> searchSpan = _chars.Slice(0, startIndex + 1);");
                    cb.WriteLine("return searchSpan.LastIndexOf(value, comparisonType);");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.WriteLine("#endif");
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("#endregion LastIndexOf");
            });
            cb.WriteLine("}");

            return sb.ToString();
        }
    }
}
