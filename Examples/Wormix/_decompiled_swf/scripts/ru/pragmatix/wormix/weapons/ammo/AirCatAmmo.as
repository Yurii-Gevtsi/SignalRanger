package ru.pragmatix.wormix.weapons.ammo
{
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-h2B§;
   import §_-sQ§.§_-V1n§;
   import §_-w2i§.§_-Zd§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.feats.HomingFeat;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class AirCatAmmo extends §_-Qi§
   {
      
      private static const DEFAULT_MAX_BOUNCE:int = 3;
      
      private static const DAMAGE_FOR_BOUNCE:int = 0;
      
      private static const TARGETING_SPEED:Number = 20;
      
      private var hits:int = 0;
      
      private var paramSet:§_-C3e§;
      
      private var damageForBounce:§_-TA§;
      
      private var maxBounce:§_-TA§;
      
      private var bonusDamage:§_-TA§ = new §_-TA§(0);
      
      private var autoTarget:Boolean = false;
      
      private var firstUnitHit:§_-01J§;
      
      private var homingFeat:HomingFeat;
      
      public function AirCatAmmo(param1:§_-01J§, param2:§_-C3e§, param3:int, param4:int, param5:int, param6:int, param7:DamageType, param8:int = 3, param9:int = 0)
      {
         super(param1,param3,param4,false,§_-Qi§.§_-n18§,param7,false);
         this.paramSet = param2;
         §_-h2B§.play(§_-d27§.§_-I1P§);
         initialize(Pool.getPoint(param3,param4),param2.ammoClassName);
         setSpeedXY(param5,param6);
         §_-32§ = §_-h2D§;
         this.damageForBounce = new §_-TA§(param9);
         this.maxBounce = new §_-TA§(param8);
         this.homingFeat = new HomingFeat(this,TARGETING_SPEED,MathUtil.PI_ONE_12);
         §_-P1P§.push(this.homingFeat);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      override protected function initPhysicModel() : void
      {
         super.initPhysicModel();
         physParams.collidesWith = PhysParams.COLLIDES_WITH_GROUND + PhysParams.COLLIDES_WITH_UNIT;
      }
      
      override protected function defaultOnCollide(param1:Event) : void
      {
         var _loc2_:§_-V1n§ = null;
         var _loc3_:Number = NaN;
         var _loc4_:Number = NaN;
         super.defaultOnCollide(param1);
         if(!disposed)
         {
            Effects.explodeParticles(x,y,16);
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,x,y,this.paramSet.damageRadius.value / §_-K1t§.§_-31c§,§_-d27§.§_-S14§);
            inflictDamage(param1.data.other,this.paramSet.damage.value + this.bonusDamage.value,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value);
            if(++this.hits == this.maxBounce.value)
            {
               disposed = true;
            }
            else
            {
               if(§_-X1j§.§_-12n§.scene.hitTestPermanent(x,y))
               {
                  setSpeedXY(-speedX * 0.8,-speedY * 0.8);
               }
               else
               {
                  _loc2_ = §_-X1j§.randomSeed;
                  _loc3_ = -MathUtil.PI_HALF + _loc2_.§_-Fh§(-8,8) * MathUtil.PI_ONE_64;
                  _loc4_ = this.firstUnitHit ? TARGETING_SPEED : 1 * _loc2_.§_-Fh§(5,15 + 5 * (this.maxBounce.value - this.hits));
                  setSpeedVector(_loc3_,_loc4_);
               }
               x = param1.data.x + speedX;
               y = param1.data.y + speedY;
               this.bonusDamage.value += this.damageForBounce.value;
            }
            if(this.autoTarget)
            {
               this.processAutotargeting(param1.data.other);
            }
         }
      }
      
      private function processAutotargeting(param1:§_-F3o§) : void
      {
         var unit:§_-01J§ = null;
         var directTarget:§_-F3o§ = param1;
         if(this.firstUnitHit == null)
         {
            if(directTarget is §_-01J§)
            {
               this.firstUnitHit = §_-01J§(directTarget);
            }
            else
            {
               var _loc3_:int = 0;
               var _loc4_:* = Physics.getUnitsInCircle(x,y,this.paramSet.damageRadius.value);
               for each(unit in _loc4_)
               {
                  this.firstUnitHit = unit;
               }
            }
         }
         if(this.firstUnitHit)
         {
            this.homingFeat.setTarget(null);
            §_-Zd§.§_-33o§(8,function():void
            {
               if(!homingFeat.isTargetValid())
               {
                  homingFeat.setTarget(firstUnitHit);
               }
            });
         }
      }
      
      public function setAutoTargeting() : void
      {
         this.autoTarget = true;
      }
   }
}

