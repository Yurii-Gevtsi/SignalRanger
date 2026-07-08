package ru.pragmatix.wormix.pvp.messages.ex
{
   public class CancelBattle
   {
      
      public static const ID:uint = 1004;
      
      public var battleId:uint;
      
      public var error:uint;
      
      public function CancelBattle()
      {
         super();
      }
   }
}

