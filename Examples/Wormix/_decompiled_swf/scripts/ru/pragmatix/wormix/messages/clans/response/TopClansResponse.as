package ru.pragmatix.wormix.messages.clans.response
{
   import §_-Y1O§.§_-42h§;
   import §_-Y1O§.§_-V23§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.utils.§_-z1x§;
   
   public class TopClansResponse extends CommonClanResponse
   {
      
      public var topClans:Vector.<ClanTO> = new Vector.<ClanTO>();
      
      public var oldPlaces:Array = [];
      
      public var rating:int;
      
      public var oldPlace:int;
      
      public var playerClanPosition:int;
      
      public var startSeasonDate:uint;
      
      public var finishSeasonDate:uint;
      
      public var seasonId:int;
      
      public function TopClansResponse()
      {
         super();
      }
   }
}

