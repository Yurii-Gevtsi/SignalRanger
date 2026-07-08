package ru.pragmatix.flox.logger
{
   import §_-31N§.§_-42G§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.Bullet;
   import starling.events.Event;
   
   public class TracePublisher extends §_-Q1B§ implements §_-a1K§
   {
      
      public function TracePublisher()
      {
         super();
      }
      
      override public function toString() : String
      {
         return "ru.pragmatix.flox.logger.TracePublisher";
      }
      
      override public function publish(param1:§_-V2x§) : void
      {
         var _loc2_:Object = param1.argument;
         var _loc3_:String = "*" + param1.level.toString();
         if(param1.group != §_-V2x§.§_-02t§)
         {
            _loc3_ += "[" + param1.group + "]";
         }
         _loc3_ += "*";
         if(param1.loggerId)
         {
            _loc3_ += ":" + param1.loggerId;
         }
         _loc3_ += ":";
         _loc3_ += §_-81e§(_loc2_);
      }
   }
}

