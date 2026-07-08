package ru.pragmatix.wormix.messages.server
{
   import §_-416§.*;
   import §_-79§.§_-11E§;
   import §_-A1K§.§_-TA§;
   import §_-A2U§.§_-A23§;
   import §_-Ly§.§_-81L§;
   import §_-k1u§.§_-Oi§;
   import §_-r1C§.§_-h2B§;
   import com.greensock.core.TweenCore;
   import feathers.layout.HorizontalLayout;
   import ru.pragmatix.flox.gui.controls.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import starling.events.Event;
   
   public class RefundPvpBattleLossResult
   {
      
      public var status:int;
      
      public var adEventId:String;
      
      public var awards:Array;
      
      public var advPurchases:Array;
      
      public var sessionKey:String;
      
      public var battleId:int;
      
      public function RefundPvpBattleLossResult()
      {
         super();
      }
      
      public function toString() : String
      {
         return "RefundPvpBattleLossResult{" + "status=" + this.status + ",adEventId=" + this.adEventId + ",battleId=" + this.battleId + ",sessionKey=" + this.sessionKey + ",awards=" + this.awards + ",advPurchases=" + this.advPurchases + "}";
      }
   }
}

