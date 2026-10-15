package ru.pragmatix.wormix.weapons.ammo
{
   import §_-L1H§.§_-C3e§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.abstracts.AbstractAirMissile;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import starling.utils.Pool;
   
   public class AirFireMissile extends AbstractAirMissile
   {
      
      private static const FIRE_DURATION:uint = 150;
      
      public function AirFireMissile(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:Number, param5:int, param6:int)
      {
         super(param1,param2,param3,param4,param5,param6);
         Effects.attachParticleTrail(ParticleEffectType.AMMO_TRAIL,this);
      }
      
      override protected function explode(param1:§_-F3o§) : void
      {
         var _loc2_:BurningOil = null;
         if(disposed)
         {
            return;
         }
         inflictDamage(param1,paramSet.damage.value,paramSet.damageRadius.value,paramSet.explosionRadius.value);
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_DRILL,x,y,5 * paramSet.explosionRadius.value,§_-d27§.§_-J1x§);
         var _loc3_:Point = Pool.getPoint();
         var _loc4_:int = -2;
         while(_loc4_ <= 2)
         {
            _loc3_.setTo(x + 7 * _loc4_,y + §_-X1j§.randomSeed.§_-Fh§(-3,3));
            _loc2_ = AmmosFactory.getInstance().createObject(AmmosFactory.OBJECT_BURNING_OIL) as BurningOil;
            _loc2_.initAmmo(AmmosFactory.BURNING_OIL_SKIN,owner,_loc3_,null,-MathUtil.PI_HALF,3);
            _loc2_.setDuration(FIRE_DURATION);
            _loc4_++;
         }
         Pool.putPoint(_loc3_);
         disposed = true;
      }
   }
}

