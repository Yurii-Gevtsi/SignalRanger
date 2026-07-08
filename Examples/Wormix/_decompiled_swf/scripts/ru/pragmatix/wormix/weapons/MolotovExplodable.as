package ru.pragmatix.wormix.weapons
{
   import ru.pragmatix.flox.social.utils.id.UserIdType;
   import ru.pragmatix.wormix.weapons.ammo.MolotovProjectile;
   import ru.pragmatix.wormix.weapons.interfaces.§_-y2Q§;
   
   public class MolotovExplodable extends Molotov implements §_-y2Q§
   {
      
      public function MolotovExplodable()
      {
         super();
      }
      
      override public function weaponCharge() : void
      {
         if(Boolean(ammo) && Boolean(!ammo.disposed) && !MolotovProjectile(ammo).exploding)
         {
            ammo.explode();
         }
      }
   }
}

