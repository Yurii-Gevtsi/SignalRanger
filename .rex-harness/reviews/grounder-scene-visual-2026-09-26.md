# Grounder scene visual review

Fixed point: `62f03955f5bde09802941c5299044779b5829430`; working-tree diff is nonempty. Reviewed only the grounder visual handlers, event subscriptions, cleanup, and focused test because shared files have unrelated dirty work. Sources: `AGENTS.md`, `docs/design/weapon-combat-specification.md:159-163`.

## Standards

All 12 smell categories checked. The small dictionary and two handlers follow existing mine/laser visual patterns; no new dependency, player text, copied art, or misplaced gameplay rule. No finding in this bounded slice.

## Spec

The existing `grounder.png` is now the visible placed-device skin. The scene removes it for both expiry and projectile destruction through one simulation event. The Unity EditMode test verifies loaded texture, visible SpriteRenderer, world position, and ID cleanup. Remaining full-weapon verification: interactive PlayMode visual scale and collision context, plus negative projectile-class tests. No finding within the scene lifecycle slice.

Verdict: scene visual slice verified by focused test. Standards: zero findings. Spec: zero findings within slice; weapon 28 still incomplete.
