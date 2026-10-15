package ru.pragmatix.wormix.gui.screenOverlay.view
{
   import §_-CF§.§_-x2t§;
   import §_-L1H§.§_-C3e§;
   import §_-Ly§.§_-81L§;
   import §_-l1K§.*;
   import §_-r1C§.§_-h2B§;
   import feathers.controls.LayoutGroup;
   import ru.pragmatix.wormix.controller.§_-J2d§;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.ammo.AirCatAmmo;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.events.Event;
   import starlingbuilder.extensions.uicomponents.ImageButton;
   
   public class MobileScreenOverlayVipIndicator extends LayoutGroup implements §_-o2R§
   {
      
      private var §_-P1r§:ImageButton;
      
      private var _data:§_-r1d§;
      
      public function MobileScreenOverlayVipIndicator()
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
               this.§_-P1r§.isEnabled = this._data.userProfile.§_-33n§();
               this.§_-P1r§.useHandCursor = this._data.§_-x2k§;
            }
            else
            {
               this.visible = false;
               this.§_-P1r§.isEnabled = false;
               this.§_-P1r§.useHandCursor = false;
            }
            §_-YQ§.§_-Xv§(this,this.visible);
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

