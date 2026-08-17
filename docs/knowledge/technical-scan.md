# Technical scan

Date: 2026-08-14  
Scope: `Assets/_Game`, `ProjectSettings`, `Packages`, scenes/prefabs/art, tests, and architecture/design documents. This is a read-only audit; reference material was not modified.

## Executive summary

The repository contains a layered Unity 2D mobile MVP with a composition-root style application shell and a substantial, deterministic battle domain. Runtime assemblies separate Core, Data, Gameplay, App, UI, Progression, Economy, Inventory, Audio, Tutorial, and Infrastructure. The build currently includes Boot, Menu, and Battle scenes; the first two are intentionally lightweight and are populated by runtime controllers, while Battle contains camera/light/volume infrastructure and is bootstrapped by code.

The battle implementation is more complete than the authored content: turn flow, aim, ballistic simulation, weapon effects, AI, terrain rasterization, persistence, progression, localization and no-op service adapters are present. The largest gaps are production content and release hardening: authored ScriptableObject content is sparse, most presentation assets are placeholder/prototype art, audio is no-op, and external services (ads, purchases, notifications, account/backend and remote config) are adapters rather than configured production flows. The existing MVP summary also records destructible terrain as out of MVP, while current code already contains a destructible terrain implementation; this should be treated as an integration/status inconsistency to resolve.

## Architecture and assemblies

The assembly graph is explicit in `Assets/_Game/**/Game.*.asmdef`:

| Layer | Assembly | Evidence and responsibility |
|---|---|---|
| Core | `Game.Core` | Dependency-free contracts, scene names, session context, localization primitives (`Runtime/Core`). |
| Data | `Game.Data` | Save model, JSON serialization, migrations and progression data (`Runtime/Data`). References Core. |
| Gameplay | `Game.Gameplay` | Battle domain, simulation, terrain, weapons, AI, views and battle assembly (`Runtime/Gameplay`). References Core/Data/Input System. |
| App | `Game.App` | Bootstrap, service composition, scene flow, menu/loadout/shop/settings/reward controllers. References all feature layers. |
| UI | `Game.UI` | Text styling and HUD view (`Runtime/UI`); uses TMP, UGUI and Input System. |
| Progression/Economy/Inventory/Tutorial/Audio | `Game.Progression`, `Game.Economy`, `Game.Inventory`, `Game.Tutorial`, `Game.Audio` | Feature services and domain data, each kept in its own assembly. |
| Infrastructure | `Game.Infrastructure` + `Game.Infrastructure.Ugs` | Interfaces/no-op adapters plus conditional Unity Gaming Services adapters. |
| Tests | `Game.Tests.EditMode`, `Game.Tests.PlayMode` | Test assemblies depend on runtime assemblies and Unity Test Runner. |

The intended dependency direction documented in `docs/architecture/target-architecture.md` is mostly reflected in the assemblies: Gameplay does not reference ads, purchasing, or backend SDKs; App composes concrete services. One caution is that `Game.Infrastructure.Ugs` references `Game.App` in addition to Infrastructure, so the online adapter is not a completely independent outer layer.

## Application and scene flow

`ProjectSettings/EditorBuildSettings.asset` enables exactly three project scenes in order: `Assets/_Game/Scenes/Boot/BootScene.unity`, `Menu/MenuScene.unity`, and `Battle/BattleScene.unity`. `GameBootstrapper.cs`, `AppServices.cs`, `GameSessionService.cs` and `UnitySceneLoadingService.cs` form the application composition and navigation path. The MVP handoff (`docs/architecture/mvp-implementation-summary.md`) describes Boot → Menu → mission/loadout → Battle → result/reward → persistence.

The Boot and Menu YAML files contain no obvious authored MonoBehaviour component records, which indicates runtime UI/controller construction or scene-independent bootstrap code. Battle contains `Main Camera`, `Directional Light`, and `Global Volume`; `BattleSceneController.cs` performs runtime battle setup, camera/input handling, unit views, trajectory display, rewards and overlays. Resources are loaded through stable `Resources/...` paths (for example character rigs and weapon icons), not Addressables.

## Gameplay and physics

The primary simulation is `Runtime/Gameplay/Battle/Simulation/BattleSimulation.cs`, implementing `IProjectileSpawner` and delayed spawns. It owns projectile stepping, impacts, status/timed effects, weapon-specific behavior, terrain carving, bounces and chained/split projectiles. The code exposes a fixed projectile sub-step length of `0.04f`; `BattleSceneController` drives the scene simulation at `1/120` second. `SimProjectile.cs` is the runtime projectile state container.

Aim and turn contracts are isolated in `AimCalculator.cs`, `AimState.cs`, `AimSettings.cs`, `TurnService.cs`, `TurnState.cs`, `TurnMovementState.cs`, `ShotCommand.cs` and `ITurnService.cs`. `WeaponExecutor.cs` dispatches effects by stable weapon id/role, while `ProjectileWeaponEffect.cs`, `BurstProjectileEffect.cs` and `ImpulseSalvoWeaponEffect.cs` implement projectile and special-shot families. `ProjectileDefinition.cs` and `WeaponDefinition.cs` hold authored/runtime weapon parameters.

