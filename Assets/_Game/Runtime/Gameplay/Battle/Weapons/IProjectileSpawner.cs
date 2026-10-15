namespace Game.Gameplay.Battle.Weapons
{
    /// <summary>
    /// Turns a <see cref="ProjectileSpawnRequest"/> into an actual scene projectile.
    /// Implemented by the scene assembly so weapon effects never touch GameObjects directly.
    /// </summary>
    public interface IProjectileSpawner
    {
        void Spawn(in ProjectileSpawnRequest request);
    }
}
