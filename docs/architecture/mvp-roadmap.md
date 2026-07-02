# MVP Roadmap

Цей документ є головною пофазовою технічно-дизайнерською дорожньою картою до MVP. Його задача: дозволити різним сесіям і агентам брати наступний крок, реалізовувати його, перевіряти acceptance criteria і не ламати загальний напрям.

## Стан реалізації (оновлюється)

Виконано: Phase 0 (foundation), Phase 1 (battle core), Phase 2 (arenas), Phase 3 (weapons: 5 зброй, inventory/executor/effects), Phase 4 (HUD, aim confirmation, result screen), Phase 5 (Boot→Menu→Battle→Result flow), Phase 6 (save/migrations), Phase 7 (8 missions + campaign), Phase 8 (tutorial system + first-battle coach), Phase 9 (economy/inventory + loadout екран), Phase 10 (AI + difficulty per mission), Phase 11 (settings/audio + feedback hooks), Phase 12 (no-op infrastructure interfaces). Також: 3 character archetypes.

Виконано додатково: Phase 13.2 PlayMode smoke, Phase 14.1 Android settings, HUD-контракти у `Game.Core.Battle`, uGUI-view у `Game.UI.Battle` через фабрику, `BurstProjectileEffect` як заготовка для role-специфічних ефектів.

MVP definition виконано. Лишилися ручні пункти: Phase 14.2 performance pass (потребує Profiler) і фінальний review gate — див. [phase-14-build-and-review.md](phase-14-build-and-review.md). Полірувальні борги: legacy uGUI `Text` без TMP/локалізації; role-специфічні ефекти зброї (`BurstProjectileEffect` є, але не інтегрований — поточна damage-модель ще не готова до multi-projectile per shot); архетипи-інструменти (cover/defensive/movement) як окремі ефекти.

## Правило для кожної фічі

Перед початком будь-якої суттєвої фічі:

1. Прочитати `AGENTS.md` та\або 'CLAUDE.md''.
2. Прочитати релевантні docs у `docs/architecture/`, `docs/design/`, `docs/research/`.
3. Подивитися відповідники у двох reference packages в `Examples/`.
4. Виписати короткий research note у task/PR/commit message або окремий doc, якщо зміна велика:

```text
Feature:
Example signals:
Useful principles:
Risks to avoid:
Our original implementation plan:
```

5. Реалізувати власну систему з власним кодом, назвами, конфігами, UI і балансом.

## Definition Of MVP

MVP вважається готовим, коли є:

- стабільний Unity-проєкт з `Assets/_Game/`;
- Boot -> Menu -> Battle -> Result flow;
- короткий tutorial;
- 8-12 campaign missions;
- 4-6 оригінальних weapons;
- 2-3 оригінальних character archetypes;
- 2-3 arenas;
- local save;
- basic loadout;
- one soft currency;
- simple rewards;
- audio/settings;
- no-op infrastructure interfaces for analytics, ads, IAP, remote config, notifications;
- мінімальний test/verification coverage для core battle logic і save migration.

MVP не включає:

- real ads SDK;
- real IAP;
- push notifications;
- backend login;
- ranked PvP;
- clans;
- complex crafting;
- many currencies;
- heavy live-ops.

## Phase 0. Project Foundation

Ціль: привести проєкт до структури, у якій кілька агентів можуть безпечно працювати над окремими модулями.

### Step 0.1. Folder Skeleton

Створити структуру:

```text
Assets/_Game/
  Runtime/Core
  Runtime/App
  Runtime/Gameplay
  Runtime/Progression
  Runtime/Economy
  Runtime/Inventory
  Runtime/Rewards
  Runtime/Tutorial
  Runtime/UI
  Runtime/Audio
  Runtime/Data
  Runtime/Infrastructure
  Content/ScriptableObjects
  Content/Prefabs
  Content/Art
  Content/Audio
  Content/Localization
  Scenes/Boot
  Scenes/Menu
  Scenes/Battle
  Tests/EditMode
  Tests/PlayMode
  Editor
```

