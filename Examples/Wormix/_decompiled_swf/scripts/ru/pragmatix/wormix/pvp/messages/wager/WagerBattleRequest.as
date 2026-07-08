package ru.pragmatix.wormix.pvp.messages.wager
{
   import §_-L1H§.§_-b2K§;
   import §_-Tm§.§_-RF§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import com.hurlant.math.*;
   import flash.utils.Dictionary;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import starling.events.Event;
   
   use namespace bi_internal;
   
   public class WagerBattleRequest
   {
      
      public var host:String;
      
      public var port:int;
      
      public var wager:int;
      
      public var mapId:uint;
      
      public function WagerBattleRequest(param1:String = null, param2:int = 0, param3:uint = 0, param4:int = 0)
      {
         super();
         this.host = param1;
         this.port = param2;
         this.wager = param4;
         this.mapId = param3;
      }
   }
}

