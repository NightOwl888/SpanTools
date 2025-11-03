// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE.txt file or at https://opensource.org/licenses/MIT.

using J2N.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using Xunit;

#if FEATURE_NAMESPACE_MYNAMESPACE
using MyNamespace;
#elif FEATURE_NAMESPACE_LUCENENETTEXT
using Lucene.Net.Text;
#elif FEATURE_NAMESPACE_GLOBAL
// Global namespace
#endif

namespace SpanTools.Generator.Tests
{
    public partial class ValueStringBuilderTests
    {
        private const string SearchForString = "ሄኢቌጕኬ\u124fቖኋዘዻ";
        private const string FixtureString = "ዬ፡ዶጶቝአሄኢቌጕኬ\u124fቖኋዘዻ፡ሆገኅጬሷ\u135cቔቿ፺ዃጫቭዄ";

        public static IEnumerable<object[]> IndexOfCharTestData
        {
            get
            {
                yield return new object[] { "hello world", 'o', 4 };
                yield return new object[] { "hello world", 'z', -1 };
                yield return new object[] { "", 'a', -1 };
                yield return new object[] { "a", 'a', 0 };
                yield return new object[] { "ababa", 'a', 0 }; // first occurrence
            }
        }

        public static IEnumerable<object[]> IndexOfStringTestData
        {
            get
            {
                yield return new object[] { "hello world", "world", 6, StringComparison.Ordinal, (CultureInfo?)null! };
                yield return new object[] { "hello world", "WORLD", -1, StringComparison.Ordinal, (CultureInfo?)null! };
                yield return new object[] { "hello world", "WORLD", 6, StringComparison.OrdinalIgnoreCase, (CultureInfo?)null! };
                yield return new object[] { "aaaaa", "aa", 0, StringComparison.Ordinal, (CultureInfo?)null! }; // first occurrence
                yield return new object[] { "abc", "", 0, StringComparison.Ordinal, (CultureInfo?)null! };      // empty target
                yield return new object[] { "", "a", -1, StringComparison.Ordinal, (CultureInfo?)null! };

                string target;
                var culture = new CultureInfo("ru-MD");
                using (var context = new CultureContext(culture))
                {
                    target = FixtureString;
                    yield return new object[] { target, SearchForString, 6, StringComparison.Ordinal, culture };
                    yield return new object[] { target, SearchForString, target.IndexOf(SearchForString, StringComparison.OrdinalIgnoreCase), StringComparison.OrdinalIgnoreCase, culture };
                    yield return new object[] { target, SearchForString, target.IndexOf(SearchForString, StringComparison.CurrentCulture), StringComparison.CurrentCulture, culture };
                    yield return new object[] { target, SearchForString, target.IndexOf(SearchForString, StringComparison.CurrentCultureIgnoreCase), StringComparison.CurrentCultureIgnoreCase, culture };
                    yield return new object[] { target, SearchForString, target.IndexOf(SearchForString, StringComparison.InvariantCulture), StringComparison.InvariantCulture, culture };
                    yield return new object[] { target, SearchForString, target.IndexOf(SearchForString, StringComparison.InvariantCultureIgnoreCase), StringComparison.InvariantCultureIgnoreCase, culture };
                }
            }
        }

        public static IEnumerable<object[]> LastIndexOfCharTestData
        {
            get
            {
                yield return new object[] { "hello world", 'o', 7 };
                yield return new object[] { "hello world", 'z', -1 };
                yield return new object[] { "", 'a', -1 };
                yield return new object[] { "a", 'a', 0 };
                yield return new object[] { "ababa", 'a', 4 }; // last occurrence
            }
        }

        public static IEnumerable<object[]> LastIndexOfStringTestData
        {
            get
            {
                yield return new object[] { "hello world world", "world", 12, StringComparison.Ordinal, (CultureInfo?)null! }; // last occurrence
                yield return new object[] { "hello world", "WORLD", -1, StringComparison.Ordinal, (CultureInfo?)null! };
                yield return new object[] { "hello world", "WORLD", 6, StringComparison.OrdinalIgnoreCase, (CultureInfo?)null! };
                yield return new object[] { "aaaaa", "aa", 3, StringComparison.Ordinal, (CultureInfo?)null! }; // last occurrence
                yield return new object[] { "abc", "", 3, StringComparison.Ordinal, (CultureInfo?)null! };     // empty target matches at end
                yield return new object[] { "", "a", -1, StringComparison.Ordinal, (CultureInfo?)null! };

                string target;
                var culture = new CultureInfo("ru-MD");
                using (var context = new CultureContext(culture))
                {
                    target = FixtureString;
                    yield return new object[] { target, SearchForString, target.LastIndexOf(SearchForString, StringComparison.Ordinal), StringComparison.Ordinal, culture };
                    yield return new object[] { target, SearchForString, target.LastIndexOf(SearchForString, StringComparison.OrdinalIgnoreCase), StringComparison.OrdinalIgnoreCase, culture };
                    yield return new object[] { target, SearchForString, target.LastIndexOf(SearchForString, StringComparison.CurrentCulture), StringComparison.CurrentCulture, culture };
                    yield return new object[] { target, SearchForString, target.LastIndexOf(SearchForString, StringComparison.CurrentCultureIgnoreCase), StringComparison.CurrentCultureIgnoreCase, culture };
                    yield return new object[] { target, SearchForString, target.LastIndexOf(SearchForString, StringComparison.InvariantCulture), StringComparison.InvariantCulture, culture };
                    yield return new object[] { target, SearchForString, target.LastIndexOf(SearchForString, StringComparison.InvariantCultureIgnoreCase), StringComparison.InvariantCultureIgnoreCase, culture };
                }

            }
        }

