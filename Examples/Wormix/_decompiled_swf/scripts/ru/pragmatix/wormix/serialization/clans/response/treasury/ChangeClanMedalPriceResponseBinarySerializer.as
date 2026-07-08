package ru.pragmatix.wormix.serialization.clans.response.treasury
{
   import §_-31W§.*;
   import §_-B1y§.*;
   import §_-C2M§.DesktopTutorialPanel;
   import §_-C2M§.MobileTutorialPanel;
   import §_-C2M§.§_-o2f§;
   import §_-F39§.§_-Ey§;
   import §_-J3L§.§_-h1D§;
   import §_-T2h§.*;
   import §_-U1i§.§_-hK§;
   import §_-Vg§.MainClanWindow;
   import §_-Vg§.§_-K3K§;
   import §_-d26§.§_-d1H§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.hurlant.math.BigInteger;
   import feathers.controls.Button;
   import feathers.controls.ButtonState;
   import feathers.controls.ToggleButton;
   import feathers.utils.touch.TapToTrigger;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.utils.id.MobilePlatformAccountController;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-fT§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginClanBase;
   import ru.pragmatix.wormix.messages.clans.response.treasury.ChangeClanMedalPriceResponse;
   import ru.pragmatix.wormix.messages.client.GetWhoPumpedReaction;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.utils.SystemUtil;
   
   public class ChangeClanMedalPriceResponseBinarySerializer implements §_-D2L§
   {
      
      public function ChangeClanMedalPriceResponseBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ChangeClanMedalPriceResponse = new ChangeClanMedalPriceResponse();
         _loc3_.serviceResult = param1.readShort();
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return ClanMessages.CHANGE_CLAN_MEDAL_PRICE_RESPONSE;
      }
   }
}

