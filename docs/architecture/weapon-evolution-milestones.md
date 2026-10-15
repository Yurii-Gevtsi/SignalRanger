# Independent weapon evolution milestones

Weapon evolutions are independent paid choices at ranks 5 and 10. The weapon
catalog exposes zero or more options for each milestone through
`WeaponProgressionDefinition.GetEvolutionOptions(rank)`.

- No options at either milestone: the weapon has only its four base TTH paths
  and is not shown in the workshop evolution card.
- One option: it is presented directly in the workshop side panel.
- Multiple options: UI presents one card per option and then purchases the
  selected option with the standard confirmation flow.

`SaveWeaponProgressData.evolutionChoices` persists the selected option by
milestone rank and option ID. The selected rank-10 option supersedes rank-5
for the active weapon identity: its name, description, art and battle profile
are the current weapon. The four base TTH choices remain independent and are
never changed by an evolution.

The old `branch` field remains only for backwards compatibility with profiles
created before independent milestones. New gameplay resolution uses the
independent selection resolver and falls back to the legacy branch only when a
profile has no new selection.
