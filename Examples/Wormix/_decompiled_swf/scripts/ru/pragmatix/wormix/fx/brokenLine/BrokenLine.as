package ru.pragmatix.wormix.fx.brokenLine
{
   import §_-G16§.§_-33h§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.display.Sprite;
   import starling.utils.Align;
   import starling.utils.Pool;
   
   public class BrokenLine extends Sprite
   {
      
      private static const brokenLinePartsPool:§_-33h§ = new §_-33h§(BrokenLinePart);
      
      private var thickness:Number;
      
      private var color:uint;
      
      private var lastPosition:Point;
      
      private var newPosition:Point;
      
      private var lines:Vector.<BrokenLinePart>;
      
      private var numLines:int;
      
      private var currentLineIndex:int = -1;
      
      public function BrokenLine(param1:Number, param2:uint)
      {
         super();
         this.thickness = param1;
         this.color = param2;
         this.lastPosition = Pool.getPoint();
         this.newPosition = Pool.getPoint();
         this.lines = new Vector.<BrokenLinePart>();
      }
      
      public function setColor(param1:uint) : void
      {
         this.color = param1;
      }
      
      public function beginDraw() : void
      {
         if(this.isDrawing)
         {
            this.endDraw();
         }
         this.currentLineIndex = 0;
      }
      
      public function endDraw() : void
      {
         if(this.isDrawing)
         {
            this.backToPool(this.currentLineIndex);
         }
         this.currentLineIndex = -1;
      }
      
      private function get isDrawing() : Boolean
      {
         return this.currentLineIndex >= 0;
      }
      
      private function backToPool(param1:int = 0) : void
      {
         var _loc3_:BrokenLinePart = null;
         if(param1 >= this.numLines)
         {
            return;
         }
         var _loc2_:int = param1;
         while(_loc2_ < this.numLines)
         {
            _loc3_ = this.lines[_loc2_];
            if(_loc3_)
            {
               removeChild(_loc3_);
               brokenLinePartsPool.put(_loc3_);
            }
            _loc2_++;
         }
         this.lines.splice(param1,this.numLines - param1);
         this.numLines = param1;
      }
      
      public function moveTo(param1:Number, param2:Number) : void
      {
         this.lastPosition.setTo(param1,param2);
      }
      
      public function lineTo(param1:Number, param2:Number) : void
      {
         this.newPosition.setTo(param1,param2);
         this.drawLine(this.lastPosition,this.newPosition);
         this.lastPosition.setTo(param1,param2);
      }
      
      public function drawLine(param1:Point, param2:Point) : void
      {
         var _loc9_:Number = NaN;
         var _loc10_:Number = NaN;
         var _loc11_:Boolean = false;
         var _loc12_:BrokenLinePart = null;
         visible = true;
         var _loc3_:Boolean = this.isDrawing;
         var _loc4_:Number = param1.x;
         var _loc5_:Number = param1.y;
         var _loc6_:Number = param2.x;
         var _loc7_:Number = param2.y;
         var _loc8_:Number = MathUtil.distance(_loc4_,_loc5_,_loc6_,_loc7_);
         if(_loc8_ > 0)
         {
            _loc9_ = MathUtil.§_-Ih§(_loc4_ + _loc8_,_loc5_,_loc6_,_loc7_);
            _loc10_ = Math.acos(MathUtil.§_-T1l§(1 - _loc9_ / (2 * _loc8_ * _loc8_),100000));
            _loc11_ = _loc3_ && this.currentLineIndex < this.numLines;
            _loc12_ = _loc11_ ? this.lines[this.currentLineIndex] : brokenLinePartsPool.get() as BrokenLinePart;
            ++this.currentLineIndex;
            _loc12_.rotation = 0;
            _loc12_.init(_loc8_,this.thickness,this.color);
            _loc12_.alignPivot(Align.LEFT,Align.CENTER);
            if(_loc4_ < _loc6_)
            {
               _loc12_.rotation = _loc5_ < _loc7_ ? _loc10_ : -_loc10_;
            }
            else
            {
               _loc12_.rotation = _loc5_ < _loc7_ ? _loc10_ : MathUtil.§_-M2N§ - _loc10_;
            }
            _loc12_.x = _loc4_;
            _loc12_.y = _loc5_;
            if(!_loc11_)
            {
               addChild(_loc12_);
               this.numLines = this.lines.push(_loc12_);
            }
         }
      }
      
      public function clear() : void
      {
         if(this.lines)
         {
            this.currentLineIndex = -1;
            this.backToPool();
         }
      }
      
      override public function dispose() : void
      {
         this.clear();
         this.lines = null;
         super.dispose();
         Pool.putPoint(this.lastPosition);
         Pool.putPoint(this.newPosition);
         this.lastPosition = null;
         this.newPosition = null;
      }
   }
}

