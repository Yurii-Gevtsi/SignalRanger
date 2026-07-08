package ru.pragmatix.wormix.weapons
{
   import §_-Y1O§.*;
   import §_-hO§.§_-03k§;
   import §_-l1K§.§_-cZ§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import flash.display.Shape;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.utils.FrameTimer;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import ru.pragmatix.wormix.weapons.ammo.VerticalDrillMissile;
   import ru.pragmatix.wormix.weapons.ammo.VerticalDrillStanding;
   import starling.display.Image;
   import starling.events.Event;
   
   public class Firework extends Weapon
   {
      
      protected var §_-l1w§:int;
      
      private var §_-n1O§:Image;
      
      private var §_-t1e§:int = 0;
      
      public function Firework()
      {
         super();
         MIN_POWER = MAX_POWER = 20;
         this.§_-l1w§ = §_-z1W§(new §_-cZ§());
      }
      
      override public function activate() : void
      {
         super.activate();
         §_-K2b§();
      }
      
      override public function deactivate() : void
      {
         super.deactivate();
         §_-G1Z§();
         if(this.§_-n1O§ != null)
         {
            §_-X1j§.§_-12n§.scene.removeElement(this.§_-n1O§);
            this.§_-n1O§ = null;
         }
      }
      
      override protected function onFrame(param1:Event) : void
      {
         this.§_-82D§();
      }
      
      override protected function shoot() : void
      {
         var ft:FrameTimer;
         var dummy:§_-Qi§ = null;
         var unit:§_-01J§ = null;
         dummy = new VerticalDrillStanding(owner);
         if(!dummy || !dummy.§_-Y2I§ || !owner || !owner.getPhysicModel())
         {
            return;
         }
         ft = new FrameTimer(84,1);
         unit = owner;
         ft.addEventListener(§_-03k§.§_-cE§,function(param1:Event):void
         {
            nextTurnOnStable();
            if(Boolean(dummy) && !dummy.disposed)
            {
               §_-h2B§.play(§_-d27§.§_-a24§);
            }
            if(dummy)
            {
               if(dummy.§_-Y2I§)
               {
                  new VerticalDrillMissile(unit,dummy.x,dummy.y - 40,paramSet);
                  dummy.disposed = true;
               }
            }
         });
         ft.start();
         detach();
      }
      
      private function §_-82D§() : void
      {
         var _loc1_:Shape = null;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         var _loc4_:int = 0;
         if(owner != null)
         {
            _loc1_ = new Shape();
            _loc1_.graphics.lineStyle(1,16777215);
            this.§_-t1e§ += 2;
            if(this.§_-t1e§ >= 30)
            {
               this.§_-t1e§ = 0;
            }
            _loc1_.graphics.beginFill(16777215);
            _loc2_ = 1;
            while(_loc2_ < 10)
            {
               _loc3_ = owner.x + (owner.direction == §_-8K§.LEFT ? 9 : -9);
               _loc4_ = owner.y - 10 - (30 * _loc2_ + this.§_-t1e§);
               _loc1_.graphics.drawCircle(_loc3_,_loc4_,3);
               _loc2_++;
            }
            this.§_-n1O§ = §_-c1S§.§_-ku§(_loc1_,this.§_-n1O§);
         }
      }
   }
}

