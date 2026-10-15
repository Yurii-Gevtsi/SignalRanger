package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-pT§;
   import §_-Y1O§.§_-y1o§;
   import §_-l1K§.§_-W2f§;
   import §_-r1C§.§_-h2B§;
   import config.§_-vR§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.server.VerifyPaymentResult;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.weapons.ammo.phase.ChainPhase;
   
   public class ChainGun extends Weapon
   {
      
      private static const POWER:uint = 30;
      
      public function ChainGun()
      {
         super();
         MIN_POWER = MAX_POWER = POWER;
         §_-z1W§(new §_-W2f§());
         §_-Y2U§ = new §_-pT§(80,MIN_POWER);
      }
      
      override protected function shoot() : void
      {
         §_-h2B§.play(§_-d27§.§_-Z1d§);
         new ChainPhase(owner,owner.x,owner.y,null,paramSet.damage.value,POWER).setSpeedVector(shotParams.angle,POWER);
         detach();
         nextTurnOnStable();
      }
   }
}

