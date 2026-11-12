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
| `PennForgeTech.Core.Caching` | `ICacheService` and `MemoryCacheService` with per-entry expiry, driven by `IClock`. |
| `PennForgeTech.Core.Events` | `IEventPublisher`, `IEventListener<TEvent>` and `InMemoryEventBus`. Listeners receive events of the exact published type. |
| `PennForgeTech.Core.Exceptions` | `PennForgeTechException`, the base type for the library's own exceptions. |
| `PennForgeTech.Core.Security` | `PasswordHasher` (PBKDF2), `HashHelper` (SHA-256) and `SecureRandom`. |
| `PennForgeTech.Core.Utilities` | String and sequence helpers and `Base64UrlEncoder`. |
| `PennForgeTech.Core.Validation` | `Validator<TInstance>` rules, `ValidationResult`, `ValidationException`, and conversion to `Result`. |
| `PennForgeTech.Core.Serialization` | `ISerializer` and `JsonTextSerializer` (System.Text.Json, camelCase, string enums). |

## Code style

Enforced by `.editorconfig` and by building with warnings as errors:

- No `var`; types are always written out.
- No expression-bodied members or other shorthand; braces and block bodies everywhere.
- Namespaces use braces (block-scoped), and comments are kept to a minimum.
- Instance members are always accessed with `this.`.

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