        public static IEnumerable<object[]> LastIndexOfStringWithStartIndexTestData
        {
            get
            {
                // Case 1: Two "world" occurrences
                yield return new object[] { "hello world world", "world", 12, 16, StringComparison.Ordinal, (CultureInfo?)null! }; // startIndex after last -> full string search
                yield return new object[] { "hello world world", "world", 6, 10, StringComparison.Ordinal, (CultureInfo?)null! };  // startIndex before last "world"

                // Case 2: Case-insensitive
                yield return new object[] { "Hello World world", "WORLD", 12, 16, StringComparison.OrdinalIgnoreCase, (CultureInfo?)null! };
                yield return new object[] { "Hello World world", "WORLD", 6, 10, StringComparison.OrdinalIgnoreCase, (CultureInfo?)null! };

                // Case 3: Overlapping substrings
                yield return new object[] { "aaaaa", "aa", 3, 4, StringComparison.Ordinal, (CultureInfo?)null! }; // last full "aa" starts at 3
                yield return new object[] { "aaaaa", "aa", 1, 2, StringComparison.Ordinal, (CultureInfo?)null! }; // restrict search to before index 2

                // Case 4: Empty target matches at end
                yield return new object[] { "abc", "", 3, 3, StringComparison.Ordinal, (CultureInfo?)null! };
                yield return new object[] { "abc", "", 3, 2, StringComparison.Ordinal, (CultureInfo?)null! }; // This is a special case that would return 2 in Java. But since we are using .NET startIndex/length semantics, it is correct to expect 3.

                // Case 5: Target not found
                yield return new object[] { "abc", "d", -1, 2, StringComparison.Ordinal, (CultureInfo?)null! };
                yield return new object[] { "", "a", -1, 0, StringComparison.Ordinal, (CultureInfo?)null! };

                string target;
                var culture = new CultureInfo("ru-MD");
                using (var context = new CultureContext(culture))
                {
                    target = FixtureString;
                    int startIndex = 20;
                    yield return new object[] { target, SearchForString, target.LastIndexOf(SearchForString, startIndex, StringComparison.Ordinal), startIndex, StringComparison.Ordinal, culture };
                    yield return new object[] { target, SearchForString, target.LastIndexOf(SearchForString, startIndex, StringComparison.OrdinalIgnoreCase), startIndex, StringComparison.OrdinalIgnoreCase, culture };
                    yield return new object[] { target, SearchForString, target.LastIndexOf(SearchForString, startIndex, StringComparison.CurrentCulture), startIndex, StringComparison.CurrentCulture, culture };
                    yield return new object[] { target, SearchForString, target.LastIndexOf(SearchForString, startIndex, StringComparison.CurrentCultureIgnoreCase), startIndex, StringComparison.CurrentCultureIgnoreCase, culture };
                    yield return new object[] { target, SearchForString, target.LastIndexOf(SearchForString, startIndex, StringComparison.InvariantCulture), startIndex, StringComparison.InvariantCulture, culture };
                    yield return new object[] { target, SearchForString, target.LastIndexOf(SearchForString, startIndex, StringComparison.InvariantCultureIgnoreCase), startIndex, StringComparison.InvariantCultureIgnoreCase, culture };
                }
            }
        }

        [Theory]
        [MemberData(nameof(IndexOfCharTestData))]
        public void Test_IndexOf_Char(string source, char target, int expected)
        {
            using ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[source.Length]);
            sb.Append(source);

