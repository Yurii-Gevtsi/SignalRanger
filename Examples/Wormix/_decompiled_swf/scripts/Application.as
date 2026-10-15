package
{
   import §_-02q§.§_-4s§;
   import §_-21p§.§_-4w§;
   import §_-2U§.*;
   import §_-31W§.§_-g16§;
   import §_-31W§.§_-r26§;
   import §_-51p§.§_-W2e§;
   import §_-52L§.§_-u2x§;
   import §_-5Y§.§_-h2c§;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-T1y§;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-ou§;
   import §_-62§.§_-zF§;
   import §_-9e§.§_-73e§;
   import §_-9e§.§_-A18§;
   import §_-A1K§.§_-TA§;
   import §_-A2F§.§_-4B§;
   import §_-A2U§.§_-A23§;
   import §_-B1y§.Cookie;
   import §_-B1y§.§_-F1P§;
   import §_-B1y§.§_-K2e§;
   import §_-B2z§.Server;
   import §_-B2z§.§_-c1G§;
   import §_-C2M§.*;
   import §_-CF§.§_-gE§;
   import §_-CF§.§_-x2t§;
   import §_-CR§.§_-S2o§;
   import §_-Cl§.*;
   import §_-D1Z§.§_-925§;
   import §_-D1Z§.§_-k2n§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-T2G§;
   import §_-DJ§.§_-fH§;
   import §_-G2H§.*;
   import §_-H2g§.*;
   import §_-J2G§.§_-P26§;
   import §_-J2G§.§_-og§;
   import §_-J3L§.§_-h1D§;
   import §_-K3r§.§_-E3O§;
   import §_-Ly§.§_-81L§;
   import §_-P1B§.§_-b21§;
   import §_-T1o§.§_-n1S§;
   import §_-T2h§.§_-92J§;
   import §_-TF§.§_-q1j§;
   import §_-Ta§.*;
   import §_-Vg§.MainClanWindow;
   import §_-Vg§.§_-K3K§;
   import §_-W§.§_-mK§;
   import §_-W§.§_-y1q§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1b§.§_-OP§;
   import §_-Y23§.§_-ZE§;
   import §_-YF§.§_-43M§;
   import §_-YF§.§_-L29§;
   import §_-YF§.§_-q2v§;
   import §_-d26§.§_-d1H§;
   import §_-hO§.SupplyPickedUpEvent;
   import §_-j1y§.§_-i1b§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-o14§.§_-Dd§;
   import §_-r1C§.§_-U2p§;
   import §_-r1C§.§_-h2B§;
   import §_-s20§.§_-J2g§;
   import §_-sQ§.§_-H3D§;
   import §_-u1T§.§_-33B§;
   import §_-u2J§.§_-T1K§;
   import §_-w2W§.§_-21w§;
   import §_-w2i§.§_-Ia§;
   import §_-x2Q§.§_-dU§;
   import §_-xu§.§_-91H§;
   import §_-z1g§.§_-F4§;
   import §_-z1g§.§_-U2k§;
   import air.net.SocketMonitor;
   import com.distriqt.extension.inappbilling.Product;
   import com.distriqt.extension.inappbilling.SubscriptionOffer;
   import com.distriqt.extension.inappbilling.SubscriptionPhase;
   import com.distriqt.extension.inappbilling.events.ProductEvent;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.hurlant.math.BigInteger;
   import config.§_-vR§;
   import dragonBones.animation.WorldClock;
   import dragonBones.starling.StarlingFactory;
   import feathers.controls.Button;
   import feathers.controls.ButtonState;
   import feathers.controls.LayoutGroup;
   import feathers.controls.List;
   import feathers.controls.ScreenNavigator;
   import feathers.controls.ToggleButton;
   import feathers.core.IToolTipManager;
   import feathers.core.PopUpManager;
   import feathers.core.ToolTipManager;
   import feathers.data.ArrayCollection;
   import feathers.events.FeathersEventType;
   import feathers.layout.AnchorLayout;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.HorizontalLayout;
   import feathers.layout.ILayout;
   import feathers.layout.RelativeDepth;
   import feathers.layout.TiledColumnsLayout;
   import feathers.layout.TiledRowsLayout;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import flash.desktop.NativeApplication;
   import flash.display.BitmapData;
   import flash.display.DisplayObject;
   import flash.display.DisplayObjectContainer;
   import flash.events.ErrorEvent;
   import flash.events.Event;
   import flash.events.MouseEvent;
   import flash.events.RemoteNotificationEvent;
   import flash.events.StatusEvent;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.media.AudioPlaybackMode;
   import flash.media.SoundMixer;
   import flash.text.TextField;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import flash.utils.setTimeout;
   import ru.pragmatix.flox.social.SocialAppContext;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.§_-w1N§;
   import ru.pragmatix.wormix.controller.§_-32w§;
   import ru.pragmatix.wormix.controller.§_-KF§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controller.§_-v2§;
   import ru.pragmatix.wormix.controller.§_-yM§;
   import ru.pragmatix.wormix.fx.ArmatureAuraEffect;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.*;
   import ru.pragmatix.wormix.gui.menu.§_-3m§;
   import ru.pragmatix.wormix.gui.menu.§_-P1h§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.RegisterForNotify;
   import ru.pragmatix.wormix.messages.server.BuyRaceResult;
   import ru.pragmatix.wormix.messages.structures.CostStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.rewards.§_-d2F§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-71v§;
   import ru.pragmatix.wormix.utils.§_-72u§;
   import ru.pragmatix.wormix.utils.§_-F2L§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.utils.§_-o19§;
   import ru.pragmatix.wormix.utils.§_-t2B§;
   import ru.pragmatix.wormix.utils.§_-y1V§;
   import ru.pragmatix.wormix.weapons.Girder;
   import ru.pragmatix.wormix.weapons.Weapon;
   import starling.Starlings;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Quad;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.textures.RenderTexture;
   import starling.textures.Texture;
   import starling.utils.AssetManager;
   import starling.utils.MathUtil;
   import starling.utils.Pool;
   import starling.utils.ScaleMode;
   import starling.utils.SystemUtil;
   import starlingbuilder.engine.CustomUiBuilder;
   import starlingbuilder.engine.tween.WormixTweenBuilder;
   
   public class Application extends Sprite implements §_-K2e§
   {
      
      public static var §_-i1N§:Application;
      
      public static var factory:StarlingFactory;
      
      public static var assetManager:AssetManager;
      
      public static var uiBuilder:CustomUiBuilder;
      
      public static var §_-E1k§:§_-E3O§;
      
      public static var §_-F23§:ScreenNavigator;
      
      public static var §_-YB§:§_-og§;
      
      public static var §_-ow§:§_-z2G§;
      
      public static var §_-Y1V§:§_-32w§;
      
      private static var §_-B1E§:§_-yM§;
      
      private static var §_-u1J§:§_-X1j§;
      
      private static var _context:§_-a16§;
      
      private static var §_-np§:§_-o19§;
      
      private static var §_-z2g§:UserProfile;
      
      private static var _scene:§_-w1N§;
      
      private static var §_-437§:Boolean;
      
      private static var §_-D3G§:Function;
      
      public static const FRAME_RATE:int = §_-vR§.fps;
      
      private static const §_-J6§:String = "wormix-cookie";
      
      private static var §_-7x§:Starlings = new Starlings();
      
      private static var §_-n1A§:§_-r1d§ = new §_-r1d§();
      
      private var context:SocialAppContext;
      
      private var §_-q1t§:Boolean;
      
      private var §_-Ti§:Boolean = false;
      
      private var isUserIgnoring:Boolean = false;
      
      public function Application()
      {
         super();
      }
      
      public static function §_-NA§(param1:String) : void
      {
      }
      
      private static function §_-H8§() : void
      {
         if(§_-437§)
         {
            §_-h2B§.§_-I2V§([§_-d27§.§_-tE§,§_-d27§.§_-22k§]);
            §_-h2B§.§_-l2z§();
         }
         Application.§_-a1k§(false);
         if(§_-D3G§ != null)
         {
            §_-i1N§.§_-Ti§ = true;
            §_-D3G§();
            §_-D3G§ = null;
         }
         §_-i1N§.§_-o2p§();
      }
      
      private static function §_-wO§() : void
      {
         §_-E1k§.activeScreen.addEventListener(starling.events.Event.COMPLETE,§_-32T§);
      }
      
      private static function §_-32T§(param1:starling.events.Event) : void
      {
         §_-E1k§.activeScreen.removeEventListener(starling.events.Event.COMPLETE,§_-32T§);
         Application.§_-i1N§.§_-V6§(§_-D3G§,§_-21w§.§_-01l§);
      }
      
      public static function §_-42J§() : Boolean
      {
         return §_-i1N§.§_-Ti§;
      }
      
      public static function §_-dv§() : Boolean
      {
         return §_-i1N§.§_-q1t§;
      }
      
      public static function §_-a1k§(param1:Boolean) : void
      {
         §_-i1N§.§_-q1t§ = param1;
      }
      
      public static function isUserIgnoring() : Boolean
      {
         return §_-i1N§.isUserIgnoring;
      }
      
      public static function §_-dq§(param1:Boolean) : void
      {
         §_-i1N§.isUserIgnoring = param1;
      }
      
      public static function get userProfile() : UserProfile
      {
         return §_-z2g§;
      }
      
      public static function set userProfile(param1:UserProfile) : void
      {
         if(Boolean(§_-z2g§) && §_-z2g§.§_-33n§())
         {
            §_-z2g§.§_-g2M§();
         }
         if(param1.§_-33n§())
         {
            param1.§_-dP§();
         }
         §_-z2g§ = param1;
         if(§_-n1A§.§_-x2k§)
         {
            §_-n1A§.§_-82K§(param1);
         }
         Application.§_-i1N§.dispatchEventWith(§_-n1S§.RECREATED);
      }
      
      public static function get scene() : §_-w1N§
      {
         return _scene ? _scene : §_-X1j§.§_-12n§.scene;
      }
      
      public static function set scene(param1:§_-w1N§) : void
      {
         _scene = param1;
      }
      
      public static function get §_-SH§() : §_-r1d§
      {
         return §_-n1A§;
      }
      
      public static function get isActive() : Boolean
      {
         return Boolean(§_-np§) && §_-np§.§_-b2l§;
      }
      
      public function start() : void
      {
         var _assetMediator:§_-43M§;
         var content:LayoutGroup;
         var platformStarter:§_-k2n§ = §_-925§.§_-o1h§();
         platformStarter.start();
         if(!Cookie.isInitialized())
         {
            Cookie.init(§_-J6§);
            §_-t2B§.§_-j2Z§();
            §_-y1V§.onStart();
            §_-U2k§.instance.§_-R1u§();
         }
         §_-i1N§ = this;
         _context = new §_-a16§(this);
         §_-Y1V§ = new §_-32w§();
         §_-B1E§ = new §_-yM§(Starling.current);
         §_-X1j§.§_-7I§();
         §_-81L§.§_-FZ§(this);
         this.context = new SocialAppContext();
         this.§_-Ti§ = false;
         assetManager = new AssetManager(1,false);
         §_-L29§.init(assetManager);
         _assetMediator = new §_-43M§(Application.assetManager);
         uiBuilder = new CustomUiBuilder(_assetMediator,false,null,null,new WormixTweenBuilder());
         §_-np§ = new §_-o19§();
         §_-ow§ = new §_-z2G§();
         this.addChild(§_-ow§);
         content = new LayoutGroup();
         content.addChild(§_-E1k§);
         content.addChild(§_-F23§);
         §_-ow§.content = content;
         §_-ow§.openMode = RelativeDepth.ABOVE;
         §_-ow§.isEnabled = false;
         §_-YB§ = new §_-og§();
         this.addChild(§_-YB§);
         PopUpManager.popUpManagerFactory = §_-P26§.factory;
         PopUpManager.root = §_-YB§;
         this.§_-63V§(§_-qN§.§_-L2b§);
         if(SystemUtil.isDesktop)
         {
            ToolTipManager.toolTipManagerFactory = function(param1:starling.display.DisplayObjectContainer):IToolTipManager
            {
               return new §_-F3B§(param1);
            };
            ToolTipManager.setEnabledForStage(stage,true);
         }
         RenderTexture.useDoubleBuffering = true;
         setTimeout(§_-E1k§.§_-MO§,100,§_-21w§.§_-lv§,this.§_-f24§);
      }
      
      private function §_-f24§(... rest) : void
      {
         §_-np§.§_-F1x§();
         SoundMixer.audioPlaybackMode = AudioPlaybackMode.AMBIENT;
         this.§_-02P§();
      }
      
      private function §_-02P§() : void
      {
         §_-u1J§ = new §_-X1j§(this.context);
         §_-u1J§.addEventListener(ErrorEvent.ERROR,function(param1:flash.events.Event):void
         {
            dispatchEventWith(ErrorEvent.ERROR);
         },false,0,true);
         §_-u1J§.addEventListener(flash.events.Event.COMPLETE,this.§_-l2U§,false,0,true);
         §_-u1J§.init();
      }
      
      private function §_-l2U§(param1:flash.events.Event) : void
      {
         var event:flash.events.Event = param1;
         §_-ow§.isEnabled = true;
         §_-X1j§.§_-G1a§.§_-5T§();
         if(§_-v2§.§_-63e§())
         {
            §_-X1j§.§_-i2n§ = new §_-v2§();
            §_-X1j§.§_-i2n§.addEventListener(flash.events.Event.COMPLETE,this.§_-i1g§);
            §_-X1j§.§_-i2n§.addEventListener(ErrorEvent.ERROR,function(param1:flash.events.Event):void
            {
               dispatchEventWith(ErrorEvent.ERROR);
            });
            §_-X1j§.§_-i2n§.§_-63j§();
         }
         else
         {
            this.initScene();
         }
         Starling.current.showStats = §_-vR§.§_-pD§;
      }
      
      private function §_-i1g§(param1:flash.events.Event) : void
      {
         var event:flash.events.Event = param1;
         if(§_-v2§.§_-72F§())
         {
            this.initScene(true,function():void
            {
               §_-v2§.§_-e2K§(function():void
               {
                  initScene(!§_-v2§.§_-72F§());
                  §_-v2§.§_-u2w§();
               },null);
            });
         }
         else
         {
            this.initScene();
         }
      }
      
      private function initScene(param1:Boolean = true, param2:Function = null) : void
      {
         §_-X1j§.§_-p1c§.§_-72O§();
         if(!§_-X1j§.§_-p1c§.isSecure())
         {
            §_-J2g§.§_-i1N§.dispose();
            §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.PROTOCOL_CHEATER),§_-fH§.§_-L2I§);
            return;
         }
         if(param1)
         {
            this.§_-V6§(param2);
         }
      }
      
      public function §_-V6§(param1:Function = null, param2:Function = null) : void
      {
         this.§_-Ti§ = false;
         §_-437§ = Application.§_-E1k§.activeScreenID != §_-21w§.§_-G31§;
         §_-D3G§ = param1;
         if(§_-E1k§.activeScreenID == §_-21w§.BATTLE_SCENE)
         {
            §_-J2l§.§_-u2b§.show(Application.§_-E1k§.§_-MO§,§_-21w§.BATTLE_CLEANING,§_-wO§,§_-21w§.§_-01l§);
         }
         else
         {
            §_-E1k§.§_-MO§(§_-21w§.§_-G31§,§_-H8§,param2);
         }
      }
      
      public function §_-i1d§() : void
      {
         if(§_-N1V§.§_-42k§)
         {
            §_-N1V§.§_-42k§.§_-I19§();
         }
      }
      
      public function §_-o2p§() : void
      {
         §_-F2n§.§_-h1w§();
      }
      
      public function §_-F3l§() : void
      {
         NativeApplication.nativeApplication.exit();
         if(§_-X1j§.§_-61v§)
         {
            §_-X1j§.§_-61v§.§_-u2Q§(function():void
            {
            });
         }
      }
      
      public function restart() : void
      {
         §_-a1k§(true);
         §_-J2l§.§_-u2b§.show(this.§_-92w§);
      }
      
      private function §_-92w§() : void
      {
         §_-E1k§.§_-MO§(§_-21w§.§_-lv§,function():void
         {
            §_-J2l§.§_-u2b§.hide();
            §_-Ti§ = false;
            §_-h2B§.§_-s2U§();
            §_-T2G§.§_-912§();
            §_-J2g§.§_-i1N§.dispose();
            §_-u1J§.dispose();
            §_-N1V§.dispose();
            setTimeout(§_-02P§,1000);
         });
      }
      
      private function §_-63V§(param1:Object) : void
      {
         var _loc2_:Object = param1;
         this.context.§_-63V§(_loc2_);
         this.context.setSocialType(SocialType.valueOf(_loc2_.socialType));
         this.context.setResConfigVersion("");
         this.context.§_-T2e§({});
         Server.§_-f1y§(§_-vR§.§_-L1W§,§_-c1G§.MAIN);
         Server.§_-f1y§(§_-vR§.§_-wA§,§_-c1G§.PVP);
         Server.§_-f1y§(§_-vR§.§_-wA§,§_-c1G§.PVP);
         var _loc3_:Server = Server.§_-O2A§(§_-c1G§.PVP);
         Server.§_-WJ§();
      }
      
      public function get stageWidth() : int
      {
         return Starling.current.stage.stageWidth;
      }
      
      public function get stageHeight() : int
      {
         return Starling.current.stage.stageHeight;
      }
      
      public function getInstance() : flash.display.DisplayObjectContainer
      {
         return Starling.current.nativeStage;
      }
      
      public function §_-c1z§() : Boolean
      {
         return §_-np§ ? §_-np§.§_-b2l§ : false;
      }
   }
}

