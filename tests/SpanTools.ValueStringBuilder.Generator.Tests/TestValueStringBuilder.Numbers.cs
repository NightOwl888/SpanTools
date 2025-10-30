// Source: https://github.com/dotnet/runtime/blob/v10.0.0-rc.2.25502.107/src/libraries/System.Runtime/tests/System.Runtime.Tests/System/Text/StringBuilderTests.cs
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MyNamespace;
using SpanTools.TestUtilities;
using SpanTools.TestUtilities.Xunit;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SpanTools.Generator.Tests
{
    public partial class ValueStringBuilderTests
    {
        // Append

        public static IEnumerable<object[]> Append_Decimal_TestData()
        {
            yield return new object[] { "Hello", (double)0, "Hello0" };
            yield return new object[] { "Hello", 1.23, "Hello1.23" };
            yield return new object[] { "", -4.56, "-4.56" };
        }

        [Fact]
        public static void Test_Append_Decimal()
        {
            using (new ThreadCultureChange(CultureInfo.InvariantCulture))
            {
                foreach (var testdata in Append_Decimal_TestData())
                {
                    Append_Decimal((string)testdata[0], (double)testdata[1], (string)testdata[2]);
                }
            }
        }

        private static void Append_Decimal(string original, double doubleValue, string expected)
        {
            using var builder = new ValueStringBuilder(stackalloc char[16]);
            builder.Append(original);
            builder.Append(new decimal(doubleValue));
            Assert.Equal(expected, builder.ToString());
        }

        //[Fact]
        //public static void Append_Decimal_NoSpareCapacity_ThrowsArgumentOutOfRangeException()
        //{
        //    var builder = new StringBuilder(0, 5);
        //    builder.Append("Hello");

        //    AssertExtensions.Throws<ArgumentOutOfRangeException>(s_noCapacityParamName, () => builder.Append((decimal)1));
        //}
        public static IEnumerable<object[]> Append_Double_TestData()
        {
#if FEATURE_VALUESTRINGBUILDER_USEJAVASTYLEFORMATTING
            yield return new object[] { "Hello", (double)0, "Hello0.0" };
            yield return new object[] { "Hello", (double)-0d, "Hello-0.0" };
#else
            yield return new object[] { "Hello", (double)0, "Hello0" };
#endif
            yield return new object[] { "Hello", 1.23, "Hello1.23" };
            yield return new object[] { "", -4.56, "-4.56" };
        }

        [Fact]
        public static void Test_Append_Double()
        {
            using (new ThreadCultureChange(CultureInfo.InvariantCulture))
            {
                foreach (var testdata in Append_Double_TestData())
                {
                    Append_Double((string)testdata[0], (double)testdata[1], (string)testdata[2]);
                }
            }
        }

        private static void Append_Double(string original, double value, string expected)
        {
            using var builder = new ValueStringBuilder(stackalloc char[16]);
            builder.Append(original);
            builder.Append(value);
            Assert.Equal(expected, builder.ToString());
        }

        //[Fact]
        //public static void Append_Double_NoSpareCapacity_ThrowsArgumentOutOfRangeException()
        //{
        //    var builder = new StringBuilder(0, 5);
        //    builder.Append("Hello");

        //    AssertExtensions.Throws<ArgumentOutOfRangeException>(s_noCapacityParamName, () => builder.Append((double)1));
        //}

        [Theory]
        [InlineData("Hello", (short)0, "Hello0")]
        [InlineData("Hello", (short)123, "Hello123")]
        [InlineData("", (short)-456, "-456")]
        public static void Append_Short(string original, short value, string expected)
        {
            using var builder = new ValueStringBuilder(stackalloc char[16]);
            builder.Append(original);
            builder.Append(value);
            Assert.Equal(expected, builder.ToString());
        }

        //[Fact]
        //public static void Append_Short_NoSpareCapacity_ThrowsArgumentOutOfRangeException()
        //{
        //    var builder = new StringBuilder(0, 5);
        //    builder.Append("Hello");

        //    AssertExtensions.Throws<ArgumentOutOfRangeException>(s_noCapacityParamName, () => builder.Append((short)1));
        //}

        [Theory]
        [InlineData("Hello", 0, "Hello0")]
        [InlineData("Hello", 123, "Hello123")]
        [InlineData("", -456, "-456")]
        public static void Append_Int(string original, int value, string expected)
        {
            using var builder = new ValueStringBuilder(stackalloc char[16]);
            builder.Append(original);
            builder.Append(value);
            Assert.Equal(expected, builder.ToString());
        }

        //[Fact]
        //public static void Append_Int_NoSpareCapacity_ThrowsArgumentOutOfRangeException()
        //{
        //    var builder = new StringBuilder(0, 5);
        //    builder.Append("Hello");

        //    AssertExtensions.Throws<ArgumentOutOfRangeException>(s_noCapacityParamName, () => builder.Append(1));
        //}

        [Theory]
        [InlineData("Hello", (long)0, "Hello0")]
        [InlineData("Hello", (long)123, "Hello123")]
        [InlineData("", (long)-456, "-456")]
        public static void Append_Long(string original, long value, string expected)
        {
            using var builder = new ValueStringBuilder(stackalloc char[16]);
            builder.Append(original);
            builder.Append(value);
            Assert.Equal(expected, builder.ToString());
        }

        //[Fact]
        //public static void Append_Long_NoSpareCapacity_ThrowsArgumentOutOfRangeException()
        //{
        //    var builder = new StringBuilder(0, 5);
        //    builder.Append("Hello");

        //    AssertExtensions.Throws<ArgumentOutOfRangeException>(s_noCapacityParamName, () => builder.Append((long)1));
        //}

        //[Theory]
        //[InlineData("Hello", "abc", "Helloabc")]
        //[InlineData("Hello", "def", "Hellodef")]
        //[InlineData("", "g", "g")]
        //[InlineData("Hello", "", "Hello")]
        //[InlineData("Hello", null, "Hello")]
        //public static void Append_Object(string original, object? value, string expected)
        //{
        //    var builder = new StringBuilder(original);
        //    builder.Append(value);
        //    Assert.Equal(expected, builder.ToString());
        //}

        //[Fact]
        //public static void Append_Object_NoSpareCapacity_ThrowsArgumentOutOfRangeException()
        //{
        //    var builder = new StringBuilder(0, 5);
        //    builder.Append("Hello");

        //    AssertExtensions.Throws<ArgumentOutOfRangeException>(s_noCapacityParamName, () => builder.Append(new object()));
        //}

        [Theory]
        [InlineData("Hello", (sbyte)0, "Hello0")]
        [InlineData("Hello", (sbyte)123, "Hello123")]
        [InlineData("", (sbyte)-123, "-123")]
        public static void Append_SByte(string original, sbyte value, string expected)
        {
            using var builder = new ValueStringBuilder(stackalloc char[16]);
            builder.Append(original);
            builder.Append(value);
            Assert.Equal(expected, builder.ToString());
        }

        //[Fact]
        //public static void Append_SByte_NoSpareCapacity_ThrowsArgumentOutOfRangeException()
        //{
        //    var builder = new StringBuilder(0, 5);
        //    builder.Append("Hello");

        //    AssertExtensions.Throws<ArgumentOutOfRangeException>(s_noCapacityParamName, () => builder.Append((sbyte)1));
        //}

        public static IEnumerable<object[]> Append_Float_TestData()
        {
#if FEATURE_VALUESTRINGBUILDER_USEJAVASTYLEFORMATTING
            yield return new object[] { "Hello", (float)0, "Hello0.0" };
            yield return new object[] { "Hello", (float)-0f, "Hello-0.0" };
#else
            yield return new object[] { "Hello", (float)0, "Hello0" };
#endif
            yield return new object[] { "Hello", (float)1.23, "Hello1.23" };
            yield return new object[] { "", (float)-4.56, "-4.56" };
        }

        [Fact]
        public static void Test_Append_Float()
        {
            using (new ThreadCultureChange(CultureInfo.InvariantCulture))
            {
                foreach (var testdata in Append_Float_TestData())
                {
                    Append_Float((string)testdata[0], (float)testdata[1], (string)testdata[2]);
                }
            }
        }

        private static void Append_Float(string original, float value, string expected)
        {
            using var builder = new ValueStringBuilder(stackalloc char[16]);
            builder.Append(original);
            builder.Append(value);
            Assert.Equal(expected, builder.ToString());
        }

        //[Fact]
        //public static void Append_Float_NoSpareCapacity_ThrowsArgumentOutOfRangeException()
        //{
        //    var builder = new StringBuilder(0, 5);
        //    builder.Append("Hello");

        //    AssertExtensions.Throws<ArgumentOutOfRangeException>(s_noCapacityParamName, () => builder.Append((float)1));
        //}

        [Theory]
        [InlineData("Hello", (byte)0, "Hello0")]
        [InlineData("Hello", (byte)123, "Hello123")]
        [InlineData("", (byte)123, "123")]
        public static void Append_Byte(string original, byte value, string expected)
        {
            using var builder = new ValueStringBuilder(stackalloc char[16]);
            builder.Append(original);
            builder.Append(value);
            Assert.Equal(expected, builder.ToString());
        }

        //[Fact]
        //public static void Append_Byte_NoSpareCapacity_ThrowsArgumentOutOfRangeException()
        //{
        //    var builder = new StringBuilder(0, 5);
        //    builder.Append("Hello");

        //    AssertExtensions.Throws<ArgumentOutOfRangeException>(s_noCapacityParamName, () => builder.Append((byte)1));
        //}

        [Theory]
        [InlineData("Hello", (uint)0, "Hello0")]
        [InlineData("Hello", (uint)123, "Hello123")]
        [InlineData("", (uint)456, "456")]
        public static void Append_UInt(string original, uint value, string expected)
        {
            using var builder = new ValueStringBuilder(stackalloc char[16]);
            builder.Append(original);
            builder.Append(value);
            Assert.Equal(expected, builder.ToString());
        }

        //[Fact]
        //public static void Append_UInt_NoSpareCapacity_ThrowsArgumentOutOfRangeException()
        //{
        //    var builder = new StringBuilder(0, 5);
        //    builder.Append("Hello");

        //    AssertExtensions.Throws<ArgumentOutOfRangeException>(s_noCapacityParamName, () => builder.Append((uint)1));
        //}

        [Theory]
        [InlineData("Hello", (ulong)0, "Hello0")]
        [InlineData("Hello", (ulong)123, "Hello123")]
        [InlineData("", (ulong)456, "456")]
        public static void Append_ULong(string original, ulong value, string expected)
        {
            using var builder = new ValueStringBuilder(stackalloc char[16]);
            builder.Append(original);
            builder.Append(value);
            Assert.Equal(expected, builder.ToString());
        }

        //[Fact]
        //public static void Append_ULong_NoSpareCapacity_ThrowsArgumentOutOfRangeException()
        //{
        //    var builder = new StringBuilder(0, 5);
        //    builder.Append("Hello");

        //    AssertExtensions.Throws<ArgumentOutOfRangeException>(s_noCapacityParamName, () => builder.Append((ulong)1));
        //}

        [Theory]
        [InlineData("Hello", (ushort)0, "Hello0")]
        [InlineData("Hello", (ushort)123, "Hello123")]
        [InlineData("", (ushort)456, "456")]
        public static void Append_UShort(string original, ushort value, string expected)
        {
            using var builder = new ValueStringBuilder(stackalloc char[16]);
            builder.Append(original);
            builder.Append(value);
            Assert.Equal(expected, builder.ToString());
        }

        //[Fact]
        //public static void Append_UShort_NoSpareCapacity_ThrowsArgumentOutOfRangeException()
        //{
        //    var builder = new StringBuilder(0, 5);
        //    builder.Append("Hello");

        //    AssertExtensions.Throws<ArgumentOutOfRangeException>(s_noCapacityParamName, () => builder.Append((ushort)1));
        //}




        // Insert

        [Theory]
        [InlineData("Hello", 0, (uint)0, "0Hello")]
        [InlineData("Hello", 3, (uint)123, "Hel123lo")]
        [InlineData("Hello", 5, (uint)456, "Hello456")]
        public static void Insert_UInt(string original, int index, uint value, string expected)
        {
            using var builder = new ValueStringBuilder(stackalloc char[16]);
            builder.Append(original);
            builder.Insert(index, value);
            Assert.Equal(expected, builder.ToString());
        }

        //[Fact]
        //public static void Insert_UInt_Invalid()
        //{
        //    var builder = new StringBuilder(0, 5);
        //    builder.Append("Hello");

        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(-1, (uint)1)); // Index < 0
        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(builder.Length + 1, (uint)1)); // Index > builder.Length
        //    Assert.Throws<OutOfMemoryException>(() => builder.Insert(builder.Length, (uint)1)); // New length > builder.MaxCapacity
        //}

        //[Theory]
        //[InlineData("Hello", 0, true, "TrueHello")]
        //[InlineData("Hello", 3, false, "HelFalselo")]
        //[InlineData("Hello", 5, false, "HelloFalse")]
        //public static void Insert_Bool(string original, int index, bool value, string expected)
        //{
        //    var builder = new StringBuilder(original);
        //    builder.Insert(index, value);
        //    Assert.Equal(expected, builder.ToString());
        //}

        //[Fact]
        //public static void Insert_Bool_Invalid()
        //{
        //    var builder = new StringBuilder(0, 5);
        //    builder.Append("Hello");

        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(-1, true)); // Index < 0
        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(builder.Length + 1, true)); // Index > builder.Length
        //    Assert.Throws<OutOfMemoryException>(() => builder.Insert(builder.Length, true)); // New length > builder.MaxCapacity
        //}

        [Theory]
        [InlineData("Hello", 0, (byte)0, "0Hello")]
        [InlineData("Hello", 3, (byte)123, "Hel123lo")]
        [InlineData("Hello", 5, (byte)123, "Hello123")]
        public static void Insert_Byte(string original, int index, byte value, string expected)
        {
            using var builder = new ValueStringBuilder(stackalloc char[16]);
            builder.Append(original);
            builder.Insert(index, value);
            Assert.Equal(expected, builder.ToString());
        }

        //[Fact]
        //public static void Insert_Byte_Invalid()
        //{
        //    var builder = new StringBuilder(0, 5);
        //    builder.Append("Hello");

        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(-1, (byte)1)); // Index < 0
        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(builder.Length + 1, (byte)1)); // Index > builder.Length
        //    Assert.Throws<OutOfMemoryException>(() => builder.Insert(builder.Length, (byte)1)); // New length > builder.MaxCapacity
        //}

        [Theory]
        [InlineData("Hello", 0, (ulong)0, "0Hello")]
        [InlineData("Hello", 3, (ulong)123, "Hel123lo")]
        [InlineData("Hello", 5, (ulong)456, "Hello456")]
        public static void Insert_ULong(string original, int index, ulong value, string expected)
        {
            using var builder = new ValueStringBuilder(stackalloc char[16]);
            builder.Append(original);
            builder.Insert(index, value);
            Assert.Equal(expected, builder.ToString());
        }

        //[Fact]
        //public static void Insert_ULong_Invalid()
        //{
        //    var builder = new StringBuilder(0, 5);
        //    builder.Append("Hello");

        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(-1, (ulong)1)); // Index < 0
        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(builder.Length + 1, (ulong)1)); // Index > builder.Length
        //    Assert.Throws<OutOfMemoryException>(() => builder.Insert(builder.Length, (ulong)1)); // New length > builder.MaxCapacity
        //}

        [Theory]
        [InlineData("Hello", 0, (ushort)0, "0Hello")]
        [InlineData("Hello", 3, (ushort)123, "Hel123lo")]
        [InlineData("Hello", 5, (ushort)456, "Hello456")]
        public static void Insert_UShort(string original, int index, ushort value, string expected)
        {
            using var builder = new ValueStringBuilder(stackalloc char[16]);
            builder.Append(original);
            builder.Insert(index, value);
            Assert.Equal(expected, builder.ToString());
        }

        //[Fact]
        //public static void Insert_UShort_Invalid()
        //{
        //    var builder = new StringBuilder(0, 5);
        //    builder.Append("Hello");

        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(-1, (ushort)1)); // Index < 0
        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(builder.Length + 1, (ushort)1)); // Index > builder.Length
        //    Assert.Throws<OutOfMemoryException>(() => builder.Insert(builder.Length, (ushort)1)); // New length > builder.MaxCapacity
        //}

        //[Theory]
        //[InlineData("Hello", 0, '\0', "\0Hello")]
        //[InlineData("Hello", 3, 'a', "Helalo")]
        //[InlineData("Hello", 5, 'b', "Hellob")]
        //public static void Insert_Char(string original, int index, char value, string expected)
        //{
        //    var builder = new StringBuilder(original);
        //    builder.Insert(index, value);
        //    Assert.Equal(expected, builder.ToString());
        //}

        //[Fact]
        //public static void Insert_Char_Invalid()
        //{
        //    var builder = new StringBuilder(0, 5);
        //    builder.Append("Hello");

        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(-1, '\0')); // Index < 0
        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(builder.Length + 1, '\0')); // Index > builder.Length
        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("requiredLength", () => builder.Insert(builder.Length, '\0')); // New length > builder.MaxCapacity
        //}

        public static IEnumerable<object[]> Insert_Float_TestData()
        {
#if FEATURE_VALUESTRINGBUILDER_USEJAVASTYLEFORMATTING
            yield return new object[] { "Hello", 0, (float)0, "0.0Hello" };
            yield return new object[] { "Hello", 0, (float)-0f, "-0.0Hello" };
#else
            yield return new object[] { "Hello", 0, (float)0, "0Hello" };
#endif
            yield return new object[] { "Hello", 3, (float)1.23, "Hel1.23lo" };
            yield return new object[] { "Hello", 5, (float)-4.56, "Hello-4.56" };
        }

        [Fact]
        public static void Test_Insert_Float()
        {
            using (new ThreadCultureChange(CultureInfo.InvariantCulture))
            {
                foreach (var testdata in Insert_Float_TestData())
                {
                    Insert_Float((string)testdata[0], (int)testdata[1], (float)testdata[2], (string)testdata[3]);
                }
            }
        }

        private static void Insert_Float(string original, int index, float value, string expected)
        {
            using var builder = new ValueStringBuilder(stackalloc char[16]);
            builder.Append(original);
            builder.Insert(index, value);
            Assert.Equal(expected, builder.ToString());
        }

        //[Fact]
        //public static void Insert_Float_Invalid()
        //{
        //    var builder = new StringBuilder(0, 5);
        //    builder.Append("Hello");

        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(-1, (float)1)); // Index < 0
        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(builder.Length + 1, (float)1)); // Index > builder.Length
        //    Assert.Throws<OutOfMemoryException>(() => builder.Insert(builder.Length, (float)1)); // New length > builder.MaxCapacity
        //}

        //[Theory]
        //[InlineData("Hello", 0, "\0", "\0Hello")]
        //[InlineData("Hello", 3, "abc", "Helabclo")]
        //[InlineData("Hello", 5, "def", "Hellodef")]
        //[InlineData("Hello", 0, "", "Hello")]
        //[InlineData("Hello", 0, null, "Hello")]
        //public static void Insert_Object(string original, int index, object? value, string expected)
        //{
        //    var builder = new StringBuilder(original);
        //    builder.Insert(index, value);
        //    Assert.Equal(expected, builder.ToString());
        //}

        //[Fact]
        //public static void Insert_Object_Invalid()
        //{
        //    var builder = new StringBuilder(0, 5);
        //    builder.Append("Hello");

        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(-1, new object())); // Index < 0
        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(builder.Length + 1, new object())); // Index > builder.Length
        //    Assert.Throws<OutOfMemoryException>(() => builder.Insert(builder.Length, new object())); // New length > builder.MaxCapacity
        //}

        [Theory]
        [InlineData("Hello", 0, (long)0, "0Hello")]
        [InlineData("Hello", 3, (long)123, "Hel123lo")]
        [InlineData("Hello", 5, (long)-456, "Hello-456")]
        public static void Insert_Long(string original, int index, long value, string expected)
        {
            using var builder = new ValueStringBuilder(stackalloc char[16]);
            builder.Append(original);
            builder.Insert(index, value);
            Assert.Equal(expected, builder.ToString());
        }

        //[Fact]
        //public static void Insert_Long_Invalid()
        //{
        //    var builder = new StringBuilder(0, 5);
        //    builder.Append("Hello");

        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(-1, (long)1)); // Index < 0
        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(builder.Length + 1, (long)1)); // Index > builder.Length
        //    Assert.Throws<OutOfMemoryException>(() => builder.Insert(builder.Length, (long)1)); // New length > builder.MaxCapacity
        //}

        [Theory]
        [InlineData("Hello", 0, 0, "0Hello")]
        [InlineData("Hello", 3, 123, "Hel123lo")]
        [InlineData("Hello", 5, -456, "Hello-456")]
        public static void Insert_Int(string original, int index, int value, string expected)
        {
            using var builder = new ValueStringBuilder(stackalloc char[16]);
            builder.Append(original);
            builder.Insert(index, value);
            Assert.Equal(expected, builder.ToString());
        }

        //[Fact]
        //public static void Insert_Int_Invalid()
        //{
        //    var builder = new StringBuilder(0, 5);
        //    builder.Append("Hello");

        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(-1, 1)); // Index < 0
        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(builder.Length + 1, 1)); // Index > builder.Length
        //    Assert.Throws<OutOfMemoryException>(() => builder.Insert(builder.Length, 1)); // New length > builder.MaxCapacity
        //}

        [Theory]
        [InlineData("Hello", 0, (short)0, "0Hello")]
        [InlineData("Hello", 3, (short)123, "Hel123lo")]
        [InlineData("Hello", 5, (short)-456, "Hello-456")]
        public static void Insert_Short(string original, int index, short value, string expected)
        {
            using var builder = new ValueStringBuilder(stackalloc char[16]);
            builder.Append(original);
            builder.Insert(index, value);
            Assert.Equal(expected, builder.ToString());
        }

        //[Fact]
        //public static void Insert_Short_Invalid()
        //{
        //    var builder = new StringBuilder(0, 5);
        //    builder.Append("Hello");

        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(-1, (short)1)); // Index < 0
        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(builder.Length + 1, (short)1)); // Index > builder.Length
        //    Assert.Throws<OutOfMemoryException>(() => builder.Insert(builder.Length, (short)1)); // New length > builder.MaxCapacity
        //}

        public static IEnumerable<object[]> Insert_Double_TestData()
        {
#if FEATURE_VALUESTRINGBUILDER_USEJAVASTYLEFORMATTING
            yield return new object[] { "Hello", 0, (double)0, "0.0Hello" };
            yield return new object[] { "Hello", 0, (double)-0d, "-0.0Hello" };
#else
            yield return new object[] { "Hello", 0, (double)0, "0Hello" };
#endif
            yield return new object[] { "Hello", 3, 1.23, "Hel1.23lo" };
            yield return new object[] { "Hello", 5, -4.56, "Hello-4.56" };
        }

        [Fact]
        public static void Test_Insert_Double()
        {
            using (new ThreadCultureChange(CultureInfo.InvariantCulture))
            {
                foreach (var testdata in Insert_Double_TestData())
                {
                    Insert_Double((string)testdata[0], (int)testdata[1], (double)testdata[2], (string)testdata[3]);
                }
            }
        }

        private static void Insert_Double(string original, int index, double value, string expected)
        {
            using var builder = new ValueStringBuilder(stackalloc char[16]);
            builder.Append(original);
            builder.Insert(index, value);
            Assert.Equal(expected, builder.ToString());
        }

        //[Fact]
        //public static void Insert_Double_Invalid()
        //{
        //    var builder = new StringBuilder(0, 5);
        //    builder.Append("Hello");

        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(-1, (double)1)); // Index < 0
        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(builder.Length + 1, (double)1)); // Index > builder.Length
        //    Assert.Throws<OutOfMemoryException>(() => builder.Insert(builder.Length, (double)1)); // New length > builder.MaxCapacity
        //}

        public static IEnumerable<object[]> Test_Insert_Decimal_TestData()
        {
            yield return new object[] { "Hello", 0, (double)0, "0Hello" };
            yield return new object[] { "Hello", 3, 1.23, "Hel1.23lo" };
            yield return new object[] { "Hello", 5, -4.56, "Hello-4.56" };
        }

        [Fact]
        public static void Test_Insert_Decimal()
        {
            using (new ThreadCultureChange(CultureInfo.InvariantCulture))
            {
                foreach (var testdata in Test_Insert_Decimal_TestData())
                {
                    Insert_Decimal((string)testdata[0], (int)testdata[1], (double)testdata[2], (string)testdata[3]);
                }
            }
        }

        private static void Insert_Decimal(string original, int index, double doubleValue, string expected)
        {
            using var builder = new ValueStringBuilder(stackalloc char[16]);
            builder.Append(original);
            builder.Insert(index, new decimal(doubleValue));
            Assert.Equal(expected, builder.ToString());
        }

        //[Fact]
        //public static void Insert_Decimal_Invalid()
        //{
        //    var builder = new StringBuilder(0, 5);
        //    builder.Append("Hello");

        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(-1, (decimal)1)); // Index < 0
        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(builder.Length + 1, (decimal)1)); // Index > builder.Length
        //    Assert.Throws<OutOfMemoryException>(() => builder.Insert(builder.Length, (decimal)1)); // New length > builder.MaxCapacity
        //}

        [Theory]
        [InlineData("Hello", 0, (sbyte)0, "0Hello")]
        [InlineData("Hello", 3, (sbyte)123, "Hel123lo")]
        [InlineData("Hello", 5, (sbyte)-123, "Hello-123")]
        public static void Insert_SByte(string original, int index, sbyte value, string expected)
        {
            using var builder = new ValueStringBuilder(stackalloc char[16]);
            builder.Append(original);
            builder.Insert(index, value);
            Assert.Equal(expected, builder.ToString());
        }

        //[Fact]
        //public static void Insert_SByte_Invalid()
        //{
        //    var builder = new StringBuilder(0, 5);
        //    builder.Append("Hello");

        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(-1, (sbyte)1)); // Index < 0
        //    AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => builder.Insert(builder.Length + 1, (sbyte)1)); // Index > builder.Length
        //    Assert.Throws<OutOfMemoryException>(() => builder.Insert(builder.Length, (sbyte)1)); // New length > builder.MaxCapacity
        //}
    }
}
