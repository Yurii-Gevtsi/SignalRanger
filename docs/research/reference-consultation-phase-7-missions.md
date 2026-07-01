# Reference Consultation: Phase 7 Missions

Feature:
: Linear MVP campaign missions, next mission CTA, mission rewards and unlock progression.

Example signals:
: Reference packages show separate mission/progression content, tutorial/learning mission groups, map selection signals, reward UI and unlock flows. The useful lesson for MVP is progressive teaching and explicit mission ownership, not copying mission layouts, texts, rewards or economy.

Useful principles:
: Early missions should teach one concept at a time and keep the player close to battle. Mission data should be separate from battle runtime code. Completion should apply rewards once, unlock the next mission and persist progress locally.

Risks to avoid:
: Do not copy mission names, tutorial text, map layouts, unlock costs, reward tables, difficulty curves or UI composition. Do not introduce heavy meta progression before the battle loop is stable.

Our original implementation plan:
: Add `Game.Progression`, `MissionDefinition`, `MissionRewardDefinition`, `MvpMissionLibrary`, `CampaignProgressionService` and campaign tests. Use `GameSessionContext` to pass selected arena and allowed weapon IDs into battle without making gameplay depend on app/progression assemblies. Apply mission completion on successful result continue, then save and return to menu.