Terrain is represented by `TerrainGrid.cs` and `BattleTerrain.cs`. `BattleTerrain` rasterizes alpha-based surfaces into a solidity grid (threshold `96`), supports procedural and sprite-derived cells, and updates destructible visuals after carving. `BattleTerrainSpriteSurface.cs`, `ArenaSpriteSurface.cs`, `ArenaTerrainBlock.cs`, `ArenaDefinition.cs` and `PrototypeArenaLibrary.cs` connect arena data to visual surfaces and spawn points. `DamageResolver.cs`, `DamageEvent.cs`, `RadialDamageSettings.cs` and `BattleResultEvaluator.cs` keep damage and outcome logic separate from rendering.

AI uses a reusable ballistic path: `BallisticsSolver.cs`, `TrajectoryProbe.cs`, `TrajectoryObstacles.cs`, `TrajectoryReport.cs`, `HeuristicShotPlanner.cs`, `SimpleAiShooter.cs`, `BotTurnController.cs` and movement planners under `AI/Movement`. Difficulty/profile content is provided by `BotProfile.cs`, `BotProfileLibrary.cs`, `BotProfileProvider.cs` and `AiProfileDefinition.cs`.

Important physics evidence: the controller has a 30-second player turn, explicit pointer dead zones, trajectory preview sample limits, fall-death height, projectile muzzle offset and camera zoom constants. This supports a mobile touch-first control model with explicit fire confirmation and readable trajectory previews. The domain is largely custom/deterministic rather than relying on Rigidbody2D for every gameplay decision.

## Data flow and persistence

`DefaultGameContentCatalog.cs` and `RuntimeGameContentCatalog.cs` provide runtime content: missions, weapons, archetypes, bot/difficulty profiles and arena definitions. `BattleLoadoutResolver.cs` combines player inventory with mission/opponent restrictions. During a battle, controller/input commands feed turn and weapon services; simulation emits feedback and result events; App bridges (`BattleFeedbackBridge`, `BattleRewardBridge`, `BattleActivityBridge`, `BattleTutorialBridge`) forward those to UI/audio/progression.

`SaveData.cs` is a versioned JSON model at schema version 7, covering profile, campaign, inventory, weapon progression, economy, settings, tutorial, activity counters and a bounded ledger. `JsonFileSaveService.cs`, `SaveDataSerializer.cs`, `SaveMigrationService.cs` and `SaveMigrationV0ToV1.cs` through `V6ToV7.cs` implement local persistence and upgrades. `InventoryService.cs`, `EconomyService.cs`, `CampaignProgressionService.cs`, `ActivityProgressService.cs`, `BattleRewardService.cs` and `WeaponUpgradeService.cs` mutate the loaded model and are covered by focused tests.

Localization uses 13 JSON dictionaries in `Assets/_Game/Content/Resources/Localization` (`ar`, `de`, `en`, `es`, `fr`, `it`, `ja`, `ko`, `pl`, `pt-BR`, `tr`, `uk`, `zh-Hans`). Each currently parses as valid JSON with 20 top-level keys. Runtime access is through `JsonLocalizationService.cs`, `LocalizationRuntime.cs`, `ILocalizationService.cs` and `GameText.cs`.

## Rendering, art and animation

Unity version is `6000.5.1f1` (`ProjectSettings/ProjectVersion.txt`). URP is configured through `GraphicsSettings.asset` with a custom Universal Render Pipeline asset and a mobile quality profile in `QualitySettings.asset`; Input System is enabled through `ProjectSettings/EditorBuildSettings.asset` and package `com.unity.inputsystem` 1.19.0. TMP/UGUI are used for runtime text and controls.

The project contains approximately 250 C# files, 260 PNGs, 5 animation clips and 4 prefabs under `Assets/_Game` (counted by extension during this scan). Character art is split into layered sprites and rigs for Droid, Raccoon and Sigma under `Assets/_Game/Art/Characters`; each rig has controller/idle assets, with movement clips for Droid and Sigma. Runtime character prefabs are in `Content/Resources/Visuals/Characters`. `WeaponProjectileSpriteAnimator.cs`, `TimedWeaponEffectVisual.cs`, `BattleUnitView.cs`, `BattleUnitPresenter.cs`, `AnimatedSkyBackground.cs`, `BattleVisualLayer.cs` and `BattleVisualThemeLibrary.cs` connect effects and visuals.

UI raster assets are organized under `Content/Resources/UI` into Battle, Common, Menu, Shop, Loaders, Flags, Weapons and result/reward subfolders. Fonts include Exo 2, Orbitron and CJK/Arabic fallback files. The art pipeline and arena integration rules are documented in `docs/design/unity-2d-art-pipeline.md` and `docs/design/arena-unity-integration-instructions.md`; editor automation is in `Runtime/../Editor/Art/Unity2DArtImporterAutomation.cs`.

## Integrations and platform services

