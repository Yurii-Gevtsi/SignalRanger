# Grounder health interception hardening

The current 45-HP prototype survived the first 24-damage hostile physical impact and was removed after the second, with normal explosions both times (`receipt:76ddd1c5-c145-4d61-bffc-96aced7c1bc6`). Extracted the health literal into `GrounderBaseHealth` and guarded `DamageGrounder` against a stale device reference, so removal stays single-shot if two contacts are processed close together. The unchanged public scenario passed after the edit (`receipt:dd7d8879-4373-497d-a302-b91ff4bd27e7`). No assertion was weakened.
