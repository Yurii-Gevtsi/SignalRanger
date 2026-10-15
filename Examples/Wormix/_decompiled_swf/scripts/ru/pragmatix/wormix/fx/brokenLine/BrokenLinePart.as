package ru.pragmatix.wormix.fx.brokenLine
{
   import §_-G16§.§_-Y2x§;
   import flash.display.BitmapData;
   import starling.display.Image;
   import starling.textures.Texture;
   
   public class BrokenLinePart extends Image implements §_-Y2x§
   {
      
      private static const linePartBD:BitmapData = new BitmapData(32,32,false,16777215);
      
      private static const linePartTexture:Texture = Texture.fromBitmapData(linePartBD,false);
      
      public function BrokenLinePart()
      {
         super(linePartTexture);
      }
      
      public function init(param1:Number, param2:Number, param3:uint) : void
      {
         this.width = param1;
         this.height = param2;
         this.color = param3;
         this.alpha = 0.99;
      }
      
      public function removeFromPool() : void
      {
         dispose();
      }
   }
}

