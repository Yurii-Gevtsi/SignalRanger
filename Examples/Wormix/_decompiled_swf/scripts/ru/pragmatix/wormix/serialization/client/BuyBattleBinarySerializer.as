package ru.pragmatix.wormix.serialization.client
{
   import §_-43V§.*;
   import §_-62§.§_-O2H§;
   import §_-91A§.*;
   import §_-91B§.§_-QK§;
   import §_-91B§.§_-z2l§;
   import §_-DJ§.§_-T2G§;
   import §_-DJ§.§_-zb§;
   import §_-F39§.§_-i1i§;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-w2W§.§_-21w§;
   import §_-zI§.§_-X1q§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.client.BuyBattle;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.utils.*;
   import starling.events.Event;
   
   public class BuyBattleBinarySerializer implements §_-D2L§
   {
      
      public function BuyBattleBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 11;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:BuyBattle = BuyBattle(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeInt(_loc3_.moneyType);
         _loc5_.writeBoolean(_loc3_.bulk);
         _loc5_.writeUTF(_loc3_.adEventId);
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

