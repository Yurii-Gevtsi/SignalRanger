# Бот-суперник: балістичний ШІ з рівнями складності

Статус: **запропоновано** (етапи не розпочаті)
Дата: 2026-07-13
Пов'язані документи: `battle-simulation-extraction.md`, `battle-physics-rework.md`,
`battle-four-button-movement.md`, `target-architecture.md`

---

## 1. Мета

Замінити примітивного нерухомого бота на систему з трьома рівнями поведінки:

| Рівень | Приціл | Перешкоди | Рух |
|---|---|---|---|
| **Легкий** (тренування, перші місії) | евристика + великий розкид | ігнорує | стоїть на місці |
| **Середній** | обернена балістика, середній розкид | відбраковує заблоковані траєкторії | ходьба: відійти/підійти, вийти з «мертвої зони» |
| **Важкий** | обернена балістика, малий розкид + вибір найкращої зброї | шукає позицію з чистою траєкторією | ходьба **+ стрибки** (позиції, недосяжні ходьбою) |

Незмінні принципи:

- **Чесність.** Бот грає за тими самими правилами, що гравець: та сама фізика
  снаряда, той самий мотор руху, той самий бюджет часу ходу, ті самі ліміти
  зброї. Жодних читів (телепорт, буст урону, «підглядання» майбутнього стану).
- **Детермінованість під тестами.** Уся випадковість — через один injected
  RNG із seed'ом, щоб EditMode-тести були відтворювані.
- **Шари.** Мозок бота — чистий C# у gameplay-шарі, без MonoBehaviour,
  без сцени, без UI. Сцена лише «натискає кнопки» за планом бота.
- **Data-driven складність.** Усі ручки — в конфігу (ScriptableObject),
  жодних `switch (difficulty)` з магічними числами у коді мозку.

---

## 2. Поточний стан (що маємо і що заважає)

### 2.1. Що вже є і на що спираємось

| Компонент | Файл | Що дає боту |
|---|---|---|
| Фізика снаряда | `Assets/_Game/Runtime/Gameplay/Battle/Simulation/BattleSimulation.cs` (`StepProjectiles`) | еталонна модель польоту: `v += gravity·GravityScale·dt`, підкроки по 0.04 св.од., колізія з `TerrainGrid` попіксельно, AABB-хіт по юнітах, kill-zone, timeout |
| Маска ландшафту | `Assets/_Game/Runtime/Gameplay/Battle/Terrain/TerrainGrid.cs` | `IsSolidWorld`, `Raycast`, `TryFindSurfaceBelow` — усе для перевірки траєкторій і прохідності |
| Мотор юніта | `Assets/_Game/Runtime/Gameplay/Battle/Motion/UnitMotor.cs` | правила ходьби (висота сходинки, обриви), стрибок (`StartJump` — дальність+апекс → швидкість запуску), падіння |
| Параметри стрибка | `CharacterArchetype` (`Front/BackJumpHeight`, `Front/BackJumpDistance`) + `BattleSimulation.TryStartJump` | готова формула дуги стрибка, яку треба лише симулювати наперед |
| Зброя | `Assets/_Game/Runtime/Gameplay/Battle/Weapons/WeaponDefinition.cs` | `MinPower/MaxPower`, `GravityScale`, `Damage`, `ImpactRadius`, `Role` — все, що потрібно солверу |
| Інвентар бота | `WeaponInventoryRuntime` (`TrySelectNextUsable`) | перелік доступної зброї на хід |
| Вибір складності | `GameSessionContext.SelectedAiDifficulty` → `BattleSceneController.MapDifficulty` → `AiDifficulty` | точка інтеграції профілів складності |

### 2.2. Що заважає

1. **`SimpleAiShooter` не знає про світ.** `CreateShot` бере вектор до цілі,
   додає фіксований «підйом» і синусоїдну похибку. Траєкторія не
   перевіряється — постріл у гору перед собою можливий і частий.
2. **Рух у симуляції захардкоджений під гравця.**
   `BattleSimulation.Walk`, `TryStartJump`, `CanAcceptMovementInput`,
   `StepWalkIntent` явно перевіряють `TeamId == Player`;
   `_playerMovement` (бюджет часу ходу) існує лише для гравця.
   Бот фізично не може скористатись рухом через публічний API.
3. **Логіка бота живе в сцені.** `BattleSceneController.UpdateOpponentAi`
   (рядок ~1314) — це таймер 0.55 с + негайний постріл. Немає місця для
   багатокрокового плану «відійти → прицілитись → вистрілити».

---

