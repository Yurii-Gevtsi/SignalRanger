package ru.pragmatix.wormix.gui.menu.shop.desktop
{
   import §_-31N§.§_-Yy§;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-W1r§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-A2F§.§_-4B§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.§_-63i§;
   import §_-C2M§.*;
   import §_-C3f§.§_-1Y§;
   import §_-Cl§.*;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-HD§.§_-63z§;
   import §_-HD§.§_-N1J§;
   import §_-Vg§.ClanCreateView;
   import §_-Vx§.§_-V2t§;
   import §_-Y1O§.*;
   import §_-Y1Q§.§_-aG§;
   import §_-Y1Q§.§_-tc§;
   import §_-Y1b§.§_-jH§;
   import §_-b1R§.§_-Bx§;
   import §_-rM§.§_-61j§;
   import §_-z1g§.§_-F4§;
   import §_-zI§.§_-8D§;
   import §_-zI§.§_-F3q§;
   import feathers.controls.*;
   import feathers.data.*;
   import feathers.events.FeathersEventType;
   import feathers.layout.*;
   import flash.display.Shape;
   import flash.events.Event;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.getQualifiedClassName;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-eP§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.§_-3m§;
   import ru.pragmatix.wormix.messages.clans.request.ClanSummaryRequest;
   import ru.pragmatix.wormix.messages.clans.response.ListClansResponse;
   import ru.pragmatix.wormix.messages.client.ChooseBonusItem;
   import ru.pragmatix.wormix.messages.server.EnterAccount;
   import ru.pragmatix.wormix.messages.structures.*;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-Cq§;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.SystemUtil;
   
   public class DesktopShopWeaponInfoPanel implements §_-An§
   {
      
      private var _binder:Binder;
      
      private var itemIcon:§_-3m§;
      
      private var §_-R1z§:Number;
      
      private var §_-e2m§:§_-63z§;
      
      private var weaponId:int;
      
      public function DesktopShopWeaponInfoPanel()
      {
         super();
         this._binder = new Binder();
         var _loc1_:Object = Application.assetManager.getObject("shop_weapon_info");
         Application.uiBuilder.create(_loc1_,true,this._binder);
         this.itemIcon = new §_-3m§(this._binder._itemIcon);
         this.§_-R1z§ = this._binder._progressBarSkin.width;
         this.§_-g1r§(this._binder._properties);
         this.§_-g1r§(this._binder._nextLevelProperties);
         this.§_-g1r§(this._binder._requirements);
         this._binder._properties.itemRendererType = DesktopShopPropertiesListItem;
         this._binder._nextLevelProperties.itemRendererType = DesktopShopPropertiesListItem;
         this._binder._requirements.itemRendererType = DesktopShopnfoRequirementRenderer;
         this._binder._itemInfo.verticalScrollStep = 2;
         this._binder._itemInfo.horizontalScrollPolicy = ScrollPolicy.OFF;
         this._binder._itemInfo.verticalScrollPolicy = ScrollPolicy.AUTO;
         this._binder._itemInfo.scrollBarDisplayMode = ScrollBarDisplayMode.FIXED_FLOAT;
         this._binder._itemInfo.interactionMode = ScrollInteractionMode.TOUCH_AND_SCROLL_BARS;
         this._binder._itemInfo.verticalScrollBarFactory = §_-eP§.§_-G3p§;
         this.§_-e2m§ = new §_-N1J§(this._binder._pricePanel);
         this._binder._nextLevelDescription.text = §_-s2a§.§_-K3t§(§_-s2a§.WEAPON_AFTER_IMPROVE);
      }
      
      private function §_-g1r§(param1:List) : void
      {
         var list:List = param1;
         var propLayout:VerticalLayout = new VerticalLayout();
         with(propLayout)
         {
            padding = 0;
            gap = 10;
            hasVariableItemDimensions = true;
            horizontalAlign = HorizontalAlign.LEFT;
         }
         with(list)
         {
            layout = propLayout;
            dataProvider = new ArrayCollection();
            verticalScrollPolicy = _binder._properties.horizontalScrollPolicy = ScrollPolicy.OFF;
         }
      }
      
      public function setItem(param1:Object) : void
      {
         var _loc3_:int = 0;
         var _loc4_:Boolean = false;
         var _loc5_:int = 0;
         var _loc6_:int = 0;
         var _loc7_:Array = null;
         var _loc8_:Array = null;
         var _loc9_:Array = null;
         var _loc10_:Number = NaN;
         var _loc2_:§_-O2H§ = param1 as §_-O2H§;
         if(_loc2_)
         {
            _loc3_ = _loc2_.getId();
            _loc4_ = _loc2_.isComplex();
            _loc5_ = _loc4_ ? int(§_-Cq§.§_-sj§(_loc2_.getId())) : -1;
            _loc6_ = int(_loc2_.getMaxLevel());
            this.itemIcon.setItem(_loc2_);
            this._binder._itemName.text = §_-s2a§.§_-K3t§(_loc2_.getName());
            this._binder._levelData.visible = _loc4_;
            if(this._binder._levelData.visible)
            {
               this._binder._levelLabel.text = §_-s2a§.§_-K3t§(§_-s2a§.LEVEL_LABEL);
               this._binder._levelValue.text = _loc5_.toString() + "/" + _loc6_.toString();
               _loc10_ = Number(_loc5_) / Number(_loc6_) * this.§_-R1z§;
               if(this.weaponId == _loc3_)
               {
                  Starling.juggler.tween(this._binder._progressBarSkin,0.25,{"width":_loc10_});
               }
               else
               {
                  this._binder._progressBarSkin.width = _loc10_;
               }
            }
            this._binder._description.text = §_-4B§.§_-p1§(_loc2_);
            _loc7_ = §_-4B§.§_-81h§(_loc2_,_loc5_);
            (this._binder._properties.dataProvider as ArrayCollection).arrayData = _loc7_;
            _loc8_ = §_-4B§.§_-X1A§(_loc2_,_loc5_);
            if(_loc8_.length > 0)
            {
               this._binder._itemInfo.addChild(this._binder._nextLevelDescription);
               this._binder._itemInfo.addChild(this._binder._nextLevelProperties);
            }
            else
            {
               this._binder._nextLevelProperties.removeFromParent();
               this._binder._nextLevelDescription.removeFromParent();
            }
            (this._binder._nextLevelProperties.dataProvider as ArrayCollection).arrayData = _loc8_;
            _loc9_ = §_-F4§.§_-Nd§(_loc2_);
            if(_loc9_.length > 0)
            {
               this._binder._itemInfo.addChild(this._binder._requirements);
               this._binder._requirements.validate();
               this._binder._requirements.height = NaN;
            }
            else
            {
               this._binder._requirements.removeFromParent();
            }
            (this._binder._requirements.dataProvider as ArrayCollection).arrayData = _loc9_;
            this.§_-e2m§.setItem(_loc2_);
            this.weaponId = _loc3_;
         }
      }
      
      public function §_-Bh§() : DisplayObject
      {
         return this._binder._panel;
      }
      
      public function destroy() : void
      {
         this._binder._panel.removeFromParent(true);
         this.§_-e2m§.dispose();
      }
   }
}

import feathers.controls.Label;
import feathers.controls.LayoutGroup;
import feathers.controls.List;
import feathers.controls.Panel;
import starling.display.Image;

class Binder
{
   
   public var _panel:LayoutGroup;
   
   public var _itemIcon:LayoutGroup;
   
   public var _levelData:LayoutGroup;
   
   public var _itemInfo:Panel;
   
   public var _itemName:Label;
   
   public var _description:Label;
   
   public var _nextLevelDescription:Label;
   
   public var _levelLabel:Label;
   
   public var _levelValue:Label;
   
   public var _pricePanel:LayoutGroup;
   
   public var _properties:List;
   
   public var _nextLevelProperties:List;
   
   public var _requirements:List;
   
   public var _progressBarSkin:Image;
   
   public function Binder()
   {
      super();
   }
}
