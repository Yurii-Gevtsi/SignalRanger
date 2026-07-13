using UnityEngine;

namespace Game.Gameplay.Battle.AI
{
    /// <summary>
    /// Turns a bot's difficulty into aim error by scattering the *target point*
    /// (not the launch angle) once per turn. A shot at a nudged point still
    /// looks like an honest miss — the shell lands near the target rather than
    /// flying off at a strange angle. The scatter radius grows with distance and
    /// with the profile's error, and a small share of shots are perfectly aimed.
    /// </summary>
    public static class TargetScatter
    {
        public static Vector2 Apply(Vector2 target, Vector2 origin, BotProfile profile, IBotRandom random)
        {
            if (profile == null || random == null)
            {
                return target;
            }

            if (random.NextFloat() < profile.PerfectShotChance)
            {
                return target;
            }

            var radius = profile.ScatterBase + profile.ScatterPerDistance * Vector2.Distance(origin, target);
            if (radius <= 0f)
            {
                return target;
            }

            // Uniform point inside the disc: sqrt keeps the distribution even.
            var angle = random.NextFloat() * Mathf.PI * 2f;
            var distance = radius * Mathf.Sqrt(random.NextFloat());
            var offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
            return target + offset;
        }
    }
}
