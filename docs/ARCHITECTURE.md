# Architecture

## Dependency direction

Application hosts -> optional Framework -> Extensions and Infrastructure -> Foundation.

- Foundation depends on nothing outside Foundation. `Results` depends on `Primitives`; the rest have no project references.
- Extensions and Infrastructure depend on Foundation only, never on Framework and never on each other, except `Serialization.Json` which uses `Abstractions` and `Exceptions`.
- Framework depends on Foundation. Only `WebApi` references ASP.NET Core.

## Conventions

- Expected failures are returned as `Result` values with a `ResultError` whose code looks like `Area.Reason` (`Validation.Failed`, `Http.Status404`).
- Unexpected failures throw `PennForgeException` or a subclass, which always carries a `Code`.
- Time comes from `IClock`; tests use `FixedClock`.
- Contracts live in `Abstractions`; implementations live in their own package so consumers take only what they use.

## Guardrails

No secret values in logs. No blind retries on writes. Prefer framework-provided primitives over custom replacements. Validate and benchmark before production use.

## Future

Typed DI registration, SQL transactions, richer HTTP error policies, health checks, telemetry exporters, files and messaging.
