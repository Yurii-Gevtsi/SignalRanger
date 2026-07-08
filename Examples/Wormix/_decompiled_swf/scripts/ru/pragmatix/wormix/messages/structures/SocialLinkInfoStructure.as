package ru.pragmatix.wormix.messages.structures
{
   import §_-DJ§.§_-73§;
   import §_-DJ§.§_-fH§;
   import §_-Ly§.§_-81L§;
   import §_-YF§.§_-q2v§;
   import §_-r1C§.§_-h2B§;
   import §_-w2W§.§_-21w§;
   import feathers.controls.Button;
   import feathers.controls.ImageLoader;
   import feathers.controls.text.TextInstanceCreator;
   import feathers.core.ITextRenderer;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.VerticalAlign;
   import feathers.themes.FontColor;
   import feathers.themes.FontSize;
   import flash.text.TextFormatAlign;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.screenOverlay.view.*;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.ScaleMode;
   
   public class SocialLinkInfoStructure
   {
      
      public var socialType:int;
      
      public var socialId:String;
      
      public function SocialLinkInfoStructure(param1:int, param2:String)
      {
         super();
         this.socialType = param1;
         this.socialId = param2;
      }
      
      public function toString() : String
      {
         return "SocialLinkInfoStructure{" + "socialType=" + this.socialType + ",socialId=" + this.socialId + "}";
      }
   }
}

