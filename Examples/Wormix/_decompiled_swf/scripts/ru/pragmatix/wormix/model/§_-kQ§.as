package ru.pragmatix.wormix.model
{
   import §_-73I§.§_-K2s§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-w2i§.§_-Zd§;
   import dragonBones.animation.WorldClock;
   import flash.display.Shape;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import starling.display.Image;
   import starling.events.Event;
   import starling.utils.ScaleMode;
   
   public class §_-kQ§
   {
      
      public var §_-z1X§:int;
      
      public var §_-I2r§:Vector.<int> = new Vector.<int>();
      
      public function §_-kQ§(param1:int)
      {
         super();
         this.§_-z1X§ = param1;
         this.§_-I2r§.push(param1);
      }
      
      public function toString() : String
      {
         var _loc2_:int = 0;
         var _loc1_:String = this.§_-z1X§ + ":";
         for each(_loc2_ in this.§_-I2r§)
         {
            _loc1_ += " " + _loc2_.toString();
         }
         return _loc1_;
      }
   }
}

