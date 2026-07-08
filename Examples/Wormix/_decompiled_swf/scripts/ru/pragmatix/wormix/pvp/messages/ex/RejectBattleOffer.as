package ru.pragmatix.wormix.pvp.messages.ex
{
   import flash.utils.ByteArray;
   
   public class RejectBattleOffer extends PvpLogin
   {
      
      public static const ID:uint = 1003;
      
      public var battleId:uint;
      
      public var manual:Boolean;
      
      public function RejectBattleOffer()
      {
         super();
      }
   }
}