## 3. Принцип, перевірений референсом

Референсний проєкт (декомпільований код у `Examples/`, пакет `game/ai/`)
реалізує бота як **конвеєр із чотирьох етапів**; ми переймаємо принцип,
але реалізуємо власною структурою під Unity та нашу симуляцію.

### 3.1. Генерація кандидатів — обернена балістика (аналітично)

Замість перебору кутів наосліп розв'язується рівняння польоту: для джерела
`S`, цілі `T` і заданої сили `p` знаходиться час польоту `t` із
біквадратного рівняння, звідки виводиться кут. Розв'язків 0–2:
«настильна» і «навісна» дуги. Прогін по дискретних рівнях сили дає
повний набір кандидатів за O(powerSteps), без симуляції.

Важлива деталь: знайдений кут одразу **заокруглюється до кроку прицілу**,
доступного гравцеві, і похибка заокруглення пишеться в кандидата як штраф.
Бот не може цілитись точніше, ніж дозволяє інтерфейс гравця.

### 3.2. Валідація — форвард-симуляція траєкторії

Кожен кандидат проганяється покроково через ту саму фізику
(інтегрування + попіксельний хіт-тест ландшафту). Результат:

- точка вибуху та її відстань до цілі;
- **`clearPathPc`** — відсоток чистого шляху (скільки траєкторії пройдено
  до зіткнення) — головна метрика «врахування перешкод»;
- відстань до найближчого союзника вздовж траєкторії (щоб не вбити своїх);
- вихід за межі світу → кандидат бракується.

### 3.3. Оцінка — utility-скоринг з шумом

Для кожної зброї свій «тестер» (навісній зброї важлива дуга, прямострільній —
пряма видимість). Рейтинг кандидата множиться на випадковий коефіцієнт
1.0–1.5, щоб бот не був детермінованим у грі; перемагає найкращий кандидат
серед усієї доступної зброї, з fallback на базову зброю.

### 3.4. Похибка складності — розкид **точки цілі**, не кута

`missFactor` виводиться з рівня бота (log₂ від рівня, затиснутий у 1..5)
або задається явно на юніті. Точка прицілювання зсувається випадково;
радіус розкиду росте з дистанцією та missFactor. З імовірністю ~5%
постріл ідеально точний незалежно від складності. Розкид точки виглядає
природніше за «тремтіння кута»: близькі цілі легкий бот все одно влучає.

### 3.5. Рух — зондування прохідності + тактичні правила

- *Планувальник позиції*: покрокове зондування ландшафту (зсув на крок убік,
  пошук опори вгору/вниз у межах висоти «сходинки», ліміт кроків) — повертає
  найдальшу досяжну X у заданому напрямку. Ніякого A*: у 2D artillery
  прохідність — це відрізок уздовж поверхні.
- *Зони небезпеки*: об'єкти на мапі (міни тощо) публікують «радіус небезпеки»,
  планувальник обходить їх.
- *Тактика*: якщо ворог ближче мінімальної дистанції — спершу відійти в бік,
  де більше простору, і лише потім цілитись; після пострілу — відбігти
  у випадковий бік.

### 3.6. Виконання — емуляція контролів гравця

Бот не встановлює значення прицілу миттєво: він «натискає» ті самі кнопки
(поворот прицілу по кадру до epsilon, ходьба по кадру з лімітом кадрів,
стоп при втраті опори), а кроки зшиті async-ланцюжком
*прицілитись → вистрілити → дочекатись стабілізації сцени → відійти*.
Це дає і чесність, і читабельність для гравця.

**Чого в референсі немає:** стрибків у бота (тільки ходьба). Стрибки на
важкій складності — наше розширення, але воно лягає в ту саму схему:
дуга стрибка — та сама балістика, точки приземлення — додаткові кандидатні
позиції.

---

## 4. Цільова архітектура

### 4.1. Склад модуля `Assets/_Game/Runtime/Gameplay/Battle/AI/`

