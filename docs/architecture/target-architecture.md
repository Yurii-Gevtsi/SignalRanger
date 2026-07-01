# Цільова архітектура

## Продуктовий напрям

Ми будуємо оригінальну мобільну 2D тактичну artillery-гру в Unity. Перша версія має бути легшою для входу, ніж старі перевантажені жанрові приклади:

- короткі матчі;
- малі команди;
- небагато зброї на старті;
- чисте touch-керування;
- перша сесія з м’яким навчанням;
- читабельний UI;
- прогресія, яка починається просто і з часом росте.

Довгострокова структура має підтримувати більше зброї, архетипи персонажів, апгрейди, події, косметику, монетизацію, live tuning і optional online services, але ці шари не повинні проникати в core gameplay напряму.

Висновок із аналізу відгуків: спочатку потрібне чисте бойове ядро, потім поступова прогресія. Fairness і trust є core feature, а не “поліруванням потім”.

Перед реалізацією кожної великої системи треба перевіряти відповідники в `Examples/`, виписувати принципи та ризики, а потім робити власну реалізацію.

## Рекомендована структура Unity-проєкту

Проєктний root:

```text
Assets/_Game/
  Runtime/
    Core/
    App/
    Gameplay/
    Progression/
    Economy/
    Inventory/
    Rewards/
    Tutorial/
    UI/
    Audio/
    Data/
    Infrastructure/
  Content/
    Art/
    Audio/
    Prefabs/
    ScriptableObjects/
    Localization/
  Scenes/
    Boot/
    Menu/
    Battle/
  Tests/
    EditMode/
    PlayMode/
  Editor/
```

Це наша власна структура. Вона може бути натхненна спостереженнями з прикладів, але не має повторювати їхню точну структуру.

## Шари

`Core`
: Спільні інтерфейси, базові result/event типи, time abstractions, легкий service registry, domain models без залежності від сцен, UI або SDK.

`App`
: Bootstrap, composition root, application flow, scene loading, platform lifecycle.

`Gameplay`
: Battle rules, turn flow, unit control, weapon execution, projectiles, terrain interaction, damage, win/loss, battle camera.

`Progression`
: Кампанія, місії, unlock-и, chapters, difficulty ramp, feature gates.

`Economy`
: Soft currency, rewards, sinks, pricing models, transaction validation для локальної гри.

`Inventory`
: Owned weapons, loadout, cosmetics, consumables, upgrades.

`Rewards`
: Match rewards, first-time rewards, daily rewards later, event rewards.

`Tutorial`
: Step-based onboarding, contextual prompts, tutorial constraints, feature explanations.

`UI`
: Screens, HUD, navigation, presenters/view models, reusable widgets.

`Audio`
: Music, SFX, mixer routing, user settings, battle cues.

`Data`
: Save data, authored configs, local defaults, migrations, repositories.

`Infrastructure`
: Concrete adapters for ads, IAP, analytics, remote config, notifications, backend, platform APIs.

## Основні сервіси

Початковий набір:

- `SceneLoadingService`
- `GameSessionService`
- `SaveService`
- `ConfigService`
- `ProgressionService`
- `EconomyService`
- `InventoryService`
- `RewardService`
- `TutorialService`
- `UiNavigationService`
- `AudioService`
- `SettingsService`
- `AnalyticsService`
- `AdsService`
- `PurchaseService`
- `RemoteConfigService`
- `NotificationService`

SDK-facing сервіси мають бути за власними інтерфейсами. UI і gameplay не повинні напряму викликати Firebase, Ads, Billing, Android plugins або backend SDK.

## MVP Scope

Рекомендований MVP:

- Boot scene.
- Main menu.
- Battle scene.
- Один tutorial battle.
- Проста campaign path з 8-12 handcrafted missions.
- 4-6 оригінальних видів зброї.
- 2-3 оригінальні архетипи юнітів.
- Local save.
- Одна soft currency.
- Basic inventory/loadout.
- Settings для audio.
- Language-ready text keys.
- Analytics interface з local/no-op реалізацією.
- Ads/IAP/remote config interfaces з no-op реалізаціями.

Після перевіреного прототипу launch direction:

- 12-16 оригінальних видів зброї;
- 4-6 оригінальних character archetypes;
- 4-6 арен;
- одна soft currency;
- account level, character growth, weapon mastery як ранні progression layers;
- ранній доступ до casual PvP або safe PvP-like equivalent;
- normalized competitive power, якщо буде ranked PvP.

Відкласти до моменту, коли core loop стане fun:

- реальний Ads SDK;
- реальний Billing/IAP SDK;
- push notifications;
- backend login;
- clans/social features;
- large event systems;
- complex crafting;
- many currencies;
- heavy upgrade trees.

Уникати постійно:

- paid/VIP combat advantage в ranked play;
- forced ads після кожного матчу;
- progression power gap між новими й старими гравцями;
- account/save/purchase flows без restore/recovery plan.

## Data Strategy

Для early development використовувати ScriptableObjects:

- weapon definitions;
- unit archetypes;
- mission definitions;
- reward tables;
- progression chapters;
- tutorial steps;
- UI theme references.

Для local save використовувати JSON або versioned serializable model. Save data має зберігати прогрес і ownership нашого гравця, а не посилання на чужий контент.

## Напрям залежностей

Бажаний dependency direction:

```text
UI -> App/Core interfaces
Gameplay -> Core/Data interfaces
Progression/Economy/Inventory -> Core/Data interfaces
Infrastructure -> Core interfaces
App -> composes concrete implementations
```

Gameplay не залежить напряму від ads, purchases, Firebase, Android plugins або backend.

## Передфічевий research checkpoint

Для кожної суттєвої системи:

1. Знайти аналоги або сигнали у двох reference packages в `Examples/`.
2. Подивитися APK/XAPK structure, assets categories, layouts/configs/classes, якщо релевантно.
3. Виписати 3-7 високорівневих уроків: що взяти як принцип, що спростити, чого уникати.
4. Не переносити names, values, files, layouts або logic 1:1.
5. Створити власну модель/інтерфейс/config.
6. Оновити docs, якщо фіча змінює архітектурний напрям.

## Рішення, які треба переглянути пізніше

- Чи використовувати Addressables після росту content volume.
- Чи додавати dependency injection framework або лишити легкий власний composition root.
- Який terrain approach обрати: Unity 2D colliders, tilemaps, generated meshes, destructible mask.
- Який multiplayer path: offline/local first, async later або real-time later.
