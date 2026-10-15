package ru.pragmatix.wormix.serialization.structures
{
   import §_-62§.§_-O2H§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.§_-F1P§;
   import §_-j1w§.§_-L1x§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-d2j§;
   import §_-rM§.§_-02j§;
   import §_-rM§.§_-61j§;
   import §_-z1g§.§_-4l§;
   import §_-z1g§.§_-522§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.messages.server.BuyRaceResult;
   import ru.pragmatix.wormix.messages.server.SteamProductsInfoResponse;
   import ru.pragmatix.wormix.messages.structures.CostStructure;
   import ru.pragmatix.wormix.messages.structures.SteamProductInfoStructure;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import starling.textures.Texture;
   
   public class SteamProductInfoStructureBinarySerializer implements §_-02j§
   {
      
      public function SteamProductInfoStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
         var _loc4_:SteamProductInfoStructure = SteamProductInfoStructure(param1);
         param2.writeUTF(_loc4_.code);
         param2.writeUTF(_loc4_.category);
         param2.writeInt(_loc4_.cost);
         param2.writeUTF(_loc4_.description);
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:SteamProductInfoStructure = new SteamProductInfoStructure();
         _loc3_.code = param1.readUTF();
         _loc3_.category = param1.readUTF();
         _loc3_.cost = param1.readInt();
         _loc3_.description = param1.readUTF();
         return _loc3_;
      }
   }
}

