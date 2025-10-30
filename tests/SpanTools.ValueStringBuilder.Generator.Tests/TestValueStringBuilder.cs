// Source: https://github.com/dotnet/runtime/blob/v10.0.0-rc.1.25451.107/src/libraries/Common/tests/Tests/System/Text/ValueStringBuilderTests.cs
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MyNamespace;
using NUnit.Framework;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
#nullable enable

namespace SpanTools.Generator.Tests
{
    public partial class ValueStringBuilderTests
    {
        [Test]
        public void Ctor_Default_CanAppend()
        {
            using var vsb = default(ValueStringBuilder);
            Assert.AreEqual(0, vsb.Length);

            vsb.Append('a');
            Assert.AreEqual(1, vsb.Length);
            Assert.AreEqual("a", vsb.ToString());
        }

        [Test]
        public void Ctor_Span_CanAppend()
        {
            using var vsb = new ValueStringBuilder(new char[1]);
            Assert.AreEqual(0, vsb.Length);

            vsb.Append('a');
            Assert.AreEqual(1, vsb.Length);
            Assert.AreEqual("a", vsb.ToString());
        }

        [Test]
        public void Ctor_InitialCapacity_CanAppend()
        {
            using var vsb = new ValueStringBuilder(1);
            Assert.AreEqual(0, vsb.Length);

            vsb.Append('a');
            Assert.AreEqual(1, vsb.Length);
            Assert.AreEqual("a", vsb.ToString());
        }

        [Test]
        public void Append_Char_CapacityExceeded_TracksMaxLength()
        {
            Span<char> output = stackalloc char[5];

            var sb = new ValueStringBuilder(output);
            try
            {
                Assert.AreEqual(5, sb.Capacity);
                for (int i = 0; i < 9; i++)
                    sb.Append((char)i);

                Assert.AreEqual(9, sb.Length);
                Assert.AreEqual(9, sb.MaxLength);

                sb.Length = 0;
                Assert.AreEqual(0, sb.Length);
                Assert.AreEqual(9, sb.MaxLength);

                sb.Length = 5;
                Assert.AreEqual(5, sb.Length);
                Assert.AreEqual(9, sb.MaxLength);
            }
            finally
            {
                sb.Dispose();
            }
        }

        [Test]
        public void Append_Char_MatchesStringBuilder()
        {
            var sb = new StringBuilder();
            using var vsb = new ValueStringBuilder();
            for (int i = 1; i <= 100; i++)
            {
                sb.Append((char)i);
                vsb.Append((char)i);
            }

            Assert.AreEqual(sb.Length, vsb.Length);
            Assert.AreEqual(sb.ToString(), vsb.ToString());
        }

        [Test]
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

            Assert.AreEqual(sb.Length, vsb.Length);
            Assert.AreEqual(sb.ToString(), vsb.ToString());
        }

        [Theory]
        [TestCase(0, 4 * 1024 * 1024)]
        [TestCase(1025, 4 * 1024 * 1024)]
        [TestCase(3 * 1024 * 1024, 6 * 1024 * 1024)]
        public void Append_String_Large_MatchesStringBuilder(int initialLength, int stringLength)
        {
            var sb = new StringBuilder(initialLength);
            using var vsb = new ValueStringBuilder(new char[initialLength]);

            string s = new string('a', stringLength);
            sb.Append(s);
            vsb.Append(s);

            Assert.AreEqual(sb.Length, vsb.Length);
            Assert.AreEqual(sb.ToString(), vsb.ToString());
        }

        [Test]
        public void Append_String_BeyondCapacity_SetsCapacityExceeded()
        {
            using var sb = new ValueStringBuilder(stackalloc char[5]);
            Assert.IsFalse(sb.CapacityExceeded);

            sb.Append("012345678");
            Assert.IsTrue(sb.CapacityExceeded);
        }

        [Test]
        public void Append_String_BeyondCapacity_TracksMaxLength()
        {
            var sb = new ValueStringBuilder(stackalloc char[5]);
            try
            {
                Assert.AreEqual(0, sb.Length);
                Assert.AreEqual(0, sb.MaxLength);

                sb.Append("012345678");
                Assert.AreEqual(9, sb.Length);
                Assert.AreEqual(9, sb.MaxLength);

                sb.Length = 4;
                Assert.AreEqual(4, sb.Length);
                Assert.AreEqual(9, sb.MaxLength);

                sb.Append("012345678");
                Assert.AreEqual(13, sb.Length);
                Assert.AreEqual(13, sb.MaxLength);

                sb.Length = 4;
                Assert.AreEqual(4, sb.Length);
                Assert.AreEqual(13, sb.MaxLength);
            }
            finally
            {
                sb.Dispose();
            }
        }

