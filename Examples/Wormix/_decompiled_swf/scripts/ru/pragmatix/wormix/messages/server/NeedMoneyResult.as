package ru.pragmatix.wormix.messages.server
{
   import §_-62§.§_-O2H§;
   import §_-62§.§_-T1y§;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-ou§;
   import §_-62§.§_-zF§;
   import §_-oq§.§_-c1o§;
   import §_-u1T§.§_-33B§;
   import §_-z1g§.§_-F4§;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.LoaderStatus;
   import flash.events.Event;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.ScaleMode;
   
   public class NeedMoneyResult
   {
      
      public static const SUCCESS:int = 0;
      
      public static const ERROR:int = 1;
      
      public static const NOT_ENOUGH_MONEY:int = 3;
      
      public var value:int;
      
      public var result:int;
      
      public function NeedMoneyResult()
      {
         super();
      }
   }
}