Deliverables:

- folders and `.meta`;
- no unrelated Unity settings churn.

Acceptance criteria:

- Unity opens without compile errors;
- folders visible in Project window.

Status:

- Completed: `Assets/_Game/` skeleton, runtime module folders, content folders, scene folders, editor/tests folders and Unity `.meta` files exist.

### Step 0.2. Assembly Definitions

Додати asmdefs для core modules:

- `Game.Core`
- `Game.App`
- `Game.Gameplay`
- `Game.Data`
- `Game.UI`
- `Game.Infrastructure`
- `Game.Tests`

Dependency direction:

```text
Core <- Gameplay
Core <- Data
Core <- UI
Core <- Infrastructure
App -> all concrete modules
```

Acceptance criteria:

- circular dependencies відсутні;
- gameplay не залежить від infrastructure;
- tests можуть бачити Core/Gameplay/Data.

Status:

- Completed first pass: core asmdefs exist for App/Core/Data/Gameplay/UI/Infrastructure and edit-mode tests.
- `Game.App` currently references concrete modules for MVP composition.
- `Game.Gameplay` remains free from SDK/infrastructure dependencies.

### Step 0.3. Prototype Isolation

Поточний `FairShotPrototype` лишити як reference slice, але ізолювати:

- namespace `Game.Prototype`;
- не змішувати з production gameplay;
- не будувати MVP поверх monolithic prototype script.

Acceptance criteria:

- prototype still runs;
- production folders готові для нової реалізації.

Status:

- Completed: prototype is isolated under `Game.Prototype`; production battle logic lives under `Game.Gameplay.Battle`.

## Phase 1. Battle Core

Ціль: створити production-ready battle foundation, не монолітний prototype.

### Step 1.1. Battle Domain Model

Перед роботою:

- подивитися в прикладах ознаки battle screen, HUD, maps, weapons, unit categories;
- виписати, які systems явно потрібні, а які відкласти.

Створити чисті моделі:

- `BattleState`
- `BattleTeam`
- `BattleUnitState`
- `TurnState`
- `BattleResult`
- `DamageEvent`
- `ShotCommand`

Rules:

- models не залежать від MonoBehaviour;
- IDs stable string або struct;
- health/team/position/active turn explicit.

Acceptance criteria:

- edit-mode tests можуть створити battle state без scene;
- можна перевірити win/loss чистою функцією.

Status:

- Completed: production battle domain models and win/loss evaluator exist with edit-mode coverage.

### Step 1.2. Turn System

Створити:

- `ITurnService`
- `TurnService`
- turn order;
- start/end turn;
- one action per turn;
- battle locked while projectile in flight.

Acceptance criteria:

- tests: blue starts, after shot red turn, defeated unit skipped;
- no UI dependency.

Status:

- Completed: `TurnService` handles start, action spend and turn advance without UI dependency; covered by edit-mode tests.

### Step 1.3. Unit Runtime Controller

Створити runtime layer:

- `BattleUnitView`
- `BattleUnitPresenter`
- health display hook;
- fire point;
- selection/active indicator.

Acceptance criteria:

- scene can spawn two units from config/prefab;
- active unit visually obvious;
- damage updates view.

Status:

- Completed first pass: runtime battle scene spawns player/opponent presenters, active indicators and health visuals update during damage.

### Step 1.4. Aiming And Shot Input

Створити:

- `AimController`
- `ShotPowerModel`
- explicit fire confirmation;
- cancel aim;
- touch/mouse support via Input System.

Acceptance criteria:

- no accidental fire on pointer release;
- visible aim direction;
- power clamped;
- works in editor with mouse.

Status:

- Completed first pass: aim state, power clamp, trajectory preview, cancel and explicit fire confirmation work with editor mouse input.
- Pointer-over-UI and locked aim behavior prevent the fire button from changing aim.

