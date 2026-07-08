package ru.pragmatix.wormix.pvp.messages.ex
{
   import §_-z1g§.§_-82X§;
   import ru.pragmatix.wormix.messages.structures.PvpProfileStructure;
   
   public class CallToBattle
   {
      
      public static const ID:int = 2002;
      
      public var profileId:uint;
      
      public var battleWager:§_-82X§;
      
      public var battleId:uint;
      
      public var mapId:uint;
      
      public var missionIds:Vector.<int> = new Vector.<int>();
      
      public var participantStructs:Vector.<PvpProfileStructure>;
      
      public var clientParams:Object;
      
      public function CallToBattle()
      {
         super();
      }
   }
}

