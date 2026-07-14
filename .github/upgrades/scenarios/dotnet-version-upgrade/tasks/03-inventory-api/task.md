# 03-inventory-api: Upgrade Store.InventoryApi to net10.0

Second application task (Top-Down). Monitoring already multi-targets net6.0;net10.0 (task 02) — no library work needed.

## Research Findings

### Assessment issues (2)
- **NuGet.0001** Microsoft.VisualStudio.Azure.Containers.Tools.Targets 1.15.1 → bump to 1.23.0 (same resolution as task 02)
- **Project.0002** TFM net6.0 → net10.0
- No API issues flagged for this project

### Package check
- Swashbuckle.AspNetCore 6.2.3 → **10.2.3** (confirmed supported for net10.0 via get_supported_package_version). Usage surface is minimal (`AddSwaggerGen()`, `UseSwagger()`, `UseSwaggerUI()`) — unchanged across 6→10.

### Files to modify
1. src/Store.InventoryApi/Store.InventoryApi.csproj — TFM net10.0; Containers.Tools.Targets 1.23.0; Swashbuckle 10.2.3
2. src/Store.InventoryApi/Dockerfile — aspnet/sdk 6.0 → 10.0; add `ENV ASPNETCORE_HTTP_PORTS=80` (same reason as task 02: 10.0 images default to 8080, infra targetPort is 80)
3. Program.cs — minimal API with IMemoryCache; no flagged issues; watch for nullable warnings on build

### Decomposition
Atomic — single project, one concern cluster.

## Done when

- Store.InventoryApi targets net10.0
- Packages restore without incompatibility warnings
- Full solution builds warning-free (except transitional net6.0 EOL notices from remaining Phase 1 projects)
- Dockerfile references 10.0 images
