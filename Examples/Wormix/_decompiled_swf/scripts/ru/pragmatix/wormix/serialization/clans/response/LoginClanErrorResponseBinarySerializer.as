package ru.pragmatix.wormix.serialization.clans.response
{
   import §_-02q§.§_-4s§;
   import §_-21p§.§_-4w§;
   import §_-63U§.ProgressBarBoss;
   import §_-63U§.§_-21K§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-C3f§.§_-1Y§;
   import §_-C3f§.§_-Z1Y§;
   import §_-D3A§.*;
   import §_-DJ§.§_-fH§;
   import §_-F2Q§.GestureEvent;
   import §_-H3§.*;
   import §_-L1H§.§_-b2K§;
   import §_-TF§.§_-q1j§;
   import §_-Z1K§.*;
   import §_-g2r§.§_-03A§;
   import §_-j22§.§_-5g§;
   import §_-j2R§.§_-73G§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import com.distriqt.extension.pushnotifications.PushNotifications;
   import config.§_-vR§;
   import feathers.controls.LayoutGroup;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.ILayoutDisplayObject;
   import feathers.layout.RelativePosition;
   import flash.events.Event;
   import flash.text.TextField;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.gui.controls.§_-zi§;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.§_-3m§;
   import ru.pragmatix.wormix.gui.menu.shop.*;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanErrorResponse;
   import ru.pragmatix.wormix.messages.client.EndTurn;
   import ru.pragmatix.wormix.messages.client.GetProfiles;
   import ru.pragmatix.wormix.messages.client.StartBattle;
   import ru.pragmatix.wormix.messages.server.VerifyPaymentResult;
   import ru.pragmatix.wormix.messages.structures.TurnStructure;
   import ru.pragmatix.wormix.model.items.craft.§_-13q§;
   import ru.pragmatix.wormix.serialization.client.*;
   import starling.display.Image;
   import starling.events.Event;
   
   public class LoginClanErrorResponseBinarySerializer implements §_-D2L§
   {
      
      public function LoginClanErrorResponseBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:LoginClanErrorResponse = new LoginClanErrorResponse();
         _loc3_.serviceResult = param1.readShort();
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return ClanMessages.LOGIN_ERROR_RESPONSE;
      }
   }
}

