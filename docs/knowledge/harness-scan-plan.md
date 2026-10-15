# AIOS Harness Scan Plan

Status: initialized; no scan iteration has completed yet.

Session: `codex-cli-20260814T083201Z-solo`  
Provider: Codex  
Objective: complete a technical and economic scan of the Unity project and record evidence-backed knowledge artifacts.

## Run controls

- Keep the run resumable and idempotent. One stage at a time; each stage must leave a durable artifact and a manifest entry.
- Treat `.aios/context-db` as storage/evidence, not as prompt replay. Resume only after explicit operator/orchestrator intent.
- Preserve the existing dirty worktree. Do not reset, clean, or overwrite unrelated user changes.
- Do not modify `Examples/` or include prohibited external product names in project documentation.
- Every conclusion must point to a repository file, test output, CLI result, or ContextDB event. Mark unverified items as open.

## Staged checklist

### 1. Orientation and inventory — pending

- [x] Record the active session, objective, provider, and harness paths.
- [x] Confirm no prior plan is active (`aios plan status` reports `active: null`).
- [ ] Inventory runtime assemblies, scenes, data/config assets, tests, and current build/release gates.
- [ ] Capture the inventory command output as evidence.

### 2. Technical scan — pending

- [ ] Map app/bootstrap, gameplay/battle, terrain/arena, UI, progression, economy, SDK, and platform boundaries.
- [ ] Compare architecture documents with the current source tree and record drift.
- [ ] Run focused EditMode/PlayMode/build checks that are available in the workspace.
- [ ] Record blockers separately from confirmed defects; do not infer runtime behavior from file existence alone.

### 3. Economy and balance scan — pending

- [ ] Extract currencies, rewards, unlock gates, upgrade costs, and progression formulas from design docs and runtime data.
- [ ] Produce a compact balance table with source locations and confidence per row.
- [ ] Identify economy risks (pacing, pay-to-win pressure, unclear sinks/sources) and link each to evidence.

### 4. Knowledge artifacts — pending

- [ ] Publish or update focused documents under `docs/knowledge/`, `docs/architecture/`, or `docs/design/` only when findings are supported.
- [ ] Add durable `ACTIVE:`/`ARCH:`/`SYSTEM:`/`CONVENTION:`/`WORKFLOW:` memos for decisions that must survive the chat.
- [ ] Update the evidence manifest after each completed stage.

### 5. Verification and handoff — pending

- [ ] Validate all generated JSON as UTF-8 and parseable.
- [ ] Verify every manifest artifact path exists (or is explicitly marked `planned`/`missing`).
- [ ] Write a continuity summary with completed work, unresolved questions, evidence refs, and the next single action.
- [ ] Report the harness status and exact evidence paths before claiming completion.

## Current evidence and next action

The session journal exists but is at iteration 0: `l1-checkpoints.jsonl` and `l2-events.jsonl` are empty, `latestEvidence` is empty, and no checkpoint or continuity file has been written. The next action is a read-only project inventory followed by a checkpoint; do not resume or spawn another harness run until the operator explicitly requests it.

Existing high-value source documents for the scan include:

- `docs/architecture/mvp-implementation-summary.md`
- `docs/architecture/target-architecture.md`
- `docs/architecture/battle-physics-rework.md`
- `docs/architecture/backend-economy-implementation.md`
- `docs/design/weapon-progression-economy-audit.md`
- `docs/design/arena-unity-integration-instructions.md`
- `docs/plans/desert-sci-fi-arena-unity-phase-3.md`

