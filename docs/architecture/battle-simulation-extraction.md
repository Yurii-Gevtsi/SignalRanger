# Винесення бойового циклу в BattleSimulation + ґрунтовка детермінізму

Статус: специфікація, готова до виконання. Позначки: `[x]` виконано,
`[ ]` заплановано. Виконавець: після завершення кожного етапу —
позначити чекбокси, закомітити, прогнати тести.

Продовження [battle-physics-rework.md](battle-physics-rework.md):
фізика вже винесена в чисті `TerrainGrid`/`BattleTerrain`
(`Runtime/Gameplay/Battle/Terrain/`) і `UnitMotor`
(`Runtime/Gameplay/Battle/Motion/`). Тепер відокремлюємо **правила бою**
від сцени/вводу/візуалу.

## Правила виконання (обов'язково)

1. Прочитати `CLAUDE.md`. Головне: жодних згадок назв ігор-референсів у
   коді/доках/комітах; `Examples/` не змінювати.
2. Нові `.cs` файли потребують `.meta` (2 рядки: `fileFormatVersion: 2`
   + `guid: <32 hex>`), нові теки — folderAsset-варіант .meta (зразок:
   `Runtime/Gameplay/Battle/Terrain.meta`). Не комітити .meta порожніх тек.
3. Тести: `"C:\Program Files\Unity\Hub\Editor\6000.5.1f1\Editor\Unity.exe"
   -batchmode -projectPath C:\Projects\Game -runTests -testPlatform EditMode
   -testFilter "Game.Tests.EditMode.Gameplay" -testResults <абс.шлях xml>
   -logFile <абс.шлях log>` (5–7 хв). Ліцензійні помилки в лозі — не
   фатальні; результат дивитись в xml (`passed=`/`failed=`).
4. Після batchmode-рану Unity може прибрати define
   `SENTIS_ANALYTICS_ENABLED` зі `ProjectSettings/ProjectSettings.asset`
   — цю зміну **не комітити** (`git checkout -- ProjectSettings/ProjectSettings.asset`).
5. Відомий falling тест `TutorialServiceTests.FindNext_WaitsForRequiredSteps`
   — існував до цих робіт, не чіпати, не «лагодити мимохідь».
6. Коміт після кожного етапу; повідомлення — англійською, як у
   `git log` (стисле дієслівне перше речення + абзац деталей).
7. Геймфіл не змінювати: величини імпульсів, шкоди, таймінгів ходу
   переносити 1:1. Це рефакторинг структури, не балансу.

## Поточний стан (що є зараз)

`Runtime/Gameplay/Battle/Assembly/BattleSceneController.cs` (~2200
рядків) — MonoBehaviour, який суміщає:

- **правила бою**: `_battleState`, `_turnService`, `_playerMovement`
  (таймер ходу), `_unitMotors`, `_battleTerrain`,
  `_projectilesInFlight`, `Fire()`-валідацію, `ResolveImpact`,
  `ResolveDamageImpact`, `ApplyTerrainDestruction`,
  `ApplyExplosionKnockback`, `UseCloseRangeBurst`, `UseInstantUtility`,
  `DeployCover`, `RestoreFriendlyHealth`, `RepositionOwner`,
  `UpdateUnitPhysics` (фіксований тік 1/120), `NudgeApartFromOthers`,
  `ResolveBattleAfterMotion`, `TryResolveGroundPosition`,
  `CompleteInstantAction`, `EndPlayerTurnFromTimer`;
- **ввід**: клавіатура/тач у `Update()`, кнопки HUD;
- **візуал**: камера, спрайти, лінії прицілу, trajectory preview,
  спалахи, статусні тексти (`GameText`), `BattleUnitPresenter`-и;
- **AI**: `UpdateOpponentAi` + `SimpleAiShooter` (48 рядків, без
  Random — вже детермінований);
- **містки**: `BattleFeedbackBridge`, `BattleTutorialBridge`, `IBattleHud`.

Снаряд — MonoBehaviour `Projectiles/ProjectileController.cs`, сам
інтегрується у своєму `Update()` по `IProjectileWorld` (реалізує
контролер).

## Цільова архітектура

```text
Runtime/Gameplay/Battle/Simulation/   (нова тека, чиста C#)
  BattleSimulation.cs        — правила бою + фіксований тік
  BattleSimulationEvents.cs  — типи подій (readonly struct-и)
  SimProjectile.cs           — снаряд як дані симуляції (етап B)

BattleSceneController (залишається MonoBehaviour):
  - будує сцену/візуали/камеру, створює BattleTerrain і передає в сим;
  - транслює ввід у команди сим (Fire/Walk/Jump/SelectWeapon/...);
  - викликає sim.Tick(Time.deltaTime) в Update();
  - підписаний на події сим → статуси, спалахи, HUD, feedback/tutorial;
  - AI лишається тут як «генератор команд» (кличе ті самі методи сим);
  - trajectory preview лишається тут (читає стан сим).
```

