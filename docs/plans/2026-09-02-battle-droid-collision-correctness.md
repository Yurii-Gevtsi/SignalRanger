# Battle droid collision correctness

> AIOS Planning Contract (schema v3)
> created: 2026-09-02T09:10:03.457Z
> client: cli
> source: aios plan start
> route: implement

## Objective

Ensure projectiles detonate on any contact with a droid body, and terrain supports droid movement only through the lower track/platform contact surface; body or weapon terrain contact while airborne must resolve as falling rather than sticking.

## Route skills

1. `Rex selects the current provider`

## Tasks

- [ ] **t1-understand**: Clarify objective: Ensure projectiles detonate on any contact with a droid body, and terrai — _Objective restated; constraints listed_
- [ ] **t2-plan**: Break work into executable tasks — _Plan tasks updated beyond scaffold if needed_
- [ ] **t3-implement**: Implement changes — _Code changes match objective_
- [ ] **t4-verify**: Verify with tests/checks — _Evidence recorded (command or artifact path)_

## Progress

- status: active

## Decision Log

- Test-scope contract (2026-09-02): projectile impact uses the complete
  gameplay droid hitbox, while support and stable movement use only the lower
  track/platform samples. Airborne body, weapon, side, or ceiling contact may
  redirect motion but must never settle the droid or leave it suspended.

## Test-scope contract

- User-visible goals:
  - a projectile detonates when its swept path touches the droid centre, upper
    body, or any other part of the gameplay hitbox;
  - a jumping or knocked-back droid that catches terrain with its body, weapon,
    side, or top remains airborne and subsequently falls to a valid lower-track
    landing surface.
- Non-goals: weapon-specific behaviour that deliberately ignores units
  (teleport, mine deployment, and resting grenade cases), terrain destruction,
  visuals, camera, and balance tuning.
- In scope:
  - `BattleSimulation.TryFindHitUnit` and the projectile fixed-substep loop;
  - `UnitMotor.ResolveCollision`, `IsGrounded`, and lower-track support probes;
  - focused EditMode tests for `BattleSimulation` and `UnitMotor`.
- Out of scope: Unity scene colliders and Physics2D collision callbacks. The
  authoritative gameplay seams are the pure-C# simulation APIs
  (`TryFire`/`Tick`) and `UnitMotor.AddImpulse`/`Step`.

## Acceptance-test mapping

| Observable acceptance behaviour | Public test seam | Focused test |
| --- | --- | --- |
| A shot through the droid centre detonates before passing it | `BattleSimulation.TryFire` + `Tick`, observe `ProjectileExploded` | `BattleSimulationTests` |
| A shot grazing the upper body detonates | `BattleSimulation.TryFire` + `Tick`, observe `ProjectileExploded` | `BattleSimulationTests` |
| A droid colliding with an overhang by its upper body finishes falling and lands by its tracks | `UnitMotor.AddImpulse` + `Step`, observe outcome and `IsStable` | `UnitMotorTests` |
| A non-foot terrain contact cannot make the droid stable | `UnitMotor.AddImpulse` + `Step`, assert stability only after lower support | `UnitMotorTests` |

The tests must exercise the real fixed-step simulation; mock-only tests,
relaxed assertions, and removing existing regression cases are not acceptable.

## Acceptance

- Complete planned tasks and record verification evidence.

## Next Actions

- Start with the first pending task.

## Verification evidence

- Attach via `aios plan add-evidence --kind command|path|test --value "..."`
- Plan cannot be `done` without evidence and completed tasks

## Status

- status: active
