package ru.pragmatix.wormix.serialization.clans.structures
{
   import §_-12a§.§_-B16§;
   import §_-C3f§.*;
   import §_-P2i§.§_-G3J§;
   import §_-Z1K§.§_-q24§;
   import §_-rM§.§_-02j§;
   import §_-rM§.§_-61j§;
   import flash.geom.Point;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.server.BuyVipResult;
   import ru.pragmatix.wormix.messages.structures.RentedItemsStructure;
   import ru.pragmatix.wormix.model.rewards.§_-P2q§;
   import ru.pragmatix.wormix.objects.Waypoint;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.weapons.*;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class ClanChatMessageTOBinarySerializer implements §_-02j§
   {
      
      public function ClanChatMessageTOBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:ClanChatMessageTO = null;
         _loc3_ = new ClanChatMessageTO();
         _loc3_.actionCode = param1.readInt();
         _loc3_.logDate = param1.readInt();
         _loc3_.publisherSocialId = param1.readShort();
         _loc3_.publisherProfileId = param1.readInt();
         _loc3_.publisherName = param1.readUTF();
         _loc3_.memberSocialId = param1.readShort();
         _loc3_.memberProfileId = param1.readInt();
         _loc3_.memberName = param1.readUTF();
         _loc3_.params = param1.readUTF();
         return _loc3_;
      }
   }
}

