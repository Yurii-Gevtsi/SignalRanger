package ru.pragmatix.wormix.pvp.messages.ex
{
   public class PvpBattleType
   {
      
      public static const PVE_FRIEND:PvpBattleType = new PvpBattleType(1);
      
      public static const PVE_PARTNER:PvpBattleType = new PvpBattleType(2);
      
      public static const FRIEND_PVP:PvpBattleType = new PvpBattleType(3);
      
      public static const WAGER_PVP_DUEL:PvpBattleType = new PvpBattleType(4);
      
      public static const WAGER_PVP_4_FOR_ALL:PvpBattleType = new PvpBattleType(5);
      
      public static const WAGER_PVP_2X2:PvpBattleType = new PvpBattleType(6);
      
      public static const WAGER_PVP_2X2_FRIEND:PvpBattleType = new PvpBattleType(7);
      
      public static const WAGER_PVP_3_FOR_ALL:PvpBattleType = new PvpBattleType(8);
      
      private static const §_-R1o§:Array = [PVE_FRIEND,FRIEND_PVP,WAGER_PVP_DUEL,WAGER_PVP_4_FOR_ALL,WAGER_PVP_2X2,WAGER_PVP_2X2_FRIEND];
      
      private var _value:int;
      
      public function PvpBattleType(param1:int)
      {
         super();
         this._value = param1;
      }
      
      public static function valueOf(param1:uint) : PvpBattleType
      {
         var _loc2_:PvpBattleType = null;
         for each(_loc2_ in §_-R1o§)
         {
            if(_loc2_.value == param1)
            {
               return _loc2_;
            }
         }
         return null;
      }
      
      public function get value() : int
      {
         return this._value;
      }
   }
}

