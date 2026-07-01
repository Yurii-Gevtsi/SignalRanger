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
- HUD `FIRE` button або `Space`: постріл (явне підтвердження).
- HUD `Cancel` button або `Esc`: скинути приціл.
- HUD `Weapon` button або `Tab`: наступна зброя.
- HUD `Restart` button або `R`: restart scene.
- `N` (debug, keyboard): наступна runtime prototype arena.

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

## Phase 3.3: MVP Weapon Set (projectile-роль)

`PrototypeWeaponLibrary` розширено до 5 зброй з різними tactical niches у межах наявного `ProjectileWeaponEffect`:

| ID | Роль | Ніша |
|---|---|---|
| `weapon.steady-orb` | DirectProjectile | стартова, низька дуга, збалансована |
| `weapon.long-lance` | DirectProjectile | precision: пласка/швидка, малий радіус, високий одиночний damage |
| `weapon.pulse-shell` | ArcingExplosive | стартова навісна, збалансований радіус/damage |
| `weapon.scatter-arc` | ArcingExplosive | area: висока дуга, широкий м'який blast, тиск на позицію |
| `weapon.close-pop` | ShortRangeBurst | ближній burst, коротка дальність |

Баланс: жодна зброя не завдає ≥100 damage (юніт стартує зі 100 HP), тож немає one-shot із повного HP на першому ході (покрито тестом `PrototypeWeapons_CannotOneShotFullHealthUnit`).

Ще не реалізовані архетипи-інструменти (cover/defensive/movement) винесені в окрему пропозицію архітектури — вони потребують нових core-абстракцій (мутація арени/юнітів), а не лише даних.

## Phase 4.1: Battle HUD

IMGUI debug-панель (`OnGUI`) замінена на справжній mobile uGUI HUD:

- `BattleHudState`: незмінний snapshot стану бою, який сцена штовхає у HUD щокадру (HUD не знає gameplay-типів).
- `IBattleHud`: контракт HUD (події `FireRequested` / `CancelRequested` / `NextWeaponRequested` / `RestartRequested` + `Render`).
- `BattleHudView`: runtime-побудований `Canvas` (`ScaleWithScreenSize`, ref 1080×1920) з top status/turn bar, per-team health bars, power meter і великими touch-кнопками `FIRE` / `Cancel` / `Weapon` / `Restart`.

Елементи заякорені по кутах екрана → не перекриваються на різних mobile-співвідношеннях. `FIRE` явно неактивна, коли постріл неможливий (немає боєзапасу, снаряд у польоті, бій завершено). `BattleSceneController` спілкується лише через `IBattleHud`.

Відомий gap: HUD поки живе у збірці `Game.Gameplay` (додано reference `UnityEngine.UI`), а не в `Game.UI` — бо `Game.UI` бачить лише `Game.Core`. Перенесення потребує Core-рівневого view-model і виділення composition root у `Game.App`; заплановано пізніше.

## Що перевірити вручну

- У Console немає червоних compile/runtime errors.
- У сцені видно фон, terrain blocks, два юніти й uGUI HUD.
- HUD-панелі (status/turn, health, power, кнопки) не перекриваються — перевірити в Game view на 16:9 landscape і tall mobile (напр. 9:19.5).
- Top bar показує status і `Turn N — Team`.
- Per-team health bars згори зменшуються після влучань.
- `N` (keyboard debug) перемикає 3 арени без накопичення старих колайдерів/юнітів.
- HUD `Weapon` button / `Tab` перемикає 5 зброй; показує назву + боєзапас (`Ammo ∞`).
- У різної зброї відрізняються power range, damage/radius і вигляд снаряда.
- Power meter заповнюється залежно від сили прицілювання.
- `FIRE` неактивна під час польоту снаряда і після завершення бою.
- `Cancel` / `Esc` скидає приціл.
- Приціл і trajectory preview рухаються за мишкою/пальцем.
- Постріл летить дугою; після impact HP змінюється; хід переходить до іншої команди.
- Win/loss показується у status після знищення юніта.

## Поточні обмеження

- Placeholder visuals.
- Немає AI.
- HUD-текст поки через legacy uGUI `Text` (без TMP/локалізації); показує raw weapon ID.
- HUD у збірці `Game.Gameplay`, ще не в `Game.UI`.
- Є weapon inventory/executor + 5 зброй, але поки один спільний projectile-ефект на всі ролі (без cover/defensive/movement).
- Terrain collision є, але destructible terrain ще немає.
