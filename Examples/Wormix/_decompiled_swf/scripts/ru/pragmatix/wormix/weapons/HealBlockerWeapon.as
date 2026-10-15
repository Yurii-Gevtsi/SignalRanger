package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-g1J§;
   import §_-A1K§.§_-TA§;
   import §_-l1K§.§_-d2j§;
   
   public class HealBlockerWeapon extends Weapon
   {
      
      public function HealBlockerWeapon()
      {
         super();
         MIN_POWER = 5;
         MAX_POWER = 35;
         §_-Y2U§ = new §_-g1J§(MIN_POWER,MAX_POWER,80);
         §_-z1W§(new §_-d2j§());
      }
      
      override protected function shoot() : void
      {
         var _loc1_:§_-TA§ = §_-TA§(getProperty("duration"));
         var _loc2_:Class = Class(getProperty("ammoClass"));
         new _loc2_(owner,paramSet,shotParams.angle,§_-Yz§(shotParams.power),_loc1_);
         detach();
         nextTurnOnStable();
      }
   }
}

