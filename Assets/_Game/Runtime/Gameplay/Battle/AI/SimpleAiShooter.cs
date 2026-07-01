using Game.Gameplay.Battle.Domain;
using UnityEngine;

namespace Game.Gameplay.Battle.AI
{
    public sealed class SimpleAiShooter
    {
        public AiShotPlan CreateShot(Vector2 origin, Vector2 target, AimSettings aimSettings)
        {
            return CreateShot(origin, target, aimSettings, AiDifficulty.Normal, 0);
        }

        public AiShotPlan CreateShot(Vector2 origin, Vector2 target, AimSettings aimSettings, AiDifficulty difficulty, int shotIndex)
        {
            var delta = target - origin;
            var distance = delta.magnitude;
            var horizontal = Mathf.Abs(delta.x);
            var lift = Mathf.Clamp(horizontal * 0.18f + 0.65f, 0.65f, 2.1f);
            var direction = new Vector2(delta.x, delta.y + lift).normalized;
            var normalizedDistance = Mathf.InverseLerp(1.5f, 9f, distance);
            var power = Mathf.Lerp(aimSettings.MinPower, aimSettings.MaxPower * 0.82f, normalizedDistance);
            ApplyDifficulty(difficulty, shotIndex, ref direction, ref power);
            return new AiShotPlan(direction, Mathf.Clamp(power, aimSettings.MinPower, aimSettings.MaxPower));
        }

        private static void ApplyDifficulty(AiDifficulty difficulty, int shotIndex, ref Vector2 direction, ref float power)
        {
            var normalizedWave = Mathf.Sin((shotIndex + 1) * 1.618f);
            var angleError = difficulty switch
            {
                AiDifficulty.Easy => 10f * normalizedWave,
                AiDifficulty.Normal => 4f * normalizedWave,
                AiDifficulty.Hard => 1.5f * normalizedWave,
                _ => 4f * normalizedWave
            };
            var powerError = difficulty switch
            {
                AiDifficulty.Easy => 0.12f * normalizedWave,
                AiDifficulty.Normal => 0.055f * normalizedWave,
                AiDifficulty.Hard => 0.02f * normalizedWave,
                _ => 0.055f * normalizedWave
            };

            direction = Quaternion.Euler(0f, 0f, angleError) * direction;
            power *= 1f + powerError;
        }
    }
}
