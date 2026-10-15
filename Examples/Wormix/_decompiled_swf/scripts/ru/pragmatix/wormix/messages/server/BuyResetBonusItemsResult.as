package ru.pragmatix.wormix.messages.server
{
   import §_-31W§.*;
   import §_-B1y§.*;
   import §_-J31§.§_-e2J§;
   import §_-R2S§.*;
   import §_-Z1w§.§_-s1x§;
   import §_-q11§.§_-C2n§;
   import §_-x2Q§.§_-dU§;
   import com.hurlant.math.BigInteger;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginClanBase;
   import starling.events.Event;
   
   public class BuyResetBonusItemsResult
   {
      
      public static const SUCCESS:int = 0;
      
      public static const ERROR:int = 1;
      
      public static const MIN_REQUIREMENTS_ERROR:int = 2;
      
      public static const NOT_ENOUGH_MONEY:int = 3;
      
      public static const CONFIRM_FAILURE:int = 4;
      
      public var result:int;
      
      public var requestNum:int;
      
      public var isSecure:Boolean;
      
      public function BuyResetBonusItemsResult()
      {
         super();
      }
   }
}

