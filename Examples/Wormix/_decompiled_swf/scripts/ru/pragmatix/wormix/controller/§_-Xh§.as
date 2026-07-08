package ru.pragmatix.wormix.controller
{
   import §_-21p§.§_-4w§;
   import §_-31W§.*;
   import §_-51W§.§_-SN§;
   import §_-52L§.§_-u2x§;
   import §_-52V§.§_-031§;
   import §_-52V§.§_-b2z§;
   import §_-5P§.*;
   import §_-5Y§.*;
   import §_-62§.§_-F1D§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-T1y§;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-a3§;
   import §_-62§.§_-ou§;
   import §_-62§.§_-zF§;
   import §_-63M§.§_-F3f§;
   import §_-73I§.§_-Y1l§;
   import §_-79§.*;
   import §_-82a§.*;
   import §_-92a§.ArenaMenu;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-A2U§.§_-A23§;
   import §_-B1y§.§_-F1P§;
   import §_-B2z§.§_-63i§;
   import §_-Bv§.§_-G3P§;
   import §_-C2t§.§_-92W§;
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-p2U§;
   import §_-CF§.§_-x2t§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.MessageBox;
   import §_-DJ§.NewParamsWindow;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-E1I§.§_-b2n§;
   import §_-H2g§.§_-di§;
   import §_-H3§.*;
   import §_-Ly§.§_-81L§;
   import §_-P1B§.§_-b21§;
   import §_-P1B§.§_-bb§;
   import §_-P2i§.§_-b7§;
   import §_-RE§.§_-qW§;
   import §_-RE§.§_-r2q§;
   import §_-T2h§.§_-43C§;
   import §_-T2h§.§_-C1R§;
   import §_-U1i§.§_-81M§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-O2i§;
   import §_-U1i§.§_-XA§;
   import §_-Vu§.MD5;
   import §_-W§.§_-L1V§;
   import §_-W§.§_-ix§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1b§.§_-s2Z§;
   import §_-YF§.§_-q2v§;
   import §_-aM§.*;
   import §_-b1F§.§_-G4§;
   import §_-bI§.*;
   import §_-d26§.§_-d1H§;
   import §_-d2p§.*;
   import §_-j1w§.§_-B2i§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-vO§;
   import §_-l1g§.§_-L3Q§;
   import §_-l1g§.§_-T2N§;
   import §_-n1w§.*;
   import §_-nE§.SelectCraftWeaponScreen;
   import §_-q11§.*;
   import §_-q2b§.§_-S2L§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-d21§;
   import §_-t10§.§_-B29§;
   import §_-u13§.MD2;
   import §_-u13§.MD5;
   import §_-u13§.SHA1;
   import §_-u13§.§_-F3§;
   import §_-z1g§.§_-4l§;
   import §_-z1g§.§_-F4§;
   import §_-z1g§.§_-y1S§;
   import §_-zI§.§_-92Q§;
   import §_-zI§.§_-F3q§;
   import §_-zI§.§_-X1q§;
   import §_-zI§.§_-sI§;
   import com.distriqt.extension.inappbilling.InAppBillingAvailability;
   import com.distriqt.extension.inappbilling.events.AvailabilityEvent;
   import com.distriqt.extension.pushnotifications.PushNotifications;
   import com.hurlant.util.der.Sequence;
   import com.hurlant.util.der.§_-cz§;
   import com.hurlant.util.der.§_-l1b§;
   import config.§_-B1K§;
   import config.§_-V2w§;
   import config.§_-vR§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Button;
   import feathers.controls.ImageLoader;
   import feathers.controls.LayoutGroup;
   import feathers.controls.Panel;
   import feathers.controls.ScrollPolicy;
   import feathers.controls.SimpleScrollBar;
   import feathers.controls.ToggleButton;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.controls.renderers.IListItemRenderer;
   import feathers.data.ListCollection;
   import feathers.events.FeathersEventType;
   import feathers.layout.AnchorLayout;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.HorizontalLayout;
   import feathers.layout.VerticalAlign;
   import feathers.themes.styles.AlternateScrollBarStyles;
   import flash.display.MovieClip;
   import flash.events.MouseEvent;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.utils.id.GetAuthKeyEvent;
   import ru.pragmatix.flox.social.utils.id.IPlatformAccountController;
   import ru.pragmatix.flox.social.utils.id.UserIdType;
   import ru.pragmatix.flox.social.utils.storage.StorageType;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.*;
   import ru.pragmatix.wormix.gui.menu.shop.*;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.GetArena;
   import ru.pragmatix.wormix.messages.client.MobileLogin;
   import ru.pragmatix.wormix.messages.client.PumpReactionRates;
   import ru.pragmatix.wormix.messages.client.UpgradeWeapon;
   import ru.pragmatix.wormix.messages.server.ArenaResult;
   import ru.pragmatix.wormix.messages.server.ShopResult;
   import ru.pragmatix.wormix.messages.structures.RatingProfileStructure;
   import ru.pragmatix.wormix.messages.structures.ReconnectToSimpleBattleResultStructure;
   import ru.pragmatix.wormix.messages.structures.SelectStuffResultStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.§_-4V§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.pvp.messages.server.PvpSystemMessage;
   import ru.pragmatix.wormix.pvp.messages.wager.WagerCancelBattle;
   import ru.pragmatix.wormix.scene.tile.AtlasTileData;
   import ru.pragmatix.wormix.serialization.client.*;
   import ru.pragmatix.wormix.serialization.server.*;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.social.common.post.*;
   import ru.pragmatix.wormix.utils.StringUtil;
   import ru.pragmatix.wormix.utils.§_-6A§;
   import ru.pragmatix.wormix.utils.§_-MN§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import ru.pragmatix.wormix.utils.§_-v2b§;
   import ru.pragmatix.wormix.weapons.SwitchTurn;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.§_-73v§;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Quad;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.textures.Texture;
   import starling.textures.TextureAtlas;
   import starling.utils.AssetManager;
   import starling.utils.Pool;
   import starling.utils.ScaleMode;
   import starling.utils.SystemUtil;
   import starlingbuilder.engine.UIBuilder;
   
   public class §_-Xh§
   {
      
      private static var §_-Y2m§:String;
      
      private var §_-I1M§:ArenaMenu;
      
      private var §_-E3n§:Array = [];
      
      private var §_-M10§:Array = [];
      
      private var §_-l1I§:UserProfile;
      
      public var §_-1S§:Object;
      
      private var §_-K1M§:int;
      
      private var §_-fU§:int;
      
      public function §_-Xh§()
      {
         super();
         §_-J2g§.§_-o1R§(ArenaResult,this.§_-y29§);
      }
      
      public static function §_-418§(param1:§_-SN§) : int
      {
         var _loc2_:int = 0;
         switch(param1)
         {
            case §_-SN§.§_-C39§:
               _loc2_ = int(§_-X1j§.§_-B35§.§_-w1n§());
               break;
            case §_-SN§.§_-21i§:
               _loc2_ = §_-X1j§.§_-A24§.§_-03g§();
               break;
            case §_-SN§.§_-X1D§:
               _loc2_ = §_-X1j§.§_-A24§.§_-41O§();
         }
         return _loc2_;
      }
      
      public function dispose() : void
      {
         this.§_-l1I§ = null;
         if(this.§_-I1M§)
         {
            this.§_-I1M§.dispose();
         }
         this.§_-I1M§ = null;
         this.§_-E3n§ = [];
         this.§_-M10§ = [];
         §_-J2g§.§_-fy§(ArenaResult,this.§_-y29§);
      }
      
      public function §_-73E§() : ArenaMenu
      {
         return this.§_-I1M§;
      }
      
      public function §_-Z2j§() : void
      {
         if(this.§_-I1M§)
         {
            if(this.§_-I1M§.§_-h1C§())
            {
               this.§_-I1M§.hide();
            }
         }
         §_-J2l§.§_-u2b§.hide();
      }
      
      public function §_-Nr§() : void
      {
         if(this.§_-I1M§ == null)
         {
            this.§_-I1M§ = new ArenaMenu();
            this.§_-I1M§.getContainer().addEventListener(Event.REMOVED_FROM_STAGE,this.onRemovedFromStage);
         }
      }
      
      private function onRemovedFromStage(param1:Event) : void
      {
         this.§_-I1M§.getContainer().addEventListener(Event.REMOVED_FROM_STAGE,this.onRemovedFromStage);
         this.§_-I1M§ = null;
      }
      
      public function §_-KU§(param1:Boolean = true) : void
      {
         var _loc2_:int = 0;
         var _loc3_:NewParamsWindow = null;
         if(param1)
         {
            _loc2_ = §_-33r§.§_-r1A§(Application.userProfile);
            if(_loc2_ > 0 && Application.userProfile.getLevel() >= §_-F4§.§_-t1s§)
            {
               _loc3_ = new NewParamsWindow();
               _loc3_.show();
               return;
            }
         }
         this.§_-ka§();
      }
      
      private function §_-ka§(param1:UserProfile = null, param2:Object = null) : void
      {
         if(param2 != null)
         {
            this.§_-1S§ = param2;
         }
         this.§_-l1I§ = param1;
         §_-J2l§.§_-u2b§.show(this.§_-I3T§,this.§_-UT§,this.§_-Q1N§,this.§_-j26§);
      }
      
      public function §_-y1t§(param1:UserProfile = null, param2:String = "") : void
      {
         §_-Y2m§ = param2;
         this.§_-ka§(param1,ArenaMenu.§_-TW§);
      }
      
      private function §_-j26§() : void
      {
         SystemUtil.executeWhenApplicationIsActive(this.§_-Z2j§);
      }
      
      private function §_-UT§() : void
      {
         SystemUtil.executeWhenApplicationIsActive(this.§_-Nx§,true);
      }
      
      private function §_-Q1N§(param1:uint) : void
      {
         SystemUtil.executeWhenApplicationIsActive(this.§_-Nx§,false);
      }
      
      private function §_-Nx§(param1:Boolean) : void
      {
         var _loc2_:Boolean = false;
         var _loc3_:Boolean = false;
         if(§_-X1j§.§_-12n§.gameMaster != null)
         {
            return;
         }
         this.§_-Nr§();
         if(this.§_-I1M§)
         {
            this.§_-I1M§.§_-P2F§();
            _loc2_ = §_-F4§.§_-U4§(Application.userProfile);
            _loc3_ = §_-F4§.§_-11A§(Application.userProfile);
            if(!this.§_-I1M§.§_-h1C§())
            {
               if(this.§_-1S§)
               {
                  this.§_-I1M§.§_-i2x§(this.§_-1S§);
               }
               else if(_loc2_ || _loc3_ && !param1)
               {
                  this.§_-I1M§.§_-i2x§(ArenaMenu.§_-I2S§);
               }
               else
               {
                  this.§_-I1M§.§_-i2x§(ArenaMenu.§_-Or§);
               }
            }
            this.§_-I1M§.show();
            if(Boolean(this.§_-l1I§) && Boolean(§_-Y2m§))
            {
               this.§_-I1M§.§_-R5§(this.§_-l1I§);
            }
         }
         §_-J2l§.§_-u2b§.hide();
      }
      
      public function §_-I3T§(param1:Function, param2:Function, param3:Function) : void
      {
         if(param1 != null && this.§_-E3n§.indexOf(param1) == -1)
         {
            this.§_-E3n§.push(param1);
         }
         if(param2 != null && this.§_-M10§.indexOf(param2) == -1)
         {
            this.§_-M10§.push(param2);
         }
         §_-d21§.§_-B1U§(§_-J2g§.§_-i1N§,new GetArena(),param3);
      }
      
      private function §_-y29§(param1:§_-63i§) : void
      {
         var _loc3_:Function = null;
         if(§_-X1j§.§_-12n§.gameMaster != null)
         {
            return;
         }
         var _loc2_:ArenaResult = ArenaResult(param1.message);
         this.§_-K1M§ = _loc2_.wagerWinAwardToken;
         this.§_-fU§ = _loc2_.bossWinAwardToken;
         §_-N1V§.§_-I39§.§_-X1P§().§_-rm§ = _loc2_.curSoloMissionId;
         §_-N1V§.§_-I39§.§_-N1i§().§_-rm§ = _loc2_.curCooperativeMissionId;
         §_-qW§.getInstance().§_-Ip§(_loc2_.heroicStructures);
         §_-r2q§.bossAvailable = true;
         §_-r2q§.§_-UJ§ = _loc2_.superBossAvailable;
         §_-X1j§.§_-B35§.§_-Vl§(_loc2_.battlesCount);
         §_-X1j§.§_-B35§.§_-f2d§(new Date().time + _loc2_.delay);
         §_-J2l§.§_-u2b§.hide();
         if(_loc2_.battlesCount == 0 && _loc2_.delay > 0)
         {
            for each(_loc3_ in this.§_-M10§)
            {
               _loc3_(_loc2_.delay);
            }
         }
         else
         {
            for each(_loc3_ in this.§_-E3n§)
            {
               _loc3_();
            }
         }
         this.§_-s1R§();
      }
      
      private function §_-s1R§() : void
      {
         this.§_-E3n§ = [];
         this.§_-M10§ = [];
      }
      
      public function get §_-mV§() : UserProfile
      {
         return this.§_-l1I§;
      }
      
      public function get §_-2z§() : String
      {
         return §_-Y2m§;
      }
      
      public function set §_-2z§(param1:String) : void
      {
         §_-Y2m§ = param1;
      }
      
      public function §_-03g§() : int
      {
         return this.§_-K1M§;
      }
      
      public function §_-41O§() : int
      {
         return this.§_-fU§;
      }
      
      public function §_-92u§() : Boolean
      {
         return this.§_-K1M§ > 0;
      }
      
      public function §_-X2A§() : Boolean
      {
         return this.§_-fU§ > 0;
      }
      
      public function §_-3b§() : void
      {
         --this.§_-K1M§;
      }
      
      public function §_-13X§() : void
      {
         --this.§_-fU§;
      }
   }
}

