# MVP Roadmap

Цей документ є головною пофазовою технічно-дизайнерською дорожньою картою до MVP. Його задача: дозволити різним сесіям і агентам брати наступний крок, реалізовувати його, перевіряти acceptance criteria і не ламати загальний напрям.

## Правило для кожної фічі

Перед початком будь-якої суттєвої фічі:

1. Прочитати `AGENTS.md` та\або 'CLAUDE.md''.
2. Прочитати релевантні docs у `docs/architecture/`, `docs/design/`, `docs/research/`.
3. Подивитися відповідники в `Examples/Warlings` і `Examples/Wormix`.
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

### Step 0.3. Prototype Isolation

Поточний `FairShotPrototype` лишити як reference slice, але ізолювати:

- namespace `Game.Prototype`;
- не змішувати з production gameplay;
- не будувати MVP поверх monolithic prototype script.

Acceptance criteria:

- prototype still runs;
- production folders готові для нової реалізації.

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

### Step 2.2. Placeholder Arena Set

Створити 2-3 власні placeholder arenas:

- simple valley;
- split hills;
- central cover.

Acceptance criteria:

- no first-turn cheap death;
- each arena readable on 16:9 and tall mobile aspect.

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

### Step 4.2. Result Screen

Створити:

- win/loss;
- mission reward placeholder;
- continue/retry.

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

### Step 5.2. Scene Loading Service

Створити:

- `ISceneLoadingService`;
- async scene loading wrapper;
- load menu/battle/result.

Acceptance criteria:

- scene names centralized;
- no direct scattered `SceneManager.LoadScene` in UI.

### Step 5.3. Game Session Service

Створити:

- selected mission;
- selected loadout;
- battle result payload;
- reward payload.

Acceptance criteria:

- result screen knows battle outcome;
- menu knows next mission.

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

### Step 6.2. Save Service

Створити:

- `ISaveService`;
- `JsonFileSaveService`;
- autosave on mission complete/settings change;
- manual save/load API.

Acceptance criteria:

- progress persists after editor play restart;
- corrupted save fallback safe with backup.

### Step 6.3. Save Migration

Створити:

- migration interface;
- version bump pattern;
- test for migration from v0/v1.

Acceptance criteria:

- old save can load;
- migration failures logged, not silently ignored.

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

### Step 7.2. Campaign Path

Створити:

- linear mission list;
- unlock next mission;
- replay completed mission;
- next mission CTA.

Acceptance criteria:

- after win mission 1 unlocks mission 2;
- loss allows retry.

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

Ціль: легка meta-progression без Wormix-like overload.

### Step 9.1. Inventory Model

Створити:

- owned weapon IDs;
- selected loadout;
- unlock status;
- simple validation.

Acceptance criteria:

- loadout persists;
- locked weapons unavailable.

### Step 9.2. Economy Service

Створити:

- one soft currency;
- grant/spend API;
- transaction reason enum/string;
- validation.

Acceptance criteria:

- mission rewards grant currency;
- spend cannot go negative.

### Step 9.3. Rewards Screen

Створити:

- earned currency;
- unlocked weapon/mission;
- continue CTA.

Acceptance criteria:

- reward result comes from mission config;
- reward applied once.

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
5. Phase 4.1: proper battle HUD.
6. Phase 5: boot/menu/battle/result flow.
7. Phase 6: save.
8. Phase 7-8: missions/tutorial.
9. Phase 9-10: rewards/loadout/AI.
10. Phase 11-14: feel, interfaces, tests, Android build.

