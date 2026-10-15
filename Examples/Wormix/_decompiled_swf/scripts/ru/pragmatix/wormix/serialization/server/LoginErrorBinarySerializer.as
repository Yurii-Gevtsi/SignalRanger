package ru.pragmatix.wormix.serialization.server
{
   import §_-62§.§_-W1r§;
   import §_-Vg§.*;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-z1g§.§_-F4§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.fx.AbstractAuraEffect;
   import ru.pragmatix.wormix.messages.server.LoginError;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   
   public class LoginErrorBinarySerializer implements §_-D2L§
   {
      
      public function LoginErrorBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10002;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:LoginError = LoginError(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeShort(_loc3_.code);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:LoginError = new LoginError();
         _loc3_.code = param1.readShort();
         return _loc3_;
      }
   }
}

