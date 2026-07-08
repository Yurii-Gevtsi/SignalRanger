package ru.pragmatix.wormix.weapons
{
   import §_-l1K§.§_-d2j§;
   import ru.pragmatix.wormix.weapons.ammo.FrozenOrbAmmo;
   
   public class FrozenOrbWeapon extends Weapon
   {
      
      public function FrozenOrbWeapon()
      {
         super();
         MIN_POWER = 15;
         MAX_POWER = 30;
         §_-z1W§(new §_-d2j§());
      }
      
      override protected function shoot() : void
      {
         new FrozenOrbAmmo(owner,paramSet,shotParams.angle,§_-Yz§(shotParams.power),paramSetEx);
         detach();
         nextTurnOnStable();
      }
   }
}

