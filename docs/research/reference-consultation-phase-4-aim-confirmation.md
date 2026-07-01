# Reference Consultation: Phase 4 Aim Confirmation

Feature:
: Fix battle input so the fire button does not change the aimed shot while the pointer moves toward UI.

Example signals:
: Reference packages use separate battle controls, weapon controls and fire confirmation affordances. The useful product lesson is that aiming and confirming the shot should feel deliberate, especially on touch screens where UI travel can otherwise corrupt the final shot.

Useful principles:
: Aiming should update only from intentional input in the battle area. Once the player releases the drag, the aim should stay locked until they fire, cancel or change weapon. The fire button should confirm the locked shot, not become a second aim target.

Risks to avoid:
: Do not copy layouts, exact controls, labels, art, assets or tuning. Do not allow hover movement to silently alter shot direction. Do not enable fire before a valid aim exists.

Our original implementation plan:
: Keep the existing explicit fire button, but require drag/touch input to create a valid `AimState`. Mouse movement without the left button held no longer updates aim. `ВОГОНЬ` is disabled until a valid aim exists.
