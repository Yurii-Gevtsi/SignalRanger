namespace Game.Gameplay.Battle.Weapons
{
    /// <summary>
    /// Behaviour that runs when a weapon is fired. One effect per <see cref="WeaponRole"/> family.
    /// Effects translate a <see cref="WeaponFireContext"/> into spawn requests and, later,
    /// other tactical outcomes (cover placement, repositioning, etc.).
    /// </summary>
    public interface IWeaponEffect
    {
        void Fire(in WeaponFireContext context);
    }
}
