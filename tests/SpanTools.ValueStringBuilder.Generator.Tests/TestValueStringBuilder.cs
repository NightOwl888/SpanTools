// Source: https://github.com/dotnet/runtime/blob/v10.0.0-rc.1.25451.107/src/libraries/Common/tests/Tests/System/Text/ValueStringBuilderTests.cs
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MyNamespace;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Xunit;

namespace SpanTools.Generator.Tests
{
    public partial class ValueStringBuilderTests
    {
        [Fact]
        public void Ctor_Default_CanAppend()
        {
            using var vsb = default(ValueStringBuilder);
            Assert.Equal(0, vsb.Length);

            vsb.Append('a');
            Assert.Equal(1, vsb.Length);
            Assert.Equal("a", vsb.ToString());
        }

        [Fact]
        public void Ctor_Span_CanAppend()
        {
            using var vsb = new ValueStringBuilder(new char[1]);
            Assert.Equal(0, vsb.Length);

            vsb.Append('a');
            Assert.Equal(1, vsb.Length);
            Assert.Equal("a", vsb.ToString());
        }

        [Fact]
        public void Ctor_InitialCapacity_CanAppend()
        {
            using var vsb = new ValueStringBuilder(1);
            Assert.Equal(0, vsb.Length);

            vsb.Append('a');
            Assert.Equal(1, vsb.Length);
            Assert.Equal("a", vsb.ToString());
        }

        [Fact]
        public void Append_Char_CapacityExceeded_TracksMaxLength()
        {
            Span<char> output = stackalloc char[5];

            var sb = new ValueStringBuilder(output);
            try
            {
                Assert.Equal(5, sb.Capacity);
                for (int i = 0; i < 9; i++)
                    sb.Append((char)i);

                Assert.Equal(9, sb.Length);
                Assert.Equal(9, sb.MaxLength);

                sb.Length = 0;
                Assert.Equal(0, sb.Length);
                Assert.Equal(9, sb.MaxLength);

                sb.Length = 5;
                Assert.Equal(5, sb.Length);
                Assert.Equal(9, sb.MaxLength);
            }
            finally
            {
                sb.Dispose();
            }
        }

        [Fact]
        public void Append_Char_MatchesStringBuilder()
        {
            var sb = new StringBuilder();
            using var vsb = new ValueStringBuilder();
            for (int i = 1; i <= 100; i++)
            {
                sb.Append((char)i);
                vsb.Append((char)i);
            }

            Assert.Equal(sb.Length, vsb.Length);
            Assert.Equal(sb.ToString(), vsb.ToString());
        }

        [Fact]
        public void Append_String_MatchesStringBuilder()
        {
            var sb = new StringBuilder();
            using var vsb = new ValueStringBuilder();
            for (int i = 1; i <= 100; i++)
            {
                string s = i.ToString();
                sb.Append(s);
                vsb.Append(s);
            }

            Assert.Equal(sb.Length, vsb.Length);
            Assert.Equal(sb.ToString(), vsb.ToString());
        }

        [Theory]
        [InlineData(0, 4 * 1024 * 1024)]
        [InlineData(1025, 4 * 1024 * 1024)]
        [InlineData(3 * 1024 * 1024, 6 * 1024 * 1024)]
        public void Append_String_Large_MatchesStringBuilder(int initialLength, int stringLength)
        {
            var sb = new StringBuilder(initialLength);
            using var vsb = new ValueStringBuilder(new char[initialLength]);

            string s = new string('a', stringLength);
            sb.Append(s);
            vsb.Append(s);

            Assert.Equal(sb.Length, vsb.Length);
            Assert.Equal(sb.ToString(), vsb.ToString());
        }

        [Fact]
        public void Append_String_BeyondCapacity_SetsCapacityExceeded()
        {
            using var sb = new ValueStringBuilder(stackalloc char[5]);
            Assert.False(sb.CapacityExceeded);

            sb.Append("012345678");
            Assert.True(sb.CapacityExceeded);
        }

