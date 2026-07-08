package ru.pragmatix.wormix.scene.camera
{
   import flash.geom.Rectangle;
   import starling.utils.MathUtil;
   import starling.utils.Pool;
   
   public class CameraRect
   {
      
      private var _rect:Rectangle;
      
      private var _scale:Number;
      
      private var _scaleMin:Number;
      
      private var _scaleMax:Number;
      
      private var _sceneWidth:Number;
      
      private var _sceneHeight:Number;
      
      public function CameraRect(param1:Number, param2:Number, param3:Number, param4:Number, param5:Number = 1)
      {
         super();
         this._rect = Pool.getRectangle(0,0,param1,param2);
         this._sceneWidth = param3;
         this._sceneHeight = param4;
         this._scale = param5;
         this._scaleMax = Math.min(param3 / param1,param4 / param2);
         this._scaleMin = 0.5;
      }
      
      public function dispose() : void
      {
         Pool.putRectangle(this._rect);
         this._rect = null;
      }
      
      public function get scale() : Number
      {
         return this._scale;
      }
      
      public function set scale(param1:Number) : void
      {
         var _loc2_:Number = NaN;
         var _loc3_:Number = NaN;
         var _loc4_:Number = NaN;
         param1 = MathUtil.clamp(param1,this._scaleMin,this._scaleMax);
         if(param1 != this._scale)
         {
            _loc2_ = param1 / this._scale;
            _loc3_ = this._rect.width * (1 - _loc2_);
            _loc4_ = this._rect.height * (1 - _loc2_);
            this._scale = param1;
            this._rect.width -= _loc3_;
            this._rect.height -= _loc4_;
            this.x += _loc3_ * 0.5;
            this.y += _loc4_ * 0.5;
         }
      }
      
      public function get x() : Number
      {
         return this._rect.x;
      }
      
      public function set x(param1:Number) : void
      {
         this._rect.x = MathUtil.clamp(param1,0,this._sceneWidth - this._rect.width);
      }
      
      public function get y() : Number
      {
         return this._rect.y;
      }
      
      public function set y(param1:Number) : void
      {
         this._rect.y = MathUtil.clamp(param1,0,this._sceneHeight - this._rect.height);
      }
      
      public function get width() : Number
      {
         return this._rect.width;
      }
      
      public function get height() : Number
      {
         return this._rect.height;
      }
      
      public function get rect() : Rectangle
      {
         return this._rect;
      }
      
      public function get scaleMin() : Number
      {
         return this._scaleMin;
      }
      
      public function get scaleMax() : Number
      {
         return this._scaleMax;
      }
      
      public function get centerX() : Number
      {
         return this._rect.x + this._rect.width * 0.5;
      }
      
      public function get centerY() : Number
      {
         return this._rect.y + this._rect.height * 0.5;
      }
   }
}

