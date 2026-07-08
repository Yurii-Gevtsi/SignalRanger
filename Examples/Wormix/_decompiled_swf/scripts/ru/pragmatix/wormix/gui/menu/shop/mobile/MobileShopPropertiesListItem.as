package ru.pragmatix.wormix.gui.menu.shop.mobile
{
   import §_-62§.§_-O2H§;
   import §_-62§.§_-W1r§;
   import §_-zI§.*;
   import feathers.controls.Label;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.utils.getQualifiedClassName;
   import ru.pragmatix.flox.logger.ConsolePublisher;
   import ru.pragmatix.flox.logger.§_-93v§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.weapons.Weapon;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.events.Event;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   
   public class MobileShopPropertiesListItem extends DefaultListItemRenderer
   {
      
      private var §_-817§:Rectangle;
      
      private var binder:Binder;
      
      public function MobileShopPropertiesListItem()
      {
         super();
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         _itemHasLabel = false;
         _itemHasIcon = false;
         _itemHasAccessory = false;
         _isQuickHitAreaEnabled = true;
         this.binder = new Binder();
         Application.uiBuilder.create(Application.assetManager.getObject("shop_menu_param_cell"),true,this.binder);
         this.§_-817§ = Pool.getRectangle(this.binder._icon.x,this.binder._icon.y,this.binder._icon.width,this.binder._icon.height);
         this.binder._text.wordWrap = true;
         this.binder._text.textRendererProperties.isHTML = true;
         defaultAccessory = this.binder._container;
      }
      
      override public function dispose() : void
      {
         super.dispose();
         Pool.putRectangle(this.§_-817§);
         this.§_-817§ = null;
      }
      
      override protected function commitData() : void
      {
         if(!data)
         {
            return;
         }
         this.setIcon(data.icon,this.binder._icon);
         this.§_-d1T§(this.binder._title,data.name + ":");
         this.§_-d1T§(this.binder._text,data.text);
      }
      
      private function §_-d1T§(param1:Label, param2:String) : void
      {
         param1.text = param2;
      }
      
      private function setIcon(param1:String, param2:Image) : void
      {
         param2.texture = Application.assetManager.getTexture(param1);
         param2.readjustSize();
         var _loc3_:Rectangle = RectangleUtil.fit(param2.getBounds(param2),this.§_-817§);
         param2.x = _loc3_.x;
         param2.y = _loc3_.y;
         param2.width = _loc3_.width;
         param2.height = _loc3_.height;
         Pool.putRectangle(_loc3_);
      }
   }
}

import feathers.controls.Label;
import feathers.controls.LayoutGroup;
import starling.display.Image;

class Binder
{
   
   public var _container:LayoutGroup;
   
   public var _icon:Image;
   
   public var _title:Label;
   
   public var _text:Label;
   
   public function Binder()
   {
      super();
   }
}
