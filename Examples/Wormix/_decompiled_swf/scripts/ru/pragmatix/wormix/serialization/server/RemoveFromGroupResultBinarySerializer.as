package ru.pragmatix.wormix.serialization.server
{
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-zI§.§_-sI§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.utils.id.*;
   import ru.pragmatix.wormix.messages.server.RemoveFromGroupResult;
   import starling.animation.Tween;
   import starling.core.Starling;
   import starling.events.Event;
   
   public class RemoveFromGroupResultBinarySerializer implements §_-D2L§
   {
      
      public function RemoveFromGroupResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10014;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:RemoveFromGroupResult = RemoveFromGroupResult(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeShort(_loc3_.result);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:RemoveFromGroupResult = new RemoveFromGroupResult();
         _loc3_.result = param1.readShort();
         return _loc3_;
      }
   }
}

