package ru.pragmatix.wormix.weapons
{
   import §_-A1K§.§_-TA§;
   import §_-D3A§.§_-TZ§;
   import §_-L1H§.§_-C3e§;
   import §_-l1g§.§_-L3Q§;
   import §_-u1m§.§_-61A§;
   import com.distriqt.extension.application.Application;
   import com.distriqt.extension.application.display.Display;
   import com.distriqt.extension.application.display.DisplayCutout;
   import flash.geom.Rectangle;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.weapons.ammo.AirFireMissile;
   import ru.pragmatix.wormix.weapons.interfaces.§_-jm§;
   import starling.events.Event;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   public class NapalmStrike extends §_-61A§ implements §_-jm§
   {
      
      private static const §_-7§:uint = 5;
      
      private static const §_-a2Q§:uint = 50;
      
      private var missiles:§_-TA§ = new §_-TA§(0);
      
      public function NapalmStrike()
      {
         super();
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         this.missiles.value = param1["missiles"] ? int(param1["missiles"].value) : int(§_-7§);
         super.§_-z2Y§(param1);
      }
      
      override protected function §_-B1T§(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:Number, param5:Number, param6:Number) : void
      {
         super.§_-B1T§(param1,param2,param3,param4,param5,param6);
         param3 -= int((this.missiles.value - 1) * §_-a2Q§ / 2);
         var _loc7_:int = 0;
         while(_loc7_ < this.missiles.value)
         {
            new AirFireMissile(param1,param2,param3 + §_-a2Q§ * _loc7_,param4,param5,param6);
            _loc7_++;
         }
      }
   }
}

