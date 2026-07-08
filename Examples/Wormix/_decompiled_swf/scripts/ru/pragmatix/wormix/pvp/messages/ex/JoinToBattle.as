package ru.pragmatix.wormix.pvp.messages.ex
{
   public class JoinToBattle extends PvpLogin
   {
      
      public static const ID:uint = 1002;
      
      public var battleId:uint;
      
      public function JoinToBattle()
      {
         super();
      }
   }
}

