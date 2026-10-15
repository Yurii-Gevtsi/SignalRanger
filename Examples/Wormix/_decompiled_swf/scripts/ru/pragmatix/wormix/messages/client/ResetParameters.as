package ru.pragmatix.wormix.messages.client
{
   import §_-73I§.*;
   import §_-p18§.§_-83X§;
   import flash.events.*;
   import flash.geom.ColorTransform;
   import flash.utils.*;
   import org.as3commons.zip.*;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import starling.core.Starling;
   import starling.utils.Pool;
   
   public class ResetParameters
   {
      
      public static const REAL_MONEY:int = 0;
      
      public static const MONEY:int = 1;
      
      public var moneyType:int;
      
      public function ResetParameters(param1:int = -1)
      {
         super();
         this.moneyType = param1;
      }
   }
}

