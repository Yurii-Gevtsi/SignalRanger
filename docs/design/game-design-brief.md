# Короткий дизайн-документ гри

## Робоча концепція

Оригінальна мобільна 2D покрокова tactical artillery-гра про маленькі команди виразних бійців, які воюють на компактних інтерактивних аренах. Гра має бути зрозумілою за першу хвилину, приємною для коротких повторних сесій і здатною розростатися через нову зброю, арени, події та прогресію.

Робочий опис жанру:

```text
Короткі tactical artillery-дуелі з чесною фізикою, читабельним terrain, легкою прогресією і поступовим ростом контенту.
```

Це оригінальний проєкт. Не копіювати назви, assets, balance, layouts, configs, weapons, maps, characters або proprietary systems з існуючих ігор. Приклади можна глибоко аналізувати для принципів і сильних практик.

## Північна зірка дизайну

Гравець має відчувати:

```text
Я зрозумів ситуацію.
Я зробив розумний постріл.
Результат був чесним.
Я отримав прогрес.
Я хочу ще один матч.
```

## Product Position

Гра має бути між двома крайнощами:

- чистіша й доступніша, ніж важкі live-service tactical games;
- глибша з часом, ніж одноразова arcade-гра.

Перша публічна версія має відчуватися як polished battle game з легкою кампанією та progression, а не як crowded meta-system.

## Цільовий досвід гравця

Перша сесія:

- гравець швидко стартує;
- перший бій навчає aiming і power;
- гравець стріляє протягом першої хвилини;
- результат читається навіть при промаху;
- бій швидко завершується;
- гравець отримує маленьку нагороду;
- наступна місія або матч очевидні.

Довший термін:

- поступово відкривається нова зброя;
- гравець знаходить різні архетипи персонажів;
- вивчає arena hazards і terrain tricks;
- покращує weapon mastery;
- повертається за подіями, аренами й короткими challenges.

## Core Loop

```text
Обрати місію або матч
-> увійти в бій
-> рух/прицілювання/постріл під час ходу
-> реакція на terrain і хід суперника
-> перемога або поразка
-> rewards і mastery progress
-> unlock або покращення опцій
-> ще один короткий матч
```

## Battle Rules для MVP

Початкові правила:

- 1 team versus 1 team;
- 1-2 units per side на ранніх prototypes;
- turn-based actions;
- one active unit per turn;
- limited move range;
- one shot per turn;
- simple health;
- victory when all enemy units are defeated;
- цільова тривалість бою: 2-5 хвилин.

Додати пізніше:

- wind;
- water/fall deaths;
- multi-unit teams;
- arena hazards;
- alternate objectives;
- ranked rules;
- event mutators.

## Controls

Якість керування є top priority.

Prototype control model:

- drag або hold to aim;
- видимий aim direction;
- видимий power indicator;
- explicit fire confirmation;
- без accidental release-to-fire, доки playtests не доведуть, що це добре;
- cancel aim button або gesture;
- trajectory preview для tutorial і early weapons.

Постріл має відчуватися чесним до додавання progression або economy.

## Scope першого прототипу

Перший Unity prototype доводить тільки battle feel:

- одна battle scene;
- два placeholder units;
- turn switching;
- aim and power control;
- one projectile weapon;
- projectile arc;
- collision with unit або terrain;
- simple radial damage;
- health display;
- win/loss state;
- restart button.

Без shop, account level, ads, IAP, login, live services.

## MVP Scope

Після того як перший прототип відчувається добре, MVP має включати:

- boot flow;
- main menu;
- battle scene;
- short tutorial;
- 8-12 handcrafted campaign missions;
- 4-6 original weapons;
- 2-3 original character archetypes;
- 2-3 arenas;
- local save;
- basic loadout;
- one soft currency;
- simple reward screen;
- audio settings;
- no-op interfaces for analytics, ads, IAP, remote config, notifications.

## Scope першої публічної версії

Перша публічна версія може вирости до:

- 12-16 original weapons;
- 4-6 original character archetypes;
- 4-6 arenas;
- offline AI battles;
- campaign progression;
- early casual PvP або safe PvP-like equivalent, якщо feasible;
- account level;
- character growth;
- weapon mastery;
- one soft currency;
- optional premium currency лише для cosmetics, pass або convenience later.

## Оригінальні архетипи персонажів

Починати з tactical role, а не копій fantasy/species/classes.

Ранні archetypes:

