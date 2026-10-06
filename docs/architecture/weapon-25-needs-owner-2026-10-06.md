# Weapon 25 — turn transfer: blocked, needs owner decision

## Status

**Not implemented — requires the owner.** The card depends on team battles that the product does not have yet. Catalog already said "planned; requires team PvP".

## Why it cannot be closed by a weapon-level change

- `TurnService.CreateTurn` always gives the turn to the first living unit of the team; there is no rotation or choice of the acting droid, so a second droid of a team never acts. Transfer needs a defined team turn model (who acts, in what order, how the remaining timer follows the acting droid, how status ticks and "end of turn" apply to a droid that did not act).
- The mechanic is deliberately switched off in `BattleSimulation.IsBattleMechanicAvailable` (together with force field, boost, sky hook, virus, bomber, UFO carrier, self-destruct and nuclear charge).
- The card needs new battle UI: camera at minimum zoom, two left/right buttons, hidden jump buttons, a transfer skin above the chosen ally, the second press confirming, the timer continuing during the choice.

## Decisions needed from the owner

1. Team turn model for team PvP: one acting droid per team turn? the order/rotation? does "end of turn" and status counting follow the team or the droid (spec shared rule 2 already says the per-droid counter for inactive droids "needs agreement with turn transfer").
2. Confirm the proposals in the card: the countdown keeps running during the choice; the charge is spent at confirmation; the left/right press browses allies consecutively in that direction; no ally → confirmation unavailable; no artificial turn end and no extra status tick on transfer.
3. The registry says loadout `1→2` (one purchase of the kit); confirm the TTH limits 0/0/1/0.
4. Art: `UI/Weapons/turn_transfer.png` exists; the "transfer skin shown above the chosen ally" asset is not specified.

## What is ready

- Weapon id, registry entry (`0/0/1/0`), icon `turn_transfer`, localisation texts, and the unavailable-mechanic gate.
