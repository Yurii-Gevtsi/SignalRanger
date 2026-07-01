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

        public string ArenaId => _arenaId;
        public string DisplayName => _displayName;
        public Vector3 CameraPosition => _cameraPosition;
        public float CameraSize => _cameraSize;
        public Vector2 PlayerSpawn => _playerSpawn;
        public Vector2 OpponentSpawn => _opponentSpawn;
        public IReadOnlyList<ArenaTerrainBlock> TerrainBlocks => _terrainBlocks;

        public static ArenaDefinition CreateRuntime(
            string arenaId,
            string displayName,
            Vector3 cameraPosition,
            float cameraSize,
            Vector2 playerSpawn,
            Vector2 opponentSpawn,
            ArenaTerrainBlock[] terrainBlocks)
        {
            var definition = CreateInstance<ArenaDefinition>();
            definition._arenaId = arenaId;
            definition._displayName = displayName;
            definition._cameraPosition = cameraPosition;
            definition._cameraSize = cameraSize;
            definition._playerSpawn = playerSpawn;
            definition._opponentSpawn = opponentSpawn;
            definition._terrainBlocks = terrainBlocks;
            return definition;
        }
    }
}
