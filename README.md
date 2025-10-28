SpanTools for .NET
========

[![GitHub](https://img.shields.io/github/license/NightOwl888/SpanTools)](https://github.com/NightOwl888/SpanTools/blob/main/LICENSE.txt)
[![GitHub Sponsors](https://img.shields.io/badge/-Sponsor-fafbfc?logo=GitHub%20Sponsors)](https://github.com/sponsors/NightOwl888)

This package provides Roslyn code generators to create a `ValueStringBuilder` ref struct in your project.

`ValueStringBuilder` is included in the BCL, but is an internal type and not available to users. This package allows you to include `ValueStringBuilder` along with extended support for `Insert operations, `IndexOf` operations, `LastIndexOf` operations, code points, string formating, max length tracking, and more.

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