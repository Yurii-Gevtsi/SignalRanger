package ru.pragmatix.wormix.pvp.messages.ex
{
   public class BattleCreationFailure
   {
      
      public static const ID:uint = 2003;
      
      public var battleId:uint;
      
      public var refuserId:uint;
      
      public var socialNetId:uint;
      
      public var reason:CallToBattleResultType;
      
      public function BattleCreationFailure()
      {
         super();
      }
   }
}

