package ru.pragmatix.wormix.serialization.server
{
   import §_-82l§.§_-A2Y§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-Y1O§.*;
   import §_-j22§.§_-5g§;
   import §_-o14§.§_-Dd§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-z1g§.§_-M2v§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.server.SearchTheHouseResult;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.pvp.messages.server.PvpSystemMessage;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   
   public class SearchTheHouseResultBinarySerializer implements §_-D2L§
   {
      
      public function SearchTheHouseResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10021;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:SearchTheHouseResult = SearchTheHouseResult(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _temp_2:* = new ByteArray();
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeByte(_loc3_.result);
         _loc5_.writeInt(_loc3_.value);
         _loc5_.writeByte(_loc3_.availableSearchKeys);
         _loc5_.writeUnsignedInt(_loc3_.friendId);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ByteArray = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         var _temp_1:* = new SearchTheHouseResult();
         var _loc4_:SearchTheHouseResult = new SearchTheHouseResult();
         _loc4_.result = _loc3_.readByte();
         _loc4_.value = _loc3_.readInt();
         _loc4_.availableSearchKeys = _loc3_.readByte();
         _loc4_.friendId = _loc3_.readUnsignedInt();
         _loc4_.isSecure = §_-U11§.isSecure(_loc3_,§_-6L§.§_-95§);
         return _loc4_;
      }
   }
}

