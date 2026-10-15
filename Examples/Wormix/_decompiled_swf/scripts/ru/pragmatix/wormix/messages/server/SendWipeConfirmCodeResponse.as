package ru.pragmatix.wormix.messages.server
{
   import §_-92a§.§_-Y2Y§;
   import §_-TF§.*;
   import §_-j1y§.§_-i1b§;
   import §_-l1K§.*;
   import §_-l2r§.§_-K1t§;
   import feathers.controls.Button;
   import feathers.core.ITextRenderer;
   import feathers.utils.touch.TapToTrigger;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.weapons.Girder;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.utils.MathUtil;
   
   public class SendWipeConfirmCodeResponse
   {
      
      public static const OK:int = 0;
      
      public static const DAILY_LIMIT_EXCEEDED:int = 1;
      
      public static const TODAY_ALREADY_WIPED:int = 2;
      
      public static const WRONG_MOBILE_NUMBER:int = 3;
      
      public static const ERROR:int = 3;
      
      public var result:int;
      
      public function SendWipeConfirmCodeResponse()
      {
         super();
      }
   }
}

