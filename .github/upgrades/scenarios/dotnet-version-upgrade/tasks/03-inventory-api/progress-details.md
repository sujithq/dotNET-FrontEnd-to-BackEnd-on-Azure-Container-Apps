# 03-inventory-api — Progress Details

## What changed
- src/Store.InventoryApi/Store.InventoryApi.csproj — TFM `net6.0` → `net10.0`; Microsoft.VisualStudio.Azure.Containers.Tools.Targets `1.15.1` → `1.23.0`; Swashbuckle.AspNetCore `6.2.3` → `10.2.3`
- src/Store.InventoryApi/Dockerfile — `aspnet:6.0`/`sdk:6.0` → `10.0`; added `ENV ASPNETCORE_HTTP_PORTS=80`

## Build/test results
- `dotnet build src/Store.InventoryApi/Store.InventoryApi.csproj` — success, 0 warnings
- `dotnet build src/Store.sln` — success; remaining warnings only in Store.ProductApi (task 04 scope: CS8618 + NETSDK1138) and Monitoring net6.0 EOL notice (transitional, removed in task 05)
- Swashbuckle 6.2.3 → 10.2.3: usage surface (`AddSwaggerGen`/`UseSwagger`/`UseSwaggerUI`) compiled unchanged
- Tests: none exist in the solution

## Issues
- None.
