# 06-final-validation — Progress Details

## Validation results

| Check | Result |
|---|---|
| Clean rebuild `dotnet build src/Store.sln --no-incremental` | ✅ Success, 0 warnings |
| All 4 csproj target net10.0 (single-target) | ✅ Monitoring, Store, Store.InventoryApi, Store.ProductApi |
| No net6.0 / aspnet:6 / sdk:6 references in src/ (excl. bin/obj) | ✅ None found |
| 3 Dockerfiles on `aspnet:10.0` / `sdk:10.0` | ✅ Store, Store.InventoryApi, Store.ProductApi |
| `dotnet list package --vulnerable --include-transitive` | ✅ No vulnerable packages in any project |
| Tests | N/A — solution contains no test projects |

## No code changes in this task
Validation-only; no files modified (workflow artifacts only).

## Deferred recommendations
1. **OpenTelemetry migration** — the classic Application Insights SDK (Microsoft.ApplicationInsights.AspNetCore 2.23.0) is in maintenance mode; consider migrating Monitoring to the Azure Monitor OpenTelemetry Distro (`Azure.Monitor.OpenTelemetry.AspNetCore`).
2. **Central Package Management** — all projects are now SDK-style on a single TFM (net10.0); CPM (`Directory.Packages.props`) can be added cleanly if the solution grows.
3. **Container port modernization** — Dockerfiles keep port 80 via `ASPNETCORE_HTTP_PORTS=80` to match the existing infra ingress (`targetPort: 80` in infra/app/*.bicep). Optionally switch to the .NET 10 default 8080 + non-root `USER app` and update the Bicep targetPort values together.
4. **Swagger vs built-in OpenAPI** — .NET 10 minimal APIs support built-in OpenAPI document generation (`Microsoft.AspNetCore.OpenApi`); Swashbuckle 10.2.3 works fine, but the built-in support could replace it.
