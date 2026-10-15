package ru.pragmatix.wormix.weapons
{
   import §_-l1K§.§_-W2f§;
   import ru.pragmatix.wormix.weapons.ammo.RicochetingDisc;
   
   public class DiscLauncher extends Weapon
   {
      
      public function DiscLauncher()
      {
         super();
         MAX_POWER = MIN_POWER = 50;
         §_-z1W§(new §_-W2f§());
      }
      
      override protected function shoot() : void
      {
         new RicochetingDisc(owner,paramSet,shotParams.angle,MAX_POWER);
         detach();
         nextTurnOnStable();
      }
   }
}

