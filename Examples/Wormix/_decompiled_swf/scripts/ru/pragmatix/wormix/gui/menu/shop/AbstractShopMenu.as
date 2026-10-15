package ru.pragmatix.wormix.gui.menu.shop
{
   import §_-02q§.§_-4s§;
   import §_-21p§.§_-4w§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-L1r§;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-a3§;
   import §_-73I§.§_-Z2e§;
   import §_-73d§.§_-x2C§;
   import §_-82a§.*;
   import §_-931§.§_-sz§;
   import §_-A1K§.§_-TA§;
   import §_-A2U§.§_-A23§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.§_-63i§;
   import §_-Bv§.§_-G3P§;
   import §_-C3f§.§_-1Y§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-T22§;
   import §_-DJ§.§_-fH§;
   import §_-F2Q§.GestureEvent;
   import §_-H3§.*;
   import §_-Ly§.§_-81L§;
   import §_-SP§.§_-M4§;
   import §_-TF§.§_-q1j§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1b§.§_-jH§;
   import §_-b1R§.§_-Bx§;
   import §_-f1G§.§_-kf§;
   import §_-l1K§.§_-d2j§;
   import §_-l1K§.§_-vO§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-s1s§.*;
   import §_-x2Q§.§_-dU§;
   import §_-z1g§.§_-lp§;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.hurlant.crypto.tls.§_-21v§;
   import com.hurlant.math.BigInteger;
   import com.hurlant.math.bi_internal;
   import feathers.controls.LayoutGroup;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.core.PopUpManager;
   import feathers.data.ArrayCollection;
   import feathers.events.FeathersEventType;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.RelativePosition;
   import feathers.layout.VerticalAlign;
   import flash.display.BitmapData;
   import flash.events.*;
   import flash.geom.Point;
   import flash.text.TextField;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataOutput;
   import org.gestouch.core.GestureState;
   import org.gestouch.core.gestouch_internal;
   import ru.pragmatix.flox.gui.controls.MessageBox;
   import ru.pragmatix.flox.gui.controls.§_-zi§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.FontColor;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.ClanServiceResult;
   import ru.pragmatix.wormix.messages.clans.request.ClanSummaryRequest;
   import ru.pragmatix.wormix.messages.clans.request.LowerRankInClanRequest;
   import ru.pragmatix.wormix.messages.clans.response.LowerRankInClanResponse;
   import ru.pragmatix.wormix.messages.clans.response.treasury.DonateClanResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.structures.DailyBonusStructure;
   import ru.pragmatix.wormix.messages.structures.HeroicMissionStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.items.craft.§_-IE§;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.model.user.§_-m1x§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.server.PvpRetryCommandRequestServer;
   import ru.pragmatix.wormix.scene.tile.TileData;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.AirBlocker;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.extensions.PDParticleSystem;
   import starling.filters.ColorMatrixFilter;
   
   use namespace bi_internal;
   use namespace gestouch_internal;
   
   public class AbstractShopMenu extends LayoutGroup implements §_-T22§
   {
      
      protected var binder:Binder;
      
      private var §_-A1z§:String;
      
      public function AbstractShopMenu(param1:String = "shop_menu")
      {
         super();
         this.§_-A1z§ = param1;
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         this.binder = new Binder();
         Application.uiBuilder.create(Application.assetManager.getObject(this.§_-A1z§),true,this.binder);
         addChild(this.binder._container);
         this.§_-zA§();
         this.binder._closeButton.addEventListener(starling.events.Event.TRIGGERED,this.§_-03a§);
         if(this.binder._backButton)
         {
            this.binder._backButton.addEventListener(starling.events.Event.TRIGGERED,this.§_-52u§);
         }
         this.§_-Hx§();
      }
      
      protected function §_-zA§() : void
      {
         var _loc1_:int = 0;
         var _loc2_:int = 0;
         _loc1_ = Starling.current.stage.stageWidth;
         _loc2_ = Starling.current.stage.stageHeight;
         this.width = _loc1_;
         this.height = _loc2_;
         this.binder._container.x = 0;
         this.binder._container.y = 0;
         this.binder._background.scaleX = Math.ceil(_loc1_ / 128);
         this.binder._background.scaleY = Math.ceil(_loc2_ / 128);
         this.binder._background.x = (_loc1_ - this.binder._background.width) * 0.5;
         this.binder._background.y = (_loc2_ - this.binder._background.height) * 0.5;
         this.§_-43R§(_loc1_,_loc2_);
         this.binder._topShadow.x = 0;
         this.binder._topShadow.y = 0;
         this.binder._topShadow.width = _loc1_;
         this.binder._bottomShadow.x = 0;
         this.binder._bottomShadow.y = _loc2_;
         this.binder._bottomShadow.width = _loc1_;
      }
      
      protected function §_-43R§(param1:int, param2:int) : void
      {
         this.binder._container.width = param1;
         this.binder._container.height = param2;
      }
      
      protected function §_-Hx§() : void
      {
         var _loc1_:AnchorLayoutData = new AnchorLayoutData();
         _loc1_.bottom = 5;
         _loc1_.percentWidth = 100;
         _loc1_.bottomAnchorDisplayObject = this.binder._shopContent;
         this.binder._topContent.layoutData = _loc1_;
         _loc1_ = new AnchorLayoutData();
         _loc1_.top = 5;
         _loc1_.percentWidth = 100;
         _loc1_.topAnchorDisplayObject = this.binder._shopContent;
         this.binder._bottomContent.layoutData = _loc1_;
         this.binder._container.validate();
      }
      
      protected function §_-MZ§(param1:DisplayObject) : DisplayObject
      {
         this.binder._shopContent.addChild(param1);
         return param1;
      }
      
      protected function §_-ie§() : LayoutGroup
      {
         return this.binder._shopContent;
      }
      
      protected function setTitle(param1:String) : void
      {
         this.binder._titleLabel.text = param1;
      }
      
      protected function §_-03a§(param1:starling.events.Event) : void
      {
         dispatchEventWith(§_-M4§.WINDOW_CLOSE);
      }
      
      public function show() : void
      {
         PopUpManager.addPopUp(this,false,true);
         this.§_-oG§(Application.userProfile);
      }
      
      public function hide() : void
      {
         if(PopUpManager.isPopUp(this))
         {
            PopUpManager.removePopUp(this);
         }
      }
      
      public function §_-h1C§() : Boolean
      {
         return parent != null;
      }
      
      public function §_-52u§(param1:starling.events.Event = null) : void
      {
         §_-h2B§.play(§_-81L§.§_-o1Z§);
      }
      
      public function §_-oG§(param1:UserProfile) : void
      {
         var _loc2_:int = param1.§_-51e§() - §_-X1j§.§_-zU§.§_-F20§();
         var _loc3_:int = param1.§_-o1N§() - §_-X1j§.§_-zU§.§_-t2L§();
         this.§_-CI§(_loc2_,_loc3_);
      }
      
      public function §_-CI§(param1:int, param2:int) : void
      {
         if(this.binder)
         {
            this.binder._fuzyValue.text = param1.toString();
            this.binder._rubyValue.text = param2.toString();
         }
      }
      
      public function §_-x16§(param1:Boolean) : void
      {
         if(this.binder)
         {
            this.binder._backButton.visible = param1;
         }
      }
      
      public function §_-X2O§() : void
      {
      }
      
      public function get isSubpanelOpened() : Boolean
      {
         return false;
      }
      
      override public function dispose() : void
      {
         this.binder._closeButton.removeEventListener(starling.events.Event.TRIGGERED,this.§_-03a§);
         if(this.binder._backButton)
         {
            this.binder._backButton.removeEventListener(starling.events.Event.TRIGGERED,this.§_-52u§);
         }
         this.binder = null;
         super.dispose();
      }
   }
}

import feathers.controls.Label;
import feathers.controls.LayoutGroup;
import feathers.controls.List;
import feathers.controls.TabBar;
import starling.display.Button;
import starling.display.Image;
import starling.text.TextField;
import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;

class Binder
{
   
   public var _container:LayoutGroup;
   
   public var _detailsGroup:LayoutGroup;
   
   public var _shopContent:LayoutGroup;
   
   public var _topContent:LayoutGroup;
   
   public var _bottomContent:LayoutGroup;
   
   public var _background:Image;
   
   public var _topShadow:Image;
   
   public var _bottomShadow:Image;
   
   public var _fuzyValue:TextField;
   
   public var _rubyValue:TextField;
   
   public var _closeButton:Button;
   
   public var _titleLabel:Label;
   
   public var _backButton:ContainerButtonWithStates;
   
   public var _tabBar:TabBar;
   
   public var _itemsList:List;
   
   public var _pageIndicator:LayoutGroup;
   
   public var _prevBtn:Button;
   
   public var _nextBtn:Button;
   
   public var _description:Label;
   
   public function Binder()
   {
      super();
   }
}
