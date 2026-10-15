package ru.pragmatix.wormix.gui.menu.shop.mobile
{
   import §_-62§.§_-G1p§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.Purchase;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.VerticalAlign;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.server.VerifyPaymentResult;
   import starling.utils.ScaleMode;
   
   public class MobileShopCatecoryItemRenderer extends DefaultListItemRenderer
   {
      
      private var binder:Binder;
      
      public function MobileShopCatecoryItemRenderer()
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
         var _loc1_:Object = Application.assetManager.getObject("shop_menu_cell");
         Application.uiBuilder.create(_loc1_,true,this.binder);
         defaultAccessory = this.binder._content;
         this.binder._icon.scaleMode = ScaleMode.SHOW_ALL;
         this.binder._icon.maintainAspectRatio = true;
         this.binder._icon.horizontalAlign = HorizontalAlign.CENTER;
         this.binder._icon.verticalAlign = VerticalAlign.MIDDLE;
      }
      
      override protected function commitData() : void
      {
         var _loc1_:int = 0;
         var _loc2_:Boolean = false;
         if(_data)
         {
            this.binder._icon.source = Application.assetManager.getTexture(_data.icon);
            this.binder._purchasedLabel.text = §_-s2a§.§_-K3t§(§_-s2a§.CATEGORY_PURCHASED);
            this.binder._nameText.text = §_-s2a§.§_-K3t§(_data.title);
            _loc1_ = int(_data.category);
            _loc2_ = §_-G1p§.§_-O1b§(_loc1_).length == 0;
            this.binder._purchasedInfo.visible = _loc2_;
            this.isEnabled = !_loc2_;
         }
      }
   }
}

import feathers.controls.ImageLoader;
import feathers.controls.Label;
import feathers.controls.LayoutGroup;

class Binder
{
   
   public var _content:LayoutGroup;
   
   public var _purchasedInfo:LayoutGroup;
   
   public var _nameInfo:LayoutGroup;
   
   public var _icon:ImageLoader;
   
   public var _purchasedLabel:Label;
   
   public var _nameText:Label;
   
   public function Binder()
   {
      super();
   }
}
