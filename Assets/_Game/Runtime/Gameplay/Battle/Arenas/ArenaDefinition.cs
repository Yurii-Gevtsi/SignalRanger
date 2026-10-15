using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.Battle.Arenas
{
    [CreateAssetMenu(menuName = "Game/Battle/Arena Definition", fileName = "ArenaDefinition")]
    public sealed class ArenaDefinition : ScriptableObject
    {
        [SerializeField] private string _arenaId;
        [SerializeField] private string _displayName;
        [SerializeField] private Vector3 _cameraPosition = new(0f, 1.2f, -10f);
        [SerializeField] private float _cameraSize = 5.6f;
        [SerializeField] private Vector2 _playerSpawn;
        [SerializeField] private Vector2 _opponentSpawn;
        [SerializeField] private ArenaTerrainBlock[] _terrainBlocks;
        [SerializeField] private ArenaSpriteSurface[] _spriteSurfaces;
        [SerializeField] private bool _limitZoomOutToSpriteBounds;
        [SerializeField, Range(0f, 1f)] private float _spriteArenaVerticalAnchor;
        [SerializeField] private float _spriteArenaVerticalScreenOffset;
        [SerializeField] private bool _lockCameraAtZoomOutLimit;

        public string ArenaId => _arenaId;
        public string DisplayName => _displayName;
        public Vector3 CameraPosition => _cameraPosition;
        public float CameraSize => _cameraSize;
        public Vector2 PlayerSpawn => _playerSpawn;
        public Vector2 OpponentSpawn => _opponentSpawn;
        public IReadOnlyList<ArenaTerrainBlock> TerrainBlocks => _terrainBlocks ?? System.Array.Empty<ArenaTerrainBlock>();
        public IReadOnlyList<ArenaSpriteSurface> SpriteSurfaces => _spriteSurfaces ?? System.Array.Empty<ArenaSpriteSurface>();
        public bool LimitZoomOutToSpriteBounds => _limitZoomOutToSpriteBounds;
        public float SpriteArenaVerticalAnchor => _spriteArenaVerticalAnchor;
        public float SpriteArenaVerticalScreenOffset => _spriteArenaVerticalScreenOffset;
        public bool LockCameraAtZoomOutLimit => _lockCameraAtZoomOutLimit;

        public static ArenaDefinition CreateRuntime(
            string arenaId,
            string displayName,
            Vector3 cameraPosition,
            float cameraSize,
            Vector2 playerSpawn,
            Vector2 opponentSpawn,
            ArenaTerrainBlock[] terrainBlocks,
            ArenaSpriteSurface[] spriteSurfaces = null,
            bool limitZoomOutToSpriteBounds = false,
            float spriteArenaVerticalAnchor = 0f,
            float spriteArenaVerticalScreenOffset = 0f,
            bool lockCameraAtZoomOutLimit = false)
        {
            var definition = CreateInstance<ArenaDefinition>();
            definition._arenaId = arenaId;
            definition._displayName = displayName;
            definition._cameraPosition = cameraPosition;
            definition._cameraSize = cameraSize;
            definition._playerSpawn = playerSpawn;
            definition._opponentSpawn = opponentSpawn;
            definition._terrainBlocks = terrainBlocks ?? System.Array.Empty<ArenaTerrainBlock>();
            definition._spriteSurfaces = spriteSurfaces ?? System.Array.Empty<ArenaSpriteSurface>();
            definition._limitZoomOutToSpriteBounds = limitZoomOutToSpriteBounds;
            definition._spriteArenaVerticalAnchor = Mathf.Clamp01(spriteArenaVerticalAnchor);
            definition._spriteArenaVerticalScreenOffset = spriteArenaVerticalScreenOffset;
            definition._lockCameraAtZoomOutLimit = lockCameraAtZoomOutLimit;
            return definition;
        }
    }
}
