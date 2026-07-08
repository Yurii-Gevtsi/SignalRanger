package ru.pragmatix.wormix.serialization.server
{
   import §_-5Y§.§_-t2O§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-j1w§.§_-L1x§;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import feathers.events.FeathersEventType;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.messages.server.BuyRaceResult;
   import ru.pragmatix.wormix.messages.server.NeedMoneyResult;
   import ru.pragmatix.wormix.messages.server.SteamProductsInfoResponse;
   import ru.pragmatix.wormix.messages.structures.CostStructure;
   import ru.pragmatix.wormix.messages.structures.SteamProductInfoStructure;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import starling.events.Event;
   
   public class NeedMoneyResultBinarySerializer implements §_-D2L§
   {
      
      public function NeedMoneyResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10008;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:NeedMoneyResult = NeedMoneyResult(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeInt(_loc3_.value);
         _loc5_.writeShort(_loc3_.result);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:NeedMoneyResult = new NeedMoneyResult();
         _loc3_.value = param1.readInt();
         _loc3_.result = param1.readShort();
         return _loc3_;
      }
   }
}

