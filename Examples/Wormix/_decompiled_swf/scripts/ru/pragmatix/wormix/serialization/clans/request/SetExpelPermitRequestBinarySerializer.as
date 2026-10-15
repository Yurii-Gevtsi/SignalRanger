package ru.pragmatix.wormix.serialization.clans.request
{
   import §_-Ly§.§_-I2K§;
   import §_-f1G§.§_-kf§;
   import dragonBones.animation.WorldClock;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.SetExpelPermitRequest;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.serialization.BaseCommandSerializer;
   import ru.pragmatix.wormix.weapons.*;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class SetExpelPermitRequestBinarySerializer extends BaseCommandSerializer
   {
      
      public function SetExpelPermitRequestBinarySerializer()
      {
         super();
      }
      
      override protected function §_-41X§(param1:Object) : ByteArray
      {
         var _loc2_:SetExpelPermitRequest = SetExpelPermitRequest(param1);
         var _loc3_:ByteArray = new ByteArray();
         _loc3_.writeInt(_loc2_.profileId);
         _loc3_.writeBoolean(_loc2_.value);
         return _loc3_;
      }
      
      override public function getCommandId() : uint
      {
         return ClanMessages.SET_EXPEL_PERMIT_REQUEST;
      }
   }
}

