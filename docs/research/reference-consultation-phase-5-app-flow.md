# Reference Consultation: Phase 5 App Flow

Feature:
: Boot, menu and battle scene flow for the MVP.

Example signals:
: Reference packages show that mobile artillery games quickly collect many entry points around missions, battle, shop, rewards, settings and social/live systems. The useful lesson for MVP is to separate app flow from battle logic early, while keeping the first menu focused on the next playable action.

Useful principles:
: Boot should compose services and route to menu. Menu should make the next battle reachable in one clear action. Scene names should be centralized. Placeholder buttons are acceptable if they communicate future layers without blocking the core loop.

Risks to avoid:
: Do not copy UI layouts, button arrangements, texts, economy hooks, shop flow or scene structure. Do not start MVP with a crowded meta menu. Do not make gameplay depend directly on ads, billing, backend or live services.

Our original implementation plan:
: Add `GameSceneNames`, `ISceneLoadingService`, `UnitySceneLoadingService`, `AppServices`, `GameSessionService`, `GameBootstrapper` and `MenuSceneController`. Add empty Boot/Menu scenes driven by runtime bootstrap code, and set build settings to Boot -> Menu -> Battle.
