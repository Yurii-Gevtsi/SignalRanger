package ru.pragmatix.flox.social.email
{
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-fH§;
   import §_-Ly§.§_-81L§;
   import §_-SP§.§_-M4§;
   import §_-l1g§.§_-L3Q§;
   import com.greensock.loading.*;
   import com.greensock.loading.core.*;
   import feathers.controls.ImageLoader;
   import feathers.controls.text.TextInstanceCreator;
   import feathers.core.ITextRenderer;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.VerticalAlign;
   import feathers.themes.FontColor;
   import feathers.themes.FontSize;
   import flash.errors.IllegalOperationError;
   import flash.text.TextFormatAlign;
   import flash.utils.ByteArray;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import starling.core.Starling;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.ScaleMode;
   
   public class EmptyEmailSender implements IEmailSender
   {
      
      public function EmptyEmailSender()
      {
         super();
      }
      
      public function isAvailable() : Boolean
      {
         return false;
      }
      
      public function sendEmail(param1:String, param2:String, param3:String, param4:Function) : void
      {
         throw new IllegalOperationError();
      }
   }
}

