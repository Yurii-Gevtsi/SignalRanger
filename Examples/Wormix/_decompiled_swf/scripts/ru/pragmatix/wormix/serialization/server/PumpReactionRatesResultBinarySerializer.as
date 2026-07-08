package ru.pragmatix.wormix.serialization.server
{
   import §_-62§.*;
   import §_-C3f§.§_-yG§;
   import §_-Vu§.*;
   import §_-j22§.§_-5g§;
   import §_-q26§.*;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginClanBase;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanEnterAccount;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanErrorResponse;
   import ru.pragmatix.wormix.messages.server.PumpReactionRatesResult;
   import ru.pragmatix.wormix.messages.structures.PumpReactionRateStructure;
   
   public class PumpReactionRatesResultBinarySerializer implements §_-D2L§
   {
      
      public function PumpReactionRatesResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10033;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc8_:ByteArray = null;
         var _loc3_:PumpReactionRatesResult = PumpReactionRatesResult(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         var _loc6_:uint = 0;
         var _loc7_:uint = _loc3_.pumpedFriends.length;
         _loc5_.writeShort(_loc7_);
         _loc6_ = 0;
         while(_loc6_ < _loc7_)
         {
            _loc8_ = new ByteArray();
            §_-616§.§_-fk§(PumpReactionRateStructure).serializeStructure(_loc3_.pumpedFriends[_loc6_],_loc8_,null);
            _loc5_.writeShort(_loc8_.length);
            _loc5_.writeBytes(_loc8_);
            _loc6_++;
         }
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:PumpReactionRatesResult = new PumpReactionRatesResult();
         var _loc4_:uint = 0;
         var _loc5_:uint = param1.readUnsignedShort();
         _loc3_.pumpedFriends = [];
         _loc4_ = 0;
         while(_loc4_ < _loc5_)
         {
            param1.readShort();
            _loc3_.pumpedFriends[_loc4_] = §_-616§.§_-fk§(PumpReactionRateStructure).deserializeStructure(param1,null);
            _loc4_++;
         }
         return _loc3_;
      }
   }
}

