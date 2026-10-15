# Weapon 12 implementation boundary

The titanium barrier uses its existing opaque UI sprite for preview and installed terrain. Aim origin is the placement center; aim power carries the uniform placement scale through the existing fire command. Camera zoom never changes physical size. Two touch input owns resizing while placement is active.

The purchased rank-five size feature gives 110% maximum size; four paid Radius choices raise it to 150%. Base Loadout is two, with four purchases giving six. The paid feature rank is excluded from base upgrade counting. Core and Aim remain unavailable.

PlacedTerrainSpriteSurface owns rotated alpha collision and mutable rendered pixels. BattleTerrain preserves these surfaces when moving arena masks refresh, carves them with normal terrain explosions, and disposes owned runtime textures. Installation rejects terrain, unit and world-boundary overlap without spending ammo or the turn action.

Changed production targets: TitaniumBarrierRules, PlacedTerrainSpriteSurface, BattleTerrain, BattleSimulation, BattleSimulationEvents, BattleSceneController, UnitMotor, BattleWeaponProgression, PrototypeWeaponLibrary, WeaponProgressionCatalog, WeaponUpgradeChoiceResolver, WeaponUpgradeService, ArchitectScreenController, titanium_barrier texture readability, and four localization keys across all thirteen dictionaries.

RED: Captures/weapon12/red.xml, three observed failures before implementation. GREEN: Captures/weapon12/green.xml, forty passing model/progression/terrain cases; receipt 7fe7b372-daf3-4df0-9328-b6d57b31d24a. The test fixture was corrected to initialize save lists, without weakening behavior assertions. The old generic cover test now waits for the shared retreat window before requiring handover.

PlayMode input and visual acceptance are tracked separately before final closure. No numeric HP curve is required for this non-damaging utility.