        [Fact]
        public void Append_String_BeyondCapacity_TracksMaxLength()
        {
            var sb = new ValueStringBuilder(stackalloc char[5]);
            try
            {
                Assert.Equal(0, sb.Length);
                Assert.Equal(0, sb.MaxLength);

                sb.Append("012345678");
                Assert.Equal(9, sb.Length);
                Assert.Equal(9, sb.MaxLength);

                sb.Length = 4;
                Assert.Equal(4, sb.Length);
                Assert.Equal(9, sb.MaxLength);

                sb.Append("012345678");
                Assert.Equal(13, sb.Length);
                Assert.Equal(13, sb.MaxLength);

                sb.Length = 4;
                Assert.Equal(4, sb.Length);
                Assert.Equal(13, sb.MaxLength);
            }
            finally
            {
                sb.Dispose();
            }
        }

        [Fact]
        public void Append_CharInt_MatchesStringBuilder()
        {
            var sb = new StringBuilder();
            using var vsb = new ValueStringBuilder();
            for (int i = 1; i <= 100; i++)
            {
                sb.Append((char)i, i);
                vsb.Append((char)i, i);
            }

            Assert.Equal(sb.Length, vsb.Length);
            Assert.Equal(sb.ToString(), vsb.ToString());
        }

        [Fact]
        public void Append_CharInt_CapacityExeeded_SetsMaxLength()
        {
            var sb = new ValueStringBuilder(stackalloc char[5]);
            try
            {
                for (int i = 1; i <= 100; i++)
                {
                    sb.Append((char)i, i);
                }

                Assert.Equal(5050, sb.Length);
                Assert.Equal(5050, sb.MaxLength);

                sb.Length = 0;
                Assert.Equal(0, sb.Length);
                Assert.Equal(5050, sb.MaxLength);

                sb.Length = 5;
                Assert.Equal(5, sb.Length);
                Assert.Equal(5050, sb.MaxLength);
            }
            finally
            {
                sb.Dispose();
            }
        }

#if FEATURE_COMPILE_UNSAFE

        [Fact]
        public unsafe void Append_PtrInt_MatchesStringBuilder()
        {
            var sb = new StringBuilder();
            using var vsb = new ValueStringBuilder();
            for (int i = 1; i <= 100; i++)
            {
                string s = i.ToString();
                fixed (char* p = s)
                {
                    sb.Append(p, s.Length);
                    vsb.Append(p, s.Length);
                }
            }

            Assert.Equal(sb.Length, vsb.Length);
            Assert.Equal(sb.ToString(), vsb.ToString());
        }

#endif

        [Fact]
        public void AppendSpan_DataAppendedCorrectly()
        {
            var sb = new StringBuilder();
            using var vsb = new ValueStringBuilder();

            for (int i = 1; i <= 1000; i++)
            {
                string s = i.ToString();

                sb.Append(s);

                Span<char> span = vsb.AppendSpan(s.Length);
                Assert.Equal(sb.Length, vsb.Length);

                s.AsSpan().CopyTo(span);
            }

            Assert.Equal(sb.Length, vsb.Length);
            Assert.Equal(sb.ToString(), vsb.ToString());
        }

