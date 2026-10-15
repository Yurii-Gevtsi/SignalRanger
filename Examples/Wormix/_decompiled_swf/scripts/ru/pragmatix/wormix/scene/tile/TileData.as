package ru.pragmatix.wormix.scene.tile
{
   import flash.display.BitmapData;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import starling.utils.Pool;
   
   public class TileData
   {
      
      public var bitmapData:BitmapData;
      
      public var type:TileType;
      
      public var tileBounds:Rectangle;
      
      public var bdBounds:Rectangle;
      
      public var isEmpty:Boolean;
      
      public function TileData()
      {
         super();
      }
      
      public static function create(param1:BitmapData, param2:TileType, param3:int, param4:int, param5:int, param6:int, param7:int = 0, param8:int = 0) : TileData
      {
         var _loc9_:TileData = new TileData();
         _loc9_.bitmapData = param1 != null ? param1 : new BitmapData(1,1,true,0);
         _loc9_.isEmpty = param1 == null;
         _loc9_.type = param2;
         _loc9_.tileBounds = Pool.getRectangle(param3,param4,param5,param6);
         _loc9_.bdBounds = Pool.getRectangle(param7,param8,_loc9_.bitmapData.width,_loc9_.bitmapData.height);
         return _loc9_;
      }
      
      public static function createWithBounds(param1:BitmapData, param2:TileType, param3:Rectangle, param4:Point = null) : TileData
      {
         return create(param1,param2,param3.x,param3.y,param3.width,param3.height,param4 ? int(param4.x) : 0,param4 ? int(param4.y) : 0);
      }
      
      public function dispose() : void
      {
         Pool.putRectangle(this.tileBounds);
         this.tileBounds = null;
         Pool.putRectangle(this.bdBounds);
         this.bdBounds = null;
         this.bitmapData.dispose();
         this.bitmapData = null;
         this.type = null;
      }
      
      public function get x() : int
      {
         return this.tileBounds.x;
      }
      
      public function get y() : int
      {
         return this.tileBounds.y;
      }
      
      public function get xWithOffset() : int
      {
         return this.tileBounds.x + this.bdBounds.x;
      }
      
      public function get yWithOffset() : int
      {
         return this.tileBounds.y + this.bdBounds.y;
      }
      
      public function clone() : TileData
      {
         var _loc1_:TileData = create(this.bitmapData,this.type,this.tileBounds.x,this.tileBounds.y,this.tileBounds.width,this.tileBounds.height);
         _loc1_.isEmpty = this.isEmpty;
         return _loc1_;
      }
   }
}

