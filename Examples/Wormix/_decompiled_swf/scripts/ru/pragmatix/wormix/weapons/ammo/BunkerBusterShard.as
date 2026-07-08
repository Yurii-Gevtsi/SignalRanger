package ru.pragmatix.wormix.weapons.ammo
{
   import §_-L1H§.§_-C3e§;
   import §_-l1g§.§_-L3Q§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class BunkerBusterShard extends §_-Qi§
   {
      
      private var counter:uint = 4;
      
      public function BunkerBusterShard(param1:§_-01J§, param2:int, param3:int, param4:Number, param5:uint, param6:§_-C3e§)
      {
         var self:§_-Qi§ = null;
         var owner:§_-01J§ = param1;
         var baseX:int = param2;
         var baseY:int = param3;
         var angle:Number = param4;
         var power:uint = param5;
         var paramSet:§_-C3e§ = param6;
         super(owner,baseX,baseY,true);
         initialize(Pool.getPoint(baseX,baseY),paramSet.ammoClassName);
         §_-z1H§.scaleX = §_-z1H§.scaleY = §_-z1H§.scaleY * 0.5;
         setSpeedVector(angle,power);
         self = this;
         addEventListener(§_-L3Q§.§_-o13§,function(param1:Event):void
         {
            --counter;
            if(counter != 0)
            {
               self.inflictDamage(param1.data.other,paramSet.damage.value,paramSet.damageRadius.value,paramSet.explosionRadius.value);
               Effects.explodeParticles(x,y,3);
               Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_LARGE,self.x,self.y,2 * paramSet.damageRadius.value,§_-d27§.CASSETE_EXPLOSION1);
            }
            else
            {
               disposed = true;
            }
         });
         Effects.attachParticleTrail(ParticleEffectType.AMMO_TRAIL,this);
         §_-32§ = §_-uN§;
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
   }
}

