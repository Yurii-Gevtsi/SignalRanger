package ru.pragmatix.wormix.weapons
{
   import §_-63U§.§_-21K§;
   import §_-73d§.§_-w1O§;
   import §_-l1K§.§_-d2j§;
   
   public class StickyGrenade extends Weapon
   {
      
      public function StickyGrenade()
      {
         super();
         MIN_POWER = 15;
         MAX_POWER = 45;
         §_-z1W§(new §_-d2j§());
         §_-Y2U§ = new §_-w1O§(MIN_POWER,MAX_POWER,65,false);
      }
      
      override protected function shoot() : void
      {
         var _loc1_:Class = getProperty("classProjectile") as Class;
         new _loc1_(owner,paramSet,paramSetEx,getProperty("blinkClassName"),getProperty("quantityShards"),getProperty("classShard"),shotParams.angle,§_-Yz§(shotParams.power));
         detach();
         nextTurnOnStable();
      }
   }
}

