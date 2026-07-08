package ru.pragmatix.wormix.serialization.clans.request
{
   import §_-5P§.§_-H1u§;
   import §_-5Y§.§_-h2c§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-a3§;
   import §_-82l§.§_-A2Y§;
   import §_-B2z§.§_-63i§;
   import §_-H3§.*;
   import §_-m2s§.§_-R11§;
   import §_-o14§.§_-Dd§;
   import com.distriqt.extension.cloudstorage.CloudStorage;
   import com.distriqt.extension.cloudstorage.events.KeyValueStoreEvent;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.hurlant.math.BigInteger;
   import com.hurlant.math.bi_internal;
   import flash.display.BitmapData;
   import flash.events.Event;
   import flash.utils.ByteArray;
   import ru.pragmatix.flox.social.utils.id.IPlatformAccountController;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.PostToClanChatRequest;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.pvp.messages.ex.ReconnectToBattle;
   import ru.pragmatix.wormix.pvp.messages.ex.StartPvpBattle;
   import ru.pragmatix.wormix.pvp.messages.server.BattleLoginError;
   import ru.pragmatix.wormix.pvp.messages.server.PvpRetryCommandRequestServer;
   import ru.pragmatix.wormix.serialization.BaseCommandSerializer;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   import starling.events.Event;
   
   use namespace bi_internal;
   
   public class PostToClanChatRequestBinarySerializer extends BaseCommandSerializer
   {
      
      public function PostToClanChatRequestBinarySerializer()
      {
         super();
      }
      
      override protected function §_-41X§(param1:Object) : ByteArray
      {
         var _loc2_:PostToClanChatRequest = PostToClanChatRequest(param1);
         var _loc3_:ByteArray = new ByteArray();
         _loc3_.writeUTF(_loc2_.text);
         return _loc3_;
      }
      
      override public function getCommandId() : uint
      {
         return ClanMessages.POST_TO_CHAT_REQUEST;
      }
   }
}

