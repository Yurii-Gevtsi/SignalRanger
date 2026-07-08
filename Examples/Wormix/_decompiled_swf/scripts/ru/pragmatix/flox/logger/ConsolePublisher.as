package ru.pragmatix.flox.logger
{
   import §_-L1H§.§_-b2K§;
   import §_-zI§.§_-A1s§;
   import flash.events.StatusEvent;
   import flash.geom.Point;
   import flash.net.LocalConnection;
   import ru.pragmatix.wormix.weapons.Weapon;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class ConsolePublisher extends §_-Q1B§
   {
      
      public static const §_-uM§:String = "_pragmatix_log_console";
      
      private static var §_-928§:Boolean = true;
      
      private var §_-c1P§:Number = 1;
      
      private var lc:LocalConnection = null;
      
      public function ConsolePublisher()
      {
         super();
         this.lc = new LocalConnection();
         this.lc.addEventListener(StatusEvent.STATUS,this.§_-U2e§);
      }
      
      override public function publish(param1:§_-V2x§) : void
      {
         var _loc2_:Object = null;
         if(this.lc != null && §_-928§)
         {
            _loc2_ = §_-V2x§.serialize(param1);
            _loc2_.argument = _loc2_.argument.toString();
            this.lc.send(§_-uM§,"log",_loc2_);
         }
      }
      
      private function §_-U2e§(param1:StatusEvent) : void
      {
         if(param1.level == "error")
         {
            §_-928§ = false;
         }
      }
      
      override public function toString() : String
      {
         return "ru.pragmatix.flox.logger.ConsolePublisher";
      }
   }
}

