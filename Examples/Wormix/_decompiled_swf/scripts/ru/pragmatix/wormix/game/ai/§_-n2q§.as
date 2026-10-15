package ru.pragmatix.wormix.game.ai
{
   import §_-21p§.§_-4w§;
   import §_-2k§.*;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-W2f§;
   import §_-l1K§.§_-d2j§;
   import §_-p24§.*;
   import §_-sQ§.§_-H3D§;
   import com.hurlant.crypto.tls.§_-A3§;
   import feathers.controls.Button;
   import feathers.core.IFeathersControl;
   import feathers.layout.ILayoutDisplayObject;
   import feathers.layout.LayoutBoundsResult;
   import feathers.layout.ViewPortBounds;
   import flash.events.StatusEvent;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-E2w§;
   import ru.pragmatix.wormix.messages.server.SendWipeConfirmCodeResponse;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.GenericPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class §_-n2q§
   {
      
      public function §_-n2q§()
      {
         super();
      }
      
      public static function §_-Tp§(param1:int, param2:int, param3:int, param4:int, param5:uint, param6:Vector.<§_-L1S§> = null, param7:Boolean = false, param8:Boolean = true, param9:Number = 10) : Vector.<§_-L1S§>
      {
         var checkAndPushAngle:Function;
         var ca:Number;
         var cb:Number;
         var cc:Number;
         var ANGLE_DELTA:Number = NaN;
         var vx:Number = NaN;
         var vy:Number = NaN;
         var a:Number = NaN;
         var g:Number = NaN;
         var p:uint = 0;
         var x1:Number = NaN;
         var y1:Number = NaN;
         var x2:Number = NaN;
         var y2:Number = NaN;
         var cx:Number = NaN;
         var cy:Number = NaN;
         var d:Number = NaN;
         var d_:Number = NaN;
         var z1:Number = NaN;
         var z2:Number = NaN;
         var t1:Number = NaN;
         var t2:Number = NaN;
         var sourceX:int = param1;
         var sourceY:int = param2;
         var targetX:int = param3;
         var targetY:int = param4;
         var power:uint = param5;
         var result:Vector.<§_-L1S§> = param6;
         var useWind:Boolean = param7;
         var useGravity:Boolean = param8;
         var precision:Number = param9;
         result.splice(0,result.length);
         ANGLE_DELTA = §_-W2f§.§_-G3n§ / precision;
         vx = NaN;
         vy = NaN;
         a = useWind ? Force.WIND.ax : 0;
         g = useGravity ? Force.GRAVITY.ay : 0;
         p = power;
         x1 = sourceX;
         y1 = sourceY;
         x2 = targetX;
         y2 = targetY;
         cx = x2 - x1;
         cy = y2 - y1;
         checkAndPushAngle = function(param1:Number):void
         {
            var _loc2_:Number = NaN;
            var _loc3_:Number = NaN;
            if(param1 > 0)
            {
               vx = (x2 - a * param1 * param1 / 2 - x1) / param1;
               vy = (y2 - g * param1 * param1 / 2 - y1) / param1;
               _loc2_ = Math.atan2(vy,vx);
               _loc3_ = Math.abs(Math.round(_loc2_ / ANGLE_DELTA) * ANGLE_DELTA - _loc2_) / ANGLE_DELTA * 100;
               if(!isNaN(_loc2_))
               {
                  result.push(new §_-L1S§(Math.round(_loc2_ / ANGLE_DELTA) * ANGLE_DELTA,p,_loc3_,param1));
               }
            }
         };
         ca = (g * g + a * a) / 4;
         cb = a * cx + g * cy + p * p;
         cc = cy * cy + cx * cx;
         if(Math.abs(ca) > Physics.EPSILON)
         {
            d = cb * cb - 4 * ca * cc;
            if(d >= 0)
            {
               d_ = Math.sqrt(d);
               z1 = (cb + d_) / 2 / ca;
               z2 = (cb - d_) / 2 / ca;
               t1 = 0;
               t2 = 0;
               if(z1 > 0)
               {
                  t1 = Math.sqrt(z1);
               }
               if(z2 > 0)
               {
                  t2 = Math.sqrt(z2);
               }
               checkAndPushAngle(t1);
               checkAndPushAngle(t2);
            }
         }
         else
         {
            checkAndPushAngle(cc / cb);
         }
         return result;
      }
      
      public static function §_-K3o§(param1:uint, param2:uint, param3:Number, param4:Number, param5:Point = null) : Point
      {
         var _loc6_:int = 0;
         var _loc7_:int = 0;
         var _loc8_:Number = NaN;
         var _loc9_:int = 0;
         var _loc10_:int = 0;
         if(param4 < 1.05 || §_-X1j§.randomSeed.§_-y2j§() < 0.05)
         {
            _loc6_ = int(param1);
            _loc7_ = int(param2);
         }
         else
         {
            _loc8_ = Math.max(0,param3 - 100 * (6 - param4));
            _loc9_ = Math.round(1.2 * Math.sqrt(param4 * _loc8_));
            _loc10_ = _loc9_ >> 2;
            _loc6_ = param1 + §_-H3D§.sign(0.5 - §_-X1j§.randomSeed.§_-y2j§()) * (_loc10_ + §_-X1j§.randomSeed.§_-y2j§() * _loc9_);
            _loc7_ = param2 + §_-H3D§.sign(0.5 - §_-X1j§.randomSeed.§_-y2j§()) * (_loc10_ + §_-X1j§.randomSeed.§_-y2j§() * _loc9_);
         }
         if(param5 == null)
         {
            param5 = Pool.getPoint();
         }
         param5.setTo(_loc6_,_loc7_);
         return param5;
      }
      
      public static function §_-X1L§(param1:int, param2:int, param3:int, param4:int, param5:uint, param6:uint, param7:Boolean = false, param8:Boolean = true) : Vector.<§_-L1S§>
      {
         var _loc16_:uint = 0;
         var _loc17_:Number = NaN;
         var _loc18_:Number = NaN;
         var _loc19_:Number = NaN;
         var _loc20_:Number = NaN;
         var _loc21_:Number = NaN;
         var _loc22_:Number = NaN;
         var _loc23_:Number = NaN;
         var _loc24_:Number = NaN;
         var _loc25_:Number = NaN;
         var _loc26_:Number = NaN;
         var _loc27_:Number = NaN;
         var _loc28_:Number = NaN;
         var _loc29_:Number = NaN;
         var _loc30_:Number = NaN;
         var _loc31_:Number = NaN;
         var _loc32_:Number = NaN;
         var _loc33_:Number = NaN;
         var _loc34_:Number = NaN;
         var _loc35_:Number = NaN;
         var _loc36_:Number = NaN;
         var _loc9_:uint = uint(§_-d2j§.MAX_POWER);
         var _loc10_:Number = §_-W2f§.§_-G3n§;
         var _loc11_:Vector.<§_-L1S§> = new Vector.<§_-L1S§>(0);
         var _loc12_:Number = param7 ? Force.WIND.ax : 0;
         var _loc13_:Number = param8 ? Force.GRAVITY.ay : 0;
         var _loc14_:int = Math.round((param6 - param5) / _loc9_);
         var _loc15_:int = 1;
         while(_loc15_ <= _loc9_)
         {
            _loc16_ = _loc14_ * _loc15_ + param5;
            _loc17_ = param1;
            _loc18_ = param2;
            _loc19_ = param3;
            _loc20_ = param4;
            _loc21_ = _loc19_ - _loc17_;
            _loc22_ = _loc20_ - _loc18_;
            _loc23_ = (_loc13_ * _loc13_ + _loc12_ * _loc12_) / 4;
            _loc24_ = _loc12_ * _loc21_ + _loc13_ * _loc22_ + _loc16_ * _loc16_;
            _loc25_ = _loc22_ * _loc22_ + _loc21_ * _loc21_;
            _loc26_ = _loc24_ * _loc24_ - 4 * _loc23_ * _loc25_;
            if(_loc26_ >= 0)
            {
               _loc27_ = Math.sqrt(_loc26_);
               _loc28_ = (_loc24_ + _loc27_) / 2 / _loc23_;
               _loc29_ = (_loc24_ - _loc27_) / 2 / _loc23_;
               _loc30_ = 0;
               _loc31_ = 0;
               if(_loc28_ > 0)
               {
                  _loc30_ = Math.sqrt(_loc28_);
               }
               if(_loc29_ > 0)
               {
                  _loc31_ = Math.sqrt(_loc29_);
               }
               for each(_loc32_ in [_loc30_,_loc31_])
               {
                  if(_loc32_ > 0)
                  {
                     _loc33_ = (_loc19_ - _loc12_ * _loc32_ * _loc32_ / 2 - _loc17_) / _loc32_;
                     _loc34_ = (_loc20_ - _loc13_ * _loc32_ * _loc32_ / 2 - _loc18_) / _loc32_;
                     _loc35_ = Math.atan2(_loc34_,_loc33_);
                     _loc36_ = Math.abs(Math.round(_loc35_ / _loc10_) * _loc10_ - _loc35_) / _loc10_ * 100;
                     _loc11_[_loc11_.length] = new §_-L1S§(Math.round(_loc35_ / _loc10_) * _loc10_,_loc16_,_loc36_,_loc32_);
                  }
               }
            }
            _loc15_++;
         }
         return _loc11_;
      }
      
      public static function §_-a1M§(param1:int, param2:int, param3:int, param4:int, param5:uint, param6:Number, param7:Vector.<§_-01J§>, param8:uint = 50, param9:Boolean = false, param10:Boolean = true, param11:Boolean = false) : §_-O6§
      {
         var _loc30_:Number = NaN;
         var _loc31_:Number = NaN;
         var _loc32_:int = 0;
         var _loc33_:int = 0;
         var _loc34_:int = 0;
         var _loc35_:int = 0;
         var _loc12_:Scene = §_-X1j§.§_-12n§.scene;
         var _loc13_:int = param1;
         var _loc14_:int = param2;
         var _loc15_:Number = param5 * Math.cos(param6);
         var _loc16_:Number = param5 * Math.sin(param6);
         var _loc17_:Number = param9 ? Force.WIND.ax : 0;
         var _loc18_:Number = param10 ? Force.GRAVITY.ay : 0;
         var _loc19_:int = 0;
         var _loc20_:int = 0;
         var _loc21_:int = 0;
         var _loc22_:int = -1;
         var _loc23_:int = 0;
         var _loc24_:int = 10;
         var _loc25_:Number = _loc13_;
         var _loc26_:Number = _loc14_;
         var _loc27_:Number = _loc15_ / _loc24_;
         var _loc28_:Number = _loc16_ / _loc24_;
         var _loc29_:* = 1;
         while(Math.abs(_loc13_ - param3) > param8 || Math.abs(_loc14_ - param4) > param8)
         {
            _loc25_ += _loc27_;
            _loc26_ += _loc28_;
            _loc30_ = _loc25_;
            _loc31_ = _loc26_;
            if(_loc29_++ % _loc24_ == 0)
            {
               _loc27_ += _loc17_ / _loc24_;
               _loc28_ += _loc18_ / _loc24_;
               _loc29_ = 1;
            }
            _loc23_ += Math.round(Math.sqrt(_loc27_ * _loc27_ + _loc28_ * _loc28_));
            if(_loc13_ != Math.round(_loc30_) || _loc14_ != Math.round(_loc31_))
            {
               _loc13_ = Math.round(_loc30_);
               _loc14_ = Math.round(_loc31_);
               _loc20_++;
               if(_loc12_.hitTestPoint(_loc13_,_loc14_))
               {
                  if(_loc19_ == 0)
                  {
                     _loc32_ = _loc13_ - param1;
                     _loc33_ = _loc14_ - param2;
                     _loc19_ = Math.round(Math.sqrt(_loc32_ * _loc32_ + _loc33_ * _loc33_));
                  }
               }
               else if(§_-bK§(_loc13_,_loc14_,param8,param7))
               {
                  if(_loc22_ == -1)
                  {
                     _loc34_ = _loc13_ - param1;
                     _loc35_ = _loc14_ - param2;
                     _loc22_ = Math.round(Math.sqrt(_loc34_ * _loc34_ + _loc35_ * _loc35_));
                  }
               }
               else
               {
                  _loc21_++;
               }
               if(_loc19_ != 0 && _loc19_ < 100 && !param11)
               {
                  _loc21_ = 0;
                  break;
               }
               if(_loc13_ < 0 || _loc13_ > _loc12_.sceneW || _loc14_ < -_loc12_.sceneH || _loc14_ > _loc12_.sceneH)
               {
                  return new §_-O6§(0,1,0,-1);
               }
            }
         }
         return new §_-O6§(_loc23_,_loc19_,Math.round(_loc21_ * 100 / _loc20_),_loc22_);
      }
      
      public static function §_-A2q§(param1:int, param2:int, param3:int, param4:int, param5:Number, param6:Number, param7:Boolean = false, param8:Boolean = true, param9:Point = null) : Boolean
      {
         var _loc20_:Number = NaN;
         var _loc21_:Number = NaN;
         var _loc10_:int = param1;
         var _loc11_:int = param2;
         var _loc12_:Number = param7 ? Force.WIND.ax : 0;
         var _loc13_:Number = param8 ? Force.GRAVITY.ay : 0;
         var _loc14_:int = 10;
         var _loc15_:Number = _loc10_;
         var _loc16_:Number = _loc11_;
         var _loc17_:Number = param5 / _loc14_;
         var _loc18_:Number = param6 / _loc14_;
         var _loc19_:* = 1;
         while(Math.abs(_loc11_ - param4) > 10)
         {
            _loc15_ += _loc17_;
            _loc16_ += _loc18_;
            _loc20_ = _loc15_;
            _loc21_ = _loc16_;
            if(_loc19_++ % _loc14_ == 0)
            {
               _loc17_ += _loc12_ / _loc14_;
               _loc18_ += _loc13_ / _loc14_;
               _loc19_ = 1;
            }
            if(_loc10_ != Math.round(_loc20_) || _loc11_ != Math.round(_loc21_))
            {
               _loc10_ = Math.round(_loc20_);
               _loc11_ = Math.round(_loc21_);
               if(§_-X1j§.§_-12n§.scene.hitTestPoint(_loc10_,_loc11_))
               {
                  if(param9 != null)
                  {
                     param9.setTo(_loc10_,_loc11_);
                  }
                  return true;
               }
            }
         }
         return false;
      }
      
      private static function §_-bK§(param1:int, param2:int, param3:int, param4:Vector.<§_-01J§>) : Boolean
      {
         var _loc6_:§_-01J§ = null;
         var _loc5_:Boolean = false;
         for each(_loc6_ in param4)
         {
            if(Math.abs(_loc6_.x - param1) < param3 && Math.abs(_loc6_.y - param2) < param3)
            {
               return true;
            }
         }
         return _loc5_;
      }
      
      public static function §_-41S§(param1:§_-01J§) : §_-01J§
      {
         var _loc4_:§_-01J§ = null;
         var _loc5_:§_-01J§ = null;
         var _loc6_:int = 0;
         var _loc7_:int = 0;
         var _loc8_:Number = NaN;
         var _loc2_:Vector.<§_-01J§> = §_-X1j§.§_-12n§.gameMaster.§_-83I§(param1);
         var _loc3_:int = int.MAX_VALUE;
         for each(_loc5_ in _loc2_)
         {
            _loc6_ = _loc5_.x - param1.x;
            _loc7_ = _loc5_.y - param1.y;
            _loc8_ = Math.sqrt(_loc6_ * _loc6_ + _loc7_ * _loc7_);
            if(_loc8_ < _loc3_)
            {
               _loc3_ = _loc8_;
               _loc4_ = _loc5_;
            }
         }
         return _loc4_;
      }
   }
}

