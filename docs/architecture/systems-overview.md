# Огляд систем

## Core Game Loop

1. Гравець відкриває гру.
2. Boot scene ініціалізує сервіси й завантажує save.
3. Main menu показує наступну рекомендовану місію або матч.
4. Гравець входить у короткий tactical battle.
5. Battle завершується зрозумілим win/loss feedback.
6. Гравець отримує rewards і progression.
7. Нові weapons, missions або features відкриваються поступово.

Перша сесія не повинна бути перевантажена меню. Гравець має швидко дійти до бою.

## Battle System

Battle system спочатку має бути маленьким і читабельним:

- turn-based teams;
- turn timer пізніше, після базового feel;
- просте movement і aiming;
- wind/environment modifiers лише після tutorial basics;
- weapon selection з малим стартовим набором;
- clear hit feedback;
- deterministic rules там, де це корисно для тестів.

Підмодулі:

- turn controller;
- battle state model;
- unit controller;
- weapon executor;
- projectile simulation;
- damage resolver;
- terrain interaction;
- battle result evaluator;
- battle camera;
- battle HUD presenter.

Перед реалізацією battle-модуля треба подивитися в прикладах: HUD categories, weapon categories, maps/terrain categories, battle layouts, replayable session structure. Переймати принципи, не копіювати implementation.

## Weapons

Починати треба з оригінальної зброї, яка вчить різним поняттям і не перевантажує:

- direct projectile;
- arcing explosive;
- short-range burst;
- utility placement;
- defensive tool;
- movement/repositioning tool.

Кожна зброя має мати:

- stable ID;
- display text key;
- icon reference;
- category;
- aim mode;
- ammo rules;
- cooldown або availability rules;
- damage/effect model;
- tutorial hint key.

Не використовувати назви, іконки, stats або upgrade paths з прикладів. Дозволено аналізувати, які ролі зброї існують у жанрі, які типи викликають cheap losses, які animation timings дратують гравців.

## Progression

Перша версія:

- linear mission path;
- gradual mechanic unlocks;
- small reward after each battle;
- optional replay goals later.

Майбутній розвиток:

- chapters;
- challenge missions;
- events;
- difficulty tiers;
- cosmetic unlocks;
- seasonal reward tracks.

Ранній MVP має уникати важких level gates і багатьох ресурсів. Progression має вести до бою, а не ховати бій за меню.

## Economy

Початкова економіка максимально проста:

- одна soft currency;
- зрозумілі reward sources;
- небагато sinks;
- premium currency лише після окремого monetization design.

Economy data створюється у власних configs і налаштовується через playtests. Не переносити economy values або pricing logic з прикладів.

## Inventory And Loadout

Initial version:

- owned weapons;
- simple loadout slots;
- unlock-only progression;
- no complex crafting in MVP.

Пізніше:

- upgrades;
- skins;
- consumables;
- event items;
- rarity лише якщо вона додає clarity і retention без clutter.

## Tutorial

Tutorial має бути step-based і коротким:

- first aim;
- first shot;
- movement;
- weapon swap;
- objective completion;
- reward screen;
- return to next mission.

Tutorial system посилається на abstract triggers і UI targets, а не hard-coded screen logic.

## UI Navigation

MVP screens:

- boot/loading;
- main menu;
- mission select або next mission panel;
- loadout;
- battle HUD;
- pause/settings;
- result screen;
- simple shop placeholder лише коли economy справді цього потребує.

Не копіювати crowded HUD старих ігор. Перевага: fewer visible choices, larger touch targets, progressive disclosure.

## Save System

Save data:

- schema version;
- player profile;
- completed missions;
- unlocked content IDs;
- owned inventory IDs;
- currency balances;
- settings;
- tutorial progress;
- last reward timestamps, якщо daily rewards увімкнені.

Коли schema змінюється, потрібні migrations.

## SDK And Platform Services

З першого дня використовувати інтерфейси з no-op/local реалізаціями:

- analytics;
- ads;
- IAP;
- remote config;
- notifications;
- backend API.

Gameplay і UI викликають наші interfaces. Real SDK adapters додаються пізніше в `Infrastructure`.

## Recommended First Milestone

Milestone 1 має довести feel:

- Boot to battle.
- One controllable unit per team.
- Turn switching.
- One projectile weapon.
- Health and win/loss.
- Basic mobile-friendly HUD.
- Local restart.

Тільки після цього додавати progression, economy, shop, ads, IAP або notifications.