Interfaces live in `Runtime/Infrastructure`: auth, cloud sync, remote config, purchases, premium status, notifications, analytics, ads and server time. No-op implementations keep offline/editor gameplay working (`NoOp*Service.cs`). Google Mobile Ads has concrete bootstrap/service files at `Runtime/Ads/GoogleMobileAdsBootstrap.cs` and `GoogleMobileAdsService.cs`, but the MVP handoff identifies real ads as outside the release slice.

`Game.Infrastructure.Ugs.asmdef` conditionally compiles adapters for Unity Services Core 1.18.0, Authentication 3.7.3, Cloud Save 3.4.1 and Remote Config 4.2.5. `UgsOnlineBootstrap.cs`, `UgsAuthService.cs`, `UgsCloudSaveSyncService.cs` and `UgsRemoteConfigService.cs` are the integration points. The code is designed to fall back to no-op services when initialization/network is unavailable. `docs/setup/ugs-setup.md` records required dashboard/environment setup and explicitly states that production account/service configuration remains a handoff step. Google Play Games project references exist in generated assemblies, but no verified production credentials or release flow were found in the scanned project files.

## Tests and verification surface

There are 44 test C# files under `Assets/_Game/Tests` (plus a small battle UI test assembly). EditMode coverage includes save/migrations, inventory, economy, progression, tutorial, infrastructure, audio, app flow and battle domain behavior. Gameplay tests cover aim, turns/movement, terrain grid, damage, simulation, weapon definitions/execution/inventory, AI ballistics/movement/difficulty/content, battle results and visual resources. `AppFlowSmokeTests.cs` is the PlayMode smoke entry point. The MVP summary reports 76 tests green and manual menu/battle/loadout/weapon smoke, but a fresh Unity test run and device/profile pass remain release gates.

## Gaps, risks and follow-up questions

1. **Content/config gap:** runtime libraries are code-defined (`Prototype*Library`, `MvpMissionLibrary`) and `Content/ScriptableObjects` is not populated with a broad authored catalog. Decide when to move balance/content from code into ScriptableObjects or remote data.
2. **Terrain status mismatch:** the MVP summary says destructible terrain is not in MVP, while `BattleTerrain` and the simulation already implement sprite/grid carving. Confirm whether this is shipped behavior, experimental code, or an incomplete integration path.
3. **Production integrations:** ads, purchases, analytics, notifications, online auth, Cloud Save and Remote Config have interfaces/adapters but require dashboard credentials, environment validation, error/restore flows and release testing.
4. **Presentation readiness:** visuals are still described as placeholders; final audio clips/mixer routing, complete animation coverage, VFX and device-specific UI validation are absent or not evidenced in the scanned content.

## Balance and monetization implementation status

The current slice now includes `HistoricalPowerService` with save schema 8 migration, `RewardedAdPolicy` guardrails, provider-agnostic product/receipt/entitlement contracts, and canonical campaign pacing metadata. These are local deterministic contracts; platform receipt validation, server-backed entitlement storage and actual matchmaking orchestration remain integration work.
5. **Asset loading/scaling:** runtime `Resources` paths are convenient for MVP but can increase memory coupling as content expands; evaluate Addressables or another explicit catalog before large live content.
6. **Verification:** run the complete Unity suite, build Android ARM64, profile allocations/frame rate, test a representative device, and review orientation/permissions/package metadata before distribution (as listed in the MVP summary).

## Key file index

- Composition: `Assets/_Game/Runtime/App/GameBootstrapper.cs`, `AppServices.cs`, `GameSessionService.cs`, `UnitySceneLoadingService.cs`.
- Battle entry: `Assets/_Game/Runtime/Gameplay/Battle/Assembly/BattleSceneController.cs`.
- Simulation: `.../Battle/Simulation/BattleSimulation.cs`, `SimProjectile.cs`.
- Rules: `.../Battle/Domain/TurnService.cs`, `AimCalculator.cs`, `DamageResolver.cs`, `BattleResultEvaluator.cs`.
- Weapons: `.../Battle/Weapons/WeaponDefinition.cs`, `WeaponExecutor.cs`, `ProjectileWeaponEffect.cs`, `BurstProjectileEffect.cs`.
- Terrain: `.../Battle/Terrain/BattleTerrain.cs`, `TerrainGrid.cs`, `BattleTerrainSpriteSurface.cs`.
- AI: `.../Battle/AI/Ballistics/BallisticsSolver.cs`, `BotTurnController.cs`, `HeuristicShotPlanner.cs`.
- Persistence: `Assets/_Game/Runtime/Data/Save/SaveData.cs`, `JsonFileSaveService.cs`, `SaveMigrationService.cs`.
- Platform boundary: `Assets/_Game/Runtime/Infrastructure/I*.cs`, `NoOp*.cs`, `Infrastructure/Ugs/*.cs`.
- Scenes/build: `Assets/_Game/Scenes/{Boot,Menu,Battle}`, `ProjectSettings/EditorBuildSettings.asset`, `ProjectSettings/ProjectVersion.txt`.
- Tests: `Assets/_Game/Tests/EditMode`, `Assets/_Game/Tests/PlayMode/AppFlowSmokeTests.cs`.
- Existing architectural source of truth: `docs/architecture/mvp-implementation-summary.md`, `docs/architecture/target-architecture.md`.
