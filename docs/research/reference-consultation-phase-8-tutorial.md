# Reference Consultation: Phase 8 Tutorial

Feature:
: Lightweight tutorial step system for first battle guidance.

Example signals:
: Reference packages include explicit learning/tutorial content groups and step-like instructional text around movement, aiming, weapon use and objective completion. The useful lesson for MVP is staged teaching with clear triggers, not copying tutorial copy, mission layout or exact step order.

Useful principles:
: Tutorial state should be saved separately from UI. Steps need stable IDs, trigger conditions, prerequisites and completion rules. The first implementation should not block battle flow with heavy modal UI.

Risks to avoid:
: Do not copy tutorial text, target coordinates, mission layouts, UI art, timing or exact lesson sequence. Do not force long interruptions before the player can shoot.

Our original implementation plan:
: Add `Game.Tutorial`, `TutorialStepDefinition`, trigger/completion enums, first-battle runtime step library, `TutorialService` and edit-mode tests. Keep visual tutorial overlay deferred until the basic system is safe.

## Update: Tutorial wired into the first battle (Phase 8.2)

Feature:
: Показ коучинг-підказок у першому бою й прогрес туторіалу, що зберігається.

Useful principles:
: Підказка — короткий тост у момент відповідної події (наведення, постріл, вибух, зміна ходу, перемога), а не блокуючий діалог. Прогрес — стан профілю; кроки завершуються самі при спостереженні тригера й більше не показуються.

Risks to avoid:
: Не блокувати гравця до першого пострілу. Не тягнути залежність gameplay від app/tutorial-збірок.

Our original implementation:
: Міст `Game.Core.BattleTutorialBridge` + `IBattleTutorialFeed`; app-адаптер `AppBattleTutorialFeed` мапить події бою на тригери туторіалу, повертає повідомлення кроку, завершує його і зберігає прогрес. Бій репортить `mission-start`/`aim-started`/`shot-fired`/`projectile-impact`/`battle-won` і показує повідомлення як коуч-банер у HUD-статусі ~4.5с. Gameplay бачить лише Core-контракт, тож не залежить від app/tutorial. Покрито edit-mode тестом мапінгу/завершення.
