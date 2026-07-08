package ru.pragmatix.wormix.messages.server
{
   import §_-62§.*;
   import §_-63U§.§_-412§;
   import §_-j1w§.§_-L1x§;
   import §_-r1C§.§_-h2B§;
   import com.hurlant.math.*;
   import flash.events.MouseEvent;
   import flash.system.ApplicationDomain;
   import flash.utils.ByteArray;
   import org.swiftsuspenders.Injector;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   
   use namespace bi_internal;
   
   public class RepeatBossBattleAwardResult
   {
      
      public var status:int;
      
      public var adEventId:String;
      
      public var awards:Array;
      
      public var advPurchases:Array;
      
      public var sessionKey:String;
      
      public var battleId:int;
      
      public function RepeatBossBattleAwardResult()
      {
         super();
      }
      
      public function toString() : String
      {
         return "RepeatBossBattleAwardResult{" + "status=" + this.status + ",adEventId=" + this.adEventId + ",battleId=" + this.battleId + ",sessionKey=" + this.sessionKey + ",awards=" + this.awards + ",advPurchases=" + this.advPurchases + "}";
      }
   }
}

