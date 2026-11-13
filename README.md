# PennForgeTech.Core

Modular .NET 8 foundation with optional application framework packages. Each project under `src` is an independently packable NuGet library.

Copyright (c) Eric Heasley

## Packages

| Group | Package | Contents |
| --- | --- | --- |
| Foundation | `PennForgeTech.Core.Primitives` | `Guard`, `PageRequest`, `PagedResult<TItem>`, `Entity<TId>`, `Base64UrlEncoder` |
| Foundation | `PennForgeTech.Core.Abstractions` | `IClock`, `SystemClock`, `FixedClock`, `IAppLogger`, `LogLevel`, `ICacheService`, `IEventPublisher`, `IEventListener<TEvent>`, `ISerializer` |
| Foundation | `PennForgeTech.Core.Results` | `Result`, `Result<TValue>`, `ResultError` |
| Foundation | `PennForgeTech.Core.Exceptions` | `PennForgeException` (carries an error code) |
| Extensions | `PennForgeTech.Core.Logging` | `ConsoleAppLogger`, `InMemoryAppLogger`, `NullAppLogger` |
| Extensions | `PennForgeTech.Core.Caching` | `MemoryCacheService` with per-entry expiry |
| Extensions | `PennForgeTech.Core.Events` | `InMemoryEventBus` |
| Extensions | `PennForgeTech.Core.Security` | `PasswordHasher` (PBKDF2), `HashHelper` (SHA-256), `SecureRandom` |
| Extensions | `PennForgeTech.Core.Validation` | `Validator<TInstance>`, `ValidationResult`, `ValidationException` |
| Extensions | `PennForgeTech.Core.Extensions` | Extension methods for strings, sequences, dictionaries, `DateTimeOffset`, enums and nullable values |
| Extensions | `PennForgeTech.Core.Configuration` | `RequiredSetting.Require` |
| Extensions | `PennForgeTech.Core.Resilience` | `RetryExecutor` |
| Infrastructure | `PennForgeTech.Core.Serialization.Json` | `JsonTextSerializer` |
| Infrastructure | `PennForgeTech.Core.Logging.MicrosoftExtensions` | `MicrosoftAppLogger`, an adapter over `Microsoft.Extensions.Logging` |
| Infrastructure | `PennForgeTech.Core.Http` | `IHttpService`, `HttpService` |
| Infrastructure | `PennForgeTech.Core.Data.SqlServer` | `SqlExecutor` |
| Observability | `PennForgeTech.Core.Diagnostics` | `PennForgeDiagnostics.ActivitySource` |
| Framework | `PennForgeTech.Framework.Application` | `IRequestHandler<TRequest, TResponse>` |
| Framework | `PennForgeTech.Framework.Pipelines` | `Pipeline<TRequest, TResponse>`, `IPipelineBehavior<TRequest, TResponse>`, `LoggingBehavior<TRequest, TResponse>` |
| Framework | `PennForgeTech.Framework.WebApi` | `ToHttpResult` for `Result` and `Result<TValue>` |

Namespaces match package names.

## Code style

Enforced by `.editorconfig` and by building with warnings as errors:

- No `var`; types are always written out.
- No expression-bodied members or other shorthand; braces and block bodies everywhere.
- Instance members are always accessed with `this.`.
- Namespaces use braces (block-scoped), usings are explicit (no implicit usings), and comments are kept to a minimum.

## Build, test and pack

```bash
dotnet build
dotnet test
dotnet pack -c Release
```

## Status

Alpha. `SqlExecutor` takes a `DbProviderFactory`, so add `Microsoft.Data.SqlClient` in the consuming app and pass `SqlClientFactory.Instance`. `RetryExecutor` is only appropriate for operations that are safe to repeat; nothing retries automatically.

See `docs/ARCHITECTURE.md` for dependency rules.
