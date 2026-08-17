# Monetization and competitive policy

This is the implementation target for production purchases, rewarded ads and future competitive matchmaking. It intentionally keeps paid value in convenience and pacing rather than exclusive combat strength.

## Initial product set

Recommended first release products:

| Product | Suggested price | Contents |
|---|---:|---|
| Starter convenience pack | $1.99 | credits, blueprints and one cosmetic; no exclusive weapon |
| Progress pack | $4.99 | credits, blueprints and a limited-time 10% progression boost |
| Monthly pass | $4.99/month | ad-free claim flow, +10% PvE activity rewards, cosmetics, one extra loadout preset |
| Premium currency small | $0.99 | 100 crystals |
| Premium currency medium | $4.99 | 550 crystals |
| Premium currency large | $9.99 | 1,200 crystals |

Prices are hypotheses for store configuration, not revenue guarantees. Crystal bundles must not directly buy a PvP-only advantage.

## Receipt and restore contract

The client submits the platform receipt and product ID to a server-authoritative purchase endpoint. The server validates the receipt with the platform, records a unique transaction ID, grants the entitlement exactly once, and returns the current entitlement snapshot. Restore repeats the same lookup; it never blindly grants from local state. Refunds/revocations remove renewable entitlements and flag consumables for investigation.

Required fields: player ID, platform, product ID, transaction ID, receipt payload, purchase timestamp, validation status, entitlement state and server audit timestamp.

## Rewarded advertising policy

- Placement IDs are environment-configured, not hard-coded in gameplay logic.
- Production and test IDs must be separate.
- Start with a maximum of 5 rewarded completions per UTC day.
- A rewarded combat extension is PvE-only, expires after the battle and cannot apply to one-use super weapons.
- A post-battle multiplier is capped at 2× and can be claimed once per battle.
- Premium skips the ad interaction but does not multiply the reward beyond the same cap.

## Required store-console inputs before activation

Provide the Android application ID, production rewarded-ad unit ID, product IDs, base prices, subscription period, service-account/server credentials through a secure channel, and the desired regional tax/pricing configuration. Never put credentials in the repository or in the client binary.

