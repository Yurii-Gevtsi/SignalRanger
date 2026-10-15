package ru.pragmatix.wormix.serialization.clans.request.edit
{
   import §_-5P§.§_-H1u§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-L1r§;
   import §_-Bv§.§_-G3P§;
   import §_-DJ§.§_-J2l§;
   import §_-YF§.§_-q2v§;
   import §_-n1w§.*;
   import §_-u13§.MD2;
   import §_-u13§.MD5;
   import §_-u13§.SHA1;
   import §_-u13§.§_-F3§;
   import §_-z1g§.§_-F4§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.hurlant.util.der.Sequence;
   import com.hurlant.util.der.§_-cz§;
   import com.hurlant.util.der.§_-l1b§;
   import feathers.core.FeathersControl;
   import feathers.data.ArrayCollection;
   import feathers.layout.TiledColumnsLayout;
   import flash.errors.IllegalOperationError;
   import flash.events.Event;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import ru.pragmatix.flox.social.utils.id.IPlatformAccountController;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.edit.ChangeClanDescriptionRequest;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.pvp.messages.ex.ReconnectToBattle;
   import ru.pragmatix.wormix.pvp.messages.ex.StartPvpBattle;
   import ru.pragmatix.wormix.pvp.messages.server.BattleLoginError;
   import ru.pragmatix.wormix.serialization.BaseCommandSerializer;
   import ru.pragmatix.wormix.utils.§_-2t§;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class ChangeClanDescriptionRequestBinarySerializer extends BaseCommandSerializer
   {
      
      public function ChangeClanDescriptionRequestBinarySerializer()
      {
         super();
      }
      
      override protected function §_-41X§(param1:Object) : ByteArray
      {
         var _loc2_:ChangeClanDescriptionRequest = ChangeClanDescriptionRequest(param1);
         var _loc3_:ByteArray = new ByteArray();
         _loc3_.writeUTF(_loc2_.description);
         _loc3_.writeBoolean(_loc2_.fromTreas);
         return _loc3_;
      }
      
      override public function getCommandId() : uint
      {
         return ClanMessages.CHANGE_CLAN_DESCRIPTION_REQUEST;
      }
   }
}

