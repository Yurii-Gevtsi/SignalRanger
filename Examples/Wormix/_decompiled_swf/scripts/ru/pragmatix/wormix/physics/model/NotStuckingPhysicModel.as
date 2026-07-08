package ru.pragmatix.wormix.physics.model
{
   import flash.geom.Point;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import starling.utils.Pool;
   
   public class NotStuckingPhysicModel extends GenericPhysicModel
   {
      
      private static const MAX_TRIES:int = 10;
      
      private var tries:int = 0;
      
      private var _radius:int;
      
      private var _moveLimit:int = 100;
      
      public function NotStuckingPhysicModel(param1:PhysParams, param2:int = 10, param3:int = 10)
      {
         super(param1);
         this._radius = param2;
         this._moveLimit = param3;
      }
      
      override public function processCollision(param1:§_-F3o§, param2:int, param3:int, param4:Object) : void
      {
         super.processCollision(param1,param2,param3,param4);
         if(param1 == null)
         {
            this.collideWithGround(param2,param3);
         }
      }
      
      private function collideWithGround(param1:int, param2:int) : void
      {
         var _loc3_:Point = null;
         if(x == param1 && y == param2)
         {
            ++this.tries;
            if(this.tries >= MAX_TRIES)
            {
               _loc3_ = Pool.getPoint();
               if(Physics.nearestFreePoint(param1,param2,HitTest.unit,this._moveLimit,_loc3_))
               {
                  x = _loc3_.x;
                  y = _loc3_.y;
               }
               Pool.putPoint(_loc3_);
               stable = true;
            }
         }
         else
         {
            this.tries = 0;
         }
      }
      
      public function get radius() : int
      {
         return this._radius;
      }
      
      public function set radius(param1:int) : void
      {
         this._radius = param1;
      }
      
      public function get moveLimit() : int
      {
         return this._moveLimit;
      }
      
      public function set moveLimit(param1:int) : void
      {
         this._moveLimit = param1;
      }
   }
}

