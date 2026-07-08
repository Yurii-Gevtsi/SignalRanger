package ru.pragmatix.wormix.serialization.clans.main.server
{
   import §_-21p§.§_-N1H§;
   import §_-937§.§_-BO§;
   import §_-D3A§.§_-TZ§;
   import §_-L1H§.§_-b2K§;
   import §_-Tm§.§_-RF§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-02j§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanInviteStructure;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class ClanInviteStructureBinarySerializer implements §_-02j§
   {
      
      public function ClanInviteStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:ClanInviteStructure = new ClanInviteStructure();
         _loc3_.clanId = param1.readInt();
         _loc3_.clanName = param1.readUTF();
         var _loc4_:int = param1.readShort();
         var _loc5_:ByteArray = new ByteArray();
         param1.readBytes(_loc5_,0,_loc4_);
         SerializationUtils.readUintVector(_loc5_,_loc3_.clanEmblem);
         _loc3_.profileId = param1.readUnsignedInt();
         _loc3_.stringProfileId = param1.readUTF();
         _loc3_.rank = param1.readShort();
         _loc3_.name = param1.readUTF();
         _loc3_.inviteDate = param1.readInt();
         return _loc3_;
      }
   }
}

