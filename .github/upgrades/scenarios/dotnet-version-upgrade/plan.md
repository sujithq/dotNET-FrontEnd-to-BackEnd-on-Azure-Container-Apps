# .NET 10 Upgrade Plan — Store.sln

## Overview

**Target**: Upgrade all 4 projects (Store, Store.InventoryApi, Store.ProductApi, Monitoring) from net6.0 to net10.0
**Scope**: Small solution — 4 SDK-style projects, ~280 LOC, 2-tier dependency graph, all rated Low difficulty

### Selected Strategy
**Top-Down (Application-First)** — Applications upgraded first, libraries multi-targeted temporarily.
**Rationale**: User-selected. The 3 applications are upgraded in priority order (Store first — it carries the Refit security vulnerability); the shared Monitoring library multi-targets `net6.0;net10.0` during Phase 1 so the solution stays buildable, then consolidates to `net10.0` in Phase 2.

**Applications** (priority order): Store (Blazor Server frontend, security vulnerability), Store.InventoryApi, Store.ProductApi
**Libraries needing multi-targeting**: Monitoring (referenced by all 3 applications)
**Application dependency map**: Store → Monitoring; Store.InventoryApi → Monitoring; Store.ProductApi → Monitoring
**Phase 2 trigger**: All 3 applications on net10.0 — no consumer needs the net6.0 target anymore.

## Tasks

### 01-prerequisites: Verify .NET 10 SDK and toolchain

Verify a .NET 10 SDK is installed and usable for the solution. The repo has no global.json, so no SDK pinning changes are needed — only validation that `dotnet` resolves a 10.x SDK that can restore and build the solution.

**Done when**: A .NET 10 SDK is confirmed installed and `dotnet --version` resolves to a 10.x SDK from the repo root.

---

### 02-store-frontend: Upgrade Store (Blazor Server frontend) to net10.0

Upgrade the Store application TFM from net6.0 to net10.0. As the first application upgraded, this task also adds multi-targeting (`net6.0;net10.0`) to the Monitoring library so the two APIs still on net6.0 keep building. Store carries the highest risk in the assessment: Refit 6.3.2 has a known security vulnerability (upgrade to 13.1.0+, which has breaking API changes across major versions), Microsoft.VisualStudio.Azure.Containers.Tools.Targets 1.15.1 is flagged incompatible (bump to a current version), and 11 API issues are flagged — 2 binary-incompatible usages of `ConfigurationBinder.GetValue` in Program.cs plus behavioral changes around `System.Uri`, `AddHttpClient`, and `UseExceptionHandler`.

Fix all flagged API changes inline (confirmed option — no stubs). Update the Store Dockerfile base images from `mcr.microsoft.com/dotnet/aspnet:6.0` / `sdk:6.0` to the 10.0 equivalents. Research starting points: Refit 6→13 breaking changes (namespace/settings changes, `IHttpContentSerializer`), Blazor Server breaking changes net6→net10, and the Store Program.cs service registrations.

**Done when**: Store targets net10.0, Monitoring multi-targets net6.0;net10.0, Refit is ≥13.1.0 with consuming code compiling, the full solution builds warning-free, and the Store Dockerfile references 10.0 images.

---

### 03-inventory-api: Upgrade Store.InventoryApi to net10.0

Upgrade the Store.InventoryApi application TFM from net6.0 to net10.0. Monitoring already multi-targets after task 02, so no library work is expected. Package work: Swashbuckle.AspNetCore 6.2.3 predates .NET 10 (verify/bump to a compatible version; note .NET 10 templates favor built-in OpenAPI support, but keeping Swashbuckle updated is the minimal-change path) and Microsoft.VisualStudio.Azure.Containers.Tools.Targets 1.15.1 is flagged incompatible (bump). Update the Dockerfile base images to 10.0. Assessment flags no API issues for this project.

**Done when**: Store.InventoryApi targets net10.0, packages restore without incompatibility warnings, the full solution builds warning-free, and the Dockerfile references 10.0 images.

---

### 04-product-api: Upgrade Store.ProductApi to net10.0

Upgrade the Store.ProductApi application TFM from net6.0 to net10.0 — the last application, structurally identical to Store.InventoryApi. Package work: Bogus 34.0.2 (verify compatibility, bump if needed), Swashbuckle.AspNetCore 6.2.3, and Microsoft.VisualStudio.Azure.Containers.Tools.Targets 1.15.1 (bump). Update the Dockerfile base images to 10.0. Assessment flags no API issues for this project.

**Done when**: Store.ProductApi targets net10.0, packages restore without incompatibility warnings, the full solution builds warning-free, and the Dockerfile references 10.0 images.

---

### 05-monitoring-consolidation: Consolidate Monitoring library to net10.0

Phase 2 — all applications are on net10.0, so remove multi-targeting from Monitoring and single-target net10.0. Clean up any conditional compilation or TFM-conditioned package references introduced during Phase 1. Also modernize the deprecated Microsoft.ApplicationInsights.AspNetCore 2.20.0 package: bump to the latest 2.x servicing release as the minimal-change path (the library's `ApplicationMapNodeNameInitializer` uses the classic AI SDK; a full OpenTelemetry migration is out of scope and recorded as a deferred recommendation).

**Done when**: Monitoring targets net10.0 only, no TFM conditions remain, the deprecated package reference is updated, and the full solution builds warning-free.

---

### 06-final-validation: Full solution validation

Validate the completed upgrade end-to-end: `dotnet restore` and `dotnet build` the solution warning-free, run any discovered tests (none found in assessment — confirm), verify all 4 csproj files target net10.0 with no leftover net6.0 references, and confirm the 3 Dockerfiles build against 10.0 images. Document deferred recommendations (OpenTelemetry migration for Application Insights, optional CPM adoption now that all projects share one TFM, azd/Bicep container image tags if applicable).

**Done when**: Solution builds warning-free on net10.0, no net6.0 references remain in src/, and deferred recommendations are recorded in progress notes.
