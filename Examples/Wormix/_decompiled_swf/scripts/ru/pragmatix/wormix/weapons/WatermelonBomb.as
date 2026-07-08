package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-w1O§;
   import flash.events.TimerEvent;
   import ru.pragmatix.wormix.weapons.interfaces.§_-O1c§;
   
   public class WatermelonBomb extends Grenade implements §_-O1c§
   {
      
      private static const §_-w1V§:Number = 0.7;
      
      public function WatermelonBomb()
      {
         super();
         MIN_POWER = 5;
         MAX_POWER = 35;
         §_-Y2U§ = new §_-w1O§(MIN_POWER,MAX_POWER,90);
      }
      
      override protected function shoot() : void
      {
         §_-BB§();
         var _loc1_:Class = getProperty("ammoClass") as Class;
         ammo = new _loc1_(owner,paramSet,paramSetEx,clock.value,shotParams.angle,§_-Yz§(shotParams.power),§_-w1V§);
         §_-Ws§();
         nextTurnOnStable();
      }
   }
}

