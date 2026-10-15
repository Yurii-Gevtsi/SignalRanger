package ru.pragmatix.wormix.serialization.server
{
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-E1n§;
   import §_-l1K§.§_-m2§;
   import §_-l1K§.§_-n2T§;
   import §_-lh§.§_-z2A§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.logger.*;
   import ru.pragmatix.wormix.messages.server.PumpReactionTheHouseResult;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.AirBlocker;
   import starling.utils.SystemUtil;
   
   public class PumpReactionRateResultBinarySerializer implements §_-D2L§
   {
      
      public function PumpReactionRateResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10032;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:PumpReactionTheHouseResult = PumpReactionTheHouseResult(param1);
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
         var _loc3_:PumpReactionTheHouseResult = new PumpReactionTheHouseResult();
         _loc3_.result = param1.readByte();
         return _loc3_;
      }
   }
}

