// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE.txt file or at https://opensource.org/licenses/MIT.

using MyNamespace;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpanTools.Generator.Tests
{
    public partial class ValueStringBuilderTests
    {
        [Test]
        public virtual void TestAppendCodePointBmp()
        {
            using var sb = new ValueStringBuilder(stackalloc char[16]);
            sb.Append("foo bar");

            int codePoint = 97; // a

            sb.AppendCodePoint(codePoint);

            Assert.AreEqual("foo bara", sb.ToString());
        }

        [Test]
        public virtual void TestAppendCodePointUnicode()
        {
            using var sb = new ValueStringBuilder(stackalloc char[16]);
            sb.Append("foo bar");

            int codePoint = 3594; // ช

            sb.AppendCodePoint(codePoint);

            Assert.AreEqual("foo barช", sb.ToString());
        }

        [Test]
        public virtual void TestAppendCodePointUTF16Surrogates()
        {
            using var sb = new ValueStringBuilder(stackalloc char[16]);
            sb.Append("foo bar");

            int codePoint = 176129; // '\uD86C', '\uDC01' (𫀁)

            sb.AppendCodePoint(codePoint);

            Assert.AreEqual("foo bar𫀁", sb.ToString());
        }

        [Test]
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

        [Test]
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


        [Test]
        public void InsertCodePoint_CapacityExceeded_TracksMaxLength()
        {
            var sb = new ValueStringBuilder(stackalloc char[7]);
            try
            {
                sb.Append("foo bar");

                int codePoint = 97; // a

                sb.InsertCodePoint(0, codePoint);

                Assert.AreEqual("afoo bar", sb.AsSpan().ToString());
                Assert.IsTrue(sb.CapacityExceeded);
                Assert.AreEqual(8, sb.Length);
                Assert.AreEqual(8, sb.MaxLength);

                sb.Length = 5;
                Assert.AreEqual(5, sb.Length);
                Assert.AreEqual(8, sb.MaxLength);
            }
            finally
            {
                sb.Dispose();
            }
        }

        [Test]
        public virtual void TestInsertCodePointBmp()
        {
            using var sb = new ValueStringBuilder(stackalloc char[16]);
            sb.Append("foo bar");

            int codePoint = 97; // a

            sb.InsertCodePoint(0, codePoint);

            Assert.AreEqual("afoo bar", sb.ToString());
        }

        [Test]
        public virtual void TestInsertCodePointUnicode()
        {
            using var sb = new ValueStringBuilder(stackalloc char[16]);
            sb.Append("foo bar");

            int codePoint = 3594; // ช

            sb.InsertCodePoint(1, codePoint);

            Assert.AreEqual("fชoo bar", sb.ToString());
        }

        [Test]
        public virtual void TestInsertCodePointUTF16Surrogates()
        {
            using var sb = new ValueStringBuilder(stackalloc char[16]);
            sb.Append("foo bar");

            int codePoint = 176129; // '\uD86C', '\uDC01' (𫀁)

            sb.InsertCodePoint(2, codePoint);

            Assert.AreEqual("fo𫀁o bar", sb.ToString());
        }

        [Test]
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

        [Test]
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
