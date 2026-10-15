package ru.pragmatix.wormix.gui.screenOverlay.view
{
   import §_-03T§.§_-2p§;
   import §_-63U§.§_-21K§;
   import §_-A1K§.§_-TA§;
   import §_-Ly§.§_-81L§;
   import §_-b1F§.§_-G4§;
   import §_-r1C§.§_-h2B§;
   import feathers.controls.LayoutGroup;
   import flash.net.Socket;
   import flash.utils.ByteArray;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.*;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   public class MobileScreenOverlayDepositIndicator extends LayoutGroup implements §_-v2M§
   {
      
      private var §_-C2C§:DisplayObject;
      
      private var §_-S1X§:DisplayObject;
      
      private var §_-U1U§:Boolean;
      
      private var §_-93Z§:Boolean;
      
      public function MobileScreenOverlayDepositIndicator()
      {
         super();
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         this.§_-C2C§ = §_-YQ§.getClip("active",this);
         this.§_-S1X§ = §_-YQ§.getClip("inactive",this);
         this.§_-C2C§.addEventListener(Event.TRIGGERED,this.§_-y1l§);
         this.§_-S1X§.addEventListener(Event.TRIGGERED,this.§_-y1l§);
      }
      
      override public function dispose() : void
      {
         this.§_-C2C§.removeEventListener(Event.TRIGGERED,this.§_-y1l§);
         this.§_-S1X§.removeEventListener(Event.TRIGGERED,this.§_-y1l§);
      }
      
      override protected function draw() : void
      {
         super.draw();
         if(isInvalid(INVALIDATION_FLAG_DATA))
         {
            visible = this.§_-U1U§;
            if(this.§_-C2C§)
            {
               this.§_-C2C§.visible = this.§_-93Z§;
            }
            if(this.§_-S1X§)
            {
               this.§_-S1X§.visible = !this.§_-93Z§;
            }
         }
      }
      
      private function §_-y1l§(param1:Event) : void
      {
         §_-h2B§.play(§_-81L§.§_-o1Z§);
      }
      
      public function §_-n1M§(param1:Boolean, param2:Boolean) : void
      {
         this.§_-U1U§ = param1;
         this.§_-93Z§ = param2;
         invalidate(INVALIDATION_FLAG_DATA);
      }
   }
}

