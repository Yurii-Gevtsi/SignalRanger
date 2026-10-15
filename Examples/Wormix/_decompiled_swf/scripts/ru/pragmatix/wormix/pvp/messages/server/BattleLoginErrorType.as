package ru.pragmatix.wormix.pvp.messages.server
{
   import §_-O2L§.*;
   import §_-x2Q§.§_-dU§;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.LoaderStatus;
   import com.greensock.loading.core.*;
   import flash.events.Event;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.client.GoogleValidateInappPurchase;
   import ru.pragmatix.wormix.messages.server.LoginError;
   import ru.pragmatix.wormix.objects.*;
   import starling.display.Sprite;
   import starling.events.Event;
   
   public class BattleLoginErrorType
   {
      
      public static const PROFILACTIC_WORK:BattleLoginErrorType = new BattleLoginErrorType(LoginError.PROFILACTIC_WORK,§_-s2a§.PVP_SERVER_PROFILACTIC_WORK);
      
      public static const INCORRECT_KEY:BattleLoginErrorType = new BattleLoginErrorType(LoginError.INCORRECT_KEY,§_-s2a§.ERROR_AUTH_KEY);
      
      public static const ALREADY_IN_GAME:BattleLoginErrorType = new BattleLoginErrorType(LoginError.ALREADY_IN_GAME,§_-s2a§.BATTLE_REQUEST_BUSY);
      
      public static const INTERNAL_SERVER_ERROR:BattleLoginErrorType = new BattleLoginErrorType(LoginError.INTERNAL_SERVER_ERROR,§_-s2a§.PVP_SERVER_LOGIN_ERROR);
      
      public static const BANNED:BattleLoginErrorType = new BattleLoginErrorType(LoginError.BANNED,§_-s2a§.PVP_SERVER_BANNED);
      
      public static const RECONNECT_TIMEOUT:BattleLoginErrorType = new BattleLoginErrorType(LoginError.RECONNECT_TIMEOUT,§_-s2a§.RECONNECT_TIMEOUT);
      
      public static const INCORRECT_PROTOCOL_VERSION:BattleLoginErrorType = new BattleLoginErrorType(LoginError.INCORRECT_PROTOCOL_VERSION,§_-s2a§.INCORRECT_PROTOCOL_VERSION);
      
      private static const §_-R1o§:Array = [PROFILACTIC_WORK,INCORRECT_KEY,ALREADY_IN_GAME,INTERNAL_SERVER_ERROR,BANNED,RECONNECT_TIMEOUT,INCORRECT_PROTOCOL_VERSION];
      
      private var _value:int;
      
      private var _msg:String;
      
      public function BattleLoginErrorType(param1:int, param2:String)
      {
         super();
         this._value = param1;
         this._msg = §_-s2a§.§_-K3t§(param2);
      }
      
      public static function valueOf(param1:uint) : BattleLoginErrorType
      {
         return §_-R1o§[param1];
      }
      
      public function get value() : int
      {
         return this._value;
      }
      
      public function get message() : String
      {
         return this._msg;
      }
   }
}

