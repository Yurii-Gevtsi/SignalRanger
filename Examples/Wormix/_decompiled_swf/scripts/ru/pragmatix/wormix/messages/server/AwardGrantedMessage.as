package ru.pragmatix.wormix.messages.server
{
   import §_-62§.§_-W1r§;
   import §_-73d§.§_-g1J§;
   import §_-Y1O§.§_-V23§;
   import §_-k13§.§_-j23§;
   import §_-l1K§.§_-d2j§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import dragonBones.animation.WorldClock;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.items.craft.§_-6S§;
   import ru.pragmatix.wormix.model.rewards.§_-d2F§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-O1f§;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   public class AwardGrantedMessage
   {
      
      public var awardType:int;
      
      public var attach:String;
      
      public var awards:Array;
      
      public var bonus:§_-d2F§;
      
      public function AwardGrantedMessage()
      {
         super();
      }
      
      public function toString() : String
      {
         return "AwardGrantedMessage{attach=" + this.attach + ",awardType=" + this.awardType + ",awards=" + this.awards + "}";
      }
   }
}

