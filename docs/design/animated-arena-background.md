# Animated arena background

## Goal

Arena skies are packaged as one reusable prefab instead of rebuilding visual layers for every map.

## Sky1 template

`AnimatedSkyBackground.prefab` is a single arena-level object at `Y = 1` with a root scale of `1.5 × 1.2`. Its runtime component creates:

- one static `sky4` desert-and-sky layer;
- four looping cloud instances derived from two cloud textures, with different sizes, heights, directions and speeds;
- one small planet following an elliptical orbit around the large planet in the static layer;
- one station with a slow vertical idle motion.

The component owns only decorative renderers and does not participate in terrain collision, aiming, simulation or camera bounds. The arena visual theme selects the prefab by Resources path, so another arena can reuse the same template through one configuration value.

## Extension

Future sky sets should use a sibling Resources folder and a prefab with the same responsibility boundary. Arena-specific foreground terrain remains a separate destructible visual layer.
