// Based upon: https://github.com/dotnet/runtime/blob/v10.0.0-rc.2.25502.107/src/libraries/Common/src/System/Text/ValueStringBuilder.cs
// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE.txt file or at https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.Text;

namespace SpanTools
{
    internal static partial class Generators
    {
        public static string GenerateValueStringBuilder(ValueStringBuilderOptions options)
        {
            var sb = new StringBuilder();
            var cb = new CodeBuilder(sb);
            cb.WriteLine("// Licensed to the .NET Foundation under one or more agreements.");
            cb.WriteLine("// The .NET Foundation licenses this file to you under the MIT license.");
            cb.WriteLine();
            cb.WriteLine("using System;");
            cb.WriteLine("using System.Buffers;");
            cb.WriteLine("using System.Diagnostics;");
            cb.WriteLine("using System.Runtime.CompilerServices;");
            cb.WriteLine("using System.Runtime.InteropServices;");
            cb.WriteLine("using System.Text;");
            cb.WriteLine("#nullable enable");
            cb.WriteLine();
            WriteNamespace(cb, options);
            cb.WriteLine();
            cb.WriteLine("/// <summary>");
            cb.WriteLine("/// Represents a mutable set of characters and provides access to the underlying memory.");
            cb.WriteLine("/// </summary>");
            cb.WriteLine("/// <remarks>");
            cb.WriteLine("/// This is a ref struct that allocates on the stack and is intended primarily for advanced");
            cb.WriteLine("/// string building logic where it doesn't make sense to allocate a <see cref=\"StringBuilder\"/> instance");
            cb.WriteLine("/// and/or there are round trips between string appending and indexing. The <see cref=\"StringBuilder.this[Int32]\"/>");
            cb.WriteLine("/// property is known to have performance problems and the <c>StringBuilder.GetChunks()</c> method only helps");
            cb.WriteLine("/// in specific scenarios where it is sensible to traverse the chunks in order.");
            cb.WriteLine("/// <para/>");
            cb.WriteLine("/// The primary advantage of <see cref=\"ValueStringBuilder\"/> over <see cref=\"StringBuilder\"/> is that it can be");
            cb.WriteLine("/// initialized from the stack, which eliminates the cost of garbage collection. When that stack allocation overflows,");
            cb.WriteLine("/// it automatically switches to use the shared <see cref=\"ArrayPool{Char}\"/>. A typical stack allocation can be");
            cb.WriteLine("/// performed like:");
            cb.WriteLine("/// <code>");
            cb.WriteLine("/// ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[32]);");
            cb.WriteLine("/// </code>");
            cb.WriteLine("/// Note that in cases where the stack overflows, calling <see cref=\"Dispose()\"/> is required to return the memory");
            cb.WriteLine("/// to the array pool. As a best practice, we recommend always ensuring it is called.");
            cb.WriteLine("/// <para/>");
            cb.WriteLine("/// Rather than managing chunks of memory, <see cref=\"ValueStringBuilder\"/> manages a single contiguous block");
            cb.WriteLine("/// and each append or insert will act on that memory rather than creating new chunks. This makes these operations");
            cb.WriteLine("/// slower than <see cref=\"StringBuilder\"/>. However, in most cases the ability to shift the memory being acted upon");
            cb.WriteLine("/// from the managed heap to the stack provides performance benefits that more than offset that performance cost,");
            cb.WriteLine("/// especially when there are a small number of concatenations that don't even warrant creating a <see cref=\"StringBuilder\"/>");
            cb.WriteLine("/// instance.");
            cb.WriteLine("/// <para/>");
            cb.WriteLine("/// Indexing through <see cref=\"ValueStringBuilder\"/> is similar in performance to indexing through a <see cref=\"String\"/>");
            cb.WriteLine("/// without necessarily allocating a new <see cref=\"String\"/> on the heap.");
            cb.WriteLine("/// <para/>");
            cb.WriteLine("/// Furthermore, since it uses a contiguous block of memory, the underlying memory of <see cref=\"ValueStringBuilder\"/> can");
            cb.WriteLine("/// be passed directly into methods that accept <see cref=\"ReadOnlySpan{Char}\"/> by using the <see cref=\"AsSpan()\"/> method");
            cb.WriteLine("/// or one of its overloads.");
            cb.WriteLine("/// <code>");
            cb.WriteLine("/// ValueStringBuilder sb = new ValueStringBuilder(stackalloc char[32]);");
            cb.WriteLine("/// sb.Append('4');");
            cb.WriteLine("/// sb.Append('2');");
            cb.WriteLine("/// if (int.TryParse(sb.AsSpan(), out int value))");
            cb.WriteLine("///     Console.WriteLine($\"The value is {value}.\");");
            cb.WriteLine("/// else");
            cb.WriteLine("///     Console.WriteLine(\"The text failed to parse to an int.\");");
            cb.WriteLine("/// </code>");
            cb.WriteLine("/// </remarks>");
            cb.WriteLine("internal ref partial struct ValueStringBuilder");
            cb.WriteLine("{");
            cb.IndentBlock(() =>
            {
                cb.WriteLine("private const int CharStackBufferSize = 32;");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("private char[]? _arrayToReturnToPool;");
                cb.WriteLine("private Span<char> _chars;");
                cb.WriteLine("private int _pos;");
                cb.WriteLineIf(options.IncludeMaxLengthTracking, "private int _maxLength;");
                cb.WriteLineIf(options.IncludeMaxLengthTracking, "private bool _capacityExceeded;");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Initializes a new instance of the <see cref=\"ValueStringBuilder\"/> ref struct.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"initialBuffer\">The initial contents of this builder. Typically this is a new block of memory from the stack.</param>");
                cb.WriteLine("/// <remarks>If the <paramref name=\"initialBuffer\"/> overflows, a new allocation will occur from the shared <see cref=\"ArrayPool{Char}\"/>");
                cb.WriteLine("/// which will make <paramref name=\"initialBuffer\"/> stale.</remarks>");
                cb.WriteLine("public ValueStringBuilder(Span<char> initialBuffer)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("_arrayToReturnToPool = null;");
                    cb.WriteLine("_chars = initialBuffer;");
                    cb.WriteLine("_pos = 0;");
                    cb.WriteLineIf(options.IncludeMaxLengthTracking, "_maxLength = 0;");
                    cb.WriteLineIf(options.IncludeMaxLengthTracking, "_capacityExceeded = false;");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Initializes a new instance of the <see cref=\"ValueStringBuilder\"/> ref struct.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"initialCapacity\">The initial capacity of this builder.</param>");
                cb.WriteLine("/// <remarks>This constructor always allocates from the shared <see cref=\"ArrayPool{Char}\"/>.</remarks>");
                cb.WriteLine("public ValueStringBuilder(int initialCapacity)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("_arrayToReturnToPool = ArrayPool<char>.Shared.Rent(initialCapacity);");
                    cb.WriteLine("_chars = _arrayToReturnToPool;");
                    cb.WriteLine("_pos = 0;");
                    cb.WriteLineIf(options.IncludeMaxLengthTracking, "_maxLength = 0;");
                    cb.WriteLineIf(options.IncludeMaxLengthTracking, "_capacityExceeded = false;");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Gets or sets the length of this builder.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <value>The length of this instance.</value>");
                cb.WriteLine("public int Length");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("get => _pos;");
                    cb.WriteLine("set");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Debug.Assert(value >= 0);");
                        cb.WriteLine("Debug.Assert(value <= _chars.Length);");
                        cb.WriteLine("_pos = value;");
                        cb.WriteLineIf(options.IncludeMaxLengthTracking, "UpdateMaxLength();");
                    });
                    cb.WriteLine("}");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Gets the maximum number of characters that can be contained in the memory allocated by the current instance.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <value>The maximum number of characters that can be contained in the memory allocated by the current instance.</value>");
                cb.WriteLine("public int Capacity => _chars.Length;");
            });

