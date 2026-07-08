package ru.pragmatix.wormix.weapons.ammo.type.missile.bazooka
{
   import §_-L1H§.§_-C3e§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.Force;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class BazookaMissile extends §_-Qi§
   {
      
      protected var paramSet:§_-C3e§;
      
      public function BazookaMissile(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:uint)
      {
         super(param1,param1.x,param1.y,false);
         this.paramSet = param2;
         §_-U1o§ = true;
         §_-A3d§ = true;
         §_-h2B§.play(§_-d27§.§_-I1P§);
         var _loc5_:Point = §_-fm§(param3,0);
         initialize(_loc5_,param2.ammoClassName);
         Pool.putPoint(_loc5_);
         this.§_-z1H§.rotation = param3;
         physParams.addForce(Force.WIND);
         setSpeedVector(param3,param4);
         addEventListener(§_-L3Q§.§_-o13§,this.onCollisionEvent);
         §_-32§ = §_-uN§;
         Effects.attachParticleTrail(ParticleEffectType.AMMO_TRAIL,this);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      protected function onCollisionEvent(param1:Event) : void
      {
         this.explode(param1.data.other);
         disposed = true;
      }
      
      protected function explode(param1:§_-F3o§) : void
      {
         Effects.explodeParticles(x,y,16);
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,x,y,3 * this.paramSet.damageRadius.value,§_-d27§.§_-33W§);
         inflictDamage(param1,this.paramSet.damage.value,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value);
      }
   }
}