### Step 1.5. Projectile Simulation

Створити:

- `ProjectileController`
- `ProjectileDefinition`
- gravity-based movement;
- collision detection;
- impact event.

Acceptance criteria:

- projectile arc readable;
- collision with terrain and unit works;
- owner collision ignored briefly or safely;
- projectile timeout handled.

Status:

- Completed first pass: projectile runtime uses gravity, collision, impact events and timeout cleanup.

### Step 1.6. Damage Resolver

Створити:

- `DamageResolver`
- radial damage;
- optional self-damage multiplier;
- damage events for UI/VFX.

Acceptance criteria:

- tests: damage decreases with distance;
- defeated units trigger battle result;
- no hidden random values in MVP.

Status:

- Completed: radial damage resolver and battle-result evaluation are covered by edit-mode tests.

### Step 1.7. Battle Scene Assembly

Створити scene:

```text
Assets/_Game/Scenes/Battle/BattleScene.unity
```

Composition:

- camera;
- arena placeholder;
- two teams;
- HUD;
- battle installer/composition root.

Acceptance criteria:

- Play starts battle scene directly;
- aim/fire/damage/turn/win/loss works;
- no prototype script required for production scene.

Status:

- Completed first pass: `BattleScene` runs production battle assembly without the prototype script and supports aim/fire/damage/turn/result loop.

## Phase 2. Terrain And Arena

Ціль: зробити arenas читабельними й готовими до майбутньої destructible системи.

### Step 2.1. Arena Definition

Створити:

- `ArenaDefinition`
- spawn points;
- camera bounds;
- terrain prefab reference;
- hazard flags later.

Research:

- подивитися, як у прикладах assets/maps/categories розділені за аренами;
- не копіювати maps або names.

Acceptance criteria:

- battle scene can load selected arena definition;
- spawn points safe and visible.

Status:

- Completed first pass: runtime `ArenaDefinition` and arena library drive camera, spawn points and terrain blocks.

### Step 2.2. Placeholder Arena Set

Створити 2-3 власні placeholder arenas:

- simple valley;
- split hills;
- central cover.

Acceptance criteria:

- no first-turn cheap death;
- each arena readable on 16:9 and tall mobile aspect.

Status:

- Completed first pass: three original placeholder arenas are available and selectable by runtime flow.

### Step 2.3. Terrain Interaction V1

MVP terrain interaction:

- collidable terrain;
- impact decal/crater visual;
- optional simple removable chunks later.

Not required for first MVP slice:

- full pixel-perfect destructible terrain.

Acceptance criteria:

- explosion feedback visible;
- terrain collision reliable.

Status:

- Completed first pass: terrain blocks are collidable and impacts create readable explosion feedback; real destructible terrain is deferred.

## Phase 3. Weapons

Ціль: перейти від одного shell до data-driven weapon system.

### Step 3.1. Weapon Data Model

Створити `WeaponDefinition` ScriptableObject:

- stable ID;
- display key;
- role;
- icon;
- projectile prefab;
- damage;
- radius;
- power range;
- ammo count;
- tutorial hint;
- tags.

Research:

- аналізувати categories і roles зброї в прикладах;
- не копіювати names/stats/icons/upgrades.

Acceptance criteria:

- weapon can be changed without code;
- tests or editor validation catch missing ID/display key.

Status:

- Completed first pass: `WeaponDefinition`, weapon roles and runtime prototype library exist with edit-mode validation.

### Step 3.2. Weapon Runtime

Створити:

- `WeaponInventoryRuntime`
- `WeaponExecutor`
- `IWeaponEffect`
- base projectile effect.

Acceptance criteria:

- active unit can select weapon;
- selected weapon fires with its own definition;
- ammo decreases if ammo enabled.

Status:

- Completed: `WeaponInventoryRuntime` and `WeaponExecutor` select/fire weapons and handle limited ammo through a projectile spawner abstraction.

