# Unity 2D Art Pipeline

This document defines the project-owned raster art profiles and the boundary
for importer automation. It complements the arena integration instructions and
does not rewrite existing art metadata in bulk.

## Existing UI convention

The current UI under `Assets/_Game/Content/Resources/UI/` is intentionally
imported as `Texture2D` (`TextureImporterType.Default`). Runtime screen
controllers load those textures from `Resources` and create sprites with
`Sprite.Create(..., 100f, ...)`; sliced panels provide their border at runtime.

Because of that convention, existing UI assets must not be mass-converted to
Sprite importer mode. Their current metadata is mixed in small details such as
readability and wrap mode, so new automation applies only to the explicit
`UI/Generated` folder and leaves the established UI untouched.

## Managed import roots

The editor automation owns only these paths and filename profiles:

| Root or suffix | Profile | Purpose |
| --- | --- | --- |
| `Content/Resources/UI/Generated/` | `GeneratedUiTexture` | New UI textures consumed as runtime `Texture2D` |
| `Content/Art/Generated/` | `GeneratedSprite` | New ordinary gameplay sprites |
| `Art/Characters/Generated/` | `GeneratedSprite` | New generated character/frame sprites |
| `Content/Art/Arenas/**/_visual.png` | `ArenaVisual` | Renderable arena fragment |
| `Content/Art/Arenas/**/_mask.png` | `ArenaMask` | CPU-readable collision mask |

Existing files outside these roots are validation-only or unmanaged. In
particular, `Background.png`, current character builders, and existing UI
assets are not changed by the postprocessor.

## Profiles

### GeneratedUiTexture

Keep `TextureImporterType.Default`, sRGB on, alpha transparency on, mipmaps
off, Bilinear filtering, Clamp wrapping, no readback, max size 2048, and
platform compression suitable for mobile. Runtime code remains responsible for
creating the sprite and applying sliced borders.

### GeneratedSprite

Use Sprite/Single/Full Rect, centered pivot, PPU 100, sRGB on, alpha
transparency on, mipmaps off, Bilinear filtering, Clamp wrapping, and max size
2048. Keep the generated source separate from any checkerboard or cleanup
input.

### ArenaVisual

Use Sprite/Single/Full Rect, centered pivot, PPU 256, sRGB on, mipmaps off,
Bilinear filtering, Clamp wrapping, max size 4096, and Android ASTC 4x4 with
Crunch disabled. Mutable destructible surfaces must not be packed into a Sprite
Atlas.

### ArenaMask

Keep the mask CPU-readable, linear, lossless/uncompressed, mipmaps off, Point
filtered, and Clamp wrapped. It must have the same dimensions and stem pair as
the visual texture. The mask, not decorative visual alpha, is the collision
authority.

## Automation boundary

`Unity2DArtImporterAutomation` provides:

- an `AssetPostprocessor` for the managed roots above;
- a menu command to apply a profile only to selected managed textures;
- validation of importer settings, visual/mask pairing, dimensions, naming, and
  Android overrides.

It deliberately does not modify existing UI, broad-import all PNG files, alter
source art, or create Sprite Atlases. Ambiguous files are reported for manual
decisions.
