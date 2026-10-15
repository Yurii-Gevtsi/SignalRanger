package ru.pragmatix.wormix.serialization.server
{
   import §_-B1y§.§_-83E§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.*;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import flash.events.Event;
   import flash.events.IOErrorEvent;
   import flash.events.SecurityErrorEvent;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.messages.server.BuyVipResult;
   import ru.pragmatix.wormix.messages.structures.RentedItemsStructure;
   import ru.pragmatix.wormix.objects.§_-01J§;
   
   public class BuyVipResultBinarySerializer implements §_-D2L§
   {
      
      public function BuyVipResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10130;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:BuyVipResult = null;
         param1.readShort();
         _loc3_ = new BuyVipResult();
         _loc3_.rentedItemsStructure = RentedItemsStructure(§_-616§.§_-fk§(RentedItemsStructure).deserializeStructure(param1,null));
         _loc3_.renameAct = param1.readByte();
         _loc3_.sessionKey = param1.readUTF();
         return _loc3_;
      }
   }
}

