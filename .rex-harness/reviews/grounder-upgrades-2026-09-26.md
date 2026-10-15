# Grounder upgrades standards and specification review

Fixed point: `62f03955f5bde09802941c5299044779b5829430`. Current working-tree diff is nonempty; review is confined to grounder progression, choice resolution, placement, and new tests because these shared files contain other uncommitted work. Source: `docs/design/weapon-combat-specification.md:159-163`, canonical catalog, and `AGENTS.md`.

## Standards

The changed paths remain in progression, weapons, and simulation ownership. All 12 smell categories checked. Judgement finding: three grounder-specific branches now live in the generic `WeaponUpgradeChoiceResolver` (`ResolveValue` Core/Radius and `ResolveImpactRadiusPercent`), so another special defensive weapon could duplicate them. No immediate extraction is justified for one weapon; keep a future weapon-specific policy boundary in mind. Other smells not observed in this bounded slice. No player-facing text was added.

## Spec

Four Core and four Radius purchases are available. Combat uses two through six opposing turns and `N0 × 1.0..1.4`; focused Unity tests prove the full four-step endpoints and an actual physical-shot capture outside base range. The exact player-facing Core description in the spec has not been localized or wired to the workshop; this remains a separate weapon-28 completion issue at `docs/design/weapon-combat-specification.md:161`. Severity: judgement. Repair: update all localization dictionaries together using the project's translator and audit gates. Other grounder gaps remain skin rendering, friendly/energy exclusions, and interactive visual verification.

Verdict: upgrade mechanics pass focused tests; weapon 28 is not complete. Standards: one judgement finding. Spec: one outstanding UI-text finding for full weapon scope.
