# Reference Consultations

Цей журнал фіксує короткі консультації з прикладами перед суттєвими фічами. Він не містить копій чужих файлів, assets, назв, конфігів або логіки; лише власні висновки.

## Phase 0: Project Foundation

Feature:
: Folder skeleton and assembly definitions.

Example signals:
: `Examples/Wormix` має виразні asset/data категорії `maps`, `ui`, `sounds`, `particles`, `layouts`, `learning`. `Examples/Warlings` у base APK показує категорії на кшталт `game_menu`, `maps`, `weapons`, `rewards`, `shop`, `particles`, `environment`.

Useful principles:
: Великі мобільні tactical games швидко накопичують окремі шари для battle, UI, maps, weapons, rewards, shop, sounds, particles і SDK. Їх треба розділяти рано, щоб системи не злиплись.

Risks to avoid:
: Не будувати Unity-проєкт як копію asset tree з прикладів. Не змішувати SDK, economy, UI і gameplay в одному runtime namespace. Не будувати MVP поверх монолітного prototype script.

Our original implementation plan:
: Створити власну clean Unity-структуру `Assets/_Game` з модулями `Core`, `App`, `Gameplay`, `Data`, `UI`, `Infrastructure`, `Content`, `Scenes`, `Tests`. Додати asmdef-и з dependency direction, де gameplay не залежить від infrastructure.

## Phase 1.1: Battle Domain Model

Feature:
: Чисті battle domain models без MonoBehaviour.

Example signals:
: В обох прикладах battle layer проявляється як окрема група assets і систем: `game_menu`, `maps`, `weapons`, `particles`, battle/result UI, map content, weapon content. Це натякає, що бій має бути ядром, відділеним від shop/rewards/SDK.

Useful principles:
: Battle state має явно знати teams, units, active turn, health, shot intent і result. Win/loss evaluation має бути незалежним від UI, SDK і scene objects.

Risks to avoid:
: Не починати production battle core з монолітного runtime script. Не змішувати damage/win/loss із UI callbacks. Не копіювати чужі назви класів або формули.

Our original implementation plan:
: Створити власні `BattleState`, `BattleTeamState`, `BattleUnitState`, `TurnState`, `ShotCommand`, `DamageEvent`, `BattleResult` і `BattleResultEvaluator` у `Game.Gameplay.Battle.Domain`, плюс перші edit-mode tests для win/loss.

## Phase 1.2: Turn System

Feature:
: Чистий turn service для start/spend/advance turn.

Example signals:
: У прикладах жанрова структура явно обертається навколо покрокового battle loop, weapon action і result flow. З asset categories видно окремі battle HUD/weapon/map layers, але turn logic не має бути зашита в UI.

Useful principles:
: Turn ownership, active unit, action spent і перехід ходу мають бути явним state. UI лише відображає стан і надсилає command.

Risks to avoid:
: Не прив’язувати зміну ходу до animation/UI callbacks. Не дозволяти другий постріл у той самий turn. Не робити hidden turn state у MonoBehaviour-only полі.

Our original implementation plan:
: Додати `ITurnService` і `TurnService`, які працюють із `BattleState`, вміють стартувати перший хід, позначати action spent, перемикати команду і пропускати defeated units. Покрити edit-mode tests.

## Phase 1.3: Unit Runtime Controller

Feature:
: Runtime view/presenter для battle units.

Example signals:
: У прикладах є виразні категорії персонажів/units/customization/HUD, але вони змішані з великою кількістю assets і UI. Для MVP нам потрібна тонша межа: state окремо, view окремо.

Useful principles:
: Unit має мати visible body, active indicator, fire point і health feedback. View не має вирішувати battle rules.

Risks to avoid:
: Не зберігати health або team truth тільки в MonoBehaviour. Не прив’язувати win/loss до sprite state. Не копіювати чужі unit/class names або visuals.

Our original implementation plan:
: Додати `BattleUnitView` для Unity-відображення і `BattleUnitPresenter`, який синхронізує `BattleUnitState` з view. Це підготує production battle scene без залежності від prototype script.

## Phase 1.4: Aiming Foundation

Feature:
: Чиста математика aim direction і shot power.

Example signals:
: В аналізі відгуків найболючіший сигнал - незручний або випадковий постріл. У жанрових прикладах battle fun напряму залежить від того, чи гравець довіряє прицілу, силі й траєкторії.

Useful principles:
: Aim direction, power clamp і мінімальна дистанція input мають бути явними та тестованими. Fire має лишатися explicit, а не випадковим release.

Risks to avoid:
: Не ховати формулу сили всередині UI drag handler. Не робити accidental fire. Не копіювати чужі формули або input behavior.

