package ru.pragmatix.wormix.fx.trail
{
   import §_-H2g§.§_-di§;
   import §_-hO§.§_-03k§;
   import §_-l2r§.§_-K1t§;
   import dragonBones.animation.WorldClock;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.utils.FrameTimer;
   import starling.animation.Tween;
   import starling.core.Starling;
   import starling.events.Event;
   
   public class ShadowTrail
   {
      
      private var ft:FrameTimer;
      
      private var gobj:§_-F3o§;
      
      private var className:String;
      
      public function ShadowTrail(param1:§_-F3o§, param2:String, param3:uint = 1)
      {
         super();
         this.gobj = param1;
         this.className = param2;
         this.ft = new FrameTimer(param3);
         this.ft.addEventListener(§_-03k§.§_-836§,this.spawnShadow);
         this.ft.start();
      }
      
      protected function spawnShadow(param1:Event) : void
      {
         var _loc2_:§_-di§ = null;
         var _loc3_:Tween = null;
         if(this.gobj == null || this.gobj.disposed)
         {
            this.remove();
         }
         else
         {
            _loc2_ = §_-K1t§.buildArmature("resurces/_ammo/" + this.className,§_-K1t§.§_-G1E§,§_-K1t§.§_-t1V§);
            _loc2_.display.scaleX = _loc2_.display.scaleY = §_-K1t§.§_-31c§;
            _loc2_.display.x = this.gobj.x;
            _loc2_.display.y = this.gobj.y;
            _loc2_.display.rotation = this.gobj.clip.rotation;
            WorldClock.clock.add(_loc2_);
            _loc2_.playAnimation(§_-K1t§.§_-Y1c§);
            if(_loc2_.getBone(this.className))
            {
               _loc2_.getBone(this.className).armature.animation.gotoAndPlay(§_-K1t§.§_-Y1c§);
            }
            §_-X1j§.§_-12n§.scene.addElementToCertainSortingLayer(_loc2_.display,SortingLayer.EFFECT_LAYER);
            _loc3_ = new Tween(_loc2_.display,0.2);
            _loc3_.fadeTo(0);
            _loc3_.onComplete = this.removeArmature;
            _loc3_.onCompleteArgs = [_loc2_];
            Starling.juggler.add(_loc3_);
         }
      }
      
      private function removeArmature(param1:§_-di§) : void
      {
         if(!param1)
         {
            return;
         }
         if(param1.display)
         {
            param1.display.removeFromParent();
         }
         WorldClock.clock.remove(param1);
         param1.dispose();
      }
      
      public function remove() : void
      {
         if(this.ft.running)
         {
            this.ft.stop();
            this.gobj = null;
         }
      }
   }
}

