# Reference Consultation: Phase 4.2 Result Overlay

Feature:
: Battle result UI for win/loss/draw, retry, continue and temporary reward preview.

Example signals:
: The reference packages separate battle UI, result feedback, rewards and progression into visible product layers. The useful pattern is not the exact layout or data, but the idea that the player should get a clear end-of-battle state, an immediate retry path, and a forward path after success.

Useful principles:
: Result UI should interrupt gameplay input, explain the outcome in one glance, and expose only the next useful actions. Reward preview should be separate from reward application so save/economy can be added later without coupling battle UI to persistence.

Risks to avoid:
: Do not copy result layouts, texts, economy values, reward tables, asset naming, visual treatment or proprietary flow. Do not leave battle completion as only a debug/status text. Do not let aim input continue behind result UI.

Our original implementation plan:
: Add a small result view-model and factory in gameplay, extend the HUD contract with continue, render a runtime modal overlay in the battle HUD, and keep reward application deferred until Phase 6/9.
