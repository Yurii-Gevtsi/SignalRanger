package ru.pragmatix.wormix.weapons.ammo
{
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-sQ§.§_-V1n§;
   import §_-w2i§.§_-Zd§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.ScreenShake;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class GroundBreakerMissile extends §_-Qi§
   {
      
      private static const SHARDS_COUNT:§_-TA§ = new §_-TA§(10);
      
      public function GroundBreakerMissile(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:uint)
      {
         var base:Point;
         var self:§_-Qi§ = null;
         var owner:§_-01J§ = param1;
         var paramSet:§_-C3e§ = param2;
         var angle:Number = param3;
         var power:uint = param4;
         super(owner,owner.x,owner.y,false);
         §_-U1o§ = true;
         §_-A3d§ = true;
         §_-h2B§.play(§_-d27§.§_-I1P§);
         base = §_-fm§(angle,0);
         initialize(base,paramSet.ammoClassName);
         Pool.putPoint(base);
         this.§_-z1H§.rotation = angle;
         physParams.addForce(Force.WIND);
         setSpeedVector(angle,power);
         self = this;
         §_-Zd§.once(this,§_-L3Q§.§_-o13§,function(param1:Event):void
         {
            var _loc4_:Number = NaN;
            var _loc5_:int = 0;
            var _loc6_:int = 0;
            var _loc2_:Number = self.angle;
            var _loc3_:§_-V1n§ = §_-X1j§.randomSeed;
            if(SHARDS_COUNT.value > 10)
            {
               §_-X1j§.§_-12n§.scene.dispatchEvent(new §_-H1K§(§_-A2V§.§_-A2e§,"ground breaker missile hack"));
            }
            var _loc7_:int = 0;
            while(_loc7_ < SHARDS_COUNT.value)
            {
               _loc4_ = Math.PI + §_-X1j§.randomSeed.§_-H3r§(_loc2_ - MathUtil.PI_QUARTER,_loc2_ + MathUtil.PI_QUARTER);
               _loc5_ = self.x + Math.cos(_loc4_) * 10;
               _loc6_ = self.y + Math.sin(_loc4_) * 10;
               new BouncingShard(owner,_loc5_,_loc6_,-MathUtil.PI_HALF + _loc4_ * (_loc7_ % 2 == 0 ? 1 : -1),_loc3_.§_-Fh§(9,20),40 + _loc7_ * 6);
               _loc7_++;
            }
            self.inflictDamage(param1.data.other,paramSet.damage.value,paramSet.damageRadius.value,paramSet.explosionRadius.value);
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,self.x,self.y,3 * paramSet.damageRadius.value,§_-d27§.§_-33W§);
            ScreenShake.go(paramSet.damage.value);
            self.disposed = true;
         });
         §_-32§ = §_-t3§;
         Effects.attachParticleTrail(ParticleEffectType.AMMO_TRAIL,this);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
   }
}

