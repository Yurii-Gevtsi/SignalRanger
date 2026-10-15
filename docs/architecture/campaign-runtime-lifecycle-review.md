# Campaign runtime lifecycle review

Fixed point: HEAD e4f4a0f4456f9955a0eb630a703f46faaa0afd53; review uses the
non-empty working-tree diff for AppServices.cs and AppFlowSmokeTests.cs.
Spec: campaign-runtime-lifecycle-fix.md and the reported first-planet/admin lock.
Standards: AGENTS.md and CLAUDE.md, composition-root ownership, gameplay/UI
separation, no player text changes, no save migration or reward changes.

Standards findings: none. Reviewed naming, duplication, feature envy, data
clumps, primitive obsession, repeated switches, shotgun surgery, divergent
change, speculative generality, message chains, middle man, and refused
bequest; no actionable smells. Shared content setup has two real consumers.

Spec findings: none. Destroyed Unity references trigger regeneration; live
references preserve idempotence. Existing save/economy instances remain in use,
so the repair does not reload player progress or grant rewards again. Assertions
were added without altering existing expectations. The framework test-runner
limitation is explicitly recorded; actual NUnit assertions and live scene
checks provide the bounded evidence.

VERDICT:
FILES_REVIEWED:
  - Assets/_Game/Runtime/App/AppServices.cs: lines 75-157 (changed)
  - Assets/_Game/Tests/PlayMode/AppFlowSmokeTests.cs: lines 19-67 (changed)
CHECKS:
  - Unity compilation/console: PASS (no errors)
  - node .rex-harness/checks/campaign-lifecycle-check.mjs: PASS
  - CampaignProgressionServiceTests NUnit methods in Unity: PASS (7/7)
  - Live menu and BattleScene entry, two Play sessions: PASS
  - node tools/localization-integrity-gate.mjs: PASS (13 dictionaries)
  - git diff --check -- scoped C# files: PASS
  - Full PlayMode runner: unavailable (framework exception before tests)
CODE:
  > if (Content == null || Missions == null || Missions.Length == 0
  >     || Missions.Any(mission => mission == null)
  >     || Content.Weapons.Any(weapon => weapon == null)
  >     || Content.Arenas.Any(arena => arena == null))
  > { InitializeRuntimeContent(); }
VALIDATION:
  APPROVED — bounded regression and real battle entry verified.
