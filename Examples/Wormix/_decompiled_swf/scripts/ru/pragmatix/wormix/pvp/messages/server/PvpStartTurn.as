package ru.pragmatix.wormix.pvp.messages.server
{
   import §_-A1K§.§_-TA§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.model.items.craft.§_-6S§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   
   public class PvpStartTurn
   {
      
      public var battleId:uint;
      
      public var nextPlayerNum:uint;
      
      public var turnNum:uint;
      
      public var commandNum:uint;
      
      public var droppedNums:Array;
      
      public function PvpStartTurn()
      {
         super();
      }
   }
}

