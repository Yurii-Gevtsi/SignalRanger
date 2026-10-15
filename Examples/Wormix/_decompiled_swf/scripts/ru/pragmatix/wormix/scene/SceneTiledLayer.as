package ru.pragmatix.wormix.scene
{
   import flash.geom.Rectangle;
   import starling.extensions.QuadtreeSprite;
   import starling.utils.Pool;
   
   public class SceneTiledLayer extends QuadtreeSprite
   {
      
      private var tileWidth:int;
      
      private var tileHeight:int;
      
      private var viewportCorner_0:int;
      
      private var viewportCorner_1:int;
      
      private var correctedViewport:Rectangle;
      
      public function SceneTiledLayer(param1:int, param2:int, param3:int, param4:int, param5:Number = 1)
      {
         var _loc6_:Rectangle = Pool.getRectangle(0,0,param1,param2);
         super(_loc6_);
         Pool.putRectangle(_loc6_);
         this.tileWidth = param3;
         this.tileHeight = param4;
         super.scale = param5;
         this.viewportCorner_0 = this.viewportCorner_1 = 0;
         this.correctedViewport = Pool.getRectangle();
      }
      
      override public function dispose() : void
      {
         super.dispose();
         Pool.putRectangle(this.correctedViewport);
         this.correctedViewport = null;
      }
      
      public function update(param1:Rectangle) : void
      {
         this.correctedViewport.copyFrom(param1);
         this.correctedViewport.x -= x;
         this.correctedViewport.y -= y;
         if(this.isViewportPosChanged(this.correctedViewport))
         {
            visibleViewport = this.correctedViewport;
         }
      }
      
      private function isViewportPosChanged(param1:Rectangle) : Boolean
      {
         var _loc2_:int = param1.left % this.tileWidth * 100 + param1.top % this.tileHeight;
         var _loc3_:int = param1.right % this.tileWidth * 100 + param1.bottom % this.tileHeight;
         if(_loc2_ != this.viewportCorner_0 || _loc3_ != this.viewportCorner_1)
         {
            this.viewportCorner_0 = _loc2_;
            this.viewportCorner_1 = _loc3_;
            return true;
         }
         return false;
      }
   }
}

