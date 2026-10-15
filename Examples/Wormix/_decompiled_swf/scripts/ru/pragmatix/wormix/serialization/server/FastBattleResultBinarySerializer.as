package ru.pragmatix.wormix.serialization.server
{
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-DJ§.§_-fH§;
   import §_-Vg§.§_-M11§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.server.FastBattleResult;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import starling.utils.ScaleMode;
   
   public class FastBattleResultBinarySerializer implements §_-D2L§
   {
      
      public function FastBattleResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10142;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:FastBattleResult = new FastBattleResult();
         var _loc4_:ByteArray = new ByteArray();
         param1.readBytes(_loc4_,0,_loc4_.bytesAvailable);
         _loc3_.result = _loc4_.readShort();
         _loc3_.adEventId = _loc4_.readUTF();
         _loc3_.awards = SerializationUtils.readStructureArray(_loc4_,GenericAwardStructure);
         _loc3_.advPurchases = SerializationUtils.readStructureArray(_loc4_,AdvertisingPurchaseStructure);
         _loc3_.sessionKey = _loc4_.readUTF();
         var _loc5_:Boolean = §_-U11§.isSecure(_loc4_,§_-6L§.§_-95§);
         if(!_loc5_)
         {
            §_-J2g§.§_-i1N§.dispose();
            §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.PROTOCOL_CHEATER),§_-fH§.§_-L2I§);
         }
         return _loc3_;
      }
   }
}

