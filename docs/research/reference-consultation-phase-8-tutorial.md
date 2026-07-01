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
