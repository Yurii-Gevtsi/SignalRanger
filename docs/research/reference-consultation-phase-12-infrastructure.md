# Reference Consultation: Phase 12 Infrastructure Interfaces

Feature:
: No-op analytics, ads, purchases, remote config and notification service interfaces.

Example signals:
: Reference packages show multiple SDK categories around ads, billing, analytics, notifications, config and platform services. The useful MVP lesson is to isolate SDK access behind our interfaces before real SDK packages are added.

Useful principles:
: Gameplay and UI should call project-owned abstractions. No-op implementations keep editor and early builds deterministic, network-free and permission-free. Real adapters can be added later under infrastructure without touching gameplay logic.

Risks to avoid:
: Do not copy SDK keys, package names, placements, product IDs, permissions, billing flows or notification text. Do not add real SDK dependencies before the MVP needs them.

Our original implementation plan:
: Add small interfaces and no-op implementations for analytics, ads, purchases, remote config and notifications, then expose them from `AppServices`.
