package ru.pragmatix.wormix.controller
{
   import §_-02q§.§_-4s§;
   import §_-12a§.*;
   import §_-21p§.§_-4w§;
   import §_-23P§.*;
   import §_-2U§.*;
   import §_-2k§.§_-51r§;
   import §_-2k§.§_-m2i§;
   import §_-31W§.*;
   import §_-416§.§_-YS§;
   import §_-52L§.§_-u2x§;
   import §_-5P§.§_-H1u§;
   import §_-5Y§.§_-h2c§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-L1r§;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-T1y§;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-a3§;
   import §_-62§.§_-ou§;
   import §_-62§.§_-zF§;
   import §_-72j§.§_-Z6§;
   import §_-72j§.§_-i2G§;
   import §_-73I§.§_-K2s§;
   import §_-73I§.§_-q15§;
   import §_-82l§.§_-1I§;
   import §_-9§.§_-52A§;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-A2U§.§_-A23§;
   import §_-B1y§.§_-F1P§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.§_-63i§;
   import §_-Bv§.§_-G3P§;
   import §_-C2t§.*;
   import §_-CF§.*;
   import §_-Cl§.*;
   import §_-D3A§.*;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-T2G§;
   import §_-DJ§.§_-fH§;
   import §_-E1I§.§_-b2n§;
   import §_-F39§.§_-r2l§;
   import §_-F3k§.§_-d1U§;
   import §_-G16§.*;
   import §_-H2g§.§_-di§;
   import §_-H2g§.§_-s1d§;
   import §_-H3§.*;
   import §_-J31§.§_-R1h§;
   import §_-K35§.§_-p1m§;
   import §_-L1H§.§_-C3e§;
   import §_-L1H§.§_-b2K§;
   import §_-L2F§.§_-I1q§;
   import §_-L2F§.§_-t2Z§;
   import §_-L2F§.§_-xF§;
   import §_-Ly§.§_-I2K§;
   import §_-N8§.§_-o2w§;
   import §_-RE§.§_-V1m§;
   import §_-TF§.§_-13z§;
   import §_-TF§.§_-J3K§;
   import §_-TF§.§_-Sq§;
   import §_-TF§.§_-q1j§;
   import §_-TF§.§_-t2r§;
   import §_-Ta§.*;
   import §_-Tm§.§_-RF§;
   import §_-U1i§.§_-hK§;
   import §_-Vg§.*;
   import §_-Y1O§.§_-V23§;
   import §_-Y1b§.§_-OP§;
   import §_-YF§.*;
   import §_-Z1K§.§_-M6§;
   import §_-Z1K§.§_-q24§;
   import §_-Z1w§.§_-s1x§;
   import §_-ZB§.§_-32D§;
   import §_-aM§.*;
   import §_-d2p§.*;
   import §_-f1G§.§_-kf§;
   import §_-fo§.AbstractConfigLoader;
   import §_-fo§.§_-21L§;
   import §_-fo§.§_-hH§;
   import §_-fo§.§_-r5§;
   import §_-g2r§.§_-03A§;
   import §_-j1w§.§_-L1x§;
   import §_-j1w§.§_-lD§;
   import §_-j22§.§_-5g§;
   import §_-k1u§.§_-Vr§;
   import §_-l1K§.§_-d2j§;
   import §_-l1g§.§_-L3Q§;
   import §_-m0§.§_-X1F§;
   import §_-o14§.§_-Dd§;
   import §_-r1C§.§_-V1R§;
   import §_-r1C§.§_-h2B§;
   import §_-r1C§.§_-t1R§;
   import §_-rM§.§_-02j§;
   import §_-s1Q§.§_-W2D§;
   import §_-s20§.§_-J2g§;
   import §_-sQ§.§_-737§;
   import §_-u13§.MAC;
   import §_-w2i§.§_-Zd§;
   import §_-x22§.§_-xY§;
   import §_-x2Q§.§_-dU§;
   import §_-z1g§.§_-C32§;
   import §_-zI§.§_-X1q§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.*;
   import com.greensock.loading.core.*;
   import com.hurlant.crypto.*;
   import com.hurlant.math.BigInteger;
   import com.worlize.websocket.§_-x2j§;
   import config.§_-B1K§;
   import config.§_-J1p§;
   import config.§_-J1r§;
   import config.§_-W1a§;
   import config.§_-vR§;
   import config.§_-xD§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.ImageLoader;
   import feathers.controls.LayoutGroup;
   import feathers.controls.ProgressBar;
   import feathers.controls.SimpleScrollBar;
   import feathers.controls.text.TextInstanceCreator;
   import feathers.core.FeathersControl;
   import feathers.core.ITextRenderer;
   import feathers.events.FeathersEventType;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.ILayout;
   import feathers.layout.RelativePosition;
   import feathers.layout.TiledRowsLayout;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.themes.FontColor;
   import feathers.themes.FontSize;
   import feathers.themes.WormixMobileThemeWithAssetManager;
   import flash.desktop.NativeApplication;
   import flash.display.DisplayObject;
   import flash.display.DisplayObjectContainer;
   import flash.display.StageDisplayState;
   import flash.events.Event;
   import flash.events.EventDispatcher;
   import flash.events.SecurityErrorEvent;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.media.SoundTransform;
   import flash.system.LoaderContext;
   import flash.text.TextField;
   import flash.text.TextFormatAlign;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.getTimer;
   import org.gestouch.core.gestouch_internal;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.controller.ISocialController;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.model.IUserProfile;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.flox.social.utils.id.IPlatformAccountController;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.analytics.Analytics;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.gui.*;
   import ru.pragmatix.wormix.gui.menu.§_-3m§;
   import ru.pragmatix.wormix.gui.menu.§_-P1h§;
   import ru.pragmatix.wormix.gui.menu.shop.*;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.edit.ChangeClanDescriptionRequest;
   import ru.pragmatix.wormix.messages.clans.response.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.Disconnect;
   import ru.pragmatix.wormix.messages.client.EndBattle;
   import ru.pragmatix.wormix.messages.client.JoinToChatResult;
   import ru.pragmatix.wormix.messages.client.SelectStuff;
   import ru.pragmatix.wormix.messages.client.StartBattle;
   import ru.pragmatix.wormix.messages.server.ChatMessageEvent;
   import ru.pragmatix.wormix.messages.server.EnterAccount;
   import ru.pragmatix.wormix.messages.server.LoginError;
   import ru.pragmatix.wormix.messages.server.ShowSystemMessage;
   import ru.pragmatix.wormix.messages.server.UserIsBanned;
   import ru.pragmatix.wormix.messages.structures.SelectStuffStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.model.items.craft.§_-IE§;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-i14§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.UnitPhysicModel;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurn;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurnResponse;
   import ru.pragmatix.wormix.pvp.messages.ex.ReconnectToBattle;
   import ru.pragmatix.wormix.pvp.messages.ex.StartPvpBattle;
   import ru.pragmatix.wormix.pvp.messages.server.BattleLoginError;
   import ru.pragmatix.wormix.scene.water.Water;
   import ru.pragmatix.wormix.serialization.client.*;
   import ru.pragmatix.wormix.social.*;
   import ru.pragmatix.wormix.utils.§_-6A§;
   import ru.pragmatix.wormix.utils.§_-72u§;
   import ru.pragmatix.wormix.utils.§_-F2L§;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.IPooledAmmo;
   import ru.pragmatix.wormix.weapons.ammo.phase.HealBolt;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.cluster.ShellBurst;
   import starling.core.Starling;
   import starling.display.Button;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Quad;
   import starling.display.Sprite;
   import starling.display.Stage;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.utils.Align;
   import starling.utils.Color;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   import starling.utils.SystemUtil;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   use namespace gestouch_internal;
   
   public class §_-Yp§ extends EventDispatcher
   {
      
      public static var isValid:Boolean = true;
      
      private static var §_-mB§:Boolean = false;
      
      private static const §_-333§:String = "data/";
      
      private static const §_-29§:String = §_-333§ + "lang/platform/";
      
      private static const §_-RW§:String = §_-333§ + "lang/common/";
      
      private static const §_-C10§:String = "resconfig.xml";
      
      private var §_-a2d§:§_-r5§;
      
      private var §_-q1r§:§_-616§;
      
      public var §_-G21§:§_-52A§;
      
      private var §_-g2E§:§_-J1y§;
      
      private var §_-32Y§:Function;
      
      private var §_-92I§:Boolean;
      
      public function §_-Yp§()
      {
         super();
      }
      
      public static function §_-x27§() : String
      {
         return §_-333§;
      }
      
      public function init() : void
      {
         var _loc1_:String = null;
         §_-I2K§.init();
         if(§_-r5§.instance == null)
         {
            _loc1_ = §_-C10§;
            if(!§_-X1j§.context.§_-g2d§().isLocal())
            {
               _loc1_ = this.§_-Z1v§(_loc1_,§_-X1j§.context.getResConfigVersion());
            }
            this.§_-a2d§ = new §_-r5§(this.§_-OH§ + §_-333§,this.§_-O26§ + §_-333§,new <String>[_loc1_],this.§_-D3K§);
            this.§_-a2d§.load(this.§_-v16§,this.§_-t2k§,{
               "pathParams":{"social":this.§_-C2S§()},
               "wormActions":{"lazy":true},
               "news":{"lazy":!§_-N1V§.§_-E3l§()}
            },SystemUtil.isAIR);
         }
         else
         {
            this.§_-v16§();
         }
      }
      
      public function §_-I2B§(param1:Function) : void
      {
         this.§_-32Y§ = param1;
         this.§_-92I§ = true;
         this.§_-xv§();
      }
      
      private function §_-v16§() : void
      {
         var _loc1_:§_-21L§ = new §_-21L§();
         _loc1_.§_-11t§(Water);
         §_-r5§.§_-f18§("Water",_loc1_);
         var _loc2_:Boolean = §_-616§.getInstance() != null;
         this.§_-q1r§ = _loc2_ ? §_-616§.getInstance() : new §_-616§(§_-X1j§.context.§_-K3U§(),this.§_-O26§,this.§_-D3K§);
         this.§_-L1§(this.§_-q1r§,!_loc2_,this.§_-wk§,this.§_-t2k§);
      }
      
      private function §_-wk§(param1:flash.events.Event = null) : void
      {
         if(param1)
         {
            this.§_-J27§(param1.target,this.§_-wk§,this.§_-t2k§);
            §_-FJ§.§_-Q1E§(this.§_-xv§);
         }
         else
         {
            this.onComplete(null);
         }
      }
      
      private function §_-xv§() : void
      {
         §_-FJ§.§_-y1K§(this.§_-Z1t§,this.§_-Z1t§);
      }
      
      private function §_-Z1t§() : void
      {
         var _loc2_:Vector.<String> = null;
         var _loc1_:Boolean = §_-52A§.§_-Ji§() != null;
         if(_loc1_)
         {
            this.§_-G21§ = §_-52A§.§_-Ji§();
            if(this.§_-92I§)
            {
               this.§_-G21§.setLanguage(§_-X1j§.§_-82T§.§_-I9§());
            }
         }
         else
         {
            _loc2_ = Vector.<String>(this.§_-Ek§());
            this.§_-G21§ = new §_-52A§(§_-X1j§.§_-82T§.§_-I9§(),this.§_-OH§,this.§_-O26§,this.§_-D3K§,_loc2_,§_-X1j§.context.getLangFilesVersion());
         }
         this.§_-L1§(this.§_-G21§,!_loc1_ || this.§_-92I§,this.§_-83V§,this.§_-t2k§);
      }
      
      private function §_-83V§(param1:flash.events.Event) : void
      {
         if(param1)
         {
            this.§_-J27§(param1.target,this.§_-wk§,this.§_-t2k§);
         }
         var _loc2_:Boolean = §_-r2l§.getInstance() != null;
         this.§_-g2E§ = new §_-J1y§(§_-X1j§.§_-82T§.§_-I9§(),this.§_-OH§,this.§_-O26§,this.§_-D3K§,new <String>[this.§_-jR§() + "taunts"]);
         this.§_-L1§(this.§_-g2E§,!_loc2_ || this.§_-92I§,this.§_-11D§,this.§_-t2k§);
      }
      
      private function §_-11D§(param1:flash.events.Event) : void
      {
         if(param1)
         {
            this.§_-J27§(param1.target,this.§_-wk§,this.§_-t2k§);
         }
         §_-r2l§.init(this.§_-g2E§.§_-S2w§,this.§_-g2E§.§_-z2f§,this.§_-g2E§.§_-Q17§);
         §_-X1j§.§_-sO§ = new §_-YS§(this.§_-OH§,this.§_-O26§,this.§_-A2G§(),this.§_-D3K§);
         this.§_-L1§(§_-X1j§.§_-sO§,true,this.§_-82§,this.§_-t2k§);
      }
      
      private function §_-82§(param1:flash.events.Event) : void
      {
         if(this.§_-92I§)
         {
            if(this.§_-32Y§ != null)
            {
               this.§_-32Y§();
            }
            this.§_-92I§ = false;
         }
         else if(!§_-L29§.§_-c2R§)
         {
            §_-L29§.§_-Le§(this.§_-Z11§);
         }
         else
         {
            this.§_-u2g§();
         }
      }
      
      private function §_-Z11§() : void
      {
         var _loc1_:WormixMobileThemeWithAssetManager = new WormixMobileThemeWithAssetManager(Application.assetManager,"spriteSheet_menu");
         this.§_-A2§();
      }
      
      private function §_-A2§() : void
      {
         AmmosFactory.getInstance().createPoolOfObjects(AmmosFactory.OBJECT_BULLET,20);
         AmmosFactory.getInstance().createPoolOfObjects(AmmosFactory.OBJECT_BURNING_OIL,50);
         this.onComplete(null);
      }
      
      private function errorHandler(param1:flash.events.Event) : void
      {
      }
      
      private function check() : Boolean
      {
         try
         {
            return !§_-KF§.validate();
         }
         catch(e:Error)
         {
            return true;
         }
      }
      
      private function onComplete(param1:flash.events.Event) : void
      {
         var _loc2_:§_-32D§ = null;
         if(!§_-mB§)
         {
            _loc2_ = §_-N1V§.§_-I39§;
            §_-y27§.§_-51u§(new §_-J1r§().chars,§_-lD§.§_-I3U§());
            §_-y27§.§_-V1U§(_loc2_.§_-3X§());
            §_-y27§.§_-p1J§(_loc2_.§_-12I§(),_loc2_.§_-O12§());
            §_-y27§.§_-51S§(_loc2_.§_-d2c§(),§_-G1p§.§_-S1n§());
            §_-y27§.§_-51S§(new §_-M6§(),§_-G1p§.§_-S1n§());
            §_-y27§.§_-T1C§(_loc2_.§_-Xc§().§_-uO§(),§_-Y2u§.§_-uO§(),§_-IE§);
            §_-y27§.§_-T1C§(_loc2_.§_-OG§().§_-h1o§(),§_-Y2u§.§_-h1o§(),§_-W1r§);
            §_-y27§.§_-T1C§(new §_-V1m§().§_-r2§(),§_-I1q§.§_-QB§(),§_-xF§);
            §_-y27§.§_-T1C§(new §_-d1U§().§_-M1w§,§_-I1q§.§_-x9§(),§_-t2Z§);
            §_-y27§.§_-T1C§(new §_-W2D§().artifacts,§_-G1p§.§_-x1a§(),§_-T1y§);
            §_-y27§.§_-T1C§(§_-J1p§.§_-q1O§,§_-G1p§.§_-M1n§(),§_-a3§);
            §_-y27§.§_-o2j§(§_-W1a§.§_-62Q§);
            §_-Y2u§.§_-c1i§(§_-N1V§.§_-I39§.§_-R1m§());
            §_-mB§ = true;
         }
         if(this.check())
         {
            isValid = false;
         }
         this.§_-u2g§();
      }
      
      private function §_-u2g§() : void
      {
         this.dispatchEvent(new flash.events.Event(flash.events.Event.COMPLETE,true));
      }
      
      private function §_-L1§(param1:AbstractConfigLoader, param2:Boolean, param3:Function, param4:Function) : void
      {
         if(param2)
         {
            this.§_-Q5§(param1,param3,param4);
            param1.load();
         }
         else
         {
            param3();
         }
      }
      
      private function §_-Q5§(param1:AbstractConfigLoader, param2:Function, param3:Function) : void
      {
         param1.addEventListener(flash.events.Event.COMPLETE,param2,false,0,true);
         param1.addEventListener(§_-hH§.LOAD_ERROR,param3,false,0,true);
      }
      
      private function §_-J27§(param1:Object, param2:Function, param3:Function) : void
      {
         param1.removeEventListener(flash.events.Event.COMPLETE,param2);
         param1.removeEventListener(§_-hH§.LOAD_ERROR,param3);
      }
      
      private function get §_-OH§() : String
      {
         return §_-X1j§.context.§_-K3U§();
      }
      
      private function get §_-O26§() : String
      {
         return §_-X1j§.context.§_-C1Y§();
      }
      
      private function get §_-D3K§() : LoaderContext
      {
         return §_-X1j§.context.§_-21I§();
      }
      
      public function §_-Ek§() : Array
      {
         var _loc1_:String = this.§_-jR§();
         var _loc2_:String = this.§_-e1j§();
         var _loc3_:String = this.§_-b1O§(§_-X1j§.§_-It§.getPlatformType());
         var _loc4_:Array = this.§_-L3K§(_loc1_,["boss.achievments","items.messages","help","date","scenario","upgrades","bot.names","ban","clan","plural"]);
         _loc4_ = _loc4_.concat(this.§_-L3K§(_loc3_,["messages","achievments","craft","tutorials","weapons","system","platform","viral.messages"]));
         if(§_-N1V§.settings.§_-bB§())
         {
            _loc4_ = _loc4_.concat(this.§_-L3K§(_loc2_,["avatar.messages"]));
         }
         if(§_-N1V§.§_-E3l§())
         {
            _loc4_ = _loc4_.concat(this.§_-L3K§(_loc2_,["news"]));
         }
         return _loc4_;
      }
      
      private function §_-L3K§(param1:String, param2:Array) : Array
      {
         var _loc4_:String = null;
         var _loc3_:Array = [];
         for each(_loc4_ in param2)
         {
            _loc3_.push(param1 + _loc4_);
         }
         return _loc3_;
      }
      
      private function §_-A2G§() : Vector.<String>
      {
         var _loc2_:§_-p2U§ = null;
         var _loc1_:Vector.<String> = new Vector.<String>();
         _loc1_.push(§_-333§ + "common/chat.xml");
         for each(_loc2_ in §_-N1V§.settings.§_-L2e§())
         {
            _loc1_.push(this.§_-Pl§() + "chat_" + _loc2_.§_-b2§() + ".xml");
         }
         return _loc1_;
      }
      
      private function §_-jR§() : String
      {
         return §_-RW§;
      }
      
      private function §_-Pl§() : String
      {
         return §_-RW§;
      }
      
      private function §_-e1j§() : String
      {
         return §_-29§ + "common/";
      }
      
      private function §_-b1O§(param1:SocialType) : String
      {
         switch(param1)
         {
            case SocialType.STEAM:
               return §_-29§ + "desktop/";
            case SocialType.MOBILE_ANDROID:
            case SocialType.MOBILE_IOS:
               return §_-29§ + "mobile/";
            default:
               throw new Error("Get current platform lang folder error. Wrong platform social type.");
         }
      }
      
      private function §_-C2S§() : String
      {
         return "mobile";
      }
      
      private function §_-t2k§(param1:§_-hH§) : void
      {
         this.dispatchEvent(param1);
      }
      
      private function §_-Z1v§(param1:String, param2:String, param3:String = ".xml") : String
      {
         var _loc4_:int = param1.indexOf(param3);
         if(_loc4_ != -1)
         {
            return param1.slice(0,_loc4_) + param2 + param3;
         }
         return param1;
      }
      
      public function dispose() : void
      {
         this.§_-a2d§ = null;
         this.§_-q1r§ = null;
         this.§_-G21§ = null;
         this.§_-g2E§ = null;
      }
   }
}