        [Test]
        public void Append_CharInt_MatchesStringBuilder()
        {
            var sb = new StringBuilder();
            using var vsb = new ValueStringBuilder();
            for (int i = 1; i <= 100; i++)
            {
                sb.Append((char)i, i);
                vsb.Append((char)i, i);
            }

            Assert.AreEqual(sb.Length, vsb.Length);
            Assert.AreEqual(sb.ToString(), vsb.ToString());
        }

        [Test]
        public void Append_CharInt_CapacityExeeded_SetsMaxLength()
        {
            var sb = new ValueStringBuilder(stackalloc char[5]);
            try
            {
                for (int i = 1; i <= 100; i++)
                {
                    sb.Append((char)i, i);
                }

                Assert.AreEqual(5050, sb.Length);
                Assert.AreEqual(5050, sb.MaxLength);

                sb.Length = 0;
                Assert.AreEqual(0, sb.Length);
                Assert.AreEqual(5050, sb.MaxLength);

                sb.Length = 5;
                Assert.AreEqual(5, sb.Length);
                Assert.AreEqual(5050, sb.MaxLength);
            }
            finally
            {
                sb.Dispose();
            }
        }

#if FEATURE_COMPILE_UNSAFE

        [Test]
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

            Assert.AreEqual(sb.Length, vsb.Length);
            Assert.AreEqual(sb.ToString(), vsb.ToString());
        }

