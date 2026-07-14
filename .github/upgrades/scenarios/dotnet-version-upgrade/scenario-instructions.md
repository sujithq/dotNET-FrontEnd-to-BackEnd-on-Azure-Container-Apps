# .NET Version Upgrade

## Preferences
- **Flow Mode**: Automatic
- **Target Framework**: net10.0

## Source Control
- **Source Branch**: main
- **Working Branch**: upgrade-dotnet-10
- **Commit Strategy**: After Each Task
- **Branch Sync**: Auto (Merge)

## Upgrade Options
**Source**: .github/upgrades/scenarios/dotnet-version-upgrade/upgrade-options.md

### Strategy
- Upgrade Strategy: Top-Down

### Project Structure
- Package Management: Per-Project

### Compatibility
- Unsupported API Handling: Fix Inline

## Strategy
**Selected**: Top-Down (Application-First)
**Rationale**: User-selected over the All-at-Once default. 3 applications (Store, Store.InventoryApi, Store.ProductApi) upgraded first in priority order; shared Monitoring library multi-targets net6.0;net10.0 during Phase 1, consolidated to net10.0 in Phase 2.

### Execution Constraints
- Applications first, in priority order: Store (security vulnerability) → Store.InventoryApi → Store.ProductApi
- Add multi-targeting to Monitoring only when the first application that needs it is upgraded — not before
- Solution must stay buildable after each application task
- Phase 2 (Monitoring consolidation to net10.0 only) starts only after ALL applications are upgraded
- Fix all flagged API changes inline within the task that touches them — no stubs or deferred work

## Decisions
- Upgrade Strategy: Top-Down — user changed from recommended All-at-Once in the options form

