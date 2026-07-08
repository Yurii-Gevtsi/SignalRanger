package ru.pragmatix.wormix.weapons.ammo.ammoFactory
{
   import §_-L1H§.§_-C3e§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.objects.§_-01J§;
   
   public interface IPooledAmmo
   {
      
      function initAmmo(param1:int, param2:§_-01J§, param3:Point, param4:§_-C3e§, param5:Number, param6:uint, param7:Number = 2, param8:Boolean = false, param9:Boolean = false, param10:Boolean = true) : void;
      
      function initAmmoByBulletName(param1:String, param2:§_-01J§, param3:Point, param4:§_-C3e§, param5:Number, param6:uint, param7:Number = 2, param8:Boolean = false, param9:Boolean = false, param10:Boolean = true) : void;
      
      function addTrail(param1:uint = 16762931, param2:uint = 16777215, param3:uint = 6, param4:uint = 2, param5:Number = 0.996) : void;
      
      function setDefault() : void;
      
      function remove() : void;
      
      function dispose() : void;
   }
}

