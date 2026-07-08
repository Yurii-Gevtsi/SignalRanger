package ru.pragmatix.wormix.weapons.ammo
{
   import §_-L1H§.§_-C3e§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.ScreenShake;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class Meteor extends §_-Qi§
   {
      
      public static const DEFAULT_SHARDS:uint = 4;
      
      private var paramSet:§_-C3e§;
      
      private var shards:uint;
      
      public function Meteor(param1:§_-01J§, param2:§_-C3e§, param3:int, param4:int, param5:int, param6:int, param7:uint = 4)
      {
         super(param1,param3,param4,false,§_-Qi§.§_-n18§,DamageType.AIR,false);
         this.shards = param7;
         initialize(Pool.getPoint(param3,param4),param2.ammoClassName);
         setSpeedXY(param5,param6);
         this.paramSet = param2;
         §_-32§ = §_-uN§;
         Effects.attachParticleTrail(ParticleEffectType.AMMO_TRAIL,this);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      override protected function defaultOnCollide(param1:Event) : void
      {
         var _loc2_:int = 0;
         var _loc3_:Number = NaN;
         var _loc4_:int = 0;
         if(!disposed)
         {
            super.defaultOnCollide(param1);
            inflictDamage(param1.data.other,this.paramSet.damage.value,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value);
            _loc2_ = 0;
            while(_loc2_ < this.shards)
            {
               _loc3_ = §_-X1j§.randomSeed.§_-y2j§() * MathUtil.§_-4S§ - MathUtil.PI_ONE_16 - MathUtil.PI_HALF;
               _loc4_ = §_-X1j§.randomSeed.§_-Fh§(12,20);
               new MeteorShard(owner,x,y,_loc3_,_loc4_);
               _loc2_++;
            }
            disposed = true;
            Effects.explodeParticles(x,y,20);
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_LARGE,x,y,3 * this.paramSet.explosionRadius.value,§_-d27§.§_-33W§);
            ScreenShake.go(this.paramSet.damage.value,true);
            this.paramSet = null;
         }
      }
   }
}

