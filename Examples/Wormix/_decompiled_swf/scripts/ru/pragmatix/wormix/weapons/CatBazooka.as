package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-x2C§;
   import §_-A1K§.§_-TA§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.weapons.ammo.AirCatAmmo;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   
   public class CatBazooka extends Bazooka
   {
      
      public function CatBazooka()
      {
         super();
         §_-Y2U§ = new §_-x2C§(105,false,0.5);
      }
      
      override protected function shoot() : void
      {
         §_-h2B§.play(§_-d27§.§_-h1g§);
         var _loc1_:Class = getProperty("ammoClassName") as Class;
         var _loc2_:int = owner.x + Math.cos(shotParams.angle) * 25;
         var _loc3_:int = owner.y + Math.sin(shotParams.angle) * 25;
         var _loc4_:AirCatAmmo = new _loc1_(owner,paramSet,_loc2_,_loc3_,0,0,DamageType.PHYSICAL,§_-TA§(getProperty("maxBounce")).value,§_-TA§(getProperty("damageForBounce")).value);
         if(getProperty("autoTargeting"))
         {
            _loc4_.setAutoTargeting();
         }
         _loc4_.setSpeedVector(shotParams.angle,§_-Yz§(shotParams.power));
         detach();
         nextTurnOnStable();
      }
   }
}

