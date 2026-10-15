package ru.pragmatix.wormix.gui.screenOverlay.view
{
   import §_-B1y§.§_-83E§;
   import §_-Ly§.§_-81L§;
   import §_-P1B§.§_-b21§;
   import §_-Z1K§.§_-q24§;
   import §_-k1u§.§_-2m§;
   import §_-k1u§.§_-Oi§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Ia§;
   import §_-w2i§.§_-Zd§;
   import §_-x2Q§.§_-dU§;
   import feathers.controls.LayoutGroup;
   import flash.events.IEventDispatcher;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-Cq§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class DesktopScreenOverlayDepositIndicator extends LayoutGroup implements §_-v2M§
   {
      
      private var §_-C2C§:DisplayObject;
      
      private var §_-S1X§:DisplayObject;
      
      private var §_-U1U§:Boolean;
      
      private var §_-93Z§:Boolean;
      
      public function DesktopScreenOverlayDepositIndicator()
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
         this.useHandCursor = true;
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
         if(isInvalid(§_-dU§.§_-h26§))
         {
            this.toolTip = §_-s2a§.§_-K3t§(§_-s2a§.DEPOSIT_QTIP);
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

