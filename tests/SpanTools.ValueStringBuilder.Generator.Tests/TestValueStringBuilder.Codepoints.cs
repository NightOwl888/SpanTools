// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE.txt file or at https://opensource.org/licenses/MIT.

using System;
using Xunit;

#if FEATURE_NAMESPACE_MYNAMESPACE
using MyNamespace;
#elif FEATURE_NAMESPACE_LUCENENETTEXT
using Lucene.Net.Text;
#elif FEATURE_NAMESPACE_EMPTY
// Global namespace
#endif

namespace SpanTools.Generator.Tests
{
    public partial class ValueStringBuilderTests
    {
        [Fact]
        public virtual void TestAppendCodePointBmp()
        {
            using var sb = new ValueStringBuilder(stackalloc char[16]);
            sb.Append("foo bar");

            int codePoint = 97; // a

            sb.AppendCodePoint(codePoint);

            Assert.Equal("foo bara", sb.ToString());
        }

        [Fact]
        public virtual void TestAppendCodePointUnicode()
        {
            using var sb = new ValueStringBuilder(stackalloc char[16]);
            sb.Append("foo bar");

            int codePoint = 3594; // ช

            sb.AppendCodePoint(codePoint);

            Assert.Equal("foo barช", sb.ToString());
        }

        [Fact]
        public virtual void TestAppendCodePointUTF16Surrogates()
        {
            using var sb = new ValueStringBuilder(stackalloc char[16]);
            sb.Append("foo bar");

            int codePoint = 176129; // '\uD86C', '\uDC01' (𫀁)

            sb.AppendCodePoint(codePoint);

            Assert.Equal("foo bar𫀁", sb.ToString());
        }

        [Fact]
        public virtual void TestAppendCodePointTooHigh()
        {
            using var sb = new ValueStringBuilder(stackalloc char[16]);
            sb.Append("foo bar");

            int codePoint = J2N.Character.MaxCodePoint + 1;

            try
            {
                sb.AppendCodePoint(codePoint);
                Assert.Fail("Expected ArgumentException");
            }
            catch (ArgumentException)
            {
            }
        }

        [Fact]
        public virtual void TestAppendCodePointTooLow()
        {
            using var sb = new ValueStringBuilder(stackalloc char[16]);
            sb.Append("foo bar");

            int codePoint = J2N.Character.MinCodePoint - 1;

            try
            {
                sb.AppendCodePoint(codePoint);
                Assert.Fail("Expected ArgumentException");
            }
            catch (ArgumentException)
            {
            }
        }

#if FEATURE_VALUESTRINGBUILDER_INCLUDEMAXLENGTHTRACKING

        [Fact]
        public void InsertCodePoint_CapacityExceeded_TracksMaxLength()
        {
            var sb = new ValueStringBuilder(stackalloc char[7]);
            try
            {
                sb.Append("foo bar");

                int codePoint = 97; // a

                sb.InsertCodePoint(0, codePoint);

                Assert.Equal("afoo bar", sb.AsSpan().ToString());
                Assert.True(sb.CapacityExceeded);
                Assert.Equal(8, sb.Length);
                Assert.Equal(8, sb.MaxLength);

                sb.Length = 5;
                Assert.Equal(5, sb.Length);
                Assert.Equal(8, sb.MaxLength);
            }
            finally
            {
                sb.Dispose();
            }
        }

#endif

        [Fact]
        public virtual void TestInsertCodePointBmp()
        {
            using var sb = new ValueStringBuilder(stackalloc char[16]);
            sb.Append("foo bar");

            int codePoint = 97; // a

            sb.InsertCodePoint(0, codePoint);

            Assert.Equal("afoo bar", sb.ToString());
        }

        [Fact]
        public virtual void TestInsertCodePointUnicode()
        {
            using var sb = new ValueStringBuilder(stackalloc char[16]);
            sb.Append("foo bar");

            int codePoint = 3594; // ช

            sb.InsertCodePoint(1, codePoint);

            Assert.Equal("fชoo bar", sb.ToString());
        }

        [Fact]
        public virtual void TestInsertCodePointUTF16Surrogates()
        {
            using var sb = new ValueStringBuilder(stackalloc char[16]);
            sb.Append("foo bar");

            int codePoint = 176129; // '\uD86C', '\uDC01' (𫀁)

            sb.InsertCodePoint(2, codePoint);

            Assert.Equal("fo𫀁o bar", sb.ToString());
        }

        [Fact]
        public virtual void TestInsertCodePointTooHigh()
        {
            using var sb = new ValueStringBuilder(stackalloc char[16]);
            sb.Append("foo bar");

            int codePoint = J2N.Character.MaxCodePoint + 1;

            try
            {
                sb.InsertCodePoint(0, codePoint);
                Assert.Fail("Expected ArgumentException");
            }
            catch (ArgumentException)
            {
            }
        }

        [Fact]
        public virtual void TestInsertCodePointTooLow()
        {
            using var sb = new ValueStringBuilder(stackalloc char[16]);
            sb.Append("foo bar");

            int codePoint = J2N.Character.MinCodePoint - 1;

            try
            {
                sb.InsertCodePoint(0, codePoint);
                Assert.Fail("Expected ArgumentException");
            }
            catch (ArgumentException)
            {
            }
        }
    }
}
