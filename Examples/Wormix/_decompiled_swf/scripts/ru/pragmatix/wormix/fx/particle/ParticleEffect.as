package ru.pragmatix.wormix.fx.particle
{
   import starling.core.Starling;
   import starling.extensions.PDParticleSystem;
   
   public class ParticleEffect extends PDParticleSystem
   {
      
      private var lifetime:Number;
      
      private var isFinite:Boolean;
      
      public function ParticleEffect(param1:ParticleEffectType, param2:Number = -1)
      {
         super(param1.getConfigXML(),param1.getTexture());
         this.lifetime = param2;
         this.isFinite = param2 != -1;
         start();
         Starling.juggler.add(this);
      }
      
      override public function advanceTime(param1:Number) : void
      {
         super.advanceTime(param1);
         if(this.isFinite)
         {
            if(this.lifetime < 0)
            {
               if(isEmitting)
               {
                  stop();
               }
            }
            else
            {
               this.lifetime -= param1;
            }
         }
         if(!isEmitting && numParticles == 0)
         {
            removeFromParent(true);
         }
      }
      
      override public function dispose() : void
      {
         Starling.juggler.remove(this);
         super.dispose();
      }
   }
}

