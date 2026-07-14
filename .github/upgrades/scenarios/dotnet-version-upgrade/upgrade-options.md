# Upgrade Options — Store.sln

Assessment: 4 projects, all net6.0 SDK-style → net10.0; 1 vulnerable package (Refit), 1 deprecated + 1 incompatible package, 11 API issues (2 binary incompatible).

## Strategy

### Upgrade Strategy
Small solution (4 projects, 2-tier dependency graph, all Low difficulty) upgrading modern-to-modern — a single atomic pass is fastest with no incremental overhead.

| Value | Description |
|-------|-------------|
| All-at-Once | Upgrade all projects simultaneously in a single atomic pass; validate full solution build after upgrade |
| **Top-Down** (selected) | Upgrade entry-point applications first, temporarily multi-targeting the shared Monitoring library; consolidate afterwards |

## Project Structure

### Package Management
4 projects use per-project PackageReference without central package management; solution is below the scale where CPM pays off (~8 direct references, no version conflicts).

| Value | Description |
|-------|-------------|
| **Per-Project** (selected) | Each project retains its own package versions; no structural change during the upgrade |
| Central Package Management (CPM) | Create Directory.Packages.props and move all versions there for centralized maintenance |

## Compatibility

### Unsupported API Handling
Assessment flags 2 binary-incompatible API usages (ConfigurationBinder.GetValue in Store) and 9 behavioral changes — few and minor, typical for modern-to-modern upgrades.

| Value | Description |
|-------|-------------|
| **Fix Inline** (selected) | Resolve every flagged API change within the upgrade task itself; no stubs or deferred work |
| Defer Complex Changes | Apply simple replacements inline; stub complex changes and resolve them in follow-up subtasks |
