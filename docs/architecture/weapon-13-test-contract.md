# Weapon 13 combat test contract

Goal: finish mortar combat behavior from canonical card 13. Non-goals: later weapons, HP cap curve, new artwork, unrelated terrain architecture.

Acceptance mapping: BattleSimulation.TryFire/Tick and public projectile/impact events must expose one arcing primary, exactly two side fragments without feature, full primary explosion radius on each smaller fragment, and purchased cascade-mortar with two rolling fragments at first purchase and three/four/five/six on four subsequent purchases. All upgraded fragments physically coexist, fall and roll with gravity and terrain collision, and detonate in stable sequence after landing, following newly carved terrain. No scripted underground explosion coordinates or simultaneous random contact detonations. Paid WeaponUpgradeService must offer one rank-five feature with five total purchases; no rank-ten alternative. Existing stock two plus four Loadout upgrades produces six. Core/Aim/Radius keep their canonical four/four/two caps. Original sprite mortira_shot remains visible for main and smaller fragments; upgrade icon uses existing B skin.

Seams: existing model harness, paid progression resolver/service, public physics projectile state, production BattleSceneController/HUD scene smoke. Tests may set deterministic terrain and sample projectile positions, impact times and terrain solidity. Final closure requires targeted EditMode and production PlayMode tests, no weakened expectations, all language integrity and specification/status updates.

Existing legacy tests require obsolete five base fragments, half-radius fragments and two scripted decreasing cascade charges. Replace these expectations with accepted behavior, preserve main-shot and turn lifecycle coverage. RED begins with the canonical two-fragment count and catalog purchase caps.

Implementation boundary: adapt existing grenade collision/rolling physics for mortar fragments and keep ordering policy in BattleSimulation. Reference study confirmed main impact precedes physical shard creation; adapt the principle with our counts and sequential crater policy, without copying code or names.
