package ru.pragmatix.wormix.serialization.server
{
   import §_-91B§.§_-z2l§;
   import §_-DJ§.MessageBox;
   import §_-Vg§.*;
   import §_-j1w§.§_-B2i§;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.messages.server.SendWipeConfirmCodeResponse;
   import ru.pragmatix.wormix.weapons.*;
   import starling.events.Event;
   
   public class SendWipeConfirmCodeResponseBinarySerializer implements §_-D2L§
   {
      
      public function SendWipeConfirmCodeResponseBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10040;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:SendWipeConfirmCodeResponse = SendWipeConfirmCodeResponse(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeByte(_loc3_.result);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:SendWipeConfirmCodeResponse = new SendWipeConfirmCodeResponse();
         _loc3_.result = param1.readByte();
         return _loc3_;
      }
   }
}

