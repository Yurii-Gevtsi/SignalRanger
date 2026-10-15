package ru.pragmatix.wormix.messages.server
{
   import §_-51M§.Dragon;
   import §_-B2z§.§_-63i§;
   import §_-H3§.*;
   import §_-gG§.§_-Nl§;
   import flash.display.BitmapData;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.server.PvpRetryCommandRequestServer;
   import ru.pragmatix.wormix.social.§_-N1V§;
   
   public class DistributePointsResult
   {
      
      public static const SUCCESS:int = 0;
      
      public static const ERROR:int = 1;
      
      public static const NOT_ENOUGH_POINTS:int = 2;
      
      public var result:int;
      
      public function DistributePointsResult()
      {
         super();
      }
   }
}

