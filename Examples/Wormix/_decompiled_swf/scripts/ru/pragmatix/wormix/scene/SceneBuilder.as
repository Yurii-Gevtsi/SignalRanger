package ru.pragmatix.wormix.scene
{
   import §_-dS§.§_-D3a§;
   import flash.display.BitmapData;
   import flash.display.DisplayObject;
   import flash.display.MovieClip;
   import flash.display.StageQuality;
   import flash.geom.Matrix;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import ru.pragmatix.wormix.scene.tile.GeneratedTilesData;
   import ru.pragmatix.wormix.scene.tile.TileData;
   import ru.pragmatix.wormix.scene.tile.TileType;
   import starling.utils.Pool;
   
   public class SceneBuilder
   {
      
      public function SceneBuilder()
      {
         super();
      }
      
      public static function getMapDimensions(param1:MovieClip, param2:§_-D3a§) : Point
      {
         var _loc3_:int = param2.§_-s1§;
         var _loc4_:int = param2.indentHeight;
         var _loc5_:MovieClip = MovieClip(param1.getChildByName("bg"));
         var _loc6_:MovieClip = MovieClip(_loc5_.getChildByName("bgMask"));
         var _loc7_:Number = _loc6_ == null ? _loc5_.width : _loc6_.width * _loc5_.scaleX;
         var _loc8_:Number = _loc6_ == null ? _loc5_.height : _loc6_.height * _loc5_.scaleY;
         var _loc9_:Number = Math.floor(_loc3_ + _loc7_);
         var _loc10_:Number = Math.floor(_loc4_ + _loc8_);
         return Pool.getPoint(_loc9_,_loc10_);
      }
      
      public static function createBackgroundBitmapData(param1:MovieClip, param2:§_-D3a§, param3:Number) : BitmapData
      {
         var _loc4_:Point = Pool.getPoint();
         var _loc5_:int = param2.§_-s1§ / param3;
         var _loc6_:int = param2.indentHeight / param3;
         var _loc7_:MovieClip = MovieClip(param1.getChildByName("bg"));
         var _loc8_:MovieClip = MovieClip(_loc7_.getChildByName("bgMask"));
         _loc7_.scaleX /= param3;
         _loc7_.scaleY /= param3;
         var _loc9_:Number = _loc8_ == null ? _loc7_.width : _loc8_.width * _loc7_.scaleX;
         var _loc10_:Number = _loc8_ == null ? _loc7_.height : _loc8_.height * _loc7_.scaleY;
         var _loc11_:BitmapData = new BitmapData(_loc9_ + _loc5_,_loc10_ + _loc6_,true,0);
         var _loc12_:Matrix = _loc7_.transform.matrix;
         _loc12_.scale((_loc9_ + _loc5_) / _loc9_,(_loc10_ + _loc6_) / _loc10_);
         _loc11_.drawWithQuality(_loc7_,_loc12_,null,null,null,true,StageQuality.BEST);
         var _loc13_:Rectangle = _loc11_.getColorBoundsRect(16777215,0,false);
         var _loc14_:BitmapData = new BitmapData(_loc13_.width,_loc13_.height,true,0);
         _loc14_.copyPixels(_loc11_,_loc13_,_loc4_);
         Pool.putPoint(_loc4_);
         return _loc14_;
      }
      
      public static function createBackLayerBitmapDataWithOffset(param1:MovieClip, param2:§_-D3a§, param3:Point) : BitmapData
      {
         var _loc6_:BitmapData = null;
         var _loc7_:Rectangle = null;
         var _loc8_:Matrix = null;
         var _loc9_:BitmapData = null;
         var _loc4_:MovieClip = param1.getChildByName("ground_back") as MovieClip;
         var _loc5_:Point = Pool.getPoint();
         if(_loc4_)
         {
            _loc7_ = _loc4_.getBounds(param1);
            _loc8_ = _loc4_.transform.matrix;
            param3.setTo(_loc7_.x,_loc7_.y);
            _loc8_.translate(-_loc7_.x,-_loc7_.y);
            _loc9_ = new BitmapData(_loc7_.width,_loc7_.height,true,0);
            _loc9_.drawWithQuality(_loc4_,_loc8_,null,null,null,true,StageQuality.BEST);
            _loc7_ = _loc9_.getColorBoundsRect(16777215,0,false);
            _loc6_ = new BitmapData(_loc7_.width,_loc7_.height,true,0);
            _loc6_.copyPixels(_loc9_,_loc7_,_loc5_);
            param3.offset(_loc7_.x + param2.§_-s1§ * 0.5,_loc7_.y + param2.indentHeight);
         }
         Pool.putPoint(_loc5_);
         return _loc6_;
      }
      
      public static function createCloudsLayerBitmapDataWithOffset(param1:MovieClip, param2:§_-D3a§, param3:Point) : BitmapData
      {
         var _loc6_:BitmapData = null;
         var _loc7_:Rectangle = null;
         var _loc8_:Matrix = null;
         var _loc9_:BitmapData = null;
         var _loc4_:MovieClip = param1.getChildByName("clouds") as MovieClip;
         var _loc5_:Point = Pool.getPoint();
         if(_loc4_)
         {
            _loc7_ = _loc4_.getBounds(param1);
            if(_loc7_.isEmpty())
            {
               return null;
            }
            _loc8_ = _loc4_.transform.matrix;
            param3.setTo(_loc7_.x,_loc7_.y);
            _loc8_.translate(-_loc7_.x,-_loc7_.y);
            _loc9_ = new BitmapData(_loc7_.width,_loc7_.height,true,0);
            _loc9_.drawWithQuality(_loc4_,_loc8_,null,null,null,true,StageQuality.BEST);
            _loc7_ = _loc9_.getColorBoundsRect(16777215,0,false);
            _loc6_ = new BitmapData(_loc7_.width,_loc7_.height,true,0);
            _loc6_.copyPixels(_loc9_,_loc7_,_loc5_);
            param3.offset(_loc7_.x + param2.§_-s1§ * 0.5,_loc7_.y + param2.indentHeight);
         }
         Pool.putPoint(_loc5_);
         return _loc6_;
      }
      
      public static function generateTileDataArray(param1:BitmapData, param2:TileType, param3:int, param4:int, param5:Array = null) : Array
      {
         var _loc13_:Rectangle = null;
         var _loc16_:int = 0;
         var _loc17_:int = 0;
         var _loc19_:uint = 0;
         var _loc6_:uint = Math.ceil(param1.height / param3);
         var _loc7_:uint = Math.ceil(param1.width / param4);
         var _loc8_:uint = 16777215;
         var _loc9_:uint = 0;
         var _loc10_:int = param1.width - _loc7_ * param3;
         if(_loc10_ <= 0)
         {
            _loc10_ = param3;
         }
         var _loc11_:int = param1.height - _loc6_ * param4;
         if(_loc11_ <= 0)
         {
            _loc11_ = param4;
         }
         var _loc12_:BitmapData = new BitmapData(param3,param4,true,0);
         var _loc14_:Point = Pool.getPoint();
         var _loc15_:Rectangle = Pool.getRectangle();
         if(param5 == null)
         {
            param5 = [];
         }
         else
         {
            param5.splice(0);
         }
         var _loc18_:uint = 0;
         while(_loc18_ <= _loc6_ - 1)
         {
            _loc19_ = 0;
            while(_loc19_ <= _loc7_ - 1)
            {
               _loc16_ = _loc19_ == _loc7_ ? _loc10_ : param3;
               _loc17_ = _loc18_ == _loc6_ ? _loc11_ : param4;
               _loc15_.setTo(_loc19_ * param3,_loc18_ * param4,_loc16_,_loc17_);
               _loc12_.fillRect(_loc12_.rect,0);
               _loc12_.copyPixels(param1,_loc15_,_loc14_);
               _loc13_ = _loc12_.getColorBoundsRect(_loc8_,_loc9_,false);
               if(!_loc13_.isEmpty())
               {
                  param5.push(TileData.createWithBounds(_loc12_.clone(),param2,_loc15_));
               }
               _loc19_++;
            }
            _loc18_++;
         }
         Pool.putRectangle(_loc15_);
         Pool.putPoint(_loc14_);
         return param5;
      }
      
      public static function generateGroundTileDataArray(param1:DisplayObject, param2:int, param3:int, param4:§_-D3a§, param5:int, param6:int) : GeneratedTilesData
      {
         var _loc16_:int = 0;
         var _loc17_:int = 0;
         var _loc18_:Rectangle = null;
         var _loc23_:BitmapData = null;
         var _loc25_:uint = 0;
         var _loc7_:GeneratedTilesData = new GeneratedTilesData();
         var _loc8_:Rectangle = param1.getBounds(param1);
         if(_loc8_.isEmpty())
         {
            _loc8_.setTo(0,0,param2,param3);
         }
         var _loc9_:Matrix = new Matrix(1,0,0,1,-_loc8_.x,-_loc8_.y);
         var _loc10_:BitmapData = new BitmapData(_loc8_.width,_loc8_.height,true,0);
         _loc10_.draw(param1,_loc9_,null,null,null);
         _loc8_ = param1.getBounds(param1.parent);
         var _loc11_:int = Math.floor(_loc8_.x + param4.§_-s1§ * 0.5);
         var _loc12_:int = Math.floor(_loc8_.y + param4.indentHeight);
         _loc7_.numVerticalTiles = Math.ceil(param3 / param5) + 1;
         _loc7_.numHorizontalTiles = Math.ceil(param2 / param6);
         var _loc13_:uint = 16777215;
         var _loc14_:uint = 0;
         var _loc15_:int = param2 - _loc7_.numHorizontalTiles * param5;
         if(_loc15_ <= 0)
         {
            _loc15_ = param5;
         }
         var _loc19_:Point = Pool.getPoint();
         var _loc20_:Point = Pool.getPoint();
         var _loc21_:Rectangle = Pool.getRectangle();
         var _loc22_:BitmapData = new BitmapData(param5,param6,true,0);
         var _loc24_:uint = 0;
         while(_loc24_ <= _loc7_.numVerticalTiles - 1)
         {
            _loc25_ = 0;
            while(_loc25_ <= _loc7_.numHorizontalTiles - 1)
            {
               _loc16_ = _loc25_ == _loc7_.numHorizontalTiles ? _loc15_ : param5;
               _loc17_ = param6;
               _loc21_.setTo(_loc25_ * param5 - _loc11_,_loc24_ * param6 - _loc12_,_loc16_,_loc17_);
               _loc22_.fillRect(_loc22_.rect,0);
               _loc22_.copyPixels(_loc10_,_loc21_,_loc19_);
               _loc18_ = _loc22_.getColorBoundsRect(_loc13_,_loc14_,false);
               _loc21_.x += _loc11_;
               _loc21_.y += _loc12_;
               _loc20_.setTo(0,0);
               if(_loc18_.isEmpty())
               {
                  _loc23_ = null;
               }
               else if(!_loc18_.equals(_loc22_.rect))
               {
                  _loc23_ = new BitmapData(_loc18_.width,_loc18_.height,true,0);
                  _loc23_.copyPixels(_loc22_,_loc18_,_loc19_);
                  _loc20_.copyFrom(_loc18_.topLeft);
               }
               else
               {
                  _loc23_ = _loc22_.clone();
               }
               _loc7_.tileDataArray.push(TileData.createWithBounds(_loc23_,TileType.GROUND,_loc21_,_loc20_));
               _loc25_++;
            }
            _loc24_++;
         }
         _loc22_.dispose();
         Pool.putRectangle(_loc21_);
         Pool.putPoint(_loc19_);
         Pool.putPoint(_loc20_);
         return _loc7_;
      }
   }
}

