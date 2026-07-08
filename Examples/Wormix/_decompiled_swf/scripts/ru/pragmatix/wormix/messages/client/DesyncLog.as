package ru.pragmatix.wormix.messages.client
{
   import §_-DJ§.§_-J2l§;
   import §_-T2h§.*;
   import §_-d26§.§_-d1H§;
   import com.hurlant.util.der.§_-UO§;
   import feathers.utils.touch.TapToTrigger;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import starling.display.Sprite;
   import starling.events.Event;
   
   public class DesyncLog
   {
      
      public var battleId:int;
      
      public var battleType:int;
      
      public var wager:int;
      
      public var mapId:int;
      
      public var missionIds:Vector.<int>;
      
      public var battleLogs:Array;
      
      public function DesyncLog()
      {
         super();
      }
   }
}

