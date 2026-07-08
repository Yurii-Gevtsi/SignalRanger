package ru.pragmatix.wormix.scene.tile
{
   import flash.display.BitmapData;
   import flash.display.BlendMode;
   import flash.display.DisplayObject;
   import flash.display.StageQuality;
   import flash.display3D.Context3DTextureFormat;
   import flash.geom.Matrix;
   import flash.geom.Rectangle;
   import starling.display.Image;
   import starling.textures.RenderTexture;
   import starling.textures.Texture;
   import starling.textures.TextureSmoothing;
   
   public class GroundTile
   {
      
      private static var objectToBundleDraw:Image;
      
      private static const imageSmoothing:String = TextureSmoothing.BILINEAR;
      
      private static const textureFormat:String = Context3DTextureFormat.BGRA_PACKED;
      
      private static var transformMatrix:Matrix = new Matrix();
      
      public var image:Image;
      
      private var texture:Texture;
      
      private var renderTexture:RenderTexture;
      
      private var data:TileData;
      
      private var bundledDrawArray:Array = new Array();
      
      private var bitmapDataRecreated:Boolean = false;
      
      public function GroundTile(param1:TileData)
      {
         super();
         this.data = param1;
         this.texture = Texture.fromBitmapData(this.data.bitmapData,false,false,1,textureFormat);
         this.image = new Image(this.texture);
         this.image.textureSmoothing = imageSmoothing;
         this.image.x = this.data.xWithOffset;
         this.image.y = this.data.yWithOffset;
      }
      
      public function dispose() : void
      {
         this.data.dispose();
         this.image.dispose();
         this.texture.dispose();
         this.disposeRenderTexture();
      }
      
      public function refresh() : void
      {
         if(this.data.bitmapData == null)
         {
            return;
         }
         if(this.bitmapDataRecreated)
         {
            this.texture = Texture.fromBitmapData(this.data.bitmapData,false,false,1,textureFormat);
            this.bitmapDataRecreated = false;
         }
         else
         {
            this.texture.root.uploadBitmapData(this.data.bitmapData);
         }
         this.changeTexture(this.texture);
         this.disposeRenderTexture();
         this.image.pivotX = this.image.pivotY = 0;
         this.image.x = this.data.xWithOffset;
         this.image.y = this.data.yWithOffset;
      }
      
      public function hitTest(param1:int, param2:int) : Boolean
      {
         var _loc3_:int = param1 - this.data.xWithOffset;
         var _loc4_:int = param2 - this.data.yWithOffset;
         return this.data.bitmapData.getPixel(_loc3_,_loc4_) != 0;
      }
      
      public function get x() : int
      {
         return this.data.x;
      }
      
      public function get y() : int
      {
         return this.data.y;
      }
      
      public function set y(param1:int) : void
      {
         this.data.tileBounds.y = param1;
         if(this.bitmapDataRecreated)
         {
            this.image.pivotY = this.data.bdBounds.y;
         }
         this.image.y = this.data.yWithOffset;
      }
      
      public function isRefreshed() : Boolean
      {
         return this.renderTexture == null;
      }
      
      public function drawObjectInBitmapData(param1:DisplayObject, param2:Rectangle, param3:Boolean = true) : void
      {
         var _loc4_:Rectangle = null;
         var _loc5_:Rectangle = null;
         var _loc6_:BitmapData = null;
         var _loc7_:BitmapData = null;
         if(!param3)
         {
            _loc4_ = this.data.tileBounds.intersection(param2);
            _loc4_.x = Math.floor(_loc4_.x - this.data.tileBounds.x);
            _loc4_.y = Math.floor(_loc4_.y - this.data.tileBounds.y);
            _loc5_ = this.data.isEmpty ? _loc4_ : this.data.bdBounds.union(_loc4_);
            if(!this.data.bdBounds.containsRect(_loc5_) && _loc5_.width != 0 && _loc5_.height != 0)
            {
               _loc5_.width = Math.ceil(_loc5_.width);
               _loc5_.height = Math.ceil(_loc5_.height);
               _loc6_ = this.data.bitmapData;
               _loc7_ = new BitmapData(_loc5_.width,_loc5_.height,true,0);
               _loc7_.copyPixels(_loc6_,_loc6_.rect,this.data.bdBounds.topLeft.subtract(_loc5_.topLeft));
               this.data.bitmapData = _loc7_;
               this.data.bdBounds = _loc5_;
               this.data.isEmpty = false;
               this.bitmapDataRecreated = true;
            }
         }
         transformMatrix.copyFrom(param1.transform.matrix);
         transformMatrix.translate(-this.data.xWithOffset,-this.data.yWithOffset);
         this.data.bitmapData.drawWithQuality(param1,transformMatrix,null,param3 ? BlendMode.ERASE : BlendMode.NORMAL,null,true,StageQuality.BEST);
      }
      
      public function drawObjectInImage(param1:Image, param2:Boolean = true) : void
      {
         var _loc3_:RenderTexture = this.getRenderTexture();
         param1.blendMode = param2 ? BlendMode.ERASE : BlendMode.NORMAL;
         this.drawToRenderTexture(param1);
         this.changeTexture(_loc3_);
      }
      
      public function saveForBundledDraw(param1:Image) : void
      {
         this.bundledDrawArray.push(BundledDrawData.create(param1.x,param1.y,param1.scaleX,param1.scaleY));
      }
      
      public function performBundledDraw(param1:Image) : void
      {
         objectToBundleDraw = param1;
         var _loc2_:RenderTexture = this.getRenderTexture();
         _loc2_.drawBundled(this.bundleDrawFunction);
         this.bundledDrawArray.splice(0);
         this.changeTexture(_loc2_);
      }
      
      private function bundleDrawFunction() : void
      {
         var _loc2_:BundledDrawData = null;
         var _loc1_:int = int(this.bundledDrawArray.length);
         while(--_loc1_ > -1)
         {
            _loc2_ = this.bundledDrawArray[_loc1_];
            objectToBundleDraw.blendMode = BlendMode.ERASE;
            objectToBundleDraw.scaleX = _loc2_.scX;
            objectToBundleDraw.scaleY = _loc2_.scY;
            objectToBundleDraw.x = _loc2_.posX;
            objectToBundleDraw.y = _loc2_.posY;
            this.drawToRenderTexture(objectToBundleDraw);
         }
      }
      
      private function drawToRenderTexture(param1:Image) : void
      {
         transformMatrix.copyFrom(param1.transformationMatrix);
         transformMatrix.tx = param1.x - this.data.x;
         transformMatrix.ty = param1.y - this.data.y;
         this.renderTexture.draw(param1,transformMatrix);
      }
      
      public function clear() : void
      {
         this.data.bitmapData.fillRect(this.data.bitmapData.rect,0);
         var _loc1_:RenderTexture = this.getRenderTexture();
         _loc1_.clear();
         this.bundledDrawArray.splice();
         this.changeTexture(_loc1_);
      }
      
      private function changeTexture(param1:Texture) : void
      {
         this.image.texture = param1;
         this.image.readjustSize();
      }
      
      private function getRenderTexture() : RenderTexture
      {
         if(!this.renderTexture)
         {
            this.renderTexture = new RenderTexture(this.data.tileBounds.width,this.data.tileBounds.height,true,1);
            transformMatrix.identity();
            transformMatrix.translate(this.data.bdBounds.x,this.data.bdBounds.y);
            this.renderTexture.draw(new Image(this.texture),transformMatrix);
            this.image.pivotX = this.data.bdBounds.x;
            this.image.pivotY = this.data.bdBounds.y;
         }
         return this.renderTexture;
      }
      
      private function disposeRenderTexture() : void
      {
         if(this.renderTexture)
         {
            this.renderTexture.dispose();
         }
         this.renderTexture = null;
      }
   }
}