### Step 3.3. MVP Weapon Set

Створити 4-6 оригінальних weapons by role:

- direct projectile;
- arcing explosive;
- short-range burst;
- cover/placement tool;
- defensive tool;
- movement/repositioning tool.

Acceptance criteria:

- кожна зброя має власну tactical purpose;
- no weapon creates unavoidable first-turn cheap loss;
- animations/effects короткі.

Status:

- Completed first pass: five original projectile-role weapons exist with distinct damage/radius/power/ammo profiles.
- Cover/defensive/movement archetypes are intentionally deferred until arena/domain mutation is ready.

## Phase 4. UI And UX

Ціль: зробити battle playable на mobile без clutter.

### Step 4.1. Battle HUD

Створити:

- active unit panel;
- health indicators;
- power indicator;
- fire/cancel buttons;
- weapon button;
- end/restart in debug or pause.

Research:

- подивитися HUD/layout categories у прикладах;
- виписати, що перевантажує старі UI;
- зробити власний simpler HUD.

Acceptance criteria:

- touch targets readable;
- no overlap on common mobile aspect ratios;
- fire confirmation explicit.

Status:

- Completed first pass: runtime `BattleHudView` shows health, turn/status, weapon/ammo, power and fire/cancel/restart controls.
- Fire confirmation is explicit, aim does not change when clicking UI, and visible text is Ukrainian for the MVP slice.
- Known gap: HUD currently lives in `Game.Gameplay`; moving it to `Game.UI` and replacing legacy `Text` with TMP/localization is deferred.

### Step 4.2. Result Screen

Створити:

- win/loss;
- mission reward placeholder;
- continue/retry.

Status:

- Completed as runtime battle HUD overlay.
- `Retry` reloads the current runtime arena.
- `Continue` returns from battle result overlay to `MenuScene`; separate Result scene is deferred.

Acceptance criteria:

- battle result leads to result UI;
- retry reloads battle;
- continue returns to menu later.

### Step 4.3. Main Menu V1

Створити:

- next mission CTA;
- loadout button;
- settings button;
- debug direct battle button during MVP development.

Acceptance criteria:

- first screen is usable, not landing page;
- player can enter next mission quickly.

Status:

- Completed as runtime-built `MenuSceneController` in `MenuScene`.
- Main CTA starts the next battle.
- Loadout/settings buttons are present as MVP placeholders with Ukrainian status feedback.

## Phase 5. App Flow And Scenes

Ціль: повний Boot -> Menu -> Battle -> Result loop.

### Step 5.1. Boot Scene

Створити:

- `GameBootstrapper`;
- service composition;
- load configs;
- load save;
- go to menu.

Acceptance criteria:

- boot scene is first in build settings;
- missing save creates new profile.

Status:

- Completed for MVP flow: `BootScene` is first in build settings.
- `GameBootstrapper` initializes app services and routes to `MenuScene`.
- Save/profile creation is deferred to Phase 6.

### Step 5.2. Scene Loading Service

Створити:

- `ISceneLoadingService`;
- async scene loading wrapper;
- load menu/battle/result.

Acceptance criteria:

- scene names centralized;
- no direct scattered `SceneManager.LoadScene` in UI.

Status:

- Completed first pass: `GameSceneNames` centralizes scene names and paths.
- `ISceneLoadingService` + `UnitySceneLoadingService` load scenes for App/UI flow.
- Legacy direct scene reload remains inside battle runtime for local restart; full cleanup can happen when battle exits through App composition.

### Step 5.3. Game Session Service

Створити:

- selected mission;
- selected loadout;
- battle result payload;
- reward payload.

Acceptance criteria:

- result screen knows battle outcome;
- menu knows next mission.

Status:

- Completed first pass: `GameSessionService` stores selected mission index.
- Menu can select/start the next mission.
- Battle result payload and real reward payload are deferred until separate Result/Save/Economy layers.

