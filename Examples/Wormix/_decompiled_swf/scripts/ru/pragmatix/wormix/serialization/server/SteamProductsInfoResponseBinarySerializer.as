package ru.pragmatix.wormix.serialization.server
{
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.messages.server.SteamProductsInfoResponse;
   import ru.pragmatix.wormix.messages.structures.SteamProductInfoStructure;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   
   public class SteamProductsInfoResponseBinarySerializer implements §_-D2L§
   {
      
      public function SteamProductsInfoResponseBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 6006;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:SteamProductsInfoResponse = SteamProductsInfoResponse(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUTF(_loc3_.countryCode);
         _loc5_.writeUTF(_loc3_.currencyCode);
         SerializationUtils.writeStructureArray(_loc5_,SteamProductInfoStructure,_loc3_.products);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:SteamProductsInfoResponse = new SteamProductsInfoResponse();
         _loc3_.countryCode = param1.readUTF();
         _loc3_.currencyCode = param1.readUTF();
         _loc3_.products = SerializationUtils.readStructureArray(param1 as ByteArray,SteamProductInfoStructure);
         return _loc3_;
      }
   }
}

