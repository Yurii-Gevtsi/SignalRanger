package ru.pragmatix.wormix.gui.menu.shop.desktop
{
   import §_-21p§.§_-4w§;
   import §_-533§.§_-c2b§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-L1r§;
   import §_-62§.§_-zF§;
   import §_-73d§.§_-V19§;
   import §_-C3f§.§_-4I§;
   import §_-CF§.§_-E19§;
   import §_-F39§.*;
   import §_-HD§.§_-63z§;
   import §_-L2F§.§_-D2p§;
   import §_-Ly§.§_-81L§;
   import §_-TF§.§_-F1B§;
   import §_-TF§.§_-q1j§;
   import §_-Tm§.§_-RF§;
   import §_-b1F§.§_-G4§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-N1Z§;
   import §_-l1K§.§_-W2f§;
   import §_-q26§.*;
   import §_-r1C§.§_-h2B§;
   import §_-z1g§.§_-F4§;
   import com.distriqt.extension.application.Application;
   import com.distriqt.extension.application.display.Display;
   import com.distriqt.extension.pushnotifications.events.AuthorisationEvent;
   import com.hurlant.math.*;
   import feathers.controls.*;
   import feathers.data.*;
   import feathers.layout.*;
   import flash.text.TextFormatAlign;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.logger.*;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-eP§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.§_-3m§;
   import ru.pragmatix.wormix.messages.clans.*;
   import ru.pragmatix.wormix.messages.clans.request.DeleteClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.QuitClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginCreateClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginJoinClanRequest;
   import ru.pragmatix.wormix.messages.client.AddToGroup;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.RopePhysicModel;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.AssetManager;
   import starling.utils.Color;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   import starlingbuilder.extensions.uicomponents.CustomLabel;
   
   use namespace bi_internal;
   
   public class DesktopShopEquipInfoPanel implements §_-An§
   {
      
      private var _binder:Binder;
      
      private var itemIcon:§_-3m§;
      
      private var §_-e2m§:§_-63z§;
      
      public function DesktopShopEquipInfoPanel()
      {
         super();
         this._binder = new Binder();
         var _loc1_:Object = Application.assetManager.getObject("shop_equip_info");
         Application.uiBuilder.create(_loc1_,true,this._binder);
         this.itemIcon = new §_-3m§(this._binder._itemIcon);
         this.§_-g1r§(this._binder._properties);
         this.§_-g1r§(this._binder._requirements);
         this._binder._properties.itemRendererType = DesktopShopPropertiesListItem;
         this._binder._requirements.itemRendererType = DesktopShopnfoRequirementRenderer;
         this._binder._itemInfo.verticalScrollStep = 2;
         this._binder._itemInfo.horizontalScrollPolicy = ScrollPolicy.OFF;
         this._binder._itemInfo.verticalScrollPolicy = ScrollPolicy.AUTO;
         this._binder._itemInfo.scrollBarDisplayMode = ScrollBarDisplayMode.FIXED_FLOAT;
         this._binder._itemInfo.interactionMode = ScrollInteractionMode.TOUCH_AND_SCROLL_BARS;
         this._binder._itemInfo.verticalScrollBarFactory = §_-eP§.§_-G3p§;
         this.§_-e2m§ = new §_-63z§(this._binder._pricePanel);
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
         var _loc3_:Boolean = false;
         var _loc4_:Array = null;
         var _loc5_:Array = null;
         var _loc2_:§_-L1r§ = param1 as §_-L1r§;
         if(_loc2_)
         {
            _loc3_ = §_-G1p§.§_-61d§(_loc2_);
            this.itemIcon.setItem(_loc2_);
            this._binder._itemName.text = §_-s2a§.§_-K3t§(_loc2_.getName());
            this._binder._description.text = §_-2t§.§_-B34§(_loc2_);
            _loc4_ = this.§_-z2H§(_loc2_);
            (this._binder._properties.dataProvider as ArrayCollection).arrayData = _loc4_;
            _loc5_ = _loc3_ ? [] : §_-F4§.§_-Nd§(_loc2_);
            (this._binder._requirements.dataProvider as ArrayCollection).arrayData = _loc5_;
            this._binder._requirements.validate();
            this._binder._requirements.height = NaN;
            this.§_-e2m§.setItem(_loc2_);
         }
      }
      
      private function §_-z2H§(param1:§_-L1r§) : Array
      {
         var _loc3_:Vector.<§_-4w§> = null;
         var _loc4_:§_-4w§ = null;
         var _loc2_:Array = [];
         if(param1)
         {
            _loc3_ = param1.§_-V1v§().§_-b2I§();
            for each(_loc4_ in _loc3_)
            {
               if(_loc4_.§_-I2a§())
               {
                  _loc2_.push({
                     "icon":_loc4_.getType().§_-M2c§(),
                     "name":§_-s2a§.§_-K3t§(_loc4_.getType().§_-23r§()),
                     "text":_loc4_.§_-I2a§()
                  });
               }
            }
         }
         return _loc2_;
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

class Binder
{
   
   public var _panel:LayoutGroup;
   
   public var _itemIcon:LayoutGroup;
   
   public var _pricePanel:LayoutGroup;
   
   public var _itemName:Label;
   
   public var _description:Label;
   
   public var _itemInfo:Panel;
   
   public var _properties:List;
   
   public var _requirements:List;
   
   public function Binder()
   {
      super();
   }
}