```text
AI/
  BotProfile.cs               (ScriptableObject: усі ручки складності)
  BotProfileLibrary.cs        (runtime-фабрика профілів Easy/Normal/Hard,
                               за зразком PrototypeWeaponLibrary)
  IBotShotPlanner.cs          (інтерфейс: план пострілу з позиції)
  SimpleAiShooter.cs          (існуючий; стає реалізацією Easy)
  BallisticShotPlanner.cs     (Normal/Hard: солвер + проба + скоринг)
  Ballistics/
    BallisticsSolver.cs       (чиста математика: кути з рівняння польоту)
    TrajectoryProbe.cs        (форвард-симуляція по TerrainGrid)
    TrajectoryReport.cs       (readonly struct: результат проби)
    ShotCandidate.cs          (readonly struct: кут+сила+оцінка)
  Movement/
    WalkReachabilityScanner.cs (зондування прохідності вздовж поверхні)
    JumpCandidateGenerator.cs  (симуляція дуг стрибка → точки приземлення)
    PositionCandidate.cs       (readonly struct: позиція + як дістатись + оцінка)
    BotPositionPlanner.cs      (об'єднує ходьбу і стрибки, оцінює позиції)
  TargetScatter.cs            (розкид точки цілі за missFactor)
  BotTurnPlan.cs              (готовий план ходу: кроки Reposition/Aim/Fire)
  BotTurnController.cs        (чистий C# state machine, виконує план
                               через команди BattleSimulation по тіках)
```

Все, крім `BotProfile` (SO) — прості класи без Unity-залежностей понад
`Vector2/Mathf`, тобто повністю покриваються EditMode-тестами
(як `BattleSimulationTests`).

### 4.2. Потоки даних

```text
                     ┌────────────────────────────────────────────┐
                     │            BotTurnController               │
                     │  (state machine ходу бота, тікається        │
                     │   з BattleSceneController.Update)           │
                     └──────┬─────────────────────────┬───────────┘
                 план ходу  │                         │ команди
                            ▼                         ▼
   ┌────────────────────────────────┐   ┌─────────────────────────────┐
   │        Планування (1 раз       │   │   BattleSimulation          │
   │        на початку ходу):       │   │   Walk / TryStartJump /     │
   │  BotPositionPlanner            │   │   TryFire(AimState)         │
   │    ├─ WalkReachabilityScanner  │   └─────────────────────────────┘
   │    └─ JumpCandidateGenerator   │
   │  BallisticShotPlanner          │
   │    ├─ BallisticsSolver         │      читає (read-only):
   │    ├─ TrajectoryProbe ─────────┼───►  TerrainGrid, BattleState,
   │    └─ TargetScatter            │      WeaponDefinition, ProfileSO
   └────────────────────────────────┘
```

Планування читає світ **тільки через read-only контракти** (`TerrainGrid`,
позиції юнітів зі `BattleState`, параметри зброї) і не мутує нічого.
Виконання йде **тільки через існуючі публічні команди** симуляції — ті самі,
якими користується гравець.

### 4.3. Ключові контракти

```csharp
// Чиста математика. 0–2 розв'язки (настильна/навісна дуга).
public static class BallisticsSolver
{
    // gravityY — модуль прискорення для цієї зброї:
    // |projectileGravityPerScale.y| * weapon.GravityScale
    public static int SolveLaunchVelocities(
        Vector2 origin, Vector2 target, float speed, float gravityY,
        out Vector2 lowArc, out Vector2 highArc);
}

// Форвард-симуляція, дзеркало констант StepProjectiles.
public sealed class TrajectoryProbe
{
    public TrajectoryReport Simulate(
        Vector2 origin, Vector2 velocity, float gravityY,
        float timeoutSeconds, in TrajectoryObstacles obstacles);
}

public readonly struct TrajectoryReport
{
    public TrajectoryEndKind EndKind { get; }      // Terrain | Unit | Timeout | OutOfWorld
    public Vector2 EndPoint { get; }
    public float DistanceToTarget { get; }          // від точки вибуху до цілі
    public float ClearPathRatio { get; }            // 0..1, частка шляху без перешкод
    public float MinAllyDistance { get; }           // найменша відстань траєкторії до союзника
    public float FlightTime { get; }
}

public interface IBotShotPlanner
{
    // origin — точка пострілу з КАНДИДАТНОЇ позиції (не обовʼязково поточної)
    BotShotPlan? PlanShot(in BotShotQuery query); // null = з цієї позиції нема пострілу
}

public readonly struct BotShotPlan
{
    public AimState Aim { get; }         // готово для BattleSimulation.TryFire
    public string WeaponId { get; }
    public float Score { get; }
}
```

`BotTurnPlan` — послідовність кроків, які контролер виконує по тіках:

```csharp
public abstract class BotTurnStep { }
public sealed class RepositionWalkStep : BotTurnStep { public float TargetX; }
public sealed class RepositionJumpStep : BotTurnStep { public int DirectionX; }
public sealed class WaitStableStep     : BotTurnStep { }
public sealed class AimPauseStep       : BotTurnStep { public float Seconds; }   // «думає» — читабельність
public sealed class FireStep           : BotTurnStep { public BotShotPlan Shot; }
```

