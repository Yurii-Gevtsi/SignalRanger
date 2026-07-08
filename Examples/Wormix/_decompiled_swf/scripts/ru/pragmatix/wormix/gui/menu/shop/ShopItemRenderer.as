package ru.pragmatix.wormix.gui.menu.shop
{
   import §_-31W§.*;
   import §_-5Y§.§_-h2c§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-L1r§;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-W1r§;
   import §_-72j§.§_-Z6§;
   import §_-72j§.§_-i2G§;
   import §_-73I§.§_-q15§;
   import §_-931§.§_-sz§;
   import §_-A1K§.§_-TA§;
   import §_-A2U§.§_-A23§;
   import §_-B2z§.Server;
   import §_-B2z§.§_-c1G§;
   import §_-C3f§.§_-1Y§;
   import §_-CF§.§_-x2t§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-H2g§.§_-di§;
   import §_-H3§.*;
   import §_-TF§.§_-Z1U§;
   import §_-TF§.§_-q1j§;
   import §_-TF§.§_-y2n§;
   import §_-Ta§.*;
   import §_-Tm§.Action;
   import §_-Tm§.§_-K1I§;
   import §_-Tm§.§_-RF§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-o14§.§_-Dd§;
   import §_-r1C§.§_-h2B§;
   import §_-u2J§.§_-T1K§;
   import §_-z1g§.§_-F4§;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.events.TweenEvent;
   import com.greensock.plugins.*;
   import com.hurlant.math.BigInteger;
   import config.§_-xD§;
   import feathers.controls.ImageLoader;
   import feathers.controls.ScrollPolicy;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.controls.renderers.IListItemRenderer;
   import feathers.controls.text.BitmapFontTextRenderer;
   import feathers.core.ITextRenderer;
   import feathers.data.ArrayCollection;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.HorizontalLayout;
   import feathers.layout.RelativePosition;
   import feathers.layout.VerticalAlign;
   import feathers.text.BitmapFontTextFormat;
   import flash.display.*;
   import flash.events.*;
   import flash.geom.Rectangle;
   import flash.utils.*;
   import ru.pragmatix.flox.social.controller.ISocialController;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.structures.ProfileDoubleKeyStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.model.NotStuckingPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-Cq§;
   import ru.pragmatix.wormix.weapons.*;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.events.Event;
   import starling.utils.AssetManager;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   import starling.utils.SystemUtil;
   
   public class ShopItemRenderer extends DefaultListItemRenderer implements §_-Z1U§
   {
      
      public static const §_-MG§:Number = 15;
      
      private var binder:Binder;
      
      private var icon:starling.display.DisplayObject;
      
      private var §_-m1Q§:§_-di§;
      
      public function ShopItemRenderer()
      {
         super();
      }
      
      override protected function initialize() : void
      {
         var data:Object;
         var priceBack:Image;
         var priceListLayout:HorizontalLayout;
         var scale:Number;
         super.initialize();
         _isQuickHitAreaEnabled = true;
         _isToggle = false;
         this.binder = new Binder();
         data = Application.assetManager.getObject("shop_cell_renderer");
         Application.uiBuilder.create(data,true,this.binder);
         this.addChild(this.binder._panel);
         this.binder._panel.touchable = false;
         priceBack = new Image(Application.assetManager.getTexture("ShopCell_back_normal_bottomShade"));
         priceBack.scale9Grid = Pool.getRectangle(7,6,1,3);
         this.binder._priceList.backgroundSkin = priceBack;
         priceListLayout = new HorizontalLayout();
         priceListLayout.horizontalAlign = HorizontalAlign.CENTER;
         priceListLayout.verticalAlign = VerticalAlign.MIDDLE;
         priceListLayout.gap = 5;
         this.binder._priceList.layout = priceListLayout;
         this.binder._priceList.dataProvider = new ArrayCollection();
         this.binder._priceList.itemRendererFactory = function():IListItemRenderer
         {
            var itemRenderer:DefaultListItemRenderer = new DefaultListItemRenderer();
            itemRenderer.labelField = "label";
            itemRenderer.labelFactory = function():ITextRenderer
            {
               var _loc1_:BitmapFontTextRenderer = new BitmapFontTextRenderer();
               _loc1_.textFormat = new BitmapFontTextFormat(BitmapFonts.GROBOLD_NORMAL,25,7166536);
               _loc1_.height = 25;
               return _loc1_;
            };
            itemRenderer.iconSourceField = "icon";
            itemRenderer.iconLoaderFactory = function():ImageLoader
            {
               var _loc1_:ImageLoader = new ImageLoader();
               _loc1_.width = 40;
               _loc1_.height = 40;
               _loc1_.maintainAspectRatio = true;
               _loc1_.scaleMode = ScaleMode.SHOW_ALL;
               return _loc1_;
            };
            itemRenderer.gap = 5;
            return itemRenderer;
         };
         this.binder._priceList.horizontalScrollPolicy = ScrollPolicy.OFF;
         this.binder._priceList.verticalScrollPolicy = ScrollPolicy.OFF;
         scale = Math.min(width / this.binder._panel.width,height / this.binder._panel.height);
         this.scale = scale;
         this.width = this.binder._panel.width * scale;
         this.height = this.binder._panel.height * scale;
         if(SystemUtil.isDesktop)
         {
            §_-q1j§.register(this);
         }
      }
      
      override protected function commitData() : void
      {
         var _loc2_:Boolean = false;
         var _loc3_:Boolean = false;
         var _loc4_:int = 0;
         var _loc5_:Boolean = false;
         var _loc6_:Boolean = false;
         var _loc7_:AssetManager = null;
         var _loc8_:Rectangle = null;
         var _loc1_:§_-W1r§ = _data as §_-W1r§;
         if(_loc1_)
         {
            this.binder._title.text = §_-s2a§.§_-K3t§(_loc1_.getName());
            if(this.icon)
            {
               this.icon.removeFromParent(true);
            }
            if(this.§_-m1Q§)
            {
               this.§_-m1Q§.dispose();
            }
            if(_loc1_ is §_-O2H§)
            {
               this.icon = §_-q15§.§_-82W§(_loc1_ as §_-O2H§);
               this.icon.alignPivot();
               _loc8_ = Pool.getRectangle();
               RectangleUtil.fit(this.icon.bounds,this.binder._iconContainer.bounds,ScaleMode.SHOW_ALL,false,_loc8_);
               this.icon.width = _loc8_.width;
               this.icon.height = _loc8_.height;
               this.icon.x = this.binder._iconContainer.width * 0.5;
               this.icon.y = this.binder._iconContainer.height * 0.5;
               Pool.putRectangle(_loc8_);
            }
            else if(_loc1_ is §_-L1r§)
            {
               this.§_-m1Q§ = §_-q15§.§_-zE§(_loc1_ as §_-L1r§);
               this.icon = this.§_-m1Q§.display;
               this.icon.scale = 0.7;
               this.icon.x = this.binder._iconContainer.width * 0.5;
               this.icon.y = this.binder._iconContainer.height * 0.5 + §_-MG§;
            }
            this.binder._iconContainer.addChild(this.icon);
            _loc2_ = §_-G1p§.§_-61d§(_loc1_);
            _loc3_ = §_-Cq§.§_-hr§(_loc1_ as §_-O2H§);
            _loc4_ = _loc1_.§_-T1J§() > Application.userProfile.getLevel() ? int(_loc1_.§_-T1J§()) : 0;
            _loc5_ = !§_-F4§.§_-EY§(_loc1_ as §_-1Y§) || !_loc1_.§_-gc§() && !§_-F4§.§_-f16§(_loc1_ as §_-1Y§);
            _loc6_ = !_loc2_ && !_loc3_ && _loc1_ is §_-O2H§ && _loc4_ > 0;
            this.binder._lockLevel.text = _loc4_ > 0 ? _loc4_.toString() : "";
            this.binder._lock.visible = _loc5_ || _loc6_;
            this.binder._purchased.visible = _loc2_;
            this.binder._hasImprovements.visible = _loc3_;
            this.binder._priceList.dataProvider.removeAll();
            _loc7_ = Application.assetManager;
            if(_loc1_.§_-91e§() > 0)
            {
               this.binder._priceList.dataProvider.addItem({
                  "icon":_loc7_.getTexture("icon_fuzy"),
                  "label":_loc1_.§_-91e§().toString()
               });
            }
            if(_loc1_.§_-gc§() > 0)
            {
               this.binder._priceList.dataProvider.addItem({
                  "icon":_loc7_.getTexture("icon_ruby"),
                  "label":_loc1_.§_-gc§().toString()
               });
            }
         }
      }
      
      override public function set isSelected(param1:Boolean) : void
      {
         super.isSelected = param1;
         if(_isInitialized)
         {
            this.binder._selectionFrame.visible = param1;
         }
      }
      
      override public function dispose() : void
      {
         super.dispose();
         if(this.icon)
         {
            this.icon.removeFromParent(true);
         }
         if(this.§_-m1Q§)
         {
            this.§_-m1Q§.dispose();
         }
         if(SystemUtil.isDesktop)
         {
            §_-q1j§.remove(this);
         }
      }
      
      public function getOrigin() : starling.display.DisplayObject
      {
         return this;
      }
      
      public function §_-8I§() : starling.display.DisplayObject
      {
         return §_-y2n§.§_-i1N§.§_-8N§(data as §_-W1r§);
      }
      
      public function §_-52e§() : Object
      {
         return new <String>[RelativePosition.BOTTOM,RelativePosition.TOP];
      }
   }
}

import feathers.controls.LayoutGroup;
import feathers.controls.List;
import starling.display.Image;
import starlingbuilder.extensions.uicomponents.CustomLabel;

class Binder
{
   
   public var _panel:LayoutGroup;
   
   public var _iconContainer:LayoutGroup;
   
   public var _lock:LayoutGroup;
   
   public var _priceList:List;
   
   public var _title:CustomLabel;
   
   public var _lockLevel:CustomLabel;
   
   public var _selectionFrame:Image;
   
   public var _hasImprovements:Image;
   
   public var _purchased:Image;
   
   public function Binder()
   {
      super();
   }
}