Правило шарів: у теці `Simulation/` заборонені `UnityEngine.Time`,
`UnityEngine.Random`, `Physics2D` (крім константи гравітації, яку
передаємо в конструктор), `GameObject/MonoBehaviour`, `Resources`,
`Input`. Математичні типи (`Vector2`, `Mathf`) — дозволені.

## Етап A — BattleSimulation (правила бою)

- [x] Створити `Simulation/BattleSimulation.cs` (чиста C#). Конструктор:
      `(BattleState state, TurnService turnService, BattleTerrain terrain,
      IReadOnlyDictionary<string, UnitMotor> motors,
      WeaponInventoryRuntime playerInventory,
      WeaponInventoryRuntime opponentInventory,
      WeaponExecutor weaponExecutor, Vector2 projectileGravityPerScale,
      BattleSimulationSettings settings)`.
      `BattleSimulationSettings` — новий клас з константами, що
      переїжджають з контролера: `FallDeathY`, `SimulationTickSeconds`,
      `PlayerTurnDurationSeconds`, `OccupiedRadius`,
      `UnitCollisionHalfWidth`, `KnockbackLandingSearchPadding`,
      `GroundRayStartHeight`, `GroundRayDistance`.
- [x] Перенести в сим (пойменований move-map зі списку «Поточний стан»,
      логіка 1:1): валідацію пострілу з `Fire()` (без вводу/прев'ю),
      `UseCloseRangeBurst`, `UseInstantUtility`, `ResolveImpact`,
      `ResolveDamageImpact`, `ApplyTerrainDestruction`,
      `ApplyExplosionKnockback`, `RestoreFriendlyHealth`,
      `RepositionOwner`, `DeployCover` (логіка: штамп у маску + подія;
      візуальний бокс малює контролер за подією),
      `UpdateUnitPhysics`→`Tick`, `NudgeApartFromOthers`,
      `ResolveBattleAfterMotion`, `TryResolveGroundPosition`,
      `ResolveUnitLandingPosition`, `CompleteInstantAction`,
      `EndPlayerTurnFromTimer`, `_playerMovement` і
      `UpdatePlayerTurnTimer`-логіку.
- [x] Публічний API сим (команди повертають результат-enum, а не пишуть
      статус-тексти): `TryFire(AimState aim)`, `Walk(int dir, float dt)`,
      `TryStartJump(bool forward)`, `Tick(float dt)` (внутрішній
      акумулятор 1/120), `NotifyProjectileImpact(ProjectileImpact)`
      (тимчасово, до етапу B), властивості: `State`, `Result`,
      `HasActiveMotion`, `ProjectilesInFlight`, `Terrain`,
      `ActiveInventory`, `MovementTimeRemaining`.
- [x] Події сим у `BattleSimulationEvents.cs` (C# `event`):
      `ShotFired`, `ImpactResolved(point, weapon, terrainChanged)`,
      `UnitDefeated(unitId, reason)`, `TurnAdvanced(team)`,
      `BattleFinished(result)`, `CoverDeployed(position, size, weapon)`,
      `HealthRestored(point, amount, team)`, `OwnerRepositioned(unitId)`,
      `UnitsMoved` (раз на кадр після тіку — сигнал контролеру
      синхронізувати view). Усі статусні рядки (`GameText.*`) формує
      контролер у обробниках подій — у сим текстів немає.
- [x] Контролер: залишає собі ввід, візуали, HUD, туторіал, AI;
      усі виклики правил замінити на команди сим; підписки на події;
      `IProjectileWorld` поки що реалізує контролер (юніти бере з
      `sim.State`).
- [x] `WeaponFireContext`: прибрати параметр `Collider2D` (мертвий після
      фізичного рефакторингу). Оновити `ProjectileWeaponEffect`,
      `BurstProjectileEffect`, `ProjectileSpawnRequest`
      (поле `IgnoredOwnerCollider` видалити) і місця створення.
- [x] EditMode-тести `Tests/EditMode/Gameplay/BattleSimulationTests.cs`
      (сцена не потрібна — процедурний `BattleTerrain.CreateProcedural`,
      два юніти): постріл → імпакт → шкода + відкидання → зміна ходу;
      падіння за межі → поразка → `BattleFinished`; таймер ходу
      вичерпано → хід переходить; cover штампується в маску.
- [x] Критерій: контролер не містить жодного виклику
      `DamageResolver`/`TurnService`/`BattleResultEvaluator` напряму;
      всі 82+ Gameplay-тести зелені; коміт.

## Етап B — снаряди всередину симуляції

- [ ] `SimProjectile` (дані: id, definition, ownerUnitId, position,
      velocity, remainingLifetime). Сим веде `List<SimProjectile>`,
      інтегрує їх у своєму `Tick` тими самими субкроками
      (`SubStepLength = 0.04`), hit-test по масці + юнітам (логіка з
      поточного `ProjectileController.Update` і
      `IProjectileWorld.TryHitUnit` переїжджає в сим). Сим реалізує
      `IProjectileSpawner` (для `WeaponExecutor`).
- [ ] Події: `ProjectileSpawned(SimProjectile)`,
      `ProjectileExploded(projectileId, impact)`. Контролер тримає
      `Dictionary<int, GameObject>` візуалів: створює за
      `ProjectileSpawned` (нинішні `Spawn`-візуали:
      `ApplyProjectileVisualTransform`, `ResolveProjectileVisualColor`),
      після кожного кадру копіює позиції з `sim.Projectiles`, видаляє й
      малює спалах за `ProjectileExploded`.
- [ ] Видалити `ProjectileController.cs` (MonoBehaviour) і
      `IProjectileWorld.cs` разом з .meta; `NotifyProjectileImpact`
      прибрати з API сим.
- [ ] Наслідок для черги: снаряд тепер тікає тим самим фіксованим кроком,
      що і юніти — прибрати окремий стан «снаряд у польоті поза тіком»
      (`_projectilesInFlight` живе в сим і оновлюється в `Tick`).
- [ ] Тести: постріл через сим → снаряд летить → кратер + відкидання без
      жодного GameObject; таймаут снаряда; постріл повз юніта-власника
      не влучає у власника.
- [ ] Критерій: у бойовій симуляції немає MonoBehaviour; тести зелені; коміт.

## Етап C — детермінізм-ґрунтовка (не fixed-point!)

Мета скромна і чесна: **повторюваність на одній платформі** (реплеї,
тести, майбутній lockstep-фундамент). Крос-платформний біт-в-біт
детермінізм (fixed-point математика) — свідомо поза скоупом.

- [ ] Аудит теки `Simulation/`: жодного `UnityEngine.Time`,
      `UnityEngine.Random`, `Physics2D.gravity` (гравітація — параметр
      конструктора; контролер передає `Physics2D.gravity` при створенні).
      Якщо десь знадобиться випадковість — тільки ін'єктований
      `System.Random(seed)` через конструктор.
- [ ] `Tick(float dt)` вже квантує час фіксованим кроком — переконатися,
      що ВСЯ зміна стану (мотори, снаряди, таймер ходу) відбувається
      тільки всередині фіксованих кроків, а не з сирим `dt`
      (зараз `Walk` викликається з `Time.deltaTime` повз акумулятор —
      перевести ходьбу на чергу «walkIntent», яку тік споживає).
- [ ] Детермінізм-тест: скрипт команд (N ходів: walk/jump/fire із
      заданими векторами) виконується двічі на двох свіжих сим;
      після кожного тіку акумулювати хеш (`float.GetHashCode` позицій
      юнітів і снарядів); хеші прогонів мають збігтися. Другий тест —
      той самий скрипт, але `Tick` викликається порціями різного розміру
      (імітація нерівного фреймрейту) → той самий фінальний стан.
- [ ] Задокументувати в цьому файлі обмеження: детермінізм гарантовано
      в межах однієї платформи/архітектури; для крос-платформного PvP
      знадобиться fixed-point заміна математики в `Terrain/Motion/Simulation`.
- [ ] Критерій: тести зелені (включно з двома новими); коміт.

## Що свідомо НЕ робити

- Не чіпати HUD, туторіал, меню, економіку, save-систему.
- Не вводити DI-контейнер/івент-шину — прямі C# events достатні.
- Не переносити AI всередину сим (він — джерело команд, як гравець).
- Не міняти числа геймфілу і формати збережень.
- Не робити fixed-point математику.

## Порядок і ризики

Етапи виконувати строго A → B → C, з komітом і зеленими тестами після
кожного. Найризиковіший — етап A (великий move-map): виконувати
методами «перенести групу методів → компіл → тести», не намагатися
за один прохід. Якщо після перенесення тести падають — звіряти
поведінку з git diff проти оригінальних методів контролера (логіка
має бути 1:1).
