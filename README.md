# PennForgeTech.Core

Shared foundation library for all PennForgeTech projects. Written in C# for .NET 8.

Copyright (c) Eric Heasley

## What is included

| Namespace | Purpose |
| --- | --- |
| `PennForgeTech.Core.Guards` | `Guard` methods that validate arguments and return the checked value. |
| `PennForgeTech.Core.Results` | `Result`, `Result<TValue>` and `ResultError` for expected failures without exceptions. |
| `PennForgeTech.Core.Time` | `IClock`, `SystemClock` and `FixedClock` so time can be controlled in tests. |
| `PennForgeTech.Core.Paging` | `PageRequest`, `PagedResult<TItem>` and `ToPagedResult` for paging sequences. |
| `PennForgeTech.Core.Domain` | `Entity<TId>` base class with identity-based equality. |

## Code style

Enforced by `.editorconfig` and by building with warnings as errors:

- No `var`; types are always written out.
- No expression-bodied members or other shorthand; braces and block bodies everywhere.
- Instance members are always accessed with `this.`.
- Every public type and member has an XML documentation comment.

## Build and test

```bash
dotnet build
dotnet test
```

## Using it from another project

```bash
dotnet add reference path/to/PennForgeTech.Core/src/PennForgeTech.Core/PennForgeTech.Core.csproj
```

Or pack it with `dotnet pack src/PennForgeTech.Core -c Release` and publish the `.nupkg` to a private feed.
