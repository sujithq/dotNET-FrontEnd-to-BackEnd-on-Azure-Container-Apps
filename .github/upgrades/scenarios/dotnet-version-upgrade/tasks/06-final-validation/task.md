# 06-final-validation: Full solution validation

End-to-end validation of the completed .NET 10 upgrade.

## Validation checklist (from plan)

- Clean rebuild (`dotnet build src/Store.sln --no-incremental`) — warning-free
- Tests — confirm none exist (assessment showed no test projects; solution has 4 projects, none test)
- All 4 csproj files single-target net10.0, no leftover net6.0 references in src/
- 3 Dockerfiles reference 10.0 images
- Vulnerable package scan (`dotnet list package --vulnerable --include-transitive`)
- Record deferred recommendations
