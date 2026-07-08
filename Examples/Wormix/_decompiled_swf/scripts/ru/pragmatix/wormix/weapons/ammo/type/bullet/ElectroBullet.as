package ru.pragmatix.wormix.weapons.ammo.type.bullet
{
   import §_-A2F§.§_-4B§;
   import §_-L1H§.§_-C3e§;
   import §_-b1F§.§_-G4§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Zd§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class ElectroBullet extends §_-Qi§
   {
      
      private static const BOUNCES:uint = 3;
      
      private static const BOUNCE_POWER:uint = 15;
      
      private static const BOUNCE_DAMAGE:uint = 30;
      
      private var paramSet:§_-C3e§;
      
      public function ElectroBullet(param1:§_-01J§, param2:Point, param3:§_-C3e§, param4:Number, param5:uint)
      {
         super(param1,param1.x,param1.y,false,§_-n18§,DamageType.SHOCK);
         this.paramSet = param3;
         §_-U1o§ = true;
         §_-h2B§.play(§_-d27§.§_-Z1d§);
         §_-In§ = DamageType.SHOCK;
         initialize(param2,param3.ammoClassName);
         this.clip.rotation = param4 * 180 / Math.PI;
         setSpeedVector(param4,param5);
         §_-Zd§.once(this,§_-L3Q§.§_-o13§,this.onCollide);
         §_-32§ = §_-uN§;
         Effects.attachParticleTrail(ParticleEffectType.AMMO_TRAIL,this);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      private function onCollide(param1:Event) : void
      {
         Effects.explodeParticles(x,y,8);
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_ELECTRIC,x,y,this.paramSet.damageRadius.value,§_-d27§.§_-Z1d§);
         if(param1.data.other is §_-01J§)
         {
            §_-4B§.§_-L2K§(§_-01J§(param1.data.other),this.bounce,BOUNCES);
            param1.data.other.addSpeedXY(0,-BOUNCE_POWER);
         }
         inflictDamage(param1.data.other,this.paramSet.damage.value,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value,true);
         disposed = true;
      }
      
      private function bounce(param1:§_-01J§, param2:Event = null) : void
      {
         var unit:§_-01J§ = param1;
         var e:Event = param2;
         var bounceAngle:Number = -MathUtil.PI_HALF + Math.PI / 32 * §_-X1j§.randomSeed.§_-Fh§(-8,8);
         var ground:Point = HitTest.castRayVec(unit.x,unit.y,MathUtil.PI_HALF,Physics.UNIT_HALF_HEIGHT);
         if(Boolean(ground) && !HitTest.point(ground.x,ground.y,true))
         {
            §_-G4§.§_-Ee§(owner,ground.x,ground.y,null,0,Physics.UNIT_HALF_HEIGHT + 10,Physics.UNIT_HALF_HEIGHT,false);
            unit.x = ground.x;
            unit.y = ground.y;
         }
         if(ground)
         {
            Pool.putPoint(ground);
         }
         unit.graphics.§_-310§();
         §_-Zd§.§_-33o§(20,function():void
         {
            if(Boolean(unit) && !unit.disposed)
            {
               unit.graphics.§_-41n§();
            }
         });
         §_-G4§.§_-Ee§(owner,unit.x,unit.y,unit as §_-F3o§,BOUNCE_DAMAGE,0,0,false,true,DamageType.SHOCK);
         §_-G4§.§_-H1E§(unit as §_-F3o§,bounceAngle,BOUNCE_POWER,false);
      }
   }
}

