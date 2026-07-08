package ru.pragmatix.wormix.gui.menu.shop.desktop
{
   import §_-62§.§_-O2H§;
   import §_-Bv§.§_-G3P§;
   import §_-Ly§.§_-81L§;
   import §_-RE§.§_-D3v§;
   import §_-Y1O§.*;
   import §_-Z1K§.§_-q24§;
   import §_-f1G§.*;
   import §_-k1u§.§_-F3H§;
   import §_-k1u§.§_-Oi§;
   import §_-kP§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-lh§.§_-z2A§;
   import §_-s1s§.TeamRoomTeammateItemRenderer;
   import §_-s1s§.§_-A1o§;
   import §_-t1w§.*;
   import §_-z1g§.§_-y1S§;
   import com.hurlant.math.BigInteger;
   import config.§_-V2w§;
   import dragonBones.Slot;
   import feathers.controls.Label;
   import feathers.controls.TabBar;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.data.ArrayCollection;
   import flash.display.Shape;
   import flash.events.TimerEvent;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.net.URLRequest;
   import flash.net.navigateToURL;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.Timer;
   import org.gestouch.core.§_-H3d§;
   import org.gestouch.core.gestouch_internal;
   import org.swiftsuspenders.*;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.model.items.craft.§_-6S§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-41p§;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.weapons.ammo.phase.PoisonPhase;
   import starling.display.Image;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   
   use namespace gestouch_internal;
   
   public class DesktopShopPropertiesListItem extends DefaultListItemRenderer
   {
      
      private var §_-817§:Rectangle;
      
      private var binder:Binder;
      
      public function DesktopShopPropertiesListItem()
      {
         super();
         this.binder = new Binder();
         Application.uiBuilder.create(Application.assetManager.getObject("shop_menu_param_cell"),true,this.binder);
         addChild(this.binder._container);
         this.§_-817§ = Pool.getRectangle(this.binder._icon.x,this.binder._icon.y,this.binder._icon.width,this.binder._icon.height);
      }
      
      override public function dispose() : void
      {
         super.dispose();
         Pool.putRectangle(this.§_-817§);
         this.§_-817§ = null;
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         this.binder._text.wordWrap = true;
         this.binder._text.textRendererProperties.isHTML = true;
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
         this.binder._container.width = NaN;
         this.binder._container.height = NaN;
         this.binder._container.readjustLayout();
         this.binder._container.validate();
         this.width = this.binder._container.width;
         this.height = this.binder._container.height;
      }
      
      private function §_-d1T§(param1:Label, param2:String) : void
      {
         param1.text = param2;
         param1.height = NaN;
         param1.validate();
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
