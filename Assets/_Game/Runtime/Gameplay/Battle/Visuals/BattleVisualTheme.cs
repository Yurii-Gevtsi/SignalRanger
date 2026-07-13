using UnityEngine;

namespace Game.Gameplay.Battle.Visuals
{
    public sealed class BattleVisualTheme
    {
        public BattleVisualTheme(
            BattleVisualLayer background,
            BattleVisualLayer foreground,
            string playerSkinResourcePath,
            string opponentSkinResourcePath,
            Vector2 unitSpriteScale,
            Vector2 unitSpriteOffset,
            bool hideProceduralTerrain,
            bool useProceduralTerrainColliders,
            int playerSkinFacingDirectionX = 1,
            int opponentSkinFacingDirectionX = 1,
            string playerRigResourcePath = null,
            string opponentRigResourcePath = null,
            string killZonePrefabResourcePath = null,
            float? killZoneDeathY = null)
        {
            Background = background;
            Foreground = foreground;
            PlayerSkinResourcePath = playerSkinResourcePath ?? string.Empty;
            OpponentSkinResourcePath = opponentSkinResourcePath ?? string.Empty;
            PlayerSkinFacingDirectionX = playerSkinFacingDirectionX >= 0 ? 1 : -1;
            OpponentSkinFacingDirectionX = opponentSkinFacingDirectionX >= 0 ? 1 : -1;
            UnitSpriteScale = unitSpriteScale;
            UnitSpriteOffset = unitSpriteOffset;
            HideProceduralTerrain = hideProceduralTerrain;
            UseProceduralTerrainColliders = useProceduralTerrainColliders;
            PlayerRigResourcePath = playerRigResourcePath ?? string.Empty;
            OpponentRigResourcePath = opponentRigResourcePath ?? string.Empty;
            KillZonePrefabResourcePath = killZonePrefabResourcePath ?? string.Empty;
            KillZoneDeathY = killZoneDeathY;
        }

        public BattleVisualLayer Background { get; }
        public BattleVisualLayer Foreground { get; }
        public string PlayerSkinResourcePath { get; }
        public string OpponentSkinResourcePath { get; }

        /// <summary>Optional animated rig prefab; wins over the flat skin sprite when set.</summary>
        public string PlayerRigResourcePath { get; }
        public string OpponentRigResourcePath { get; }
        public string KillZonePrefabResourcePath { get; }
        public float? KillZoneDeathY { get; }

        /// <summary>Which way the source art looks before any runtime flip.</summary>
        public int PlayerSkinFacingDirectionX { get; }
        public int OpponentSkinFacingDirectionX { get; }
        public Vector2 UnitSpriteScale { get; }
        public Vector2 UnitSpriteOffset { get; }
        public bool HideProceduralTerrain { get; }
        public bool UseProceduralTerrainColliders { get; }

        public bool HasPlayerSkin => !string.IsNullOrWhiteSpace(PlayerSkinResourcePath);
        public bool HasOpponentSkin => !string.IsNullOrWhiteSpace(OpponentSkinResourcePath);
        public bool HasPlayerRig => !string.IsNullOrWhiteSpace(PlayerRigResourcePath);
        public bool HasOpponentRig => !string.IsNullOrWhiteSpace(OpponentRigResourcePath);
        public bool HasKillZonePrefab => !string.IsNullOrWhiteSpace(KillZonePrefabResourcePath);
    }
}
