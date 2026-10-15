# Battle camera frame

Every arena theme defines a fixed world-space camera frame. The frame is the
large background behind the playable terrain and is the single source of truth
for three things:

- Camera panning clamps to the frame edges, not to terrain sprite bounds.
- The maximum orthographic size is the smallest size that contains the whole
  frame at the active viewport aspect ratio.
- The kill zone is rendered and simulated at one shared inset above the frame
  bottom edge.

Terrain may be smaller than the frame and must not expand camera bounds. Static
background sprites and animated background prefabs are sized once to the frame
in world space, so a pan cannot expose the camera clear colour.
