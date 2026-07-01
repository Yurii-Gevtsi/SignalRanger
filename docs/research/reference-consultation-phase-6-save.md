# Reference Consultation: Phase 6 Local Save

Feature:
: Local profile/save data for MVP progress, inventory placeholders, currency, settings and tutorial state.

Example signals:
: Reference packages show that mature artillery games quickly connect profile data with unlocks, currencies, rewards, missions, settings, localization and online services. The useful lesson for MVP is not the complexity itself, but the need for explicit save ownership and versioned data.

Useful principles:
: Save data should be a clear domain snapshot with schema version and safe defaults. Local loading should recover from missing or corrupt data without blocking the player. App flow should read save once during boot/composition, then expose only the small data needed by menu/session.

Risks to avoid:
: Do not copy profile fields, serialized command names, economy values, unlock tables, server contracts or save structure. Do not couple MVP save to backend, ads, billing or live-service systems. Do not apply rewards twice.

Our original implementation plan:
: Add `SaveData`, nested save sections, `SaveDataSerializer`, `ISaveService`, `JsonFileSaveService` and save tests. Wire save creation/loading into `AppServices`, let `GameSessionService` read the next mission from campaign data, and show a minimal profile summary in the menu. Defer migrations and real reward application to later phases.
