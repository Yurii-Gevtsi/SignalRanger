package ru.pragmatix.wormix.weapons.ammo
{
   import §_-L1H§.§_-C3e§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-F2L§;
   
   public class GuardingBot extends Mine
   {
      
      private static const ammoParamSet:§_-C3e§ = §_-C3e§.§_-y1O§(null,3,30,15);
      
      private static const FIRST_SHOT_FRAME:int = 10;
      
      private static const LAST_SHOT_FRAME:int = 40;
      
      protected static const SHOT_PERIOD:int = 3;
      
      protected var firstShotFrame:int;
      
      protected var lastShotFrame:int;
      
      protected var shotPeriod:int;
      
      public function GuardingBot(param1:§_-01J§, param2:int, param3:int, param4:§_-C3e§, param5:Boolean = true)
      {
         super(param1,param2,param3,param4,param5,false);
         mineDistance.value = 100;
         explosionDelay.value = 60;
         this.firstShotFrame = FIRST_SHOT_FRAME;
         this.lastShotFrame = LAST_SHOT_FRAME;
         this.shotPeriod = SHOT_PERIOD;
      }
      
      override protected function beforeExplosion() : void
      {
         super.beforeExplosion();
         addSpeedXY(0,-20);
      }
      
      override protected function precheckTarget(param1:§_-01J§) : Boolean
      {
         return param1.stable;
      }
      
      override protected function onExplosionCounter(param1:uint) : void
      {
         if(!activator.disposed && param1 > this.firstShotFrame && param1 < this.lastShotFrame && param1 % this.shotPeriod == 0)
         {
            LightningBullet.create(owner,this,ammoParamSet,Math.atan2(activator.y - y,activator.x - x),50,10,4);
         }
      }
      
      override protected function explode() : void
      {
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_ELECTRIC,this.x,this.y,2 * paramSet.explosionRadius.value,§_-d27§.§_-a2n§);
         disposed = true;
      }
      
      override protected function updateEnemiesVec() : void
      {
         if(!§_-X1j§.§_-12n§.gameMaster)
         {
            return;
         }
         enemyUnitsVec = §_-X1j§.§_-12n§.gameMaster.§_-D2A§(owner);
      }
      
      override protected function findTarget() : §_-01J§
      {
         var unit:§_-01J§ = null;
         var posX:int = 0;
         var posY:int = 0;
         var sqDist:int = 0;
         unit = null;
         posX = this.x;
         posY = this.y;
         sqDist = mineDistance.value * mineDistance.value;
         §_-F2L§.§_-x1v§(enemyUnitsVec,"disposed",false,function(param1:§_-01J§):Boolean
         {
            if(param1.stable && MathUtil.§_-Ih§(param1.x,param1.y,posX,posY) < sqDist)
            {
               unit = param1;
               activator = unit;
               return true;
            }
            return false;
         });
         return unit;
      }
   }
}

