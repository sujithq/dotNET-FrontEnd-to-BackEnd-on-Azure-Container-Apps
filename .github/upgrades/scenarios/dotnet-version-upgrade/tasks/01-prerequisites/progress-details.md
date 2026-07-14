# 01-prerequisites — Progress Details

## What changed
- Added global.json at repo root pinning SDK `10.0.301` with `rollForward: latestFeature`.

## Why
- SDKs installed: 9.0.315, 10.0.301, 11.0.100-preview.5. Without global.json the repo resolved to the 11.0 **preview** SDK; the pin ensures the stable .NET 10 toolchain is used for the upgrade.

## Validation
- `validate_dotnet_sdk_installation(net10.0)` → Compatible SDK found
- `dotnet --version` from repo root → `10.0.301` ✔

## Issues
- None.
