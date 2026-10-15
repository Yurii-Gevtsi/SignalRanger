package ru.pragmatix.wormix.physics
{
   import §_-31Y§.§_-D3B§;
   import §_-m0§.§_-v21§;
   import §_-sQ§.§_-H3D§;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.model.IPhysicModel;
   import ru.pragmatix.wormix.physics.model.UnitPhysicModel;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-z1x§;
   import starling.utils.Pool;
   
   public class Physics
   {
      
      public static const PI2:Number = Math.PI * 2;
      
      private static const GOBJ_HALF_WIDTH:uint = 4;
      
      private static const GOBJ_HALF_QUATER:uint = GOBJ_HALF_WIDTH << 1;
      
      public static const EPSILON:Number = 0.000001;
      
      public static const METER:uint = 60;
      
      public static const SQMETER:uint = METER * METER;
      
      public static const UNIT_WIDTH:uint = 34;
      
      public static const UNIT_HEIGHT:uint = 36;
      
      public static const UNIT_HALF_WIDTH:uint = UNIT_WIDTH >> 1;
      
      public static const UNIT_HALF_HEIGHT:uint = UNIT_HEIGHT >> 1;
      
      public function Physics()
      {
         super();
      }
      
      public static function teleportUnit(param1:§_-F3o§, param2:int, param3:int, param4:Function = null) : void
      {
         var _loc5_:Point = Pool.getPoint(param1.x,param1.y);
         var _loc6_:Point = Pool.getPoint(param2,param3);
         param1.x = param2;
         param1.y = param3;
         if(param4 != null)
         {
            param4();
         }
         param1.setSpeedXY(0,0);
         param1.§_-q1Q§();
         param1.dispatchEvent(new §_-v21§(_loc5_,_loc6_));
         param1.stable = false;
      }
      
      public static function processEachUnitCollidingUnits(param1:UnitPhysicModel, param2:§_-z1x§) : void
      {
         var _loc5_:§_-01J§ = null;
         var _loc6_:IPhysicModel = null;
         var _loc3_:Vector.<§_-01J§> = Application.scene.gameObjectProcessor.cache.getUnits();
         var _loc4_:int = int(_loc3_.length);
         var _loc7_:int = 0;
         while(_loc7_ < _loc4_)
         {
            _loc5_ = _loc3_[_loc7_];
            _loc6_ = _loc5_.getPhysicModel();
            if(_loc6_ != param1 && !_loc5_.disposed && _loc6_.stable)
            {
               if(Math.abs(_loc6_.x - param1.x) < GOBJ_HALF_QUATER << 1 && Math.abs(_loc6_.y - param1.y) < UNIT_HEIGHT)
               {
                  if(param2.processUnit(_loc5_))
                  {
                     break;
                  }
               }
            }
            _loc7_++;
         }
      }
      
      public static function getUnitsInCircle(param1:int, param2:int, param3:Number, param4:Vector.<§_-01J§> = null, param5:Boolean = true) : Vector.<§_-01J§>
      {
         var _loc6_:§_-01J§ = null;
         var _loc8_:Boolean = false;
         if(!param4)
         {
            param4 = new Vector.<§_-01J§>();
            param4 = param4.concat(Application.scene.gameObjectProcessor.cache.getUnits());
         }
         var _loc7_:* = int(param4.length);
         while(_loc7_--)
         {
            _loc6_ = param4[_loc7_];
            _loc8_ = isGobjInRadius(_loc6_ as §_-F3o§,param1,param2,param3,true);
            if(!param5 && _loc8_ || param5 && !_loc8_)
            {
               param4.splice(_loc7_,1);
            }
         }
         return param4;
      }
      
      public static function getGameObjectsInCircle(param1:int, param2:int, param3:Number, param4:Vector.<§_-F3o§>, param5:Boolean = true) : Vector.<§_-F3o§>
      {
         var _loc7_:§_-F3o§ = null;
         var _loc8_:Boolean = false;
         var _loc9_:Boolean = false;
         var _loc6_:Vector.<§_-F3o§> = new Vector.<§_-F3o§>(0);
         for each(_loc7_ in param4)
         {
            _loc8_ = _loc7_.physParams.collideType == CollideType.UNIT;
            _loc9_ = isGobjInRadius(_loc7_,param1,param2,param3,_loc8_);
            if(param5 && _loc9_ || !param5 && !_loc9_)
            {
               _loc6_.push(_loc7_);
            }
         }
         return _loc6_;
      }
      
      public static function isGameObjectsInCircle(param1:int, param2:int, param3:Number, param4:Vector.<§_-F3o§>, param5:Boolean = true, param6:Boolean = false) : Boolean
      {
         var _loc8_:Boolean = false;
         var _loc9_:Boolean = false;
         var _loc10_:§_-F3o§ = null;
         var _loc7_:Number = param3 * param3;
         for each(_loc10_ in param4)
         {
            _loc8_ = _loc10_.physParams.collideType == CollideType.UNIT;
            _loc9_ = isGobjInRadiusFast(_loc10_,param1,param2,param3,_loc7_,_loc8_,param6);
            if(param5 && _loc9_ || !param5 && !_loc9_)
            {
               return true;
            }
         }
         return false;
      }
      
      public static function girderCheckGameObjectsInCircle(param1:int, param2:int, param3:Number, param4:Vector.<§_-F3o§>) : Boolean
      {
         var _loc6_:Boolean = false;
         var _loc7_:Boolean = false;
         var _loc8_:§_-F3o§ = null;
         var _loc5_:Number = param3 * param3;
         var _loc9_:int = int(param4.length);
         while(--_loc9_ > -1)
         {
            _loc8_ = param4[_loc9_];
            if(!_loc8_.§_-l28§(§_-D3B§.§_-E25§))
            {
               _loc6_ = _loc8_.physParams.collideType == CollideType.UNIT;
               _loc7_ = isGobjInRadiusFast(_loc8_,param1,param2,param3,_loc5_,_loc6_,true);
               if(_loc7_)
               {
                  return true;
               }
            }
         }
         return false;
      }
      
      public static function isGameObjectInZone(param1:int, param2:int, param3:int, param4:int, param5:§_-F3o§) : Boolean
      {
         var _loc6_:int = param5.width * 0.5;
         var _loc7_:int = param5.height * 0.5;
         if(param5.x > param1 - _loc6_ && param5.x < param3 + _loc6_ && param5.y > param2 - _loc7_ && param5.y < param4 + _loc7_)
         {
            return true;
         }
         return false;
      }
      
      public static function isGameObjectInRectZone(param1:Rectangle, param2:§_-F3o§) : Boolean
      {
         return isGameObjectInZone(param1.left,param1.top,param1.right,param1.bottom,param2);
      }
      
      public static function getGameObjectsInCone(param1:int, param2:int, param3:Number, param4:Number, param5:Number, param6:Vector.<§_-F3o§>) : Vector.<§_-F3o§>
      {
         var _loc9_:§_-F3o§ = null;
         var _loc10_:Number = NaN;
         var _loc7_:Vector.<§_-F3o§> = new Vector.<§_-F3o§>(0);
         var _loc8_:Vector.<§_-F3o§> = getGameObjectsInCircle(param1,param2,param3,param6);
         for each(_loc9_ in _loc8_)
         {
            _loc10_ = Math.atan2(_loc9_.y - param2,_loc9_.x - param1);
            if(Math.abs(_loc10_ - param4) < param5 || Math.abs(_loc10_ - param4) > PI2 - param5 && Math.abs(_loc10_ - param4) < PI2 + param5)
            {
               _loc7_.push(_loc9_);
            }
         }
         return _loc7_;
      }
      
      public static function isGobjInRadius(param1:§_-F3o§, param2:int, param3:int, param4:Number, param5:Boolean) : Boolean
      {
         if(!param1.getPhysicModel())
         {
            return false;
         }
         var _loc6_:int = param1.x;
         var _loc7_:int = param1.y;
         if(param5)
         {
            if(param3 > param1.y + UNIT_HALF_HEIGHT)
            {
               _loc7_ = param1.y + UNIT_HALF_HEIGHT;
            }
            else if(param3 < param1.y - UNIT_HALF_HEIGHT)
            {
               _loc7_ = param1.y - UNIT_HALF_HEIGHT;
            }
         }
         return isPointInCircle(_loc6_,_loc7_,param2,param3,param4,param4 * param4);
      }
      
      public static function isGobjInRadiusFast(param1:§_-F3o§, param2:int, param3:int, param4:Number, param5:Number, param6:Boolean, param7:Boolean = false) : Boolean
      {
         var _loc11_:int = 0;
         var _loc12_:int = 0;
         var _loc13_:Boolean = false;
         if(!param1.getPhysicModel())
         {
            return false;
         }
         var _loc8_:int = param1.x;
         var _loc9_:int = param1.y;
         if(param6)
         {
            if(param3 > param1.y + UNIT_HALF_HEIGHT)
            {
               _loc9_ = param1.y + UNIT_HALF_HEIGHT;
            }
            else if(param3 < param1.y - UNIT_HALF_HEIGHT)
            {
               _loc9_ = param1.y - UNIT_HALF_HEIGHT;
            }
         }
         var _loc10_:Boolean = isPointInCircle(_loc8_,_loc9_,param2,param3,param4,param5);
         if(param7)
         {
            _loc11_ = param1.physParams.groundShiftX + _loc8_;
            _loc12_ = param1.physParams.groundShiftY + _loc9_;
            _loc13_ = isPointInCircle(_loc11_,_loc12_,param2,param3,param4,param5);
            return _loc10_ || _loc13_;
         }
         return _loc10_;
      }
      
      private static function isPointInCircle(param1:int, param2:int, param3:int, param4:int, param5:Number, param6:Number) : Boolean
      {
         var _loc7_:int = param1 - param3;
         var _loc8_:int = param2 - param4;
         if(Math.abs(_loc7_) > param5 || Math.abs(_loc8_) > param5)
         {
            return false;
         }
         return param6 >= _loc7_ * _loc7_ + _loc8_ * _loc8_;
      }
      
      public static function getGameObjectsInZone(param1:int, param2:int, param3:int, param4:int, param5:Vector.<§_-F3o§>) : Vector.<§_-F3o§>
      {
         var _loc7_:§_-F3o§ = null;
         var _loc8_:int = 0;
         var _loc9_:int = 0;
         var _loc6_:Vector.<§_-F3o§> = new Vector.<§_-F3o§>(0);
         for each(_loc7_ in param5)
         {
            _loc8_ = _loc7_.width / 2;
            _loc9_ = _loc7_.height / 2;
            if(_loc7_.x > param1 - _loc8_ && _loc7_.x < param3 + _loc8_ && _loc7_.y > param2 - _loc9_ && _loc7_.y < param4 + _loc9_)
            {
               _loc6_.push(_loc7_);
            }
         }
         return _loc6_;
      }
      
      public static function nearestFreePoint(param1:int, param2:int, param3:Function, param4:int = 100, param5:Point = null) : Boolean
      {
         var _loc7_:* = 0;
         if(param5 == null)
         {
            param5 = Pool.getPoint();
         }
         if(!param3(param1,param2))
         {
            param5.setTo(param1,param2);
            return true;
         }
         if(param4 < 1)
         {
            param4 = 1;
         }
         var _loc6_:int = 1;
         while(_loc6_ <= param4)
         {
            _loc7_ = int(param1 - _loc6_);
            while(_loc7_ < param1 + _loc6_)
            {
               if(!param3(_loc7_,param2 - _loc6_))
               {
                  param5.setTo(_loc7_,param2 - _loc6_);
                  return true;
               }
               _loc7_++;
            }
            _loc7_ = int(param2 - _loc6_);
            while(_loc7_ < param2 + _loc6_)
            {
               if(!param3(param1 + _loc6_,_loc7_))
               {
                  param5.setTo(param1 + _loc6_,_loc7_);
                  return true;
               }
               _loc7_++;
            }
            _loc7_ = int(param1 + _loc6_);
            while(_loc7_ > param1 - _loc6_)
            {
               if(!param3(_loc7_,param2 + _loc6_))
               {
                  param5.setTo(_loc7_,param2 + _loc6_);
                  return true;
               }
               _loc7_--;
            }
            _loc7_ = int(param2 + _loc6_);
            while(_loc7_ < param2 - _loc6_)
            {
               if(!param3(param1 - _loc6_,_loc7_))
               {
                  param5.setTo(param1 - _loc6_,_loc7_);
                  return true;
               }
               _loc7_--;
            }
            _loc6_++;
         }
         return false;
      }
      
      public static function nearestFreePlaceForObject(param1:int, param2:int, param3:int, param4:int = 100, param5:Point = null) : Boolean
      {
         var r:int;
         var i:int = 0;
         var hitTestFunction:Function = null;
         var radius:int = 0;
         var x:int = param1;
         var y:int = param2;
         var areaSize:int = param3;
         var limit:int = param4;
         var out:Point = param5;
         if(out == null)
         {
            out = Pool.getPoint();
         }
         if(!HitTest.circle(x,y,true,areaSize))
         {
            out.setTo(x,y);
            return true;
         }
         if(limit < 1)
         {
            limit = 1;
         }
         r = 1;
         radius = Math.floor(areaSize * 0.5);
         if(areaSize == 1)
         {
            hitTestFunction = function(param1:int, param2:int):Boolean
            {
               return HitTest.point(param1,param2);
            };
         }
         else
         {
            hitTestFunction = function(param1:int, param2:int):Boolean
            {
               return HitTest.circle(param1,param2,true,radius);
            };
         }
         while(r <= limit)
         {
            i = x - r;
            while(i < x + r)
            {
               if(!hitTestFunction(i,y - r))
               {
                  out.setTo(i,y - r);
                  return true;
               }
               i++;
            }
            i = y - r;
            while(i < y + r)
            {
               if(!hitTestFunction(x + r,i))
               {
                  out.setTo(x + r,i);
                  return true;
               }
               i++;
            }
            i = x + r;
            while(i > x - r)
            {
               if(!hitTestFunction(i,y + r))
               {
                  out.setTo(i,y + r);
                  return true;
               }
               i--;
            }
            i = y + r;
            while(i < y - r)
            {
               if(!hitTestFunction(x - r,i))
               {
                  out.setTo(x - r,i);
                  return true;
               }
               i--;
            }
            r++;
         }
         return false;
      }
      
      public static function nearestObject(param1:§_-F3o§, param2:*, param3:int = -1) : §_-F3o§
      {
         return nearestObjectToPoint(param1.x,param1.y,param2,param3,param1);
      }
      
      public static function nearestObjectToPoint(param1:int, param2:int, param3:*, param4:int = -1, param5:§_-F3o§ = null) : §_-F3o§
      {
         var _loc9_:int = 0;
         var _loc10_:int = 0;
         var _loc12_:§_-F3o§ = null;
         var _loc6_:§_-F3o§ = null;
         var _loc7_:Number = Number.MAX_VALUE;
         var _loc8_:Number = 0;
         var _loc11_:int = int(param3.length);
         while(--_loc11_ > -1)
         {
            _loc12_ = param3[_loc11_];
            if((Boolean(_loc12_)) && Boolean(!_loc12_.disposed) && _loc12_ != param5)
            {
               _loc9_ = _loc12_.x - param1;
               _loc10_ = _loc12_.y - param2;
               _loc8_ = _loc9_ * _loc9_ + _loc10_ * _loc10_;
               if(_loc8_ < _loc7_)
               {
                  _loc7_ = _loc8_;
                  _loc6_ = _loc12_;
               }
            }
         }
         if(param4 > 0 && _loc7_ > param4 * param4)
         {
            _loc6_ = null;
         }
         return _loc6_;
      }
      
      public static function distance(param1:§_-F3o§, param2:§_-F3o§) : Number
      {
         return §_-H3D§.distance(param1.x - param2.x,param1.y - param2.y);
      }
      
      public static function getGroundDiff(param1:int, param2:int, param3:int = 20) : int
      {
         var _loc5_:int = 0;
         var _loc4_:int = param2;
         if(Application.scene.hitTestPoint(param1,param2))
         {
            _loc4_ = param2 - 10;
            _loc5_ = 1;
            while(_loc5_ <= param3)
            {
               if(!Application.scene.hitTestPoint(param1,param2 - _loc5_))
               {
                  _loc4_ = param2 - _loc5_;
                  break;
               }
               _loc5_++;
            }
         }
         else
         {
            _loc4_ = param2;
            _loc5_ = 1;
            while(_loc5_ <= param3 + 1)
            {
               if(Application.scene.hitTestPoint(param1,param2 + _loc5_))
               {
                  _loc4_ = param2 + _loc5_ - 1;
                  break;
               }
               _loc5_++;
            }
         }
         return param2 - _loc4_;
      }
      
      public static function getNormalInPoint(param1:int, param2:int) : Number
      {
         var _loc3_:int = getGroundDiff(param1 - 3,param2);
         var _loc4_:int = getGroundDiff(param1 + 3,param2);
         var _loc5_:Number = Math.atan2(_loc3_ - _loc4_,4);
         if(_loc5_ < 0)
         {
            _loc5_ += Math.PI;
         }
         return _loc5_ < MathUtil.PI_HALF ? _loc5_ + MathUtil.PI_HALF : _loc5_ - MathUtil.PI_HALF;
      }
      
      public static function getNormal(param1:int, param2:int, param3:uint = 20, param4:Number = 1.5707963267948966) : Number
      {
         var _loc7_:Number = NaN;
         var _loc8_:Number = NaN;
         var _loc10_:int = 0;
         var _loc11_:int = 0;
         var _loc5_:int = 64;
         var _loc6_:uint = 0;
         var _loc9_:int = -_loc5_;
         while(_loc9_ < _loc5_)
         {
            _loc10_ = Math.cos(_loc9_ * Math.PI / _loc5_) * param3;
            _loc11_ = Math.sin(_loc9_ * Math.PI / _loc5_) * param3;
            if(Application.scene.hitTestPoint(param1 + _loc10_,param2 + _loc11_))
            {
               if(_loc6_ == 0)
               {
                  _loc7_ = _loc10_;
                  _loc8_ = _loc11_;
               }
               else
               {
                  _loc7_ += _loc10_;
                  _loc8_ += _loc11_;
               }
               _loc6_++;
            }
            _loc9_++;
         }
         return _loc6_ != 0 ? Math.atan2(_loc8_,_loc7_) : param4;
      }
      
      public static function sortByDistanceToPoint(param1:int, param2:int, param3:Array) : Vector.<§_-F3o§>
      {
         var gobj:§_-F3o§ = null;
         var x:int = param1;
         var y:int = param2;
         var selection:Array = param3;
         var result:Vector.<§_-F3o§> = new Vector.<§_-F3o§>(0);
         for each(gobj in selection)
         {
            if(!gobj.disposed)
            {
               result.push(gobj);
            }
         }
         result.sort(function(param1:§_-F3o§, param2:§_-F3o§):int
         {
            var _loc3_:Number = (param1.x - x) * (param1.x - x) + (param1.y - y) * (param1.y - y);
            var _loc4_:Number = (param2.x - x) * (param2.x - x) + (param2.y - y) * (param2.y - y);
            return §_-H3D§.sign(_loc3_ - _loc4_);
         });
         return result;
      }
   }
}

