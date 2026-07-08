package ru.pragmatix.wormix.serialization.clans.response
{
   import §_-02q§.§_-4s§;
   import §_-23P§.§_-u1z§;
   import §_-62§.§_-T1y§;
   import §_-62§.§_-zF§;
   import §_-B1y§.§_-bs§;
   import §_-C2t§.§_-92W§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-F2Q§.GestureEvent;
   import §_-G16§.*;
   import §_-Ly§.§_-81L§;
   import §_-Tm§.§_-RF§;
   import §_-U1i§.§_-J1§;
   import §_-W§.§_-Ap§;
   import §_-Z2S§.§_-Ew§;
   import §_-Z2S§.§_-p19§;
   import §_-Z2S§.§_-yB§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-d21§;
   import feathers.controls.ScrollPolicy;
   import feathers.data.ListCollection;
   import flash.events.Event;
   import flash.geom.Rectangle;
   import flash.text.TextField;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.gui.controls.§_-NY§;
   import ru.pragmatix.flox.gui.controls.§_-zi§;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.*;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.response.InviteToClanResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanInviteTO;
   import ru.pragmatix.wormix.messages.client.ToggleTeamMember;
   import ru.pragmatix.wormix.messages.server.ToggleTeamMemberResult;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.platform.§_-WK§;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.SystemUtil;
   
   public class InviteToClanResponseBinarySerializer implements §_-D2L§
   {
      
      public function InviteToClanResponseBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:InviteToClanResponse = new InviteToClanResponse();
         _loc3_.serviceResult = param1.readShort();
         _loc3_.interopCode = param1.readInt();
         var _loc4_:uint = param1.readUnsignedShort();
         var _loc5_:uint = 0;
         while(_loc5_ < _loc4_)
         {
            param1.readShort();
            _loc3_.invites.push(§_-616§.§_-fk§(ClanInviteTO).deserializeStructure(param1,null));
            _loc5_++;
         }
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return ClanMessages.INVITE_TO_CLAN_RESPONSE;
      }
   }
}

