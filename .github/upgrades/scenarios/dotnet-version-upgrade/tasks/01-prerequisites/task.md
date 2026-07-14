# 01-prerequisites: 01-prerequisites

Verify a .NET 10 SDK is installed and usable for the solution from the repo root.

## Research Findings

- `validate_dotnet_sdk_installation(net10.0)` → Compatible SDK found
- Installed SDKs: 9.0.315, 10.0.301, **11.0.100-preview.5.26302.115**
- No global.json existed → `dotnet --version` from repo root resolved to the **11.0 preview** SDK
- Decision: add `global.json` at repo root pinning SDK `10.0.301` with `rollForward: latestFeature` so the upgrade builds with the stable .NET 10 toolchain instead of a preview SDK

## Done when

- A .NET 10 SDK is confirmed installed ✔ (10.0.301)
- `dotnet --version` resolves to a 10.x SDK from the repo root (via global.json pin)