## Phase 6. Data And Save

Ціль: зберігати прогрес локально без backend.

### Step 6.1. Save Model

Створити:

- `SaveData`;
- schema version;
- player profile;
- completed missions;
- unlocked weapons;
- selected loadout;
- soft currency;
- settings;
- tutorial progress.

Acceptance criteria:

- save serializes/deserializes JSON;
- missing fields handled by defaults.

Status:

- Completed first pass: `SaveData` stores schema version, profile, campaign, inventory, economy, settings and tutorial sections.
- `SaveDataSerializer` serializes/deserializes JSON and fills defaults for missing sections.
- Edit-mode tests cover default creation, JSON roundtrip and missing-field defaults.

### Step 6.2. Save Service

Створити:

- `ISaveService`;
- `JsonFileSaveService`;
- autosave on mission complete/settings change;
- manual save/load API.

Acceptance criteria:

- progress persists after editor play restart;
- corrupted save fallback safe with backup.

Status:

- Completed first pass: `ISaveService` + `JsonFileSaveService` read/write local JSON at `Application.persistentDataPath`.
- Missing save creates a new profile during app bootstrap.
- Corrupt save creates a backup copy and recovers with a new default profile.
- Menu shows a minimal profile/currency/mission summary from loaded save.
- Autosave on mission rewards/settings changes is deferred until Phase 7/9/11 add those real mutations.

### Step 6.3. Save Migration

Створити:

- migration interface;
- version bump pattern;
- test for migration from v0/v1.

Acceptance criteria:

- old save can load;
- migration failures logged, not silently ignored.

Status:

- Completed first pass: `ISaveMigration`, `SaveMigrationService`, v0->v1 and v1->v2 migrations exist.
- `SaveDataSerializer` migrates loaded JSON to `SaveData.CurrentSchemaVersion`.
- Tests cover v1 migration and rejection of newer unsupported schemas.

## Phase 7. Progression And Missions

Ціль: 8-12 handcrafted missions з поступовим навчанням.

### Step 7.1. Mission Definition

ScriptableObject:

- mission ID;
- title key;
- arena;
- player team setup;
- enemy team setup;
- allowed weapons;
- objectives;
- rewards;
- tutorial steps optional.

Research:

- подивитися, як приклади розділяють maps/modes/progression categories;
- не копіювати missions/maps/balance.

Acceptance criteria:

- mission can launch battle with selected config;
- mission completion saved.

Status:

- Completed first pass: `Game.Progression` contains `MissionDefinition`, reward data and an 8-mission MVP runtime library.
- Menu reads the next mission title/description/reward from campaign data.
- Battle receives selected arena and allowed weapon IDs through `GameSessionContext`.
- Known gap: authored ScriptableObject assets and editor validation are deferred; runtime definitions drive the MVP slice.

### Step 7.2. Campaign Path

Створити:

- linear mission list;
- unlock next mission;
- replay completed mission;
- next mission CTA.

Acceptance criteria:

- after win mission 1 unlocks mission 2;
- loss allows retry.

Status:

- Completed first pass: `CampaignProgressionService` unlocks the next mission, applies rewards once and persists via local save when returning from a successful battle.
- Loss still supports retry without applying progress.
- Tests cover reward application, idempotency, weapon unlock and next mission lookup.

### Step 7.3. MVP Mission Set

План 8-12 missions:

1. Aim and fire.
2. Arc shot.
3. Movement and cover.
4. Multiple enemy units.
5. Weapon selection.
6. Defensive tool.
7. Terrain trick.
8. First mini-boss or harder duel.
9-12 optional challenge missions.

Acceptance criteria:

- кожна місія вчить одну нову річ;
- no mission requires hidden knowledge.

## Phase 8. Tutorial

Ціль: коротке навчання без нудного коридору.

### Step 8.1. Tutorial Step System

Створити:

- `TutorialStepDefinition`;
- trigger conditions;
- completion conditions;
- UI target references;
- tutorial state in save.

