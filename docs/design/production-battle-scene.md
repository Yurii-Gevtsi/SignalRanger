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
- HUD `ВОГОНЬ` button або `Space`: постріл (явне підтвердження зафіксованого прицілу).
- HUD `Скинути` button або `Esc`: скинути приціл.
- HUD `Зброя` button або `Tab`: наступна зброя.
- HUD `Рестарт` button або `R`: перезапуск сцени.
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

`BattleSceneController` реалізує `IProjectileSpawner`; `Fire()` делегує постріл `WeaponExecutor`, а створення снаряда живе в `Spawn`. Боєзапас `-1` — нескінченний; обмежена зброя декрементиться і блокує постріл на нулі (HUD показує `Боєприпаси: N` / `Боєприпаси: безліміт`).

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
- `BattleHudView`: runtime-побудований `Canvas` (`ScaleWithScreenSize`, ref 1080×1920) з top status/turn bar, per-team health bars, power meter і великими touch-кнопками `ВОГОНЬ` / `Скинути` / `Зброя` / `Рестарт`.

Елементи заякорені по кутах екрана → не перекриваються на різних mobile-співвідношеннях. `ВОГОНЬ` явно неактивна, коли постріл неможливий (немає зафіксованого прицілу, немає боєзапасу, снаряд у польоті, бій завершено). `BattleSceneController` спілкується лише через `IBattleHud`.

Постріл має двокроковий flow: гравець затискає/тягне в зоні бою, відпускає для фіксації прицілу, потім натискає `ВОГОНЬ`. Рух курсора без затиснутої кнопки не змінює приціл, тому шлях до HUD-кнопки не псує напрямок пострілу.

Відомий gap: HUD поки живе у збірці `Game.Gameplay` (додано reference `UnityEngine.UI`), а не в `Game.UI` — бо `Game.UI` бачить лише `Game.Core`. Перенесення потребує Core-рівневого view-model і виділення composition root у `Game.App`; заплановано пізніше.

## Phase 4.2: Result Overlay

Після завершення бою `BattleSceneController` більше не лишає результат тільки в top status. Додано власний runtime result flow:

- `BattleResultScreenState`: view-model для результату бою.
- `BattleRewardPreview`: тимчасовий preview нагороди без застосування save/economy.
- `BattleResultScreenFactory`: чиста фабрика, яка переводить `BattleResult` у title/subtitle/reward/buttons.
- `IBattleHud.ContinueRequested`: HUD-контракт для переходу далі після перемоги/нічиєї.
- `BattleHudView`: повноекранний modal overlay з результатом, reward preview, `Повторити` і `Далі`.

`Повторити` перезапускає поточну арену. `Далі` поки переходить до наступної runtime arena, доки немає окремих Menu/Result scenes. Після завершення бою aim/input заблоковані, а pointer над HUD не рухає приціл.

## Що перевірити вручну

- У Console немає червоних compile/runtime errors.
- У сцені видно фон, terrain blocks, два юніти й uGUI HUD.
- HUD-панелі (status/turn, health, power, кнопки) не перекриваються — перевірити в Game view на 16:9 landscape і tall mobile (напр. 9:19.5).
- Top bar показує status і `Хід N — команда`.
- Per-team health bars згори зменшуються після влучань.
- `N` (keyboard debug) перемикає 3 арени без накопичення старих колайдерів/юнітів.
- HUD `Зброя` button / `Tab` перемикає 5 зброй; показує назву + боєзапас (`Боєприпаси: безліміт`).
- У різної зброї відрізняються power range, damage/radius і вигляд снаряда.
- Power meter заповнюється залежно від сили прицілювання.
- `ВОГОНЬ` неактивна до наведення, під час польоту снаряда і після завершення бою.
- `Скинути` / `Esc` скидає приціл.
- Приціл і trajectory preview оновлюються тільки під час drag/натискання в зоні бою.
- Після відпускання мишки/пальця приціл лишається зафіксованим, а рух до кнопки `ВОГОНЬ` не змінює напрямок.
- Наведення/натискання поверх HUD-кнопок не рухає приціл.
- Постріл летить дугою; після impact HP змінюється; хід переходить до іншої команди.
- Win/loss/draw відкриває result overlay.
- `Повторити` перезапускає поточний бій; `Далі` після перемоги/нічиєї завантажує наступну runtime arena.

## Поточні обмеження

- Placeholder visuals.
- Немає AI.
- HUD-текст поки через legacy uGUI `Text`; видимі рядки українською, але без повного TMP/localization pipeline.
- HUD у збірці `Game.Gameplay`, ще не в `Game.UI`.
- Result overlay поки runtime HUD, а не окрема Result scene.
- Є weapon inventory/executor + 5 зброй, але поки один спільний projectile-ефект на всі ролі (без cover/defensive/movement).
- Terrain collision є, але destructible terrain ще немає.
