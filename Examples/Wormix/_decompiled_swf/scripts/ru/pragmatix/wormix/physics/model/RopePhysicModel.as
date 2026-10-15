package ru.pragmatix.wormix.physics.model
{
   import §_-r1C§.§_-h2B§;
   import §_-sQ§.§_-H3D§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.brokenLine.BrokenLine;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.physics.*;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.utils.MathUtil;
   
   public class RopePhysicModel extends GenericPhysicModel
   {
      
      public static const MIN_LENGTH:uint = 10;
      
      public static const MAX_LENGTH:uint = 380;
      
      private static const PIECE:int = 5;
      
      private var joints:Array = [];
      
      private var ropeLine:BrokenLine;
      
      private var lastJoint:uint = 0;
      
      private var length:uint;
      
      private var maxLength:uint;
      
      public var deltaLength:int = 0;
      
      public function RopePhysicModel(param1:PhysParams)
      {
         super(param1);
         keepUnstable = true;
         this.maxLength = MAX_LENGTH;
      }
      
      private static function normalizeAngle(param1:Number) : Number
      {
         if(param1 < -Math.PI)
         {
            return param1 + MathUtil.§_-M2N§;
         }
         if(param1 > Math.PI)
         {
            return param1 - MathUtil.§_-M2N§;
         }
         return param1;
      }
      
      override public function clear() : void
      {
         if(this.ropeLine != null)
         {
            §_-X1j§.§_-12n§.scene.removeElement(this.ropeLine);
            this.ropeLine.dispose();
            this.ropeLine = null;
         }
         this.deltaLength = 0;
         this.joints = [];
         this.lastJoint = 0;
      }
      
      override public function nextState() : void
      {
         var _loc5_:Number = NaN;
         var _loc6_:Number = NaN;
         var _loc8_:int = 0;
         var _loc9_:int = 0;
         var _loc10_:Number = NaN;
         var _loc11_:int = 0;
         var _loc12_:Number = NaN;
         var _loc13_:uint = 0;
         var _loc14_:Boolean = false;
         var _loc15_:uint = 0;
         var _loc16_:Number = NaN;
         var _loc17_:Number = NaN;
         var _loc18_:uint = 0;
         var _loc19_:Number = NaN;
         var _loc20_:Number = NaN;
         var _loc21_:int = 0;
         var _loc22_:int = 0;
         var _loc23_:uint = 0;
         var _loc24_:Force = null;
         var _loc25_:Number = NaN;
         var _loc26_:Number = NaN;
         var _loc27_:int = 0;
         var _loc28_:Number = NaN;
         checkBounds();
         if(physParams.disposed)
         {
            return;
         }
         physParams.epsilonSpeedCheck();
         var _loc1_:Joint = this.joints[this.lastJoint];
         if(_loc1_ == null)
         {
            return;
         }
         var _loc2_:Number = Math.atan2(physParams.py - _loc1_.y,physParams.px - _loc1_.x);
         if(this.deltaLength != 0 && (this.deltaLength < 0 && this.length + this.deltaLength >= MIN_LENGTH || this.deltaLength > 0 && this.totalLength() + this.deltaLength <= this.maxLength))
         {
            _loc8_ = Math.floor(physParams.px + Math.cos(_loc2_) * this.deltaLength);
            _loc9_ = Math.floor(physParams.py + Math.sin(_loc2_) * this.deltaLength);
            if(!HitTest.unit(_loc8_,_loc9_))
            {
               physParams.px = _loc8_;
               physParams.py = _loc9_;
               physParams._x = _loc8_;
               physParams._y = _loc9_;
               this.length += this.deltaLength;
            }
         }
         var _loc3_:Number = physParams.px;
         var _loc4_:Number = physParams.py;
         var _loc7_:uint = this.work();
         if(_loc7_ != 0)
         {
            _loc10_ = Math.atan2(y - _loc1_.y,x - _loc1_.x);
            if(Math.abs(_loc10_ - _loc2_) > Math.PI)
            {
               if(_loc2_ < 0)
               {
                  _loc10_ -= MathUtil.§_-M2N§;
               }
               else
               {
                  _loc10_ += MathUtil.§_-M2N§;
               }
            }
            _loc11_ = §_-H3D§.sign(_loc10_ - _loc2_);
            _loc12_ = (_loc10_ - _loc2_) / _loc7_;
            _loc13_ = 0;
            _loc14_ = false;
            _loc15_ = 0;
            while(_loc15_ < _loc7_)
            {
               _loc1_ = this.joints[this.lastJoint];
               _loc2_ += _loc12_;
               _loc16_ = PIECE * Math.cos(_loc2_);
               _loc17_ = PIECE * Math.sin(_loc2_);
               _loc18_ = Math.max(0,Math.floor(this.length / PIECE));
               _loc19_ = _loc1_.x + _loc16_ * _loc18_;
               _loc20_ = _loc1_.y + _loc17_ * _loc18_;
               if(HitTest.unit(Math.floor(_loc19_),Math.floor(_loc20_)))
               {
                  for each(_loc24_ in physParams.forces)
                  {
                     physParams.dx -= _loc24_.ax;
                     physParams.dy -= _loc24_.ay;
                  }
                  physParams.dx *= -0.9;
                  physParams.dy *= -0.5;
                  _loc25_ = this.totalLength();
                  if(_loc25_ < 100)
                  {
                     physParams.dx *= 100 / _loc25_;
                     physParams.dy *= 100 / _loc25_;
                  }
                  §_-h2B§.play(§_-d27§.§_-G2Y§);
                  break;
               }
               _loc3_ = _loc19_;
               _loc4_ = _loc20_;
               _loc5_ = _loc19_;
               _loc6_ = _loc20_;
               _loc23_ = 0;
               while(_loc23_ < _loc18_ - 1)
               {
                  _loc19_ -= _loc16_;
                  _loc20_ -= _loc17_;
                  if(§_-X1j§.§_-12n§.scene.hitTestPoint(_loc19_,_loc20_))
                  {
                     _loc2_ -= _loc12_;
                     _loc19_ = _loc1_.x + PIECE * Math.cos(_loc2_) * (_loc18_ - _loc23_);
                     _loc20_ = _loc1_.y + PIECE * Math.sin(_loc2_) * (_loc18_ - _loc23_);
                     _loc1_ = this.joints[this.lastJoint];
                     ++this.lastJoint;
                     _loc21_ = Math.floor(_loc19_);
                     _loc22_ = Math.floor(_loc20_);
                     _loc26_ = §_-H3D§.distance(_loc21_ - _loc1_.x,_loc22_ - _loc1_.y);
                     _loc1_ = this.joints[this.lastJoint] = new Joint(_loc21_,_loc22_,_loc11_,normalizeAngle(_loc2_),_loc26_);
                     this.length -= _loc26_;
                     _loc3_ = _loc1_.x + Math.cos(_loc2_) * this.length;
                     _loc4_ = _loc1_.y + Math.sin(_loc2_) * this.length;
                     _loc14_ = true;
                     break;
                  }
                  _loc23_++;
               }
               if(!_loc14_ && _loc11_ != this.joints[this.lastJoint].dir)
               {
                  _loc27_ = this.lastJoint - 1;
                  _loc28_ = NaN;
                  while(_loc27_ >= 0)
                  {
                     _loc1_ = this.joints[this.lastJoint];
                     if(this.isBetween(_loc1_.angle,_loc2_,_loc2_ - _loc12_,0.0001))
                     {
                        _loc28_ = _loc1_.angle;
                        this.length = Math.min(this.length + _loc1_.length,this.maxLength);
                        this.lastJoint = _loc27_;
                     }
                     else
                     {
                        _loc27_ = 0;
                     }
                     _loc27_--;
                  }
                  if(!isNaN(_loc28_))
                  {
                     _loc1_ = this.joints[this.lastJoint];
                     _loc3_ = _loc1_.x + this.length * Math.cos(_loc28_);
                     _loc4_ = _loc1_.y + this.length * Math.sin(_loc28_);
                     _loc14_ = true;
                  }
               }
               _loc13_++;
               if(_loc14_)
               {
                  break;
               }
               _loc15_++;
            }
         }
         physParams.px = _loc3_;
         physParams.py = _loc4_;
         physParams._x = Math.floor(_loc3_);
         physParams._y = Math.floor(_loc4_);
         this.renderRope();
      }
      
      public function setLength(param1:Number) : void
      {
         this.maxLength = param1;
      }
      
      public function reset(param1:Point) : void
      {
         this.joints = [new Joint(param1.x,param1.y,0,0,0)];
         this.lastJoint = 0;
         this.length = §_-H3D§.distance(x - param1.x,y - param1.y);
      }
      
      private function renderRope() : void
      {
         if(!this.ropeLine)
         {
            this.ropeLine = new BrokenLine(4,16448250);
            §_-X1j§.§_-12n§.scene.addElementToCertainSortingLayer(this.ropeLine,SortingLayer.AMMO_LAYER);
         }
         this.ropeLine.clear();
         var _loc1_:Joint = this.joints[0];
         this.ropeLine.beginDraw();
         this.ropeLine.moveTo(_loc1_.x,_loc1_.y);
         var _loc2_:uint = 0;
         while(_loc2_ < this.lastJoint)
         {
            _loc1_ = this.joints[_loc2_ + 1];
            this.ropeLine.lineTo(_loc1_.x,_loc1_.y);
            _loc2_++;
         }
         _loc1_ = this.joints[_loc2_];
         var _loc3_:Number = Math.atan2(_loc1_.y - y,_loc1_.x - x);
         var _loc4_:int = x + Math.cos(_loc3_) * 15;
         var _loc5_:int = y + Math.sin(_loc3_) * 15;
         this.ropeLine.lineTo(_loc4_,_loc5_);
         this.ropeLine.endDraw();
      }
      
      private function work() : uint
      {
         var _loc9_:Force = null;
         var _loc10_:int = 0;
         var _loc11_:int = 0;
         var _loc1_:Joint = this.joints[this.lastJoint];
         var _loc2_:Number = x - _loc1_.x + physParams.dx;
         var _loc3_:Number = y - _loc1_.y + physParams.dy;
         var _loc4_:Number = this.length / §_-H3D§.distance(_loc2_,_loc3_);
         var _loc5_:Number = _loc1_.x + _loc2_ * _loc4_;
         var _loc6_:Number = _loc1_.y + _loc3_ * _loc4_;
         var _loc7_:Number = _loc5_ - x;
         var _loc8_:Number = _loc6_ - y;
         for each(_loc9_ in physParams.forces)
         {
            _loc7_ += _loc9_.ax;
            _loc8_ += _loc9_.ay;
         }
         physParams.dx = MathUtil.§_-F1V§(_loc7_);
         physParams.dy = MathUtil.§_-F1V§(_loc8_);
         _loc10_ = Math.abs(_loc5_ - x);
         _loc11_ = Math.abs(_loc6_ - y);
         physParams.px = MathUtil.§_-F1V§(_loc5_);
         physParams.py = MathUtil.§_-F1V§(_loc6_);
         physParams._x = Math.floor(_loc5_);
         physParams._y = Math.floor(_loc6_);
         return Math.max(_loc10_,_loc11_);
      }
      
      private function isBetween(param1:Number, param2:Number, param3:Number, param4:Number) : Boolean
      {
         var _loc5_:Number = NaN;
         var _loc6_:Number = NaN;
         var _loc7_:Number = NaN;
         if(Math.abs(param3 - param2) < param4)
         {
            return Math.abs(param1 - param3) < param4 * 2;
         }
         _loc5_ = Math.min(param2,param3);
         _loc6_ = Math.max(param2,param3);
         _loc7_ = _loc5_;
         while(_loc7_ < _loc6_)
         {
            if(Math.abs(normalizeAngle(_loc7_) - normalizeAngle(param1)) < param4)
            {
               return true;
            }
            _loc7_ += param4;
         }
         return false;
      }
      
      private function testJoint(param1:Joint) : Boolean
      {
         var _loc7_:int = 0;
         var _loc2_:int = Math.max(Math.abs(param1.x - x),Math.abs(param1.y - y));
         var _loc3_:Number = param1.x;
         var _loc4_:Number = param1.y;
         var _loc5_:Number = physParams.dx;
         var _loc6_:Number = physParams.dy;
         if(_loc2_ > 0)
         {
            _loc7_ = 0;
            while(_loc7_ < _loc2_)
            {
               if(§_-X1j§.§_-12n§.scene.hitTestPoint(_loc3_,_loc4_))
               {
                  return true;
               }
               _loc3_ += _loc5_;
               _loc4_ += _loc6_;
               _loc7_++;
            }
         }
         return false;
      }
      
      public function totalLength() : Number
      {
         var _loc2_:int = 0;
         var _loc1_:Number = this.length;
         if(this.lastJoint != 0)
         {
            _loc2_ = 1;
            while(_loc2_ <= this.lastJoint)
            {
               _loc1_ += this.joints[_loc2_].length;
               _loc2_++;
            }
         }
         return _loc1_;
      }
      
      override public function get angle() : Number
      {
         var _loc1_:Joint = this.joints[this.lastJoint];
         return Math.atan2(y - _loc1_.y,x - _loc1_.x);
      }
      
      public function getJoints() : Array
      {
         return this.joints;
      }
   }
}

