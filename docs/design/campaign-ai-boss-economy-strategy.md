# Campaign AI and boss economy strategy

Status: planning baseline. This document defines how PvE opponents receive weapons, weapon ranks, ammunition and boss rewards without depending on the player's exact inventory.

## Goals

- Story missions teach the next tactical tool without letting opponents use unexplained late-game power.
- AI difficulty changes decision quality, not hidden damage multipliers.
- Opponent loadouts respect the same rank economy as the player, but do not require the player to own every weapon.
- Bosses are optional, repeatable mastery targets with capped economic output and meaningful first-clear rewards.

## Sources of opponent strength

Each mission resolves four independent inputs:

| Input | Owner | Meaning |
|---|---|---|
| `planetLevel` | planet/chapter | Expected commander level at entry to the planet. |
| `missionProgress01` | mission | `0` for the first mission of a chapter and `1` for its finale. |
| `missionPressure` | mission authoring | `0` tutorial, `1` normal, `2` hard, `3` chapter finale. |
| `aiDifficulty` | AI profile | Easy, Medium, Hard: thinking quality only. |

The player inventory is never copied directly. A mission may preview one upcoming weapon, but may not introduce a weapon more than one planet tier ahead of its normal unlock.

## Expected player weapon rank

The existing progression catalog is authoritative:

```text
weapon unlock level = U
rank 1: U
rank 2: U + 0
rank 3: U + 1
rank 4: U + 2
rank 5: U + 3 (branch selection)
rank 6: U + 5
rank 7: U + 7
rank 8: U + 9
rank 9: U + 12
rank 10: U + 15 (capstone)
```

For a commander level `L`, the economically plausible rank is the highest rank whose required level is at most `L`. This is the baseline for AI. It mirrors availability, not an assumption that every player has bought every upgrade.

### Mission AI rank formula

```text
expectedLevel = planetLevel + round(missionProgress01 * planetLevelSpan)
baseRank = highestEconomicallyPlausibleRank(weapon, expectedLevel)
pressureBonus =
  0 for tutorial/normal early mission
  1 for normal late mission or hard early mission
  2 for hard late mission or chapter finale

aiRank = clamp(baseRank + pressureBonus, 1, rankCap)
```

`rankCap` protects first contact with a planet:

| Mission type | Max AI rank |
|---|---:|
| Tutorial / first mission | 2 |
| Normal mission | 4 |
| Hard mission | 5 |
| Chapter finale | 6 |
| Optional boss | Defined by boss tier, maximum 8 before commander level 15 |

Rank 5 may use one authored A/B branch only if the mission explicitly teaches or telegraphs that mechanic. Ranks 9–10 are reserved for late endgame bosses; they must not appear in ordinary story missions.

Example: a planet intended for commander levels 8–13 has `planetLevel=8` and `planetLevelSpan=5`. A weapon unlocked at level 5 is rank 5 at chapter entry, rank 6–7 near the end, while an early hard mission may receive one pressure rank. A weapon unlocked at level 12 remains rank 1 when first introduced.

## AI loadout and ammunition

Each mission authors a three-slot loadout from the planet's unlocked weapon pool:

1. **Primary**: reliable damage tool, 2–3 uses.
2. **Tactical**: terrain, area, control or positioning tool, 1–2 uses.
3. **Fallback**: low-damage basic tool with unlimited uses.

No high-impact weapon is unlimited. AI ammunition is visible in the battle HUD after its first use. This prevents hidden-resource cheating and makes stalling strategies readable.

| Encounter | Primary ammo | Tactical ammo | Special ammo |
|---|---:|---:|---:|
| Tutorial | 2 | 0–1 | 0 |
| Normal | 3 | 1 | 0 |
| Hard | 3 | 2 | 1 only when telegraphed |
| Finale | 3 | 2 | 1 signature use |
| Boss | 3–4 | 2–3 | 1–2 signature uses |

