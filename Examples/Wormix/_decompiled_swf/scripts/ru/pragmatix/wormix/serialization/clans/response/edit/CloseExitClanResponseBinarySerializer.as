package ru.pragmatix.wormix.serialization.clans.response.edit
{
   import §_-533§.§_-c2b§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-zF§;
   import §_-T2V§.§_-13F§;
   import §_-Z1K§.§_-q24§;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import dragonBones.animation.WorldClock;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.controls.system.ControlSystems;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.response.edit.CloseExitClanResponse;
   import ru.pragmatix.wormix.messages.server.AddToGroupResult;
   import ru.pragmatix.wormix.messages.server.ProfilesResult;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.serialization.server.*;
   import ru.pragmatix.wormix.weapons.ammo.DynamiteStick;
   import starling.events.Event;
   import starling.utils.AssetManager;
   
   public class CloseExitClanResponseBinarySerializer implements §_-D2L§
   {
      
      public function CloseExitClanResponseBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:CloseExitClanResponse = new CloseExitClanResponse();
         _loc3_.serviceResult = param1.readShort();
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return ClanMessages.CHANGE_CLAN_CLOSE_EXIT_RESPONSE;
      }
   }
}

