package ru.pragmatix.wormix.weapons
{
   import §_-62§.§_-O2H§;
   import §_-L1H§.§_-b2K§;
   import §_-l1K§.§_-cZ§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.weapons.ammo.DynamiteStick;
   import ru.pragmatix.wormix.weapons.ammo.type.dynamite.DynamiteBunch;
   import ru.pragmatix.wormix.weapons.interfaces.§_-b29§;
   
   public class Dynamite extends Weapon implements §_-b29§
   {
      
      public function Dynamite()
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
         var _loc1_:Class = getProperty("classDynamite") as Class;
         var _loc2_:DynamiteStick = new _loc1_(owner,owner.x,owner.y,paramSet);
         if(_loc2_ is DynamiteBunch)
         {
            DynamiteBunch(_loc2_).setParamSetEx(paramSetEx);
         }
         detach();
         nextTurnOnStable();
      }
   }
}

