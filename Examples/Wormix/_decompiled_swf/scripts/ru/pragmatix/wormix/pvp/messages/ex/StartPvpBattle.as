package ru.pragmatix.wormix.pvp.messages.ex
{
   import com.distriqt.extension.inappbilling.InAppBilling;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   
   public class StartPvpBattle
   {
      
      public static const ID:uint = 2001;
      
      public var battleId:uint;
      
      public var sessionKey:String;
      
      public var playerNum:uint;
      
      public function StartPvpBattle()
      {
         super();
      }
      
      public function toString() : String
      {
         return "StartPvpBattle{" + "battleId=" + this.battleId + "playerNum=" + this.playerNum + "}";
      }
   }
}

