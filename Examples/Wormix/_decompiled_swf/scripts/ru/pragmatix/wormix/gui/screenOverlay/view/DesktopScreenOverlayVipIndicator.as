package ru.pragmatix.wormix.gui.screenOverlay.view
{
   import §_-Ly§.§_-81L§;
   import §_-r1C§.§_-h2B§;
   import §_-x2Q§.§_-dU§;
   import feathers.controls.LayoutGroup;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.events.Event;
   import starlingbuilder.extensions.uicomponents.ImageButton;
   
   public class DesktopScreenOverlayVipIndicator extends LayoutGroup implements §_-o2R§
   {
      
      private var §_-P1r§:ImageButton;
      
      private var _data:§_-r1d§;
      
      public function DesktopScreenOverlayVipIndicator()
      {
         super();
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         this.§_-P1r§ = §_-YQ§.getClip("button",this) as ImageButton;
         this.§_-P1r§.addEventListener(Event.TRIGGERED,this.§_-dc§);
      }
      
      override public function dispose() : void
      {
         super.dispose();
         if(this.§_-P1r§)
         {
            this.§_-P1r§.removeEventListener(Event.TRIGGERED,this.§_-dc§);
            this.§_-P1r§ = null;
         }
      }
      
      override protected function draw() : void
      {
         super.draw();
         if(isInvalid(INVALIDATION_FLAG_DATA))
         {
            if(this._data)
            {
               this.visible = this._data.userProfile.§_-33n§();
               this.§_-P1r§.isEnabled = this._data.§_-x2k§;
               this.§_-P1r§.useHandCursor = this._data.§_-x2k§;
            }
            else
            {
               this.visible = false;
               this.§_-P1r§.isEnabled = false;
               this.§_-P1r§.useHandCursor = false;
            }
         }
         if(isInvalid(§_-dU§.§_-h26§))
         {
            this.§_-P1r§.toolTip = §_-s2a§.§_-K3t§(§_-s2a§.VIP_QTIP);
         }
      }
      
      private function §_-dc§(param1:Event) : void
      {
         §_-h2B§.play(§_-81L§.§_-o1Z§);
         dispatchEventWith(Event.TRIGGERED);
      }
      
      public function setData(param1:§_-r1d§) : void
      {
         this._data = param1;
         invalidate(INVALIDATION_FLAG_DATA);
      }
   }
}

