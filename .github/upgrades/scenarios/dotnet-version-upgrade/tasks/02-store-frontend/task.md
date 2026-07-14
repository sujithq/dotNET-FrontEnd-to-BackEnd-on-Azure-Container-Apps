# 02-store-frontend: Upgrade Store (Blazor Server frontend) to net10.0

First application task (Top-Down): upgrade Store to net10.0 and add multi-targeting to Monitoring.

## Research Findings

### Assessment issues (Store: 14 occurrences)
- **NuGet.0001** Microsoft.VisualStudio.Azure.Containers.Tools.Targets 1.15.1 — assessment: "No supported version found"; NuGet flat-container shows latest stable **1.23.0** (MSBuild tooling-only package, TFM-agnostic) → bump to 1.23.0
- **NuGet.0004** Refit 6.3.2 → **13.1.0** (security vulnerability; confirmed via get_supported_package_version)
- **Project.0002** TFM net6.0 → net10.0
- **Api.0001 ×2** `ConfigurationBinder.GetValue<string>` (Program.cs L7-8) — binary incompatible; resolved by recompile against net10; watch for nullable annotations (`GetValue<string>` returns `string?` → CS8604 at `new Uri(...)`)
- **Api.0003 ×9** behavioral: `System.Uri` ctor, `AddHttpClient`, `UseExceptionHandler("/Error")` — no code shape change required, verify at runtime

### Refit 6.3.2 → 13.1.0 breaking-change review
- Usage surface: `[Get("...")]` attributes + `RestService.For<IStoreBackendClient>(client)` (Program.cs). This API surface is unchanged in v13; System.Text.Json is default serializer in both v6.3 and v13. No code change expected.

### Files to modify
1. src/Store/Store.csproj — TFM net10.0; Refit 13.1.0; Containers.Tools.Targets 1.23.0
2. src/Monitoring/Monitoring.csproj — `<TargetFramework>` → `<TargetFrameworks>net6.0;net10.0</TargetFrameworks>` (keeps Store.InventoryApi/Store.ProductApi on net6.0 buildable)
3. src/Store/Program.cs — fix nullable warnings that surface (Product class has uninitialized non-nullable strings → CS8618; GetValue may trigger CS8604)
4. src/Store/Dockerfile — aspnet:6.0/sdk:6.0 → 10.0; add `ENV ASPNETCORE_HTTP_PORTS=80` (aspnet:8.0+ images default to port 8080; infra ingress targetPort is 80 in infra/app/store.bicep)

### Decomposition
Atomic — one app project + one TFM line in the shared lib, single concern cluster. No breakdown.

## Done when

- Store targets net10.0; Monitoring multi-targets net6.0;net10.0
- Refit ≥13.1.0, consuming code compiles
- Full solution builds warning-free
- Store Dockerfile references 10.0 images
