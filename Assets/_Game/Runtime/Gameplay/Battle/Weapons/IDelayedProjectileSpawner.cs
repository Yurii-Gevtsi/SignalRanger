namespace Game.Gameplay.Battle.Weapons
{
    /// <summary>Optional extension for effects that need to release a volley over time.</summary>
    public interface IDelayedProjectileSpawner
    {
        void Schedule(in ProjectileSpawnRequest request, float delaySeconds);
    }
}