### 4.4. `BotProfile` — усі ручки складності

```csharp
[CreateAssetMenu(menuName = "Game/Battle/Bot Profile")]
public sealed class BotProfile : ScriptableObject
{
    [Header("Прицілювання")]
    public int   PowerSteps;            // дискретизація сили: Easy 4 / Normal 8 / Hard 12
    public float AngleStepDegrees;      // крок «прицілу», до якого заокруглюємо кут (напр. 2°)
    public float MinClearPathRatio;     // кандидат бракується нижче цього (Normal 0.92, Hard 0.97)
    public float ScatterBase;           // базовий радіус розкиду точки цілі, св.од.
    public float ScatterPerDistance;    // приріст розкиду на од. дистанції
    public float PerfectShotChance;     // шанс нульового розкиду (0.05)
    public float ScoreJitter;           // множник рейтингу 1..1+jitter (0.5 → як у референсу)

    [Header("Рух")]
    public bool  MovementEnabled;       // Easy false
    public bool  JumpEnabled;           // тільки Hard
    public float MinEnemySeparation;    // ближче — відійти перед пострілом (св.од., ~2.0)
    public float MaxWalkBudgetSeconds;  // скільки секунд ходьби бот готовий витратити
    public int   MaxPositionCandidates; // Normal 8 / Hard 16
    public bool  RetreatAfterShot;      // відбігти після пострілу (Hard)

    [Header("Вибір зброї")]
    public bool  EvaluateAllWeapons;    // Hard: перебрати весь інвентар; інакше — selected
    [Range(0f, 1f)]
    public float AllySafetyWeight;      // вага штрафу за ризик для союзників

    [Header("Темп (читабельність для гравця)")]
    public float ThinkPauseSeconds;     // пауза «думає» перед дією (0.4–0.9)
    public float AimPauseSeconds;       // пауза між прицілом і пострілом
}
```

Мапа `AiDifficulty → BotProfile` живе в `BotProfileLibrary` (runtime-дефолти,
як `PrototypeWeaponLibrary`), пізніше — asset'и в
`Assets/_Game/Content/Resources/` + `DifficultyProfileDefinition.AiDifficultyTier`
як селектор.

---

## 5. Алгоритми в деталях

### 5.1. Солвер: кут із рівняння польоту

Наша фізика снаряда (див. `StepProjectiles`): постійне прискорення
`g = projectileGravityPerScale * weapon.GravityScale` (вітру немає),
початкова швидкість `v = direction * power`, старт із
`origin + direction * MuzzleOffset(0.45)`.

Для `Δ = T − S`, швидкості `p` і модуля гравітації `g` (вісь Y вгору):

```text
позиція:  Δx = vx·t         Δy = vy·t − g·t²/2
модуль:   vx² + vy² = p²

підстановка vx = Δx/t, vy = (Δy + g·t²/2)/t дає біквадратне рівняння:

  (g²/4)·t⁴ − (p² − g·Δy)·t² + (Δx² + Δy²) = 0

  t² = [ (p² − g·Δy) ± √D ] / (g²/2),
  D  = (p² − g·Δy)² − g²·(Δx² + Δy²)
```

- `D < 0` → ціль недосяжна з цією силою (кандидатів нема);
- два корені `t` → навісна (більший t) і настильна (менший t) дуги;
- `vx = Δx/t`, `vy = (Δy + g·t²/2)/t`, напрямок = `atan2(vy, vx)`.

Далі кут **заокруглюється до `AngleStepDegrees`**, і похибка заокруглення
йде в рейтинг як штраф (бот обмежений тією ж дискретністю, що й палець
гравця). `MuzzleOffset` компенсуємо, зсунувши `S` на
`direction * 0.45` та зробивши одну ітерацію уточнення (кут майже не
змінюється, достатньо одного повтору).

Особливий випадок `GravityScale == 0` (прямий постріл): рівняння вироджується,
розв'язок — промінь `S→T`; валідність вирішує сама проба (`Raycast` по сітці).

### 5.2. Проба траєкторії

Точна калька умов `BattleSimulation.StepProjectiles` (щоб прогноз ≡ факту):

```text
крок часу     : SimulationTickSeconds (з BattleSimulationSettings)
підкрок шляху : ≤ 0.04 св.од. (ProjectileSubStepLength)
на кожному підкроці перевірити:
  y < FallDeathY − 2         → EndKind.OutOfWorld
  Grid.IsSolidWorld(p)       → EndKind.Terrain, EndPoint = p
  AABB юніта (крім власника) → EndKind.Unit
ліміт: weapon.TimeoutSeconds → EndKind.Timeout, EndPoint = поточна p
попутно акумулювати: пройдений шлях до першого контакту (ClearPathRatio),
мінімальну відстань до союзників (MinAllyDistance)
```

