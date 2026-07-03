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
            bool useProceduralTerrainColliders)
        {
            Background = background;
            Foreground = foreground;
            PlayerSkinResourcePath = playerSkinResourcePath ?? string.Empty;
            OpponentSkinResourcePath = opponentSkinResourcePath ?? string.Empty;
            UnitSpriteScale = unitSpriteScale;
            UnitSpriteOffset = unitSpriteOffset;
            HideProceduralTerrain = hideProceduralTerrain;
            UseProceduralTerrainColliders = useProceduralTerrainColliders;
        }

        public BattleVisualLayer Background { get; }
        public BattleVisualLayer Foreground { get; }
        public string PlayerSkinResourcePath { get; }
        public string OpponentSkinResourcePath { get; }
        public Vector2 UnitSpriteScale { get; }
        public Vector2 UnitSpriteOffset { get; }
        public bool HideProceduralTerrain { get; }
        public bool UseProceduralTerrainColliders { get; }

        public bool HasPlayerSkin => !string.IsNullOrWhiteSpace(PlayerSkinResourcePath);
        public bool HasOpponentSkin => !string.IsNullOrWhiteSpace(OpponentSkinResourcePath);
    }
}
