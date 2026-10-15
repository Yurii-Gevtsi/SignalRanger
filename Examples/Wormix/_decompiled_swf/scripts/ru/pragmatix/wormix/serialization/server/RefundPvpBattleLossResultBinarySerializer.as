package ru.pragmatix.wormix.serialization.server
{
   import §_-62§.§_-W1r§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-l1g§.§_-T2N§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.server.RefundPvpBattleLossResult;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   
   public class RefundPvpBattleLossResultBinarySerializer implements §_-D2L§
   {
      
      public function RefundPvpBattleLossResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10145;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:RefundPvpBattleLossResult = new RefundPvpBattleLossResult();
         _loc3_.status = param1.readShort();
         _loc3_.adEventId = param1.readUTF();
         _loc3_.awards = SerializationUtils.readStructureArray(param1 as ByteArray,GenericAwardStructure);
         _loc3_.advPurchases = SerializationUtils.readStructureArray(param1 as ByteArray,AdvertisingPurchaseStructure);
         _loc3_.sessionKey = param1.readUTF();
         _loc3_.battleId = param1.readInt();
         return _loc3_;
      }
   }
}

