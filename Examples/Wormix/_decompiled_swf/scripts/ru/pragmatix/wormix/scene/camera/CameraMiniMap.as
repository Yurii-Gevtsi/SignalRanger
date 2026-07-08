package ru.pragmatix.wormix.scene.camera
{
   import flash.display.BitmapData;
   import flash.display.Shape;
   import flash.geom.Rectangle;
   import starling.animation.IAnimatable;
   import starling.core.*;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.textures.Texture;
   import starling.utils.*;
   
   public class CameraMiniMap implements IAnimatable
   {
      
      private var camera:SceneCamera;
      
      private var mapTexture:Texture;
      
      private var cameraTexture:Texture;
      
      private var minimap:Sprite;
      
      private var mapImage:Image;
      
      private var cameraImage:Image;
      
      public function CameraMiniMap(param1:SceneCamera)
      {
         var scale9Grid:Rectangle;
         var camera:SceneCamera = param1;
         super();
         this.camera = camera;
         this.mapTexture = this.generateTexture(Color.BLUE,64,20);
         this.cameraTexture = this.generateTexture(Color.RED,64,20);
         scale9Grid = Pool.getRectangle(22,22,20,20);
         this.minimap = new Sprite();
         this.minimap.scale = 0.07;
         this.mapImage = this.generateImage(this.mapTexture,camera.scene.width,camera.scene.height,scale9Grid);
         this.cameraImage = this.generateImage(this.cameraTexture,camera.cameraRect.width,camera.cameraRect.height,scale9Grid);
         with(this.minimap)
         {
            addChild(mapImage);
            addChild(cameraImage);
            alignPivot(Align.RIGHT,Align.BOTTOM);
            x = Starling.current.stage.stageWidth;
            y = Starling.current.stage.stageHeight;
            touchable = false;
         }
         Starling.current.stage.addChild(this.minimap);
         Starling.juggler.add(this);
      }
      
      public function dispose() : void
      {
         Starling.juggler.remove(this);
         this.minimap.removeFromParent(true);
         this.mapTexture.dispose();
         this.cameraTexture.dispose();
      }
      
      public function advanceTime(param1:Number) : void
      {
         var _loc2_:CameraRect = this.camera.cameraRect;
         this.cameraImage.x = _loc2_.x;
         this.cameraImage.y = _loc2_.y;
         this.cameraImage.width = _loc2_.width;
         this.cameraImage.height = _loc2_.height;
      }
      
      private function generateTexture(param1:uint, param2:int = 32, param3:int = 4) : Texture
      {
         var bd:BitmapData;
         var color:uint = param1;
         var size:int = param2;
         var lineWidth:int = param3;
         var shape:Shape = new Shape();
         var lineHalfWidth:Number = lineWidth * 0.5;
         with(shape.graphics)
         {
            lineTo(lineHalfWidth,lineHalfWidth);
            lineStyle(lineWidth,color);
            lineTo(size - lineHalfWidth,lineHalfWidth);
            lineTo(size - lineHalfWidth,size - lineHalfWidth);
            lineTo(lineHalfWidth,size - lineHalfWidth);
            lineTo(lineHalfWidth,lineHalfWidth);
         }
         bd = new BitmapData(size,size,true,0);
         bd.draw(shape);
         return Texture.fromBitmapData(bd);
      }
      
      private function generateImage(param1:Texture, param2:Number, param3:Number, param4:Rectangle) : Image
      {
         var texture:Texture = param1;
         var imageWidth:Number = param2;
         var imageHeight:Number = param3;
         var imageScale9Grid:Rectangle = param4;
         var image:Image = new Image(texture);
         with(image)
         {
            scale9Grid = imageScale9Grid;
            width = imageWidth;
            height = imageHeight;
         }
         return image;
      }
   }
}

