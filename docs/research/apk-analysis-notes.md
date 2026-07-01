# APK/XAPK Analysis Notes

These notes summarize only high-level technical observations from the unpacked examples in `Examples/`. They must not be used to copy code, assets, configs, balance, layouts, names, scenes, or proprietary gameplay logic.

## Current Project Context

- Unity project detected.
- Unity editor version: `6000.5.1f1`.
- Installed packages include URP, Input System, UGUI, Visual Scripting, Test Framework, AI packages, Timeline, and common Unity modules.

## Example A: XAPK-Style Split Package

Observed from `Examples/Warlings`:

- Package is represented as a base APK plus many split APKs for languages and screen densities.
- Manifest metadata indicates a modern Android target and billing/ad-related permissions.
- File inventory suggests a 2D mobile game package with many bitmap assets, audio files, video files, Android resources, and Google mobile service libraries.
- No obvious Unity, Unreal, or Cocos marker was visible from the base APK file listing checked during this pass.
- Broad SDK categories observed: ads, billing, Google Play services, AndroidX, app measurement/identifier support.
- Broad feature categories suggested by asset folder names: gameplay HUD, shop, rewards, weapons, maps, unit customization, menu screens, win/result screen, environment effects.

High-level takeaway for our game:

- Keep Android SDK dependencies isolated behind our own service interfaces.
- Plan content categories separately from code modules.
- Do not start with the full scope of shops, rewards, customization, and many weapons; introduce them gradually.

## Example B: Unpacked Adobe AIR/Flash-Style Package

Observed from `Examples/Wormix`:

- The package has AIR/Flash indicators, including SWF content and AIR extension folders.
- Native libraries are present under `lib/arm64-v8a`.
- Multiple DEX files and many property/version files suggest a broad Android SDK integration layer.
- Broad SDK categories observed: Firebase, Google Play services, billing, identity/auth, app update/review, notifications, push notifications, analytics/measurement, remote config/performance-related components.
- Asset categories include layouts, maps, particles, sounds, UI, localization, sprites, and authored animation/content files.
- Both mobile and desktop layout categories appear in the package, which suggests shared content across device profiles.

High-level takeaway for our game:

- A live game can accumulate many services, but the MVP should not depend on all of them.
- Start offline-first with local saves and a small economy.
- Add analytics, remote config, ads, IAP, and notifications as adapters after core gameplay is stable.

## Safe Inspiration

Safe ideas to carry forward:

- turn-based tactical combat;
- short session structure;
- simple weapon selection;
- readable mobile HUD;
- tutorialized first battle;
- level or chapter progression;
- optional rewards and shop layers;
- clear separation between battle scene and meta screens;
- SDK facade pattern for ads, analytics, IAP, notifications, and remote config.

## Explicit Non-Goals

- No asset reuse.
- No balance reuse.
- No reverse-engineered logic.
- No cloned UI screens.
- No recreated file structure from examples.
- No copied names for classes, weapons, items, maps, modes, configs, or events.

