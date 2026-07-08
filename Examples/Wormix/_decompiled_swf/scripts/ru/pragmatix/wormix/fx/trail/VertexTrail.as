package ru.pragmatix.wormix.fx.trail
{
   import flash.geom.Point;
   import flash.geom.Vector3D;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.*;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.animation.IAnimatable;
   import starling.core.Starling;
   import starling.display.Mesh;
   import starling.events.Event;
   import starling.rendering.IndexData;
   import starling.rendering.VertexData;
   import starling.styles.MeshStyle;
   import starling.utils.Pool;
   
   public class VertexTrail extends Mesh implements IPooledEffect, IAnimatable
   {
      
      private var _effectType:EffectType;
      
      private var target:§_-F3o§;
      
      private var _length:Number;
      
      private var _width:Number;
      
      private var _color:uint;
      
      private var yCoeff:Number;
      
      private var gColor:Number;
      
      private var _alpha:Number;
      
      private var oldX:Number;
      
      private var oldY:Number;
      
      private var points:Vector.<Vector3D>;
      
      public function VertexTrail(param1:EffectType)
      {
         this._effectType = param1;
         var _loc2_:VertexData = new VertexData(MeshStyle.VERTEX_FORMAT,5);
         var _loc3_:IndexData = new IndexData(15);
         super(_loc2_,_loc3_);
      }
      
      protected function setupVertices() : void
      {
         var _loc1_:VertexData = vertexData;
         var _loc2_:IndexData = indexData;
         var _loc3_:int = int(this.points.length);
         _loc1_.clear();
         _loc2_.clear();
         if(_loc3_ == 0)
         {
            this.remove();
         }
         _loc2_.numIndices = 0;
         _loc1_.numVertices = _loc3_ + (_loc3_ - 2) * 2;
         this.updateVertices();
         _loc2_.addTriangle(3,1,0);
         _loc2_.addTriangle(2,1,0);
         _loc2_.addTriangle(4,3,1);
         _loc2_.addTriangle(4,2,1);
         var _loc4_:int = 0;
         var _loc5_:int = 2;
         while(_loc5_ < _loc3_ - 1)
         {
            _loc4_ = 3 * _loc5_;
            _loc2_.addTriangle(_loc4_ - 1,_loc4_ - 2,_loc4_ - 4);
            _loc2_.addTriangle(_loc4_,_loc4_ - 2,_loc4_ - 3);
            _loc2_.addTriangle(_loc4_ + 1,_loc4_ - 1,_loc4_ - 2);
            _loc2_.addTriangle(_loc4_ + 1,_loc4_,_loc4_ - 2);
            _loc5_++;
         }
      }
      
      public function updateVertices() : void
      {
         var _loc6_:Vector3D = null;
         var _loc1_:VertexData = vertexData;
         var _loc2_:Point = Pool.getPoint();
         var _loc3_:int = int(this.points.length);
         var _loc4_:int = 0;
         if(_loc3_ == 0)
         {
            this.remove();
         }
         var _loc5_:int = 0;
         var _loc7_:Vector3D = this.points[_loc4_];
         _loc5_ = this.setPoint(_loc1_,_loc5_,this.points[_loc4_].x,this.points[_loc4_].y);
         _loc4_++;
         _loc5_ = this.setPoint(_loc1_,_loc5_,this.points[_loc4_].x,this.points[_loc4_].y);
         this.setSupportPoint(this.points[_loc4_],_loc4_,_loc3_,true,_loc2_);
         _loc5_ = this.setPoint(_loc1_,_loc5_,_loc2_.x,_loc2_.y);
         this.setSupportPoint(this.points[_loc4_],_loc4_,_loc3_,false,_loc2_);
         _loc5_ = this.setPoint(_loc1_,_loc5_,_loc2_.x,_loc2_.y);
         _loc4_++;
         _loc5_ = this.setPoint(_loc1_,_loc5_,this.points[_loc4_].x,this.points[_loc4_].y);
         while(_loc4_ < _loc3_ - 1)
         {
            _loc7_ = this.points[_loc4_];
            _loc6_ = this.points[_loc4_ + 1];
            this.setSupportPoint(_loc7_,_loc4_,_loc3_,true,_loc2_);
            _loc5_ = this.setPoint(_loc1_,_loc5_,_loc2_.x,_loc2_.y);
            this.setSupportPoint(_loc7_,_loc4_,_loc3_,false,_loc2_);
            _loc5_ = this.setPoint(_loc1_,_loc5_,_loc2_.x,_loc2_.y);
            _loc7_.y *= this.yCoeff;
            _loc5_ = this.setPoint(_loc1_,_loc5_,_loc6_.x,_loc6_.y);
            _loc4_++;
         }
         Pool.putPoint(_loc2_);
         setRequiresRedraw();
      }
      
      private function setSupportPoint(param1:Vector3D, param2:int, param3:int, param4:Boolean, param5:Point) : void
      {
         var _loc6_:Number = NaN;
         _loc6_ = this._width * param2 / param3;
         var _loc7_:Number = _loc6_ * Math.cos(param1.z);
         var _loc8_:Number = _loc6_ * Math.sin(param1.z);
         param5.setTo(param1.x + _loc7_ * (param4 ? -1 : 1),param1.y + _loc8_ * (param4 ? -1 : 1));
      }
      
      private function setPoint(param1:VertexData, param2:int, param3:Number, param4:Number) : int
      {
         param1.setPoint(param2,"position",param3,param4);
         param1.setPoint(param2,"texCoords",param3 + 0.5,param4 + 0.5);
         param1.colorize("color",this._color,this._alpha,param2,1);
         return param2 + 1;
      }
      
      public function attach(param1:§_-F3o§, param2:Array) : void
      {
         var _loc3_:Vector3D = null;
         Starling.current.addEventListener(Event.CONTEXT3D_CREATE,this.onContextCreated);
         this.target = param1;
         this._length = param2[0];
         this._width = param2[1];
         this._color = param2[2];
         this.yCoeff = param2[3];
         this.gColor = param2[4];
         this._alpha = alpha;
         if(this._length <= 0 || this._width <= 0)
         {
            return;
         }
         this.cleanPointsVector();
         var _loc4_:int = 0;
         while(_loc4_ < this._length)
         {
            _loc3_ = new Vector3D(this.target.x,this.target.y,0);
            this.points.push(_loc3_);
            _loc4_++;
         }
         this.oldX = this.target.x;
         this.oldY = this.target.y;
         Starling.juggler.add(this);
         visible = true;
         §_-X1j§.§_-12n§.scene.addElementToCertainSortingLayer(this,SortingLayer.UNIT_LAYER);
         addEventListener(Event.REMOVED_FROM_STAGE,this.onRemovedFromStage);
         this.setupVertices();
      }
      
      public function advanceTime(param1:Number) : void
      {
         var _loc2_:Vector3D = null;
         if(!this.target)
         {
            return;
         }
         if(this.target.disposed)
         {
            this.setDefault();
            this.remove();
         }
         else
         {
            _loc2_ = this.points.splice(0,1)[0];
            _loc2_.setTo(this.target.x,this.target.y,Math.atan2(this.target.y - this.oldY,this.target.x - this.oldX) + MathUtil.PI_HALF);
            this.points.push(_loc2_);
            this.updateVertices();
         }
      }
      
      public function init(param1:int, param2:int, param3:Array) : void
      {
      }
      
      public function remove() : void
      {
         vertexData.clear();
         indexData.clear();
         removeEventListener(Event.REMOVED_FROM_STAGE,this.onRemovedFromStage);
         Starling.current.removeEventListener(Event.CONTEXT3D_CREATE,this.onContextCreated);
         Starling.juggler.remove(this);
         this.removeFromParent(false);
         this.target = null;
         this.cleanPointsVector();
         this._effectType.getObjectPool().putObject(this,this._effectType);
         setRequiresRedraw();
      }
      
      private function cleanPointsVector() : void
      {
         var _loc1_:* = 0;
         if(this.points)
         {
            _loc1_ = int(this.points.length);
            while(_loc1_--)
            {
               Pool.putPoint3D(this.points.splice(_loc1_,1)[0]);
            }
         }
         else
         {
            this.points = new Vector.<Vector3D>();
         }
      }
      
      public function setDefault() : void
      {
         setRequiresRedraw();
      }
      
      private function onContextCreated(param1:Event) : void
      {
         if(Boolean(this.target) && Boolean(this.target.§_-Y2I§))
         {
            this.setupVertices();
         }
         else
         {
            this.remove();
         }
      }
      
      private function onRemovedFromStage(param1:Event) : void
      {
         this.remove();
      }
   }
}

