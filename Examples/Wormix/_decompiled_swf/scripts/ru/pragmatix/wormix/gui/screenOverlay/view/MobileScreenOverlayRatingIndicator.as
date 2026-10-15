package ru.pragmatix.wormix.gui.screenOverlay.view
{
   import §_-B1y§.*;
   import §_-D3A§.§_-TZ§;
   import §_-L1H§.§_-b2K§;
   import §_-Ly§.§_-81L§;
   import §_-Tm§.§_-RF§;
   import §_-Vg§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import feathers.controls.Button;
   import feathers.controls.LayoutGroup;
   import feathers.core.FeathersControl;
   import feathers.data.IListCollection;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.Pool;
   
   public class MobileScreenOverlayRatingIndicator extends LayoutGroup implements §_-o2R§
   {
      
      private var button:Button;
      
      private var §_-O24§:§_-r1d§;
      
      public function MobileScreenOverlayRatingIndicator()
      {
         super();
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         this.button = §_-YQ§.getClip("button",this) as Button;
         this.button.addEventListener(Event.TRIGGERED,this.§_-d1l§);
      }
      
      override protected function draw() : void
      {
         super.draw();
         if(isInvalid(INVALIDATION_FLAG_DATA))
         {
            if(this.§_-O24§)
            {
               this.button.touchable = this.§_-O24§.§_-x2k§;
            }
            else
            {
               this.button.touchable = false;
            }
         }
      }
      
      override public function dispose() : void
      {
         super.dispose();
         this.removeEventListener(Event.TRIGGERED,this.§_-d1l§);
      }
      
      private function §_-d1l§(param1:Event) : void
      {
         §_-h2B§.play(§_-81L§.§_-o1Z§);
         dispatchEventWith(Event.TRIGGERED);
      }
      
      public function setData(param1:§_-r1d§) : void
      {
         this.§_-O24§ = param1;
         invalidate(INVALIDATION_FLAG_DATA);
      }
   }
}

