package ru.pragmatix.wormix.serialization.server
{
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-Y1b§.§_-OP§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.messages.server.RepeatBossBattleAwardResult;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.display.DisplayObject;
   import starling.utils.Pool;
   
   public class RepeatBossBattleAwardResultBinarySerializer implements §_-D2L§
   {
      
      public function RepeatBossBattleAwardResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10143;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:RepeatBossBattleAwardResult = new RepeatBossBattleAwardResult();
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

