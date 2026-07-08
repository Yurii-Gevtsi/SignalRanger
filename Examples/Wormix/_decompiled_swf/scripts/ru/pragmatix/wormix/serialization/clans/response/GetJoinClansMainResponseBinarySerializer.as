package ru.pragmatix.wormix.serialization.clans.response
{
   import §_-12W§.*;
   import §_-31W§.§_-r26§;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-o6§;
   import §_-B2z§.§_-63i§;
   import §_-CF§.§_-x2t§;
   import §_-CR§.§_-72p§;
   import §_-K35§.§_-99§;
   import §_-P1B§.§_-b21§;
   import §_-P1B§.§_-bb§;
   import §_-TF§.§_-q1j§;
   import §_-Vg§.*;
   import §_-aM§.*;
   import §_-jF§.ClanMemberItemRenderer;
   import §_-jF§.§_-U17§;
   import §_-k1u§.§_-Oi§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-z1g§.*;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.hurlant.util.§_-a2P§;
   import feathers.controls.ScrollPolicy;
   import feathers.data.ArrayCollection;
   import feathers.layout.AnchorLayout;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.RelativePosition;
   import feathers.layout.TiledColumnsLayout;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.themes.styles.AlternateButtonsStyles;
   import flash.ui.GameInputDevice;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.*;
   import ru.pragmatix.wormix.controls.GamepadControlsMap;
   import ru.pragmatix.wormix.fonts.FontColor;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-eP§;
   import ru.pragmatix.wormix.gui.screenOverlay.view.*;
   import ru.pragmatix.wormix.messages.clans.response.GetJoinClansMainResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.GetProfiles;
   import ru.pragmatix.wormix.messages.client.GoogleValidateInappPurchase;
   import ru.pragmatix.wormix.messages.server.UpgradeWeaponResult;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.items.craft.§_-6S§;
   import ru.pragmatix.wormix.model.items.craft.§_-IE§;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-WU§;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.pvp.messages.client.PvpDropPlayer;
   import ru.pragmatix.wormix.serialization.client.*;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-Cq§;
   import starling.core.Starling;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class GetJoinClansMainResponseBinarySerializer implements §_-D2L§
   {
      
      public function GetJoinClansMainResponseBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:GetJoinClansMainResponse = new GetJoinClansMainResponse();
         var _loc4_:uint = param1.readUnsignedShort();
         var _loc5_:uint = 0;
         while(_loc5_ < _loc4_)
         {
            param1.readShort();
            _loc3_.clans.push(§_-616§.§_-fk§(ClanTO).deserializeStructure(param1,null));
            _loc5_++;
         }
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return 1098;
      }
   }
}

