package ru.pragmatix.wormix.serialization.server
{
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-vO§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.worlize.websocket.§_-Y2T§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.messages.server.AchieveLoginSuccess;
   
   public class AchieveLoginSuccessBinarySerializer implements §_-D2L§
   {
      
      public function AchieveLoginSuccessBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 13001;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:AchieveLoginSuccess = AchieveLoginSuccess(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUTF(_loc3_.sessionId);
         _loc5_.writeUnsignedInt(_loc3_.loginTime);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:AchieveLoginSuccess = new AchieveLoginSuccess();
         var _loc4_:ByteArray = new ByteArray();
         param1.readBytes(_loc4_,0,param1.bytesAvailable);
         _loc3_.sessionId = _loc4_.readUTF();
         _loc3_.loginTime = _loc4_.readUnsignedInt();
         _loc3_.isSecure = §_-U11§.isSecure(_loc4_,§_-6L§.§_-95§);
         return _loc3_;
      }
   }
}

