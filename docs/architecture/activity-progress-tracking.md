# Activity progress tracking

## Goal

Quest progress is sourced from persisted gameplay activity, not from UI sample values.
The system owns period reset, progress querying and claim state; combat, campaign and
ads only report domain events.

## Boundaries

- `SaveActivityProgressData` stores a daily and weekly counter set plus claim ids.
- `ActivityProgressService` resets period data using UTC calendar boundaries and
  records activity in both active period sets.
- `ActivityQuestCatalog` contains quest ids, metrics, targets and rewards. A stable
  per-period shuffle selects the visible daily or weekly rows.
- Battle code reports completed matches, player damage, terrain cells removed,
  fired projectiles and player movement. It does not know quest thresholds.
- Campaign completion and rewarded-ad completion report their own domain events.

## Metric semantics

`battlesPlayed` counts finished non-training battles. `arenaBattlesPlayed` counts
finished non-story battles. `storyBattlesPlayed` counts finished story battles.
`storyMissionsCompleted` counts story victories. Damage only counts health removed
from the opponent by the player. Terrain progress counts actual solid grid cells
removed by player-owned effects. Distance is measured in world units and stored as
integer centi-units. Ad progress is recorded only after the platform confirms a
rewarded-ad completion.

## Rotation and claims

Daily and weekly rows are selected deterministically from each catalog pool using
the current UTC period key. The selection remains stable on reopen and changes when
the period changes. Claimed quest ids are persisted with the matching period.

## Extension rule

Adding a quest requires a catalog entry and a metric already reported through the
service. New gameplay systems should report a semantic activity event to the
service rather than change reward-screen UI code.

## Support drops

Support items are scheduled by completed team turns rather than real time. Story
battles use a base interval of three turns, non-story arena battles use four, and
Normal/Hard AI add one/two turns respectively. Training uses a three-turn interval.
A mission can disable support drops through `MissionDefinition.SupportDropsEnabled`.

Each drop chooses a valid random terrain surface away from living units and existing
pickups, then grows an orbital beam from the top of the arena to that surface. The
item appears only when the beam reaches the ground. At most two uncollected support
items may exist. The drop selects either a resource box or a medkit; a medkit restores
25% maximum health, and collecting either item contributes to the resource metric.
