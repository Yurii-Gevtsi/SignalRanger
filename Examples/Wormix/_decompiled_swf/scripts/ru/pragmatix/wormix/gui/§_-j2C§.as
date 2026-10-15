package ru.pragmatix.wormix.gui
{
   import §_-21p§.§_-4w§;
   import §_-43V§.Random;
   import §_-62§.§_-W1r§;
   import §_-63U§.§_-y2O§;
   import §_-C2M§.*;
   import §_-C2t§.§_-Ui§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-fH§;
   import §_-L1H§.§_-C3e§;
   import §_-S1N§.§_-83P§;
   import §_-S1N§.§_-aC§;
   import §_-S1N§.§_-pe§;
   import §_-S1N§.§_-zk§;
   import §_-TF§.§_-q1j§;
   import §_-Y1b§.§_-OP§;
   import §_-aM§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-o14§.*;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.events.ApplicationReceiptEvent;
   import com.distriqt.extension.inappbilling.events.AvailabilityEvent;
   import com.distriqt.extension.inappbilling.events.InAppBillingEvent;
   import com.distriqt.extension.inappbilling.events.ProductEvent;
   import com.distriqt.extension.inappbilling.events.PurchaseEvent;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.plugins.*;
   import com.hurlant.math.*;
   import com.hurlant.util.§_-a2P§;
   import feathers.controls.TabBar;
   import feathers.controls.ToggleButton;
   import feathers.core.ITextRenderer;
   import feathers.layout.VerticalAlign;
   import flash.display.*;
   import flash.events.*;
   import flash.net.URLRequest;
   import flash.net.navigateToURL;
   import flash.utils.*;
   import ru.pragmatix.flox.logger.§_-93v§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.utils.§_-R2g§;
   import starling.display.DisplayObject;
   import starling.display.Image;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.utils.Pool;
   import starling.utils.ScaleMode;
   import starling.utils.SystemUtil;
   
   use namespace bi_internal;
   
   public class §_-j2C§ extends TabBar
   {
      
      private static const DEFAULT_TAB_SKIN:String = "ChooseFriendMenu_tab_downState";
      
      private static const DEFAULT_SELECTED_TAB_SKIN:String = "ChooseFriendMenu_tab_upState";
      
      private var tabWidth:Number;
      
      private var tabHeight:Number;
      
      public function §_-j2C§(param1:Number, param2:Number, param3:starling.display.DisplayObject)
      {
         var tabWidth:Number = param1;
         var tabHeight:Number = param2;
         var placeholder:starling.display.DisplayObject = param3;
         super();
         this.tabWidth = tabWidth;
         this.tabHeight = tabHeight;
         gap = 10;
         tabFactory = function():ToggleButton
         {
            return skinnedTabFactory(DEFAULT_TAB_SKIN,DEFAULT_SELECTED_TAB_SKIN);
         };
         if(placeholder)
         {
            x = placeholder.x;
            y = placeholder.y;
            placeholder.parent.addChild(this);
         }
      }
      
      public function §_-4M§(param1:Object) : void
      {
         if(dataProvider.length > 0)
         {
            selectedItem = param1;
            dispatchEventWith(starling.events.Event.CHANGE);
         }
      }
      
      protected function skinnedTabFactory(param1:String, param2:String = "") : ToggleButton
      {
         var button:ToggleButton = null;
         var skinName:String = param1;
         var pressedSkinName:String = param2;
         button = new ToggleButton();
         button.defaultSkin = this.makeScale3Image(skinName);
         if(pressedSkinName != "")
         {
            button.defaultSelectedSkin = this.makeScale9Image(pressedSkinName);
         }
         button.labelFactory = function():ITextRenderer
         {
            return §_-R2g§.createTextRenderer(BitmapFonts.AD_LIB,18,8284506);
         };
         return button;
      }
      
      private function makeScale3Image(param1:String) : starling.display.DisplayObject
      {
         var _loc2_:Texture = Application.assetManager.getTexture(param1);
         var _loc3_:Image = new Image(_loc2_);
         _loc3_.scale9Grid = Pool.getRectangle(2,Math.floor(_loc2_.height * 0.5 - 1),1,1);
         return this.§_-M2o§(_loc3_);
      }
      
      private function makeScale9Image(param1:String) : starling.display.DisplayObject
      {
         var _loc2_:Texture = Application.assetManager.getTexture(param1);
         var _loc3_:Image = new Image(_loc2_);
         _loc3_.scale9Grid = Pool.getRectangle(7,7,1,1);
         return this.§_-M2o§(_loc3_);
      }
      
      private function §_-M2o§(param1:Object) : starling.display.DisplayObject
      {
         param1.width = this.tabWidth;
         param1.height = this.tabHeight;
         return param1 as starling.display.DisplayObject;
      }
   }
}

