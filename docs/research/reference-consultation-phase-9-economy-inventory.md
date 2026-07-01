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

## Update: Loadout screen wired to battle

Feature:
: Реальний loadout-екран у меню (замість заглушки) + вплив loadout на бій.

Useful principles:
: Loadout має мати відчутний геймплейний ефект, але не блокувати місію. Джерело правди — save; UI лише редагує вибір і валідує через сервіс. Резолв зброї бою має бути чистою функцією для тестів.

Risks to avoid:
: Не давати loadout софт-локати місію (несумісний вибір → місія неграбельна). Не дублювати правила валідації в UI — тримати їх у сервісі/резолвері.

Our original implementation:
: `LoadoutScreenController` (модальний екран з меню) показує всі зброї, дозволяє перемикати відкриті у loadout, зберігає через `InventoryService.TrySetLoadout` + save. Чистий `BattleLoadoutResolver` рахує зброю бою як `loadout ∩ mission-allowed` з безпечним fallback (allowed → всі), покритий edit-mode тестами. Loadout проходить у бій через `GameSessionContext.SelectedLoadoutWeaponIds`.
