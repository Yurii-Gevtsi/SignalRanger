package ru.pragmatix.wormix.serialization.clans.main.server
{
   import §_-91B§.§_-QK§;
   import §_-B1y§.§_-bs§;
   import §_-C3f§.§_-yG§;
   import §_-G2H§.*;
   import §_-H16§.*;
   import §_-T1o§.§_-L3M§;
   import §_-b1F§.§_-v2Q§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-w2i§.§_-52k§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import org.swiftsuspenders.Injector;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanInviteStructure;
   import ru.pragmatix.wormix.messages.clans.main.server.InviteFriendToClan;
   import ru.pragmatix.wormix.messages.clans.request.edit.ChangeClanDescriptionRequest;
   import ru.pragmatix.wormix.messages.clans.response.edit.ChangeClanDescriptionResponse;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseAmmoWithArmature;
   import starling.events.Event;
   
   public class InviteFriendToClanBinarySerializer implements §_-D2L§
   {
      
      public function InviteFriendToClanBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:InviteFriendToClan = new InviteFriendToClan();
         param1.readShort();
         _loc3_.invite = ClanInviteStructure(§_-616§.§_-fk§(ClanInviteStructure).deserializeStructure(param1,null));
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return 94;
      }
   }
}

