package ru.pragmatix.wormix.physics
{
   import §_-sQ§.§_-H3D§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.§_-w1N§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.Girder;
   import starling.utils.Pool;
   
   public class HitTest
   {
      
      internal static const GOBJ_HALF_WIDTH:uint = 4;
      
      internal static const GOBJ_MIN_HEIGHT:uint = 5;
      
      internal static const CAT_RADIUS:uint = 12;
      
      public function HitTest()
      {
         super();
      }
      
      public static function unit(param1:int, param2:int, param3:Boolean = false) : Boolean
      {
         return gameObject(param1,param2,Physics.UNIT_HALF_HEIGHT,param3);
      }
      
      public static function girder(param1:int, param2:int, param3:Number, param4:Number) : Boolean
      {
         var testRadius:uint;
         var dx:Number;
         var dy:Number;
         var testObjects:Vector.<§_-F3o§>;
         var objHitTest:Boolean;
         var angleInRadians:Number = NaN;
         var halfWidth:int = 0;
         var cos:Number = NaN;
         var sin:Number = NaN;
         var groundHitTest:Boolean = false;
         var x1:int = 0;
         var y1:int = 0;
         var x2:int = 0;
         var y2:int = 0;
         var testStep:uint = 0;
         var ox1:int = 0;
         var oy1:int = 0;
         var ox2:int = 0;
         var oy2:int = 0;
         var steps:int = 0;
         var x:int = param1;
         var y:int = param2;
         var angle:Number = param3;
         var width:Number = param4;
         angleInRadians = MathUtil.§_-Z1G§(angle);
         halfWidth = width / 2;
         var halfHeight:int = Girder.HEIGHT.value / 2;
         cos = Math.cos(angleInRadians);
         sin = Math.sin(angleInRadians);
         var xcos1:Number = cos * -halfWidth;
         var xsin1:Number = sin * -halfWidth;
         var xcos2:Number = cos * halfWidth;
         var xsin2:Number = sin * halfWidth;
         groundHitTest = false;
         var i:int = -halfHeight;
         while(i <= halfHeight)
         {
            x1 = x + xcos1 - i * sin;
            y1 = y + xsin1 + i * cos;
            x2 = x + xcos2 - i * sin;
            y2 = y + xsin2 + i * cos;
            §_-H3D§.§_-w1I§(x1,y1,x2,y2,function(param1:int, param2:int):void
            {
               if(!groundHitTest)
               {
                  groundHitTest = groundHitTest || §_-X1j§.§_-12n§.scene.hitTestPoint(param1,param2);
                  if(param1 < 20 || param2 < 100 || param1 > §_-X1j§.§_-12n§.scene.sceneW - 20 || param2 > §_-X1j§.§_-12n§.scene.sceneH - 20)
                  {
                     groundHitTest = true;
                  }
               }
            });
            if(groundHitTest)
            {
               return false;
            }
            i++;
         }
         testRadius = 25;
         testStep = 15;
         dx = cos * testStep;
         dy = sin * testStep;
         testObjects = §_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-a2S§();
         objHitTest = Physics.girderCheckGameObjectsInCircle(x,y,testRadius,testObjects);
         if(!objHitTest)
         {
            ox1 = x;
            oy1 = y;
            ox2 = x;
            oy2 = y;
            steps = halfWidth / testStep;
            i = 1;
            while(i <= steps)
            {
               ox1 += dx;
               oy1 += dy;
               ox2 -= dx;
               oy2 -= dy;
               objHitTest = objHitTest || Physics.girderCheckGameObjectsInCircle(ox1,oy1,testRadius,testObjects) || Physics.girderCheckGameObjectsInCircle(ox2,oy2,testRadius,testObjects);
               if(objHitTest)
               {
                  return false;
               }
               i++;
            }
            return true;
         }
         return false;
      }
      
      public static function gameObject(param1:int, param2:int, param3:uint, param4:Boolean = false) : Boolean
      {
         var _loc6_:int = 0;
         if(param3 < 5)
         {
            param3 = GOBJ_MIN_HEIGHT;
         }
         var _loc5_:int = -GOBJ_HALF_WIDTH;
         while(_loc5_ <= GOBJ_HALF_WIDTH)
         {
            _loc6_ = param2 - param3 + Math.abs(_loc5_);
            while(_loc6_ <= param2 + param3 - Math.abs(_loc5_))
            {
               if(point(param1 + _loc5_,_loc6_,param4))
               {
                  §_-X1j§.§_-12n§.§_-Y0§.§_-N1p§("gameObject collides at " + param1 + ":" + param2,param4,"actual point: " + (param1 + _loc5_) + ":" + _loc6_);
                  return true;
               }
               _loc6_++;
            }
            _loc5_++;
         }
         return false;
      }
      
      public static function point(param1:int, param2:int, param3:Boolean = false) : Boolean
      {
         return param3 ? Application.scene.hitTestPermanent(param1,param2) : Application.scene.hitTestPoint(param1,param2);
      }
      
      public static function gobjUpDown(param1:int, param2:int, param3:uint, param4:Boolean, param5:Boolean, param6:Boolean = true) : Boolean
      {
         var _loc7_:int = param5 ? -1 : 1;
         var _loc8_:int = param2 + _loc7_ * (param3 - GOBJ_HALF_WIDTH);
         var _loc9_:int = param6 ? int(GOBJ_HALF_WIDTH) : int(GOBJ_HALF_WIDTH - 1);
         if(!param6)
         {
            _loc8_ += _loc7_;
         }
         var _loc10_:int = -_loc9_;
         while(_loc10_ <= _loc9_)
         {
            if(point(param1 - _loc10_,_loc8_,param4))
            {
               §_-X1j§.§_-12n§.§_-Y0§.§_-N1p§("gameObject collides up/down at " + param1 + ":" + param2,param4,"actual point: " + (param1 - _loc10_) + ":" + _loc8_);
               return true;
            }
            if(_loc10_ == 0)
            {
               _loc7_ *= -1;
            }
            _loc8_ += _loc7_;
            _loc10_++;
         }
         return false;
      }
      
      public static function gobjLeftRight(param1:int, param2:int, param3:uint, param4:Boolean, param5:Boolean) : Boolean
      {
         var _loc9_:int = 0;
         var _loc6_:int = param5 ? -1 : 1;
         var _loc7_:int = param1 + _loc6_ * GOBJ_HALF_WIDTH;
         var _loc8_:int = -param3;
         while(_loc8_ <= param3)
         {
            _loc9_ = param5 ? int(Math.max(param1,_loc7_)) : int(Math.min(param1,_loc7_));
            if(point(_loc9_,param2 + _loc8_,param4))
            {
               return true;
            }
            if(_loc8_ == 0)
            {
               _loc6_ *= -1;
            }
            param1 += _loc6_;
            _loc8_++;
         }
         return false;
      }
      
      public static function circle(param1:int, param2:int, param3:Boolean = true, param4:uint = 12, param5:Boolean = false) : Boolean
      {
         var _loc6_:int = 0;
         var _loc7_:* = 0;
         var _loc8_:int = 0;
         var _loc9_:int = 0;
         var _loc10_:int = 0;
         var _loc11_:int = 0;
         var _loc12_:uint = 0;
         var _loc14_:int = 0;
         _loc6_ = _loc8_ = 0;
         _loc7_ = int(param4);
         _loc10_ = 2 * (1 - param4);
         var _loc13_:§_-w1N§ = Application.scene;
         do
         {
            _loc12_ = param3 || _loc6_ == 0 ? 1 : uint(_loc6_ << 1);
            _loc11_ = 0;
            _loc14_ = param1 - _loc6_;
            while(_loc14_ <= param1 + _loc6_)
            {
               if(param5)
               {
                  if(_loc13_.hitTestPermanent(_loc14_,param2 - _loc7_))
                  {
                     return true;
                  }
                  if(_loc13_.hitTestPermanent(_loc14_,param2 + _loc7_))
                  {
                     return true;
                  }
               }
               else
               {
                  if(_loc13_.hitTestPoint(_loc14_,param2 - _loc7_))
                  {
                     return true;
                  }
                  if(_loc13_.hitTestPoint(_loc14_,param2 + _loc7_))
                  {
                     return true;
                  }
               }
               _loc14_ += _loc12_;
            }
            if(_loc7_ < _loc8_)
            {
               break;
            }
            if(_loc10_ < 0)
            {
               _loc9_ = 2 * (_loc10_ + _loc7_) - 1;
               if(_loc9_ <= 0)
               {
                  _loc6_++;
                  _loc10_ += 2 * _loc6_ + 1;
                  _loc11_ = 1;
               }
            }
            else if(_loc10_ > 0)
            {
               _loc9_ = 2 * (_loc10_ - _loc6_) - 1;
               if(_loc9_ > 0)
               {
                  _loc7_--;
                  _loc10_ += 1 - 2 * _loc7_;
                  _loc11_ = 1;
               }
            }
            if(!_loc11_)
            {
               _loc6_++;
               _loc7_--;
               _loc10_ += 2 * (_loc6_ - _loc7_ - 1);
            }
         }
         while(1);
         return false;
      }
      
      public static function castRayVec(param1:int, param2:int, param3:Number, param4:uint) : Point
      {
         var _loc5_:int = int(param4 * Math.cos(param3));
         var _loc6_:int = int(param4 * Math.sin(param3));
         return castRay(param1,param2,_loc5_,_loc6_);
      }
      
      public static function castRay(param1:int, param2:int, param3:int, param4:int) : Point
      {
         var _loc5_:Number = NaN;
         _loc5_ = Math.max(Math.abs(param3),Math.abs(param4));
         var _loc6_:Number = param3 / _loc5_;
         var _loc7_:Number = param4 / _loc5_;
         var _loc8_:Number = 1 * param1;
         var _loc9_:Number = 1 * param2;
         var _loc10_:uint = 0;
         while(_loc10_ < int(_loc5_))
         {
            _loc8_ += _loc6_;
            _loc9_ += _loc7_;
            if(point(int(_loc8_),int(_loc9_)))
            {
               return Pool.getPoint(int(_loc8_),int(_loc9_));
            }
            _loc10_++;
         }
         return null;
      }
   }
}

