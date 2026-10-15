package ru.pragmatix.wormix.pvp.messages.ex
{
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   
   public class CallToBattleResultType
   {
      
      public static const UNDEFINED:CallToBattleResultType = new CallToBattleResultType(999,"BATTLE_REQUEST_UNDEFINED_RESULT");
      
      public static const SERVER_ERROR:CallToBattleResultType = new CallToBattleResultType(0,"PVP_SERVER_CONNECTION_ERROR");
      
      public static const ACCEPTED:CallToBattleResultType = new CallToBattleResultType(1,"BATTLE_REQUEST_ACCEPTED");
      
      public static const REJECTED:CallToBattleResultType = new CallToBattleResultType(2,"BATTLE_REQUEST_DECLINE");
      
      public static const OFFLINE:CallToBattleResultType = new CallToBattleResultType(3,"BATTLE_REQUEST_NOT_ONLINE");
      
      public static const BUSY:CallToBattleResultType = new CallToBattleResultType(4,"BATTLE_REQUEST_BUSY","CURRENT_USER_ALREADY_IN_BATTLE");
      
      public static const BANNED:CallToBattleResultType = new CallToBattleResultType(5,"BATTLE_REQUEST_BANNED");
      
      public static const CANCELED:CallToBattleResultType = new CallToBattleResultType(6,"BATTLE_CANCEL_TITLE");
      
      public static const TIMEOUT:CallToBattleResultType = new CallToBattleResultType(7,"BATTLE_REQUEST_TIMEOUT");
      
      public static const CLIENT_ERROR:CallToBattleResultType = new CallToBattleResultType(8,"BATTLE_REQUEST_CLIENT_ERROR");
      
      public static const DISCONNECTED:CallToBattleResultType = new CallToBattleResultType(9,"BATTLE_REQUEST_DISCONNECTED");
      
      public static const NO_ENOUGH_MONEY:CallToBattleResultType = new CallToBattleResultType(10,"BATTLE_REQUEST_FRIEND_LACKS_MONEY","BATTLE_REQUEST_PLAYER_LACKS_MONEY");
      
      public static const INSUFFICIENT_LEVEL:CallToBattleResultType = new CallToBattleResultType(11,"BATTLE_REQUEST_FRIEND_LACKS_LEVEL","BATTLE_REQUEST_PLAYER_LACKS_LEVEL");
      
      public static const EXCEED_BATTLES:CallToBattleResultType = new CallToBattleResultType(12,"BATTLE_REQUEST_EXCEED_BATTLES","BATTLE_REQUEST_PLAYER_EXCEED_BATTLES");
      
      public static const MISSION_LOCKED:CallToBattleResultType = new CallToBattleResultType(14,"BATTLE_REQUEST_MISSION_LOCKED");
      
      public static const ARENA_IS_LOCKED:CallToBattleResultType = new CallToBattleResultType(15,"BATTLE_REQUEST_ARENA_IS_LOCKED");
      
      public static const TEAM_IS_SMALL:CallToBattleResultType = new CallToBattleResultType(16,"BATTLE_REQUEST_TEAM_IS_SMALL");
      
      private static const §_-R1o§:Array = [SERVER_ERROR,ACCEPTED,REJECTED,OFFLINE,BUSY,BANNED,CANCELED,TIMEOUT,CLIENT_ERROR,DISCONNECTED,NO_ENOUGH_MONEY,INSUFFICIENT_LEVEL,EXCEED_BATTLES,UNDEFINED,MISSION_LOCKED,ARENA_IS_LOCKED,TEAM_IS_SMALL];
      
      private var _value:int;
      
      private var §_-P1j§:String;
      
      private var §_-w2E§:String;
      
      public function CallToBattleResultType(param1:int, param2:String, param3:String = null)
      {
         super();
         this._value = param1;
         this.§_-P1j§ = param2;
         this.§_-w2E§ = param3;
      }
      
      public static function valueOf(param1:uint) : CallToBattleResultType
      {
         return param1 < §_-R1o§.length ? §_-R1o§[param1] : UNDEFINED;
      }
      
      public function get value() : int
      {
         return this._value;
      }
      
      public function get remoteFailureReason() : String
      {
         return §_-s2a§.§_-K3t§(this.§_-P1j§);
      }
      
      public function get localFailureReason() : String
      {
         return this.§_-w2E§ ? §_-s2a§.§_-K3t§(this.§_-w2E§) : this.remoteFailureReason;
      }
   }
}

