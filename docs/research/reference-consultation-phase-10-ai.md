# Reference Consultation: Phase 10 AI

Feature:
: Simple offline AI shooter for MVP battles.

Example signals:
: Reference packages include PvE/bot and learning-battle signals, plus progression that depends on repeatable offline encounters. The useful lesson for MVP is readable challenge and pacing, not copying aim formulas, bot names, difficulty tables or encounter scripts.

Useful principles:
: AI should use the same battle rules as the player, make one clear action per turn and miss sometimes later through difficulty tuning. First pass can be deterministic and simple so shot feel remains debuggable.

Risks to avoid:
: Do not copy bot algorithms, exact difficulty curves, encounter logic, names, target priorities or balance. Do not let AI use unavailable weapons or bypass turn/action locks.

Our original implementation plan:
: Add a pure `SimpleAiShooter` that estimates direction/power from active unit to target, cover it with edit-mode tests, and let `BattleSceneController` auto-fire on opponent turns after a short delay through the same weapon executor path.