Константи не копіюємо числами: `TrajectoryProbe` приймає їх у конструкторі,
а composition root передає ті самі значення, що й у `BattleSimulation`
(одна точка правди — `BattleSimulationSettings` + `ProjectileDefinition`).

### 5.3. Скоринг кандидата

```text
score = wHit   · Hit(DistanceToTarget, weapon.ImpactRadius)   // 1 при прямому влучанні,
                                                              // спад до 0 на ~2·ImpactRadius
      + wPath  · ClearPathRatio
      − wAngle · AngleRoundingPenalty                          // з 5.1
      − wSelf  · SelfRisk(відстань вибуху до себе / ImpactRadius)
      − wAlly  · AllySafetyWeight · AllyRisk(MinAllyDistance)
потім: score ×= 1 + Rng.NextFloat() · ScoreJitter
```

Кандидати з `ClearPathRatio < MinClearPathRatio` **і** вибухом далі
`2·ImpactRadius` від цілі відкидаються до скорингу. Стартові ваги:
`wHit = 1.0, wPath = 0.35, wAngle = 0.1, wSelf = 0.8, wAlly = 0.6`
(підбираються плейтестом, живуть у `BotProfile` якщо знадобиться
диференціація за складністю).

### 5.4. Розкид цілі (missFactor)

```text
if Rng.NextFloat() < PerfectShotChance → цільова точка без змін
else:
  radius  = ScatterBase + ScatterPerDistance · distance(S, T)
  offset  = випадкова точка в колі радіуса radius (рівномірно)
  T' = T + offset
```

Розкид застосовується **до** солвера — бот чесно цілиться в зміщену точку,
тому промах виглядає природно (снаряд лягає поруч, а не летить у небо).
Орієнтовні значення: Easy `ScatterBase 0.9 / PerDistance 0.10`,
Normal `0.45 / 0.05`, Hard `0.15 / 0.015`.

### 5.5. Планувальник позиції

**Ходьба** (`WalkReachabilityScanner`): від поточної позиції в обидва боки
кроками `CellSize.x`:

```text
для кожного кроку вбік:
  шукати опору в межах ±stepHeight (правила як у UnitMotor: підйом на
  сходинку ≤ профільного, спуск — TryFindSurfaceBelow з лімітом обриву)
  обрив глибший за safeDropHeight або kill-zone → стоп у цей бік
  клітинка зайнята іншим юнітом (OccupiedRadius) → стоп
  ліміт шляху: MaxWalkBudgetSeconds · WalkSpeed
результат: упорядкований список досяжних X (семпл кожні ~0.5 св.од.)
```

**Стрибки** (`JumpCandidateGenerator`, тільки Hard): з кожної досяжної
ходьбою точки (включно з поточною) симулюються дві дуги (вліво/вправо)
з параметрами арки з `CharacterArchetype` — тією самою формулою, що
`TryStartJump` + `UnitMotor.StartJump` (дальність обрізана
`WalkSpeed·0.9·flightTime`). Дуга інтегрується по сітці як у
`UnitMotor.Step`; точка приземлення стає кандидатною позицією з міткою
«дістатись стрибком». Один рівень глибини (без ланцюжків стрибків) —
цього достатньо, щоб залазити на уступи, недосяжні ходьбою.

**Оцінка позиції**: для кожної кандидатної позиції викликається
`IBotShotPlanner.PlanShot` (з розкидом уже застосованим до цілі — один
розкид на хід, не на позицію):

```text
positionScore = bestShotScore(з цієї позиції)
              − wTravel · час дістатись (сек. ходьби; стрибок = фікс. 0.8с)
              − wExpose · близькість до ворога понад MinEnemySeparation
позиція без валідного пострілу → score = −∞ (але запам'ятати найкращу
«майже валідну» на fallback)
```

Якщо найкраща позиція — поточна, кроку Reposition у плані немає.
Якщо жодна позиція не дає пострілу — бот іде до «майже валідної»
(максимальний `ClearPathRatio`) і стріляє з найменшим злом: гірший постріл,
що руйнує перешкоду, теж прогрес (ландшафт руйнівний — наступного ходу
шлях буде чистіший).

### 5.6. State machine ходу (`BotTurnController`)