#endif

        [Test]
        public void AppendSpan_DataAppendedCorrectly()
        {
            var sb = new StringBuilder();
            using var vsb = new ValueStringBuilder();

            for (int i = 1; i <= 1000; i++)
            {
                string s = i.ToString();

                sb.Append(s);

                Span<char> span = vsb.AppendSpan(s.Length);
                Assert.AreEqual(sb.Length, vsb.Length);

                s.AsSpan().CopyTo(span);
            }

            Assert.AreEqual(sb.Length, vsb.Length);
            Assert.AreEqual(sb.ToString(), vsb.ToString());
        }

        [Test]
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

                Assert.AreEqual(2893, sb.Length);
                Assert.AreEqual(2893, sb.MaxLength);

                sb.Length = 0;
                Assert.AreEqual(0, sb.Length);
                Assert.AreEqual(2893, sb.MaxLength);

                sb.Length = 5;
                Assert.AreEqual(5, sb.Length);
                Assert.AreEqual(2893, sb.MaxLength);
            }
            finally
            {
                sb.Dispose();
            }
        }

        [Test]
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

            Assert.AreEqual(sb.Length, vsb.Length);
            Assert.AreEqual(sb.ToString(), vsb.ToString());
        }

        [Test]
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

                Assert.AreEqual(5050, sb.Length);
                Assert.AreEqual(5050, sb.MaxLength);

                sb.Length = 0;
                Assert.AreEqual(0, sb.Length);
                Assert.AreEqual(5050, sb.MaxLength);

                sb.Length = 5;
                Assert.AreEqual(5, sb.Length);
                Assert.AreEqual(5050, sb.MaxLength);
            }
            finally
            {
                sb.Dispose();
            }
        }

        [Test]
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
            Assert.AreEqual(sb.ToString(), resultString);

            Assert.AreNotEqual(0, sb.Length);
            Assert.AreEqual(sb.Length, vsb.Length);
            Assert.AreEqual(sb.ToString(), vsb.ToString());
        }

        // LUCENENET: Removed Dispose() call so we can safely call ToString() multiple times
        [Test]
        public void ToString_AllowsReuse()
        {
            const string Text1 = "test";
            using var vsb = new ValueStringBuilder();

            vsb.Append(Text1);
            Assert.AreEqual(Text1.Length, vsb.Length);

            string s = vsb.ToString();
            Assert.AreEqual(Text1, s);

            Assert.AreEqual(Text1.Length, vsb.Length);
            Assert.AreEqual(Text1, vsb.ToString());
            //Assert.True(vsb.TryCopyTo(Span<char>.Empty, out _));

            const string Text2 = "another test";
            vsb.Append(Text2);
            Assert.AreEqual(Text1.Length + Text2.Length, vsb.Length);
            Assert.AreEqual(Text1 + Text2, vsb.ToString());
        }

        [Test]
        public void TryCopyTo_FailsWhenDestinationIsTooSmall_SucceedsWhenItsLargeEnough()
        {
            using var vsb = new ValueStringBuilder();

            const string Text = "expected text";
            vsb.Append(Text);
            Assert.AreEqual(Text.Length, vsb.Length);

            Span<char> dst = new char[Text.Length - 1];
            Assert.False(vsb.TryCopyTo(dst, out int charsWritten));
            Assert.AreEqual(0, charsWritten);
            Assert.AreEqual(Text.Length, vsb.Length);
        }

        [Test]
        public void TryCopyTo_AllowsReuse()
        {
            const string Text1 = "test";
            using var vsb = new ValueStringBuilder();

            vsb.Append(Text1);
            Assert.AreEqual(Text1.Length, vsb.Length);

            Span<char> dst = new char[Text1.Length];
            Assert.True(vsb.TryCopyTo(dst, out int charsWritten));
            Assert.AreEqual(Text1.Length, charsWritten);
            Assert.AreEqual(Text1, new string(dst));

            Assert.AreEqual(Text1.Length, vsb.Length);
            Assert.AreEqual(Text1, vsb.ToString());
            //Assert.True(vsb.TryCopyTo(Span<char>.Empty, out _));

            const string Text2 = "another test";
            vsb.Append(Text2);
            Assert.AreEqual(Text1.Length + Text2.Length, vsb.Length);
            Assert.AreEqual(Text1 + Text2, vsb.ToString());
        }

        [Test]
        public void Dispose_ClearsBuilder_ThenReusable()
        {
            const string Text1 = "test";
            using var vsb = new ValueStringBuilder();

            vsb.Append(Text1);
            Assert.AreEqual(Text1.Length, vsb.Length);

            vsb.Dispose();

            Assert.AreEqual(0, vsb.Length);
            Assert.AreEqual(string.Empty, vsb.ToString());
            Assert.True(vsb.TryCopyTo(Span<char>.Empty, out _));

            const string Text2 = "another test";
            vsb.Append(Text2);
            Assert.AreEqual(Text2.Length, vsb.Length);
            Assert.AreEqual(Text2, vsb.ToString());
        }

        [Test]
        public void Indexer()
        {
            const string Text1 = "foobar";
            using var vsb = new ValueStringBuilder();

            vsb.Append(Text1);

            Assert.AreEqual('b', vsb[3]);
            vsb[3] = 'c';
            Assert.AreEqual('c', vsb[3]);
        }

        [Test]
        public void EnsureCapacity_IfRequestedCapacityWins()
        {
            // Note: constants used here may be dependent on minimal buffer size
            // the ArrayPool is able to return.
            using var builder = new ValueStringBuilder(stackalloc char[32]);

            builder.EnsureCapacity(65, out int newCapacity);

            // LUCENENET: We over-allocate compared with the BCL
            Assert.AreEqual(65, newCapacity);
            Assert.GreaterOrEqual(builder.Capacity, 128);
            Assert.IsTrue(builder.CapacityExceeded);
        }

        [Test]
        public void EnsureCapacity_IfBufferTimesTwoWins()
        {
            using var builder = new ValueStringBuilder(stackalloc char[32]);

            builder.EnsureCapacity(33, out int newCapacity);

            // LUCENENET: We over-allocate compared with the BCL
            Assert.AreEqual(64, newCapacity);
            Assert.GreaterOrEqual(builder.Capacity, 64);
            Assert.IsTrue(builder.CapacityExceeded);
        }

        [Test]
        public void EnsureCapacity_NoAllocIfNotNeeded()
        {
            // Note: constants used here may be dependent on minimal buffer size
            // the ArrayPool is able to return.
            using var builder = new ValueStringBuilder(stackalloc char[64]);

            builder.EnsureCapacity(16);

            Assert.AreEqual(64, builder.Capacity);
        }

        [Test]
        public void Length_BeyondCapacity_TracksMaxLength()
        {
            var sb = new ValueStringBuilder(stackalloc char[5]);
            try
            {
                Assert.AreEqual(0, sb.Length);
                Assert.AreEqual(0, sb.MaxLength);

                sb.Append("012345678");
                Assert.AreEqual(9, sb.Length);
                Assert.AreEqual(9, sb.MaxLength);

                sb.Length = 4;
                Assert.AreEqual(4, sb.Length);
                Assert.AreEqual(9, sb.MaxLength);

                sb.Append("012345678");
                Assert.AreEqual(13, sb.Length);
                Assert.AreEqual(13, sb.MaxLength);

                sb.Length = 4;
                Assert.AreEqual(4, sb.Length);
                Assert.AreEqual(13, sb.MaxLength);
            }
            finally
            {
                sb.Dispose();
            }
        }

        [TestCase("Hello", 0, '\0', "\0Hello")]
        [TestCase("Hello", 3, 'a', "Helalo")]
        [TestCase("Hello", 5, 'b', "Hellob")]
        public static void Insert_Char(string original, int index, char value, string expected)
        {
            using var builder = new ValueStringBuilder(stackalloc char[original.Length]);
            builder.Append(original);
            builder.Insert(index, value);
            Assert.AreEqual(expected, builder.ToString());
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

        [TestCaseSource(nameof(Insert_String_Count_TestData))]
        public void Insert_String_Count(string original, int index, string value, int count, string expected)
        {
            if (count == 1)
            {
                // Use Insert(int, string)
                using var vsb = new ValueStringBuilder(stackalloc char[original.Length]);
                vsb.Append(original);
                vsb.Insert(index, value);
                Assert.AreEqual(expected, vsb.ToString());
            }
            // Use Insert(int, string, int)
            using var builder = new ValueStringBuilder(stackalloc char[original.Length]);
            builder.Append(original);
            builder.Insert(index, value, count);
            Assert.AreEqual(expected, builder.ToString());
        }

        [TestCaseSource(nameof(Insert_String_Count_TestData))]
        public void Insert_ReadOnlySpan_Count(string original, int index, string value, int count, string expected)
        {
            if (count == 1)
            {
                // Use Insert(int, string)
                using var vsb = new ValueStringBuilder(stackalloc char[original.Length]);
                vsb.Append(original);
                vsb.Insert(index, value.AsSpan());
                Assert.AreEqual(expected, vsb.ToString());
            }
            // Use Insert(int, string, int)
            using var builder = new ValueStringBuilder(stackalloc char[original.Length]);
            builder.Append(original);
            builder.Insert(index, value.AsSpan(), count);
            Assert.AreEqual(expected, builder.ToString());
        }


        #region AsSpan


        [Test]
        public static void OpenStringBuilderAsSpanNullary()
        {
            using var s = new ValueStringBuilder(stackalloc char[16]);
            s.Append("Hello");
            ReadOnlySpan<char> span = s.AsSpan();
            char[] expected = s.RawChars.Slice(0, s.Length).ToArray();
            span.Validate(expected);
        }

        [Test]
        public static void StringAsSpanEmptyString()
        {
            using var s = new ValueStringBuilder(stackalloc char[16]);
            ReadOnlySpan<char> span = s.AsSpan();
            span.ValidateNonNullEmpty();
        }

        [TestCaseSource(typeof(TestHelpers), nameof(TestHelpers.StringSliceTestData))]
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
                Assert.AreEqual(length, span.Length);
                fixed (char* pText = &MemoryMarshal.GetReference(text.RawChars))
                {
                    // Unsafe.AsPointer is safe here since it's pinned (since text and span should be the same string)
                    char* expected = pText + start;
                    void* actual = Unsafe.AsPointer(ref MemoryMarshal.GetReference(span));
                    Assert.AreEqual((IntPtr)expected, (IntPtr)actual);
                }
            }
        }

        #endregion AsSpan

        #region AsMemory

        [TestCase(0, 0)]
        [TestCase(3, 0)]
        [TestCase(3, 1)]
        [TestCase(3, 2)]
        [TestCase(3, 3)]
        [TestCase(10, 0)]
        [TestCase(10, 3)]
        [TestCase(10, 10)]
        public static void OpenStringBuilderAsMemoryWithStart(int length, int start)
        {
            using ValueStringBuilder a = new ValueStringBuilder(length);
            a.Append('\0', length);
            ReadOnlyMemory<char> m = a.AsMemory(start);
            Assert.AreEqual(length - start, m.Length);
            if (start != length)
            {
                a[start] = (char)42;
                Assert.AreEqual(42, m.Span[0]);
            }
        }

        [TestCase(0, 0, 0)]
        [TestCase(3, 0, 3)]
        [TestCase(3, 1, 2)]
        [TestCase(3, 2, 1)]
        [TestCase(3, 3, 0)]
        [TestCase(10, 0, 5)]
        [TestCase(10, 3, 2)]
        public static void OpenStringBuilderAsMemoryWithStartAndLength(int length, int start, int subLength)
        {
            using ValueStringBuilder a = new ValueStringBuilder(length);
            a.Append('\0', length);

            ReadOnlyMemory<char> m = a.AsMemory(start, subLength);
            Assert.AreEqual(subLength, m.Length);
            if (subLength != 0)
            {
                a[start] = (char)42;
                Assert.AreEqual(42, m.Span[0]);
            }
        }

        #endregion AsMemory

    }
}
