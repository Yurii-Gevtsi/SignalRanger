package ru.pragmatix.wormix.fx
{
   import ru.pragmatix.wormix.fx.particle.ParticleEffect;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import starling.core.Starling;
   import starling.extensions.ColorArgb;
   
   public class ParticleCloudEffect extends ParticleEffect
   {
      
      public function ParticleCloudEffect(param1:ColorArgb)
      {
         super(ParticleEffectType.CLOUD);
         this.startColor = param1;
         var _loc2_:ColorArgb = new ColorArgb();
         _loc2_.copyFrom(param1);
         _loc2_.alpha = 0;
         this.endColor = _loc2_;
         this.alpha = 0.7;
         Starling.juggler.add(this);
         this.advanceTime(15);
      }
      
      override public function dispose() : void
      {
         super.dispose();
         if(Starling.juggler.contains(this))
         {
            Starling.juggler.remove(this);
         }
      }
      
      override public function advanceTime(param1:Number) : void
      {
         super.advanceTime(param1);
         if(!isEmitting && numParticles == 0)
         {
            Starling.juggler.remove(this);
            if(this.parent)
            {
               this.removeFromParent(true);
            }
         }
      }
   }
}

