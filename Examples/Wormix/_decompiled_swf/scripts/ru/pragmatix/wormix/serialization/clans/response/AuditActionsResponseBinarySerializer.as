package ru.pragmatix.wormix.serialization.clans.response
{
   import §_-5Y§.§_-h2c§;
   import §_-62§.§_-T1y§;
   import §_-82l§.§_-A2Y§;
   import §_-B1y§.§_-F1P§;
   import §_-C3f§.§_-4I§;
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-x2t§;
   import §_-CR§.§_-72p§;
   import §_-DJ§.§_-73§;
   import §_-DJ§.§_-73y§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-F39§.*;
   import §_-Tm§.§_-RF§;
   import §_-Vg§.ClanCreateView;
   import §_-Y1Q§.§_-aG§;
   import §_-Y1Q§.§_-tc§;
   import §_-YF§.§_-q2v§;
   import §_-g2C§.GlobalChatTextMessageItemRenderer;
   import §_-g2C§.§_-62U§;
   import §_-g2C§.§_-I1R§;
   import §_-g2C§.§_-K2A§;
   import §_-g2C§.§_-K2u§;
   import §_-g2C§.§_-p8§;
   import §_-g2C§.§_-w1K§;
   import §_-r1C§.*;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-z1g§.§_-F4§;
   import §_-zI§.§_-8D§;
   import §_-zI§.§_-F3q§;
   import com.distriqt.extension.application.Application;
   import feathers.controls.ScrollPolicy;
   import feathers.controls.renderers.IListItemRenderer;
   import feathers.layout.AnchorLayout;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.Direction;
   import feathers.layout.RelativePosition;
   import feathers.layout.VerticalLayout;
   import feathers.themes.styles.AlternateScrollBarStyles;
   import feathers.utils.textures.TextureCache;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.response.LoadFriendsResponse;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.response.AuditActionsResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanAuditActionTO;
   import ru.pragmatix.wormix.messages.structures.ChatMessage;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.§_-g20§;
   import starling.display.Button;
   import starling.events.Event;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   public class AuditActionsResponseBinarySerializer implements §_-D2L§
   {
      
      public function AuditActionsResponseBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:AuditActionsResponse = new AuditActionsResponse();
         _loc3_.serviceResult = param1.readShort();
         var _loc4_:uint = param1.readUnsignedShort();
         var _loc5_:uint = 0;
         while(_loc5_ < _loc4_)
         {
            param1.readShort();
            _loc3_.actions[_loc5_] = ClanAuditActionTO(§_-616§.§_-fk§(ClanAuditActionTO).deserializeStructure(param1,null));
            _loc5_++;
         }
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return ClanMessages.AUDIT_ACTIONS_RESPONSE;
      }
   }
}

