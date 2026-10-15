package ru.pragmatix.wormix.weapons.ammo.feats
{
   import §_-sQ§.§_-H3D§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   
   public class HomingFeat implements IFrameFeat
   {
      
      private var ammo:§_-F3o§;
      
      private var frames:int = 0;
      
      private var isHoming:Boolean = true;
      
      private var isSuperHoming:Boolean = false;
      
      private var target:Object;
      
      private var targetSource:ITargetSource;
      
      public var ammoSpeed:Number;
      
      public var delayBeforeAim:uint = 0;
      
      public var rotaSpeed:Number;
      
      public var rotaSpeedGain:Number;
      
      public var rotaSpeedGainPeriod:uint = 0;
      
      public var superHomingDelay:int = -1;
      
      public var superHomingDistance:int = -1;
      
      public var lockOnTargetCapture:Boolean = false;
      
      public function HomingFeat(param1:§_-F3o§, param2:Number, param3:Number)
      {
         super();
         this.ammo = param1;
         this.ammoSpeed = param2;
         this.rotaSpeed = param3;
      }
      
      public function nextState(param1:uint) : void
      {
         var _loc2_:Number = NaN;
         var _loc3_:Number = NaN;
         if(this.ammo.disposed)
         {
            return;
         }
         if(this.isTargetValid())
         {
            ++this.frames;
            if(this.frames >= this.delayBeforeAim)
            {
               this.checkSuperHoming();
               if(this.isSuperHoming)
               {
                  this.ammo.setSpeedVector(Math.atan2(this.target.y - this.ammo.y,this.target.x - this.ammo.x),this.ammoSpeed);
               }
               else if(this.isHoming)
               {
                  _loc2_ = Math.atan2(this.target.y - this.ammo.y,this.target.x - this.ammo.x);
                  _loc3_ = §_-H3D§.§_-r2H§(this.ammo.angle,_loc2_,this.rotaSpeed);
                  this.ammo.setSpeedVector(_loc3_,this.ammoSpeed);
                  if(this.lockOnTargetCapture && Math.abs(_loc3_ - _loc2_) < Physics.EPSILON)
                  {
                     this.isHoming = false;
                  }
               }
               if(this.rotaSpeedGainPeriod > 0 && !isNaN(this.rotaSpeedGain) && this.frames % this.rotaSpeedGainPeriod == 0)
               {
                  this.rotaSpeed += this.rotaSpeedGain;
               }
            }
         }
         else if(this.targetSource != null)
         {
            this.target = this.targetSource.getTarget();
         }
      }
      
      public function isTargetValid() : Boolean
      {
         if(this.target)
         {
            if(this.target is §_-F3o§ && !this.target.disposed)
            {
               return true;
            }
            if(this.target is Point)
            {
               return true;
            }
         }
         return false;
      }
      
      private function checkSuperHoming() : void
      {
         if(!this.isSuperHoming)
         {
            if(this.superHomingDistance > 0 && §_-H3D§.distance(this.target.x - this.ammo.x,this.target.y - this.ammo.y) < this.superHomingDistance || this.superHomingDelay > 0 && this.frames > this.superHomingDelay + this.delayBeforeAim)
            {
               this.isSuperHoming = true;
            }
         }
      }
      
      public function setTarget(param1:Object) : void
      {
         this.target = param1;
      }
      
      public function setTargetSource(param1:ITargetSource) : void
      {
         this.targetSource = param1;
      }
      
      public function withTarget(param1:Object) : HomingFeat
      {
         this.setTarget(param1);
         return this;
      }
   }
}