Acceptance criteria:

- tutorial can wait for aim/fire/hit/win;
- completed steps persist.

Status:

- Completed first pass: `Game.Tutorial` contains step definitions, trigger/completion enums, first-battle runtime step library and `TutorialService`.
- Tutorial completion persists through `SaveData.tutorial.completedStepIds`.
- Tests cover trigger lookup, prerequisite gating and idempotent completion.
- Visual tutorial overlay and battle event wiring are deferred to Phase 8.2.

### Step 8.2. First Battle Tutorial

Покрити:

- aim;
- power;
- fire;
- projectile arc;
- damage;
- turn switch;
- win.

Acceptance criteria:

- гравець стріляє в першу хвилину;
- tutorial не блокує довше, ніж треба.

## Phase 9. Inventory, Loadout, Economy, Rewards

Ціль: легка meta-progression без overload із progression-heavy reference.

### Step 9.1. Inventory Model

Створити:

- owned weapon IDs;
- selected loadout;
- unlock status;
- simple validation.

Acceptance criteria:

- loadout persists;
- locked weapons unavailable.

Status:

- Completed first pass: `Game.Inventory` contains `InventoryService` for weapon unlocks and loadout validation over `SaveData`.
- Tests cover idempotent unlock, locked weapon rejection and duplicate removal.
- Loadout UI is deferred.

### Step 9.2. Economy Service

Створити:

- one soft currency;
- grant/spend API;
- transaction reason enum/string;
- validation.

Acceptance criteria:

- mission rewards grant currency;
- spend cannot go negative.

Status:

- Completed first pass: `Game.Economy` contains `EconomyService` with grant/spend validation for one soft currency.
- App composition creates the service over loaded local save.
- Tests cover grant, successful spend and insufficient-balance rejection.
- Mission rewards currently apply through campaign service over save data; routing all reward grants through economy service is a later cleanup.

### Step 9.3. Rewards Screen

Створити:

- earned currency;
- unlocked weapon/mission;
- continue CTA.

Acceptance criteria:

- reward result comes from mission config;
- reward applied once.

Status:

- Completed first pass: battle result preview uses selected mission reward data from `GameSessionContext`.
- Successful `Continue` applies mission completion through campaign progression and local save.
- Menu shows a short applied reward status after return.
- Full standalone reward screen and receipt model are deferred.

## Phase 10. AI

Ціль: offline AI battles для MVP.

### Step 10.1. Simple AI Shooter

Створити:

- target selection;
- approximate aim;
- power estimate;
- random error by difficulty;
- fire selected weapon.

Research:

- подивитися, які PvE/bot signals є в прикладах;
- не копіювати algorithms, тільки принцип: AI має давати readable challenge.

Acceptance criteria:

- player can play against AI;
- easy AI misses sometimes;
- AI turn not too slow.

Status:

- Completed first pass: `SimpleAiShooter` estimates direction/power toward a living player unit and opponent turns auto-fire after a short delay.
- AI uses the existing battle fire path, turn locks and weapon executor.
- Tests cover aim direction and power bounds.
- Difficulty miss/error tuning is deferred to Phase 10.2.

### Step 10.2. AI Difficulty

Рівні:

- easy: larger aim error;
- normal: moderate;
- hard later: better weapon choice.

Acceptance criteria:

- difficulty changes behavior visibly;
- AI does not use unavailable weapons.

## Phase 11. Audio, VFX, Feel

Ціль: зробити battle readable and satisfying.

### Step 11.1. VFX Hooks

Events:

- aim start;
- fire;
- projectile trail;
- impact;
- damage;
- defeat;
- victory.

Acceptance criteria:

- events exist even if visuals are placeholders;
- gameplay logic не залежить від VFX.

### Step 11.2. Audio Service V1

Створити:

- `IAudioService`;
- SFX play API;
- music placeholder;
- volume settings.

