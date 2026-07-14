# 02-store-frontend — Progress Details

## What changed
- src/Store/Store.csproj — TFM `net6.0` → `net10.0`; Refit `6.3.2` → `13.1.0` (fixes security vulnerability); Microsoft.VisualStudio.Azure.Containers.Tools.Targets `1.15.1` → `1.23.0`
- src/Monitoring/Monitoring.csproj — `<TargetFramework>net6.0</TargetFramework>` → `<TargetFrameworks>net6.0;net10.0</TargetFrameworks>` (Top-Down Phase 1 multi-targeting); Microsoft.ApplicationInsights.AspNetCore `2.20.0` → `2.23.0` (removed transitive NU1903 Newtonsoft.Json 11.0.2 high + NU1904 System.Drawing.Common 4.7.0 critical vulnerability warnings)
- src/Store/Program.cs — CS8604 fixed: `GetValue<string>(...)` results null-guarded with explicit `InvalidOperationException`; CS8618 fixed: `Product.ProductId`/`ProductName` initialized to `string.Empty`
- src/Store/Pages/Index.razor — CS8600/CS8604 fixed: `TryGetValue(cacheKey, out Product[]? tmp) || tmp is null` pattern
- src/Store/Dockerfile — `aspnet:6.0`/`sdk:6.0` → `aspnet:10.0`/`sdk:10.0`; added `ENV ASPNETCORE_HTTP_PORTS=80` (10.0 images default to port 8080; infra ingress targetPort is 80)

## Build/test results
- `dotnet build src/Store/Store.csproj` — success, 0 warnings (Store net10.0 + Monitoring both TFMs)
- `dotnet build src/Store.sln` — success. Remaining warnings only in projects not in this task's scope: NETSDK1138 (net6.0 EOL) for Store.InventoryApi/Store.ProductApi/Monitoring-net6.0 (transitional — removed by tasks 03–05) and CS8618 in Store.ProductApi (task 04's scope)
- Refit 6.3.2 → 13.1.0 breaking-change check: usage surface (`[Get]` attributes, `RestService.For<T>(HttpClient)`) unchanged — compiled without code changes
- Tests: none exist in the solution

## Issues
- Assessment reported "No supported version found" for Containers.Tools.Targets; NuGet flat-container showed stable 1.23.0 (tooling-only MSBuild package) — bumped and verified via build.
- Note: Monitoring's net6.0 target emits NETSDK1138 (EOL) by design during Phase 1; resolved when task 05 consolidates to net10.0.
