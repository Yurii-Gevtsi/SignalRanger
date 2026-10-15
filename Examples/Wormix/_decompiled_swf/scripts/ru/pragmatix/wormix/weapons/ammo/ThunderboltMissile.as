package ru.pragmatix.wormix.weapons.ammo
{
   import §_-L1H§.§_-C3e§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Ia§;
   import §_-w2i§.§_-Zd§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class ThunderboltMissile extends §_-Qi§
   {
      
      private const AOE_RADIUS:uint = 400;
      
      private const LIGHTNING_SPEED:uint = 50;
      
      private const LIGHTNING_DURATION:uint = 15;
      
      private var paramSet:§_-C3e§;
      
      private var secParamSet:§_-C3e§;
      
      public function ThunderboltMissile(param1:§_-01J§, param2:§_-C3e§, param3:§_-C3e§, param4:Number, param5:uint)
      {
         super(param1,param1.x,param1.y,false,§_-n18§,DamageType.SHOCK);
         this.paramSet = param2;
         this.secParamSet = param3;
         §_-U1o§ = true;
         §_-h2B§.play(§_-d27§.§_-I1P§);
         §_-In§ = DamageType.SHOCK;
         var _loc6_:Point = §_-fm§(param4,5);
         initialize(_loc6_,param2.ammoClassName);
         Pool.putPoint(_loc6_);
         this.§_-z1H§.rotation = param4;
         setSpeedVector(param4,param5);
         physParams.addForce(Force.WIND);
         §_-Zd§.once(this,§_-L3Q§.§_-o13§,this.onCollide);
         §_-32§ = §_-uN§;
         Effects.attachParticleTrail(ParticleEffectType.AMMO_TRAIL,this);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      private function onCollide(param1:Event) : void
      {
         var unit:§_-01J§ = null;
         var e:Event = param1;
         Effects.explodeParticles(x,y,16);
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_ELECTRIC,x,y,this.paramSet.damageRadius.value,§_-d27§.§_-33W§);
         if(e.data.other == null)
         {
            inflictDamage(null,this.paramSet.damage.value - this.secParamSet.damage.value,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value);
         }
         else
         {
            inflictDamage(e.data.other,this.paramSet.damage.value,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value);
         }
         §_-Ia§.§_-33o§(40,function():void
         {
            if(!disposed)
            {
               disposed = true;
            }
         });
         setSpeedXY(0,0);
         physParams.removeAllForces();
         for each(unit in Physics.getUnitsInCircle(x,y,this.AOE_RADIUS))
         {
            if(unit != e.data.other)
            {
               LightningBullet.create(owner,this,this.secParamSet,Math.atan2(unit.y - y,unit.x - x),this.LIGHTNING_SPEED,Math.ceil(this.AOE_RADIUS / this.LIGHTNING_SPEED),this.LIGHTNING_DURATION,true);
            }
         }
      }
   }
}

