package ru.pragmatix.wormix.serialization.clans.request
{
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-H2g§.§_-di§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-hO§.§_-03k§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-h2B§;
   import flash.events.TimerEvent;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.SetMuteModeRequest;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.serialization.BaseCommandSerializer;
   import ru.pragmatix.wormix.utils.FrameTimer;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.Shard;
   import ru.pragmatix.wormix.weapons.ammo.VerticalDrillMissile;
   import ru.pragmatix.wormix.weapons.ammo.VerticalDrillStanding;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.cluster.*;
   import starling.events.Event;
   
   public class SetMuteModeRequestBinarySerializer extends BaseCommandSerializer
   {
      
      public function SetMuteModeRequestBinarySerializer()
      {
         super();
      }
      
      override protected function §_-41X§(param1:Object) : ByteArray
      {
         var _loc2_:SetMuteModeRequest = SetMuteModeRequest(param1);
         var _loc3_:ByteArray = new ByteArray();
         _loc3_.writeInt(_loc2_.profileId);
         _loc3_.writeBoolean(_loc2_.value);
         return _loc3_;
      }
      
      override public function getCommandId() : uint
      {
         return ClanMessages.SET_MUTE_MODE_REQUEST;
      }
   }
}

