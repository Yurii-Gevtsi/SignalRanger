package ru.pragmatix.wormix.serialization.server
{
   import §_-j22§.§_-5g§;
   import §_-o14§.BattleChatMessageRenderer;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.distriqt.extension.inappbilling.events.PurchaseEvent;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.server.BuyReactionRateResult;
   import starling.events.Event;
   
   public class BuyReactionRateResultBinarySerializer implements §_-D2L§
   {
      
      public function BuyReactionRateResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10037;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:BuyReactionRateResult = BuyReactionRateResult(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeShort(_loc3_.result);
         _loc5_.writeInt(_loc3_.reactionRateCount);
         _loc5_.writeInt(_loc3_.reactionRateLevel);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:BuyReactionRateResult = new BuyReactionRateResult();
         _loc3_.result = param1.readShort();
         _loc3_.reactionRateCount = param1.readInt();
         _loc3_.reactionRateLevel = param1.readInt();
         return _loc3_;
      }
   }
}

