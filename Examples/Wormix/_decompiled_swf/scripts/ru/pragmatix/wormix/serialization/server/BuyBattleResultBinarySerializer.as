package ru.pragmatix.wormix.serialization.server
{
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.distriqt.extension.inappbilling.events.PurchaseEvent;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.server.BuyBattleResult;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   
   public class BuyBattleResultBinarySerializer implements §_-D2L§
   {
      
      public function BuyBattleResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10012;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:BuyBattleResult = new BuyBattleResult();
         var _loc4_:ByteArray = new ByteArray();
         param1.readBytes(_loc4_,0,_loc4_.bytesAvailable);
         _loc3_.result = _loc4_.readShort();
         _loc3_.advPurchases = SerializationUtils.readStructureArray(_loc4_,AdvertisingPurchaseStructure);
         return _loc3_;
      }
   }
}

