// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE.txt file or at https://opensource.org/licenses/MIT.

#if FEATURE_J2N
using J2N.Text;
#endif
using System;
using System.Text;
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
        /**
        * @tests java.lang.StringBuilder.append(CharSequence)
        */
        [Fact]
        public void Test_Append_String()
        {
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                sb.Append("ab");
                Assert.Equal("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append("cd");
                Assert.Equal("cd", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append((string?)null);
                // assertEquals("null", sb.ToString());
                Assert.Equal("", sb.AsSpan().ToString()); // J2N: Changed the behavior to be a no-op rather than appending the string "null"
            }
            finally
            {
                sb.Dispose();
            }
        }

        /**
         * @tests java.lang.StringBuilder.append(CharSequence, int, int)
         */
        [Fact]
        public void Test_Append_String_Int32_Int32()
        {
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                sb.Append("ab", 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.Equal("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append("cd", 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.Equal("cd", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append("abcd", 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.Equal("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append("abcd", 2, 4 - 2); // J2N: corrected 3rd parameter
                Assert.Equal("cd", sb.AsSpan().ToString());
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
                Assert.Equal("", sb.AsSpan().ToString());
            }
            finally
            {
                sb.Dispose();
            }
        }

        /**
         * @tests java.lang.StringBuilder.append(CharSequence)
         */
        [Fact]
        public void Test_Append_CharArray()
        {
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                sb.Append("ab".ToCharArray());
                Assert.Equal("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append("cd".ToCharArray());
                Assert.Equal("cd", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append((char[]?)null);
                // assertEquals("null", sb.ToString());
                Assert.Equal("", sb.AsSpan().ToString()); // J2N: Changed the behavior to be a no-op rather than appending the string "null"
            }
            finally
            {
                sb.Dispose();
            }
        }

        /**
         * @tests java.lang.StringBuilder.append(CharSequence, int, int)
         */
        [Fact]
        public void Test_Append_CharArray_Int32_Int32()
        {
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                sb.Append("ab".ToCharArray(), 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.Equal("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append("cd".ToCharArray(), 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.Equal("cd", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append("abcd".ToCharArray(), 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.Equal("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append("abcd".ToCharArray(), 2, 4 - 2); // J2N: corrected 3rd parameter
                Assert.Equal("cd", sb.AsSpan().ToString());
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
                Assert.Equal("", sb.AsSpan().ToString());
            }
            finally
            {
                sb.Dispose();
            }
        }

#if FEATURE_J2N_2_0_OR_GREATER
        /**
         * @tests java.lang.StringBuilder.append(CharSequence)
         */
        [Fact]
        public void Test_Append_ICharSequence()
        {
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                sb.Append("ab".AsCharSequence());
                Assert.Equal("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append("cd".AsCharSequence());
                Assert.Equal("cd", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append((ICharSequence?)null);
                // assertEquals("null", sb.ToString());
                Assert.Equal("", sb.AsSpan().ToString()); // J2N: Changed the behavior to be a no-op rather than appending the string "null"
            }
            finally
            {
                sb.Dispose();
            }
        }

        /**
         * @tests java.lang.StringBuilder.append(CharSequence, int, int)
         */
        [Fact]
        public void Test_Append_ICharSequence_Int32_Int32()
        {
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                sb.Append("ab".AsCharSequence(), 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.Equal("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append("cd".AsCharSequence(), 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.Equal("cd", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append("abcd".AsCharSequence(), 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.Equal("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append("abcd".AsCharSequence(), 2, 4 - 2); // J2N: corrected 3rd parameter
                Assert.Equal("cd", sb.AsSpan().ToString());
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
                Assert.Equal("", sb.AsSpan().ToString());
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
        [Fact]
        public void Test_Append_ICharSequence_Custom()
        {
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                sb.Append(new MyCharSequence("ab"));
                Assert.Equal("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append(new MyCharSequence("cd"));
                Assert.Equal("cd", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append((MyCharSequence?)null);
                // assertEquals("null", sb.ToString());
                Assert.Equal("", sb.AsSpan().ToString()); // J2N: Changed the behavior to be a no-op rather than appending the string "null"
            }
            finally
            {
                sb.Dispose();
            }
        }

        /**
         * @tests java.lang.StringBuilder.append(CharSequence, int, int)
         */
        [Fact]
        public void Test_Append_ICharSequence_Int32_Int32_Custom()
        {
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                sb.Append(new MyCharSequence("ab"), 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.Equal("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append(new MyCharSequence("cd"), 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.Equal("cd", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append(new MyCharSequence("abcd"), 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.Equal("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append(new MyCharSequence("abcd"), 2, 4 - 2); // J2N: corrected 3rd parameter
                Assert.Equal("cd", sb.AsSpan().ToString());
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
                Assert.Equal("", sb.AsSpan().ToString());
            }
            finally
            {
                sb.Dispose();
            }
        }
#endif

        /**
         * @tests java.lang.StringBuilder.append(CharSequence)
         */
        [Fact]
        public void Test_Append_StringBuilder()
        {
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                sb.Append(new StringBuilder("ab"));
                Assert.Equal("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append(new StringBuilder("cd"));
                Assert.Equal("cd", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append((StringBuilder?)null);
                // assertEquals("null", sb.ToString());
                Assert.Equal("", sb.AsSpan().ToString()); // J2N: Changed the behavior to be a no-op rather than appending the string "null"
            }
            finally
            {
                sb.Dispose();
            }
        }

        /**
         * @tests java.lang.StringBuilder.append(CharSequence, int, int)
         */
        [Fact]
        public void Test_Append_StringBuilder_Int32_Int32()
        {
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                sb.Append(new StringBuilder("ab"), 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.Equal("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append(new StringBuilder("cd"), 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.Equal("cd", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append(new StringBuilder("abcd"), 0, 2 - 0); // J2N: corrected 3rd parameter
                Assert.Equal("ab", sb.AsSpan().ToString());
                sb.Length = (0);
                sb.Append(new StringBuilder("abcd"), 2, 4 - 2); // J2N: corrected 3rd parameter
                Assert.Equal("cd", sb.AsSpan().ToString());
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
                Assert.Equal("", sb.AsSpan().ToString());
            }
            finally
            {
                sb.Dispose();
            }
        }

#if FEATURE_J2N_2_0_OR_GREATER

        /**
         * @tests java.lang.StringBuilder.Insert(int, CharSequence)
         */
        [Fact]
        public void Test_Insert_ICharSequence()
        {
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                const string fixture = "0000";

                sb.Append(fixture);
                sb.Insert(0, (ICharSequence)"ab".AsCharSequence());
                Assert.Equal("ab0000", sb.ToString());
                Assert.Equal(6, sb.Length);

                sb.Length = 0;
                sb.Append(fixture);
                sb.Insert(2, (ICharSequence)"ab".AsCharSequence());
                Assert.Equal("00ab00", sb.ToString());
                Assert.Equal(6, sb.Length);

                sb.Length = 0;
                sb.Append(fixture);
                sb.Insert(4, (ICharSequence)"ab".AsCharSequence());
                Assert.Equal("0000ab", sb.ToString());
                Assert.Equal(6, sb.Length);


                sb.Length = 0;
                sb.Append(fixture);
                sb.Insert(4, (ICharSequence?)null);
                //assertEquals("0000null", sb.ToString());
                //assertEquals(8, sb.Length);
                Assert.Equal("0000", sb.ToString()); // J2N: Changed behavior to make adding null a no-op to match .NET
                Assert.Equal(4, sb.Length);

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

#if FEATURE_VALUESTRINGBUILDER_INCLUDEMAXLENGTHTRACKING

        [Fact]
        public void Test_Insert_ICharSequence_BeyondCapacity_TracksMaxLength()
        {
            using var sb = new ValueStringBuilder(stackalloc char[5]);
            Assert.Equal(0, sb.Length);
            Assert.Equal(0, sb.MaxLength);

            sb.Insert(0, "012345678".AsCharSequence());
            Assert.Equal(9, sb.Length);
            Assert.Equal(9, sb.MaxLength);

            sb.Remove(1, 5);
            Assert.Equal(4, sb.Length);
            Assert.Equal(9, sb.MaxLength);

            sb.Append("012345678");
            Assert.Equal(13, sb.Length);
            Assert.Equal(13, sb.MaxLength);

            sb.Remove(3, 9);
            Assert.Equal(4, sb.Length);
            Assert.Equal(13, sb.MaxLength);
        }

#endif
#endif

        /**
         * @tests java.lang.StringBuilder.Insert(int, CharSequence)
         */
        [Fact]
        public void Test_Insert_StringBuilder()
        {
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[16]);
            try
            {
                const string fixture = "0000";

                sb.Append(fixture);
                sb.Insert(0, new StringBuilder("ab"));
                Assert.Equal("ab0000", sb.ToString());
                Assert.Equal(6, sb.Length);

                sb.Length = 0;
                sb.Append(fixture);
                sb.Insert(2, new StringBuilder("ab"));
                Assert.Equal("00ab00", sb.ToString());
                Assert.Equal(6, sb.Length);

                sb.Length = 0;
                sb.Append(fixture);
                sb.Insert(4, new StringBuilder("ab"));
                Assert.Equal("0000ab", sb.ToString());
                Assert.Equal(6, sb.Length);


                sb.Length = 0;
                sb.Append(fixture);
                sb.Insert(4, (StringBuilder?)null);
                //assertEquals("0000null", sb.ToString());
                //assertEquals(8, sb.Length);
                Assert.Equal("0000", sb.ToString()); // J2N: Changed behavior to make adding null a no-op to match .NET
                Assert.Equal(4, sb.Length);

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

#if FEATURE_VALUESTRINGBUILDER_INCLUDEMAXLENGTHTRACKING

        [Fact]
        public void Test_Insert_StringBuilder_BeyondCapacity_TracksMaxLength()
        {
            using var sb = new ValueStringBuilder(stackalloc char[5]);
            Assert.Equal(0, sb.Length);
            Assert.Equal(0, sb.MaxLength);

            sb.Insert(0, new StringBuilder("012345678"));
            Assert.Equal(9, sb.Length);
            Assert.Equal(9, sb.MaxLength);

            sb.Remove(1, 5);
            Assert.Equal(4, sb.Length);
            Assert.Equal(9, sb.MaxLength);

            sb.Append("012345678");
            Assert.Equal(13, sb.Length);
            Assert.Equal(13, sb.MaxLength);

            sb.Remove(3, 9);
            Assert.Equal(4, sb.Length);
            Assert.Equal(13, sb.MaxLength);
        }
#endif
    }
}
