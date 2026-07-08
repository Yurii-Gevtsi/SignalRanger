package ru.pragmatix.wormix.pvp.messages.ex
{
   public class ReconnectToBattle extends PvpLogin
   {
      
      public static const ID:uint = 1015;
      
      public var battleId:uint;
      
      public var turnNum:uint;
      
      public var playerNum:uint;
      
      public var lastCommandNum:int;
      
      public function ReconnectToBattle()
      {
         super();
      }
   }
}