            int actual = sb.IndexOf(target);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(IndexOfStringTestData))]
        public void Test_IndexOf_ReadOnlySpan(string source, string target, int expected, StringComparison comparison, CultureInfo? culture)
        {
            //Assume.That(comparison, Is.EqualTo(StringComparison.Ordinal));
            if (comparison != StringComparison.Ordinal)
                return;

            using ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[source.Length]);
            sb.Append(source);

            using IDisposable context = culture is not null ? new CultureContext(culture) : new DummyDisposable();

            int actual = sb.IndexOf(target.AsSpan());
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(IndexOfStringTestData))]
        public void Test_IndexOf_String_StringComparison(string source, string target, int expected, StringComparison comparison, CultureInfo? culture)
        {
            using ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[source.Length]);
            sb.Append(source);

            using IDisposable context = culture is not null ? new CultureContext(culture) : new DummyDisposable();

            int actual = sb.IndexOf(target, comparison);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(IndexOfStringTestData))]
        public void Test_IndexOf_ReadOnlySpan_StringComparison(string source, string target, int expected, StringComparison comparison, CultureInfo? culture)
        {
            using ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[source.Length]);
            sb.Append(source);

            using IDisposable context = culture is not null ? new CultureContext(culture) : new DummyDisposable();

            int actual = sb.IndexOf(target.AsSpan(), comparison);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(IndexOfStringTestData))]
        public void Test_IndexOf_String_Int32_StringComparison(string source, string target, int expected, StringComparison comparison, CultureInfo? culture)
        {
            int startIndex = 16;
            using ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[startIndex + source.Length]);
            sb.Append(' ', startIndex);
            sb.Append(source);

            using IDisposable context = culture is not null ? new CultureContext(culture) : new DummyDisposable();

            int actual = sb.IndexOf(target, startIndex, comparison);
            Assert.Equal(expected + startIndex, actual);
        }

        [Theory]
        [MemberData(nameof(IndexOfStringTestData))]
        public void Test_IndexOf_ReadOnlySpan_Int32_StringComparison(string source, string target, int expected, StringComparison comparison, CultureInfo? culture)
        {
            int startIndex = 16;
            using ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[startIndex + source.Length]);
            sb.Append(' ', startIndex);
            sb.Append(source);

            using IDisposable context = culture is not null ? new CultureContext(culture) : new DummyDisposable();

            int actual = sb.IndexOf(target.AsSpan(), startIndex, comparison);
            Assert.Equal(expected + startIndex, actual);
        }

        [Theory]
        [MemberData(nameof(LastIndexOfCharTestData))]
        public void Test_LastIndexOf_Char(string source, char target, int expected)
        {
            using ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[source.Length]);
            sb.Append(source);

            int actual = sb.LastIndexOf(target);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(LastIndexOfStringTestData))]
        public void Test_LastIndexOf_ReadOnlySpan(string source, string target, int expected, StringComparison comparison, CultureInfo? culture)
        {
            //Assume.That(comparison, Is.EqualTo(StringComparison.Ordinal));
            if (comparison != StringComparison.Ordinal)
                return;

            using ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[source.Length]);
            sb.Append(source);

            using IDisposable context = culture is not null ? new CultureContext(culture) : new DummyDisposable();

            int actual = sb.LastIndexOf(target.AsSpan());
            Assert.Equal(expected, actual);
        }

#if NET5_0_OR_GREATER

        [Theory]
        [MemberData(nameof(LastIndexOfStringTestData))]
        public void Test_LastIndexOf_String_StringComparison(string source, string target, int expected, StringComparison comparison, CultureInfo? culture)
        {
            using ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[source.Length]);
            sb.Append(source);

            using IDisposable context = culture is not null ? new CultureContext(culture) : new DummyDisposable();

            int actual = sb.LastIndexOf(target, comparison);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(LastIndexOfStringTestData))]
        public void Test_LastIndexOf_ReadOnlySpan_StringComparison(string source, string target, int expected, StringComparison comparison, CultureInfo? culture)
        {
            using ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[source.Length]);
            sb.Append(source);

            using IDisposable context = culture is not null ? new CultureContext(culture) : new DummyDisposable();

            int actual = sb.LastIndexOf(target.AsSpan(), comparison);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(LastIndexOfStringWithStartIndexTestData))]
        public void Test_LastIndexOf_String_Int32_StringComparison(string source, string target, int expected, int startIndex, StringComparison comparison, CultureInfo? culture)
        {
            using ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[source.Length]);
            sb.Append(source);

            using IDisposable context = culture is not null ? new CultureContext(culture) : new DummyDisposable();

            int actual = sb.LastIndexOf(target, startIndex, comparison);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(LastIndexOfStringWithStartIndexTestData))]
        public void Test_LastIndexOf_ReadOnlySpan_Int32_StringComparison(string source, string target, int expected, int startIndex, StringComparison comparison, CultureInfo? culture)
        {
            using ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[source.Length]);
            sb.Append(source);

            using IDisposable context = culture is not null ? new CultureContext(culture) : new DummyDisposable();

            int actual = sb.LastIndexOf(target.AsSpan(), startIndex, comparison);
            Assert.Equal(expected, actual);
        }

#endif

        private class DummyDisposable : IDisposable
        {
            void IDisposable.Dispose()
            {
                // Intentionally empty
            }
        }
    }
}
