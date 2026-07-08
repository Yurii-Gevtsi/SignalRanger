package ru.pragmatix.wormix.physics.model
{
   import §_-31N§.§_-42G§;
   import §_-31N§.§_-Yy§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-l1g§.§_-L3Q§;
   import §_-sQ§.§_-H3D§;
   import §_-ws§.§_-B3I§;
   import ru.pragmatix.wormix.§_-w1N§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.events.EventDispatcher;
   
   public class GenericPhysicModel extends EventDispatcher implements IPhysicModel
   {
      
      public static const WATER_LEVEL_OFFSET:int = 0;
      
      private static var utilArray:Array = [];
      
      public var physParams:PhysParams;
      
      private var _keepUnstable:Boolean;
      
      public var _lastTravelledDistance:uint = 0;
      
      public function GenericPhysicModel(param1:PhysParams)
      {
         super();
         this.physParams = param1;
      }
      
      public function checkBounds() : void
      {
         var _loc1_:§_-w1N§ = Application.scene;
         if(this.y > this.getSinkLevel())
         {
            dispatchEventWith(§_-L3Q§.§_-kl§,false,new §_-B3I§(_loc1_.processingWaterIncrease,Application.scene.getWaterHeight() == 0));
         }
         else if(this.y < -1.2 * _loc1_.sceneH || this.x < -1 * _loc1_.sceneW || this.x > _loc1_.sceneW * 2)
         {
            dispatchEventWith(§_-L3Q§.§_-kl§,false,new §_-B3I§(false,true));
         }
      }
      
      public function getSinkLevel() : int
      {
         return this.physParams.sinkable ? int(Application.scene.sceneH - (Application.scene.getWaterHeight() - WATER_LEVEL_OFFSET)) : Application.scene.sceneH;
      }
      
      public function nextState() : void
      {
         var _loc1_:Number = NaN;
         var _loc2_:int = 0;
         var _loc3_:Number = NaN;
         var _loc4_:Number = NaN;
         var _loc5_:int = 0;
         var _loc6_:int = 0;
         var _loc7_:§_-w1N§ = null;
         var _loc8_:uint = 0;
         var _loc9_:int = 0;
         var _loc10_:int = 0;
         var _loc11_:Object = null;
         var _loc12_:Boolean = false;
         var _loc13_:Boolean = false;
         var _loc14_:§_-F3o§ = null;
         var _loc15_:§_-Yy§ = null;
         var _loc16_:Force = null;
         if(!this.stable && !this.physParams.disposed)
         {
            this.checkBounds();
            if(this.physParams.disposed)
            {
               return;
            }
            if(this.physParams.collidesWith != PhysParams.COLLIDES_WITH_NOTHING)
            {
               _loc1_ = Math.max(Math.abs(this.physParams.dx),Math.abs(this.physParams.dy));
               _loc2_ = 4;
               _loc3_ = this.physParams.dx / _loc1_;
               _loc4_ = this.physParams.dy / _loc1_;
               _loc5_ = this.x;
               _loc6_ = this.y;
               _loc7_ = Application.scene;
               this.physParams.stopped = false;
               _loc8_ = 0;
               while(_loc8_ < int(_loc1_))
               {
                  this.physParams.px += _loc3_;
                  this.physParams.py += _loc4_;
                  if(_loc8_ == 0)
                  {
                     if(Math.abs(_loc3_ - this.physParams.dx / _loc1_) > 0.001 || Math.abs(_loc4_ - this.physParams.dy / _loc1_) > 0.001)
                     {
                        _loc7_.dispatchEvent(new §_-H1K§(§_-A2V§.§_-A3T§,"phys steps hack"));
                     }
                     if(Math.abs(this.physParams._x - this.physParams.px) > 10 || Math.abs(this.physParams._y - this.physParams.py) > 10)
                     {
                        _loc7_.dispatchEvent(new §_-H1K§(§_-A2V§.§_-A2e§,"physical coos hack"));
                     }
                     if(this.physParams.stopped)
                     {
                        _loc7_.dispatchEvent(new §_-H1K§(§_-A2V§.§_-A2e§,"stopped flag hack"));
                     }
                  }
                  _loc9_ = this.x;
                  _loc10_ = this.y;
                  this.physParams._x = Math.floor(this.physParams.px);
                  this.physParams._y = Math.floor(this.physParams.py);
                  this.checkBounds();
                  if(this.physParams.disposed || this.stable || this.physParams.stopped)
                  {
                     break;
                  }
                  if(this.hitTestGround(_loc9_,_loc10_))
                  {
                     _loc11_ = {
                        "other":null,
                        "x":_loc9_,
                        "y":_loc10_,
                        "stopObject":false
                     };
                     dispatchEventWith(§_-L3Q§.§_-o13§,false,_loc11_);
                     if(!this.physParams.stopped)
                     {
                        this.processCollision(null,_loc9_,_loc10_,_loc11_);
                     }
                     if(this.physParams.disposed || this.stable || this.physParams.stopped)
                     {
                        break;
                     }
                  }
                  else if(_loc8_ % _loc2_ == 0)
                  {
                     _loc12_ = false;
                     _loc13_ = false;
                     utilArray.length = 0;
                     this.getHitObjects(utilArray);
                     for each(_loc14_ in utilArray)
                     {
                        if(!_loc13_ && !this.physParams.disposed && !_loc14_.disposed)
                        {
                           _loc15_ = new §_-Yy§(_loc14_);
                           dispatchEvent(_loc15_);
                           if(!_loc15_.§_-K1l§)
                           {
                              _loc11_ = {
                                 "other":_loc14_,
                                 "x":this.x,
                                 "y":this.y,
                                 "stopObject":true
                              };
                              dispatchEventWith(§_-L3Q§.§_-o13§,false,_loc11_);
                              if(!_loc12_)
                              {
                                 _loc12_ = true;
                                 this.processCollision(_loc14_,_loc9_,_loc10_,_loc11_);
                              }
                           }
                           else
                           {
                              _loc13_ = true;
                           }
                        }
                     }
                  }
                  if(this.physParams.disposed)
                  {
                     break;
                  }
                  _loc8_++;
               }
            }
            else
            {
               this.physParams.px += this.physParams.dx;
               this.physParams.py += this.physParams.dy;
               this.physParams._x = Math.floor(this.physParams.px);
               this.physParams._y = Math.floor(this.physParams.py);
            }
            this._lastTravelledDistance = uint(§_-H3D§.distance(this.x - _loc5_,this.y - _loc6_));
            if(!this.stable)
            {
               for each(_loc16_ in this.physParams.forces)
               {
                  if(Math.abs(_loc16_.ax) > Physics.EPSILON)
                  {
                     this.physParams.dx += _loc16_.ax;
                  }
                  if(Math.abs(_loc16_.ay) > Physics.EPSILON)
                  {
                     this.physParams.dy += _loc16_.ay;
                  }
               }
            }
         }
      }
      
      public function hitTestGround(param1:int, param2:int) : Boolean
      {
         return this.physParams.collidesWithGround && Application.scene.hitTestPoint(this.x + this.physParams.groundShiftX,this.y + this.physParams.groundShiftY);
      }
      
      public function hitTestObject(param1:§_-F3o§) : Boolean
      {
         if(param1.disposed)
         {
            return false;
         }
         var _loc2_:int = this.x - param1.x;
         _loc2_ = _loc2_ < 0 ? int(-_loc2_) : _loc2_;
         var _loc3_:int = this.y - param1.y;
         _loc3_ = _loc3_ < 0 ? int(-_loc3_) : _loc3_;
         return _loc2_ < param1.width / 2 && _loc3_ < param1.height / 2;
      }
      
      public function getHitObjects(param1:Array) : void
      {
         var _loc2_:§_-F3o§ = null;
         if(this.physParams.collidesWithUnit)
         {
            for each(_loc2_ in Application.scene.gameObjectProcessor.cache.getUnits())
            {
               if(this != _loc2_.§_-Y2I§ && this.hitTestObject(_loc2_))
               {
                  param1.push(_loc2_);
               }
            }
         }
      }
      
      public function processCollision(param1:§_-F3o§, param2:int, param3:int, param4:Object) : void
      {
         if(param1 == null)
         {
            if(Math.abs(this.physParams.dy) < Physics.EPSILON)
            {
               this.physParams.dy = 0;
            }
            if(this.physParams.dy > 0 && !Application.scene.hitTestPoint(this.x + this.physParams.groundShiftX,this.y + this.physParams.groundShiftY - 1))
            {
               --this.y;
               this.physParams.dx = 0;
               this.physParams.dy = 0;
               if(!this.keepUnstable)
               {
                  this.stable = true;
               }
            }
            else
            {
               if(this.physParams.dy < 0)
               {
                  this.physParams.dy *= -1;
               }
               this.physParams.dx *= -0.6;
               this.x = param2;
               this.y = param3;
            }
            this.physParams.stopped = true;
         }
      }
      
      public function setSpeedVector(param1:Number, param2:Number) : void
      {
         if(param2 != 0)
         {
            this.stable = false;
         }
         param1 = MathUtil.§_-F1V§(param1);
         this.physParams.dx = Math.cos(param1) * param2;
         this.physParams.dy = Math.sin(param1) * param2;
      }
      
      public function addSpeedVector(param1:Number, param2:Number) : void
      {
         if(param2 != 0)
         {
            this.stable = false;
         }
         param1 = MathUtil.§_-F1V§(param1);
         this.physParams.dx += Math.cos(param1) * param2;
         this.physParams.dy += Math.sin(param1) * param2;
      }
      
      public function setSpeedXY(param1:Number, param2:Number) : void
      {
         if(this.stable && Math.abs(param1) > Physics.EPSILON || Math.abs(param2) > Physics.EPSILON)
         {
            this.stable = false;
         }
         this.physParams.dx = param1;
         this.physParams.dy = param2;
      }
      
      public function addSpeedXY(param1:Number, param2:Number) : void
      {
         if(this.stable && Math.abs(param1) > Physics.EPSILON || Math.abs(param2) > Physics.EPSILON)
         {
            this.stable = false;
         }
         this.physParams.dx += param1;
         this.physParams.dy += param2;
      }
      
      public function clear() : void
      {
      }
      
      public function stop() : void
      {
         this.physParams.stopped = true;
      }
      
      public function get lastTravelledDistance() : uint
      {
         return this._lastTravelledDistance;
      }
      
      public function get isStoped() : Boolean
      {
         return this.physParams.stopped;
      }
      
      public function get speed() : Number
      {
         return MathUtil.§_-F1V§(§_-H3D§.distance(this.physParams.dx,this.physParams.dy));
      }
      
      public function get speedX() : Number
      {
         return this.physParams.dx;
      }
      
      public function get speedY() : Number
      {
         return this.physParams.dy;
      }
      
      public function get angle() : Number
      {
         return MathUtil.§_-F1V§(Math.atan2(this.physParams.dy,this.physParams.dx));
      }
      
      public function get x() : int
      {
         return this.physParams._x;
      }
      
      public function set x(param1:int) : void
      {
         this.physParams._x = param1;
         this.physParams.px = param1;
      }
      
      public function get y() : int
      {
         return this.physParams._y;
      }
      
      public function set y(param1:int) : void
      {
         this.physParams._y = param1;
         this.physParams.py = param1;
      }
      
      public function get keepUnstable() : Boolean
      {
         return this._keepUnstable;
      }
      
      public function set keepUnstable(param1:Boolean) : void
      {
         this._keepUnstable = param1;
         if(this.stable)
         {
            this.stable = false;
         }
      }
      
      public function get stable() : Boolean
      {
         return this.physParams._stable.value;
      }
      
      public function set stable(param1:Boolean) : void
      {
         this.physParams._stable.value = param1;
         if(param1)
         {
            dispatchEventWith(§_-42G§.§_-K2J§);
         }
         else
         {
            dispatchEventWith(§_-42G§.§_-o1a§);
         }
      }
   }
}

