# 04-product-api: Upgrade Store.ProductApi to net10.0

Last application task (Top-Down Phase 1). Monitoring already multi-targets — no library work.

## Research Findings

### Assessment issues (2)
- **NuGet.0001** Microsoft.VisualStudio.Azure.Containers.Tools.Targets 1.15.1 → 1.23.0
- **Project.0002** TFM net6.0 → net10.0
- No API issues flagged

### Package check (get_supported_package_version, net10.0)
- Bogus 34.0.2 → **35.6.5**
- Swashbuckle.AspNetCore 6.2.3 → **10.2.3** (same as task 03)

### Files to modify
1. src/Store.ProductApi/Store.ProductApi.csproj — TFM net10.0; Bogus 35.6.5; Containers.Tools.Targets 1.23.0; Swashbuckle 10.2.3
2. src/Store.ProductApi/Program.cs — fix pre-existing CS8618 on `Product.ProductName` (uninitialized non-nullable string)
3. src/Store.ProductApi/Dockerfile — aspnet/sdk 6.0 → 10.0; add `ENV ASPNETCORE_HTTP_PORTS=80`

### Watch item
- Bogus `Faker<Product>().StrictMode(true).RuleFor(p => p.ProductId, ...)` targets a get-only property (`ProductId => Guid.NewGuid()`); verify 35.x still compiles this pattern.

### Decomposition
Atomic — single project, one concern cluster.

## Done when

- Store.ProductApi targets net10.0
- Packages restore without incompatibility warnings
- Full solution builds warning-free (except transitional net6.0 EOL notice from Monitoring, removed in task 05)
- Dockerfile references 10.0 images
