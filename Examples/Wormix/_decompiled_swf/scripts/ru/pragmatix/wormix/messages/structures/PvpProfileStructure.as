package ru.pragmatix.wormix.messages.structures
{
   import starling.events.Event;
   
   public class PvpProfileStructure extends UserProfileStructure
   {
      
      public var socialName:String;
      
      public var socialNetworkId:int;
      
      public var playerNum:uint;
      
      public var teamNum:uint;
      
      public var dailyRating:int;
      
      public var backpackConf:Array;
      
      public var activeTeamMembers:Array;
      
      public var seasonsBestRank:Array;
      
      public function PvpProfileStructure()
      {
         super();
      }
   }
}

