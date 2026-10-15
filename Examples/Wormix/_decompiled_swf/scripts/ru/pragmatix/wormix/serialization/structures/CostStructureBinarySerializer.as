package ru.pragmatix.wormix.serialization.structures
{
   import §_-43V§.Random;
   import §_-5Y§.§_-h2c§;
   import §_-CF§.§_-E19§;
   import §_-K3q§.§_-L3A§;
   import §_-Y1Q§.§_-aG§;
   import §_-q26§.§_-Z25§;
   import §_-rM§.§_-02j§;
   import §_-z20§.§_-pl§;
   import §_-zI§.§_-A1s§;
   import com.hurlant.crypto.tls.*;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanMemberStructure;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanNewsTO;
   import ru.pragmatix.wormix.messages.structures.CostStructure;
   import ru.pragmatix.wormix.model.rewards.§_-qU§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.§_-6A§;
   import starling.events.Event;
   
   public class CostStructureBinarySerializer implements §_-02j§
   {
      
      public function CostStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
         var _loc4_:CostStructure = CostStructure(param1);
         param2.writeShort(_loc4_.currencyType.id);
         param2.writeInt(_loc4_.itemId);
         param2.writeInt(_loc4_.value);
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:CostStructure = new CostStructure();
         _loc3_.currencyType = §_-E19§.§_-H1M§(param1.readShort());
         _loc3_.itemId = param1.readInt();
         _loc3_.value = param1.readInt();
         return _loc3_;
      }
   }
}