The fallback weapon may be unlimited but is intentionally weaker than the player's comparable basic weapon. It avoids empty AI turns without producing infinite rocket, EMP, mine or healing pressure.

## Difficulty behavior

Difficulty does not alter damage, health or ammunition after the loadout has been authored.

| Profile | Aim probe | Weapon choice | Positioning |
|---|---|---|---|
| Easy | One coarse solution, large aim error | Prefers primary | Rarely uses cover or terrain setup |
| Medium | Several valid probes, moderate error | Matches weapon to distance/cover | Moves toward cover and avoids obvious falls |
| Hard | Multiple probes with risk scoring | Uses terrain removal, area denial and utility intentionally | Values retreat, height, kill zones and pickups |

AI uses the same trajectory and collision simulation as the player. It receives no hidden target coordinates, random outcomes or impossible line-of-sight information.

## Boss ladder

Bosses are not story gates and do not scale down to the player. Every boss has a fixed `recommendedCommanderLevel`, fixed health, fixed loadout tier and a legible tactical identity. A player below the recommendation may enter, but the encounter is expected to be difficult; later progression makes an older boss naturally easier.

Initial ladder: boss tiers 5, 10, 15, 20, then every five commander levels. Boss rank is resolved from its recommended level using the same rank formula, with a boss cap of `baseRank + 1`; it never receives rank 10 before its recommended level makes rank 10 economically plausible.

Each boss has:

- one signature weapon or branch mechanic;
- one readable arena/positioning pattern;
- a limited signature-ammo count;
- no hidden damage multiplier;
- a counterplay hint shown before the first attempt.

## Boss rewards and weekly limit

Boss reward state is stored per `bossId` and UTC week:

| Clear state | Reward | Purpose |
|---|---|---|
| First lifetime victory | Boss signature weapon unlock or a guaranteed weapon blueprint bundle | Permanent, high-value mastery reward |
| Second lifetime victory | Boss cosmetic skin or cosmetic set fragment | Collectible reward without power inflation |
| Weekly victory after both | Credits + crystals + blueprints, once per boss per week | Repeatable but capped economy source |
| Further victories that week | Practice badge / leaderboard progress only | No currency or blueprints |

Recommended initial weekly boss reward:

```text
credits = 1.5 × a hard arena victory at the boss tier
crystals = 5–10, fixed by boss tier
blueprints = enough for 10–15% of one relevant rank purchase
```

The weekly reward must not exceed the value of roughly two hard battles plus one mission first-completion reward at the same commander band. This makes one weekly boss meaningful without making low-level boss farming optimal.

The first weapon unlock is granted only once and uses the normal ownership path, so it cannot duplicate or bypass ledger safety. If the player already owns the weapon, grant its weapon-specific blueprint bundle instead.

## Required data model

Add data before implementation; do not encode this in bot heuristics:

```text
CampaignAiLoadoutDefinition
  missionId, planetLevel, missionProgress01, missionPressure
  primaryWeaponId, tacticalWeaponId, fallbackWeaponId
  explicitRankCaps, allowedBranches, ammo counts

BossDefinition
  bossId, recommendedCommanderLevel, aiDifficulty
  fixed health/archetype/loadout, signature weapon, signature ammo
  first-clear reward, second-clear cosmetic, weekly reward

SaveBossProgress
  defeatedBossIds, cosmeticRewardedBossIds
  weeklyBossRewardClaims keyed by bossId + UTC week
```

## Rollout and validation

1. Add the rank resolver with tests against all 15 weapon unlock levels and ranks 1–10.
2. Author one loadout per existing story mission; preserve current tutorial restrictions.
3. Add one tier-5 boss as a vertical slice with telemetry for attempts, wins, time-to-win and reward claims.
4. Tune only authored rank caps, ammo and rewards before changing AI damage or health.
5. Validate that weekly rewards, lifetime rewards and duplicate ownership all pass through idempotent economy transactions.
