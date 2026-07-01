# Production Battle Scene

Це перша production-oriented battle scene за roadmap `Phase 1.7`, тепер розширена першим шаром data-driven арен з `Phase 2.1`.

## Як запустити

1. Дочекатися, поки Unity завершить компіляцію.
2. Відкрити:

```text
Assets/_Game/Scenes/Battle/BattleScene.unity
```

3. Натиснути Play.

`BattleSceneBootstrap` автоматично додає `BattleSceneController`, якщо активна сцена називається `BattleScene`.

## Чим відрізняється від Fair Shot Prototype

`SampleScene` досі може запускати старий `FairShotPrototype` як reference slice.

`BattleScene` використовує production modules:

- `BattleState`
- `TurnService`
- `AimCalculator`
- `BattleUnitView`
- `BattleUnitPresenter`
- `ProjectileController`
- `DamageResolver`
- `BattleResultEvaluator`
- `ArenaDefinition`
- `PrototypeArenaLibrary`
- `WeaponDefinition`
- `PrototypeWeaponLibrary`

## Керування

- Mouse/touch: aim and power.
- `Fire` або `Space`: постріл.
- `Restart` або `R`: restart scene.
- `Next Arena` або `N`: перейти до наступної runtime prototype arena.
- `Next Weapon` або `Tab`: перейти до наступної runtime prototype weapon.

## Phase 2.1: Runtime Arena Definitions

Battle scene більше не будує світ лише з hardcoded `BuildWorld`.

Додано власний data layer:

- `ArenaTerrainBlock`: один terrain/background block з position, size, color, collision flag і sorting order.
- `ArenaDefinition`: опис арени: id, display name, camera, player/opponent spawn points, terrain blocks.
- `PrototypeArenaLibrary`: три оригінальні runtime arenas для MVP testing: `Training Meadow`, `Split Ridge`, `Central Cover`.

Наступний крок після ручного Play Mode тесту: перенести підтверджену форму цих даних у реальні ScriptableObject assets або залишити runtime library як швидкий prototype catalog до появи editor tooling.

## Phase 3.1: Runtime Weapon Definitions

Battle scene більше не використовує один hardcoded projectile definition.

Додано власний перший weapon data layer:

- `WeaponRole`: tactical role зброї.
- `WeaponDefinition`: stable ID, display key, role, icon slot, damage, impact radius, projectile radius, gravity, timeout, power range, ammo count, tutorial hint, tags.
- `PrototypeWeaponLibrary`: три оригінальні runtime weapons для MVP testing: `weapon.steady-orb`, `weapon.pulse-shell`, `weapon.close-pop`.

`BattleSceneController` тепер бере projectile/damage/radius/power range з активної зброї.

## Phase 3.2: Weapon Runtime

Виконання зброї винесено зі сцени в окремий runtime-шар:

- `WeaponInventoryRuntime`: стан вибору зброї та боєзапасу для одного комбатанта (чиста логіка, edit-mode тести).
- `IWeaponEffect` + `ProjectileWeaponEffect`: поведінка пострілу; базовий ефект запускає один балістичний снаряд з `WeaponDefinition`.
- `WeaponFireContext` + `ProjectileSpawnRequest` + `IProjectileSpawner`: дані пострілу й абстракція спавна, щоб ефекти не торкались GameObject-ів напряму.
- `WeaponExecutor`: валідує боєзапас і dispatch-ить ефект за роллю (з fallback на базовий).

`BattleSceneController` реалізує `IProjectileSpawner`; `Fire()` делегує постріл `WeaponExecutor`, а створення снаряда живе в `Spawn`. Боєзапас `-1` — нескінченний; обмежена зброя декрементиться і блокує постріл на нулі (HUD показує `Ammo N` / `Ammo ∞`).

## Що перевірити вручну

- У Console немає червоних compile/runtime errors.
- У сцені видно фон, terrain blocks, два юніти й HUD.
- На HUD видно назву поточної арени.
- `N` або `Next Arena` перемикає 3 арени без накопичення старих колайдерів/юнітів.
- На HUD видно активну зброю.
- `Tab` або `Next Weapon` перемикає 3 зброї.
- На HUD видно боєзапас активної зброї (`Ammo ∞` для стартових).
- У різної зброї відрізняються power range, damage/radius і вигляд снаряда.
- Білий active indicator стоїть над активним юнітом.
- Health bars видно над обома юнітами.
- Приціл і trajectory preview рухаються за мишкою.
- Постріл летить дугою.
- Після impact HP змінюється.
- Хід переходить до іншої команди.
- Win/loss показується після знищення юніта.

## Поточні обмеження

- Placeholder visuals.
- Немає AI.
- Немає production HUD canvas.
- Є weapon data layer + runtime inventory/executor, але ще немає production weapon UI (тільки debug HUD) і поки один спільний projectile-ефект на всі ролі.
- Terrain collision є, але destructible terrain ще немає.
