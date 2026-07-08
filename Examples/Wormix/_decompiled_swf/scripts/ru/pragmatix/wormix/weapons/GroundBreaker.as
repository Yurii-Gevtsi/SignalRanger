package ru.pragmatix.wormix.weapons
{
   import §_-51M§.Dragon;
   import §_-73d§.§_-x2C§;
   import §_-Y1O§.*;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.weapons.ammo.GroundBreakerMissile;
   
   public class GroundBreaker extends Bazooka
   {
      
      public function GroundBreaker()
      {
         super();
         §_-Y2U§ = new §_-x2C§(100,true,0.3);
      }
      
      override protected function shoot() : void
      {
         new GroundBreakerMissile(owner,paramSet,shotParams.angle,§_-Yz§(shotParams.power));
         detach();
         nextTurnOnStable();
      }
   }
}

