# Desert Sci-Fi fragment generation contract

Generate each reference crop as an individual PNG with a genuine RGBA alpha channel.

- Preserve the crop's composition, silhouette, placement, proportions and local lighting.
- Improve only material detail and edge clarity; do not add or remove terrain.
- The entire backdrop must be truly transparent (alpha = 0), not magenta, green,
  checkerboard, black, white, or another baked background.
- Do not add a floor, shadow, glow, particles, text, interface, characters or scenery.
- Keep all visible pixels inside the original crop bounds.

The pipeline rejects opaque or colour-keyed images. The immutable collision mask
remains the final geometry authority.
