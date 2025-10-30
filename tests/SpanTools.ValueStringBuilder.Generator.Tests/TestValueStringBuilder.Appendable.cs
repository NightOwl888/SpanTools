// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE.txt file or at https://opensource.org/licenses/MIT.

using MyNamespace;
using J2N.Text;
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
        /**
        * @tests java.lang.StringBuilder.append(CharSequence)
        */
        [Test]
        public void Test_Append_String()
        {
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                sb.Append("ab");
                Assert.AreEqual("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append("cd");
                Assert.AreEqual("cd", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append((string?)null);
                // assertEquals("null", sb.ToString());
                Assert.AreEqual("", sb.AsSpan().ToString()); // J2N: Changed the behavior to be a no-op rather than appending the string "null"
            }
            finally
            {
                sb.Dispose();
            }
        }

        /**
         * @tests java.lang.StringBuilder.append(CharSequence, int, int)
         */
        [Test]
        public void Test_Append_String_Int32_Int32()
        {
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                sb.Append("ab", 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.AreEqual("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append("cd", 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.AreEqual("cd", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append("abcd", 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.AreEqual("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append("abcd", 2, 4 - 2); // J2N: corrected 3rd parameter
                Assert.AreEqual("cd", sb.AsSpan().ToString());
                sb.Length = (0);
                //try
                //{
                //    assertSame(sb, sb.Append((ICharSequence)null, 0, 2)); // J2N: Changed the behavior to throw an exception (to match .NET Core 3.0's Append(StringBuilder,int,int) overload) rather than appending the string "null"
                //    fail("no NPE");
                //}
                //catch (ArgumentNullException e)
                //{
                //    // Expected
                //}
                //assertEquals("nu", sb.ToString());
                Assert.AreEqual("", sb.AsSpan().ToString());
            }
            finally
            {
                sb.Dispose();
            }
        }

        /**
         * @tests java.lang.StringBuilder.append(CharSequence)
         */
        [Test]
        public void Test_Append_CharArray()
        {
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                sb.Append("ab".ToCharArray());
                Assert.AreEqual("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append("cd".ToCharArray());
                Assert.AreEqual("cd", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append((char[]?)null);
                // assertEquals("null", sb.ToString());
                Assert.AreEqual("", sb.AsSpan().ToString()); // J2N: Changed the behavior to be a no-op rather than appending the string "null"
            }
            finally
            {
                sb.Dispose();
            }
        }

        /**
         * @tests java.lang.StringBuilder.append(CharSequence, int, int)
         */
        [Test]
        public void Test_Append_CharArray_Int32_Int32()
        {
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                sb.Append("ab".ToCharArray(), 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.AreEqual("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append("cd".ToCharArray(), 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.AreEqual("cd", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append("abcd".ToCharArray(), 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.AreEqual("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append("abcd".ToCharArray(), 2, 4 - 2); // J2N: corrected 3rd parameter
                Assert.AreEqual("cd", sb.AsSpan().ToString());
                sb.Length = (0);
                //try
                //{
                //    assertSame(sb, sb.Append((ICharSequence)null, 0, 2)); // J2N: Changed the behavior to throw an exception (to match .NET Core 3.0's Append(StringBuilder,int,int) overload) rather than appending the string "null"
                //    fail("no NPE");
                //}
                //catch (ArgumentNullException e)
                //{
                //    // Expected
                //}
                //assertEquals("nu", sb.ToString());
                Assert.AreEqual("", sb.AsSpan().ToString());
            }
            finally
            {
                sb.Dispose();
            }
        }


        /**
         * @tests java.lang.StringBuilder.append(CharSequence)
         */
        [Test]
        public void Test_Append_ICharSequence()
        {
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                sb.Append("ab".AsCharSequence());
                Assert.AreEqual("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append("cd".AsCharSequence());
                Assert.AreEqual("cd", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append((ICharSequence?)null);
                // assertEquals("null", sb.ToString());
                Assert.AreEqual("", sb.AsSpan().ToString()); // J2N: Changed the behavior to be a no-op rather than appending the string "null"
            }
            finally
            {
                sb.Dispose();
            }
        }

        /**
         * @tests java.lang.StringBuilder.append(CharSequence, int, int)
         */
        [Test]
        public void Test_Append_ICharSequence_Int32_Int32()
        {
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                sb.Append("ab".AsCharSequence(), 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.AreEqual("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append("cd".AsCharSequence(), 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.AreEqual("cd", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append("abcd".AsCharSequence(), 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.AreEqual("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append("abcd".AsCharSequence(), 2, 4 - 2); // J2N: corrected 3rd parameter
                Assert.AreEqual("cd", sb.AsSpan().ToString());
                sb.Length = (0);
                //try
                //{
                //    assertSame(sb, sb.Append((ICharSequence)null, 0, 2)); // J2N: Changed the behavior to throw an exception (to match .NET Core 3.0's Append(StringBuilder,int,int) overload) rather than appending the string "null"
                //    fail("no NPE");
                //}
                //catch (ArgumentNullException e)
                //{
                //    // Expected
                //}
                //assertEquals("nu", sb.ToString());
                Assert.AreEqual("", sb.AsSpan().ToString());
            }
            finally
            {
                sb.Dispose();
            }
        }

        private sealed class MyCharSequence : ICharSequence
        {
            private readonly string value;
            public MyCharSequence(string value)
            {
                this.value = value ?? throw new ArgumentNullException(nameof(value));
            }

            public char this[int index] => value[index];

            public bool HasValue => true;

            public int Length => value.Length;

            public ICharSequence Subsequence(int startIndex, int length)
                => value.Substring(startIndex, length).AsCharSequence();

            public override string ToString()
            {
                return value;
            }
        }

        /**
         * @tests java.lang.StringBuilder.append(CharSequence)
         */
        [Test]
        public void Test_Append_ICharSequence_Custom()
        {
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                sb.Append(new MyCharSequence("ab"));
                Assert.AreEqual("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append(new MyCharSequence("cd"));
                Assert.AreEqual("cd", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append((MyCharSequence?)null);
                // assertEquals("null", sb.ToString());
                Assert.AreEqual("", sb.AsSpan().ToString()); // J2N: Changed the behavior to be a no-op rather than appending the string "null"
            }
            finally
            {
                sb.Dispose();
            }
        }

        /**
         * @tests java.lang.StringBuilder.append(CharSequence, int, int)
         */
        [Test]
        public void Test_Append_ICharSequence_Int32_Int32_Custom()
        {
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                sb.Append(new MyCharSequence("ab"), 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.AreEqual("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append(new MyCharSequence("cd"), 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.AreEqual("cd", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append(new MyCharSequence("abcd"), 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.AreEqual("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append(new MyCharSequence("abcd"), 2, 4 - 2); // J2N: corrected 3rd parameter
                Assert.AreEqual("cd", sb.AsSpan().ToString());
                sb.Length = (0);
                //try
                //{
                //    assertSame(sb, sb.Append((ICharSequence)null, 0, 2)); // J2N: Changed the behavior to throw an exception (to match .NET Core 3.0's Append(StringBuilder,int,int) overload) rather than appending the string "null"
                //    fail("no NPE");
                //}
                //catch (ArgumentNullException e)
                //{
                //    // Expected
                //}
                //assertEquals("nu", sb.ToString());
                Assert.AreEqual("", sb.AsSpan().ToString());
            }
            finally
            {
                sb.Dispose();
            }
        }

        /**
         * @tests java.lang.StringBuilder.append(CharSequence)
         */
        [Test]
        public void Test_Append_StringBuilder()
        {
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                sb.Append(new StringBuilder("ab"));
                Assert.AreEqual("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append(new StringBuilder("cd"));
                Assert.AreEqual("cd", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append((StringBuilder?)null);
                // assertEquals("null", sb.ToString());
                Assert.AreEqual("", sb.AsSpan().ToString()); // J2N: Changed the behavior to be a no-op rather than appending the string "null"
            }
            finally
            {
                sb.Dispose();
            }
        }

        /**
         * @tests java.lang.StringBuilder.append(CharSequence, int, int)
         */
        [Test]
        public void Test_Append_StringBuilder_Int32_Int32()
        {
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                sb.Append(new StringBuilder("ab"), 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.AreEqual("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append(new StringBuilder("cd"), 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.AreEqual("cd", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append(new StringBuilder("abcd"), 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.AreEqual("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append(new StringBuilder("abcd"), 2, 4 - 2); // J2N: corrected 3rd parameter
                Assert.AreEqual("cd", sb.AsSpan().ToString());
                sb.Length = (0);
                //try
                //{
                //    assertSame(sb, sb.Append((StringBuilder)null, 0, 2)); // J2N: Changed the behavior to throw an exception (to match .NET Core 3.0) rather than appending the string "null"
                //    fail("no NPE");
                //}
                //catch (ArgumentNullException e)
                //{
                //    // Expected
                //}
                //assertEquals("nu", sb.ToString());
                Assert.AreEqual("", sb.AsSpan().ToString());
            }
            finally
            {
                sb.Dispose();
            }
        }

        /**
         * @tests java.lang.StringBuilder.Insert(int, CharSequence)
         */
        [Test]
        public void Test_Insert_ICharSequence()
        {
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                const string fixture = "0000";

                sb.Append(fixture);
                sb.Insert(0, (ICharSequence)"ab".AsCharSequence());
                Assert.AreEqual("ab0000", sb.ToString());
                Assert.AreEqual(6, sb.Length);

                sb.Length = 0;
                sb.Append(fixture);
                sb.Insert(2, (ICharSequence)"ab".AsCharSequence());
                Assert.AreEqual("00ab00", sb.ToString());
                Assert.AreEqual(6, sb.Length);

                sb.Length = 0;
                sb.Append(fixture);
                sb.Insert(4, (ICharSequence)"ab".AsCharSequence());
                Assert.AreEqual("0000ab", sb.ToString());
                Assert.AreEqual(6, sb.Length);


                sb.Length = 0;
                sb.Append(fixture);
                sb.Insert(4, (ICharSequence?)null);
                //assertEquals("0000null", sb.ToString());
                //assertEquals(8, sb.Length);
                Assert.AreEqual("0000", sb.ToString()); // J2N: Changed behavior to make adding null a no-op to match .NET
                Assert.AreEqual(4, sb.Length);

                //try
                //{
                //    sb = new OpenStringBuilder(fixture);
                //    sb.Insert(-1, (ICharSequence)"ab".AsCharSequence());
                //    fail("no IOOBE, negative index");
                //}
                //catch (ArgumentOutOfRangeException) // IndexOutOfBoundsException
                //{
                //    // Expected
                //}

                //try
                //{
                //    sb = new OpenStringBuilder(fixture);
                //    sb.Insert(5, (ICharSequence)"ab".AsCharSequence());
                //    fail("no IOOBE, index too large index");
                //}
                //catch (ArgumentOutOfRangeException) // IndexOutOfBoundsException
                //{
                //    // Expected
                //}
            }
            finally
            {
                sb.Dispose();
            }
        }

        [Test]
        public void Test_Insert_ICharSequence_BeyondCapacity_TracksMaxLength()
        {
            using var sb = new ValueStringBuilder(stackalloc char[5]);
            Assert.AreEqual(0, sb.Length);
            Assert.AreEqual(0, sb.MaxLength);

            sb.Insert(0, "012345678".AsCharSequence());
            Assert.AreEqual(9, sb.Length);
            Assert.AreEqual(9, sb.MaxLength);

            sb.Remove(1, 5);
            Assert.AreEqual(4, sb.Length);
            Assert.AreEqual(9, sb.MaxLength);

            sb.Append("012345678");
            Assert.AreEqual(13, sb.Length);
            Assert.AreEqual(13, sb.MaxLength);

            sb.Remove(3, 9);
            Assert.AreEqual(4, sb.Length);
            Assert.AreEqual(13, sb.MaxLength);
        }

        /**
         * @tests java.lang.StringBuilder.Insert(int, CharSequence)
         */
        [Test]
        public void Test_Insert_StringBuilder()
        {
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                const string fixture = "0000";

                sb.Append(fixture);
                sb.Insert(0, new StringBuilder("ab"));
                Assert.AreEqual("ab0000", sb.ToString());
                Assert.AreEqual(6, sb.Length);

                sb.Length = 0;
                sb.Append(fixture);
                sb.Insert(2, new StringBuilder("ab"));
                Assert.AreEqual("00ab00", sb.ToString());
                Assert.AreEqual(6, sb.Length);

                sb.Length = 0;
                sb.Append(fixture);
                sb.Insert(4, new StringBuilder("ab"));
                Assert.AreEqual("0000ab", sb.ToString());
                Assert.AreEqual(6, sb.Length);


                sb.Length = 0;
                sb.Append(fixture);
                sb.Insert(4, (StringBuilder?)null);
                //assertEquals("0000null", sb.ToString());
                //assertEquals(8, sb.Length);
                Assert.AreEqual("0000", sb.ToString()); // J2N: Changed behavior to make adding null a no-op to match .NET
                Assert.AreEqual(4, sb.Length);

                //try
                //{
                //    sb = new OpenStringBuilder(fixture);
                //    sb.Insert(-1, (ICharSequence)"ab".AsCharSequence());
                //    fail("no IOOBE, negative index");
                //}
                //catch (ArgumentOutOfRangeException) // IndexOutOfBoundsException
                //{
                //    // Expected
                //}

                //try
                //{
                //    sb = new OpenStringBuilder(fixture);
                //    sb.Insert(5, (ICharSequence)"ab".AsCharSequence());
                //    fail("no IOOBE, index too large index");
                //}
                //catch (ArgumentOutOfRangeException) // IndexOutOfBoundsException
                //{
                //    // Expected
                //}
            }
            finally
            {
                sb.Dispose();
            }
        }

        [Test]
        public void Test_Insert_StringBuilder_BeyondCapacity_TracksMaxLength()
        {
            using var sb = new ValueStringBuilder(stackalloc char[5]);
            Assert.AreEqual(0, sb.Length);
            Assert.AreEqual(0, sb.MaxLength);

            sb.Insert(0, new StringBuilder("012345678"));
            Assert.AreEqual(9, sb.Length);
            Assert.AreEqual(9, sb.MaxLength);

            sb.Remove(1, 5);
            Assert.AreEqual(4, sb.Length);
            Assert.AreEqual(9, sb.MaxLength);

            sb.Append("012345678");
            Assert.AreEqual(13, sb.Length);
            Assert.AreEqual(13, sb.MaxLength);

            sb.Remove(3, 9);
            Assert.AreEqual(4, sb.Length);
            Assert.AreEqual(13, sb.MaxLength);
        }
    }
}
