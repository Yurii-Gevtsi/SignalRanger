package ru.pragmatix.wormix.serialization.client
{
   import §_-62§.§_-G1p§;
   import §_-62§.§_-W1r§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-A2U§.*;
   import §_-Cl§.*;
   import §_-D3A§.§_-TZ§;
   import §_-Tm§.§_-RF§;
   import §_-U1i§.§_-J1§;
   import §_-Z1K§.§_-q24§;
   import §_-g2r§.§_-03A§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-w2i§.*;
   import feathers.controls.ImageLoader;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanInviteStructure;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.client.RefundPvpBattleLoss;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   
   public class RefundPvpBattleLossBinarySerializer implements §_-D2L§
   {
      
      public function RefundPvpBattleLossBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 145;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:RefundPvpBattleLoss = RefundPvpBattleLoss(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _temp_2:* = new ByteArray();
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUTF(_loc3_.adEventId);
         _loc5_.writeUTF(_loc3_.sessionKey);
         _loc5_.writeInt(_loc3_.battleId);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         return null;
      }
   }
}

