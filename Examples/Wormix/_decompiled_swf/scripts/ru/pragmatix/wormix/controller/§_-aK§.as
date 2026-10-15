package ru.pragmatix.wormix.controller
{
   import §_-11L§.*;
   import §_-5Y§.§_-h2c§;
   import §_-62§.§_-W1r§;
   import §_-63M§.§_-F3f§;
   import §_-73I§.§_-K2s§;
   import §_-CF§.§_-x2t§;
   import §_-H2g§.§_-di§;
   import §_-K3r§.§_-E3O§;
   import §_-P2i§.§_-b7§;
   import §_-QD§.§_-G2z§;
   import §_-Ta§.*;
   import §_-Y1O§.*;
   import §_-Y1Q§.§_-aG§;
   import §_-Y1Q§.§_-tc§;
   import §_-Y1b§.§_-OP§;
   import §_-Z1K§.§_-q24§;
   import §_-aM§.*;
   import §_-fo§.§_-r5§;
   import §_-k1u§.§_-Oi§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-o14§.§_-Dd§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-D2L§;
   import §_-z1g§.§_-lp§;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.greensock.loading.*;
   import com.hurlant.math.BigInteger;
   import dragonBones.animation.WorldClock;
   import feathers.core.FeathersControl;
   import flash.display.Bitmap;
   import flash.display.MovieClip;
   import flash.events.Event;
   import flash.events.TimerEvent;
   import flash.geom.Point;
   import flash.ui.Keyboard;
   import flash.utils.ByteArray;
   import flash.utils.Timer;
   import flash.utils.getDefinitionByName;
   import ru.pragmatix.flox.serialization.*;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.messages.clans.request.AuditActionsRequest;
   import ru.pragmatix.wormix.messages.clans.response.AuditActionsResponse;
   import ru.pragmatix.wormix.messages.client.AppleVerifyReceipt;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.rewards.§_-d2F§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.core.Starling;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.KeyboardEvent;
   
   public class §_-aK§
   {
      
      public function §_-aK§()
      {
         super();
         Starling.current.stage.addEventListener(KeyboardEvent.KEY_DOWN,this.onKeyDown);
      }
      
      private function onKeyDown(param1:KeyboardEvent) : void
      {
         switch(param1.keyCode)
         {
            case Keyboard.ESCAPE:
            case Keyboard.BACK:
               param1.preventDefault();
               this.§_-xo§();
         }
      }
      
      public function §_-i2w§() : void
      {
         this.§_-xo§();
      }
      
      private function §_-xo§() : void
      {
         if(Application.§_-i1N§.visible)
         {
            §_-YQ§.§_-71H§();
         }
      }
      
      public function dispose() : void
      {
         Starling.current.stage.removeEventListener(KeyboardEvent.KEY_DOWN,this.onKeyDown);
      }
   }
}

