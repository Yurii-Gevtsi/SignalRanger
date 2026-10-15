package ru.pragmatix.wormix.gui.menu.shop
{
   import §_-03T§.§_-2p§;
   import §_-12W§.*;
   import §_-21p§.§_-4w§;
   import §_-31N§.§_-42G§;
   import §_-43V§.Random;
   import §_-5Y§.§_-91F§;
   import §_-62§.*;
   import §_-63M§.§_-F3f§;
   import §_-73d§.§_-V19§;
   import §_-91B§.§_-z2l§;
   import §_-9e§.§_-73e§;
   import §_-9e§.§_-8O§;
   import §_-9e§.§_-A18§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.Server;
   import §_-B2z§.§_-c1G§;
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-x2t§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-G3w§.GlobalRatingScreen;
   import §_-G3w§.TodayRatingScreen;
   import §_-G3w§.TopClanRatingScreen;
   import §_-G3w§.YesterdayRatingScreen;
   import §_-L1H§.§_-C3e§;
   import §_-P1B§.§_-b21§;
   import §_-T2V§.§_-13F§;
   import §_-TF§.§_-F1B§;
   import §_-TF§.§_-q1j§;
   import §_-U1i§.§_-81M§;
   import §_-U1i§.§_-O2i§;
   import §_-Y1Q§.§_-F1g§;
   import §_-YF§.§_-N3§;
   import §_-YF§.§_-g2W§;
   import §_-YF§.§_-q2v§;
   import §_-Z1K§.§_-q24§;
   import §_-b1F§.*;
   import §_-g2r§.§_-03A§;
   import §_-hO§.§_-v24§;
   import §_-k1u§.§_-Oi§;
   import §_-l1K§.§_-N1Z§;
   import §_-l1K§.§_-W2f§;
   import §_-l1K§.§_-d2j§;
   import §_-l1g§.§_-L3Q§;
   import §_-l1g§.§_-T2N§;
   import §_-m1g§.§_-N1y§;
   import §_-p18§.§_-vd§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-6a§;
   import §_-u13§.MD5;
   import §_-u13§.SHA1;
   import §_-u1P§.ShopResultEvent;
   import §_-u2J§.§_-T1K§;
   import §_-w2i§.§_-Zd§;
   import §_-x2Q§.§_-dU§;
   import §_-z1g§.§_-82X§;
   import §_-z20§.§_-Tn§;
   import §_-zI§.§_-8D§;
   import com.amanitadesign.steam.SteamConstants;
   import com.amanitadesign.steam.SteamEvent;
   import com.distriqt.extension.cloudstorage.CloudStorage;
   import com.distriqt.extension.cloudstorage.events.KeyValueStoreEvent;
   import com.greensock.loading.LoaderMax;
   import com.greensock.loading.display.ContentDisplay;
   import com.hurlant.crypto.tls.*;
   import com.hurlant.math.*;
   import com.hurlant.util.§_-a2P§;
   import dragonBones.animation.WorldClock;
   import dragonBones.starling.StarlingFactory;
   import dragonBones.starling.StarlingTextureAtlasData;
   import dragonBones.textures.TextureAtlasData;
   import feathers.controls.Button;
   import feathers.controls.ButtonState;
   import feathers.controls.LayoutGroup;
   import feathers.controls.List;
   import feathers.controls.Panel;
   import feathers.controls.ScreenNavigator;
   import feathers.controls.ScreenNavigatorItem;
   import feathers.controls.ScrollBarDisplayMode;
   import feathers.controls.ScrollInteractionMode;
   import feathers.controls.ScrollPolicy;
   import feathers.controls.ToggleButton;
   import feathers.core.ToggleGroup;
   import feathers.data.ListCollection;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.HorizontalLayout;
   import feathers.layout.HorizontalLayoutData;
   import feathers.layout.ILayout;
   import feathers.layout.TiledRowsLayout;
   import feathers.layout.VerticalAlign;
   import flash.display.BitmapData;
   import flash.display.Shape;
   import flash.display.SimpleButton;
   import flash.events.Event;
   import flash.events.TimerEvent;
   import flash.geom.ColorTransform;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.globalization.DateTimeFormatter;
   import flash.net.Socket;
   import flash.text.TextFormat;
   import flash.text.TextFormatAlign;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import org.gestouch.core.GestureState;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.utils.id.GetAuthKeyEvent;
   import ru.pragmatix.flox.social.utils.id.UserIdType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.ai.§_-Q2U§;
   import ru.pragmatix.wormix.gui.§_-eP§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.clans.chat.NotInClanChatScreen;
   import ru.pragmatix.wormix.gui.menu.clans.chat.§_-D2t§;
   import ru.pragmatix.wormix.gui.menu.shop.desktop.§_-11V§;
   import ru.pragmatix.wormix.gui.menu.shop.desktop.§_-An§;
   import ru.pragmatix.wormix.messages.clans.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.server.ProfilesResult;
   import ru.pragmatix.wormix.messages.structures.BossWormStructure;
   import ru.pragmatix.wormix.messages.structures.HeroicMissionStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.IPhysicModel;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreated;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.serialization.server.*;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.StringUtil;
   import ru.pragmatix.wormix.utils.§_-2t§;
   import ru.pragmatix.wormix.utils.§_-6A§;
   import ru.pragmatix.wormix.utils.§_-Cq§;
   import ru.pragmatix.wormix.utils.§_-MN§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.MolotovProjectile;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.textures.Texture;
   import starling.textures.TextureAtlas;
   import starling.utils.AssetManager;
   import starling.utils.Color;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   import starlingbuilder.extensions.uicomponents.CustomLabel;
   
   use namespace bi_internal;
   
   public class §_-Vo§ extends AbstractShopMenu
   {
      
      protected var §_-v1M§:Class;
      
      protected var pages:§_-11V§;
      
      protected var info:§_-An§;
      
      protected var group:ToggleGroup;
      
      public function §_-Vo§(param1:String = "shop_menu")
      {
         super(param1);
         this.group = new ToggleGroup();
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         this.§_-7c§();
         this.§_-K7§();
         this.§_-21b§();
         this.§_-B3H§();
      }
      
      protected function §_-F14§() : void
      {
      }
      
      override public function show() : void
      {
         super.show();
         this.§_-u1i§();
         this.§_-F14§();
         Starling.current.stage.addEventListener(starling.events.Event.RESIZE,this.§_-536§);
      }
      
      override public function hide() : void
      {
         Starling.current.stage.removeEventListener(starling.events.Event.RESIZE,this.§_-536§);
         super.hide();
      }
      
      private function §_-536§(param1:starling.events.Event) : void
      {
         §_-zA§();
         binder._container.invalidate();
         this.§_-u1i§();
         binder._itemsList.dataProvider.removeAll();
         binder._itemsList.invalidate();
         this.§_-Y1U§();
      }
      
      protected function §_-u1i§() : void
      {
         binder._itemsList.invalidate();
      }
      
      protected function §_-7c§() : void
      {
         binder._tabBar.tabFactory = this.createTab;
         binder._tabBar.gap = 5;
         binder._tabBar.addEventListener(starling.events.Event.CHANGE,this.§_-Q1X§);
      }
      
      private function §_-Q1X§(param1:starling.events.Event) : void
      {
         this.§_-Y1U§();
         this.§_-w1G§();
      }
      
      protected function §_-Y1U§() : void
      {
      }
      
      protected function §_-t1p§(param1:Array) : void
      {
         binder._itemsList.dataProvider = new ListCollection(param1);
         this.pages.removeEventListener(starling.events.Event.CHANGE,this.§_-O1v§);
         this.pages.selectedIndex = 0;
         this.pages.addEventListener(starling.events.Event.CHANGE,this.§_-O1v§);
         if(param1.length > 0)
         {
            binder._itemsList.selectedIndex = 0;
         }
      }
      
      protected function §_-B3H§() : void
      {
         binder._detailsGroup.touchable = true;
         if(this.§_-v1M§ != null)
         {
            this.info = new this.§_-v1M§();
            binder._detailsGroup.addChild(this.info.§_-Bh§());
         }
      }
      
      protected function §_-71p§() : void
      {
         if(this.info)
         {
            this.info.destroy();
            this.info = null;
         }
      }
      
      private function createTab() : ToggleButton
      {
         var _loc1_:Sprite = §_-YQ§.§_-qM§("DesktopShopTab",§_-q2v§.§_-A2T§);
         var _loc2_:ToggleButton = _loc1_.getChildAt(0) as ToggleButton;
         _loc2_.useHandCursor = true;
         _loc2_.paddingLeft = _loc2_.paddingRight = 2;
         _loc2_.width = 170;
         _loc1_.dispose();
         return _loc2_;
      }
      
      private function §_-K7§() : void
      {
         var _loc1_:List = null;
         _loc1_ = binder._itemsList;
         _loc1_.layout = this.§_-Kc§();
         _loc1_.snapToPages = true;
         _loc1_.horizontalScrollPolicy = ScrollPolicy.OFF;
         _loc1_.verticalScrollPolicy = ScrollPolicy.OFF;
         _loc1_.addEventListener(starling.events.Event.CHANGE,this.§_-FK§);
         _loc1_.addEventListener(starling.events.Event.SCROLL,this.§_-6k§);
      }
      
      private function §_-6k§(param1:starling.events.Event) : void
      {
         this.§_-w1G§();
      }
      
      private function §_-FK§(param1:starling.events.Event) : void
      {
         if(binder._itemsList.selectedItem)
         {
            §_-h2B§.play(§_-d27§.§_-S29§);
         }
         this.info.setItem(binder._itemsList.selectedItem);
         this.§_-w1G§();
      }
      
      private function §_-w1G§() : void
      {
         if(binder._itemsList.horizontalPageCount != this.pages.§_-01o§())
         {
            this.pages.§_-03o§(binder._itemsList.horizontalPageCount);
            this.pages.invalidate();
         }
         this.pages.selectedIndex = binder._itemsList.horizontalPageIndex;
         this.§_-L36§();
      }
      
      protected function §_-G3c§() : Number
      {
         return binder._itemsList.width / this.§_-t1n§();
      }
      
      protected function §_-kV§() : Number
      {
         return binder._itemsList.height / this.§_-22Z§();
      }
      
      protected function §_-t1n§() : Number
      {
         return 1;
      }
      
      protected function §_-22Z§() : Number
      {
         return 1;
      }
      
      protected function §_-Kc§() : ILayout
      {
         var _loc1_:TiledRowsLayout = new TiledRowsLayout();
         _loc1_.horizontalGap = _loc1_.verticalGap = 10;
         _loc1_.padding = 0;
         _loc1_.paging = feathers.layout.Direction.HORIZONTAL;
         _loc1_.horizontalAlign = HorizontalAlign.LEFT;
         _loc1_.verticalAlign = VerticalAlign.TOP;
         _loc1_.useSquareTiles = false;
         return _loc1_;
      }
      
      private function §_-21b§() : void
      {
         this.pages = new §_-11V§(binder._pageIndicator);
         this.pages.§_-b2U§(binder._prevBtn,binder._nextBtn);
         this.pages.addEventListener(starling.events.Event.CHANGE,this.§_-O1v§);
      }
      
      private function §_-O1v§(param1:starling.events.Event) : void
      {
         if(binder._itemsList.horizontalPageIndex != this.pages.selectedIndex)
         {
            §_-h2B§.play(§_-d27§.§_-S29§);
            binder._itemsList.scrollToPageIndex(this.pages.selectedIndex,0);
            this.§_-L36§();
         }
      }
      
      private function §_-L36§() : void
      {
         binder._prevBtn.enabled = this.pages.selectedIndex != 0;
         binder._nextBtn.enabled = this.pages.selectedIndex != this.pages.dataProvider.length - 1;
      }
      
      override protected function §_-Hx§() : void
      {
      }
      
      public function update(param1:int) : void
      {
         binder._itemsList.dataProvider.updateItemAt(binder._itemsList.selectedIndex);
         binder._itemsList.invalidate(INVALIDATION_FLAG_DATA);
      }
      
      override public function dispose() : void
      {
         Starling.current.stage.removeEventListener(starling.events.Event.RESIZE,this.§_-536§);
         if(binder)
         {
            binder._tabBar.removeEventListener(starling.events.Event.CHANGE,this.§_-Q1X§);
            binder._itemsList.removeEventListener(starling.events.Event.CHANGE,this.§_-FK§);
            binder._itemsList.removeEventListener(starling.events.Event.SCROLL,this.§_-6k§);
         }
         if(this.pages)
         {
            this.pages.removeEventListener(starling.events.Event.CHANGE,this.§_-O1v§);
            this.pages = null;
         }
         this.§_-71p§();
         if(this.group)
         {
            this.group.removeAllItems();
            this.group = null;
         }
         super.dispose();
      }
   }
}

