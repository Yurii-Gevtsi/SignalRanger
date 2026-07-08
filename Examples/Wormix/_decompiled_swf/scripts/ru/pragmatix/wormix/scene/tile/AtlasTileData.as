package ru.pragmatix.wormix.scene.tile
{
   import flash.geom.Rectangle;
   
   public class AtlasTileData
   {
      
      public var type:TileType;
      
      public var x:int;
      
      public var y:int;
      
      public var sourceRect:Rectangle;
      
      public var atlasIndex:int;
      
      public function AtlasTileData()
      {
         super();
      }
      
      public static function create(param1:TileData, param2:Rectangle, param3:int) : AtlasTileData
      {
         var _loc4_:AtlasTileData = new AtlasTileData();
         _loc4_.type = param1.type;
         _loc4_.x = param1.x;
         _loc4_.y = param1.y;
         _loc4_.sourceRect = param2;
         _loc4_.atlasIndex = param3;
         return _loc4_;
      }
   }
}