```text
Idle ──(TurnAdvanced: ActiveTeam == Opponent)──► Planning
Planning: 1 виклик планувальника → BotTurnPlan; → Executing
Executing: по одному кроку:
   RepositionWalkStep → Simulation.Walk(dir) щотіка, поки |x−target|<ε
                        або вийшов бюджет/заблоковано; → WaitStable
   RepositionJumpStep → Simulation.TryStartJump(dir); → WaitStable
   WaitStableStep     → чекати !HasActiveMotion && !HasProjectilesInFlight
   AimPauseStep       → таймер (ThinkPause/AimPause з профілю)
   FireStep           → Simulation.TryFire(plan.Aim)
будь-який крок повернув «не можна» двічі поспіль → перейти одразу до FireStep
FireStep виконано (або TryFire неможливий) → Idle
захисний таймер ходу: > N секунд у Executing → примусовий FireStep → Idle
```

Контролер тікається з `BattleSceneController.Update` замість поточного
`UpdateOpponentAi`. Ре-планування посеред ходу немає: план будується раз,
виконується до кінця (простота > оптимальність; ландшафт під час власного
ходу бота не змінюється).

### 5.7. Бюджет продуктивності

Найважчий випадок (Hard): `16 позицій × 8 сил × 2 дуги = 256` проб.
Проба ≈ 100–300 перевірок клітинок (політ 1.5–3 с, підкрок 0.04) →
≈ 25–75 тис. викликів `IsSolidCell` на планування. Це одиниці мілісекунд
на мобільному CPU, але щоб не фризити кадр:

- Планування розбивається на **шматки по позиціях**: `Planning` обробляє
  ≤ 4 позиції за тік (бюджетний ітератор, без Task/Thread — симуляція
  синхронна й детермінована).
- Пауза `ThinkPauseSeconds` перекриває планування: гравець бачить
  «бот думає», а не фриз.

---

## 6. Зміни в існуючому коді

### 6.1. `BattleSimulation` — рух для обох команд (передумова всього)

Мінімальний рефакторинг, без зміни поведінки для гравця:

1. **Реалізовано простіше, ніж планувалось:** per-team `TurnMovementState`
   не знадобився. Оскільки таймер ходу лишається виключно player-only
   (п.3), `_playerMovement` і назва `PlayerTurnDurationSeconds`
   збережені без змін — це знижує churn і не чіпає сигнатуру
   `BattleSimulationSettings`, якою користуються тести й scene controller.
2. `Walk`, `TryStartJump`, `CanUseMovement`, `CanAcceptMovementInput`,
   `StepWalkIntent`: прибрано перевірки `TeamId == Player`. Активний юніт
   за побудовою належить активній команді, а дозвіл на рух гейтиться
   per-team у `CanAcceptMovementInput` (Player → обмежений таймером,
   Opponent → без обмеження). Хто «водить» юніта — вирішує викликач
   (input router для гравця, `BotTurnController` для бота).
3. Таймер ходу (`StepPlayerTurnTimer`) залишено **тільки для гравця**
   (бот обмежений власним захисним таймером у контролері) — інакше
   повільний бот «згорить» по таймеру і ходи почнуть пропускатись.
4. Нових публічних членів: немає (усе вже є — `Walk`, `TryStartJump`,
   `TryFire`, `HasActiveMotion`, `State`, `Terrain`).

Ризик регресій покривають існуючі `BattleSimulationTests` + нові кейси
«бот ходить/стрибає через ті самі API».

### 6.2. `BattleSceneController`

- Викинути `UpdateOpponentAi` цілком; замість нього створити в
  `Awake/Start` `BotTurnController` (профіль з `BotProfileLibrary`
  за `_aiDifficulty`) і тікати його.
- `SimpleAiShooter` більше не викликається контролером напряму — він
  стає реалізацією `IBotShotPlanner` для Easy (обгортка: та сама
  евристика, без проби траєкторії, рух вимкнено профілем).
- Статуси HUD (`OpponentAiming/OpponentFiring`) — ідуть від станів
  контролера (`Planning/Executing`), додається статус «переміщується».

### 6.3. Що НЕ чіпаємо

- Фізику снаряда, мотор, ландшафт, зброю, інвентар — нуль змін.
- UI прицілювання гравця.
- `AiDifficulty` enum і його мапінг із місій (лишається селектором профілю).

---

## 7. План впровадження

Кожен етап — окремий PR, зелені EditMode-тести, позначати чекбокси тут.

### Етап 0 — рух симуляції для обох команд (передумова)

