package ru.pragmatix.wormix.weapons.ammo.type.watermelon
{
   import §_-L1H§.§_-C3e§;
   import §_-gG§.§_-8u§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Physics;
   import starling.events.Event;
   
   public class FoulWatermelonPiece extends WatermelonPiece
   {
      
      public function FoulWatermelonPiece(param1:§_-01J§, param2:int, param3:int, param4:Number, param5:uint, param6:§_-C3e§)
      {
         super(param1,param2,param3,param4,param5,param6);
      }
      
      override protected function initialize(param1:Point, param2:String = null, param3:Boolean = false, param4:String = "resurces/_ammo/") : void
      {
         super.initialize(param1,param2,param3,param4);
         Effects.attachParticleTrail(ParticleEffectType.AMMO_POISON_TRAIL,this);
      }
      
      override protected function onCollide(param1:Event) : void
      {
         var _loc2_:§_-01J§ = null;
         inflictDamage(param1.data.other,paramSet.damage.value,paramSet.damageRadius.value,paramSet.explosionRadius.value);
         disposed = true;
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_POISON_LARGE,x,y,paramSet.damageRadius.value * 4,§_-d27§.§_-J1x§);
         for each(_loc2_ in Physics.getUnitsInCircle(x,y,paramSet.damageRadius.value))
         {
            if(!_loc2_.disposed && !_loc2_.buffs.hasBuff(§_-8u§.NAME))
            {
               _loc2_.buffs.place(new §_-8u§(owner));
            }
         }
      }
   }
}

