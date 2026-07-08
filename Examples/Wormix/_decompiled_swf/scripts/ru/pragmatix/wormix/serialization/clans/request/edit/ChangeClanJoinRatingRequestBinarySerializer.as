package ru.pragmatix.wormix.serialization.clans.request.edit
{
   import §_-63U§.§_-21K§;
   import §_-82a§.*;
   import §_-CF§.§_-E19§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-L1H§.§_-C3e§;
   import §_-k2s§.§_-Go§;
   import §_-k2s§.§_-iI§;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.edit.ChangeClanJoinRatingRequest;
   import ru.pragmatix.wormix.scene.tile.AtlasTileData;
   import ru.pragmatix.wormix.serialization.BaseCommandSerializer;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.textures.TextureAtlas;
   import starling.utils.AssetManager;
   
   public class ChangeClanJoinRatingRequestBinarySerializer extends BaseCommandSerializer
   {
      
      public function ChangeClanJoinRatingRequestBinarySerializer()
      {
         super();
      }
      
      override protected function §_-41X§(param1:Object) : ByteArray
      {
         var _loc2_:ChangeClanJoinRatingRequest = ChangeClanJoinRatingRequest(param1);
         var _loc3_:ByteArray = new ByteArray();
         _loc3_.writeInt(_loc2_.joinRating);
         return _loc3_;
      }
      
      override public function getCommandId() : uint
      {
         return ClanMessages.CHANGE_CLAN_JOIN_RATING_REQUEST;
      }
   }
}

