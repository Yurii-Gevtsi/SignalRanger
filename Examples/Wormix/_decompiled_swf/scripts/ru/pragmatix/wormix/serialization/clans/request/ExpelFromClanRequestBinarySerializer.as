package ru.pragmatix.wormix.serialization.clans.request
{
   import §_-B1y§.§_-bs§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-r1C§.§_-h2B§;
   import §_-z1g§.§_-4l§;
   import §_-z1g§.§_-522§;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.ExpelFromClanRequest;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.serialization.BaseCommandSerializer;
   import starling.core.Starling;
   
   public class ExpelFromClanRequestBinarySerializer extends BaseCommandSerializer
   {
      
      public function ExpelFromClanRequestBinarySerializer()
      {
         super();
      }
      
      override protected function §_-41X§(param1:Object) : ByteArray
      {
         var _loc2_:ExpelFromClanRequest = ExpelFromClanRequest(param1);
         var _loc3_:ByteArray = new ByteArray();
         _loc3_.writeInt(_loc2_.profileId);
         return _loc3_;
      }
      
      override public function getCommandId() : uint
      {
         return ClanMessages.EXPEL_FROM_CLAN_REQUEST;
      }
   }
}

