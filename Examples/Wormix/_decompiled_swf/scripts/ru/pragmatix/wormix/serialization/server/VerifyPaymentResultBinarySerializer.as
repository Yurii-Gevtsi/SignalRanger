package ru.pragmatix.wormix.serialization.server
{
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-DJ§.§_-fH§;
   import §_-Y1O§.*;
   import §_-j1w§.§_-B2i§;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import flash.errors.IllegalOperationError;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.server.VerifyPaymentResult;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   
   public class VerifyPaymentResultBinarySerializer implements §_-D2L§
   {
      
      public function VerifyPaymentResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10020;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:VerifyPaymentResult = VerifyPaymentResult(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeShort(_loc3_.moneyType);
         _loc5_.writeUTF(_loc3_.item);
         _loc5_.writeInt(_loc3_.count);
         _loc5_.writeUTF(_loc3_.note);
         _loc5_.writeUTF(§_-J2g§.§_-i1N§.§_-yt§());
         _loc5_.writeShort(_loc3_.result);
         _loc5_.writeInt(_loc3_.subscriptionExpiryTimeSeconds);
         _loc5_.writeUTF(_loc3_.transactionId);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ByteArray = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         var _loc4_:VerifyPaymentResult = new VerifyPaymentResult();
         _loc4_.moneyType = _loc3_.readShort();
         _loc4_.item = _loc3_.readUTF();
         _loc4_.count = _loc3_.readInt();
         _loc4_.awards = SerializationUtils.readStructureArray(_loc3_,GenericAwardStructure);
         _loc4_.note = _loc3_.readUTF();
         _loc4_.sessionKey = _loc3_.readUTF();
         _loc4_.result = _loc3_.readShort();
         _loc4_.subscriptionExpiryTimeSeconds = _loc3_.readInt();
         _loc4_.transactionId = _loc3_.readUTF();
         var _loc5_:Boolean = §_-U11§.isSecure(_loc3_,§_-6L§.§_-95§);
         if(!_loc5_)
         {
            §_-J2g§.§_-i1N§.dispose();
            §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.PROTOCOL_CHEATER) + " VerifyPaymentResult",§_-fH§.§_-L2I§);
            return null;
         }
         return _loc4_;
      }
   }
}

