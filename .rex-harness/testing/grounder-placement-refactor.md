# Grounder placement refactor check

The implementation remains a direct `TryFire` dispatch into `DeployGrounder`; no extraction or behavior change is justified yet. `ActiveGrounder` is a device record reserved for the interception/lifetime slice of the same weapon. The test diff was reviewed: it still asserts the public `TryFire` result, deployment event, unchanged owner HP, and exhausted player weapon through `CanUsePlayerWeapon`; the inventory assertion was corrected only because the action advances the active side to the opponent. No assertion was removed or tolerance weakened.

Focused Unity EditMode check repeated successfully, exit 0: `receipt:0086bbce-4cf8-4420-b23e-b97e974ee9b9`. Full weapon-28 completion is not claimed by this placement slice.
