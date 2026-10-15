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

### Fragment quality contract

The canonical source package is deliberately small and has only these stages:

1. One approved original PNG with a true alpha channel. If it has no
   alpha-zero pixels, stop and report an invalid opaque source; never infer a
   black, white, magenta, or checkerboard background.
2. One same-canvas copy marked by the artist with red section boundaries.
3. One direct, ungenerated source crop per marked section. These crops retain
   a narrow shared edge band with adjacent sections so Unity never exposes a
   transparent seam.
4. One high-quality RGBA image-to-image enhancement per source section. A
   resize alone is never promotable.
5. One binary collision mask derived from each *new enhanced visual alpha*.
   Do not reuse a source-crop mask. The enhanced alpha may move an outer edge
   only within the two-pixel layout tolerance; trim only alpha beyond that
   limit. If the enhancement leaves a genuine hole in a declared shared crop
   overlap, restore only those missing overlap pixels from the direct crop
   before deriving the mask. This is a seam repair, not a replacement of the
   full enhanced contour. When chroma-key generation premultiplies its key
   colour into an edge, replace only that contaminated RGB with the matching
   source-edge colour; never preserve key-colour RGB in a final visual.
6. One reconstructed full-arena PNG with a true transparent background.
7. One alignment report that first validates each shared source-overlap band:
   after a two-production-pixel antialiasing allowance it accepts at most 64
   missing pixels, with no connected gap larger than 32 pixels. It also compares
   the complete reconstruction to the original. Image-to-image finish may alter
   a non-shared outer contour by at most 3.5% of the original opaque area; any
   larger drift, out-of-bounds growth, or seam defect blocks promotion. The
   report records all drift even when it remains within this bounded allowance.

Do not generate or retain dark, light, blue, black, or other colour-background
full-arena QA variants. They waste generation budget and are not runtime
assets. A separately supplied arena background remains an independent,
non-destructible Unity layer.

When an artist supplies an enhanced section manually, it is ingested directly
as `segments/<arena>_section_XX_visual.png`. Do not retain a second
numerically named input copy after normalizing it to the documented production
canvas; `source_segments` remains only for the required direct original crops.

### Approved arena visual style

Every image-to-image enhancement for a battle-arena fragment must use this
visual direction in addition to the map-specific prompt:

> Stylized hand-painted 2D sci-fi illustration for a game. Use large clean
> shapes, soft cel-shaded lighting, light coloured shadows without deep black,
> moderate contrast, and warm diffused light. Keep fine texture detail,
> scratches, and micro-cracks restrained. Prioritize clear silhouettes, large
> colour planes, and readable forms. Use a slightly muted but lively palette of
> pastel turquoise, green, sand, cream, and warm orange. Keep the background
> simpler, lighter, and lower-contrast than the foreground, with gentle
> atmospheric perspective. Lines should be clean without excessive sharpening,
> HDR treatment, or coloured halos. The result should feel hand-painted,
> expressive, and alive, but not childish or overly cartoonish. No
> photorealism and no excessive material micro-detail.

The style brief must not override the locked crop, silhouette, object position,
alpha boundary, production scale, or mask. It controls only the visual finish:
colour language, lighting, texture density, line treatment, and detail level.
The provider must not introduce deep-black background pixels, magenta/chroma
backgrounds, checkerboards, UI, text, or a new distant scene.

## Automation boundary

`Unity2DArtImporterAutomation` provides:

- an `AssetPostprocessor` for the managed roots above;
- a menu command to apply a profile only to selected managed textures;
- validation of importer settings, visual/mask pairing, dimensions, naming, and
  Android overrides.

It deliberately does not modify existing UI, broad-import all PNG files, alter
source art, or create Sprite Atlases. Ambiguous files are reported for manual
decisions.
