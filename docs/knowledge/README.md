# Game Knowledge Base

This directory is the evidence-backed index for the Unity game scan. It is split into two domains:

- `technical-scan.md` — architecture, runtime flows, battle simulation, physics, terrain, visuals, animation, UI, assets, services, tests, and technical gaps.
- `economy-scan.md` — campaign, planets, missions, rewards, currencies, commander progression, weapons, upgrades, ads/premium seams, balance tables, and content gaps.

Machine-readable artifacts:

- `technical-inventory.json`
- `economy-balance.json`
- `scan-index.json`

Harness execution and evidence state are tracked in `docs/knowledge/harness-scan-plan.md` and `.aios/evidence/game-scan/manifest.json`.

Every claim should include an exact repository path and, where practical, a line or symbol reference. Confidence is `EXTRACTED` for directly observed code/data, `INFERRED` for behavior derived from multiple sources, and `AMBIGUOUS` when the repository does not establish the answer.

The scan is a snapshot. When code or content changes, update the affected domain report and run the verification checklist in `scan-index.json` before changing balance or monetization decisions.