- [x] `CanAcceptMovementInput` зроблено team-aware (Player обмежений
      таймером, Opponent — ні). Per-team `TurnMovementState` не знадобився:
      таймер лишається player-only, тому `PlayerTurnDurationSeconds` не
      перейменовано (менше churn, сигнатура settings стабільна)
- [x] Прибрано player-only перевірки з `Walk`/`TryStartJump`/
      `CanUseMovement`/`CanAcceptMovementInput`/`StepWalkIntent`
- [x] Таймер ходу лишено гравцеві; бот таймером симуляції не обмежується
- [x] Тести: `WalkIntent_MovesOpponentUnit_DuringOpponentTurn`,
      `StandardJump_OpponentJumps_DuringOpponentTurn`; існуючі тести
      гравця — регресія зелена

**Вихід:** бот *може* рухатись; поведінка гри не змінилась.

### Етап 1 — балістичне ядро (чиста математика)

- [ ] `BallisticsSolver.SolveLaunchVelocities` (+ вироджений випадок g=0)
- [ ] `TrajectoryProbe` + `TrajectoryReport` (константи інжектяться з
      `BattleSimulationSettings`, не дублюються числами)
- [ ] `TrajectoryObstacles`: ландшафт (TerrainGrid) + список AABB юнітів
- [ ] Тести солвера: симетричні кейси (ціль на одній висоті → кут 45°±,
      дві дуги), недосяжна ціль → 0 розв'язків, g=0 → промінь
- [ ] Тести проби: **пророцтво ≡ факт** — прогнана через `TrajectoryProbe`
      траєкторія і реальний постріл `BattleSimulation` з тим самим
      вектором закінчуються в одній точці (± пів клітинки) на кількох
      golden-сценах (рівна земля, стіна між, ціль вище/нижче)

**Вихід:** бібліотека, що вміє відповісти «як і куди полетить» без пострілу.

### Етап 2 — `BallisticShotPlanner` (середня складність, без руху)

- [ ] `ShotCandidate`, скоринг (5.3), `TargetScatter` (5.4),
      seeded RNG інжектиться
- [ ] `IBotShotPlanner`; `SimpleAiShooter` адаптується під інтерфейс (Easy)
- [ ] `BotProfile` + `BotProfileLibrary` (runtime-дефолти Easy/Normal/Hard)
- [ ] `BotTurnController` мінімальний: Planning → AimPause → Fire
      (без Reposition), інтеграція в `BattleSceneController` замість
      `UpdateOpponentAi`
- [ ] Тести: на сцені зі стіною Normal-бот не стріляє в стіну
      (обирає навісну дугу або валідно бракує всі кандидати);
      з фіксованим seed план детермінований; hit-rate на відкритій сцені:
      Normal ≥ 60% прямих влучань з 20 пострілів, Easy ≤ 30%
- [ ] Ручний плейтест: середня місія відчувається «небезпечно, але чесно»

**Вихід:** середня складність готова (влучає, враховує перешкоди, стоїть).

### Етап 3 — рух ходьбою (повна середня складність)

- [ ] `WalkReachabilityScanner` (правила кальковані з `UnitMotor`:
      сходинка, обрив, kill-zone, зайнятість)
- [ ] `BotPositionPlanner` (тільки ходьба): оцінка позицій (5.5),
      «майже валідний» fallback
- [ ] Кроки `RepositionWalkStep`/`WaitStableStep` у контролері,
      бюджетне планування по тіках (5.7), захисний таймер ходу
- [ ] `MinEnemySeparation`: відійти перед пострілом, якщо впритул
- [ ] HUD-статус «суперник переміщується»
- [ ] Тести: бот за стіною знаходить позицію з чистою траєкторією і
      стріляє звідти; бот на краю обриву не сходить у kill-zone;
      бот впритул до гравця спершу відходить; бюджет ходьби не
      перевищується

**Вихід:** Normal ходить і стріляє з розумної позиції.

### Етап 4 — стрибки (важка складність)

- [ ] `JumpCandidateGenerator`: симуляція дуг стрибка по сітці
      (формула з `TryStartJump`/`StartJump`), точки приземлення →
      кандидатні позиції
- [ ] `RepositionJumpStep` у контролері (через `Simulation.TryStartJump`)
- [ ] Hard-профіль: `JumpEnabled`, `EvaluateAllWeapons`,
      `RetreatAfterShot`, вужчий розкид
- [ ] Вибір зброї: перебір інвентарю, очікуваний урон у скорингу
      (`weapon.Damage · Hit(...)`), поважати ліміти боєприпасів
