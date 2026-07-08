package ru.pragmatix.wormix.serialization.server
{
   import §_-B1y§.§_-bs§;
   import §_-C3f§.§_-yG§;
   import §_-L1H§.§_-b2K§;
   import §_-Y1O§.*;
   import §_-j1w§.§_-B2i§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.distriqt.extension.inappbilling.Purchase;
   import flash.errors.IllegalOperationError;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.messages.clans.request.edit.RenameClanRequest;
   import ru.pragmatix.wormix.messages.clans.response.edit.RenameClanResponse;
   import ru.pragmatix.wormix.messages.client.GoogleValidateInappPurchase;
   import ru.pragmatix.wormix.messages.server.RepeatPvpBattleAwardResult;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   
   public class RepeatPvpBattleAwardResultBinarySerializer implements §_-D2L§
   {
      
      public function RepeatPvpBattleAwardResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10144;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:RepeatPvpBattleAwardResult = new RepeatPvpBattleAwardResult();
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

