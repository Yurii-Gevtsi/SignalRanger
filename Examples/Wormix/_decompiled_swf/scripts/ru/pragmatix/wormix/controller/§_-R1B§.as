package ru.pragmatix.wormix.controller
{
   import §_-73d§.§_-g1J§;
   import §_-B2z§.§_-63i§;
   import §_-C3f§.§_-4I§;
   import §_-CF§.§_-E19§;
   import §_-DJ§.§_-fH§;
   import §_-H2g§.§_-di§;
   import §_-T2y§.*;
   import §_-Z2S§.§_-b8§;
   import §_-aM§.*;
   import §_-hO§.§_-y2Z§;
   import §_-l1K§.§_-d2j§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-m0§.§_-v21§;
   import §_-s20§.§_-J2g§;
   import §_-z1g§.§_-C32§;
   import §_-z1g§.§_-lp§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.ScrollPolicy;
   import feathers.data.ListCollection;
   import flash.utils.ByteArray;
   import ru.pragmatix.flox.gui.controls.§_-NY§;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.response.TopClansResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.server.AwardGrantedMessage;
   import ru.pragmatix.wormix.messages.server.ClientSettings;
   import ru.pragmatix.wormix.messages.server.ShowSystemMessage;
   import ru.pragmatix.wormix.messages.structures.RentedItemsStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.social.utils.FacebookTracking;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   public class §_-R1B§
   {
      
      public function §_-R1B§()
      {
         super();
         §_-J2g§.§_-o1R§(ShowSystemMessage,this.§_-Yh§);
         §_-J2g§.§_-o1R§(AwardGrantedMessage,this.§_-Jw§);
         §_-J2g§.§_-o1R§(ClientSettings,this.§_-r18§);
      }
      
      public function §_-Yh§(param1:§_-63i§) : void
      {
         var _loc2_:ShowSystemMessage = ShowSystemMessage(param1.message);
         §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_SERVER_MSG),_loc2_.msg,§_-fH§.§_-1B§);
      }
      
      public function §_-Jw§(param1:§_-63i§) : void
      {
      }
      
      public function §_-r18§(param1:§_-63i§) : void
      {
         var _loc3_:String = null;
         var _loc4_:String = null;
         var _loc2_:ClientSettings = ClientSettings(param1.message);
         for(_loc3_ in _loc2_.list)
         {
            §_-C32§.setValue(_loc3_,_loc2_.getValue(_loc3_));
            §_-C32§.getBoolean(_loc3_);
         }
         FacebookTracking.isEnabled = false;
         _loc4_ = _loc2_.getValue(§_-C32§.§_-K2p§);
         if(_loc4_)
         {
            §_-X1j§.§_-j14§.isEnabled = _loc4_ == "true";
            §_-X1j§.§_-j14§.isEnabled = false;
         }
         var _loc5_:Boolean = §_-C32§.getBoolean(§_-C32§.§_-G2S§);
      }
   }
}

