// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE.txt file or at https://opensource.org/licenses/MIT.

using MyNamespace;
using System;
using System.Globalization;
using Xunit;

namespace SpanTools.Generator.Tests
{
    public partial class ValueStringBuilderTests
    {
        public static readonly object[] LowerCaseTestData =
{
            // ASCII
            new object[] { "HELLO", "hello", CultureInfo.InvariantCulture },
            new object[] { "WORLD", "world", new CultureInfo("en-US") },

            // Turkish (special dotless i behavior)
            new object[] { "I", "ı", new CultureInfo("tr-TR") },
            new object[] { "İ", "i", new CultureInfo("tr-TR") }, // with combining dot

            // German (ß)
            new object[] { "SS", "ss", new CultureInfo("de-DE") },
            new object[] { "STRAẞE", "straße", new CultureInfo("de-DE") },
            new object[] { "FUßBALL", "fußball", new CultureInfo("de-DE") },

            // Greek final sigma
            new object[] { "Σ", "σ", new CultureInfo("el-GR") },
        };

        public static readonly object[] UpperCaseTestData =
        {
            // ASCII
            new object[] { "hello", "HELLO", CultureInfo.InvariantCulture },
            new object[] { "world", "WORLD", new CultureInfo("en-US") },

            // Turkish (special dotted i)
            new object[] { "i", "İ", new CultureInfo("tr-TR") },
            new object[] { "ı", "I", new CultureInfo("tr-TR") },

            // German sharp S expands (length change)
            new object[] { "longinputstringthatdoesnotfit", "LONGINPUTSTRINGTHATDOESNOTFIT", new CultureInfo("de-DE") },

            // Greek
            new object[] { "ὀδυσσεύς", "ὈΔΥΣΣΕΎΣ", new CultureInfo("el-GR") },
        };

        public static readonly object[] InvariantLowerCaseTestData =
        {
            new object[] { "HELLO", "hello" },
            new object[] { "Straße", "straße" }, // stays same length (ß not expanded in lower)
            new object[] { "İ", "İ" }, // combining dot preserved
        };

        public static readonly object[] InvariantUpperCaseTestData =
        {
            new object[] { "hello", "HELLO" },
            new object[] { "fußball", "FUßBALL" },
            new object[] { "i", "I" },
        };

        // ------------------------------
        // AppendLower()
        // ------------------------------

        [Theory]
        [MemberData(nameof(LowerCaseTestData))]
        public void Test_AppendLower_Span(string input, string expected, CultureInfo culture)
        {
            using var sb = new ValueStringBuilder(stackalloc char[16]);
            sb.AppendLower(input.AsSpan(), culture);
            Assert.Equal(expected, sb.ToString());
        }

        [Theory]
        [MemberData(nameof(LowerCaseTestData))]
        public void Test_AppendLower_String(string input, string expected, CultureInfo culture)
        {
            using var sb = new ValueStringBuilder(stackalloc char[16]);
            sb.AppendLower(input, culture);
            Assert.Equal(expected, sb.ToString());
        }

        // ------------------------------
        // AppendUpper()
        // ------------------------------

        [Theory]
        [MemberData(nameof(UpperCaseTestData))]
        public void Test_AppendUpper_Span(string input, string expected, CultureInfo culture)
        {
            using var sb = new ValueStringBuilder(stackalloc char[16]);
            sb.AppendUpper(input.AsSpan(), culture);
            Assert.Equal(expected, sb.ToString());
        }

        [Theory]
        [MemberData(nameof(UpperCaseTestData))]
        public void Test_AppendUpper_String(string input, string expected, CultureInfo culture)
        {
            using var sb = new ValueStringBuilder(stackalloc char[16]);
            sb.AppendUpper(input, culture);
            Assert.Equal(expected, sb.ToString());
        }

        // ------------------------------
        // AppendLowerInvariant()
        // ------------------------------

        [Theory]
        [MemberData(nameof(InvariantLowerCaseTestData))]
        public void Test_AppendLowerInvariant_Span(string input, string expected)
        {
            using var sb = new ValueStringBuilder(stackalloc char[16]);
            sb.AppendLowerInvariant(input.AsSpan());
            Assert.Equal(expected, sb.ToString());
        }

