package ru.pragmatix.wormix.gui.screenOverlay.view
{
   import §_-A1K§.§_-TA§;
   import §_-B2z§.§_-63i§;
   import §_-L1H§.§_-b2K§;
   import §_-N8§.§_-o2w§;
   import §_-Z1w§.§_-w1D§;
   import §_-l1g§.§_-F1X§;
   import §_-n1w§.*;
   import §_-wD§.*;
   import com.distriqt.extension.inappbilling.events.ProductEvent;
   import feathers.controls.LayoutGroup;
   import feathers.core.IToggle;
   import feathers.skins.ImageSkin;
   import ru.pragmatix.wormix.controller.§_-S2Y§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.server.IncreaseAchievementResult;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.utils.Pool;
   import starlingbuilder.extensions.uicomponents.ToggleImageButton;
   
   public class MobileScreenOverlayNavButton extends LayoutGroup implements IToggle
   {
      
      private var _button:ToggleImageButton;
      
      private var _data:§_-B1m§;
      
      public function MobileScreenOverlayNavButton()
      {
         super();
      }
      
      override protected function initialize() : void
      {
         var _loc1_:Sprite = null;
         super.initialize();
         _loc1_ = getChildByName("root") as Sprite;
         _loc1_.getChildByName("back").touchable = false;
         this._button = _loc1_.getChildByName("toggleButton") as ToggleImageButton;
         this._button.addEventListener(Event.CHANGE,this.§_-31T§);
      }
      
      override protected function draw() : void
      {
         var _loc1_:Texture = null;
         var _loc2_:Texture = null;
         var _loc3_:ImageSkin = null;
         super.draw();
         if(isInvalid(INVALIDATION_FLAG_DATA))
         {
            if(this._data)
            {
               _loc1_ = Application.assetManager.getTexture(this._data.iconName + "_up");
               _loc2_ = Application.assetManager.getTexture(this._data.iconName + "_down");
               _loc3_ = new ImageSkin(_loc1_);
               _loc3_.selectedTexture = _loc2_;
               this._button.defaultIcon = _loc3_;
            }
            else
            {
               this._button.defaultIcon = null;
            }
         }
      }
      
      private function §_-31T§(param1:Event) : void
      {
         dispatchEventWith(Event.CHANGE);
      }
      
      public function get isSelected() : Boolean
      {
         return this._button ? this._button.isSelected : false;
      }
      
      public function set isSelected(param1:Boolean) : void
      {
         if(this._button)
         {
            this._button.isSelected = param1;
         }
      }
      
      public function get data() : §_-B1m§
      {
         return this._data;
      }
      
      public function set data(param1:§_-B1m§) : void
      {
         if(this._data != param1)
         {
            this._data = param1;
            invalidate(INVALIDATION_FLAG_DATA);
         }
      }
   }
}

