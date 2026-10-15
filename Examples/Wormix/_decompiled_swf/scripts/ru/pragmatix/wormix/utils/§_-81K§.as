package ru.pragmatix.wormix.utils
{
   import §_-62§.§_-W1r§;
   import §_-63U§.*;
   import §_-fo§.§_-r5§;
   import com.distriqt.extension.inappbilling.Purchase;
   import flash.display.MovieClip;
   import flash.events.TimerEvent;
   import flash.utils.Timer;
   import flash.utils.getQualifiedClassName;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.request.AuditActionsRequest;
   import ru.pragmatix.wormix.messages.clans.response.AuditActionsResponse;
   import ru.pragmatix.wormix.messages.client.AppleVerifyReceipt;
   
   public class §_-81K§
   {
      
      public function §_-81K§()
      {
         super();
      }
      
      public static function §_-92P§(param1:§_-W1r§) : MovieClip
      {
         var _loc2_:MovieClip = null;
         if(param1.§_-92P§())
         {
            _loc2_ = §_-r5§.§_-tF§(param1.§_-92P§());
            if(param1.§_-Z2t§())
            {
               _loc2_.getChildAt(0).x = 0;
               _loc2_.getChildAt(0).y = 0;
            }
         }
         else
         {
            _loc2_ = §_-r5§.§_-tF§(param1.§_-G3a§());
            _loc2_.getChildAt(0).x = 0;
            _loc2_.getChildAt(0).y = 0;
         }
         _loc2_.cacheAsBitmap = true;
         return _loc2_;
      }
   }
}

