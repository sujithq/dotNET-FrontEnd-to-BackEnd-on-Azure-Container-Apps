# 05-monitoring-consolidation: Consolidate Monitoring library to net10.0

Phase 2 (Top-Down): all 3 applications are on net10.0 — no consumer needs net6.0 anymore.

## Research Findings

- src/Monitoring/Monitoring.csproj currently: `<TargetFrameworks>net6.0;net10.0</TargetFrameworks>`; AI package **already updated** to 2.23.0 in task 02 (to clear NU1903/NU1904 transitive vulnerability warnings) — the package portion of this task is already done
- No `#if` conditional compilation and no TFM-conditioned ItemGroups were introduced during Phase 1 — nothing to clean beyond the TFM line
- Single source file (ApplicationMapNodeNameInitializer.cs): classic AI SDK `ITelemetryInitializer` + `AddApplicationInsightsTelemetry()` — compiles on net10.0 already (validated in tasks 02–04 builds)
- Deferred recommendation to record in task 06: classic Application Insights SDK is in maintenance mode → OpenTelemetry migration is the modern path (out of scope here)

### Files to modify
1. src/Monitoring/Monitoring.csproj — `<TargetFrameworks>net6.0;net10.0</TargetFrameworks>` → `<TargetFramework>net10.0</TargetFramework>`

### Decomposition
Atomic — one line in one project.

## Done when

- Monitoring targets net10.0 only; no TFM conditions remain
- Deprecated package reference updated (done in task 02: AI.AspNetCore 2.23.0)
- Full solution builds warning-free
