# MVP Implementation Summary

Status: MVP feature scope is closed.

This document is the current source of truth for what has been implemented in the playable MVP. It replaces the old phase roadmap as a concise handoff and review snapshot.

## Product Slice

The MVP is a compact mobile tactical artillery game loop:

- Boot to menu.
- Choose mission and loadout.
- Enter a short turn-based battle.
- Aim, select weapon, fire or use utility actions.
- Resolve damage, cover, healing, movement and win/loss state.
- Show result and reward preview.
- Persist progress, settings, inventory and loadout locally.

The intended direction is a simpler, accessible artillery game with readable combat, short sessions and room for later progression/economy/live-ops layers without adding pay-to-win pressure to the combat core.

## Implemented Systems

### Project Structure

- Unity project organized under `Assets/_Game/`.
- Runtime code split into App, Core, Gameplay, UI, Data, Inventory, Economy, Progression, Audio and Infrastructure layers.
- Core gameplay remains testable without Android SDKs, ads, billing, backend or external services.
- No-op infrastructure interfaces exist for future analytics, ads, IAP, remote config and notifications.

### App Flow

- Boot scene initializes app services and routes into menu.
- Menu screen provides campaign entry points, loadout and settings.
- Battle scene is bootstrapped automatically by `BattleSceneBootstrap`.
- Result overlay appears after battle finish and supports retry/continue flow.

### Battle Core

- Turn-based battle state with player and opponent teams.
- Active unit, active team, turn number and action-spent state are tracked by domain services.
- Player aim uses drag/mouse input with explicit fire confirmation.
- Projectile flight uses Unity 2D physics and per-weapon gravity/power values.
- Trajectory preview matches ballistic projectile gravity for projectile weapons.
- Turn flow waits for all projectiles in a multi-projectile shot before advancing.
- Win/loss/draw evaluation happens after impacts and utility actions.

### Units And Arenas

- Three runtime character archetypes: striker, skirmisher and bulwark-style roles.
- Three runtime prototype arenas with terrain blocks, camera settings and spawn points.
- Terrain collision exists.
- Destructible terrain is intentionally not part of MVP.

### Weapons

Eight runtime weapons are available for MVP testing:

| Weapon ID | Role | MVP Behavior |
|---|---|---|
| `weapon.steady-orb` | Direct projectile | Balanced starter projectile with low arc. |
| `weapon.pulse-shell` | Arcing explosive | Lobbed explosive projectile with broader impact. |
| `weapon.close-pop` | Short-range burst | Instant short cone hit, no projectile arc and no self-damage. |
| `weapon.long-lance` | Direct projectile | Fast, flatter precision shot with smaller impact radius. |
| `weapon.scatter-arc` | Arcing explosive variant | Weapon-specific fan shot with three projectiles. |
| `weapon.rampart-kit` | Cover tool | Short direct placement preview; creates a cover block at the preview end point. |
| `weapon.guard-pulse` | Defensive tool | Instant self/team-side heal pulse without trajectory preview. |
| `weapon.relay-step` | Movement tool | Ballistic relay projectile; owner teleports to the landing point. |

Weapon implementation details:

- `WeaponDefinition` holds stable id, display key, role, damage, radius, gravity, power range, ammo and tags.
- `WeaponInventoryRuntime` handles selection and ammo.
- `WeaponExecutor` dispatches effects by weapon id first, then role, then default projectile behavior.
- `BurstProjectileEffect` supports multi-projectile shots.
- `ProjectileImpact` carries projectile-specific damage and radius, so split or special shots resolve correctly.
- Player loadout and opponent inventory are separated; opponent currently uses a small starter weapon pool.

### UI

- Runtime UI uses TextMesh Pro.
- Shared UI text lives in `Game.Core.Localization.GameText`.
- Battle HUD shows health, turn/status, selected weapon, ammo, power and action buttons.
- Loadout modal supports scrolling, selection, save and development/test unlock path.
- Settings modal supports audio settings and saves volume.
- UI was adjusted for mobile-style large controls and modal flows.

### Progression, Economy And Save

- Eight campaign missions exist in `MvpMissionLibrary`.
- Campaign progression supports mission completion, rewards and unlocks.
- One soft currency is implemented.
- Inventory supports unlocked weapon IDs and selected loadout.
- Save data is JSON-based under `Application.persistentDataPath`.
- Save migration/fallback path exists.
- Settings are persisted.

### AI

- Simple opponent AI aims and fires through the same battle fire path as the player.
- AI difficulty is selected per mission/session.
- AI weapon pool is restricted separately from the player's loadout.

### Feedback And Audio Hooks

- Battle feedback events exist for aim, shot, impact, damage, defeat and battle finish.
- Audio service interface and no-op implementation exist.
- Real audio clips are not required for MVP closure.

### Android And Build Settings

- Android package identifier was set.
- Product/company metadata was set.
- ARM64 target architecture is configured.
- Custom Android manifest is intentionally not enabled yet.
- Real SDK integrations remain outside MVP.

## Verification Status

Known verification state:

- User-reported Unity test run: 76 tests green.
- User-reported compile state after fixes: no compile errors.
- Manual gameplay smoke passed for menu, battle, loadout and all tested weapon behaviors.
- `git diff --check` passed during final documentation update, with only expected Windows LF to CRLF warnings.

Manual weapon smoke covered:

- All eight weapons selectable after test unlock/loadout save.
- Projectile weapons have distinct behavior/visual feel.
- Scatter waits for all projectiles before ending the turn.
- Close-range hit no longer uses a projectile trajectory.
- Cover placement uses short direct placement preview.
- Guard pulse heals without trajectory.
- Relay projectile teleports to its landing point.
- Opponent no longer mirrors every selected player weapon.

## Remaining Release Gate

These items are not MVP feature work; they are build/release-readiness checks:

- Run the full Unity EditMode/PlayMode test suite one final time.
- Make an Android build.
- Run a profiler pass for frame rate, allocations, cleanup and memory growth.
- Smoke test on a real or representative Android device.
- Review Android permissions, package id and orientation before external distribution.

## Known MVP Limitations

- Visuals are placeholders.
- No destructible terrain yet.
- No real ads, IAP, notifications, backend login or remote config.
- No ranked PvP, clans, crafting, events or live-ops.
- Audio hooks exist, but final sound design is not implemented.
- Economy is intentionally simple.
- Balance is suitable for MVP testing, not production tuning.

## Suggested Post-MVP Backlog

- Replace placeholder visuals with a coherent art direction.
- Add real projectile/impact VFX and audio.
- Add destructible terrain or terrain deformation if it improves the core loop.
- Expand arena variety.
- Add weapon upgrade/mastery only after balance is stable.
- Improve AI weapon choice and tactical positioning.
- Add first Android build pipeline and device matrix.
- Add analytics only around key funnels after privacy/SDK decisions.
- Consider ads/IAP only through service wrappers and without competitive combat advantage.
