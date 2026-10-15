package ru.pragmatix.wormix.pvp.messages.ex
{
   public class PvpBattleResult
   {
      
      public static const WINNER:PvpBattleResult = new PvpBattleResult(0,2);
      
      public static const NOT_WINNER:PvpBattleResult = new PvpBattleResult(1,0);
      
      public static const DRAW_GAME:PvpBattleResult = new PvpBattleResult(2,1);
      
      public static const DRAW_DESYNC:PvpBattleResult = new PvpBattleResult(3,2);
      
      public static const DRAW_SHUTDOWN:PvpBattleResult = new PvpBattleResult(4,1);
      
      private var _type:int;
      
      private var §_-c1Z§:int;
      
      public function PvpBattleResult(param1:int, param2:int)
      {
         super();
         this._type = param1;
         this.§_-c1Z§ = param2;
      }
      
      public static function valueOf(param1:int) : PvpBattleResult
      {
         switch(param1)
         {
            case 0:
               return WINNER;
            case 1:
               return NOT_WINNER;
            case 2:
               return DRAW_GAME;
            case 3:
               return DRAW_DESYNC;
            case 4:
               return DRAW_SHUTDOWN;
            default:
               return null;
         }
      }
      
      public function get type() : int
      {
         return this._type;
      }
      
      public function get weight() : int
      {
         return this.§_-c1Z§;
      }
   }
}

