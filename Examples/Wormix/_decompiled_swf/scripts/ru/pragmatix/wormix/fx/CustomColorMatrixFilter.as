package ru.pragmatix.wormix.fx
{
   import starling.events.Event;
   import starling.filters.ColorMatrixFilter;
   
   public class CustomColorMatrixFilter extends ColorMatrixFilter
   {
      
      public function CustomColorMatrixFilter(param1:Vector.<Number> = null)
      {
         super(param1);
         Application.assetManager.addEventListener(Event.TEXTURES_RESTORED,this.onTextureRestored);
      }
      
      override public function dispose() : void
      {
         super.dispose();
         Application.assetManager.removeEventListener(Event.TEXTURES_RESTORED,this.onTextureRestored);
      }
      
      private function onTextureRestored(param1:Event) : void
      {
         if(isCached)
         {
            cache();
         }
      }
   }
}

