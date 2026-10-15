package ru.pragmatix.wormix.gui.menu
{
   import §_-52V§.§_-R9§;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-T1y§;
   import §_-62§.§_-W1r§;
   import §_-79§.§_-ps§;
   import §_-82l§.§_-A2Y§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.§_-F1P§;
   import §_-B1y§.§_-bs§;
   import §_-B1y§.§_-o2k§;
   import §_-B2z§.§_-63i§;
   import §_-CR§.§_-72p§;
   import §_-Cl§.§_-L2z§;
   import §_-DJ§.§_-fH§;
   import §_-F39§.*;
   import §_-H2g§.§_-di§;
   import §_-H3§.*;
   import §_-HD§.*;
   import §_-Y1O§.*;
   import §_-bI§.*;
   import §_-f1G§.§_-kf§;
   import §_-g2C§.GlobalChatTextMessageItemRenderer;
   import §_-g2C§.§_-62U§;
   import §_-g2C§.§_-I1R§;
   import §_-g2C§.§_-K2A§;
   import §_-g2C§.§_-K2u§;
   import §_-g2C§.§_-p8§;
   import §_-g2C§.§_-w1K§;
   import §_-g2r§.§_-03A§;
   import §_-hv§.*;
   import §_-j1w§.§_-L1x§;
   import §_-k1u§.§_-H2M§;
   import §_-k1u§.§_-Oi§;
   import §_-l1g§.§_-L3Q§;
   import §_-q26§.*;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-J2g§;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.plugins.*;
   import com.hurlant.math.*;
   import feathers.controls.List;
   import feathers.controls.NumericStepper;
   import feathers.controls.ScrollPolicy;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.controls.renderers.IListItemRenderer;
   import feathers.controls.text.TextFieldTextRenderer;
   import feathers.controls.text.TextInstanceCreator;
   import feathers.layout.AnchorLayout;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalLayout;
   import feathers.layout.ILayout;
   import feathers.layout.RelativePosition;
   import feathers.layout.VerticalLayout;
   import feathers.themes.FontSize;
   import feathers.themes.styles.AlternateScrollBarStyles;
   import feathers.utils.textures.TextureCache;
   import flash.display.*;
   import flash.events.*;
   import flash.geom.Point;
   import flash.text.TextFormatAlign;
   import flash.utils.*;
   import org.gestouch.core.*;
   import ru.pragmatix.flox.logger.§_-11S§;
   import ru.pragmatix.flox.social.controller.*;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.§_-s1V§;
   import ru.pragmatix.wormix.gui.screenOverlay.view.§_-B1m§;
   import ru.pragmatix.wormix.messages.clans.request.edit.RenameClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginCreateClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginJoinClanRequest;
   import ru.pragmatix.wormix.messages.clans.response.ListClansResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.BuyReactionRate;
   import ru.pragmatix.wormix.messages.client.PvpChatMessage;
   import ru.pragmatix.wormix.messages.client.StartBattle;
   import ru.pragmatix.wormix.messages.structures.ChatMessage;
   import ru.pragmatix.wormix.messages.structures.RatingProfileStructure;
   import ru.pragmatix.wormix.model.items.craft.§_-IE§;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.model.rewards.§_-qU§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.IPhysicModel;
   import ru.pragmatix.wormix.pvp.messages.ex.PvpBattleType;
   import ru.pragmatix.wormix.social.common.post.*;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import ru.pragmatix.wormix.utils.§_-g20§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.Bullet;
   import ru.pragmatix.wormix.weapons.interfaces.§_-y2Q§;
   import starling.display.Button;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.utils.Pool;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   use namespace gestouch_internal;
   use namespace bi_internal;
   
   public class §_-f2u§ extends §_-k25§
   {
      
      private static const §_-du§:String = "ChooseFriendMenu_dot";
      
      private static const §_-41r§:String = "ArenaMenu_redDot";
      
      private static const §_-p1K§:String = "nextIcon";
      
      private static const §_-01X§:String = "nextIconDisabled";
      
      private var §_-b2F§:String = §_-du§;
      
      private var §_-p2A§:String = §_-41r§;
      
      private var §_-R2X§:Number;
      
      private var §_-43A§:Image;
      
      private var §_-21a§:Button;
      
      private var §_-x2a§:Button;
      
      private var §_-Y2E§:Boolean;
      
      private var list:List;
      
      public function §_-f2u§(param1:List, param2:starling.display.DisplayObjectContainer, param3:Number, param4:Boolean)
      {
         this.list = param1;
         this.§_-Y2E§ = param4;
         this.§_-R2X§ = param3;
         super(param2);
      }
      
      private function §_-w1G§(param1:starling.events.Event) : void
      {
         this.§_-03o§(this.list.horizontalPageCount);
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         this.§_-43A§ = new Image(§_-L2z§.getTexture(this.§_-p2A§));
         _itemRendererFactory = this.§_-XL§;
         this.list.addEventListener(starling.events.Event.CHANGE,this.§_-w1G§);
         if(this.§_-Y2E§)
         {
            this.§_-w3§();
            this.§_-b2U§(this.§_-x2a§,this.§_-21a§);
         }
      }
      
      override public function §_-b2U§(param1:Button, param2:Button) : void
      {
         super.§_-b2U§(param1,param2);
         param1.alphaWhenDisabled = 1;
         param2.alphaWhenDisabled = 1;
      }
      
      override protected function §_-N1g§() : ILayout
      {
         var _loc1_:HorizontalLayout = new HorizontalLayout();
         _loc1_.gap = this.§_-R2X§;
         _loc1_.paddingTop = 2;
         _loc1_.paddingLeft = _loc1_.paddingLeft = 20;
         return _loc1_;
      }
      
      public function §_-w3§() : void
      {
         this.§_-x2a§ = this.§_-B17§(§_-p1K§,§_-01X§);
         this.§_-21a§ = this.§_-B17§(§_-p1K§,§_-01X§);
         this.§_-x2a§.scaleX *= -1;
         this.§_-x2a§.pivotX = this.§_-x2a§.width;
         this.§_-x2a§.x = this.§_-43A§.width * 1.5;
         this.§_-o1C§();
         _paddingLeft = _paddingRight = this.§_-43A§.width * 2;
         addEventListener(starling.events.Event.RESIZE,this.§_-L32§);
         this.§_-L36§();
      }
      
      private function §_-o1C§() : void
      {
         this.§_-21a§.x = width - this.§_-43A§.width * 1.5;
      }
      
      override public function §_-03o§(param1:int) : void
      {
         super.§_-03o§(param1);
         this.§_-L36§();
      }
      
      override public function set selectedIndex(param1:int) : void
      {
         super.selectedIndex = param1;
         this.§_-L36§();
      }
      
      private function §_-L36§() : void
      {
         if(Boolean(_dataProvider && _dataProvider.length) && Boolean(this.§_-x2a§) && Boolean(this.§_-21a§))
         {
            this.§_-x2a§.enabled = selectedIndex > 0;
            this.§_-21a§.enabled = selectedIndex < this._dataProvider.length - 1;
         }
      }
      
      private function §_-L32§(param1:starling.events.Event) : void
      {
         this.§_-o1C§();
      }
      
      private function §_-B17§(param1:String, param2:String) : Button
      {
         var _loc3_:Texture = §_-L2z§.getTexture(param1);
         var _temp_1:* = new Button(_loc3_,"",null,null,§_-L2z§.getTexture(param2));
         var _loc4_:Button = new Button(_loc3_,"",null,null,§_-L2z§.getTexture(param2));
         _loc4_.scale = this.§_-43A§.height * 1.5 / _loc3_.height;
         _loc4_.y = -this.§_-43A§.height * 0.25;
         _loc4_.scaleWhenDown = 1.1;
         addChild(_loc4_);
         return _loc4_;
      }
      
      private function §_-XL§() : starling.display.DisplayObject
      {
         var _loc1_:DefaultListItemRenderer = new DefaultListItemRenderer();
         var _loc2_:Image = new Image(§_-L2z§.getTexture(this.§_-b2F§));
         _loc2_.width = this.§_-43A§.width;
         _loc2_.height = this.§_-43A§.height;
         _loc1_.defaultSelectedSkin = this.§_-43A§;
         _loc1_.defaultSkin = _loc2_;
         _loc1_.defaultLabelProperties.alpha = 0;
         return _loc1_;
      }
      
      override public function dispose() : void
      {
         if(Boolean(this.§_-x2a§) && Boolean(this.§_-21a§))
         {
            removeEventListeners(starling.events.Event.RESIZE);
            this.§_-x2a§ = this.§_-21a§ = null;
         }
         if(this.list)
         {
            this.list.addEventListener(starling.events.Event.CHANGE,this.§_-w1G§);
            this.list = null;
         }
         this.§_-43A§ = null;
         super.dispose();
      }
   }
}

