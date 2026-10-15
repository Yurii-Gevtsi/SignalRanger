package ru.pragmatix.wormix.physics.model
{
   import §_-31N§.§_-42G§;
   import §_-sQ§.§_-H3D§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   
   public class GrenadePhysicModel extends AmmoPhysicModel
   {
      
      private const HIT_NONE:uint = 0;
      
      private const HIT_LEFT:uint = 1;
      
      private const HIT_TOP:uint = 2;
      
      private const HIT_RIGHT:uint = 4;
      
      private const HIT_BOTTOM:uint = 8;
      
      private var hitSide:uint = 0;
      
      private var bounceCoeff:Number;
      
      private var lifeTime:int;
      
      private var initTime:int = §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-O1C§();
      
      private var halfWidth:uint = 2;
      
      private var halfHeight:uint = 2;
      
      public function GrenadePhysicModel(param1:PhysParams, param2:Number, param3:int)
      {
         super(param1);
         this.bounceCoeff = param2;
         this.lifeTime = param3;
      }
      
      override public function hitTestGround(param1:int, param2:int) : Boolean
      {
         var _loc3_:int = 0;
         this.hitSide = this.HIT_NONE;
         if(physParams.collidesWithGround)
         {
            _loc3_ = y - this.halfHeight;
            while(_loc3_ <= y + this.halfHeight)
            {
               if(§_-X1j§.§_-12n§.scene.hitTestPoint(x - this.halfWidth,_loc3_))
               {
                  this.hitSide |= this.HIT_LEFT;
               }
               if(§_-X1j§.§_-12n§.scene.hitTestPoint(x + this.halfWidth,_loc3_))
               {
                  this.hitSide |= this.HIT_RIGHT;
               }
               _loc3_++;
            }
            _loc3_ = x - this.halfWidth;
            while(_loc3_ <= x + this.halfWidth)
            {
               if(§_-X1j§.§_-12n§.scene.hitTestPoint(_loc3_,y - this.halfHeight))
               {
                  this.hitSide |= this.HIT_TOP;
               }
               if(§_-X1j§.§_-12n§.scene.hitTestPoint(_loc3_,y + this.halfHeight))
               {
                  this.hitSide |= this.HIT_BOTTOM;
               }
               _loc3_++;
            }
         }
         return this.hitSide != this.HIT_NONE;
      }
      
      override public function processCollision(param1:§_-F3o§, param2:int, param3:int, param4:Object) : void
      {
         var _loc5_:Number = NaN;
         var _loc6_:Number = NaN;
         var _loc7_:Boolean = false;
         if(param1 == null)
         {
            physParams.epsilonSpeedCheck();
            if(this.stabilityCheck())
            {
               x = param2;
               y = param3;
               return;
            }
            _loc5_ = 0;
            _loc6_ = 0;
            _loc7_ = this.hitSide != this.HIT_NONE;
            if(_loc7_)
            {
               if(this.hitSide & this.HIT_TOP)
               {
                  _loc6_++;
               }
               if(this.hitSide & this.HIT_BOTTOM)
               {
                  _loc6_--;
               }
               if(this.hitSide & this.HIT_LEFT)
               {
                  _loc5_++;
               }
               if(this.hitSide & this.HIT_RIGHT)
               {
                  _loc5_--;
               }
               if(_loc5_ != 0)
               {
                  physParams.dx = Math.abs(physParams.dx) * §_-H3D§.sign(_loc5_);
               }
               physParams.dx *= this.bounceCoeff;
               if(_loc6_ != 0)
               {
                  physParams.dy = Math.abs(physParams.dy) * §_-H3D§.sign(_loc6_);
               }
               physParams.dy *= this.bounceCoeff;
               x = param2;
               y = param3;
               stop();
            }
         }
         dispatchEventWith(§_-42G§.§_-XQ§);
      }
      
      protected function stabilityCheck() : Boolean
      {
         if(this.lifeTimeExpired() || Boolean(Math.abs(speedX) < 0.2 && speed < 3) && Boolean(this.hitSide & this.HIT_BOTTOM))
         {
            this.stabilize();
            return true;
         }
         return false;
      }
      
      protected function stabilize() : void
      {
         if(keepUnstable)
         {
            setSpeedXY(0,0);
            stop();
            dispatchEventWith(§_-42G§.§_-X1C§);
         }
         else
         {
            stable = true;
            dispatchEventWith(§_-42G§.§_-K2J§);
         }
      }
      
      protected function lifeTimeExpired() : Boolean
      {
         if(this.lifeTime == 0)
         {
            return false;
         }
         return §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-O1C§() - this.initTime > this.lifeTime;
      }
   }
}

