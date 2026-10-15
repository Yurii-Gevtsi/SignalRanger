package ru.pragmatix.wormix.serialization.server
{
   import §_-21p§.§_-N1H§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-DJ§.§_-fH§;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.server.ConnectResult;
   import ru.pragmatix.wormix.messages.server.FastBattleResult;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   
   public class ConnectResultBinarySerializer implements §_-D2L§
   {
      
      public function ConnectResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 1027;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:ConnectResult = ConnectResult(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         var _loc6_:ByteArray = new ByteArray();
         var _loc7_:uint = 0;
         var _loc8_:uint = _loc3_.userProfileStructures.length;
         _loc5_.writeShort(_loc8_);
         _loc7_ = 0;
         while(_loc7_ < _loc8_)
         {
            _loc6_ = new ByteArray();
            §_-616§.§_-fk§(UserProfileStructure).serializeStructure(_loc3_.userProfileStructures[_loc7_],_loc6_,null);
            _loc5_.writeShort(_loc6_.length);
            _loc5_.writeBytes(_loc6_);
            _loc7_++;
         }
         _loc5_.writeShort(_loc3_.onlineFriends);
         _loc5_.writeShort(_loc3_.friends);
         _loc5_.writeBoolean(_loc3_.connectAwardGranted);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ConnectResult = new ConnectResult();
         var _loc4_:ByteArray = new ByteArray();
         param1.readBytes(_loc4_,0,param1.bytesAvailable);
         var _loc5_:uint = 0;
         var _loc6_:uint = _loc4_.readUnsignedShort();
         _loc3_.userProfileStructures = new Array();
         _loc5_ = 0;
         while(_loc5_ < _loc6_)
         {
            _loc4_.readShort();
            _loc3_.userProfileStructures[_loc5_] = §_-616§.§_-fk§(UserProfileStructure).deserializeStructure(_loc4_,null);
            _loc5_++;
         }
         _loc3_.onlineFriends = _loc4_.readShort();
         _loc3_.friends = _loc4_.readShort();
         _loc3_.connectAwardGranted = _loc4_.readBoolean();
         _loc3_.isSecure = §_-U11§.isSecure(_loc4_,§_-6L§.§_-95§);
         return _loc3_;
      }
   }
}

