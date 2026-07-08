package ru.pragmatix.wormix.messages.structures
{
   import §_-62§.§_-G1p§;
   import §_-62§.§_-O2H§;
   import §_-82l§.§_-A2Y§;
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import §_-P1z§.*;
   import §_-RE§.§_-52p§;
   import §_-Z1K§.§_-q24§;
   import §_-l1g§.§_-L3Q§;
   import §_-o14§.§_-Dd§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.weapons.Weapon;
   import starling.events.Event;
   
   public class ChatMessage
   {
      
      public var id:uint;
      
      public var action:§_-A2Y§;
      
      public var logDate:int;
      
      public var profileId:int;
      
      public var platformId:String;
      
      public var profileName:String;
      
      public var message:String;
      
      public var params:String;
      
      public var isDeleted:Boolean;
      
      public function ChatMessage()
      {
         super();
      }
      
      public function toString() : String
      {
         return "ChatMessage{ " + " id=" + this.id + " action=" + this.action + " action.type=" + this.action.type + " logDate=" + this.logDate + " profileId=" + this.profileId + " platformId=" + this.platformId + " profileName=" + this.profileName + " message=" + this.message + " params=" + this.params + " }";
      }
   }
}

