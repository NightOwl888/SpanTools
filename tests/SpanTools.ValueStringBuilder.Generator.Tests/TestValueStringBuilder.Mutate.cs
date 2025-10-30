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
        [TestCase("", 0, 0, "")]
        [TestCase("Hello", 0, 5, "")]
        [TestCase("Hello", 1, 3, "Ho")]
        [TestCase("Hello", 1, 4, "H")]
        [TestCase("Hello", 1, 0, "Hello")]
        [TestCase("Hello", 5, 0, "Hello")]
        public static void Remove(string value, int startIndex, int length, string expected)
        {
            using var builder = new ValueStringBuilder(stackalloc char[64]);
            builder.Append(value);
            builder.Remove(startIndex, length);
            Assert.AreEqual(expected, builder.ToString());
        }

        [Test]
        public void Remove_BeyondCapacity_TracksMaxLength()
        {
            using var sb = new ValueStringBuilder(stackalloc char[5]);
            Assert.AreEqual(0, sb.Length);
            Assert.AreEqual(0, sb.MaxLength);

            sb.Append("012345678");
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
         * @tests java.lang.StringBuilder.delete(int, int)
         */
        [Test]
        public void Test_DeleteII()
        {
            string fixture = "0123456789";
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[32]);
            try
            {
                sb.Append(fixture);
                sb.Delete(0, 0 - 0); // J2N: Corrected 2nd parameter
                Assert.AreEqual(fixture, sb.AsSpan().ToString());
                sb.Delete(5, 5 - 5); // J2N: Corrected 2nd parameter
                Assert.AreEqual(fixture, sb.AsSpan().ToString());
                sb.Delete(0, 1 - 0); // J2N: Corrected 2nd parameter
                Assert.AreEqual("123456789", sb.AsSpan().ToString());
                Assert.AreEqual(9, sb.Length);
                sb.Delete(0, sb.Length - 0); // J2N: Corrected 2nd parameter
                Assert.AreEqual("", sb.AsSpan().ToString());
                Assert.AreEqual(0, sb.Length);

                //sb = new StringBuilder(fixture);
                sb.Length = 0;
                sb.Append(fixture);
                sb.Delete(0, 11 - 0); // J2N: Corrected 2nd parameter
                Assert.AreEqual("", sb.AsSpan().ToString());
                Assert.AreEqual(0, sb.Length);

                //try
                //{
                //    new StringBuilder(fixture).Delete(-1, 2 - -1); // J2N: Corrected 2nd parameter
                //    fail("no SIOOBE, negative start");
                //}
                //catch (ArgumentOutOfRangeException e)
                //{
                //    // Expected
                //}

                //try
                //{
                //    new StringBuilder(fixture).Delete(11, 12 - 11); // J2N: Corrected 2nd parameter
                //    fail("no SIOOBE, start too far");
                //}
                //catch (ArgumentOutOfRangeException e)
                //{
                //    // Expected
                //}

                //try
                //{
                //    new StringBuilder(fixture).Delete(13, 12 - 13); // J2N: Corrected 2nd parameter
                //    fail("no SIOOBE, start larger than end");
                //}
                //catch (ArgumentOutOfRangeException e)
                //{
                //    // Expected
                //}

                // HARMONY 6212
                //sb = new StringBuilder();
                sb.Length = 0;
                sb.Append("abcde");
                String str = sb.AsSpan().ToString();
                sb.Delete(0, sb.Length - 0); // J2N: Corrected 2nd parameter
                sb.Append("YY");
                Assert.AreEqual("abcde", str);
                Assert.AreEqual("YY", sb.AsSpan().ToString());
            }
            finally
            {
                sb.Dispose();
            }
        }

        private void reverseTest(String org, String rev, String back)
        {
            // create non-shared StringBuilder
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[32]);
            try
            {
                sb.Append(org);
                sb.Reverse();
                String reversed = sb.AsSpan().ToString();
                Assert.AreEqual(rev, reversed);
                // create non-shared StringBuilder
                //sb = new ValueStringBuilder(reversed);
                sb.Length = 0;
                sb.Append(reversed);
                sb.Reverse();
                reversed = sb.AsSpan().ToString();
                Assert.AreEqual(back, reversed);

                // test algorithm when StringBuilder is shared
                //sb = new StringBuilder(org);
                sb.Length = 0;
                sb.Append(org);
                String copy = sb.AsSpan().ToString();
                Assert.AreEqual(org, copy);
                sb.Reverse();
                reversed = sb.AsSpan().ToString();
                Assert.AreEqual(rev, reversed);
                //sb = new StringBuilder(reversed);
                sb.Length = 0;
                sb.Append(reversed);
                copy = sb.AsSpan().ToString();
                Assert.AreEqual(rev, copy);
                sb.Reverse();
                reversed = sb.AsSpan().ToString();
                Assert.AreEqual(back, reversed);
            }
            finally
            {
                sb.Dispose();
            }
        }

        /**
         * @tests java.lang.StringBuilder.reverse()
         */
        [Test]
        public void Test_Reverse()
        {
            string fixture = "0123456789";
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[32]);
            try
            {
                sb.Append(fixture);
                sb.Reverse();
                Assert.AreEqual("9876543210", sb.AsSpan().ToString());

                sb.Length = 0;
                sb.Append("012345678");
                sb.Reverse();
                Assert.AreEqual("876543210", sb.AsSpan().ToString());

                sb.Length = (1);
                sb.Reverse();
                Assert.AreEqual("8", sb.AsSpan().ToString());

                sb.Length = (0);
                sb.Reverse();
                Assert.AreEqual("", sb.AsSpan().ToString());
                sb.Dispose();

                String str;
                str = "a";
                reverseTest(str, str, str);

                str = "ab";
                reverseTest(str, "ba", str);

                str = "abcdef";
                reverseTest(str, "fedcba", str);

                str = "abcdefg";
                reverseTest(str, "gfedcba", str);

                str = "\ud800\udc00";
                reverseTest(str, str, str);

                str = "\udc00\ud800";
                reverseTest(str, "\ud800\udc00", "\ud800\udc00");

                str = "a\ud800\udc00";
                reverseTest(str, "\ud800\udc00a", str);

                str = "ab\ud800\udc00";
                reverseTest(str, "\ud800\udc00ba", str);

                str = "abc\ud800\udc00";
                reverseTest(str, "\ud800\udc00cba", str);

                str = "\ud800\udc00\udc01\ud801\ud802\udc02";
                reverseTest(str, "\ud802\udc02\ud801\udc01\ud800\udc00",
                        "\ud800\udc00\ud801\udc01\ud802\udc02");

                str = "\ud800\udc00\ud801\udc01\ud802\udc02";
                reverseTest(str, "\ud802\udc02\ud801\udc01\ud800\udc00", str);

                str = "\ud800\udc00\udc01\ud801a";
                reverseTest(str, "a\ud801\udc01\ud800\udc00",
                        "\ud800\udc00\ud801\udc01a");

                str = "a\ud800\udc00\ud801\udc01";
                reverseTest(str, "\ud801\udc01\ud800\udc00a", str);

                str = "\ud800\udc00\udc01\ud801ab";
                reverseTest(str, "ba\ud801\udc01\ud800\udc00",
                        "\ud800\udc00\ud801\udc01ab");

                str = "ab\ud800\udc00\ud801\udc01";
                reverseTest(str, "\ud801\udc01\ud800\udc00ba", str);

                str = "\ud800\udc00\ud801\udc01";
                reverseTest(str, "\ud801\udc01\ud800\udc00", str);

                str = "a\ud800\udc00z\ud801\udc01";
                reverseTest(str, "\ud801\udc01z\ud800\udc00a", str);

                str = "a\ud800\udc00bz\ud801\udc01";
                reverseTest(str, "\ud801\udc01zb\ud800\udc00a", str);

                str = "abc\ud802\udc02\ud801\udc01\ud800\udc00";
                reverseTest(str, "\ud800\udc00\ud801\udc01\ud802\udc02cba", str);

                str = "abcd\ud802\udc02\ud801\udc01\ud800\udc00";
                reverseTest(str, "\ud800\udc00\ud801\udc01\ud802\udc02dcba", str);
            }
            finally
            {
                sb.Dispose();
            }
        }

        /**
         * @tests java.lang.StringBuilder.replace(int, int, String)'
         */
        [Test]
        public void Test_Replace_String()
        {
            string fixture = "0000";
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[64]);
            try
            {
                sb.Append(fixture);
                //Assert.AreSame(sb, sb.Replace(1, 3 - 1, "11")); // J2N; Corrected 2nd parameter
                sb.Replace(1, 3 - 1, "11"); // J2N; Corrected 2nd parameter
                Assert.AreEqual("0110", sb.AsSpan().ToString());
                Assert.AreEqual(4, sb.Length);

                //sb = new StringBuilder(fixture);
                sb.Length = 0;
                sb.Append(fixture);
                //Assert.AreSame(sb, );
                sb.Replace(1, 2 - 1, "11"); // J2N; Corrected 2nd parameter
                Assert.AreEqual("01100", sb.AsSpan().ToString());
                Assert.AreEqual(5, sb.Length);

                //sb = new StringBuilder(fixture);
                sb.Length = 0;
                sb.Append(fixture);
                //Assert.AreSame(sb, );
                sb.Replace(4, 5 - 4, "11"); // J2N; Corrected 2nd parameter
                Assert.AreEqual("000011", sb.AsSpan().ToString());
                Assert.AreEqual(6, sb.Length);

                //sb = new StringBuilder(fixture);
                sb.Length = 0;
                sb.Append(fixture);
                //Assert.AreSame(sb, ); 
                sb.Replace(4, 6 - 4, "11"); // J2N; Corrected 2nd parameter
                Assert.AreEqual("000011", sb.AsSpan().ToString());
                Assert.AreEqual(6, sb.Length);

                //// FIXME Undocumented NPE in Sun's JRE 5.0_5
                //try
                //{
                //    sb.Replace(1, 2 - 1, null); // J2N; Corrected 2nd parameter
                //    Assert.Fail("No NPE");
                //}
                //catch (ArgumentNullException e)
                //{
                //    // Expected
                //}

                //try
                //{
                //    //sb = new StringBuilder(fixture);
                //    sb.Length = 0;
                //    sb.Append(fixture);
                //    sb.Replace(-1, 2 - -1, "11"); // J2N; Corrected 2nd parameter
                //    Assert.Fail("No SIOOBE, negative start");
                //}
                //catch (ArgumentOutOfRangeException e)
                //{
                //    // Expected
                //}

                //try
                //{
                //    //sb = new StringBuilder(fixture);
                //    sb.Length = 0;
                //    sb.Append(fixture);
                //    sb.Replace(5, 2 - 5, "11"); // J2N; Corrected 2nd parameter
                //    Assert.Fail("No SIOOBE, start > length");
                //}
                //catch (ArgumentOutOfRangeException e)
                //{
                //    // Expected
                //}

                //try
                //{
                //    //sb = new StringBuilder(fixture);
                //    sb.Length = 0;
                //    sb.Append(fixture);
                //    sb.Replace(3, 2 - 3, "11"); // J2N; Corrected 2nd parameter
                //    Assert.Fail("No SIOOBE, start > end");
                //}
                //catch (ArgumentOutOfRangeException e)
                //{
                //    // Expected
                //}
            }
            finally
            {
                sb.Dispose();
            }

            // Regression for HARMONY-348
            using ValueStringBuilder buffer = new ValueStringBuilder(stackalloc char[8]);
            buffer.Append("1234567");
            buffer.Replace(2, 6 - 2, "XXX"); // J2N; Corrected 2nd parameter
            Assert.AreEqual("12XXX7", buffer.ToString());
        }


        /**
         * @tests java.lang.StringBuilder.replace(int, int, String)'
         */
        [Test]
        public void Test_Replace_ReadOnlySpan()
        {
            string fixture = "0000";
            ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[64]);
            try
            {
                sb.Append(fixture);
                //Assert.AreSame(sb, );
                sb.Replace(1, 3 - 1, "11".AsSpan()); // J2N; Corrected 2nd parameter
                Assert.AreEqual("0110", sb.AsSpan().ToString());
                Assert.AreEqual(4, sb.Length);

                sb.Length = 0;
                sb.Append(fixture);
                //Assert.AreSame(sb, );
                sb.Replace(1, 2 - 1, "11".AsSpan()); // J2N; Corrected 2nd parameter
                Assert.AreEqual("01100", sb.AsSpan().ToString());
                Assert.AreEqual(5, sb.Length);

                sb.Length = 0;
                sb.Append(fixture);
                //Assert.AreSame(sb, );
                sb.Replace(4, 5 - 4, "11".AsSpan()); // J2N; Corrected 2nd parameter
                Assert.AreEqual("000011", sb.AsSpan().ToString());
                Assert.AreEqual(6, sb.Length);

                sb.Length = 0;
                sb.Append(fixture);
                //Assert.AreSame(sb, );
                sb.Replace(4, 6 - 4, "11".AsSpan()); // J2N; Corrected 2nd parameter
                Assert.AreEqual("000011", sb.AsSpan().ToString());
                Assert.AreEqual(6, sb.Length);

                ////// FIXME Undocumented NPE in Sun's JRE 5.0_5
                ////try
                ////{
                ////    sb.Replace(1, 2 - 1, null); // J2N; Corrected 2nd parameter
                ////    fail("No NPE");
                ////}
                ////catch (ArgumentNullException e)
                ////{
                ////    // Expected
                ////}

                //try
                //{
                //    sb.Length = 0;
                //    sb.Append(fixture);
                //    sb.Replace(-1, 2 - -1, "11".AsSpan()); // J2N; Corrected 2nd parameter
                //    Assert.Fail("No SIOOBE, negative start");
                //}
                //catch (ArgumentOutOfRangeException e)
                //{
                //    // Expected
                //}

                //try
                //{
                //    sb.Length = 0;
                //    sb.Append(fixture);
                //    sb.Replace(5, 2 - 5, "11".AsSpan()); // J2N; Corrected 2nd parameter
                //    Assert.Fail("No SIOOBE, start > length");
                //}
                //catch (ArgumentOutOfRangeException e)
                //{
                //    // Expected
                //}

                //try
                //{
                //    sb.Length = 0;
                //    sb.Append(fixture);
                //    sb.Replace(3, 2 - 3, "11".AsSpan()); // J2N; Corrected 2nd parameter
                //    Assert.Fail("No SIOOBE, start > end");
                //}
                //catch (ArgumentOutOfRangeException e)
                //{
                //    // Expected
                //}
            }
            finally
            {
                sb.Dispose();
            }

            // Regression for HARMONY-348
            using ValueStringBuilder buffer = new ValueStringBuilder(stackalloc char[8]);
            buffer.Append("1234567");
            buffer.Replace(2, 6 - 2, "XXX".AsSpan()); // J2N; Corrected 2nd parameter
            Assert.AreEqual("12XXX7", buffer.AsSpan().ToString());
        }
    }
}
