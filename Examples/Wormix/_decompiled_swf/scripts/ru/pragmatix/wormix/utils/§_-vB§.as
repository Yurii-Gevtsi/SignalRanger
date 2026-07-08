package ru.pragmatix.wormix.utils
{
   import §_-A2U§.§_-A23§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.weapons.*;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class §_-vB§
   {
      
      public function §_-vB§()
      {
         super();
      }
      
      public static function §_-z1v§() : Boolean
      {
         if(Starling.current.stage3D == null)
         {
            return true;
         }
         if(Starling.current.context == null)
         {
            return true;
         }
         return Starling.current.context.driverInfo == "Disposed";
      }
   }
}