            if (options.IncludeMaxLengthTracking)
            {
                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Gets a value indicating whether the initial buffer that was passed into the constructor is stale.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <value><c>true</c> if the value passed through the <see cref=\"ValueStringBuilder.ValueStringBuilder(Span{char})\"/>");
                    cb.WriteLine("/// constructor has been replaced with a chunk of memory from the shared <see cref=\"ArrayPool{Char}\"/>;");
                    cb.WriteLine("/// <c>false</c> if the initial buffer can still be utilized.</value>");
                    cb.WriteLine("public bool CapacityExceeded => _capacityExceeded;");
                });
                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// The maximum length that was reached during the lifetime of this instance.");
                    cb.WriteLine("/// This is the minimum buffer size required for the operation to succeed when");
                    cb.WriteLine("/// <see cref=\"CapacityExceeded\"/> is <c>true</c>.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("public int MaxLength => _maxLength;");
                });
                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                    cb.WriteLine("private void UpdateMaxLength()");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("if (_pos > _maxLength)");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("_maxLength = _pos;");
                        });
                    });
                    cb.WriteLine("}");
                });
            }

            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Ensures that the capacity of this instance of <see cref=\"ValueStringBuilder\"/> is at least the specified value.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"capacity\">The minimum capacity to ensure.</param>");
                cb.WriteLine("public void EnsureCapacity(int capacity)");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("=> EnsureCapacity(capacity, out _);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                cb.WriteLine("internal void EnsureCapacity(int capacity, out int newCapacity) // Internal for testing");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("// This is not expected to be called this with negative capacity");
                    cb.WriteLine("Debug.Assert(capacity >= 0);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("newCapacity = default;");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("// If the caller has a bug and calls this with negative capacity, make sure to call Grow to throw an exception.");
                    cb.WriteLine("if ((uint)capacity > (uint)_chars.Length)");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Grow(capacity - _pos, out newCapacity);");
                    });
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Get a pinnable reference to the builder.");
                cb.WriteLine("/// Does not ensure there is a null char after <see cref=\"Length\"/>");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("public ref char GetPinnableReference()");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("return ref MemoryMarshal.GetReference(_chars);");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Get a pinnable reference to the builder.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"terminate\">Ensures that the builder has a null char after <see cref=\"Length\"/></param>");
                cb.WriteLine("public ref char GetPinnableReference(bool terminate)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("if (terminate)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("EnsureCapacity(Length + 1);");
                        cb.WriteLine("_chars[Length] = '\\0';");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine("return ref MemoryMarshal.GetReference(_chars);");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Gets or sets the character at the specified character position in this instance.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"index\">The position of the character.</param>");
                cb.WriteLine("/// <returns>The Unicode character at position <paramref name=\"index\"/>.</returns>");
                cb.WriteLine("/// <exception cref=\"IndexOutOfRangeException\"><paramref name=\"index\"/> is outside the bounds");
                cb.WriteLine("/// of this instance while getting a character.</exception>");
                cb.WriteLine("/// <remarks><paramref name=\"index\"/> range checks are performed using <see cref=\"Debug.Assert(bool)\"/>.</remarks>");
                cb.WriteLine("[IndexerName(\"Chars\")]");
                cb.WriteLine("public ref char this[int index]");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("get");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Debug.Assert(index >= 0 && index < _pos);");
                        cb.WriteLine("return ref _chars[index];");
                    });
                    cb.WriteLine("}");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Converts the value of this instance to a <see cref=\"String\"/>.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <returns>A string whose value is the same as this instance.</returns>");
                cb.WriteLine("/// <remarks>To get a sliced string, use <see cref=\"AsSpan(int, int)\"/>.ToString().</remarks>");
                cb.WriteLine("// LUCENENET: Removed Dispose() call so we can safely call multiple times");
                cb.WriteLine("public override string ToString() => _chars.Slice(0, _pos).ToString();");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>Returns the underlying storage of the builder.</summary>");
                cb.WriteLine("public Span<char> RawChars => _chars;");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>Returns the pooled array or <c>null</c> if the stack is still in use.</summary>");
                cb.WriteLine("public char[]? RawArray => _arrayToReturnToPool;");
            });

            if (options.IncludeAsMemory)
            {

                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Returns a memory around the contents of the builder.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// NOTE: This can only be used if this instance is constructed using the <see cref=\"ValueStringBuilder(int)\"/>");
                    cb.WriteLine("/// or <see cref=\"ValueStringBuilder()\"/> constructors and you ensure that the returned value goes out of scope");
                    cb.WriteLine("/// prior to calling <see cref=\"Dispose()\"/>.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"terminate\">Ensures that the builder has a null char after <see cref=\"Length\"/></param>");
                    cb.WriteLine("public ReadOnlyMemory<char> AsMemory(bool terminate)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Debug.Assert(_arrayToReturnToPool != null, \"ValueStringBuilder must be constructed using ValueStringBuilder(int) or ValueStringBuilder() to use as memory.\");");
                        cb.WriteLine("if (terminate)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("EnsureCapacity(Length + 1);");
                            cb.WriteLine("_chars[Length] = '\\0';");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine("return _arrayToReturnToPool.AsMemory(0, _pos);");
                    });
                    cb.WriteLine("}");
                });
                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Returns a memory around the contents of the builder.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// NOTE: This can only be used if this instance is constructed using the <see cref=\"ValueStringBuilder(int)\"/>");
                    cb.WriteLine("/// or <see cref=\"ValueStringBuilder()\"/> constructors and you ensure that the returned value goes out of scope");
                    cb.WriteLine("/// prior to calling <see cref=\"Dispose()\"/>.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("public ReadOnlyMemory<char> AsMemory()");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Debug.Assert(_arrayToReturnToPool != null, \"ValueStringBuilder must be constructed using ValueStringBuilder(int) or ValueStringBuilder() to use as memory.\");");
                        cb.WriteLine("return _arrayToReturnToPool.AsMemory(0, _pos);");
                    });
                    cb.WriteLine("}");
                });
                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Returns a memory around the contents of the builder.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// NOTE: This can only be used if this instance is constructed using the <see cref=\"ValueStringBuilder(int)\"/>");
                    cb.WriteLine("/// or <see cref=\"ValueStringBuilder()\"/> constructors and you ensure that the returned value goes out of scope");
                    cb.WriteLine("/// prior to calling <see cref=\"Dispose()\"/>.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("public ReadOnlyMemory<char> AsMemory(int start)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Debug.Assert(_arrayToReturnToPool != null, \"ValueStringBuilder must be constructed using ValueStringBuilder(int) or ValueStringBuilder() to use as memory.\");");
                        cb.WriteLine("return _arrayToReturnToPool.AsMemory(start, _pos - start);");
                    });
                    cb.WriteLine("}");
                });
                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Returns a memory around the contents of the builder.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// NOTE: This can only be used if this instance is constructed using the <see cref=\"ValueStringBuilder(int)\"/>");
                    cb.WriteLine("/// or <see cref=\"ValueStringBuilder()\"/> constructors and you ensure that the returned value goes out of scope");
                    cb.WriteLine("/// prior to calling <see cref=\"Dispose()\"/>.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("public ReadOnlyMemory<char> AsMemory(int start, int length)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Debug.Assert(_arrayToReturnToPool != null, \"ValueStringBuilder must be constructed using ValueStringBuilder(int) or ValueStringBuilder() to use as memory.\");");
                        cb.WriteLine("return _arrayToReturnToPool.AsMemory(start, length);");
                    });
                    cb.WriteLine("}");
                });

            }

            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Creates a new readonly span around the contents of the builder.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"terminate\">Ensures that the builder has a null char after <see cref=\"Length\"/>.</param>");
                cb.WriteLine("/// <returns>The read-only span representation of the string.</returns>");
                cb.WriteLine("public ReadOnlySpan<char> AsSpan(bool terminate)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("if (terminate)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("EnsureCapacity(Length + 1);");
                        cb.WriteLine("_chars[Length] = '\\0';");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine("return _chars.Slice(0, _pos);");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Creates a new read-only span around the contents of the builder.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <returns>The read-only span representation of the string.</returns>");
                cb.WriteLine("public ReadOnlySpan<char> AsSpan() => _chars.Slice(0, _pos);");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Creates a new read-only span around a portion of the contents of the builder starting");
                cb.WriteLine("/// from a specified position to <see cref=\"Length\"/>.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"start\">The index at which to begin this slice.</param>");
                cb.WriteLine("/// <returns>The read-only span representation of the string.</returns>");
                cb.WriteLine("/// <remarks><paramref name=\"start\"/> range checks are performed using <see cref=\"Debug.Assert(bool)\"/>.</remarks>");
                cb.WriteLine("public ReadOnlySpan<char> AsSpan(int start)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Debug.Assert((uint)start <= (uint)_pos);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("return _chars.Slice(start, _pos - start);");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Creates a new read-only span around a portion of the contents of the builder starting");
                cb.WriteLine("/// from a specified position for a specified number of characters.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"start\">The index at which to begin this slice.</param>");
                cb.WriteLine("/// <param name=\"length\">The desired length for the slice.</param>");
                cb.WriteLine("/// <returns>The read-only span representation of the string.</returns>");
                cb.WriteLine("/// <remarks><paramref name=\"start\"/> and <paramref name=\"length\"/> range checks are performed");
                cb.WriteLine("/// using <see cref=\"Debug.Assert(bool)\"/>.</remarks>");
                cb.WriteLine("public ReadOnlySpan<char> AsSpan(int start, int length)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Debug.Assert((uint)start <= (uint)_pos);");
                    cb.WriteLine("Debug.Assert((uint)length <= (uint)(_pos - start));");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("return _chars.Slice(start, length);");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Copies the contents of this builder into the destination span.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"destination\">The span into which to copy this builder's contents.</param>");
                cb.WriteLine("/// <param name=\"charsWritten\">When this method returns, the number of characters that were written in destination.</param>");
                cb.WriteLine("/// <returns><c>true</c> if the data was copied; <c>false</c> if the destination was too short");
                cb.WriteLine("/// to fit the contents of the string.</returns>");
                cb.WriteLine("public bool TryCopyTo(Span<char> destination, out int charsWritten)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("if (_chars.Slice(0, _pos).TryCopyTo(destination))");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("charsWritten = _pos;");
                        cb.WriteLine("return true;");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine("else");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("charsWritten = 0;");
                        cb.WriteLine("return false;");
                    });
                    cb.WriteLine("}");
                });
                cb.WriteLine("}");
            });

            if (options.IncludeMaxLengthTracking)
            {
                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Checks whether the initial buffer that was passed to <see cref=\"ValueStringBuilder.ValueStringBuilder(Span{char})\"/>");
                    cb.WriteLine("/// is still valid.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"charsLength\">Upon successful return of this method, contains the number of characters written to the initial buffer;");
                    cb.WriteLine("/// otherwise, returns the minimum length of buffer required to make a second attempt.</param>");
                    cb.WriteLine("/// <returns><c>true</c> if the initial buffer is still valid; <c>false</c> if the initial buffer is stale.</returns>");
                    cb.WriteLine("public bool FitsInitialBuffer(out int charsLength) // TODO: Tests");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("if (_capacityExceeded)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("charsLength = _maxLength;");
                            cb.WriteLine("return false;");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine("else");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("charsLength = _pos;");
                            cb.WriteLine("return true;");
                        });
                        cb.WriteLine("}");
                    });
                    cb.WriteLine("}");
                });
            }

            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Inserts the string representation of a specified Unicode character into this instance at the specified character position.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"index\">The position in this instance where insertion begins.</param>");
                cb.WriteLine("/// <param name=\"value\">The value to insert.</param>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// <paramref name=\"index\"/> range checks are performed using <see cref=\"Debug.Assert(bool)\"/>.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// Existing characters are shifted to make room for the new text. The capacity of this instance is adjusted as needed.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("public void Insert(int index, char value)");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("=> Insert(index, ref value, 1);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Inserts one or more copies of a specified <see cref=\"char\"/> into this instance at the specified character position.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"index\">The position in this instance where insertion begins.</param>");
                cb.WriteLine("/// <param name=\"value\">The value to insert.</param>");
                cb.WriteLine("/// <param name=\"valueCount\">The number of times to insert <paramref name=\"value\"/>.</param>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// <paramref name=\"index\"/> and <paramref name=\"valueCount\"/> range checks are performed using <see cref=\"Debug.Assert(bool)\"/>.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// Existing characters are shifted to make room for the new text. The capacity of this instance is adjusted as needed.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// This <see cref=\"ValueStringBuilder\"/> instance is not changed if <paramref name=\"valueCount\"/> is 0.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("public void Insert(int index, char value, int valueCount)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("MakeRoom(index, valueCount);");
                    cb.WriteLine("_chars.Slice(index, valueCount).Fill(value);");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Inserts a string into this instance at the specified character position.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"index\">The position in this instance where insertion begins.</param>");
                cb.WriteLine("/// <param name=\"value\">The string to insert.</param>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// <paramref name=\"index\"/> range checks are performed using <see cref=\"Debug.Assert(bool)\"/>.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// Existing characters are shifted to make room for the new text. The capacity of this instance is adjusted as needed.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// This <see cref=\"ValueStringBuilder\"/> instance is not changed if <paramref name=\"value\"/> is <c>null</c>,");
                cb.WriteLine("/// or <paramref name=\"value\"/> is not <c>null</c> but its length is zero.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("public void Insert(int index, string? value)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("if (value != null)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Insert(index, ref MemoryMarshal.GetReference(value.AsSpan()), value.Length);");
                    });
                    cb.WriteLine("}");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Inserts the sequence of characters into this instance at the specified character position.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"index\">The position in this instance where insertion begins.</param>");
                cb.WriteLine("/// <param name=\"value\">The character span to insert.</param>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// <paramref name=\"index\"/> range checks are performed using <see cref=\"Debug.Assert(bool)\"/>.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// Existing characters are shifted to make room for the new text. The capacity of this instance is adjusted as needed.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                cb.WriteLine("public void Insert(int index, scoped ReadOnlySpan<char> value)");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("=> Insert(index, ref MemoryMarshal.GetReference(value), value.Length);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Inserts one or more copies of a specified string into this instance at the specified character position.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"index\">The position in this instance where insertion begins.</param>");
                cb.WriteLine("/// <param name=\"value\">The string to insert.</param>");
                cb.WriteLine("/// <param name=\"valueCount\">The number of times to insert <paramref name=\"value\"/>.</param>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// <paramref name=\"index\"/> and <paramref name=\"valueCount\"/> range checks are performed using <see cref=\"Debug.Assert(bool)\"/>.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// Existing characters are shifted to make room for the new text. The capacity of this instance is adjusted as needed.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// This StringBuilder object is not changed if <paramref name=\"value\"/> is <c>null</c>, <paramref name=\"value\"/> is");
                cb.WriteLine("/// not <c>null</c> but its length is zero, or <paramref name=\"valueCount\"/> is zero.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                cb.WriteLine("public void Insert(int index, string? value, int valueCount)");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("=> Insert(index, value.AsSpan(), valueCount);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Inserts one or more copies of a specified span into this instance at the specified character position.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"index\">The position in this instance where insertion begins.</param>");
                cb.WriteLine("/// <param name=\"value\">The span to insert.</param>");
                cb.WriteLine("/// <param name=\"valueCount\">The number of times to insert <paramref name=\"value\"/>.</param>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// <paramref name=\"index\"/> and <paramref name=\"valueCount\"/> range checks are performed using <see cref=\"Debug.Assert(bool)\"/>.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// Existing characters are shifted to make room for the new text. The capacity of this instance is adjusted as needed.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// This StringBuilder object is not changed if <paramref name=\"value\"/>'s length is zero or <paramref name=\"valueCount\"/> is zero.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("public void Insert(int index, scoped ReadOnlySpan<char> value, int valueCount)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Debug.Assert(valueCount >= 0);");
                    cb.WriteLine("Debug.Assert(index <= _pos);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("if (value.IsEmpty || valueCount == 0)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("return;");
                    });
                    cb.WriteLine("}");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("// Ensure we don't insert more chars than we can hold, and we don't");
                    cb.WriteLine("// have any integer overflow in our new length.");
                    cb.WriteLine("long insertingChars = (long)value.Length * valueCount;");
                    cb.WriteLine("Debug.Assert(insertingChars + _pos < int.MaxValue);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("MakeRoom(index, (int)insertingChars);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("int valueLength = value.Length;");
                    cb.WriteLine("while (valueCount > 0)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("ReplaceInPlace(ref index, ref MemoryMarshal.GetReference(value), valueLength);");
                        cb.WriteLine("--valueCount;");
                    });
                    cb.WriteLine("}");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Appends the string representation of a specified <see cref=\"char\"/> object to this instance.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">The UTF-16-encoded code unit to append.</param>");
                cb.WriteLine("/// <remarks>The capacity of this instance is adjusted as needed.</remarks>");
                cb.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                cb.WriteLine("public void Append(char value)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("int pos = _pos;");
                    cb.WriteLine("if ((uint)pos < (uint)_chars.Length)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("_chars[pos] = value;");
                        cb.WriteLine("_pos = pos + 1;");
                        cb.WriteLineIf(options.IncludeMaxLengthTracking, "UpdateMaxLength();");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine("else");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("GrowAndAppend(value);");
                    });
                    cb.WriteLine("}");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Appends a copy of the specified string to this instance.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">The string to append.</param>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// If <paramref name=\"value\"/> is <c>null</c>, no changes are made.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// The capacity of this instance is adjusted as needed.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                cb.WriteLine("public void Append(string? value)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("if (value == null)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("return;");
                    });
                    cb.WriteLine("}");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("int pos = _pos;");
                    cb.WriteLine("if (value.Length == 1 && (uint)pos < (uint)_chars.Length) // very common case, e.g. appending strings from NumberFormatInfo like separators, percent symbols, etc.");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("_chars[pos] = value[0];");
                        cb.WriteLine("_pos = pos + 1;");
                        cb.WriteLineIf(options.IncludeMaxLengthTracking, "UpdateMaxLength();");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine("else");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("AppendSlow(value);");
                    });
                    cb.WriteLine("}");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("private void AppendSlow(string s)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("int pos = _pos;");
                    cb.WriteLine("if (pos > _chars.Length - s.Length)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Grow(s.Length);");
                    });
                    cb.WriteLine("}");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("s");
                });
            });
            cb.WriteLine("#if !NET5_0_OR_GREATER");
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine(".AsSpan()");
                    });
                });
            });
            cb.WriteLine("#endif");
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine(".CopyTo(_chars.Slice(pos));");
                    });
                    cb.WriteLine("_pos += s.Length;");
                    cb.WriteLineIf(options.IncludeMaxLengthTracking, "UpdateMaxLength();");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Appends a specified number of copies of the string representation of a Unicode");
                cb.WriteLine("/// character to this instance.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">The character to append.</param>");
                cb.WriteLine("/// <param name=\"valueCount\">The number of times to append <paramref name=\"value\"/>.</param>");
                cb.WriteLine("/// <remarks>");
                cb.WriteLine("/// <paramref name=\"valueCount\"/> range checks are performed using <see cref=\"Debug.Assert(bool)\"/>.");
                cb.WriteLine("/// <para/>");
                cb.WriteLine("/// The capacity of this instance is adjusted as needed.");
                cb.WriteLine("/// </remarks>");
                cb.WriteLine("public void Append(char value, int valueCount)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Debug.Assert(valueCount >= 0);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("if (_pos > _chars.Length - valueCount)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Grow(valueCount);");
                    });
                    cb.WriteLine("}");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Span<char> dst = _chars.Slice(_pos, valueCount);");
                    cb.WriteLine("for (int i = 0; i < dst.Length; i++)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("dst[i] = value;");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine("_pos += valueCount;");
                    cb.WriteLineIf(options.IncludeMaxLengthTracking, "UpdateMaxLength();");
                });
                cb.WriteLine("}");
            });

            if (options.AllowUnsafeBlocks)
            {
                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("/// <summary>");
                    cb.WriteLine("/// Appends an array of Unicode characters starting at a specified address to this instance.");
                    cb.WriteLine("/// </summary>");
                    cb.WriteLine("/// <param name=\"value\">A pointer to an array of characters.</param>");
                    cb.WriteLine("/// <param name=\"length\">The number of characters in the array.</param>");
                    cb.WriteLine("/// <remarks>");
                    cb.WriteLine("/// <paramref name=\"value\"/> null check and <paramref name=\"length\"/> range check are performed using <see cref=\"Debug.Assert(bool)\"/>.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// This method appends <paramref name=\"length\"/> characters to the current instance starting at the address <paramref name=\"value\"/>.");
                    cb.WriteLine("/// <para/>");
                    cb.WriteLine("/// The capacity of this instance is adjusted as needed.");
                    cb.WriteLine("/// </remarks>");
                    cb.WriteLine("public unsafe void Append(char* value, int length)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Debug.Assert(value != null);");
                        cb.WriteLine("Debug.Assert(length >= 0);");
                    });
                });
                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("int pos = _pos;");
                        cb.WriteLine("if (pos > _chars.Length - length)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("Grow(length);");
                        });
                        cb.WriteLine("}");
                    });
                });
                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Span<char> dst = _chars.Slice(_pos, length);");
                        cb.WriteLine("for (int i = 0; i < dst.Length; i++)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("dst[i] = *value++;");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine("_pos += length;");
                        cb.WriteLineIf(options.IncludeMaxLengthTracking, "UpdateMaxLength();");
                    });
                    cb.WriteLine("}");
                });
            }


            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Appends the string representation of a specified read-only character span to this instance.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"value\">The read-only character span to append.</param>");
                cb.WriteLine("/// <remarks>The capacity of this instance is adjusted as needed.</remarks>");
                cb.WriteLine("public void Append(scoped ReadOnlySpan<char> value)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("int pos = _pos;");
                    cb.WriteLine("if (pos > _chars.Length - value.Length)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Grow(value.Length);");
                    });
                    cb.WriteLine("}");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("value.CopyTo(_chars.Slice(_pos));");
                    cb.WriteLine("_pos += value.Length;");
                    cb.WriteLineIf(options.IncludeMaxLengthTracking, "UpdateMaxLength();");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Appends and returns a writable <see cref=\"Span{Char}\"/> of the specified length to this builder.");
                cb.WriteLine("/// Writes to the returned span update the value of this instance.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"length\">The number of characters to append to this instance.</param>");
                cb.WriteLine("/// <returns>A <see cref=\"Span{Char}\"/> wrapping a block of memory that is appended to the existing");
                cb.WriteLine("/// sequence of characters. The span may be written to by the caller to update this instance.</returns>");
                cb.WriteLine("/// <remarks><paramref name=\"length\"/> range checking is performed by <see cref=\"Debug.Assert(bool)\"/>.</remarks>");
                cb.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                cb.WriteLine("public Span<char> AppendSpan(int length)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Debug.Assert(length >= 0);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("int origPos = _pos;");
                    cb.WriteLine("if (origPos > _chars.Length - length)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("Grow(length);");
                    });
                    cb.WriteLine("}");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("_pos = origPos + length;");
                    cb.WriteLineIf(options.IncludeMaxLengthTracking, "UpdateMaxLength();");
                    cb.WriteLine("return _chars.Slice(origPos, length);");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("[MethodImpl(MethodImplOptions.NoInlining)]");
                cb.WriteLine("private void GrowAndAppend(char c)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Grow(1);");
                    cb.WriteLine("Append(c);");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Resize the internal buffer either by doubling current buffer size or");
                cb.WriteLine("/// by adding <paramref name=\"additionalCapacityBeyondPos\"/> to");
                cb.WriteLine("/// <see cref=\"_pos\"/> whichever is greater.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"additionalCapacityBeyondPos\">");
                cb.WriteLine("/// Number of chars requested beyond current position.");
                cb.WriteLine("/// </param>");
                cb.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                cb.WriteLine("private void Grow(int additionalCapacityBeyondPos)");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("=> Grow(additionalCapacityBeyondPos, out _);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("[MethodImpl(MethodImplOptions.NoInlining)]");
                cb.WriteLine("private void Grow(int additionalCapacityBeyondPos, out int newCapacity)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Debug.Assert(additionalCapacityBeyondPos > 0);");
                    cb.WriteLine("Debug.Assert(_pos > _chars.Length - additionalCapacityBeyondPos, \"Grow called incorrectly, no resize is needed.\");");
                });
            });

            if (options.IncludeMaxLengthTracking)
            {
                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("_capacityExceeded = true;");
                    });
                });
            }
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("newCapacity = CalculateNewArrayLength(additionalCapacityBeyondPos);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("// Make sure to let Rent throw an exception if the caller has a bug and the desired capacity is negative");
                    cb.WriteLine("char[] poolArray = ArrayPool<char>.Shared.Rent(newCapacity);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("_chars.Slice(0, _pos).CopyTo(poolArray);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("char[]? toReturn = _arrayToReturnToPool;");
                    cb.WriteLine("_chars = _arrayToReturnToPool = poolArray;");
                    cb.WriteLine("if (toReturn != null)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("ArrayPool<char>.Shared.Return(toReturn);");
                    });
                    cb.WriteLine("}");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                cb.WriteLine("private int CalculateNewArrayLength(int additionalCapacityBeyondPos)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("const uint ArrayMaxLength = 0x7FFFFFC7; // same as Array.MaxLength");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("// Increase to at least the required size (m_Position + additionalCapacityBeyondPos), but try");
                    cb.WriteLine("// to double the size if possible, bounding the doubling to not go beyond the max array length.");
                    cb.WriteLine("int newCapacity = (int)Math.Max(");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("(uint)(_pos + additionalCapacityBeyondPos),");
                        cb.WriteLine("Math.Min((uint)_chars.Length * 2, ArrayMaxLength));");
                    });
                    cb.WriteLine("return newCapacity;");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("private void MakeRoom(int index, int count)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Debug.Assert(count > 0);");
                    cb.WriteLine("Debug.Assert(index >= 0);");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("// Cool, we have some space in this block, and we don't have to copy much to get at it, so go ahead and use it.");
                    cb.WriteLine("// This typically happens when someone repeatedly inserts small strings at a spot (usually the absolute front) of the buffer.");
                    cb.WriteLine("if (_chars.Length - _pos >= count)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("for (int i = _pos; i > index;)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("--i;");
                            cb.WriteLine("_chars[i + count] = _chars[i];");
                        });
                        cb.WriteLine("}");
                        cb.WriteLine("_pos += count;");
                        cb.WriteLineIf(options.IncludeMaxLengthTracking, "UpdateMaxLength();");
                        cb.WriteLine("return;");
                    });
                    cb.WriteLine("}");
                });
            });
            if (options.IncludeMaxLengthTracking)
            {
                cb.WriteLine();
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("_capacityExceeded = true;");
                    });
                });
            }
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("// Make sure to let Rent throw an exception if the caller has a bug and the desired capacity is negative");
                    cb.WriteLine("char[] newArray = ArrayPool<char>.Shared.Rent(CalculateNewArrayLength(count));");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("if (_pos > 0)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("// Copy the head of the current buffer to the new buffer.");
                        cb.WriteLine("int copyCount1 = index; //Math.Min(count, index);");
                        cb.WriteLine("if (copyCount1 > 0)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("_chars.Slice(0, copyCount1).CopyTo(newArray);");
                        });
                        cb.WriteLine("}");
                    });
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("// Copy the tail of the current buffer to the new buffer, leaving a gap of length count.");
                        cb.WriteLine("int copyCount2 = copyCount1 + count;");
                        cb.WriteLine("if (copyCount2 > 0)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("_chars.Slice(copyCount1, _pos - copyCount1).CopyTo(newArray.AsSpan(copyCount2));");
                        });
                        cb.WriteLine("}");
                    });
                    cb.WriteLine("}");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("// Wire in the new array");
                    cb.WriteLine("char[]? toReturn = _arrayToReturnToPool;");
                    cb.WriteLine("_chars = _arrayToReturnToPool = newArray;");
                    cb.WriteLine("if (toReturn != null)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("ArrayPool<char>.Shared.Return(toReturn);");
                    });
                    cb.WriteLine("}");
                    cb.WriteLine("_pos += count;");
                    cb.WriteLineIf(options.IncludeMaxLengthTracking, "UpdateMaxLength();");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("private void ReplaceInPlace(ref int index, ref char value, int count)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("if (count != 0)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("while (true)");
                        cb.WriteLine("{");
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("int length = _pos - index;");
                            cb.WriteLine("Debug.Assert(length >= 0, \"Index isn't in the array.\");");
                        });
                    });
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("int lengthToCopy = Math.Min(length, count);");
                        });
                    });
                });
            });
            cb.WriteLine("#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER");
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("MemoryMarshal.CreateSpan(ref value, lengthToCopy).CopyTo(_chars.Slice(index));");
                        });
                    });
                });
            });
            cb.WriteLine("#else");

            if (options.AllowUnsafeBlocks)
            {
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.IndentBlock(() =>
                        {
                            cb.IndentBlock(() =>
                            {
                                cb.WriteLine("unsafe");
                                cb.WriteLine("{");
                                cb.IndentBlock(() =>
                                {
                                    cb.WriteLine("fixed (char* pSource = &value)");
                                    cb.WriteLine("{");
                                    cb.IndentBlock(() =>
                                    {
                                        cb.WriteLine("new Span<char>(pSource, lengthToCopy).CopyTo(_chars.Slice(index));");
                                    });
                                    cb.WriteLine("}");
                                });
                                cb.WriteLine("}");
                            });
                        });
                    });
                });
            }
            else
            {
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.IndentBlock(() =>
                        {
                            cb.IndentBlock(() =>
                            {
                                cb.WriteLine("int i = 0;");
                                cb.WriteLine("for (; i + 7 < lengthToCopy; i += 8)");
                                cb.WriteLine("{");
                                cb.IndentBlock(() =>
                                {
                                    cb.WriteLine("_chars[index + i] = Unsafe.Add(ref value, i);");
                                    cb.WriteLine("_chars[index + i + 1] = Unsafe.Add(ref value, i + 1);");
                                    cb.WriteLine("_chars[index + i + 2] = Unsafe.Add(ref value, i + 2);");
                                    cb.WriteLine("_chars[index + i + 3] = Unsafe.Add(ref value, i + 3);");
                                    cb.WriteLine("_chars[index + i + 4] = Unsafe.Add(ref value, i + 4);");
                                    cb.WriteLine("_chars[index + i + 5] = Unsafe.Add(ref value, i + 5);");
                                    cb.WriteLine("_chars[index + i + 6] = Unsafe.Add(ref value, i + 6);");
                                    cb.WriteLine("_chars[index + i + 7] = Unsafe.Add(ref value, i + 7);");
                                });
                                cb.WriteLine("}");
                                cb.WriteLine("for (; i < lengthToCopy; i++) _chars[index + i] = Unsafe.Add(ref value, i);");
                            });
                        });
                    });
                });
            }

            cb.WriteLine("#endif");

            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.IndentBlock(() =>
                    {
                        cb.IndentBlock(() =>
                        {
                            cb.WriteLine("// Advance the index.");
                            cb.WriteLine("index += lengthToCopy;");
                            cb.WriteLine("count -= lengthToCopy;");
                            cb.WriteLine("if (count == 0)");
                            cb.WriteLine("{");
                            cb.IndentBlock(() =>
                            {
                                cb.WriteLine("break;");
                            });
                            cb.WriteLine("}");
                            cb.WriteLine("value = ref Unsafe.Add(ref value, lengthToCopy);");
                        });
                        cb.WriteLine("}");
                    });
                    cb.WriteLine("}");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Inserts a character buffer into this builder at the specified position.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <param name=\"index\">The index to insert in this builder.</param>");
                cb.WriteLine("/// <param name=\"value\">The reference to the start of the buffer.</param>");
                cb.WriteLine("/// <param name=\"valueCount\">The number of characters in the buffer.</param>");
                cb.WriteLine("private void Insert(int index, ref char value, int valueCount)");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("Debug.Assert((uint)index <= (uint)_pos, \"Callers should check that index is a legal value.\");");
                });
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("if (valueCount > 0)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("MakeRoom(index, valueCount);");
                        cb.WriteLine("ReplaceInPlace(ref index, ref value, valueCount);");
                    });
                    cb.WriteLine("}");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine();
            cb.IndentBlock(() =>
            {
                cb.WriteLine("/// <summary>");
                cb.WriteLine("/// Returns any rented memory to the shared <see cref=\"ArrayPool{Char}\"/>.");
                cb.WriteLine("/// </summary>");
                cb.WriteLine("/// <remarks>As a best practice, this method should be called with a using block or in a finally block to ensure");
                cb.WriteLine("/// there are no array pool leaks, especially if it is unknown whether the initial buffer is large enough or there");
                cb.WriteLine("/// is a chance of an exception during the string building operation.</remarks>");
                cb.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                cb.WriteLine("public void Dispose()");
                cb.WriteLine("{");
                cb.IndentBlock(() =>
                {
                    cb.WriteLine("char[]? toReturn = _arrayToReturnToPool;");
                    cb.WriteLine("this = default; // for safety, to avoid using pooled array if this instance is erroneously appended to again");
                    cb.WriteLine("if (toReturn != null)");
                    cb.WriteLine("{");
                    cb.IndentBlock(() =>
                    {
                        cb.WriteLine("ArrayPool<char>.Shared.Return(toReturn);");
                    });
                    cb.WriteLine("}");
                });
                cb.WriteLine("}");
            });
            cb.WriteLine("}");

            return sb.ToString();
        }
    }
}
