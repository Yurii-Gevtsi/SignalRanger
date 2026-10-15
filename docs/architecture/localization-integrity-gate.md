# Localization Integrity Gate

This repository treats localization as data integrity, not presentation polish.

- Keep every JSON dictionary as strict UTF-8.
- Keep the exact same flattened key set in all 13 dictionaries.
- Keep every player-facing value non-empty.
- Reject Unicode replacement characters in every dictionary.
- Reject literal `?` loss markers in the Ukrainian dictionary.
- Add campaign sector and planet names to every dictionary in the same change.

`Assets/_Game/Tests/EditMode/Localization/LocalizationCoverageTests.cs` enforces these rules. Run that test suite, JSON parsing, key-parity, and byte-level UTF-8/mojibake checks before handing off any localization change.

Before every commit, Git executes `.githooks/pre-commit`, which runs `node tools/localization-integrity-gate.mjs --staged`. The commit is blocked if the dictionaries are invalid or divergent, a Ukrainian loss marker is found, a staged C# change introduces a non-empty `GameText` fallback, or it assigns a direct string to UI text.