Acceptance criteria:

- settings persist;
- gameplay can request SFX through service/interface.

Status:

- Completed first pass: `Game.Audio` contains `IAudioService` and `NoOpAudioService`.
- Audio volume settings persist through `SaveData.settings`.
- Real audio playback, mixer routing and gameplay SFX calls are deferred until assets/feel pass.

## Phase 12. Infrastructure Interfaces

Ціль: підготувати місце для SDK без додавання SDK.

### Step 12.1. No-op Services

Interfaces:

- `IAnalyticsService`
- `IAdsService`
- `IPurchaseService`
- `IRemoteConfigService`
- `INotificationService`

No-op implementations:

- log/debug only;
- no external dependency.

Research:

- подивитися SDK categories у прикладах;
- визначити, які events/placements можуть знадобитися later;
- не інтегрувати SDK у MVP без окремого рішення.

Acceptance criteria:

- game compiles without SDK packages;
- UI/gameplay call interfaces only.

Status:

- Completed first pass: no-op interfaces/implementations exist for analytics, ads, purchases, remote config and notifications.
- `AppServices` exposes infrastructure abstractions without real SDK packages or permissions.
- Tests cover no-op remote config, ads and purchase behavior.

## Phase 13. Testing And Verification

Ціль: не втратити trust через поломки.

### Step 13.1. EditMode Tests

Тести:

- damage falloff;
- turn switching;
- win/loss;
- economy grant/spend;
- save serialize/migrate;
- mission unlock.

Acceptance criteria:

- tests run in Unity Test Runner;
- core logic covered without scene.

### Step 13.2. PlayMode Smoke Tests

Smoke:

- boot to menu;
- start mission;
- finish battle through scripted shortcut or test hook;
- result saves progress.

Acceptance criteria:

- repeatable in editor.

### Step 13.3. Manual QA Checklist

Перед кожним milestone:

- no compile errors;
- Play works from Boot scene;
- first battle reachable;
- no UI overlap on desktop and mobile aspect;
- controls readable;
- save persists;
- no SDK required.

## Phase 14. MVP Polish And Build

Ціль: підготувати перший playable APK/internal build.

### Step 14.1. Android Settings Review

Перевірити:

- orientation;
- resolution/aspect;
- input;
- package name;
- icons placeholders;
- permissions minimal.

Research:

- подивитися permissions у прикладах;
- не додавати permissions без потреби.

Acceptance criteria:

- Android build succeeds;
- permissions list minimal.

### Step 14.2. Performance Pass

Перевірити:

- frame rate in battle;
- projectile/VFX cleanup;
- allocations in turn loop;
- scene loading time.

Acceptance criteria:

- no obvious leaks from spawned projectiles/effects;
- stable play for several battles.

### Step 14.3. MVP Review Gate

Перед “MVP complete” перевірити:

- чи core shot feel чесний;
- чи UI не перевантажений;
- чи progression не ховає fun;
- чи немає paid/SDK dependency;
- чи docs відповідають реалізації;
- чи кожна велика система мала reference consultation.

## Agent Handoff Template

Кожен агент, завершуючи задачу, має залишити короткий handoff:

```text
Completed:
Files changed:
Example research consulted:
Design/architecture decision:
How to verify:
Known gaps:
Recommended next step:
```

## Порядок реалізації після поточного прототипу

Найкращий наступний порядок:

1. Phase 0: folder/asmdef foundation.
2. Phase 1.1-1.7: production battle core.
3. Phase 2.1-2.2: arena definitions and placeholders.
4. Phase 3.1-3.3: data-driven weapons.
5. Phase 4.1-4.2: proper battle HUD and result overlay.
6. Phase 5: boot/menu/battle app flow.
7. Phase 6: save.
8. Phase 7-8: missions/tutorial.
9. Phase 9-10: rewards/loadout/AI.
10. Phase 11-14: feel, interfaces, tests, Android build.
