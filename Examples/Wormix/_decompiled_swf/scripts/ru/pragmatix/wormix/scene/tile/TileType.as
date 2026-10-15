package ru.pragmatix.wormix.scene.tile
{
   public class TileType
   {
      
      public static const BACKGROUND:TileType = create("background");
      
      public static const GROUND_BACK:TileType = create("ground_back");
      
      public static const GROUND:TileType = create("ground");
      
      public static const CLOUDS:TileType = create("clouds");
      
      public var name:String;
      
      public function TileType()
      {
         super();
      }
      
      public static function create(param1:String) : TileType
      {
         var _loc2_:TileType = new TileType();
         _loc2_.name = param1;
         return _loc2_;
      }
   }
}

