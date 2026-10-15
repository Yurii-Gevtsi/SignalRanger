# Нотатки аналізу APK/XAPK

Ці нотатки підсумовують технічні спостереження з прикладів у `Examples/`. Приклади можна глибоко аналізувати для розуміння архітектури, систем, SDK, assets і gameplay-підходів. Не можна переносити 1:1 код, assets, names, configs, balance, layouts або proprietary logic.

## Поточний контекст проєкту

- Виявлено Unity-проєкт.
- Unity editor version: `6000.5.1f1`.
- Packages: URP, Input System, UGUI, Visual Scripting, Test Framework, AI packages, Timeline, common Unity modules.

## Example A: XAPK-style split package

Спостереження з першого reference package:

- Package представлений як base APK плюс split APK для мов і screen densities.
- Manifest metadata показує modern Android target і billing/ad-related permissions.
- File inventory схожий на 2D mobile game package з багатьма bitmap assets, audio, video, Android resources і Google mobile service libraries.
- Явних Unity, Unreal або Cocos markers у base APK listing під час першого проходу не видно.
- Broad SDK categories: ads, billing, Google Play services, AndroidX, app measurement/identifier support.
- Broad feature categories за asset folder names: gameplay HUD, shop, rewards, weapons, maps, unit customization, menu screens, win/result screen, environment effects.

Висновки для нашої гри:

- Android SDK dependencies ізолювати за власними service interfaces.
- Content categories планувати окремо від code modules.
- Не стартувати з повним scope shops, rewards, customization і many weapons; вводити поступово.
- Для кожної схожої фічі можна дивитися, як вона присутня у структурі прикладу, але не копіювати файли, назви або значення.

## Example B: unpacked Adobe AIR/Flash-style package

Спостереження з другого reference package:

- Package має AIR/Flash indicators, SWF content і AIR extension folders.
- Native libraries присутні в `lib/arm64-v8a`.
- Multiple DEX files і багато property/version files вказують на broad Android SDK integration layer.
- Broad SDK categories: Firebase, Google Play services, billing, identity/auth, app update/review, notifications, push notifications, analytics/measurement, remote config/performance-related components.
- Asset categories: layouts, maps, particles, sounds, UI, localization, sprites, authored animation/content files.
- Є mobile і desktop layout categories, що натякає на shared content across device profiles.

Висновки для нашої гри:

- Live game може накопичити багато services, але MVP не має від них залежати.
- Починати offline-first із local saves і small economy.
- Analytics, remote config, ads, IAP, notifications додавати як adapters після стабілізації core gameplay.
- Другий reference package корисний як приклад масштабу й пасток live-service growth.

## Як використовувати приклади перед фічею

Перед новою системою агент має:

1. Знайти відповідники у двох reference packages в `Examples/`.
2. Виписати system role, visible UX pattern, SDK/data/assets implications.
3. Виписати ризики: overload, fairness, trust, monetization pressure, UI clutter.
4. Спроєктувати власну простішу версію.
5. Не переносити exact names, stats, configs, files або proprietary logic.

## Safe Inspiration

Можна брати як принцип:

- turn-based tactical combat;
- short session structure;
- simple weapon selection;
- readable mobile HUD;
- tutorialized first battle;
- level/chapter progression;
- optional rewards/shop layers;
- separation between battle scene and meta screens;
- SDK facade pattern for ads, analytics, IAP, notifications, remote config.

## IP застереження

- Не використовувати чужі assets.
- Не використовувати чужий balance.
- Не відтворювати proprietary logic 1:1.
- Не клонувати UI screens.
- Не повторювати exact file structure.
- Не копіювати names for classes, weapons, items, maps, modes, configs або events.
- Дозволено аналізувати, порівнювати, вчитися і формувати власні рішення.
