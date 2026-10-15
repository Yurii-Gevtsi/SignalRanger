package ru.pragmatix.wormix.serialization.server
{
   import §_-12a§.§_-6p§;
   import §_-B1y§.§_-bs§;
   import §_-Y1O§.§_-y1o§;
   import §_-b1F§.§_-G4§;
   import §_-gG§.§_-Z2z§;
   import §_-j22§.§_-5g§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.messages.clans.request.DeleteClanRequest;
   import ru.pragmatix.wormix.messages.clans.response.DeleteClanResponse;
   import ru.pragmatix.wormix.messages.server.UserIsBanned;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.utils.§_-72u§;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   
   public class UserIsBannedBinarySerializer implements §_-D2L§
   {
      
      public function UserIsBannedBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10023;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:UserIsBanned = UserIsBanned(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeInt(_loc3_.reason);
         _loc5_.writeUnsignedInt(_loc3_.endDate);
         _loc5_.writeUnsignedInt(_loc3_.profileId);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:UserIsBanned = new UserIsBanned();
         _loc3_.reason = param1.readInt();
         _loc3_.endDate = param1.readUnsignedInt();
         _loc3_.profileId = param1.readUnsignedInt();
         return _loc3_;
      }
   }
}

