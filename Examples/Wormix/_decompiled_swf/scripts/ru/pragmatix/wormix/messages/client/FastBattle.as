package ru.pragmatix.wormix.messages.client
{
   import §_-A2U§.§_-A23§;
   import §_-G16§.*;
   import §_-j22§.§_-5g§;
   import feathers.events.FeathersEventType;
   import feathers.layout.VerticalLayout;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.*;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class FastBattle
   {
      
      public var adEventId:String;
      
      public var sessionKey:String;
      
      public function FastBattle(param1:String = "", param2:String = "")
      {
         super();
         this.adEventId = param1;
         this.sessionKey = param2;
      }
      
      public function toString() : String
      {
         return "FastBattle{" + "adEventId=" + this.adEventId + ",sessionKey=" + this.sessionKey + "}";
      }
   }
}

