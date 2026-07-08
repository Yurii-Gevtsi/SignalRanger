package ru.pragmatix.wormix.serialization.server
{
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-H2g§.§_-di§;
   import §_-j22§.§_-5g§;
   import §_-l2r§.§_-K1t§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import dragonBones.animation.WorldClock;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.messages.server.SteamTransactionCommitResponse;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import starling.display.DisplayObject;
   
   public class SteamTransactionCommitResponseBinarySerializer implements §_-D2L§
   {
      
      public function SteamTransactionCommitResponseBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 6004;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:SteamTransactionCommitResponse = SteamTransactionCommitResponse(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeShort(_loc3_.moneyType);
         _loc5_.writeUTF(_loc3_.item);
         _loc5_.writeInt(_loc3_.count);
         SerializationUtils.writeStructureArray(_loc5_,GenericAwardStructure,_loc3_.awards);
         _loc5_.writeUTF(_loc3_.note);
         _loc5_.writeUTF(_loc3_.sessionKey);
         _loc5_.writeShort(_loc3_.result);
         _loc5_.writeInt(_loc3_.purchaseOrderId);
         _loc5_.writeUTF(_loc3_.purchaseTransactionId);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:SteamTransactionCommitResponse = new SteamTransactionCommitResponse();
         var _loc4_:ByteArray = new ByteArray();
         param1.readBytes(_loc4_,0,param1.bytesAvailable);
         _loc3_.moneyType = _loc4_.readShort();
         _loc3_.item = _loc4_.readUTF();
         _loc3_.count = _loc4_.readInt();
         _loc3_.awards = SerializationUtils.readStructureArray(_loc4_,GenericAwardStructure);
         _loc3_.note = _loc4_.readUTF();
         _loc3_.sessionKey = _loc4_.readUTF();
         _loc3_.result = _loc4_.readShort();
         _loc3_.purchaseOrderId = _loc4_.readInt();
         _loc3_.purchaseTransactionId = _loc4_.readUTF();
         _loc3_.isSecure = §_-U11§.isSecure(_loc4_,§_-6L§.§_-95§);
         return _loc3_;
      }
   }
}

