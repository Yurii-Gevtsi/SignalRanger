package ru.pragmatix.wormix.serialization.server
{
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.server.AssembleStuffResult;
   import ru.pragmatix.wormix.messages.server.VerifyPaymentResult;
   import ru.pragmatix.wormix.messages.utils.Result;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   
   public class AssembleStuffResultBinarySerializer implements §_-D2L§
   {
      
      public function AssembleStuffResultBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ByteArray = null;
         _loc3_ = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         var _loc4_:AssembleStuffResult = new AssembleStuffResult();
         _loc4_.equipId = _loc3_.readShort();
         _loc4_.familyId = _loc3_.readShort();
         _loc4_.result = _loc3_.readShort();
         var _loc5_:String = _loc3_.readUTF();
         SerializationUtils.checkSecure(_loc3_,_loc5_);
         return _loc4_;
      }
      
      public function getCommandId() : uint
      {
         return 10099;
      }
   }
}

