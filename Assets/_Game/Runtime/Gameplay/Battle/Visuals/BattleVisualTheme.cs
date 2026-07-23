using System;
using System.Collections.Generic;
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
            string backgroundPrefabResourcePath = null,
            IReadOnlyList<BattleVisualLayer> additionalDestructibleLayers = null,
            IReadOnlyList<BattleVisualLayer> overlayLayers = null,
            float? killZoneY = null)
        {
            KillZoneY = killZoneY;
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
            BackgroundPrefabResourcePath = backgroundPrefabResourcePath ?? string.Empty;
            AdditionalDestructibleLayers = additionalDestructibleLayers ?? Array.Empty<BattleVisualLayer>();
            OverlayLayers = overlayLayers ?? Array.Empty<BattleVisualLayer>();
        }

        public BattleVisualLayer Background { get; }
        public BattleVisualLayer Foreground { get; }
        public string PlayerSkinResourcePath { get; }
        public string OpponentSkinResourcePath { get; }

        /// <summary>Optional animated rig prefab; wins over the flat skin sprite when set.</summary>
        public string PlayerRigResourcePath { get; }
        public string OpponentRigResourcePath { get; }
        public string BackgroundPrefabResourcePath { get; }
        public IReadOnlyList<BattleVisualLayer> AdditionalDestructibleLayers { get; }
        public IReadOnlyList<BattleVisualLayer> OverlayLayers { get; }

        /// <summary>
        /// World height of the lethal line matching the arena's kill-zone art
        /// (e.g. the laser barrier). When set it overrides the terrain-derived
        /// kill height, so dying lines up with what the player sees.
        /// </summary>
        public float? KillZoneY { get; }

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
        public bool HasBackgroundPrefab => !string.IsNullOrWhiteSpace(BackgroundPrefabResourcePath);
    }
}
