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

            bool useSpanUtilities = options.IncludesJ2N_2_2_Or_Greater && options.UseJavaStyleIndexOf;

            if (useSpanUtilities)
            {
                cb.WriteLine("using J2N;");
            }
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
                cb.WriteLine("public readonly int IndexOf(char value) => System.MemoryExtensions.IndexOf(_chars.Slice(0, _pos), value);");
            });
            cb.WriteLine();
            if (useSpanUtilities)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the first occurrence of the specified string.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <returns>The zero-based index position of <paramref name=\"value\"/> from the start of");
                    cb.WriteLine("/// the current instance if that sequence of characters is found, or -1 if it is not.");
                    cb.WriteLine("/// If <paramref name=\"value\"/> is <see cref=\"string.Empty\"/>, the return value");
                    cb.WriteLine("/// is 0.</returns>");
                    cb.WriteLine("/// <exception cref=\"ArgumentNullException\"><paramref name=\"value\"/> is <c>null</c>.</exception>");
                    cb.WriteLine("/// <remarks>");
                    cb.WriteLine("/// Index numbering starts from zero.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// This method performs an ordinal (culture-insensitive) search, where a character is considered");
                    cb.WriteLine("/// equivalent to another character only if their Unicode scalar values are the same.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// To match the behavior of the JDK, this method allows searches for the empty string, which will");
                    cb.WriteLine("/// always return 0.");
                    cb.WriteLine("/// </remarks>");
                    cb.WriteLine("public readonly int IndexOf(string value) // Coverage for the JDK");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("=> SpanUtilities.IndexOf(_chars.Slice(0, _pos), value);");
                    });
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the first occurrence of the specified string.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <returns>The index of the specified character, or <c>-1</c> if the character isn't found.</returns>");
                    cb.WriteLine("/// <exception cref=\"ArgumentNullException\"><paramref name=\"value\"/> is <c>null</c>.</exception>");
                    cb.WriteLine("/// <remarks>");
                    cb.WriteLine("/// Index numbering starts from zero.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// This method performs an ordinal (culture-insensitive) search, where a character is considered");
                    cb.WriteLine("/// equivalent to another character only if their Unicode scalar values are the same.");
                    cb.WriteLine("/// </remarks>");
                    cb.WriteLine("public readonly int IndexOf(string value)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("if (value is null)");
                        cb.WriteLine("    throw new ArgumentNullException(nameof(value));");
                        cb.WriteLine();
                        cb.WriteLine("return System.MemoryExtensions.IndexOf(_chars.Slice(0, _pos), value.AsSpan());");
                    });
                    cb.WriteLine("}");
                });
            }
            cb.WriteLine();
            if (useSpanUtilities)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the first occurrence of the specified string.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <returns>The zero-based index position of <paramref name=\"value\"/> from the start of");
                    cb.WriteLine("/// the current instance if that sequence of characters is found, or -1 if it is not.");
                    cb.WriteLine("/// If <paramref name=\"value\"/> is <see cref=\"string.Empty\"/>, the return value");
                    cb.WriteLine("/// is 0.</returns>");
                    cb.WriteLine("/// <remarks>");
                    cb.WriteLine("/// Index numbering starts from zero.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// This method performs an ordinal (culture-insensitive) search, where a character is considered");
                    cb.WriteLine("/// equivalent to another character only if their Unicode scalar values are the same.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// To match the behavior of the JDK, this method allows searches for the empty string, which will");
                    cb.WriteLine("/// always return 0.");
                    cb.WriteLine("/// </remarks>");
                    cb.WriteLine("public readonly int IndexOf(ReadOnlySpan<char> value) // Coverage for the JDK");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("=> SpanUtilities.IndexOf(_chars.Slice(0, _pos), value);");
                    });
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the first occurrence of the specified string.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <returns>The index of the specified character, or <c>-1</c> if the character isn't found.</returns>");
                    cb.WriteLine("/// <remarks>");
                    cb.WriteLine("/// Index numbering starts from zero.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// This method performs an ordinal (culture-insensitive) search, where a character is considered");
                    cb.WriteLine("/// equivalent to another character only if their Unicode scalar values are the same.");
                    cb.WriteLine("/// </remarks>");
                    cb.WriteLine("public readonly int IndexOf(ReadOnlySpan<char> value)");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("=> System.MemoryExtensions.IndexOf(_chars.Slice(0, _pos), value);");
                    });
                });
            }
            cb.WriteLine();
            if (useSpanUtilities)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the first occurrence of the specified string beginning");
                    cb.WriteLine("/// at the specified index in this instance.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <param name=\"startIndex\">The search starting position.</param>");
                    cb.WriteLine("/// <returns>The zero-based index position of <paramref name=\"value\"/> from the specified index of");
                    cb.WriteLine("/// the current instance if that sequence of characters is found, or -1 if it is not.");
                    cb.WriteLine("/// If <paramref name=\"value\"/> is empty, the return value is the effective start index");
                    cb.WriteLine("/// after clamping.</returns>");
                    cb.WriteLine("/// <exception cref=\"ArgumentNullException\"><paramref name=\"value\"/> is <c>null</c>.</exception>");
                    cb.WriteLine("/// <remarks>");
                    cb.WriteLine("/// Index numbering starts from zero. The <paramref name=\"startIndex\"/> parameter is clamped to");
                    cb.WriteLine("/// the valid range of the current instance. Values less than zero are treated as zero, and values");
                    cb.WriteLine("/// greater than <see cref = \"Length\" /> are treated as equal to <see cref = \"Length\" />.");
                    cb.WriteLine("/// If <paramref name=\"startIndex\"/> equals <see cref = \"Length\" />, this method");
                    cb.WriteLine("/// returns -1 for non-empty searches. If <paramref name=\"value\"/> is empty, this method returns");
                    cb.WriteLine("/// the effective start index after clamping. The search is case-sensitive.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// This method performs an ordinal (culture-insensitive) search, where a character is considered");
                    cb.WriteLine("/// equivalent to another character only if their Unicode scalar values are the same.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// To match the behavior of the JDK, this method allows searches for the empty string, which will");
                    cb.WriteLine("/// always return <paramref name=\"startIndex\"/>.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// If <paramref name=\"startIndex\"/> is less than zero, it is treated as zero. If it is greater");
                    cb.WriteLine("/// than <see cref=\"Length\"/>, it is treated as equal to <see cref=\"Length\"/>.");
                    cb.WriteLine("/// </remarks>");
                    cb.WriteLine("public readonly int IndexOf(string value, int startIndex) // Coverage for the JDK");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("=> SpanUtilities.IndexOf(_chars.Slice(0, _pos), value, startIndex);");
                    });
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Searches for the index of the specified character. The search for the");
                    cb.WriteLine("/// character starts at the specified offset and moves towards the end.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <param name=\"startIndex\">The search starting position.</param>");
                    cb.WriteLine("/// <returns>The index of the specified character, or <c>-1</c> if the character isn't found.</returns>");
                    cb.WriteLine("/// <exception cref=\"ArgumentNullException\"><paramref name=\"value\"/> is <c>null</c>.</exception>");
                    cb.WriteLine("/// <remarks>");
                    cb.WriteLine("/// <paramref name=\"startIndex\"/> range checks are performed with <see cref=\"Debug.Assert(bool)\"/>.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// Index numbering starts from zero.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// This method performs an ordinal (culture-insensitive) search, where a character is considered");
                    cb.WriteLine("/// equivalent to another character only if their Unicode scalar values are the same.");
                    cb.WriteLine("/// </remarks>");
                    cb.WriteLine("public readonly int IndexOf(string value, int startIndex)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("if (value is null)");
                        cb.WriteLine("    throw new ArgumentNullException(nameof(value));");
                        cb.WriteLine();
                        cb.WriteLine("Debug.Assert((uint)startIndex < _chars.Length);");
                        cb.WriteLine();
                        cb.WriteLine("int actualIndex = System.MemoryExtensions.IndexOf(_chars.Slice(startIndex, _pos - startIndex), value.AsSpan());");
                        cb.WriteLine("return actualIndex >= 0 ? startIndex + actualIndex : -1;");
                    });
                    cb.WriteLine("}");
                });
            }
            cb.WriteLine();


            if (useSpanUtilities)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the first occurrence of the specified string beginning");
                    cb.WriteLine("/// at the specified index in this instance.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <param name=\"startIndex\">The search starting position.</param>");
                    cb.WriteLine("/// <returns>The zero-based index position of <paramref name=\"value\"/> from the specified index of");
                    cb.WriteLine("/// the current instance if that sequence of characters is found, or -1 if it is not.");
                    cb.WriteLine("/// If <paramref name=\"value\"/> is empty, the return value is the effective start index");
                    cb.WriteLine("/// after clamping.</returns>");
                    cb.WriteLine("/// <remarks>");
                    cb.WriteLine("/// <paramref name=\"startIndex\"/> range checks are performed with <see cref=\"Debug.Assert(bool)\"/>.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// Index numbering starts from zero. The <paramref name=\"startIndex\"/> parameter is clamped to");
                    cb.WriteLine("/// the valid range of the current instance. Values less than zero are treated as zero, and values");
                    cb.WriteLine("/// greater than <see cref = \"Length\" /> are treated as equal to <see cref = \"Length\" />.");
                    cb.WriteLine("/// If <paramref name=\"startIndex\"/> equals <see cref = \"Length\" />, this method");
                    cb.WriteLine("/// returns -1 for non-empty searches. If <paramref name=\"value\"/> is empty, this method returns");
                    cb.WriteLine("/// the effective start index after clamping. The search is case-sensitive.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// This method performs an ordinal (culture-insensitive) search, where a character is considered");
                    cb.WriteLine("/// equivalent to another character only if their Unicode scalar values are the same.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// To match the behavior of the JDK, this method allows searches for the empty string, which will");
                    cb.WriteLine("/// always return <paramref name=\"startIndex\"/>.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// If <paramref name=\"startIndex\"/> is less than zero, it is treated as zero. If it is greater");
                    cb.WriteLine("/// than <see cref=\"Length\"/>, it is treated as equal to <see cref=\"Length\"/>.");
                    cb.WriteLine("/// </remarks>");
                    cb.WriteLine("public readonly int IndexOf(ReadOnlySpan<char> value, int startIndex) // Coverage for the JDK");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("=> SpanUtilities.IndexOf(_chars.Slice(0, _pos), value, startIndex);");
                    });
                });
            }
            else
            {
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
                    cb.WriteLine("public readonly int IndexOf(ReadOnlySpan<char> value, int startIndex)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Debug.Assert((uint)startIndex < _chars.Length);");
                        cb.WriteLine();
                        cb.WriteLine("int actualIndex = System.MemoryExtensions.IndexOf(_chars.Slice(startIndex, _pos - startIndex), value);");
                        cb.WriteLine("return actualIndex >= 0 ? startIndex + actualIndex : -1;");
                    });
                    cb.WriteLine("}");
                });
            }
            cb.WriteLine();


            if (useSpanUtilities)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the first occurrence of the specified string.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <param name=\"comparisonType\">One of the enumeration values that determines how the current instance");
                    cb.WriteLine("/// and <paramref name=\"value\"/> are compared.</param>");
                    cb.WriteLine("/// <returns>The zero-based index position of <paramref name=\"value\"/> from the start of");
                    cb.WriteLine("/// the current instance if that sequence of characters is found, or -1 if it is not.");
                    cb.WriteLine("/// If <paramref name=\"value\"/> is <see cref=\"string.Empty\"/>, the return value");
                    cb.WriteLine("/// is 0.</returns>");
                    cb.WriteLine("/// <exception cref=\"ArgumentNullException\"><paramref name=\"value\"/> is <c>null</c>.</exception>");
                    cb.WriteLine("/// <remarks>");
                    cb.WriteLine("/// Index numbering starts from zero.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// The <paramref name=\"comparisonType\"/> parameter specifies to search for the <paramref name=\"value\"/>");
                    cb.WriteLine("/// parameter using the current or invariant culture, using a case-sensitive or case-insensitive search,");
                    cb.WriteLine("/// and using word or ordinal comparison rules.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// To match the behavior of the JDK, this method allows searches for the empty string, which will");
                    cb.WriteLine("/// always return 0.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// On older platforms than .NET Core, this overload provides optimizations for");
                    cb.WriteLine("/// <see cref=\"StringComparison.OrdinalIgnoreCase\"/> over and above the System.Memory package.");
                    cb.WriteLine("/// </remarks>");
                    cb.WriteLine("public readonly int IndexOf(string value, StringComparison comparisonType)");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("=> SpanUtilities.IndexOf(_chars.Slice(0, _pos), value, comparisonType);");
                    });
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the first occurrence of the specified string.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <param name=\"comparisonType\">One of the enumeration values that specifies the rules for the search.</param>");
                    cb.WriteLine("/// <returns>The index of the specified character, or <c>-1</c> if the character isn't found.</returns>");
                    cb.WriteLine("/// <remarks>");
                    cb.WriteLine("/// Index numbering starts from zero.");
                    cb.WriteLine("/// </remarks>");
                    cb.WriteLine("public readonly int IndexOf(string value, StringComparison comparisonType)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("if (value is null)");
                        cb.WriteLine("    throw new ArgumentNullException(nameof(value));");
                        cb.WriteLine();
                        cb.WriteLine("return System.MemoryExtensions.IndexOf(_chars.Slice(0, _pos), value.AsSpan(), comparisonType);");
                    });
                    cb.WriteLine("}");
                });
            }
            cb.WriteLine();

            if (useSpanUtilities)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the first occurrence of the specified string.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <param name=\"comparisonType\">One of the enumeration values that determines how the current instance");
                    cb.WriteLine("/// and <paramref name=\"value\"/> are compared.</param>");
                    cb.WriteLine("/// <returns>The zero-based index position of <paramref name=\"value\"/> from the start of");
                    cb.WriteLine("/// the current instance if that sequence of characters is found, or -1 if it is not.");
                    cb.WriteLine("/// If <paramref name=\"value\"/> is <see cref=\"string.Empty\"/>, the return value");
                    cb.WriteLine("/// is 0.</returns>");
                    cb.WriteLine("/// <remarks>");
                    cb.WriteLine("/// Index numbering starts from zero.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// The <paramref name=\"comparisonType\"/> parameter specifies to search for the <paramref name=\"value\"/>");
                    cb.WriteLine("/// parameter using the current or invariant culture, using a case-sensitive or case-insensitive search,");
                    cb.WriteLine("/// and using word or ordinal comparison rules.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// To match the behavior of the JDK, this method allows searches for the empty string, which will");
                    cb.WriteLine("/// always return 0.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// On older platforms than .NET Core, this overload provides optimizations for");
                    cb.WriteLine("/// <see cref=\"StringComparison.OrdinalIgnoreCase\"/> over and above the System.Memory package.");
                    cb.WriteLine("/// </remarks>");
                    cb.WriteLine("public readonly int IndexOf(ReadOnlySpan<char> value, StringComparison comparisonType)");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("=> SpanUtilities.IndexOf(_chars.Slice(0, _pos), value, comparisonType);");
                    });
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the first occurrence of the specified string.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <param name=\"comparisonType\">One of the enumeration values that specifies the rules for the search.</param>");
                    cb.WriteLine("/// <returns>The index of the specified character, or <c>-1</c> if the character isn't found.</returns>");
                    cb.WriteLine("/// <remarks>");
                    cb.WriteLine("/// Index numbering starts from zero.");
                    cb.WriteLine("/// </remarks>");
                    cb.WriteLine("public readonly int IndexOf(ReadOnlySpan<char> value, StringComparison comparisonType)");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("=> System.MemoryExtensions.IndexOf(_chars.Slice(0, _pos), value, comparisonType);");
                    });
                });
            }
            cb.WriteLine();

            if (useSpanUtilities)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the first occurrence of the specified string beginning");
                    cb.WriteLine("/// at the specified index in this instance.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <param name=\"startIndex\">The search starting position.</param>");
                    cb.WriteLine("/// <param name=\"comparisonType\">One of the enumeration values that determines how the current instance");
                    cb.WriteLine("/// and <paramref name=\"value\"/> are compared.</param>");
                    cb.WriteLine("/// <returns>The zero-based index position of <paramref name=\"value\"/> from the start of");
                    cb.WriteLine("/// the current instance if that sequence of characters is found, or -1 if it is not.");
                    cb.WriteLine("/// If <paramref name=\"value\"/> is empty, the return value is the effective start index");
                    cb.WriteLine("/// after clamping.</returns>");
                    cb.WriteLine("/// <exception cref=\"ArgumentNullException\"><paramref name=\"value\"/> is <c>null</c>.</exception>");
                    cb.WriteLine("/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"startIndex\"/> is less than zero");
                    cb.WriteLine("/// or greater than <see cref=\"Length\"/>.</exception>");
                    cb.WriteLine("/// <exception cref=\"ArgumentException\"><paramref name=\"comparisonType\"/> is not a");
                    cb.WriteLine("/// <see cref=\"StringComparison\"/> value.</exception>");
                    cb.WriteLine("/// <remarks>");
                    cb.WriteLine("/// Index numbering starts from zero. The <paramref name=\"startIndex\"/> parameter is clamped to");
                    cb.WriteLine("/// the valid range of the current instance. Values less than zero are treated as zero, and values");
                    cb.WriteLine("/// greater than <see cref = \"Length\" /> are treated as equal to <see cref = \"Length\" />.");
                    cb.WriteLine("/// If <paramref name=\"startIndex\"/> equals <see cref = \"Length\" />, this method");
                    cb.WriteLine("/// returns -1 for non-empty searches. If <paramref name=\"value\"/> is empty, this method returns");
                    cb.WriteLine("/// the effective start index after clamping.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// The <paramref name=\"comparisonType\"/> parameter specifies to search for the <paramref name=\"value\"/>");
                    cb.WriteLine("/// parameter using the current or invariant culture, using a case-sensitive or case-insensitive search,");
                    cb.WriteLine("/// and using word or ordinal comparison rules.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// To match the behavior of the JDK, this method allows searches for the empty string, which will");
                    cb.WriteLine("/// always return 0.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// If <paramref name=\"startIndex\"/> is less than zero, it is treated as zero. If it is greater");
                    cb.WriteLine("/// than <see cref=\"Length\"/>, it is treated as equal to <see cref=\"Length\"/>.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// On older platforms than .NET Core, this overload provides optimizations for");
                    cb.WriteLine("/// <see cref=\"StringComparison.OrdinalIgnoreCase\"/> over and above the System.Memory package.");
                    cb.WriteLine("/// </remarks>");
                    cb.WriteLine("public readonly int IndexOf(string value, int startIndex, StringComparison comparisonType)");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("=> SpanUtilities.IndexOf(_chars.Slice(0, _pos), value, startIndex, comparisonType);");
                    });
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the first occurrence of the specified string beginning");
                    cb.WriteLine("/// at the specified index in this instance.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <param name=\"startIndex\">The search starting position.</param>");
                    cb.WriteLine("/// <param name=\"comparisonType\">One of the enumeration values that specifies the rules for the search.</param>");
                    cb.WriteLine("/// <remarks>");
                    cb.WriteLine("/// Index numbering starts from zero.");
                    cb.WriteLine("/// </remarks>");
                    cb.WriteLine("public readonly int IndexOf(string value, int startIndex, StringComparison comparisonType)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("if (value is null)");
                        cb.WriteLine("    throw new ArgumentNullException(nameof(value));");
                        cb.WriteLine();
                        cb.WriteLine("int actualIndex = System.MemoryExtensions.IndexOf(_chars.Slice(startIndex, _pos - startIndex), value.AsSpan(), comparisonType);");
                        cb.WriteLine("return actualIndex >= 0 ? startIndex + actualIndex : -1;");
                    });
                    cb.WriteLine("}");
                });
            }
            cb.WriteLine();

            if (useSpanUtilities)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the first occurrence of the specified string beginning");
                    cb.WriteLine("/// at the specified index in this instance.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <param name=\"startIndex\">The search starting position.</param>");
                    cb.WriteLine("/// <param name=\"comparisonType\">One of the enumeration values that determines how the current instance");
                    cb.WriteLine("/// and <paramref name=\"value\"/> are compared.</param>");
                    cb.WriteLine("/// <returns>The zero-based index position of <paramref name=\"value\"/> from the start of");
                    cb.WriteLine("/// the current instance if that sequence of characters is found, or -1 if it is not.");
                    cb.WriteLine("/// If <paramref name=\"value\"/> is empty, the return value is the effective start index");
                    cb.WriteLine("/// after clamping.</returns>");
                    cb.WriteLine("/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"startIndex\"/> is less than zero");
                    cb.WriteLine("/// or greater than <see cref=\"Length\"/>.</exception>");
                    cb.WriteLine("/// <exception cref=\"ArgumentException\"><paramref name=\"comparisonType\"/> is not a");
                    cb.WriteLine("/// <see cref=\"StringComparison\"/> value.</exception>");
                    cb.WriteLine("/// <remarks>");
                    cb.WriteLine("/// Index numbering starts from zero. The <paramref name=\"startIndex\"/> parameter is clamped to");
                    cb.WriteLine("/// the valid range of the current instance. Values less than zero are treated as zero, and values");
                    cb.WriteLine("/// greater than <see cref = \"Length\" /> are treated as equal to <see cref = \"Length\" />.");
                    cb.WriteLine("/// If <paramref name=\"startIndex\"/> equals <see cref = \"Length\" />, this method");
                    cb.WriteLine("/// returns -1 for non-empty searches. If <paramref name=\"value\"/> is empty, this method returns");
                    cb.WriteLine("/// the effective start index after clamping.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// The <paramref name=\"comparisonType\"/> parameter specifies to search for the <paramref name=\"value\"/>");
                    cb.WriteLine("/// parameter using the current or invariant culture, using a case-sensitive or case-insensitive search,");
                    cb.WriteLine("/// and using word or ordinal comparison rules.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// To match the behavior of the JDK, this method allows searches for the empty string, which will");
                    cb.WriteLine("/// always return 0.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// If <paramref name=\"startIndex\"/> is less than zero, it is treated as zero. If it is greater");
                    cb.WriteLine("/// than <see cref=\"Length\"/>, it is treated as equal to <see cref=\"Length\"/>.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// On older platforms than .NET Core, this overload provides optimizations for");
                    cb.WriteLine("/// <see cref=\"StringComparison.OrdinalIgnoreCase\"/> over and above the System.Memory package.");
                    cb.WriteLine("/// </remarks>");
                    cb.WriteLine("public readonly int IndexOf(ReadOnlySpan<char> value, int startIndex, StringComparison comparisonType)");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("=> SpanUtilities.IndexOf(_chars.Slice(0, _pos), value, startIndex, comparisonType);");
                    });
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the first occurrence of the specified string beginning");
                    cb.WriteLine("/// at the specified index in this instance.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <param name=\"startIndex\">The search starting position.</param>");
                    cb.WriteLine("/// <param name=\"comparisonType\">One of the enumeration values that specifies the rules for the search.</param>");
                    cb.WriteLine("/// <remarks>");
                    cb.WriteLine("/// Index numbering starts from zero.");
                    cb.WriteLine("/// </remarks>");
                    cb.WriteLine("public readonly int IndexOf(ReadOnlySpan<char> value, int startIndex, StringComparison comparisonType)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("int actualIndex = System.MemoryExtensions.IndexOf(_chars.Slice(startIndex, _pos - startIndex), value, comparisonType);");
                        cb.WriteLine("return actualIndex >= 0 ? startIndex + actualIndex : -1;");
                    });
                    cb.WriteLine("}");
                });
            }
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
                cb.WriteLine("public readonly int LastIndexOf(char value) => System.MemoryExtensions.LastIndexOf(_chars.Slice(0, _pos), value);");
            });
            cb.WriteLine();


            if (useSpanUtilities)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the last occurrence of the specified string in");
                    cb.WriteLine("/// this instance.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <returns>The zero-based starting index position of value if that string is found, or -1");
                    cb.WriteLine("/// if it is not found. If <paramref name=\"value\"/> is <see cref=\"string.Empty\"/>,");
                    cb.WriteLine("/// it returns <see cref=\"Length\"/>.</returns>");
                    cb.WriteLine("/// <exception cref=\"ArgumentNullException\"><paramref name=\"value\"/> is <c>null</c>.</exception>");
                    cb.WriteLine("/// <remarks>");
                    cb.WriteLine("/// Index numbering starts from zero. That is, the first character in the current instance");
                    cb.WriteLine("/// is at index zero and the last is at <see cref=\"Length\"/> - 1.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// This method begins searching at the last character position of this instance");
                    cb.WriteLine("/// and proceeds backward toward the beginning until either <paramref name=\"value\"/>");
                    cb.WriteLine("/// is found or the first character position has been examined. The search is case-sensitive.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// This method performs an ordinal (culture-insensitive) search, where a character is");
                    cb.WriteLine("/// considered equivalent to another character only if their Unicode scalar values are");
                    cb.WriteLine("/// the same.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// To match the behavior of the JDK, this method allows searches for the empty string, which will");
                    cb.WriteLine("/// always return <see cref=\"Length\"/>.");
                    cb.WriteLine("/// </remarks>");
                    cb.WriteLine("public readonly int LastIndexOf(string value) // Coverage for the JDK");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("=> SpanUtilities.LastIndexOf(_chars.Slice(0, _pos), value);");
                    });
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the last occurrence of the specified string in");
                    cb.WriteLine("/// this instance.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <returns>The index of the specified character, <c>-1</c> if the character isn't found.</returns>");
                    cb.WriteLine("/// <exception cref=\"ArgumentNullException\"><paramref name=\"value\"/> is <c>null</c>.</exception>");
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
                    cb.WriteLine("public readonly int LastIndexOf(string value)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("if (value is null)");
                        cb.WriteLine("    throw new ArgumentNullException(nameof(value));");
                        cb.WriteLine();
                        cb.WriteLine("return System.MemoryExtensions.LastIndexOf(_chars.Slice(0, _pos), value.AsSpan());");
                    });
                    cb.WriteLine("}");
                });
            }
            cb.WriteLine();

            if (useSpanUtilities)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the last occurrence of the specified string in");
                    cb.WriteLine("/// this instance.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <returns>The zero-based starting index position of value if that string is found, or -1");
                    cb.WriteLine("/// if it is not found. If <paramref name=\"value\"/> is <see cref=\"string.Empty\"/>,");
                    cb.WriteLine("/// it returns <see cref=\"Length\"/>.</returns>");
                    cb.WriteLine("/// <remarks>");
                    cb.WriteLine("/// Index numbering starts from zero. That is, the first character in the current instance");
                    cb.WriteLine("/// is at index zero and the last is at <see cref=\"Length\"/> - 1.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// This method begins searching at the last character position of this instance");
                    cb.WriteLine("/// and proceeds backward toward the beginning until either <paramref name=\"value\"/>");
                    cb.WriteLine("/// is found or the first character position has been examined. The search is case-sensitive.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// This method performs an ordinal (culture-insensitive) search, where a character is");
                    cb.WriteLine("/// considered equivalent to another character only if their Unicode scalar values are");
                    cb.WriteLine("/// the same.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// To match the behavior of the JDK, this method allows searches for the empty string, which will");
                    cb.WriteLine("/// always return <see cref=\"Length\"/>.");
                    cb.WriteLine("/// </remarks>");
                    cb.WriteLine("public readonly int LastIndexOf(ReadOnlySpan<char> value) // Coverage for the JDK");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("=> SpanUtilities.LastIndexOf(_chars.Slice(0, _pos), value);");
                    });
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the last occurrence of the specified string in");
                    cb.WriteLine("/// this instance.");
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
                    cb.WriteLine("public readonly int LastIndexOf(ReadOnlySpan<char> value)");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("=> System.MemoryExtensions.LastIndexOf(_chars.Slice(0, _pos), value);");
                    });
                });
            }
            cb.WriteLine();

            if (useSpanUtilities)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the last occurrence of the specified string beginning");
                    cb.WriteLine("/// at the specified index in this instance.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <param name=\"startIndex\">The search starting position. The search proceeds from");
                    cb.WriteLine("/// <paramref name=\"startIndex\"/> toward the beginning of this instance.</param>");
                    cb.WriteLine("/// <returns>The zero-based starting index position of value if that string is found, or -1");
                    cb.WriteLine("/// if it is not found. If <paramref name=\"value\"/> is <see cref=\"string.Empty\"/>,");
                    cb.WriteLine("/// it returns <paramref name=\"startIndex\"/> if it is within the bounds of the current instance; otherwise,");
                    cb.WriteLine("/// if <paramref name=\"startIndex\"/> is greater than <see cref=\"Length\"/>, it");
                    cb.WriteLine("/// returns <see cref=\"Length\"/>.</returns>");
                    cb.WriteLine("/// <exception cref=\"ArgumentNullException\"><paramref name=\"value\"/> is <c>null</c>.</exception>");
                    cb.WriteLine("/// <remarks>");
                    cb.WriteLine("/// Index numbering starts from zero. That is, the first character in the current instance");
                    cb.WriteLine("/// is at index zero and the last is at <see cref=\"Length\"/> - 1.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// The search begins at the effective starting position and proceeds backward until either");
                    cb.WriteLine("/// <paramref name=\"value\"/> is found or the first character position has been examined.");
                    cb.WriteLine("/// The effective starting position is the lesser of <paramref name=\"startIndex\"/> and");
                    cb.WriteLine("/// <see cref=\"Length\"/> - <paramref name=\"value\"/>.Length. For example, if");
                    cb.WriteLine("/// <paramref name=\"startIndex\"/> is <see cref=\"Length\"/> - 1 and <paramref name=\"value\"/>");
                    cb.WriteLine("/// contains a single character, the method searches every character from the last");
                    cb.WriteLine("/// character in the string to the beginning. The search is case-sensitive.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// This method performs an ordinal (culture-insensitive) search, where a character is");
                    cb.WriteLine("/// considered equivalent to another character only if their Unicode scalar values are");
                    cb.WriteLine("/// the same.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// To match the behavior of the JDK, this method allows searches for the empty string, which will");
                    cb.WriteLine("/// always return the lesser of <paramref name=\"startIndex\"/> or <see cref=\"Length\"/>.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// If <paramref name=\"startIndex\"/> is less than zero, the method returns -1. If it is greater than");
                    cb.WriteLine("/// <see cref=\"Length\"/>, it is treated as equal to <see cref=\"Length\"/>.");
                    cb.WriteLine("/// </remarks>");
                    cb.WriteLine("public readonly int LastIndexOf(string value, int startIndex) // Coverage for the JDK");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("=> SpanUtilities.LastIndexOf(_chars.Slice(0, _pos), value, startIndex);");
                    });
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the last occurrence of the specified string beginning");
                    cb.WriteLine("/// at the specified index in this instance.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <param name=\"startIndex\">The search starting position. The search proceeds from");
                    cb.WriteLine("/// <paramref name=\"startIndex\"/> toward the beginning of this instance.</param>");
                    cb.WriteLine("/// <returns>The index of the specified character, <c>-1</c> if the character isn't found.</returns>");
                    cb.WriteLine("/// <exception cref=\"ArgumentNullException\"><paramref name=\"value\"/> is <c>null</c>.</exception>");
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
                    cb.WriteLine("public readonly int LastIndexOf(string value, int startIndex)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("if (value is null)");
                        cb.WriteLine("    throw new ArgumentNullException(nameof(value));");
                        cb.WriteLine();
                        cb.WriteLine("if ((uint)startIndex >= (uint)_pos)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("// For consistency with string.LastIndexOf:");
                            cb.WriteLine("// if startIndex >= length, use full length; if empty, just return -1.");
                            cb.WriteLine("if (_pos == 0)");
                            cb.WriteLine("    return -1;");
                            cb.WriteLine();
                            cb.WriteLine("startIndex = _pos - 1;");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine();
                        cb.WriteLine("// Equivalent of searching within the substring that ends at startIndex (inclusive)");
                        cb.WriteLine("ReadOnlySpan<char> searchSpan = _chars.Slice(0, startIndex + 1);");
                        cb.WriteLine("return System.MemoryExtensions.LastIndexOf(searchSpan, value.AsSpan());");
                    });
                    cb.WriteLine("}");
                });
            }
            cb.WriteLine();

            if (useSpanUtilities)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the last occurrence of the specified string beginning");
                    cb.WriteLine("/// at the specified index in this instance.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <param name=\"startIndex\">The search starting position. The search proceeds from");
                    cb.WriteLine("/// <paramref name=\"startIndex\"/> toward the beginning of this instance.</param>");
                    cb.WriteLine("/// <returns>The zero-based starting index position of value if that string is found, or -1");
                    cb.WriteLine("/// if it is not found. If <paramref name=\"value\"/> is <see cref=\"string.Empty\"/>,");
                    cb.WriteLine("/// it returns <paramref name=\"startIndex\"/> if it is within the bounds of the current instance; otherwise,");
                    cb.WriteLine("/// if <paramref name=\"startIndex\"/> is greater than <see cref=\"Length\"/>, it");
                    cb.WriteLine("/// returns <see cref=\"Length\"/>.</returns>");
                    cb.WriteLine("/// <remarks>");
                    cb.WriteLine("/// Index numbering starts from zero. That is, the first character in the current instance");
                    cb.WriteLine("/// is at index zero and the last is at <see cref=\"Length\"/> - 1.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// The search begins at the effective starting position and proceeds backward until either");
                    cb.WriteLine("/// <paramref name=\"value\"/> is found or the first character position has been examined.");
                    cb.WriteLine("/// The effective starting position is the lesser of <paramref name=\"startIndex\"/> and");
                    cb.WriteLine("/// <see cref=\"Length\"/> - <paramref name=\"value\"/>.Length. For example, if");
                    cb.WriteLine("/// <paramref name=\"startIndex\"/> is <see cref=\"Length\"/> - 1 and <paramref name=\"value\"/>");
                    cb.WriteLine("/// contains a single character, the method searches every character from the last");
                    cb.WriteLine("/// character in the string to the beginning. The search is case-sensitive.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// This method performs an ordinal (culture-insensitive) search, where a character is");
                    cb.WriteLine("/// considered equivalent to another character only if their Unicode scalar values are");
                    cb.WriteLine("/// the same.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// To match the behavior of the JDK, this method allows searches for the empty string, which will");
                    cb.WriteLine("/// always return the lesser of <paramref name=\"startIndex\"/> or <see cref=\"Length\"/>.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// If <paramref name=\"startIndex\"/> is less than zero, the method returns -1. If it is greater than");
                    cb.WriteLine("/// <see cref=\"Length\"/>, it is treated as equal to <see cref=\"Length\"/>.");
                    cb.WriteLine("/// </remarks>");
                    cb.WriteLine("public readonly int LastIndexOf(ReadOnlySpan<char> value, int startIndex) // Coverage for the JDK");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("=> SpanUtilities.LastIndexOf(_chars.Slice(0, _pos), value, startIndex);");
                    });
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the last occurrence of the specified string beginning");
                    cb.WriteLine("/// at the specified index in this instance.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <param name=\"startIndex\">The search starting position. The search proceeds from");
                    cb.WriteLine("/// <paramref name=\"startIndex\"/> toward the beginning of this instance.</param>");
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
                    cb.WriteLine("public readonly int LastIndexOf(ReadOnlySpan<char> value, int startIndex) // Coverage for the JDK");
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
                            cb.WriteLine("    return -1;");
                            cb.WriteLine();
                            cb.WriteLine("startIndex = _pos - 1;");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine();
                        cb.WriteLine("// Equivalent of searching within the substring that ends at startIndex (inclusive)");
                        cb.WriteLine("ReadOnlySpan<char> searchSpan = _chars.Slice(0, startIndex + 1);");
                        cb.WriteLine("return System.MemoryExtensions.LastIndexOf(searchSpan, value);");
                    });
                    cb.WriteLine("}");
                });
            }
            cb.WriteLine();

            if (!useSpanUtilities)
            {
                cb.WriteLine("#if NET5_0_OR_GREATER");
                cb.WriteLine();
            }

            if (useSpanUtilities)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the last occurrence of the specified string in the");
                    cb.WriteLine("/// this instance.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <param name=\"comparisonType\">One of the enumeration values that determines how the current instance");
                    cb.WriteLine("/// and <paramref name=\"value\"/> are compared.</param>");
                    cb.WriteLine("/// <returns>The zero-based starting index position of value if that string is found, or -1");
                    cb.WriteLine("/// if it is not found. If <paramref name=\"value\"/> is <see cref=\"string.Empty\"/>,");
                    cb.WriteLine("/// it returns <see cref=\"Length\"/>.</returns>");
                    cb.WriteLine("/// <exception cref=\"ArgumentNullException\"><paramref name=\"value\"/> is <c>null</c>.</exception>");
                    cb.WriteLine("/// <exception cref=\"ArgumentException\"><paramref name=\"comparisonType\"/> is not a");
                    cb.WriteLine("/// <see cref=\"StringComparison\"/> value.</exception>");
                    cb.WriteLine("/// <remarks>");
                    cb.WriteLine("/// Index numbering starts from zero. That is, the first character in the current instance");
                    cb.WriteLine("/// is at index zero and the last is at <see cref=\"Length\"/> - 1.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// This method begins searching at the last character position of this instance");
                    cb.WriteLine("/// and proceeds backward toward the beginning until either <paramref name=\"value\"/>");
                    cb.WriteLine("/// is found or the first character position has been examined.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// The <paramref name=\"comparisonType\"/> parameter specifies to search for the <paramref name=\"value\"/>");
                    cb.WriteLine("/// parameter using the current or invariant culture, using a case-sensitive or case-insensitive search,");
                    cb.WriteLine("/// and using word or ordinal comparison rules.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// To match the behavior of the JDK, this method allows searches for the empty string, which will");
                    cb.WriteLine("/// always return <see cref=\"Length\"/>.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// On older platforms than .NET Core, this overload provides optimizations for");
                    cb.WriteLine("/// <see cref=\"StringComparison.OrdinalIgnoreCase\"/> over and above the System.Memory package.");
                    cb.WriteLine("/// </remarks>");
                    cb.WriteLine("public readonly int LastIndexOf(string value, StringComparison comparisonType)");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("=> SpanUtilities.LastIndexOf(_chars.Slice(0, _pos), value, comparisonType);");
                    });
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the last occurrence of the specified string in the");
                    cb.WriteLine("/// this instance.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <param name=\"comparisonType\">One of the enumeration values that specifies the rules for the search.</param>");
                    cb.WriteLine("/// <returns>The index of the specified character, <c>-1</c> if the character isn't found.</returns>");
                    cb.WriteLine("/// <exception cref=\"ArgumentNullException\"><paramref name=\"value\"/> is <c>null</c>.</exception>");
                    cb.WriteLine("/// <exception cref=\"ArgumentException\"><paramref name=\"comparisonType\"/> is not a");
                    cb.WriteLine("/// <see cref=\"StringComparison\"/> value.</exception>");
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
                    cb.WriteLine("public readonly int LastIndexOf(string value, StringComparison comparisonType)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("if (value is null)");
                        cb.WriteLine("    throw new ArgumentNullException(nameof(value));");
                        cb.WriteLine();
                        cb.WriteLine("return System.MemoryExtensions.LastIndexOf(_chars.Slice(0, _pos), value.AsSpan(), comparisonType);");
                    });
                    cb.WriteLine("}");
                });
            }
            cb.WriteLine();


            if (useSpanUtilities)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the last occurrence of the specified string in the");
                    cb.WriteLine("/// this instance.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <param name=\"comparisonType\">One of the enumeration values that determines how the current instance");
                    cb.WriteLine("/// and <paramref name=\"value\"/> are compared.</param>");
                    cb.WriteLine("/// <returns>The zero-based starting index position of value if that string is found, or -1");
                    cb.WriteLine("/// if it is not found. If <paramref name=\"value\"/> is <see cref=\"string.Empty\"/>,");
                    cb.WriteLine("/// it returns <see cref=\"Length\"/>.</returns>");
                    cb.WriteLine("/// <exception cref=\"ArgumentException\"><paramref name=\"comparisonType\"/> is not a");
                    cb.WriteLine("/// <see cref=\"StringComparison\"/> value.</exception>");
                    cb.WriteLine("/// <remarks>");
                    cb.WriteLine("/// Index numbering starts from zero. That is, the first character in the current instance");
                    cb.WriteLine("/// is at index zero and the last is at <see cref=\"Length\"/> - 1.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// This method begins searching at the last character position of this instance");
                    cb.WriteLine("/// and proceeds backward toward the beginning until either <paramref name=\"value\"/>");
                    cb.WriteLine("/// is found or the first character position has been examined.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// The <paramref name=\"comparisonType\"/> parameter specifies to search for the <paramref name=\"value\"/>");
                    cb.WriteLine("/// parameter using the current or invariant culture, using a case-sensitive or case-insensitive search,");
                    cb.WriteLine("/// and using word or ordinal comparison rules.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// To match the behavior of the JDK, this method allows searches for the empty string, which will");
                    cb.WriteLine("/// always return <see cref=\"Length\"/>.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// On older platforms than .NET Core, this overload provides optimizations for");
                    cb.WriteLine("/// <see cref=\"StringComparison.OrdinalIgnoreCase\"/> over and above the System.Memory package.");
                    cb.WriteLine("/// </remarks>");
                    cb.WriteLine("public readonly int LastIndexOf(ReadOnlySpan<char> value, StringComparison comparisonType)");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("=> SpanUtilities.LastIndexOf(_chars.Slice(0, _pos), value, comparisonType);");
                    });
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the last occurrence of the specified string in the");
                    cb.WriteLine("/// this instance.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <param name=\"comparisonType\">One of the enumeration values that specifies the rules for the search.</param>");
                    cb.WriteLine("/// <returns>The index of the specified character, <c>-1</c> if the character isn't found.</returns>");
                    cb.WriteLine("/// <exception cref=\"ArgumentException\"><paramref name=\"comparisonType\"/> is not a");
                    cb.WriteLine("/// <see cref=\"StringComparison\"/> value.</exception>");
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
                    cb.WriteLine("public readonly int LastIndexOf(ReadOnlySpan<char> value, StringComparison comparisonType)");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("=> System.MemoryExtensions.LastIndexOf(_chars.Slice(0, _pos), value, comparisonType);");
                    });
                });
            }
            cb.WriteLine();



            if (useSpanUtilities)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the last occurrence of the specified string beginning");
                    cb.WriteLine("/// at the specified index in this instance.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <param name=\"startIndex\">The search starting position. The search proceeds from <paramref name=\"startIndex\"/> toward the");
                    cb.WriteLine("/// beginning of this instance.</param>");
                    cb.WriteLine("/// <param name=\"comparisonType\">One of the enumeration values that determines how the current instance");
                    cb.WriteLine("/// and <paramref name=\"value\"/> are compared.</param>");
                    cb.WriteLine("/// <returns>The zero-based starting index position of value if that string is found, or -1");
                    cb.WriteLine("/// if it is not found. If <paramref name=\"value\"/> is <see cref=\"string.Empty\"/>,");
                    cb.WriteLine("/// it returns <paramref name=\"startIndex\"/> if it is within the bounds of the the current instance; otherwise, if");
                    cb.WriteLine("/// <paramref name=\"startIndex\"/> is greater than <see cref=\"Length\"/>, it returns");
                    cb.WriteLine("/// the <see cref=\"Length\"/>.</returns>");
                    cb.WriteLine("/// <exception cref=\"ArgumentNullException\"><paramref name=\"value\"/> is <c>null</c>.</exception>");
                    cb.WriteLine("/// <exception cref=\"ArgumentException\"><paramref name=\"comparisonType\"/> is not a");
                    cb.WriteLine("/// <see cref=\"StringComparison\"/> value.</exception>");
                    cb.WriteLine("/// <remarks>");
                    cb.WriteLine("/// Index numbering starts from zero. That is, the first character in the current instance");
                    cb.WriteLine("/// is at index zero and the last is at <see cref=\"Length\"/> - 1.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// The search begins at the effective starting position and proceeds backward until either");
                    cb.WriteLine("/// <paramref name=\"value\"/> is found or the first character position has been examined.");
                    cb.WriteLine("/// The effective starting position is the lesser of <paramref name=\"startIndex\"/> and");
                    cb.WriteLine("/// <see cref=\"Length\"/> - <paramref name=\"value\"/>.Length. For example, if");
                    cb.WriteLine("/// <paramref name=\"startIndex\"/> is <see cref=\"Length\"/> - 1 and <paramref name=\"value\"/>");
                    cb.WriteLine("/// contains a single character, the method searches every character from the last");
                    cb.WriteLine("/// character in the string to the beginning.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// The <paramref name=\"comparisonType\"/> parameter specifies to search for the <paramref name=\"value\"/>");
                    cb.WriteLine("/// parameter using the current or invariant culture, using a case-sensitive or case-insensitive search,");
                    cb.WriteLine("/// and using word or ordinal comparison rules.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// To match the behavior of the JDK, this method allows searches for the empty string, which will");
                    cb.WriteLine("/// always return the lesser of <paramref name=\"startIndex\"/> or <see cref=\"Length\"/>.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// If <paramref name=\"startIndex\"/> is less than zero, the method returns -1. If it is greater than");
                    cb.WriteLine("/// <see cref=\"Length\"/>, it is treated as equal to <see cref=\"Length\"/>.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// On older platforms than .NET Core, this overload provides optimizations for");
                    cb.WriteLine("/// <see cref=\"StringComparison.OrdinalIgnoreCase\"/> over and above the System.Memory package.");
                    cb.WriteLine("/// </remarks>");
                    cb.WriteLine("public readonly int LastIndexOf(string value, int startIndex, StringComparison comparisonType)");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("=> SpanUtilities.LastIndexOf(_chars.Slice(0, _pos), value, startIndex, comparisonType);");
                    });
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the last occurrence of the specified string beginning");
                    cb.WriteLine("/// at the specified index in this instance.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <param name=\"startIndex\">The search starting position. The search proceeds from");
                    cb.WriteLine("/// <paramref name=\"startIndex\"/> toward the beginning of this instance.</param>");
                    cb.WriteLine("/// <param name=\"comparisonType\">One of the enumeration values that specifies the rules for the search.</param>");
                    cb.WriteLine("/// <returns>The index of the specified character, <c>-1</c> if the character isn't found.</returns>");
                    cb.WriteLine("/// <exception cref=\"ArgumentNullException\"><paramref name=\"value\"/> is <c>null</c>.</exception>");
                    cb.WriteLine("/// <exception cref=\"ArgumentException\"><paramref name=\"comparisonType\"/> is not a");
                    cb.WriteLine("/// <see cref=\"StringComparison\"/> value.</exception>");
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
                    cb.WriteLine("public readonly int LastIndexOf(string value, int startIndex, StringComparison comparisonType)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("if (value is null)");
                        cb.WriteLine("    throw new ArgumentNullException(nameof(value));");
                        cb.WriteLine();
                        cb.WriteLine("if ((uint)startIndex >= (uint)_pos)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("// For consistency with string.LastIndexOf:");
                            cb.WriteLine("// if startIndex >= length, use full length; if empty, just return -1.");
                            cb.WriteLine("if (_pos == 0)");
                            cb.WriteLine("    return -1;");
                            cb.WriteLine();
                            cb.WriteLine("startIndex = _pos - 1;");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine();
                        cb.WriteLine("// Equivalent of searching within the substring that ends at startIndex (inclusive)");
                        cb.WriteLine("ReadOnlySpan<char> searchSpan = _chars.Slice(0, startIndex + 1);");
                        cb.WriteLine("return System.MemoryExtensions.LastIndexOf(searchSpan, value.AsSpan(), comparisonType);");
                    });
                    cb.WriteLine("}");
                });
            }
            cb.WriteLine();

            if (useSpanUtilities)
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the last occurrence of the specified string beginning");
                    cb.WriteLine("/// at the specified index in this instance.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <param name=\"startIndex\">The search starting position. The search proceeds from <paramref name=\"startIndex\"/> toward the");
                    cb.WriteLine("/// beginning of this instance.</param>");
                    cb.WriteLine("/// <param name=\"comparisonType\">One of the enumeration values that determines how the current instance");
                    cb.WriteLine("/// and <paramref name=\"value\"/> are compared.</param>");
                    cb.WriteLine("/// <returns>The zero-based starting index position of value if that string is found, or -1");
                    cb.WriteLine("/// if it is not found. If <paramref name=\"value\"/> is <see cref=\"string.Empty\"/>,");
                    cb.WriteLine("/// it returns <paramref name=\"startIndex\"/> if it is within the bounds of the the current instance; otherwise, if");
                    cb.WriteLine("/// <paramref name=\"startIndex\"/> is greater than <see cref=\"Length\"/>, it returns");
                    cb.WriteLine("/// the <see cref=\"Length\"/>.</returns>");
                    cb.WriteLine("/// <exception cref=\"ArgumentException\"><paramref name=\"comparisonType\"/> is not a");
                    cb.WriteLine("/// <see cref=\"StringComparison\"/> value.</exception>");
                    cb.WriteLine("/// <remarks>");
                    cb.WriteLine("/// Index numbering starts from zero. That is, the first character in the current instance");
                    cb.WriteLine("/// is at index zero and the last is at <see cref=\"Length\"/> - 1.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// The search begins at the effective starting position and proceeds backward until either");
                    cb.WriteLine("/// <paramref name=\"value\"/> is found or the first character position has been examined.");
                    cb.WriteLine("/// The effective starting position is the lesser of <paramref name=\"startIndex\"/> and");
                    cb.WriteLine("/// <see cref=\"Length\"/> - <paramref name=\"value\"/>.Length. For example, if");
                    cb.WriteLine("/// <paramref name=\"startIndex\"/> is <see cref=\"Length\"/> - 1 and <paramref name=\"value\"/>");
                    cb.WriteLine("/// contains a single character, the method searches every character from the last");
                    cb.WriteLine("/// character in the string to the beginning.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// The <paramref name=\"comparisonType\"/> parameter specifies to search for the <paramref name=\"value\"/>");
                    cb.WriteLine("/// parameter using the current or invariant culture, using a case-sensitive or case-insensitive search,");
                    cb.WriteLine("/// and using word or ordinal comparison rules.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// To match the behavior of the JDK, this method allows searches for the empty string, which will");
                    cb.WriteLine("/// always return the lesser of <paramref name=\"startIndex\"/> or <see cref=\"Length\"/>.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// If <paramref name=\"startIndex\"/> is less than zero, the method returns -1. If it is greater than");
                    cb.WriteLine("/// <see cref=\"Length\"/>, it is treated as equal to <see cref=\"Length\"/>.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// On older platforms than .NET Core, this overload provides optimizations for");
                    cb.WriteLine("/// <see cref=\"StringComparison.OrdinalIgnoreCase\"/> over and above the System.Memory package.");
                    cb.WriteLine("/// </remarks>");
                    cb.WriteLine("public readonly int LastIndexOf(ReadOnlySpan<char> value, int startIndex, StringComparison comparisonType)");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("=> SpanUtilities.LastIndexOf(_chars.Slice(0, _pos), value, startIndex, comparisonType);");
                    });
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Reports the zero-based index of the last occurrence of the specified string beginning");
                    cb.WriteLine("/// at the specified index in this instance.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">The string to find.</param>");
                    cb.WriteLine("/// <param name=\"startIndex\">The search starting position. The search proceeds from");
                    cb.WriteLine("/// <paramref name=\"startIndex\"/> toward the beginning of this instance.</param>");
                    cb.WriteLine("/// <param name=\"comparisonType\">One of the enumeration values that specifies the rules for the search.</param>");
                    cb.WriteLine("/// <returns>The index of the specified character, <c>-1</c> if the character isn't found.</returns>");
                    cb.WriteLine("/// <exception cref=\"ArgumentException\"><paramref name=\"comparisonType\"/> is not a");
                    cb.WriteLine("/// <see cref=\"StringComparison\"/> value.</exception>");
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
                    cb.WriteLine("public readonly int LastIndexOf(ReadOnlySpan<char> value, int startIndex, StringComparison comparisonType)");
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
                            cb.WriteLine("    return -1;");
                            cb.WriteLine();
                            cb.WriteLine("startIndex = _pos - 1;");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine();
                        cb.WriteLine("// Equivalent of searching within the substring that ends at startIndex (inclusive)");
                        cb.WriteLine("ReadOnlySpan<char> searchSpan = _chars.Slice(0, startIndex + 1);");
                        cb.WriteLine("return System.MemoryExtensions.LastIndexOf(searchSpan, value, comparisonType);");
                    });
                    cb.WriteLine("}");
                });
            }
            cb.WriteLine();

            if (!useSpanUtilities)
            {
                cb.WriteLine("#endif");
                cb.WriteLine();
            }

            cb.IndentBlock(() =>
            {
                cb.WriteLine("#endregion LastIndexOf");
            });
            cb.WriteLine("}");

            return sb.ToString();
        }
    }
}