Our original implementation plan:
: Додати `AimSettings`, `AimState`, `AimCalculator` як чисті gameplay domain utilities з edit-mode tests. Runtime input controller буде наступним шаром поверх цієї математики.

## Phase 1.5: Projectile Foundation

Feature:
: Data/runtime foundation для projectile simulation.

Example signals:
: У прикладах багато weapon/projectile/particles/map assets. Це підтверджує, що projectile behavior, impact feedback і damage мають бути окремими системами, які пізніше можна розширювати.

Useful principles:
: Projectile має мати data definition, runtime controller, impact event і timeout. Damage/effects мають слухати impact, а не бути зашитими в projectile prefab.

Risks to avoid:
: Не змішувати projectile movement, damage, UI і rewards. Не копіювати чужі weapon/projectile names або exact tuning values.

Our original implementation plan:
: Додати `ProjectileDefinition`, `ProjectileImpact`, `ProjectileController`. Контролер відповідає за flight/collision/impact event, а damage resolver підключиться окремо.

## Phase 1.6: Damage Resolver

Feature:
: Чиста radial damage система.

Example signals:
: У відгуках recurring pain - cheap losses і незрозумілий баланс. Це означає, що damage formulas мають бути явними, простими й тестованими.

Useful principles:
: Damage має падати з distance, мати чіткий radius, max damage і self-damage multiplier. Події damage потрібні для UI/VFX, але сама формула не залежить від UI/VFX.

Risks to avoid:
: Не ховати damage у projectile prefab або animation callback. Не переносити чужі weapon stats. Не додавати random damage у MVP.

Our original implementation plan:
: Додати `RadialDamageSettings` і `DamageResolver.ResolveRadialDamage` з edit-mode tests для falloff, radius і self-damage.

## Phase 1.7: Battle Scene Assembly

Feature:
: Production-oriented battle scene assembly.

Example signals:
: Приклади мають окремі battle/map/weapon/HUD/result asset categories. Це підтверджує, що сцена бою має бути композицією кількох систем, а не одним змішаним script.

Useful principles:
: Battle scene повинна збирати state, unit views, input/aim, projectile, damage і result flow через окремі модулі. Prototype можна лишити як reference, але MVP має перейти на production modules.

Risks to avoid:
: Не будувати далі поверх `FairShotPrototype`. Не змішувати scene bootstrap із SDK/economy. Не копіювати чужі scene layouts або maps.

Our original implementation plan:
: Додати `BattleSceneController` і `BattleSceneBootstrap`, які запускаються лише в `BattleScene`. Створити `Assets/_Game/Scenes/Battle/BattleScene.unity` як окрему сцену, де runtime assembly використовує `BattleState`, `TurnService`, `AimCalculator`, `ProjectileController`, `DamageResolver`, `BattleResultEvaluator`.

## Phase 2.1: Runtime Arenas

Feature:
: Перший data-driven шар арен для `BattleScene`.

Example signals:
: У прикладах карти винесені в окремі content/data групи, а не змішані з UI, shop або SDK. У Warlings APK видно окремі `assets/maps/...`, у Wormix є `assets/data/maps`.

Useful principles:
: Арена має бути окремою сутністю з camera framing, spawn points і terrain/collision blocks. Battle scene повинна збирати її з визначення, а не тримати всі координати в одному методі.

Risks to avoid:
: Не копіювати чужі maps, layouts, coordinates, assets або names. Не переносити чужу файлову структуру 1:1.

Our original implementation plan:
: Додано `ArenaTerrainBlock`, `ArenaDefinition`, `PrototypeArenaLibrary` і 3 оригінальні runtime arenas. `BattleSceneController` перемикає їх через `N`/`Next Arena`.

## Phase 3.1: Runtime Weapons

Feature:
: Перший data-driven шар зброї для `BattleScene`.

Example signals:
: У Warlings APK видно окремі battle HUD/weapon-related assets у `assets/game_menu`, включно з weapon UI/count/modifier елементами. Відгуки гравців також підкреслюють важливість різної зброї з чіткими tactical roles.

Useful principles:
: Зброя має бути окремою data/content сутністю: stable ID, role, damage/radius, projectile behavior, power range, ammo/tutorial metadata. Battle scene не повинна напряму знати всі числа конкретної зброї.

Risks to avoid:
: Не копіювати чужі weapon names, icons, stats, formulas, upgrade trees, balance або UI layouts. Не стартувати з великого арсеналу до стабілізації core loop.

Our original implementation plan:
: Додано `WeaponRole`, `WeaponDefinition`, `PrototypeWeaponLibrary` і 3 оригінальні runtime weapons. `BattleSceneController` перемикає їх через `Tab`/`Next Weapon` і бере projectile/damage/radius/power range з активної `WeaponDefinition`.
