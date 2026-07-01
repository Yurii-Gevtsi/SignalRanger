# Reference Consultation: Phase 2 Arenas

Feature:
: Перший data-driven шар арен для `BattleScene`.

Example signals:
: У `Examples/Wormix` карти винесені в окрему групу даних/assets, а не змішані з UI чи економікою. У `Examples/Warlings` також видно окрему категорію `assets/maps` разом із суміжними gameplay/environment групами.

Useful principles:
: Арена має бути окремою content/data сутністю: spawn points, camera framing, background/terrain blocks і collision. Battle scene повинна збирати арену з визначення, а не тримати всі координати всередині одного scene controller.

Risks to avoid:
: Не копіювати чужі мапи, назви файлів, layouts, точні координати, assets або баланс. Не переносити чужу файлову структуру 1:1. Не робити destructible terrain до того, як базові spawn/collision/camera правила стабільні.

Our original implementation:
: Додано власні `ArenaTerrainBlock`, `ArenaDefinition`, `PrototypeArenaLibrary` і три оригінальні runtime arenas: `Training Meadow`, `Split Ridge`, `Central Cover`. `BattleSceneController` тепер будує terrain і spawn points з `ArenaDefinition`, а `N`/`Next Arena` перемикає арени в Play Mode.
