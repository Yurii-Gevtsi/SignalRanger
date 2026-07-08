package ru.pragmatix.wormix.serialization.client
{
   import §_-5Y§.*;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-a3§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-DJ§.§_-fH§;
   import §_-Y1O§.§_-42h§;
   import §_-YF§.§_-q2v§;
   import §_-j22§.§_-5g§;
   import §_-jF§.ClanInviteRenderer;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.hurlant.crypto.tls.§_-A3§;
   import feathers.controls.Button;
   import feathers.controls.ImageLoader;
   import feathers.controls.LayoutGroup;
   import feathers.controls.ScrollPolicy;
   import feathers.controls.ToggleButton;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.controls.renderers.IListItemRenderer;
   import feathers.data.ListCollection;
   import feathers.events.FeathersEventType;
   import feathers.layout.AnchorLayout;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.HorizontalLayout;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.themes.styles.AlternateButtonsStyles;
   import feathers.themes.styles.AlternateScrollBarStyles;
   import flash.net.Socket;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.clans.chat.*;
   import ru.pragmatix.wormix.messages.client.GoogleValidateInappPurchase;
   import ru.pragmatix.wormix.messages.server.SteamTransactionInitResponse;
   import ru.pragmatix.wormix.messages.structures.ShopItemStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurnResponse;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.§_-MN§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.core.Starling;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.utils.AssetManager;
   import starling.utils.ScaleMode;
   import starlingbuilder.engine.UIBuilder;
   
   public class PvpEndTurnResponseBinarySerializer implements §_-D2L§
   {
      
      public function PvpEndTurnResponseBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 1017;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:PvpEndTurnResponse = PvpEndTurnResponse(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeShort(_loc3_.turnNum);
         _loc5_.writeShort(_loc3_.commandNum);
         _loc5_.writeUnsignedInt(_loc3_.battleId);
         SerializationUtils.writeByteArray(_loc5_,_loc3_.eliminated);
         SerializationUtils.writeByteArray(_loc5_,_loc3_.collectedReagents);
         SerializationUtils.writeStructureArray(_loc5_,WeaponStructure,_loc3_.items);
         _loc5_.writeInt(_loc3_.battleState);
         SerializationUtils.writeByteArray(_loc5_,_loc3_.droppedUnits);
         SerializationUtils.writeShortArray(_loc5_,_loc3_.participantsHealthInPercent);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ByteArray = null;
         var _loc4_:PvpEndTurnResponse = null;
         _loc3_ = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         _loc4_ = new PvpEndTurnResponse();
         _loc4_.turnNum = _loc3_.readShort();
         _loc4_.commandNum = _loc3_.readShort();
         _loc4_.battleId = _loc3_.readUnsignedInt();
         _loc4_.eliminated = SerializationUtils.readByteArray(_loc3_);
         _loc4_.collectedReagents = SerializationUtils.readByteArray(_loc3_);
         _loc4_.items = SerializationUtils.readStructureArray(_loc3_,WeaponStructure);
         _loc4_.battleState = _loc3_.readInt();
         _loc4_.droppedUnits = SerializationUtils.readByteArray(_loc3_);
         _loc4_.participantsHealthInPercent = SerializationUtils.readShortArray(_loc3_);
         var _loc5_:Boolean = §_-U11§.isSecure(_loc3_,§_-6L§.§_-95§);
         if(!_loc5_)
         {
            §_-J2g§.§_-i1N§.dispose();
            §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.PROTOCOL_CHEATER),§_-fH§.§_-L2I§);
         }
         return _loc4_;
      }
   }
}

