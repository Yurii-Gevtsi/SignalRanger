package ru.pragmatix.wormix.gui.screenOverlay.view
{
   import §_-73d§.§_-v1o§;
   import §_-Ly§.§_-81L§;
   import §_-YF§.§_-q2v§;
   import §_-l1K§.§_-W2f§;
   import §_-r1C§.§_-h2B§;
   import §_-x2Q§.§_-dU§;
   import feathers.controls.LayoutGroup;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.AbstractAuraEffect;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.structures.RatingProfileStructure;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionData;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   import starlingbuilder.extensions.uicomponents.CustomLabel;
   
   public class DesktopScreenOverlayRatingIndicator extends LayoutGroup implements §_-o2R§
   {
      
      private var _ratingValue:CustomLabel;
      
      private var §_-c22§:ContainerButtonWithStates;
      
      private var §_-O24§:§_-r1d§;
      
      public function DesktopScreenOverlayRatingIndicator()
      {
         super();
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         this._ratingValue = §_-YQ§.getClip("value",this) as CustomLabel;
         this._ratingValue.maxWidth = 150;
         this._ratingValue.width = NaN;
         this.§_-c22§ = §_-YQ§.getClip("button",this) as ContainerButtonWithStates;
         this.§_-c22§.addEventListener(Event.TRIGGERED,this.§_-d1l§);
      }
      
      override protected function draw() : void
      {
         super.draw();
         if(isInvalid(INVALIDATION_FLAG_DATA))
         {
            if(this.§_-O24§)
            {
               this.§_-c22§.enabled = this.§_-O24§.§_-x2k§;
               this._ratingValue.text = this.§_-O24§.userProfile.§_-Zp§().toString();
            }
            else
            {
               this.§_-c22§.enabled = false;
               this._ratingValue.text = "";
            }
         }
         if(isInvalid(§_-dU§.§_-h26§))
         {
            this.§_-c22§.toolTip = §_-s2a§.§_-K3t§(§_-s2a§.RATING_QTIP);
            this._ratingValue.toolTip = §_-s2a§.§_-K3t§(§_-s2a§.RATING_QTIP);
         }
      }
      
      override public function dispose() : void
      {
         super.dispose();
         this.§_-c22§.removeEventListener(Event.TRIGGERED,this.§_-d1l§);
      }
      
      private function §_-d1l§(param1:Event) : void
      {
         §_-h2B§.play(§_-81L§.§_-o1Z§);
         param1.stopPropagation();
         dispatchEventWith(Event.TRIGGERED);
      }
      
      public function setData(param1:§_-r1d§) : void
      {
         this.§_-O24§ = param1;
         invalidate(INVALIDATION_FLAG_DATA);
      }
   }
}

