package ru.pragmatix.wormix.messages.clans.response
{
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   
   public class ListClansResponse extends CommonClanResponse
   {
      
      public var clans:Vector.<ClanTO> = new Vector.<ClanTO>();
      
      public function ListClansResponse()
      {
         super();
      }
   }
}

