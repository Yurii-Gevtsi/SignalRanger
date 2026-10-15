package ru.pragmatix.wormix.serialization.server
{
   import §_-C3f§.§_-yG§;
   import §_-G2H§.*;
   import §_-T1o§.§_-L3M§;
   import §_-TF§.§_-P19§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-wD§.*;
   import §_-zI§.§_-8D§;
   import com.mesmotronic.ane.AndroidID;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-S2Y§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.server.StuffExpired;
   import starling.display.DisplayObject;
   
   public class StuffExpiredBinarySerializer implements §_-D2L§
   {
      
      public function StuffExpiredBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10093;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ByteArray = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         var _loc4_:StuffExpired = new StuffExpired();
         var _loc5_:uint = _loc3_.readUnsignedShort();
         _loc4_.stuff = [];
         var _loc6_:uint = 0;
         while(_loc6_ < _loc5_)
         {
            _loc4_.stuff[_loc6_] = _loc3_.readShort();
            _loc6_++;
         }
         _loc4_.hat = _loc3_.readShort();
         _loc4_.kit = _loc3_.readShort();
         return _loc4_;
      }
   }
}

