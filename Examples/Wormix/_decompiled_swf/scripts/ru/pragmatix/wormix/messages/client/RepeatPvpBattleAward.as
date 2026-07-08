package ru.pragmatix.wormix.messages.client
{
   import §_-DJ§.§_-J2l§;
   import §_-Y1O§.*;
   import §_-Z1w§.§_-s1x§;
   import §_-sK§.*;
   import com.hurlant.util.der.§_-UO§;
   import feathers.core.ITextRenderer;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.utils.§_-R2g§;
   import starling.events.Event;
   
   public class RepeatPvpBattleAward
   {
      
      public var adEventId:String = "";
      
      public var sessionKey:String;
      
      public var battleId:int;
      
      public function RepeatPvpBattleAward()
      {
         super();
      }
      
      public function toString() : String
      {
         return "RepeatPvpBattleAward{" + "battleId=" + this.battleId + ",adEventId=" + this.adEventId + ",sessionKey=" + this.sessionKey + "}";
      }
   }
}

