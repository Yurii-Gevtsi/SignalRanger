# Напрям з аналізу відгуків гравців

Цей документ підсумовує продуктовий напрям із аналізу відгуків про два reference-проєкти. Це не джерело для копіювання content, names, balance, UI, economy або proprietary systems. Це високорівнева інтерпретація player needs, frustrations і opportunities для оригінальної гри.

## Стратегічна формула

Найкращий напрям для проєкту:

```text
читабельність і чистота компактної battle-first моделі
+ відчуття довгострокової прогресії progression-heavy моделі
- перевантаження економікою/підсистемами progression-heavy моделі
- несправедливий PvP, агресивна реклама, account trust problems
```

Практично: спочатку чистий, зрозумілий, приємний battle loop, потім progression layers. Не починати з heavy meta-game.

## Що гравцям подобається

Сигнали жанру:

- short turn-based tactical duels;
- funny або expressive characters;
- varied weapons with clear tactical roles;
- readable projectile physics;
- destructible або interactive terrain;
- PvE і PvP options;
- unlocks that create anticipation;
- new maps, weapons, seasonal arenas, event reasons to return.

Фантазія працює: clever shots, surprising weapons, compact battles, victory through skillful decisions.

## Що ламає довіру

Найнебезпечніші проблеми не в нестачі контенту, а в trust failures:

- незручне прицілювання або accidental shots;
- unfair matchmaking;
- великий power gap між новими й старими гравцями;
- VIP або paid advantages у competitive combat;
- exploits, cheating або unclear server authority;
- lost purchases або broken account migration;
- unstable reconnects;
- ads як покарання після кожного матчу;
- довгі animations або weapons that create cheap losses;
- bad spawn positions, які вирішують матч до першої дії.

У цьому жанрі trust є primary feature. Гравець має вірити controls, physics, match result, saves, purchases і ranking.

## Уроки з progression-heavy reference

Корисні ідеї:

- long-term content progression може утримувати гравців;
- multiple character archetypes поглиблюють team identity;
- PvE і PvP можуть співіснувати;
- events, bosses і new content оживляють гру.

Уникати:

- забагато currencies;
- забагато crafting resources;
- забагато parameters;
- забагато early gates;
- VIP-driven combat advantage;
- перевантаження новачка до real fun;
- permanent systems on top of permanent systems.

Progression-heavy reference корисний як warning про system weight. Зберегти tactical battles і progression, не complexity mass.

## Уроки з battle-first reference

Корисні ідеї:

- compact, readable battle-first product легше зрозуміти;
- moderate weapon roster дає variety без exhaustion;
- offline AI, local hotseat і online PvP закривають різні moods;
- destructible terrain і short sessions є strong genre anchors;
- прохання гравців про більше maps/weapons часто означає, що core loop працює.

Уникати:

- unfair online matchmaking;
- aggressive ad cadence;
- obvious weapon imbalance;
- spawn positions that create unfair early deaths;
- weapons або animations, які waste too much match time;
- відкладати anti-cheat і competitive integrity надто пізно.

Battle-first reference ближчий до product shape, який нам потрібен: battle first, meta second.

## Product Pillars

### 1. Постріл має бути чесним

Перший пріоритет: aiming, power control, projectile readability, collision clarity, feedback. Кожен промах має відчуватися як помилка гравця або readable tactical risk, а не UI failure.

Design implications:

- deliberate fire confirmation;
- visible trajectory preview або partial assist early;
- clear wind/force indicators, якщо wind exists;
- generous touch targets;
- no accidental release-to-fire during early tests;
- instant feedback on damage, knockback, terrain effects.

### 2. PvP має бути безпечним рано

Гравець має рано відчути human tactical tension, але competitive power має бути normalized.

Design implications:

- early casual PvP або bot-assisted PvP equivalent;
- ranked PvP only after basics;
- normalized stats in ranked;
- matchmaking by skill/rating and available arsenal band;
- no paid combat advantage in ranked;
- server-authoritative results when online arrives.

### 3. Progression має бути читабельною

Progression мотивує, але не ховає гру.

Ранні layers:

- account level;
- character growth;
- weapon mastery.

Уникати early:

- many currencies;
- many crafting resources;
- nested upgrade trees;
- VIP battle power;
- dozens of unlock conditions.

### 4. Content має рости через події, не через шум

Long-term freshness краще давати time-limited content і новими battle contexts, а не постійним economy expansion.

Кандидати:

- seasonal arenas;
- weekly boss challenges;
- temporary weapon pools;
- mutator modes;
- short PvE event chains;
- cosmetic event rewards.

### 5. Монетизація не повинна карати за гру

Ads і purchases не мають робити кожен матч “оподаткованим”.

Guidelines:

- no forced ad after every battle;
- rewarded ads optional and clear;
- remove-ads purchase later;
- paid content: cosmetics, convenience, passes, non-ranked expression;
- ranked combat must remain fair.

## Рекомендований launch scope

Small but complete first public version:

- 12-16 original weapons;
- 4-6 original character archetypes;
- 4-6 arenas;
- short campaign/tutorial path;
- offline AI battles;
- local або casual PvP path if feasible;
- simple account level;
- simple character and weapon progression;
- one soft currency;
- optional premium currency only for cosmetics/pass/convenience later.

Для першого prototype це ще забагато. Prototype доводить тільки:

- aiming;
- projectile feel;
- turn flow;
- damage;
- terrain interaction;
- win/loss;
- one readable HUD.

## Priority Risks

1. Bad aiming feel.
2. Cheap losses from spawn, edge falls, overpowered weapons.
3. Matchmaking unfairness.
4. Power gap from progression or monetization.
5. Account/save/purchase trust failures.
6. Too much meta complexity before player loves core.
7. Ads damaging rhythm.
8. Updates adding permanent complexity instead of fresh play.

## Design North Star

Гравець має відчувати:

```text
Я зрозумів ситуацію.
Я зробив розумний постріл.
Результат був чесним.
Я отримав прогрес.
Я хочу ще один матч.
```
