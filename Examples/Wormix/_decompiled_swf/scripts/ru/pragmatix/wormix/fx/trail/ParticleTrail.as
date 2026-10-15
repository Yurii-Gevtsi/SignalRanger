package ru.pragmatix.wormix.fx.trail
{
   import §_-l1g§.§_-L3Q§;
   import ru.pragmatix.wormix.fx.particle.ParticleEffect;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   public class ParticleTrail extends ParticleEffect
   {
      
      private var gobj:§_-F3o§;
      
      public function ParticleTrail(param1:ParticleEffectType, param2:§_-F3o§)
      {
         super(param1,-1);
         this.gobj = param2;
         _emitterX = param2.x;
         _emitterY = param2.y;
         param2.addEventListener(§_-L3Q§.DISPOSED,this.onTargetDisposed);
      }
      
      override public function advanceTime(param1:Number) : void
      {
         var _loc2_:DisplayObject = null;
         if(isEmitting)
         {
            _loc2_ = this.gobj.clip;
            if(_loc2_)
            {
               _emitterX = _loc2_.x;
               _emitterY = _loc2_.y;
            }
            else
            {
               _emitterX = this.gobj.x;
               _emitterY = this.gobj.y;
            }
         }
         super.advanceTime(param1);
      }
      
      private function onTargetDisposed(param1:Event) : void
      {
         this.gobj.removeEventListener(§_-L3Q§.DISPOSED,this.onTargetDisposed);
         stop();
      }
      
      override public function dispose() : void
      {
         if(this.gobj)
         {
            this.gobj.removeEventListener(§_-L3Q§.DISPOSED,this.onTargetDisposed);
            this.gobj = null;
         }
         super.dispose();
      }
   }
}

