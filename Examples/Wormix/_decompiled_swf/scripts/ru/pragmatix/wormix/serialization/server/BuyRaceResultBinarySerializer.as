package ru.pragmatix.wormix.serialization.server
{
   import §_-31N§.§_-42G§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-YF§.§_-q2v§;
   import §_-j1w§.§_-L1x§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-w2i§.§_-Zd§;
   import feathers.events.FeathersEventType;
   import flash.geom.ColorTransform;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.server.BuyRaceResult;
   import ru.pragmatix.wormix.messages.server.SteamProductsInfoResponse;
   import ru.pragmatix.wormix.messages.structures.CostStructure;
   import ru.pragmatix.wormix.messages.structures.SteamProductInfoStructure;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.§_-MN§;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   
   public class BuyRaceResultBinarySerializer implements §_-D2L§
   {
      
      public function BuyRaceResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10028;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:BuyRaceResult = BuyRaceResult(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeShort(_loc3_.result);
         SerializationUtils.writeStructureArray(param2 as ByteArray,CostStructure,_loc3_.cost);
         _loc5_.writeUTF(_loc3_.sessionKey);
         _loc5_.writeShort(_loc3_.race.getId());
         _loc5_.writeShort(_loc3_.races);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:BuyRaceResult = new BuyRaceResult();
         var _loc4_:ByteArray = new ByteArray();
         param1.readBytes(_loc4_,0,param1.bytesAvailable);
         _loc3_.result = _loc4_.readShort();
         _loc3_.cost = SerializationUtils.readStructureArray(_loc4_,CostStructure);
         _loc3_.sessionKey = _loc4_.readUTF();
         _loc3_.race = §_-L1x§.§_-S1D§(_loc4_.readShort());
         _loc3_.races = _loc4_.readShort();
         _loc3_.isSecure = §_-U11§.isSecure(_loc4_,§_-6L§.§_-95§);
         return _loc3_;
      }
   }
}

