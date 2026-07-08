package ru.pragmatix.wormix.gui
{
   import §_-21A§.*;
   import §_-62§.§_-F1D§;
   import §_-63U§.ProgressBarBoss;
   import §_-82l§.§_-1I§;
   import §_-91A§.§_-sH§;
   import §_-937§.§_-81u§;
   import §_-937§.§_-BO§;
   import §_-937§.§_-T15§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.Cookie;
   import §_-B2z§.§_-63i§;
   import §_-CR§.§_-72p§;
   import §_-DJ§.§_-73§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-F39§.*;
   import §_-H2g§.§_-di§;
   import §_-H3§.*;
   import §_-HD§.*;
   import §_-K35§.§_-99§;
   import §_-Ly§.§_-81L§;
   import §_-RE§.§_-52p§;
   import §_-RE§.§_-r2q§;
   import §_-SP§.§_-j1f§;
   import §_-T1t§.§_-u19§;
   import §_-TF§.§_-q1j§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-O2i§;
   import §_-Vg§.*;
   import §_-YF§.§_-q2v§;
   import §_-aM§.*;
   import §_-d2p§.*;
   import §_-fo§.*;
   import §_-g2r§.§_-03A§;
   import §_-j1w§.§_-B2i§;
   import §_-jF§.ClanMemberItemRenderer;
   import §_-jF§.§_-U17§;
   import §_-k1r§.AlertPanel;
   import §_-l2r§.§_-K1t§;
   import §_-q11§.§_-C2n§;
   import §_-qH§.§_-S25§;
   import §_-rM§.§_-61j§;
   import §_-s1Q§.Artifacts;
   import §_-s20§.§_-J2g§;
   import §_-sQ§.§_-G1Y§;
   import §_-u13§.MD5;
   import §_-u13§.SHA1;
   import §_-z1g§.§_-y1S§;
   import §_-z20§.*;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.loading.LoaderMax;
   import com.greensock.loading.LoaderStatus;
   import com.greensock.plugins.*;
   import com.hurlant.crypto.tls.§_-21v§;
   import com.hurlant.math.*;
   import com.hurlant.util.§_-F2L§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Button;
   import feathers.controls.LayoutGroup;
   import feathers.controls.NumericStepper;
   import feathers.controls.ScrollPolicy;
   import feathers.controls.ToggleButton;
   import feathers.controls.text.TextFieldTextRenderer;
   import feathers.controls.text.TextInstanceCreator;
   import feathers.core.ITextRenderer;
   import feathers.core.ToggleGroup;
   import feathers.data.ArrayCollection;
   import feathers.data.IListCollection;
   import feathers.layout.AnchorLayout;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.HorizontalLayout;
   import feathers.layout.ILayoutDisplayObject;
   import feathers.layout.RelativePosition;
   import feathers.layout.TiledColumnsLayout;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.themes.FontSize;
   import feathers.themes.styles.AlternateButtonsStyles;
   import flash.display.Shape;
   import flash.geom.Point;
   import flash.text.TextFormatAlign;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.controller.SocialApiSettings;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.gui.menu.shop.*;
   import ru.pragmatix.wormix.messages.clans.request.edit.ChangeClanDescriptionRequest;
   import ru.pragmatix.wormix.messages.clans.request.treasury.CashMedalsClanRequest;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.EndTurn;
   import ru.pragmatix.wormix.messages.client.GoogleValidateInappPurchase;
   import ru.pragmatix.wormix.messages.client.StartBattle;
   import ru.pragmatix.wormix.messages.server.EndTurnResponse;
   import ru.pragmatix.wormix.messages.structures.ShopItemStructure;
   import ru.pragmatix.wormix.messages.structures.TurnStructure;
   import ru.pragmatix.wormix.model.rewards.§_-d2F§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.model.IPhysicModel;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurn;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurnResponse;
   import ru.pragmatix.wormix.pvp.messages.ex.*;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.scene.tile.AtlasTileData;
   import ru.pragmatix.wormix.scene.tile.TileType;
   import ru.pragmatix.wormix.social.common.post.*;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.*;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   use namespace bi_internal;
   
   public class §_-M2w§ extends §_-qY§
   {
      
      public static const §_-53l§:int = 2;
      
      public static const §_-81m§:int = 30;
      
      public static const §_-xp§:int = 10;
      
      private static const §_-R1K§:String = "PHOTO_BUTTON_POSITION";
      
      private static const §_-n1B§:String = "HINT_BUTTON_POSITION";
      
      private static const CONTROL_HINT_BUTTON_POSITION:String = "CONTROL_HINT_BUTTON_POSITION";
      
      private static const HATS_PANEL_POSITION:String = "HATS_PANEL_POSITION";
      
      private static const UNITS_INFO_POSITION:String = "UNITS_INFO_POSITION";
      
      private static const ALERT_PANEL_POSITION:String = "ALERT_PANEL_POSITION";
      
      private static const HAS_RELATIVE_COORDS:String = "HAS_RELATIVE_COORDS";
      
      public var exitButton:Sprite;
      
      public var weaponHintButton:ToggleButton;
      
      public var weaponHintContainer:LayoutGroup;
      
      public var controlHintButton:Button;
      
      public var §_-tw§:§_-Ey§;
      
      public var §_-ki§:§_-F3s§;
      
      public var §_-Q2J§:ToggleButton;
      
      public var unitsInfoSwitchButton:Button;
      
      public var unitsInfoSwitch:§_-BO§;
      
      public var cloudsVisibilitySwitchButton:Button;
      
      public var cloudsVisibilitySwitch:§_-81u§;
      
      public var bagPositionSwitchButton:Button;
      
      public var bagPositionSwitch:§_-T15§;
      
      public var resetLayoutButton:Button;
      
      public var §_-Up§:§_-F3s§;
      
      public var screenshotButton:Button;
      
      public var hatsPanelContainer:DisplayObjectContainer;
      
      public var hatsPanel:§_-1N§;
      
      public var alertPanel:AlertPanel;
      
      public var windContainer:DisplayObject;
      
      public var timer:§_-93t§;
      
      public var wind:§_-m1F§;
      
      public var §_-k1A§:ITextRenderer;
      
      private var §_-nh§:Point;
      
      private var leftColumn:LayoutGroup;
      
      private var rightColumn:LayoutGroup;
      
      private var §_-BM§:LayoutGroup;
      
      private var §_-uJ§:LayoutGroup;
      
      private var §_-B1O§:Boolean;
      
      private var §_-X2c§:Number = 0;
      
      private var §_-L6§:Array = [];
      
      private var §_-K2c§:Boolean;
      
      public function §_-M2w§(param1:§_-E2w§, param2:Number, param3:Number, param4:Boolean = false)
      {
         this.§_-K2c§ = param4;
         super(param1,param2,param3);
         this.§_-B1O§ = Cookie.§_-t2R§(HAS_RELATIVE_COORDS,false);
      }
      
      override protected function §_-I2w§() : void
      {
         this.exitButton = §_-I3V§("exit") as Sprite;
         this.controlHintButton = §_-I3V§("controlHelp") as Button;
         this.weaponHintButton = §_-I3V§("tips") as ToggleButton;
         this.weaponHintContainer = §_-I3V§("weaponHintContainer") as LayoutGroup;
         this.§_-tw§ = new §_-Ey§(this.weaponHintButton,this.weaponHintContainer);
         this.§_-Q2J§ = §_-I3V§("moveModeSwitch") as ToggleButton;
         this.unitsInfoSwitchButton = §_-I3V§("unitsInfoSwitch") as Button;
         this.unitsInfoSwitch = new §_-BO§(this.unitsInfoSwitchButton);
         this.cloudsVisibilitySwitchButton = §_-I3V§("cloudsVisibilitySwitch") as Button;
         this.cloudsVisibilitySwitch = new §_-81u§(this.cloudsVisibilitySwitchButton);
         this.bagPositionSwitchButton = §_-I3V§("bagPositionSwitch") as Button;
         this.bagPositionSwitch = new §_-T15§(this.bagPositionSwitchButton);
         this.resetLayoutButton = §_-I3V§("resetButton") as Button;
         this.§_-Up§ = new §_-F3s§(§_-I3V§("zoomSlider") as DisplayObjectContainer,false);
         this.screenshotButton = §_-I3V§("photo") as Button;
         this.hatsPanelContainer = §_-I3V§("hatsPanel") as DisplayObjectContainer;
         this.hatsPanelContainer.touchable = true;
         this.hatsPanelContainer.getChildByName("hats").touchable = true;
         this.hatsPanel = new §_-1N§(this.hatsPanelContainer.getChildByName("hats"));
         this.timer = new §_-93t§(§_-I3V§("windContainer.timer") as DisplayObjectContainer);
         this.windContainer = §_-I3V§("windContainer");
         this.wind = new §_-m1F§(§_-I3V§("windContainer.wind") as DisplayObjectContainer);
         this.§_-k1A§ = §_-I3V§("taunts") as ITextRenderer;
         var _loc1_:DisplayObjectContainer = §_-I3V§("zoom") as DisplayObjectContainer;
         if(SystemUtil.isDesktop)
         {
            this.§_-ki§ = new §_-F3s§(_loc1_,true);
         }
         else
         {
            _loc1_.removeFromParent(true);
         }
         if(SystemUtil.isDesktop)
         {
            this.controlHintButton.addEventListener(Event.TRIGGERED,this.§_-K1L§);
         }
         else
         {
            this.controlHintButton.visible = false;
         }
         this.§_-nh§ = Pool.getPoint(this.hatsPanelContainer.x,this.hatsPanelContainer.y);
         this.alertPanel = new AlertPanel();
      }
      
      private function §_-K1L§(param1:Event) : void
      {
         if(!this.§_-Q2J§.isSelected)
         {
            §_-73§.§_-D3u§(§_-q2v§.§_-S1m§);
         }
      }
      
      override protected function §_-7R§() : void
      {
         §_-92r§(this.hatsPanelContainer,true,HATS_PANEL_POSITION);
         §_-92r§(this.screenshotButton,true,§_-R1K§);
         this.§_-L6§.push(this.screenshotButton);
         §_-92r§(this.weaponHintButton,true,§_-n1B§);
         this.§_-L6§.push(this.weaponHintButton);
         if(this.controlHintButton.visible)
         {
            §_-92r§(this.controlHintButton,true,CONTROL_HINT_BUTTON_POSITION);
            this.§_-L6§.push(this.controlHintButton);
         }
         if(!this.§_-K2c§)
         {
            if(this.§_-B1O§)
            {
               §_-92r§(this.alertPanel,true,ALERT_PANEL_POSITION);
            }
            this.§_-L6§.push(this.weaponHintButton);
         }
      }
      
      override protected function §_-y2P§() : void
      {
         var _loc4_:AnchorLayoutData = null;
         this.§_-BM§ = new LayoutGroup();
         var _loc1_:HorizontalLayout = new HorizontalLayout();
         _loc1_.verticalAlign = VerticalAlign.MIDDLE;
         _loc1_.gap = 10;
         this.§_-BM§.layout = _loc1_;
         §_-9H§(this.§_-BM§,this.§_-Q2J§);
         §_-9H§(this.§_-BM§,this.resetLayoutButton);
         §_-9H§(this.§_-BM§,this.§_-Up§.getContainer());
         this.§_-uJ§ = new LayoutGroup();
         _loc1_ = new HorizontalLayout();
         _loc1_.verticalAlign = VerticalAlign.MIDDLE;
         _loc1_.gap = 10;
         this.§_-uJ§.layout = _loc1_;
         §_-9H§(this.§_-uJ§,this.unitsInfoSwitchButton);
         §_-9H§(this.§_-uJ§,this.cloudsVisibilitySwitchButton);
         §_-9H§(this.§_-uJ§,this.bagPositionSwitchButton);
         this.leftColumn = new LayoutGroup();
         this.leftColumn.name = "leftColumn";
         var _loc2_:VerticalLayout = new VerticalLayout();
         _loc2_.gap = 5;
         this.leftColumn.layout = _loc2_;
         this.leftColumn.layoutData = new AnchorLayoutData(0,NaN,NaN,5);
         this.rightColumn = new LayoutGroup();
         this.rightColumn.name = "rightColumn";
         var _loc3_:VerticalLayout = new VerticalLayout();
         _loc3_.gap = 5;
         _loc3_.horizontalAlign = HorizontalAlign.RIGHT;
         this.rightColumn.layout = _loc3_;
         this.rightColumn.layoutData = new AnchorLayoutData(0,5);
         addChild(this.leftColumn);
         addChild(this.rightColumn);
         §_-r1t§(this.windContainer,new AnchorLayoutData(0,NaN,NaN,NaN,0));
         _loc4_ = new AnchorLayoutData(-10,NaN,NaN,NaN,0);
         _loc4_.topAnchorDisplayObject = this.windContainer;
         §_-r1t§(this.§_-k1A§ as DisplayObject,_loc4_);
         _loc4_ = new AnchorLayoutData(5,NaN,NaN,NaN,0);
         _loc4_.horizontalCenter = 0;
         _loc4_.topAnchorDisplayObject = this.§_-k1A§ as DisplayObject;
         §_-r1t§(this.weaponHintContainer,_loc4_);
         _loc4_ = new AnchorLayoutData(10,NaN,NaN,10);
         _loc4_.topAnchorDisplayObject = this.leftColumn;
         §_-r1t§(this.§_-BM§,_loc4_);
         _loc4_ = new AnchorLayoutData(10,NaN,NaN,80);
         _loc4_.topAnchorDisplayObject = this.§_-BM§;
         §_-r1t§(this.§_-uJ§,_loc4_);
         if(this.§_-ki§)
         {
            _loc4_ = new AnchorLayoutData(150,10,NaN,NaN);
            _loc4_.topAnchorDisplayObject = this.rightColumn;
            §_-r1t§(this.§_-ki§.getContainer(),_loc4_);
         }
         var _temp_6:* = new AnchorLayoutData(0,NaN,NaN,0,NaN,NaN);
         _loc4_ = new AnchorLayoutData(0,NaN,NaN,0,NaN,NaN);
         _loc4_.leftAnchorDisplayObject = _loc4_.verticalCenterAnchorDisplayObject = this.windContainer.parent;
         §_-r1t§(this.exitButton,_loc4_);
         this.§_-E31§();
      }
      
      private function §_-E31§() : void
      {
         var _loc1_:AnchorLayoutData = null;
         _loc1_ = new AnchorLayoutData(10,10);
         _loc1_.topAnchorDisplayObject = this.rightColumn;
         §_-r1t§(this.screenshotButton,_loc1_);
         _loc1_ = new AnchorLayoutData(10,-this.screenshotButton.width);
         _loc1_.topAnchorDisplayObject = this.screenshotButton;
         _loc1_.rightAnchorDisplayObject = this.screenshotButton;
         §_-r1t§(this.weaponHintButton,_loc1_);
         if(this.§_-K2c§)
         {
            _loc1_ = new AnchorLayoutData(0,15,NaN,NaN,NaN,0);
            _loc1_.rightAnchorDisplayObject = _loc1_.verticalCenterAnchorDisplayObject = this.windContainer.parent;
         }
         else
         {
            _loc1_ = new AnchorLayoutData(-30,NaN,NaN,75);
            _loc1_.topAnchorDisplayObject = this.leftColumn;
         }
         §_-r1t§(this.alertPanel,_loc1_);
         _loc1_ = new AnchorLayoutData(15,NaN,NaN,NaN,0);
         _loc1_.topAnchorDisplayObject = _loc1_.horizontalCenterAnchorDisplayObject = this.controlHintButton;
         §_-r1t§(this.controlHintButton,_loc1_);
      }
      
      override protected function §_-w7§() : void
      {
         this.screenshotButton.layoutData = null;
         this.weaponHintButton.layoutData = null;
         this.controlHintButton.layoutData = null;
         this.alertPanel.layoutData = null;
      }
      
      override protected function §_-vj§() : void
      {
         this.§_-E31§();
      }
      
      public function init(param1:Function, param2:Function) : void
      {
         this.timer.hide();
         this.§_-Q2J§.addEventListener(Event.TRIGGERED,param1);
         this.unitsInfoSwitch.§_-w1f§ = Cookie.getValue(§_-Q2v§.INFO_STATE_VALUE,§_-BO§.§_-EM§) as int;
         this.cloudsVisibilitySwitch.§_-X2D§ = Cookie.getValue(§_-Q2v§.CLOUDS_VISIBILITY_VALUE,§_-81u§.ENABLED) as Boolean;
         this.bagPositionSwitch.§_-m1y§ = Cookie.getValue(§_-Q2v§.BAG_POSITION_VALUE,§_-T15§.DEFAULT) as int;
         §_-X1j§.§_-NA§(this + ".init: unitsInfoSwitch.infoState=" + this.unitsInfoSwitch.§_-w1f§);
         §_-X1j§.§_-NA§(this + ".init: bagPositionSwitch.bagPos=" + this.bagPositionSwitch.§_-m1y§);
         this.§_-Up§.§_-c1v§(false);
         this.§_-Up§.§_-i2y§(§_-53l§);
         this.§_-Up§.§_-E2N§(§_-81m§);
         this.§_-Up§.addEventListener(Event.CHANGE,param2);
         this.§_-Up§.setValue(§_-xp§,false);
         this.hatsPanel.§_-K3A§();
         this.resetLayoutButton.visible = false;
         this.screenshotButton.visible = false;
         this.unitsInfoSwitchButton.visible = false;
         this.cloudsVisibilitySwitchButton.visible = false;
         this.bagPositionSwitchButton.visible = false;
      }
      
      public function §_-9C§(param1:Array) : void
      {
         var _loc5_:§_-D14§ = null;
         var _loc6_:int = 0;
         var _loc2_:Array = [0,0];
         var _loc3_:int = 0;
         var _loc4_:int = int(param1.length);
         while(_loc3_ < _loc4_)
         {
            _loc5_ = param1[_loc3_];
            _loc6_ = _loc2_[_loc5_.§_-A1U§().getTeamId()] > 1 ? 0 : int(_loc5_.§_-A1U§().getTeamId());
            §_-9H§(_loc6_ == 0 ? this.leftColumn : this.rightColumn,_loc5_.getContainer(),-1);
            ++_loc2_[_loc6_];
            _loc5_.getContainer().visible = true;
            _loc3_++;
         }
         this.leftColumn.validate();
         this.rightColumn.validate();
      }
      
      internal function §_-a11§(param1:int, param2:String) : ProgressBarBoss
      {
         var _loc3_:ProgressBarBoss = new ProgressBarBoss(param1,§_-s2a§.§_-K3t§(param2));
         var _loc4_:LayoutGroup = _loc3_.getClip() as LayoutGroup;
         var _temp_1:* = new AnchorLayoutData();
         var _loc5_:AnchorLayoutData = new AnchorLayoutData();
         _loc5_.topAnchorDisplayObject = this.timer.getContainer();
         _loc5_.top = 10;
         _loc5_.horizontalCenter = 0;
         _loc4_.layoutData = _loc5_;
         addChild(_loc4_);
         AnchorLayoutData(ILayoutDisplayObject(this.§_-k1A§).layoutData).top = 15;
         return _loc3_;
      }
      
      override public function §_-Q1a§(param1:Number) : void
      {
         §_-I1m§(this.screenshotButton,param1);
         §_-I1m§(this.hatsPanelContainer,param1);
         §_-I1m§(this.weaponHintButton,param1);
         §_-I1m§(this.controlHintButton,param1);
      }
      
      override protected function §_-U2r§(param1:DisplayObject, param2:Point) : void
      {
         if(param1 is AlertPanel && this.§_-K2c§)
         {
            return;
         }
         if(this.§_-B1O§ && this.§_-M1x§(param1))
         {
            param2.y += this.§_-X2c§;
         }
         super.§_-U2r§(param1,param2);
      }
      
      override public function §_-D1P§() : void
      {
         super.§_-D1P§();
         Cookie.setValue(HAS_RELATIVE_COORDS,true);
         this.§_-B1O§ = true;
      }
      
      override public function §_-E2e§() : void
      {
         super.§_-E2e§();
         Cookie.setValue(HAS_RELATIVE_COORDS,false);
         this.§_-B1O§ = false;
      }
      
      override public function §_-EW§() : void
      {
         super.§_-EW§();
         this.§_-D1P§();
      }
      
      internal function §_-iZ§(param1:DisplayObjectContainer) : void
      {
         var _loc2_:DisplayObject = this.timer.getContainer();
         var _loc3_:Point = Pool.getPoint();
         var _loc4_:Point = Pool.getPoint();
         _loc2_.localToGlobal(_loc3_,_loc4_);
         param1.addChild(_loc2_);
         _loc2_.x = _loc4_.x;
         _loc2_.y = _loc4_.y;
         Pool.putPoint(_loc3_);
         Pool.putPoint(_loc4_);
      }
      
      private function §_-M1x§(param1:Object) : Boolean
      {
         return this.§_-L6§.indexOf(param1) >= 0;
      }
      
      public function §_-V2l§() : void
      {
         this.§_-U2r§(this.hatsPanelContainer,this.§_-nh§);
      }
      
      override public function dispose() : void
      {
         if(this.exitButton)
         {
            this.exitButton.dispose();
         }
         this.exitButton = null;
         if(this.weaponHintButton)
         {
            this.weaponHintButton.dispose();
         }
         this.weaponHintButton = null;
         if(this.controlHintButton)
         {
            this.controlHintButton.dispose();
         }
         this.controlHintButton = null;
         if(this.§_-Q2J§)
         {
            this.§_-Q2J§.dispose();
         }
         this.§_-Q2J§ = null;
         if(this.unitsInfoSwitch)
         {
            this.unitsInfoSwitch.dispose();
         }
         this.unitsInfoSwitch = null;
         if(this.unitsInfoSwitchButton)
         {
            this.unitsInfoSwitchButton.dispose();
         }
         this.unitsInfoSwitchButton = null;
         if(this.cloudsVisibilitySwitch)
         {
            this.cloudsVisibilitySwitch.dispose();
         }
         this.cloudsVisibilitySwitch = null;
         if(this.cloudsVisibilitySwitchButton)
         {
            this.cloudsVisibilitySwitchButton.dispose();
         }
         this.cloudsVisibilitySwitchButton = null;
         if(this.bagPositionSwitch)
         {
            this.bagPositionSwitch.dispose();
         }
         this.bagPositionSwitch = null;
         if(this.bagPositionSwitchButton)
         {
            this.bagPositionSwitchButton.dispose();
         }
         this.bagPositionSwitchButton = null;
         if(this.resetLayoutButton)
         {
            this.resetLayoutButton.dispose();
         }
         this.resetLayoutButton = null;
         if(this.§_-ki§)
         {
            this.§_-ki§.dispose();
            this.§_-ki§ = null;
         }
         if(this.§_-Up§)
         {
            this.§_-Up§.removeEventListeners(Event.CHANGE);
            this.§_-Up§.dispose();
            this.§_-Up§ = null;
         }
         if(this.screenshotButton)
         {
            this.screenshotButton.dispose();
         }
         this.screenshotButton = null;
         if(this.hatsPanel)
         {
            this.hatsPanel.dispose();
         }
         this.hatsPanel = null;
         this.hatsPanelContainer = null;
         if(this.alertPanel)
         {
            this.alertPanel.dispose();
         }
         this.alertPanel = null;
         if(this.timer)
         {
            this.timer.getContainer().removeFromParent();
            this.timer.dispose();
            this.timer = null;
         }
         if(this.wind)
         {
            this.wind.dispose();
         }
         this.wind = null;
         if(this.§_-tw§)
         {
            this.§_-tw§.dispose();
         }
         this.§_-tw§ = null;
         if(this.§_-k1A§)
         {
            this.§_-k1A§.dispose();
         }
         this.§_-k1A§ = null;
         Pool.putPoint(this.§_-nh§);
         this.§_-nh§ = null;
         super.dispose();
      }
   }
}

