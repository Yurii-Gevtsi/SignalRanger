package ru.pragmatix.wormix.weapons
{
   import §_-82l§.§_-A2Y§;
   import §_-L1H§.§_-b2K§;
   import §_-l1K§.§_-cZ§;
   import §_-l1K§.§_-d2j§;
   import §_-m0§.§_-X1F§;
   import §_-m2s§.§_-R11§;
   import §_-o14§.*;
   import §_-r1C§.§_-h2B§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.§_-g20§;
   import ru.pragmatix.wormix.weapons.interfaces.§_-b29§;
   import starling.events.Event;
   
   public class SpiderWeapon extends Weapon implements §_-b29§
   {
      
      public function SpiderWeapon()
      {
         super();
         §_-z1W§(new §_-cZ§());
      }
      
      public function jumpShoot(param1:Boolean) : void
      {
         var _loc2_:§_-b2K§ = null;
         if(!§_-X1j§.§_-12n§.gameMaster.§_-Rk§())
         {
            _loc2_ = new §_-b2K§();
            shootWithParams(_loc2_);
         }
      }
      
      override protected function shoot() : void
      {
         var _loc1_:Class = getProperty("classMine") as Class;
         new _loc1_(owner,paramSet);
         detach();
         nextTurnOnStable();
      }
   }
}

