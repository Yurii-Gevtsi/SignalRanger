# Campaign content canonical sheet

Status: editable planning baseline. Runtime currently contains four planet slots, two authored chapters and eight missions. This document is the single planning location for filling the two reserved planets and expanding the campaign.

## Global campaign rules

- A planet owns one chapter. A chapter owns an ordered mission list.
- Main progression is linear; optional stars and replay goals never hard-lock the next planet.
- Each planet introduces one primary environment rule, one enemy/loadout escalation and one meaningful reward theme.
- A normal player should reach the next story gate with no more than 30–45 minutes of arena play after completing the previous story content.
- Mission rewards must be recorded as credits, commander XP, weapon-specific blueprints and explicit unlocks. Avoid opaque rewards in campaign missions.

## Existing baseline

| Planet slot | Chapter | Missions | Status | Primary arena |
|---|---|---:|---|---|
| 1 | `chapter.basics` | 1–5 | implemented | `arena.desert-sci-fi` |
| 2 | `chapter.tactical-tools` | 6–8 | implemented | `arena.desert-sci-fi` |
| 3 | TBD | TBD | reserved | TBD |
| 4 | TBD | TBD | reserved | TBD |

Evidence: `Assets/_Game/Runtime/App/CampaignPlanetCatalog.cs`, `Assets/_Game/Runtime/App/Content/DefaultGameContentCatalog.cs`, `Assets/_Game/Runtime/Progression/Missions/MvpMissionLibrary.cs`.

## Planet 3 — working design

Working identity: **Gale Foundry**. Final name, art and story text remain editable.

- Chapter ID: `chapter.gale-foundry`
- Mission count target: 6
- Recommended commander-level band: 8–13
- Environment modifier: variable wind; wind changes only at turn boundaries and is previewed qualitatively, never as an exact hit solution.
- Arena set: 2 new wind-shaped arenas plus the existing arena as a low-frequency fallback.
- Enemy escalation: indirect fire, cover use and controlled movement; no new damage multiplier.
- Reward theme: first access to long-range/area tools, credits and blueprints for already-unlocked weapons.
- Story gate: complete the chapter finale and collect one navigation core; no premium purchase gate.

Target mission pacing: 250–850 credits, 100–320 XP, 25–90 blueprints per first completion, tuned against the time-to-rank table in `docs/design/weapon-economy-canonical.md`.

## Planet 4 — working design

Working identity: **Cryo Relay**. Final name, art and story text remain editable.

- Chapter ID: `chapter.cryo-relay`
- Mission count target: 7
- Recommended commander-level band: 14–21
- Environment modifier: slippery/frozen terrain with limited movement disruption; shooting remains available while movement is impaired.
- Arena set: 3 new arenas with distinct cover silhouettes.
- Enemy escalation: mixed utility loadouts, stronger positioning and one scripted boss encounter; no hidden stat inflation.
- Reward theme: utility weapons, evolution blueprints and cosmetic campaign rewards.
- Story gate: complete the boss mission and collect one navigation core; no premium purchase gate.

Target mission pacing: 400–1,200 credits, 160–450 XP, 40–120 blueprints per first completion, tuned against the same time-to-rank table.

## Per-mission authoring template

```text
Mission ID:
Planet / chapter:
Sequence:
Arena ID:
Difficulty:
Environment modifier:
Enemy archetype/loadout:
Allowed weapons:
Primary objective:
Optional objective(s):
First-completion credits / XP / blueprints:
Unlock or blueprint weapon ID:
Recommended commander level:
Story beat:
Replay reward:
Implementation status:
Balance notes:
```

Before moving a planet from `reserved` to `implemented`, add its chapter, mission definitions, arena IDs, tests, localization keys and reward rows to the runtime catalog and update this sheet.

