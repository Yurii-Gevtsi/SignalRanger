package ru.pragmatix.wormix.weapons.ammo.phase
{
   import §_-w2i§.§_-Ia§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.model.AmmoPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import starling.utils.Pool;
   
   public class PhaseToPointAmmo extends §_-Qi§
   {
      
      protected var ammoSpeed:uint;
      
      protected var ammoSpeed2:uint;
      
      protected var callback:Function;
      
      protected var targetPoint:Point;
      
      protected var squaredDistToTarget:Number;
      
      private var color:uint;
      
      private var alpha:Number;
      
      protected var isCleared:Boolean;
      
      private var autoClearTargetPoint:Boolean = true;
      
      public function PhaseToPointAmmo(param1:§_-01J§, param2:int, param3:int, param4:Boolean, param5:Point, param6:uint, param7:uint, param8:Number = 0.8)
      {
         super(param1,param2,param3,param4,10000);
         this.color = param6;
         this.alpha = param8;
         §_-92S§ = false;
         this.targetPoint = param5;
         this.ammoSpeed = param7;
         this.ammoSpeed2 = param7 * param7;
         var _loc9_:Point = Pool.getPoint(param2,param3);
         initialize(_loc9_);
         Pool.putPoint(_loc9_);
         this.initTrail();
         §_-ni§ = false;
         §_-32§ = §_-uN§;
         physParams.collidesWith = PhysParams.COLLIDES_WITH_UNIT;
         physParams.ignoresTerrain = true;
         this.isCleared = false;
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      override public function setSpeedVector(param1:Number, param2:Number) : void
      {
         super.setSpeedVector(param1,param2);
      }
      
      override protected function initPhysicModel() : void
      {
         physParams = new PhysParams(CollideType.NONE);
         physParams.collidesWith = PhysParams.COLLIDES_WITH_UNIT;
         §_-Y2I§ = new AmmoPhysicModel(physParams);
      }
      
      override public function nextState() : void
      {
         super.nextState();
         this.processMovement();
      }
      
      protected function initTrail() : void
      {
         Effects.attachParticleTrailWithColor(ParticleEffectType.REVENGE,this,this.color,this.color,this.alpha);
      }
      
      protected function processMovement() : void
      {
         var _loc1_:int = 0;
         var _loc2_:int = 0;
         var _loc3_:Number = NaN;
         if(!disposed)
         {
            _loc1_ = this.targetPoint.x - x;
            _loc2_ = this.targetPoint.y - y;
            this.squaredDistToTarget = _loc1_ * _loc1_ + _loc2_ * _loc2_;
            if(this.squaredDistToTarget > this.ammoSpeed2)
            {
               _loc3_ = Math.atan2(_loc2_,_loc1_);
               this.setSpeedVector(_loc3_,this.ammoSpeed);
            }
            else if(this.squaredDistToTarget != 0)
            {
               x = this.targetPoint.x;
               y = this.targetPoint.y;
               setSpeedXY(0,0);
            }
            else
            {
               this.clear();
            }
         }
      }
      
      protected function clear() : void
      {
         if(this.isCleared)
         {
            return;
         }
         this.isCleared = true;
         §_-Ia§.§_-33o§(2,this.remove);
      }
      
      private function remove() : void
      {
         if(physParams)
         {
            disposed = true;
            if(this.autoClearTargetPoint)
            {
               this.clearTargetPoint();
            }
         }
      }
      
      public function getTargetPoint() : Point
      {
         return this.targetPoint;
      }
      
      public function clearTargetPoint() : void
      {
         Pool.putPoint(this.targetPoint);
         this.targetPoint = null;
      }
      
      public function disableClearTargetPoint() : void
      {
         this.autoClearTargetPoint = false;
      }
   }
}