- [ ] Тести: уступ, досяжний лише стрибком → Hard залазить і влучає,
      Normal — ні; стрибок не обирається, якщо приземлення в kill-zone;
      відхід після пострілу не ламає завершення ходу
- [ ] Ручний плейтест Hard: бот займає висоти, змінює позиції, тисне

**Вихід:** важка складність повна.

### Етап 5 — контент, баланс, телеметрія

- [ ] `BotProfile` як asset'и в `Assets/_Game/Content/Resources/`
      (runtime-дефолти лишаються fallback'ом), прив'язка через
      `DifficultyProfileDefinition.AiDifficultyTier`
- [ ] Балансний прохід: скатер/ваги скорингу по місіях кампанії
      (легкі місії — Easy, середина — Normal, фінали — Hard)
- [ ] Метрики в дебаг-оверлей: hit-rate бота, час планування,
      число кандидатів/бракованих (для тюнінгу, не в релізний білд)
- [ ] Оновити `docs/design/game-design-brief.md` розділом про складності

**Вихід:** складність — контентна ручка, а не код.

### Етап 6 — заділ на майбутнє (окремими задачами, поза цим планом)

- Зони небезпеки: інтерфейс `IDangerSource {Position, DangerRadius}`
  для майбутніх мін/пасток — `WalkReachabilityScanner` вже готовий
  його враховувати (референсний патерн 3.5).
- Командні бої 2v2+: вибір цілі (зараз — перший живий юнітгравця;
  узагальнити до «найвигідніша ціль» = найближча/найслабша).
- PvP-реванш офлайн: бот грає замість гравця, що вийшов.
- Кешування прохідності між ходами (інвалідація по dirtyCells від кратерів).

---

## 8. Тестова стратегія

- **Рівень 1 — математика (без сцени):** солвер і проба — чисті функції,
  табличні кейси. Найцінніший інваріант: *прогноз проби ≡ результат
  симуляції* (етап 1) — він гарантує, що бот ніколи не «здивується»
  власному пострілу.
- **Рівень 2 — планувальник (golden-сцени):** маленькі `TerrainGrid`
  (20×12), збудовані в тесті: рівнина / стіна / яма / уступ. Асерти на
  *властивості* плану (не на конкретні числа): «траєкторія обраного
  кандидата чиста», «позиція досяжна», «бот не в kill-zone».
- **Рівень 3 — інтеграція ходу:** `BattleSimulation` + `BotTurnController`
  тікаються вручну N тіків; асерти: хід завершується завжди (захисний
  таймер), дія витрачається один раз, стан симуляції валідний.
- **Статистика з seed'ами:** hit-rate по 20 пострілів на фіксованих seed —
  межі Easy/Normal/Hard не перетинаються.
- Запуск: headless EditMode (усталений батч-режим проєкту).

---

## 9. Ризики та рішення

| Ризик | Рішення |
|---|---|
| Дрейф констант: проба і симуляція розійшлись після зміни фізики | константи інжектяться з одного джерела (`BattleSimulationSettings`); тест «прогноз ≡ факт» в CI зловить дрейф |
| Фриз кадру на плануванні (Hard, велика мапа) | бюджетний ітератор по позиціях + пауза «думає» маскує; метрика часу планування в дебаг-оверлеї |
| Бот виглядає «роботом» (миттєві ідеальні рішення) | паузи темпу з профілю, розкид цілі, jitter скорингу, рух видимий покроково через звичайний мотор |
| Бот застряг (заблокована ходьба, нестабільна опора) | кожен крок плану має ліміт тіків і правило «двічі не вийшло → стріляй як є»; захисний таймер ходу |
| Надто сильний Hard фруструє | `PerfectShotChance`/скатер — контентні ручки; межа зверху: розкид ніколи не 0, дискретність кута як у гравця |
| Регресія руху гравця після рефакторингу етапу 0 | етап 0 ізольований і мінімальний, існуючі тести — обов'язкова умова мержа |

---

## 10. Відкриті питання (вирішити до етапу 3)

1. Чи показувати гравцеві траєкторію прицілювання бота (як телеграф
   загрози на легкому рівні)? Продуктове рішення, на архітектуру не впливає.
2. Бюджет ходу бота: фіксований захисний таймер (пропозиція: 12 с) чи
   той самий `TurnDurationSeconds`, що в гравця? Пропозиція — окремий,
   бо бот не повинен «згоряти» по таймеру через паузи читабельності.
3. Чи потрібен Easy-боту рух у тренувальній арені (зараз — ні, стоїть)?