        [Theory]
        [MemberData(nameof(InvariantLowerCaseTestData))]
        public void Test_AppendLowerInvariant_String(string input, string expected)
        {
            using var sb = new ValueStringBuilder(stackalloc char[16]);
            sb.AppendLowerInvariant(input);
            Assert.Equal(expected, sb.ToString());
        }

        // ------------------------------
        // AppendUpperInvariant()
        // ------------------------------

        [Theory]
        [MemberData(nameof(InvariantUpperCaseTestData))]
        public void Test_AppendUpperInvariant_Span(string input, string expected)
        {
            using var sb = new ValueStringBuilder(stackalloc char[16]);
            sb.AppendUpperInvariant(input.AsSpan());
            Assert.Equal(expected, sb.ToString());
        }

        [Theory]
        [MemberData(nameof(InvariantUpperCaseTestData))]
        public void Test_AppendUpperInvariant_String(string input, string expected)
        {
            using var sb = new ValueStringBuilder(stackalloc char[16]);
            sb.AppendUpperInvariant(input);
            Assert.Equal(expected, sb.ToString());
        }

        [Fact]
        public void AppendUpper_CapacityExceeded_TracksMaxLength()
        {
            var sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                for (int i = 1; i <= 1000; i++)
                {
                    string s = i.ToString(CultureInfo.InvariantCulture);
                    sb.AppendUpper(s, CultureInfo.InvariantCulture);
                }

                Assert.Equal(2893, sb.Length);
                Assert.Equal(2893, sb.MaxLength);

                sb.Length = 0;
                Assert.Equal(0, sb.Length);
                Assert.Equal(2893, sb.MaxLength);

                sb.Length = 5;
                Assert.Equal(5, sb.Length);
                Assert.Equal(2893, sb.MaxLength);
            }
            finally
            {
                sb.Dispose();
            }
        }

        [Fact]
        public void AppendUpperInvariant_CapacityExceeded_TracksMaxLength()
        {
            var sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                for (int i = 1; i <= 1000; i++)
                {
                    string s = i.ToString(CultureInfo.InvariantCulture);
                    sb.AppendUpperInvariant(s);
                }

                Assert.Equal(2893, sb.Length);
                Assert.Equal(2893, sb.MaxLength);

                sb.Length = 0;
                Assert.Equal(0, sb.Length);
                Assert.Equal(2893, sb.MaxLength);

                sb.Length = 5;
                Assert.Equal(5, sb.Length);
                Assert.Equal(2893, sb.MaxLength);
            }
            finally
            {
                sb.Dispose();
            }
        }

        [Fact]
        public void AppendLower_CapacityExceeded_TracksMaxLength()
        {
            var sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                for (int i = 1; i <= 1000; i++)
                {
                    string s = i.ToString(CultureInfo.InvariantCulture);
                    sb.AppendLower(s, CultureInfo.InvariantCulture);
                }

                Assert.Equal(2893, sb.Length);
                Assert.Equal(2893, sb.MaxLength);

                sb.Length = 0;
                Assert.Equal(0, sb.Length);
                Assert.Equal(2893, sb.MaxLength);

                sb.Length = 5;
                Assert.Equal(5, sb.Length);
                Assert.Equal(2893, sb.MaxLength);
            }
            finally
            {
                sb.Dispose();
            }
        }

        [Fact]
        public void AppendLowerInvariant_CapacityExceeded_TracksMaxLength()
        {
            var sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                for (int i = 1; i <= 1000; i++)
                {
                    string s = i.ToString(CultureInfo.InvariantCulture);
                    sb.AppendLowerInvariant(s);
                }

                Assert.Equal(2893, sb.Length);
                Assert.Equal(2893, sb.MaxLength);

                sb.Length = 0;
                Assert.Equal(0, sb.Length);
                Assert.Equal(2893, sb.MaxLength);

                sb.Length = 5;
                Assert.Equal(5, sb.Length);
                Assert.Equal(2893, sb.MaxLength);
            }
            finally
            {
                sb.Dispose();
            }
        }
    }
}
