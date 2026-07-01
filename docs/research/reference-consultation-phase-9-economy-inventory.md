# Reference Consultation: Phase 9 Economy And Inventory

Feature:
: Lightweight inventory/loadout ownership and one soft-currency economy service.

Example signals:
: Reference packages show that mature games can accumulate many currencies, unlocks, shop gates, reward sources and item categories. For MVP the useful principle is service ownership and validation, not copying currency names, prices, tables or shop flows.

Useful principles:
: Economy should expose grant/spend validation through a service, not direct UI mutation. Inventory should know owned IDs and validate loadouts before battle. Keep one soft currency until battle feel and mission progression are stable.

Risks to avoid:
: Do not copy economy values, prices, item IDs, rarity systems, unlock tables, shop layouts or monetization logic. Do not introduce premium advantage or multiple currencies in the MVP slice.

Our original implementation plan:
: Add `Game.Economy` with `EconomyService`, `Game.Inventory` with `InventoryService`, app composition wiring and edit-mode tests. Keep shop/loadout UI and reward screen for later passes.
