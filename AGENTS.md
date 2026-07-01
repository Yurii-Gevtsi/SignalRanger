# Codex Project Rules

This project is an original Unity mobile game built from scratch.

## Primary Goal

Design and implement a simpler, more approachable mobile tactical artillery game inspired only at a high level by older successful examples. The game should be easy for a new player to enter, while leaving room for long-term progression, content growth, live tuning, events, and retention systems.

## Strict IP Boundaries

Do not copy from APK/XAPK examples:

- source code or decompiled code;
- class names, method names, package names, or proprietary identifiers;
- assets, sprites, images, audio, video, fonts, text, layouts, configs, or balance data;
- scene structure, file structure, economy numbers, weapon stats, maps, UI layouts, or proprietary gameplay logic.

APK/XAPK analysis is allowed only as high-level technical research. Use it to understand broad architecture choices, common mobile SDK categories, packaging patterns, asset categories, and possible game systems. Never port implementation details from the examples.

## Allowed Reference Use

The unpacked examples in `Examples/` may be inspected only for high-level observations such as:

- engine or runtime indicators;
- Android permissions;
- SDK categories such as ads, analytics, billing, notifications, remote config, or backend clients;
- broad asset categories;
- broad feature categories such as shop, rewards, inventory, tutorial, level progression, settings, or multiplayer.

When documenting research, summarize patterns in original wording. Avoid reproducing proprietary file lists, config values, internal names, or extracted content.

## Architecture Principles

- Keep gameplay, UI, data, and SDK integrations separated.
- Wrap third-party SDKs behind project-owned service interfaces.
- Keep core gameplay testable without Android, Firebase, ads, billing, or other external SDKs.
- Prefer Unity 2D with URP, the Input System, Addressables later when needed, and ScriptableObject-based authored content for early development.
- Keep the first version deliberately small: fast onboarding, few core weapons, clear UI, short matches, and minimal meta systems.
- Add live/game-economy systems in layers after the core loop is fun.

## Collaboration Rules

- Read this file and relevant files in `docs/` before large design or implementation changes.
- Propose architecture before broad changes to core systems, data models, SDK integrations, or folder structure.
- Document important architecture decisions in `docs/architecture/`.
- Do not modify or delete files in `Examples/` unless explicitly asked.
- Treat `Examples/` as research material, not source material.
- Prefer small, modular changes with focused verification.

