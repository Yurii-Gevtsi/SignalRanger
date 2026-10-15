# Campaign runtime lifecycle fix

Scope: restore battle selection after entering Play again with domain reload disabled.
Non-goals: change campaign unlock rules, rewards, localization, or player saves.

Observed in the running menu: all 27 mission wrappers survived in AppServices,
but every mission compared equal to Unity null. Weapons and arenas were also
destroyed. Admin mode was enabled and planet reachability returned true, while
mission selection returned false and the menu reported 0/0 missions.

Ownership: AppServices composition root must recreate services when its runtime
content has been destroyed, preserving idempotence while content remains alive.
Use the existing Initialize entry point; do not bypass Unity null checks in UI.

Acceptance mapping and test seam: AppFlowSmokeTests calls AppServices.Initialize,
destroys the first runtime mission to reproduce stale content, calls Initialize
again, and asserts all missions are alive, the first is selectable, admin can
select the final mission without advancing progress, and a repeated Initialize
with live content preserves the same service instances.

Focused command: `node .rex-harness/checks/campaign-lifecycle-check.mjs`.
This invokes the actual NUnit regression test through Unity execute_code.
The standard PlayMode test run was attempted but Unity Test Framework raised
a NullReferenceException in PlayModeRunTask.cs:52 before executing any tests.

- [x] Reproduce regression with real Unity objects.
- [x] Repair composition-root lifetime.
- [x] Verify regression, progression, and live battle entry.

Implementation: Initialize checks for destroyed missions, weapons, and arenas
before honoring IsInitialized. Shared InitializeRuntimeContent rebuilds the
catalog, campaign, tutorial/feed and session against the existing save and
economy. It neither reloads the save nor grants login rewards again.

Evidence: RED receipt:21292f05-4b64-4cd9-99e4-57c9cae1e0a4;
GREEN receipt:dd0c359e-be7f-4441-8246-06f39dd1bbed;
repeat PASS receipt:94907b04-c4db-4fc9-8f7c-95fb605303a0.
Seven existing CampaignProgressionServiceTests passed by invoking their NUnit
methods in Unity. Live admin menu selected every implemented chapter across
three sectors; first-planet battle entry reached BattleScene with a live
BattleSceneController. A second Stop/Play cycle retained 27 live missions and
restored the first planet's battle button. Final Unity console had no errors.
