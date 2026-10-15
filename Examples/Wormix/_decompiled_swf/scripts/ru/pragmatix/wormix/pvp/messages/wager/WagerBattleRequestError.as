package ru.pragmatix.wormix.pvp.messages.wager
{
   import §_-73d§.§_-pT§;
   import §_-l1K§.§_-W2f§;
   import com.hurlant.math.*;
   import flash.utils.Dictionary;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.messages.structures.ChatMessage;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import starling.events.Event;
   
   use namespace bi_internal;
   
   public class WagerBattleRequestError
   {
      
      public static const ERROR:int = 1;
      
      public static const NOT_ENOUGH_MONEY:int = 3;
      
      public var code:int;
      
      public function WagerBattleRequestError()
      {
         super();
      }
   }
}

