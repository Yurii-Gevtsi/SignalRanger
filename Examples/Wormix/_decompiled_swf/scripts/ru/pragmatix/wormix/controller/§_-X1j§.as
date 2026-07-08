package ru.pragmatix.wormix.controller
{
   import §_-02q§.§_-4s§;
   import §_-11L§.Archdemon;
   import §_-11L§.Hacker;
   import §_-11L§.Paladin;
   import §_-11L§.Phantoms;
   import §_-11L§.Thieves;
   import §_-11L§.§_-P25§;
   import §_-12W§.*;
   import §_-2U§.*;
   import §_-31N§.§_-42G§;
   import §_-31Y§.§_-D3B§;
   import §_-416§.§_-YS§;
   import §_-52L§.§_-u2x§;
   import §_-5P§.§_-52y§;
   import §_-5P§.§_-u2H§;
   import §_-61n§.§_-vB§;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-T1y§;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-ou§;
   import §_-62§.§_-zF§;
   import §_-63k§.§_-51T§;
   import §_-63k§.§_-hB§;
   import §_-73I§.*;
   import §_-931§.§_-i2r§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.§_-83E§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.Server;
   import §_-B2z§.§_-c1G§;
   import §_-CF§.§_-E19§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-73§;
   import §_-DJ§.§_-73y§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-F2M§.*;
   import §_-F39§.*;
   import §_-G2H§.*;
   import §_-H3§.§_-C1B§;
   import §_-J31§.§_-e2J§;
   import §_-L1H§.§_-C3e§;
   import §_-L2F§.§_-D2p§;
   import §_-Ly§.§_-7f§;
   import §_-Ly§.§_-81L§;
   import §_-RE§.*;
   import §_-S§.*;
   import §_-S1N§.§_-RO§;
   import §_-SP§.§_-M4§;
   import §_-Ta§.*;
   import §_-Y1O§.§_-42h§;
   import §_-Y1Q§.§_-61Z§;
   import §_-YF§.§_-q2v§;
   import §_-Yk§.*;
   import §_-Z1K§.§_-q24§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-Z1L§.§_-K2Z§;
   import §_-Z1w§.§_-s1x§;
   import §_-ZB§.§_-R2Y§;
   import §_-ZB§.§_-W2Q§;
   import §_-ZP§.§_-H1H§;
   import §_-ZP§.§_-w1J§;
   import §_-aM§.*;
   import §_-b1F§.§_-v2Q§;
   import §_-fo§.§_-hH§;
   import §_-gG§.§_-8u§;
   import §_-j22§.§_-5g§;
   import §_-k1u§.§_-F3H§;
   import §_-k1u§.§_-Oi§;
   import §_-k2s§.§_-b0§;
   import §_-k2s§.§_-e1U§;
   import §_-k2s§.§_-iI§;
   import §_-k2s§.§_-uU§;
   import §_-l1K§.§_-m2§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-lh§.§_-z2A§;
   import §_-m0§.*;
   import §_-nE§.SelectCraftWeaponScreen;
   import §_-nE§.§_-p16§;
   import §_-q11§.*;
   import §_-q26§.§_-Z25§;
   import §_-q2b§.§_-S2L§;
   import §_-q2b§.§_-x2T§;
   import §_-qH§.§_-S25§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-s1Q§.Artifacts;
   import §_-s1s§.§_-62d§;
   import §_-s20§.§_-J2g§;
   import §_-sQ§.§_-V1n§;
   import §_-t10§.§_-14§;
   import §_-t10§.§_-B29§;
   import §_-u1F§.§_-h14§;
   import §_-u1F§.§_-r2f§;
   import §_-u1T§.LocalizationService;
   import §_-u2J§.§_-A0§;
   import §_-v1A§.*;
   import §_-w2i§.§_-52k§;
   import §_-w2i§.§_-Zd§;
   import §_-wD§.§_-D3h§;
   import §_-wD§.§_-S1S§;
   import §_-wD§.§_-Y1t§;
   import §_-z1g§.§_-522§;
   import §_-z1g§.§_-M2v§;
   import §_-z1g§.§_-y1S§;
   import §_-zI§.§_-8D§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.distriqt.extension.inappbilling.events.ProductEvent;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.plugins.*;
   import config.§_-vR§;
   import feathers.controls.Button;
   import feathers.controls.ImageLoader;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.core.ITextRenderer;
   import feathers.data.ListCollection;
   import feathers.events.FeathersEventType;
   import flash.display.*;
   import flash.events.*;
   import flash.filesystem.File;
   import flash.geom.ColorTransform;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.net.SharedObject;
   import flash.text.TextField;
   import flash.text.TextFormat;
   import flash.utils.*;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.SocialAppContext;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.controller.mobile.BaseMobileSocialController;
   import ru.pragmatix.flox.social.email.EmailSenderFactory;
   import ru.pragmatix.flox.social.email.IEmailSender;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.flox.social.response.SocialResponse;
   import ru.pragmatix.flox.social.utils.id.IPlatformAccountController;
   import ru.pragmatix.wormix.controller.analytics.AnalyticsControllerFactory;
   import ru.pragmatix.wormix.gui.*;
   import ru.pragmatix.wormix.gui.menu.§_-W1G§;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanInviteStructure;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.structures.EndBattleStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.model.items.craft.§_-6S§;
   import ru.pragmatix.wormix.model.rewards.§_-P2q§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.pvp.messages.client.PvpRetryCommandRequestClient;
   import ru.pragmatix.wormix.pvp.messages.ex.RejectBattleOffer;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.serialization.Serializers;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-71v§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.AtomBomb;
   import ru.pragmatix.wormix.weapons.ammo.LaserRay;
   import ru.pragmatix.wormix.weapons.ammo.LightningBullet;
   import ru.pragmatix.wormix.weapons.ammo.phase.PoisonPhase;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Quad;
   import starling.display.Sprite;
   import starling.display.Stage;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.AssetManager;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   public class §_-X1j§ extends flash.events.EventDispatcher
   {
      
      public static var context:SocialAppContext;
      
      public static var serverTime:int;
      
      public static var §_-Q9§:int;
      
      public static var §_-P2t§:§_-33s§;
      
      public static var §_-73H§:§_-R1B§;
      
      public static var §_-ns§:§_-J2g§;
      
      public static var §_-It§:IPlatformAccountController;
      
      public static var §_-31p§:IEmailSender;
      
      public static var §_-p1c§:§_-52y§;
      
      public static var §_-41H§:§_-Yp§;
      
      public static var §_-zU§:§_-W2C§;
      
      public static var §_-02s§:§_-MJ§;
      
      public static var §_-Rr§:§_-K2f§;
      
      public static var §_-Y2W§:§_-J2d§;
      
      public static var §_-D3H§:§_-I2Y§;
      
      public static var §_-t1T§:§_-41c§;
      
      public static var §_-a2G§:§_-Y18§;
      
      public static var §_-H15§:§_-21j§;
      
      public static var §_-A24§:§_-Xh§;
      
      public static var §_-12n§:§_-D8§;
      
      public static var §_-41U§:§_-C1B§;
      
      public static var §_-B35§:§_-z1O§;
      
      public static var §_-k2F§:§_-D3h§;
      
      public static var §_-i2n§:§_-v2§;
      
      public static var §_-82T§:§_-b1g§;
      
      public static var §_-dr§:§_-Z2p§;
      
      public static var §_-FI§:§_-aK§;
      
      public static var §_-G1a§:§_-A2V§;
      
      public static var §_-61i§:§_-dp§;
      
      public static var §_-2I§:§_-hB§;
      
      public static var §_-sO§:§_-YS§;
      
      public static var §_-w24§:§_-Q1V§;
      
      public static var §_-f2i§:§_-Z21§;
      
      private static var §_-W1s§:§_-b1s§;
      
      public static var §_-61v§:§_-B29§;
      
      public static var §_-d1s§:§_-h14§;
      
      public static var §_-D1T§:§_-S2L§;
      
      public static var §_-j14§:§_-W2Q§;
      
      public static var localizationService:LocalizationService;
      
      private static var §_-O1q§:Serializers = new Serializers();
      
      public static var §_-b2y§:§_-D33§ = new §_-D33§();
      
      public static var §_-G3m§:§_-F2G§ = new §_-F2G§();
      
      public static var §_-hl§:§_-Z25§ = new §_-Z25§();
      
      private static var §_-Ld§:§_-V1n§ = new §_-V1n§(0);
      
      public static var §_-71i§:§_-K2Z§ = new §_-K2Z§();
      
      private static var isDisposed:Boolean = true;
      
      public function §_-X1j§(param1:SocialAppContext)
      {
         super();
         §_-X1j§.context = param1;
      }
      
      public static function get randomSeed() : §_-V1n§
      {
         return §_-Ld§;
      }
      
      public static function §_-7I§() : void
      {
         if(!§_-dr§)
         {
            §_-dr§ = new §_-Z2p§();
         }
      }
      
      public static function §_-nT§() : void
      {
         §_-N1V§.settings.§_-r1u§();
         var _loc1_:§_-u2H§ = §_-X1j§.§_-p1c§;
         var _loc2_:IPlatformAccountController = §_-X1j§.§_-It§;
         if(!§_-k2F§)
         {
            if(§_-N1V§.settings.§_-l9§())
            {
               §_-k2F§ = new §_-Y1t§(_loc2_,_loc1_);
            }
            else
            {
               §_-k2F§ = new §_-S1S§();
            }
         }
      }
      
      public static function get socialController() : BaseMobileSocialController
      {
         return §_-N1V§.socialController;
      }
      
      public static function get §_-42k§() : §_-R2Y§
      {
         return §_-N1V§.§_-42k§;
      }
      
      public static function §_-E1P§() : flash.display.Stage
      {
         return Application.§_-i1N§.getInstance().stage;
      }
      
      public static function §_-B3j§() : starling.display.Stage
      {
         return Starling.current.stage;
      }
      
      public static function §_-NA§(param1:String) : void
      {
         Application.§_-NA§(param1);
      }
      
      public function init() : void
      {
         §_-6L§.§_-95§ = §_-X1j§.context.§_-E1e§().secretKey;
         Application.§_-i1N§.dispatchEventWith(§_-A0§.PROGRESS,false,0);
         var _loc1_:§_-o1p§ = new §_-o1p§(context);
         _loc1_.addEventListener(flash.events.Event.COMPLETE,this.§_-e1q§);
         _loc1_.init();
      }
      
      private function §_-e1q§(param1:flash.events.Event) : void
      {
         var _loc2_:§_-o1p§ = param1.target as §_-o1p§;
         _loc2_.removeEventListener(flash.events.Event.COMPLETE,this.§_-e1q§);
         §_-It§ = _loc2_.§_-It§;
         §_-vR§.§_-uA§(§_-It§.getUserId());
         §_-82T§ = new §_-b1g§();
         localizationService = new LocalizationService(§_-82T§);
         §_-H15§ = new §_-21j§();
         §_-FI§ = new §_-aK§();
         §_-j14§ = AnalyticsControllerFactory.create(§_-It§.getPlatformType());
         §_-j14§.setUserId(§_-It§.getUserId());
         §_-31p§ = EmailSenderFactory.create();
         §_-N1V§.create();
         §_-N1V§.addListener(flash.events.Event.COMPLETE,this.§_-p1I§);
         §_-N1V§.init(context);
      }
      
      private function §_-p1I§(param1:Object) : void
      {
         §_-N1V§.removeListener(flash.events.Event.COMPLETE,this.§_-p1I§);
         §_-41H§ = new §_-Yp§();
         §_-41H§.addEventListener(flash.events.Event.COMPLETE,this.§_-Y2h§,false,0,true);
         §_-41H§.addEventListener(§_-hH§.LOAD_ERROR,this.onError,false,0,true);
         §_-7I§();
         SystemUtil.executeWhenApplicationIsActive(§_-41H§.init);
         isDisposed = false;
      }
      
      public function dispose() : void
      {
         if(isDisposed)
         {
            return;
         }
         isDisposed = true;
         if(§_-d1s§)
         {
            §_-d1s§.dispose();
         }
         §_-d1s§ = null;
         if(§_-D1T§)
         {
            §_-D1T§.dispose();
         }
         §_-D1T§ = null;
         if(§_-61v§)
         {
            §_-61v§.dispose();
         }
         §_-61v§ = null;
         if(§_-p1c§)
         {
            §_-p1c§.dispose();
         }
         §_-p1c§ = null;
         if(§_-41H§)
         {
            §_-41H§.dispose();
         }
         §_-41H§ = null;
         if(§_-zU§)
         {
            §_-zU§.dispose();
         }
         §_-zU§ = null;
         if(§_-02s§)
         {
            §_-02s§.dispose();
         }
         §_-02s§ = null;
         if(§_-D3H§)
         {
            §_-D3H§.dispose();
         }
         §_-D3H§ = null;
         if(§_-H15§)
         {
            §_-H15§.dispose();
         }
         §_-H15§ = null;
         if(§_-dr§)
         {
            §_-dr§.dispose();
         }
         §_-dr§ = null;
         if(§_-FI§)
         {
            §_-FI§.dispose();
         }
         §_-FI§ = null;
         if(§_-A24§)
         {
            §_-A24§.dispose();
         }
         §_-A24§ = null;
         if(§_-12n§)
         {
            §_-12n§.dispose();
         }
         §_-12n§ = null;
         if(§_-41U§)
         {
            §_-41U§.dispose();
         }
         §_-41U§ = null;
         if(§_-Rr§)
         {
            §_-Rr§.dispose();
         }
         §_-Rr§ = null;
         if(§_-P2t§)
         {
            §_-P2t§.dispose();
         }
         §_-P2t§ = null;
         if(§_-j14§)
         {
            §_-j14§.dispose();
         }
         §_-j14§ = null;
         if(§_-k2F§)
         {
            §_-k2F§.dispose();
         }
         §_-k2F§ = null;
         if(§_-ns§)
         {
            §_-ns§.dispose();
         }
         §_-ns§ = null;
         §_-O1q§ = null;
         context = null;
         §_-73H§ = null;
         §_-73H§ = null;
         §_-Y2W§ = null;
         §_-t1T§ = null;
         §_-a2G§ = null;
         §_-B35§ = null;
         §_-i2n§ = null;
         §_-G1a§ = null;
         §_-61i§ = null;
         §_-W1s§ = null;
      }
      
      private function check() : void
      {
         if(§_-Yp§.isValid)
         {
            return;
         }
         var _loc1_:ByteArray = new ByteArray();
         var _loc2_:§_-5g§ = new §_-5g§();
         _loc2_.§_-c2D§(70 + 17);
         var _loc3_:ByteArray = new ByteArray();
         _loc3_.writeUTF(§_-J2g§.§_-i1N§.§_-yt§());
         _loc3_.writeShort(42 + 7);
         _loc3_.writeUTF("C" + "F" + "G" + int(Math.random() * 10000).toString());
         §_-U11§.§_-E2n§(_loc3_,§_-6L§.§_-95§);
         _loc2_.setLength(_loc3_.length);
         _loc2_.write(_loc1_);
         _loc1_.writeBytes(_loc3_);
         _loc1_.position = 0;
         §_-J2g§.§_-22R§(_loc1_);
      }
      
      private function §_-Y2h§(param1:flash.events.Event) : void
      {
         §_-41H§.removeEventListener(flash.events.Event.COMPLETE,this.§_-Y2h§);
         §_-41H§.removeEventListener(§_-hH§.LOAD_ERROR,this.onError);
         Application.§_-i1N§.dispatchEventWith(§_-A0§.PROGRESS,false,85);
         this.§_-WO§();
         §_-ns§ = new §_-J2g§(Server.§_-O2A§(§_-c1G§.MAIN));
         §_-83E§.single(§_-ns§,flash.events.Event.INIT,this.§_-g2t§);
         §_-ns§.init();
      }
      
      private function §_-g2t§(param1:flash.events.Event) : void
      {
         §_-83E§.single(§_-ns§,flash.events.Event.CONNECT,this.§_-u24§);
         §_-ns§.§_-p1e§();
      }
      
      private function §_-u24§(param1:flash.events.Event) : void
      {
         §_-w24§ = new §_-Q1V§(§_-N1V§.settings);
         §_-f2i§ = new §_-Z21§();
         §_-61i§ = new §_-dp§();
         Application.§_-i1N§.dispatchEventWith(§_-A0§.PROGRESS,false,90);
         §_-73H§ = new §_-R1B§();
         §_-P2t§ = new §_-33s§();
         §_-N1V§.§_-t1N§(socialController);
         §_-W1s§ = new §_-b1s§();
         §_-02s§ = new §_-MJ§();
         §_-Rr§ = new §_-K2f§();
         §_-Y2W§ = new §_-J2d§();
         §_-D3H§ = new §_-I2Y§();
         §_-t1T§ = new §_-41c§();
         §_-a2G§ = new §_-Y18§();
         §_-41U§ = new §_-C1B§();
         §_-A24§ = new §_-Xh§();
         §_-12n§ = new §_-D8§();
         §_-B35§ = new §_-z1O§();
         §_-zU§ = new §_-W2C§();
         §_-G1a§ = new §_-A2V§();
         §_-b2y§.create();
         §_-D1T§ = §_-x2T§.create();
         §_-d1s§ = §_-r2f§.create();
         §_-61v§ = §_-14§.create();
         §_-p1c§ = new §_-52y§();
         §_-83E§.single(§_-p1c§,flash.events.Event.COMPLETE,this.§_-OS§);
         §_-83E§.single(§_-p1c§,ErrorEvent.ERROR,this.onError);
         §_-p1c§.login();
      }
      
      private function §_-U2h§() : void
      {
      }
      
      private function §_-OS§(param1:flash.events.Event) : void
      {
         Application.§_-i1N§.dispatchEventWith(§_-A0§.PROGRESS,false,99);
         this.check();
         if(!§_-2I§)
         {
            §_-2I§ = §_-51T§.create();
         }
         this.dispatchEvent(new flash.events.Event(flash.events.Event.COMPLETE,true));
         §_-nT§();
         §_-hl§.init(§_-J2g§.§_-i1N§,§_-p1c§);
      }
      
      private function onError(param1:flash.events.Event) : void
      {
         this.dispatchEvent(new ErrorEvent(ErrorEvent.ERROR,true));
      }
      
      private function §_-WO§() : void
      {
         §_-7f§.LOADING = §_-s2a§.LOADING;
         §_-7f§.BUTTON_OK = §_-s2a§.BUTTON_OK;
         §_-7f§.BUTTON_CANCEL = §_-s2a§.BUTTON_CANCEL;
         §_-7f§.BUTTON_YES = §_-s2a§.BUTTON_YES;
         §_-81L§.§_-WH§ = §_-d27§.§_-b2H§;
         §_-81L§.§_-o1Z§ = §_-d27§.§_-BA§;
         §_-81L§.§_-I3X§ = §_-q2v§.§_-E§;
         §_-81L§.§_-jn§ = §_-d27§.§_-S29§;
         §_-81L§.§_-B3D§ = §_-d27§.§_-k2x§;
         §_-81L§.§_-D1q§ = §_-d27§.§_-S29§;
         §_-81L§.§_-TH§ = §_-d27§.§_-A7§;
         §_-81L§.§_-q2C§ = §_-d27§.§_-md§;
         §_-81L§.§_-Cu§ = §_-d27§.§_-F1z§;
         §_-81L§.§_-B3D§ = null;
         §_-81L§.§_-YC§ = 0;
         §_-81L§.§_-a1e§ = 0.3;
         §_-81L§.§_-sq§ = false;
         §_-81L§.§_-N2p§ = true;
         §_-81L§.§_-a1Z§ = true;
         §_-h2B§.§_-kK§ = false;
      }
   }
}

