package ru.pragmatix.wormix.serialization.clans.structures
{
   import §_-21p§.§_-4w§;
   import §_-62§.*;
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import §_-rM§.§_-02j§;
   import config.§_-B1K§;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.messages.clans.structures.ClanInviteTO;
   import ru.pragmatix.wormix.objects.§_-01J§;
   
   public class ClanInviteTOBinarySerializer implements §_-02j§
   {
      
      public function ClanInviteTOBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:ClanInviteTO = new ClanInviteTO();
         _loc3_.clanId = param1.readInt();
         _loc3_.profileId = param1.readInt();
         _loc3_.inviteDate = param1.readInt();
         return _loc3_;
      }
   }
}