- `Striker`: balanced movement, reliable damage, easy for beginners.
- `Tinker`: utility tools, terrain placement, indirect tactics.
- `Bulwark`: higher health, defensive tools, slower movement.
- `Scout`: mobility and repositioning, lower health.
- `Medic`: light support and recovery, limited damage.
- `Saboteur`: status effects, traps, disruption.

MVP стартує з 2-3 архетипів. Більше лише після стабілізації core rules.

## Оригінальні ролі зброї

Починати з role categories перед фінальними назвами, visuals і stats.

MVP weapon set:

- direct projectile;
- arcing explosive;
- short-range burst;
- terrain placement tool;
- defensive shield або cover tool;
- movement/repositioning tool.

First public version може розширитися:

- delayed explosive;
- bouncing projectile;
- area denial;
- low-damage multi-hit;
- healing/support shot;
- knockback tool;
- precision high-risk shot;
- terrain cutter;
- status effect tool;
- event-only experimental weapon.

Правила:

- кожна зброя має мати чітку tactical purpose;
- уникати зброї, яка вирішує матч до появи counterplay;
- тримати animations короткими;
- дозволити problematic weapons вимикати в special/private modes later;
- tuning робити з playtests, а не з balance values прикладів.

## Напрям арен

Цілі арен:

- достатньо компактні для коротких матчів;
- читабельні на mobile;
- різні terrain shapes;
- safe spawn positions;
- no first-turn cheap deaths;
- clear hazards лише після розуміння базового movement і shooting.

Тематика арен має бути оригінальною. У прототипі використовувати placeholder geometry.

## Напрям прогресії

Три ранні progression layers:

- account level;
- character growth;
- weapon mastery.

Вони мають бути читабельні:

- account level відкриває broad access;
- character growth дає невеликі PvE-oriented improvements і cosmetic identity;
- weapon mastery підтримує commitment і unlocks variants/cosmetics перед power creep.

Ranked PvP має normalizе combat power, щоб старі акаунти або paying players не домінували через stats.

## Напрям економіки

Early economy:

- one soft currency;
- rewards from missions and battles;
- currency for unlocks/basic upgrades/cosmetics depending on balance;
- no crafting resource web;
- no VIP combat power;
- no many-currency system.

Premium currency, якщо буде later:

- cosmetics;
- season pass;
- convenience;
- non-ranked expression;
- no ranked combat dominance.

## Напрям PvP

PvP має з’явитися достатньо рано, щоб гравець відчув tactical tension, але воно має бути захищене.

Principles:

- casual PvP before ranked PvP;
- bot-assisted PvP-like mode acceptable без backend;
- normalized stats in ranked;
- matchmaking by skill and arsenal band;
- reconnect plan before serious ranked launch;
- server validation і anti-cheat до meaningful competitive stakes.

## Напрям монетизації

Monetization should not punish playing.

Дозволено later:

- optional rewarded ads;
- cosmetics;
- battle pass або event pass;
- remove-ads purchase;
- convenience purchases без ranked dominance.

Уникати:

- forced ad after every match;
- two forced ads in a row;
- paid/VIP combat advantage;
- purchase flows без restore and account recovery planning.

## Live-Ops Direction

Перевага тимчасовій свіжості замість permanent system bloat:

- seasonal arenas;
- weekly boss challenges;
- temporary weapon pools;
- mutator events;
- short PvE challenge chains;
- cosmetic rewards;
- limited-time casual PvP rules.

Не додавати новий permanent economy layer у кожному update.

## Що не будуємо спочатку

Не в першому прототипі:

- campaign;
- shop;
- inventory;
- ads;
- IAP;
- login;
- cloud save;
- multiplayer backend;
- complex destructible terrain;
- many weapons.

Не в MVP:

- clans;
- real-money shop;
- push notifications;
- complex crafting;
- many currencies;
- ranked PvP;
- seasonal battle pass;
- large live-ops system;
- advanced anti-cheat.

Уникати постійно:

- copied content from existing games;
- paid combat dominance;
- overloaded first-session UI;
- unfair spawn logic;
- hidden formulas that make losses feel fake;
- progression that buries the fun.

## Найближча implementation target

Build `Milestone 1: Fair Shot Prototype`.

Acceptance criteria:

- project opens in Unity;
- battle scene playable in editor;
- player can aim deliberately;
- player can fire one projectile;
- projectile arc is readable;
- hit detection works;
- damage updates health;
- turns alternate;
- match ends with win/loss;
- restart works;
- no external SDK required.

