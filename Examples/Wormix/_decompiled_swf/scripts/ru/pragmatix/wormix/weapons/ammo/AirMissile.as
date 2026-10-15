package ru.pragmatix.wormix.weapons.ammo
{
   import §_-L1H§.§_-C3e§;
   import §_-L1H§.§_-b2K§;
   import §_-sQ§.§_-H3D§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.abstracts.AbstractAirMissile;
   
   public class AirMissile extends AbstractAirMissile
   {
      
      private static const ROTATION_SPEED:Number = 2 * MathUtil.PI_ONE_360;
      
      private var shotParams:§_-b2K§;
      
      public function AirMissile(param1:§_-01J§, param2:§_-C3e§, param3:int, param4:int, param5:int, param6:int, param7:§_-b2K§)
      {
         super(param1,param2,param3,param4,param5,param6);
         this.shotParams = param7;
         Effects.attachParticleTrail(ParticleEffectType.AMMO_TRAIL,this);
      }
      
      override protected function explode(param1:§_-F3o§) : void
      {
         super.explode(param1);
         Effects.explodeParticles(x,y,16);
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,x,y,paramSet.explosionRadius.value * 2,§_-d27§.§_-33W§);
      }
      
      override public function nextState() : void
      {
         var _loc1_:Number = NaN;
         var _loc2_:Number = NaN;
         super.nextState();
         if(this.shotParams)
         {
            _loc1_ = Math.atan2(this.shotParams.point.y - y,this.shotParams.point.x - x);
            if(_loc1_ > 0)
            {
               _loc2_ = §_-H3D§.§_-r2H§(angle,_loc1_,ROTATION_SPEED);
               setSpeedVector(_loc2_,speed);
            }
         }
      }
   }
}

