package ru.pragmatix.wormix.gui.menu
{
   import §_-2U§.*;
   import §_-31N§.§_-42G§;
   import §_-31Y§.§_-D3B§;
   import §_-62§.*;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-Af§.§_-k1I§;
   import §_-B1y§.§_-F1P§;
   import §_-C2t§.§_-92W§;
   import §_-C3f§.§_-1Y§;
   import §_-C3f§.§_-yG§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-fH§;
   import §_-L1H§.§_-C3e§;
   import §_-Ly§.§_-81L§;
   import §_-TF§.§_-I1E§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1b§.*;
   import §_-Z1K§.§_-q24§;
   import §_-aM§.*;
   import §_-b1F§.§_-G4§;
   import §_-b1F§.§_-v2Q§;
   import §_-j1w§.§_-L1x§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-q26§.*;
   import §_-qH§.§_-S25§;
   import §_-r1C§.§_-h2B§;
   import §_-r1C§.§_-t1R§;
   import §_-rM§.§_-61j§;
   import §_-s1Q§.Artifacts;
   import §_-s20§.§_-J2g§;
   import §_-z1g§.§_-F4§;
   import §_-z1g§.§_-M2v§;
   import com.hurlant.math.BigInteger;
   import com.hurlant.math.bi_internal;
   import flash.events.IOErrorEvent;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.clearTimeout;
   import flash.utils.getTimer;
   import org.gestouch.core.*;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.FontColor;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginClanBase;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanEnterAccount;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanErrorResponse;
   import ru.pragmatix.wormix.messages.client.BuyBattle;
   import ru.pragmatix.wormix.messages.client.BuyShopItems;
   import ru.pragmatix.wormix.messages.server.VerifyPaymentResult;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.messages.structures.ShopItemStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.serialization.client.*;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.LaserRay;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   use namespace bi_internal;
   
   public class §_-i2l§ extends §_-T1z§
   {
      
      protected var §_-F11§:§_-t1R§;
      
      public function §_-i2l§(param1:String)
      {
         super();
         this.§_-Y1L§(param1 == "default" ? §_-81L§.§_-o1Z§ : new §_-t1R§(param1));
         addEventListener(Event.TRIGGERED,this.playSound);
      }
      
      override protected function feathersControl_addedToStageHandler(param1:Event) : void
      {
         if(this.stage)
         {
            super.feathersControl_addedToStageHandler(param1);
         }
      }
      
      private function playSound(param1:Event) : void
      {
         if(this.§_-F11§)
         {
            §_-h2B§.play(this.§_-F11§);
         }
      }
      
      public function §_-Y1L§(param1:§_-t1R§) : void
      {
         this.§_-F11§ = param1;
      }
      
      override public function dispose() : void
      {
         removeEventListener(Event.TRIGGERED,this.playSound);
         this.§_-F11§ = null;
      }
   }
}

