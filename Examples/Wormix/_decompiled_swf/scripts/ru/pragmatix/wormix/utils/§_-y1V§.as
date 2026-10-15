package ru.pragmatix.wormix.utils
{
   import §_-62§.§_-G1p§;
   import §_-62§.§_-O2H§;
   import §_-B1y§.Cookie;
   import §_-B2z§.§_-63i§;
   import §_-P1z§.*;
   import §_-W§.§_-ix§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1O§.§_-y1o§;
   import §_-Y1b§.§_-81l§;
   import §_-j1w§.§_-L1x§;
   import §_-k1u§.§_-Oi§;
   import §_-r1C§.§_-h2B§;
   import §_-r1C§.§_-t1R§;
   import com.distriqt.extension.inappbilling.events.ApplicationReceiptEvent;
   import feathers.controls.NumericStepper;
   import flash.display.StageDisplayState;
   import flash.geom.Point;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.server.PvpSystemMessage;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.SystemUtil;
   
   public class §_-y1V§
   {
      
      private static const FULLSCREEN:String = "Fullscreen";
      
      public function §_-y1V§()
      {
         super();
      }
      
      public static function onStart() : void
      {
         if(SystemUtil.isAIR && SystemUtil.isDesktop)
         {
            if(Cookie.getValue(FULLSCREEN,true))
            {
               if(!§_-BV§)
               {
                  §_-B39§();
               }
            }
            else if(§_-BV§)
            {
               §_-B39§();
            }
         }
      }
      
      public static function §_-B39§() : void
      {
         if(§_-BV§)
         {
            Starling.current.nativeStage.displayState = StageDisplayState.NORMAL;
         }
         else
         {
            Starling.current.nativeStage.displayState = StageDisplayState.FULL_SCREEN_INTERACTIVE;
         }
         Cookie.setValue(FULLSCREEN,§_-BV§);
      }
      
      public static function get §_-BV§() : Boolean
      {
         return Starling.current.nativeStage.displayState == StageDisplayState.FULL_SCREEN_INTERACTIVE;
      }
      
      public static function get isAvailable() : Boolean
      {
         return SystemUtil.isDesktop;
      }
   }
}

