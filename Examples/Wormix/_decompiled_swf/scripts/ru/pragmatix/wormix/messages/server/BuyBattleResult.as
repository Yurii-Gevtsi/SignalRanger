package ru.pragmatix.wormix.messages.server
{
   import §_-31W§.*;
   import §_-5Y§.§_-h2c§;
   import §_-B1y§.§_-F1P§;
   import §_-L1H§.§_-b2K§;
   import §_-Ly§.§_-81L§;
   import §_-r1C§.§_-h2B§;
   import §_-z1g§.§_-F4§;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.events.TweenEvent;
   import com.greensock.plugins.*;
   import feathers.controls.TabBar;
   import flash.display.*;
   import flash.events.*;
   import flash.utils.*;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.model.items.craft.§_-6S§;
   import starling.events.Event;
   
   public class BuyBattleResult
   {
      
      public var result:int;
      
      public var advPurchases:Array;
      
      public function BuyBattleResult()
      {
         super();
      }
      
      public function toString() : String
      {
         return "BuyBattleResult{" + "result=" + this.result + ",ads=" + this.advPurchases + "}";
      }
   }
}