        [Fact]
        public void AppendSpan_CapacityExceeded_TracksMaxLength()
        {
            var sb = new ValueStringBuilder(stackalloc char[128]);
            try
            {
                for (int i = 1; i <= 1000; i++)
                {
                    string s = i.ToString();
                    Span<char> span = sb.AppendSpan(s.Length);
                    s.AsSpan().CopyTo(span);
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
        public void Insert_IntCharInt_MatchesStringBuilder()
        {
            var sb = new StringBuilder();
            using var vsb = new ValueStringBuilder();
            var rand = new Random(42);

            for (int i = 1; i <= 100; i++)
            {
                int index = rand.Next(sb.Length);
                sb.Insert(index, new string((char)i, 1), i);
                vsb.Insert(index, (char)i, i);
            }

            Assert.Equal(sb.Length, vsb.Length);
            Assert.Equal(sb.ToString(), vsb.ToString());
        }

        [Fact]
        public void Insert_IntCharInt_TracksMaxLength()
        {
            var sb = new ValueStringBuilder();
            try
            {
                var rand = new Random(42);

                for (int i = 1; i <= 100; i++)
                {
                    int index = rand.Next(sb.Length);
                    sb.Insert(index, (char)i, i);
                }

                Assert.Equal(5050, sb.Length);
                Assert.Equal(5050, sb.MaxLength);

                sb.Length = 0;
                Assert.Equal(0, sb.Length);
                Assert.Equal(5050, sb.MaxLength);

                sb.Length = 5;
                Assert.Equal(5, sb.Length);
                Assert.Equal(5050, sb.MaxLength);
            }
            finally
            {
                sb.Dispose();
            }
        }

        [Fact]
        public void AsSpan_ReturnsCorrectValue_DoesntClearBuilder()
        {
            var sb = new StringBuilder();
            using var vsb = new ValueStringBuilder();

            for (int i = 1; i <= 100; i++)
            {
                string s = i.ToString();
                sb.Append(s);
                vsb.Append(s);
            }

            var resultString = new string(vsb.AsSpan());
            Assert.Equal(sb.ToString(), resultString);

            Assert.NotEqual(0, sb.Length);
            Assert.Equal(sb.Length, vsb.Length);
            Assert.Equal(sb.ToString(), vsb.ToString());
        }

        // LUCENENET: Removed Dispose() call so we can safely call ToString() multiple times
        [Fact]
        public void ToString_AllowsReuse()
        {
            const string Text1 = "test";
            using var vsb = new ValueStringBuilder();

            vsb.Append(Text1);
            Assert.Equal(Text1.Length, vsb.Length);

            string s = vsb.ToString();
            Assert.Equal(Text1, s);

            Assert.Equal(Text1.Length, vsb.Length);
            Assert.Equal(Text1, vsb.ToString());
            //Assert.True(vsb.TryCopyTo(Span<char>.Empty, out _));

            const string Text2 = "another test";
            vsb.Append(Text2);
            Assert.Equal(Text1.Length + Text2.Length, vsb.Length);
            Assert.Equal(Text1 + Text2, vsb.ToString());
        }

        [Fact]
        public void TryCopyTo_FailsWhenDestinationIsTooSmall_SucceedsWhenItsLargeEnough()
        {
            using var vsb = new ValueStringBuilder();

            const string Text = "expected text";
            vsb.Append(Text);
            Assert.Equal(Text.Length, vsb.Length);

            Span<char> dst = new char[Text.Length - 1];
            Assert.False(vsb.TryCopyTo(dst, out int charsWritten));
            Assert.Equal(0, charsWritten);
            Assert.Equal(Text.Length, vsb.Length);
        }

        [Fact]
        public void TryCopyTo_AllowsReuse()
        {
            const string Text1 = "test";
            using var vsb = new ValueStringBuilder();

            vsb.Append(Text1);
            Assert.Equal(Text1.Length, vsb.Length);

            Span<char> dst = new char[Text1.Length];
            Assert.True(vsb.TryCopyTo(dst, out int charsWritten));
            Assert.Equal(Text1.Length, charsWritten);
            Assert.Equal(Text1, new string(dst));

            Assert.Equal(Text1.Length, vsb.Length);
            Assert.Equal(Text1, vsb.ToString());
            //Assert.True(vsb.TryCopyTo(Span<char>.Empty, out _));

            const string Text2 = "another test";
            vsb.Append(Text2);
            Assert.Equal(Text1.Length + Text2.Length, vsb.Length);
            Assert.Equal(Text1 + Text2, vsb.ToString());
        }

        [Fact]
        public void Dispose_ClearsBuilder_ThenReusable()
        {
            const string Text1 = "test";
            using var vsb = new ValueStringBuilder();

            vsb.Append(Text1);
            Assert.Equal(Text1.Length, vsb.Length);

            vsb.Dispose();

            Assert.Equal(0, vsb.Length);
            Assert.Equal(string.Empty, vsb.ToString());
            Assert.True(vsb.TryCopyTo(Span<char>.Empty, out _));

            const string Text2 = "another test";
            vsb.Append(Text2);
            Assert.Equal(Text2.Length, vsb.Length);
            Assert.Equal(Text2, vsb.ToString());
        }

        [Fact]
        public void Indexer()
        {
            const string Text1 = "foobar";
            using var vsb = new ValueStringBuilder();

            vsb.Append(Text1);

            Assert.Equal('b', vsb[3]);
            vsb[3] = 'c';
            Assert.Equal('c', vsb[3]);
        }

        [Fact]
        public void EnsureCapacity_IfRequestedCapacityWins()
        {
            // Note: constants used here may be dependent on minimal buffer size
            // the ArrayPool is able to return.
            using var builder = new ValueStringBuilder(stackalloc char[32]);

            builder.EnsureCapacity(65, out int newCapacity);

            // LUCENENET: We over-allocate compared with the BCL
            Assert.Equal(65, newCapacity);
            Assert.True(builder.Capacity >= 128, $"Expected capacity >= 128 but was {builder.Capacity}.");
            Assert.True(builder.CapacityExceeded);
        }

        [Fact]
        public void EnsureCapacity_IfBufferTimesTwoWins()
        {
            using var builder = new ValueStringBuilder(stackalloc char[32]);

            builder.EnsureCapacity(33, out int newCapacity);

            // LUCENENET: We over-allocate compared with the BCL
            Assert.Equal(64, newCapacity);
            Assert.True(builder.Capacity >= 64, $"Expected capacity >= 64 but was {builder.Capacity}.");
            Assert.True(builder.CapacityExceeded);
        }

        [Fact]
        public void EnsureCapacity_NoAllocIfNotNeeded()
        {
            // Note: constants used here may be dependent on minimal buffer size
            // the ArrayPool is able to return.
            using var builder = new ValueStringBuilder(stackalloc char[64]);

            builder.EnsureCapacity(16);

            Assert.Equal(64, builder.Capacity);
        }

        [Fact]
        public void Length_BeyondCapacity_TracksMaxLength()
        {
            var sb = new ValueStringBuilder(stackalloc char[5]);
            try
            {
                Assert.Equal(0, sb.Length);
                Assert.Equal(0, sb.MaxLength);

                sb.Append("012345678");
                Assert.Equal(9, sb.Length);
                Assert.Equal(9, sb.MaxLength);

                sb.Length = 4;
                Assert.Equal(4, sb.Length);
                Assert.Equal(9, sb.MaxLength);

                sb.Append("012345678");
                Assert.Equal(13, sb.Length);
                Assert.Equal(13, sb.MaxLength);

                sb.Length = 4;
                Assert.Equal(4, sb.Length);
                Assert.Equal(13, sb.MaxLength);
            }
            finally
            {
                sb.Dispose();
            }
        }

        [Theory]
        [InlineData("Hello", 0, '\0', "\0Hello")]
        [InlineData("Hello", 3, 'a', "Helalo")]
        [InlineData("Hello", 5, 'b', "Hellob")]
        public static void Insert_Char(string original, int index, char value, string expected)
        {
            using var builder = new ValueStringBuilder(stackalloc char[original.Length]);
            builder.Append(original);
            builder.Insert(index, value);
            Assert.Equal(expected, builder.ToString());
        }

        public static IEnumerable Insert_String_Count_TestData
        {
            get
            {
                yield return new object[] { "Hello", 0, "\0", 0, "Hello" };
                yield return new object[] { "Hello", 0, "\0", 1, "\0Hello" };
                yield return new object[] { "Hello", 3, "abc", 1, "Helabclo" };
                yield return new object[] { "Hello", 5, "def", 1, "Hellodef" };
                yield return new object[] { "Hello", 0, "", 1, "Hello" };
                yield return new object[] { "Hello", 0, null!, 1, "Hello" };
                yield return new object[] { "Hello", 3, "abc", 2, "Helabcabclo" };
                yield return new object[] { "Hello", 5, "def", 2, "Hellodefdef" };
            }
        }

        [Theory]
        [MemberData(nameof(Insert_String_Count_TestData))]
        public void Insert_String_Count(string original, int index, string value, int count, string expected)
        {
            if (count == 1)
            {
                // Use Insert(int, string)
                using var vsb = new ValueStringBuilder(stackalloc char[original.Length]);
                vsb.Append(original);
                vsb.Insert(index, value);
                Assert.Equal(expected, vsb.ToString());
            }
            // Use Insert(int, string, int)
            using var builder = new ValueStringBuilder(stackalloc char[original.Length]);
            builder.Append(original);
            builder.Insert(index, value, count);
            Assert.Equal(expected, builder.ToString());
        }

        [Theory]
        [MemberData(nameof(Insert_String_Count_TestData))]
        public void Insert_ReadOnlySpan_Count(string original, int index, string value, int count, string expected)
        {
            if (count == 1)
            {
                // Use Insert(int, string)
                using var vsb = new ValueStringBuilder(stackalloc char[original.Length]);
                vsb.Append(original);
                vsb.Insert(index, value.AsSpan());
                Assert.Equal(expected, vsb.ToString());
            }
            // Use Insert(int, string, int)
            using var builder = new ValueStringBuilder(stackalloc char[original.Length]);
            builder.Append(original);
            builder.Insert(index, value.AsSpan(), count);
            Assert.Equal(expected, builder.ToString());
        }


        #region AsSpan


        [Fact]
        public static void OpenStringBuilderAsSpanNullary()
        {
            using var s = new ValueStringBuilder(stackalloc char[16]);
            s.Append("Hello");
            ReadOnlySpan<char> span = s.AsSpan();
            char[] expected = s.RawChars.Slice(0, s.Length).ToArray();
            span.Validate(expected);
        }

        [Fact]
        public static void StringAsSpanEmptyString()
        {
            using var s = new ValueStringBuilder(stackalloc char[16]);
            ReadOnlySpan<char> span = s.AsSpan();
            span.ValidateNonNullEmpty();
        }

        [Theory]
        [MemberData(nameof(TestHelpers.StringSliceTestData), MemberType = typeof(TestHelpers))]
        public static void AsSpan_StartAndLength(string textStr, int start, int length)
        {
            ValueStringBuilder text = new ValueStringBuilder(stackalloc char[16]);
            text.Append(textStr);
            try
            {
                if (start == -1)
                {
                    Validate(ref text, 0, text.Length, text.AsSpan());
                    Validate(ref text, 0, text.Length, text.AsSpan(0));
#if FEATURE_INDEX_RANGE
                    Validate(ref text, 0, text.Length, text.AsSpan(0..^0));
#endif
                }
                else if (length == -1)
                {
                    Validate(ref text, start, text.Length - start, text.AsSpan(start));
#if FEATURE_INDEX_RANGE
                    Validate(ref text, start, text.Length - start, text.AsSpan(start..));
#endif
                }
                else
                {
                    Validate(ref text, start, length, text.AsSpan(start, length));
#if FEATURE_INDEX_RANGE
                    Validate(ref text, start, length, text.AsSpan(start..(start + length)));
#endif
                }
            }
            finally
            {
                text.Dispose();
            }


            static unsafe void Validate(ref ValueStringBuilder text, int start, int length, ReadOnlySpan<char> span)
            {
                Assert.Equal(length, span.Length);
                fixed (char* pText = &MemoryMarshal.GetReference(text.RawChars))
                {
                    // Unsafe.AsPointer is safe here since it's pinned (since text and span should be the same string)
                    char* expected = pText + start;
                    void* actual = Unsafe.AsPointer(ref MemoryMarshal.GetReference(span));
                    Assert.Equal((IntPtr)expected, (IntPtr)actual);
                }
            }
        }

        #endregion AsSpan

        #region AsMemory

        [Theory]
        [InlineData(0, 0)]
        [InlineData(3, 0)]
        [InlineData(3, 1)]
        [InlineData(3, 2)]
        [InlineData(3, 3)]
        [InlineData(10, 0)]
        [InlineData(10, 3)]
        [InlineData(10, 10)]
        public static void OpenStringBuilderAsMemoryWithStart(int length, int start)
        {
            using ValueStringBuilder a = new ValueStringBuilder(length);
            a.Append('\0', length);
            ReadOnlyMemory<char> m = a.AsMemory(start);
            Assert.Equal(length - start, m.Length);
            if (start != length)
            {
                a[start] = (char)42;
                Assert.Equal(42, m.Span[0]);
            }
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(3, 0, 3)]
        [InlineData(3, 1, 2)]
        [InlineData(3, 2, 1)]
        [InlineData(3, 3, 0)]
        [InlineData(10, 0, 5)]
        [InlineData(10, 3, 2)]
        public static void OpenStringBuilderAsMemoryWithStartAndLength(int length, int start, int subLength)
        {
            using ValueStringBuilder a = new ValueStringBuilder(length);
            a.Append('\0', length);

            ReadOnlyMemory<char> m = a.AsMemory(start, subLength);
            Assert.Equal(subLength, m.Length);
            if (subLength != 0)
            {
                a[start] = (char)42;
                Assert.Equal(42, m.Span[0]);
            }
        }

        #endregion AsMemory

    }
}
