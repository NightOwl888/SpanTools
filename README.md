SpanTools for .NET
========

[![GitHub](https://img.shields.io/github/license/NightOwl888/SpanTools)](https://github.com/NightOwl888/SpanTools/blob/main/LICENSE.txt)
[![GitHub Sponsors](https://img.shields.io/badge/-Sponsor-fafbfc?logo=GitHub%20Sponsors)](https://github.com/sponsors/NightOwl888)

This package provides Roslyn code generators to create a `ValueStringBuilder` ref struct in your project.

`ValueStringBuilder` is included in the BCL, but is an internal type and not available to users. This package allows you to include `ValueStringBuilder` along with extended support for `Insert` operations, `IndexOf` operations, `LastIndexOf` operations, code points, string formatting, max length tracking, and more.

## Configuration

The following options are user configurable via MSBuild properties.

| Option                                                   | Default Value                       | Description                                                            |
|----------------------------------------------------------|-------------------------------------|------------------------------------------------------------------------|
| **ValueStringBuilder_Namespace**                         | `RootNamespace` ?? `AssemblyName`   | Sets the namespace of the `ValueStringBuilder` ref struct. Use the value `global::` to specify the global namespace.             |
| **ValueStringBuilder_IncludeMaxLengthTracking**          | `false`                             | Enables the `MaxLength`, `CapacityExceeded`, and `FitsInitialBuffer()` members. This can be used to write directly to the Span<char> that is passed to the constructor. FitsInitalBuffer() returns the number of characters written or the minimum length of buffer required to re-attempt the operation. |
| **ValueStringBuilder_IncludeAsMemoryMethods**            | `false`                             | Enables `AsMemory()` method overloads. These can be used to get a `ReadOnlyMemory<char>` to pass as fields of heap-only types such as classes if (and only if) used in conjunction with the `ValueStringBuilder(int initialCapacity)` constructor. The consumer is responsible for ensuring the returned `ReadOnlyMemory<char>` goes out of scope prior to calling `ValueStringBuilder.Dispose()`, when the memory used will be returned to the array pool. |
| **ValueStringBuilder_IncludesJ2N**                       | `false`                             | Indicates that [J2N](https://www.nuget.org/packages/J2N) is available as a package dependency. Generally, this option does not have to be set because J2N will be detected automatically if part of the dependency graph. |
| **ValueStringBuilder_IncludeCodepointSupport**           | `true`                              | If J2N is referenced as a `PackageReference`, this enables the `AppendCodePoint()`, `InsertCodePoint()`, `CodePointAt()`, `CodePointBefore()`, `CodePointCount()`, and `OffsetByCodePoints()` methods. If J2N is not referenced, this has no effect. |
| **ValueStringBuilder_UseJavaStyleFormatting**            | `false`                             | If J2N is referenced as a `PackageReference`, this changes the default behavior of the `Append()` overloads that accept numeric types to use the invariant culture and `J` format to emulate Java-style floating point number formatting. If J2N is not referenced, this has no effect. |

### Why a Code Generator instead of a Library?

Distributing as a code generator has the following advantages and disadvantages.

#### Advantages

1. No NuGet dependencies are shipped with your NuGet package. The code is included within your assembly.
2. Shipping as a partial class allows you to extend it directly and manipulate the private state of the type directly with your extensions.

#### Disadvantages

1. Users don't have a way to upgrade the generated code if there are updates. To upgrade, you must deploy a new version.


------------------

## NuGet

[![Nuget](https://img.shields.io/nuget/dt/SpanTools.ValueStringBuilder.Generator)](https://www.nuget.org/packages/SpanTools.ValueStringBuilder.Generator)

------------------

## Saying Thanks

If you find these libraries to be useful, please star us [on GitHub](https://github.com/NightOwl888/SpanTools) and consider a sponsorship so we can continue bringing you great free tools like these. It really would make a big difference!

[![GitHub Sponsors](https://img.shields.io/badge/-Sponsor-fafbfc?logo=GitHub%20Sponsors)](https://github.com/sponsors/NightOwl888)