package ru.pragmatix.wormix.weapons
{
   import §_-A1K§.§_-7u§;
   import §_-L1H§.§_-C3e§;
   import §_-L1H§.§_-b2K§;
   import §_-P2i§.§_-b7§;
   import §_-o14§.*;
   import §_-u1m§.§_-61A§;
   import com.distriqt.extension.inappbilling.events.InAppBillingEvent;
   import feathers.layout.VerticalAlign;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.weapons.ammo.AirMissile;
   import ru.pragmatix.wormix.weapons.interfaces.§_-jm§;
   import starling.utils.ScaleMode;
   
   public class AirStrike extends §_-61A§ implements §_-jm§
   {
      
      public function AirStrike()
      {
         super();
      }
      
      override public function activate() : void
      {
         super.activate();
         §_-b7§.§_-G3i§(owner);
      }
      
      override protected function §_-B1T§(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:Number, param5:Number, param6:Number) : void
      {
         var _loc7_:Boolean = false;
         _loc7_ = §_-7u§(getProperty("isHoming")).value;
         var _loc8_:§_-b2K§ = _loc7_ ? shotParams : null;
         var _loc9_:int = -2;
         while(_loc9_ <= 2)
         {
            new AirMissile(param1,param2,param3 + 50 * _loc9_,param4,param5,param6,_loc8_);
            _loc9_++;
         }
         super.§_-B1T§(param1,param2,param3,param4,param5,param6);
      }
   }
}

