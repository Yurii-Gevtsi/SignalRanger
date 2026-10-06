# Weapon 13 implementation evidence

Scope: canonical mortar card 13, based on repository HEAD ab2d91442a37aa6ca9c353dc215ae85f648d70a9. No later weapon implementation is included.

Runtime changes: main impact carves terrain before spawning exactly two base side fragments. Purchased cascade-mortar spawns all two to six physical fragments together; waiting fragments continue gravity and terrain reflection, recheck support after every crater, and detonate in order after 0.8 seconds of contact. Each inherits the effective primary blast radius and half the effective primary damage. Magnet attraction and interception use the shared physical projectile path; projectile removal releases queue membership. Lifetime prevents indefinite flight. Main shot uses shared speed 12.

Progression changes: one rank-five feature, five total purchases, no rank-ten alternative. Save normalization retains the fifth mortar purchase. Explicit TTH choices prevent feature ranks from inventing unpurchased TTH upgrades. Loadout increases stock from two to six. Radius has two upgrades of 10% each. Existing B evolution icon is used for the sole feature; the existing mortira_shot texture is used at smaller scale for fragments.

Evidence: initial RED receipt dff698a6-2dcb-4e47-8684-332cf82dbbfd, preserved Captures/weapon12/mortar-original-red.xml. GREEN receipt fa8def3b-b0fd-4176-ad81-6273234a8429, Captures/weapon12/mortar-red.xml: eight focused tests passed, including each purchase count, physical descent into successive craters, stable impact intervals, full fragment radius and sole feature/icon. Broader progression/save and production scene tests are tracked in the final validation report.

Boundaries: the global maximum-HP-by-level curve remains an explicitly pending design input. Prototype primary damage remains 48; fragment damage is half the effective upgraded shot, never a percentage of the target's current HP. No final economy curve or later weapon is invented here.
