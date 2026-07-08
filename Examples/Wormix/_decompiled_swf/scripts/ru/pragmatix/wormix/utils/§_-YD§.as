package ru.pragmatix.wormix.utils
{
   import §_-DJ§.§_-J2l§;
   import §_-L1H§.§_-b2K§;
   import §_-Tm§.§_-RF§;
   import §_-l1g§.§_-L3Q§;
   import flash.errors.IllegalOperationError;
   import flash.system.Capabilities;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class §_-YD§
   {
      
      public function §_-YD§()
      {
         super();
      }
      
      public static function get §_-t1O§() : Boolean
      {
         return Capabilities.os.indexOf("Mac") != -1 || Capabilities.os.indexOf("Win") != -1;
      }
   }
}

