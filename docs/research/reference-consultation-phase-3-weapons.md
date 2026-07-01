# Reference Consultation: Phase 3 Weapons

Feature:
: Перший data-driven шар зброї для `BattleScene`.

Example signals:
: У `Examples/Warlings/com.warlings5.apk` видно окремі battle HUD та weapon-related assets у `assets/game_menu`, зокрема елементи для weapon UI/count/modifiers. У `Examples/Wormix` є окремі gameplay data/assets категорії, а відгуки гравців окремо підкреслюють важливість різної зброї з читабельними tactical roles.

Useful principles:
: Зброя має бути окремою data/content сутністю: stable ID, роль, damage/radius, projectile behavior, power range, ammo/tutorial metadata. Battle scene не повинна знати конкретні числа кожної зброї напряму.

Risks to avoid:
: Не копіювати чужі weapon names, icons, stats, formulas, upgrade trees, balance або UI layouts. Не стартувати з великого арсеналу, доки core loop і counterplay не стабільні.

Our original implementation:
: Додано `WeaponRole`, `WeaponDefinition`, `PrototypeWeaponLibrary` і три оригінальні runtime weapons: `weapon.steady-orb`, `weapon.pulse-shell`, `weapon.close-pop`. `BattleSceneController` тепер перемикає зброю через `Tab`/`Next Weapon` і бере projectile/damage/radius/power range з активної `WeaponDefinition`.

## Phase 3.2 update: Weapon Runtime

Feature:
: Винесення виконання зброї зі сцени в окремий runtime-шар (selection + ammo + effect dispatch).

Useful principles:
: Battle scene не повинна знати, як поводиться конкретна зброя. Вибір і боєзапас — це стан комбатанта, а «що відбувається при пострілі» — окрема стратегія за роллю. Це готує ґрунт під non-projectile ролі (cover/defensive/movement) без розростання сцени.

Risks to avoid:
: Не тягнути спавн GameObject-ів у чисту логіку — інакше core loop стає нетестованим. Не хардкодити ammo/визуал у сцені.

Our original implementation:
: Додано `WeaponInventoryRuntime` (чистий стан: вибір зброї + боєзапас, покритий edit-mode тестами), `IWeaponEffect` + `WeaponFireContext` + `ProjectileSpawnRequest` + `IProjectileSpawner` (абстракція спавна), базовий `ProjectileWeaponEffect` і `WeaponExecutor` (валідація боєзапасу + dispatch ефекту за роллю з fallback). `BattleSceneController` тепер реалізує `IProjectileSpawner`, а `Fire()` делегує постріл `WeaponExecutor`. Боєзапас: `-1` = нескінченний; обмежена зброя декрементиться й блокує постріл на нулі.
