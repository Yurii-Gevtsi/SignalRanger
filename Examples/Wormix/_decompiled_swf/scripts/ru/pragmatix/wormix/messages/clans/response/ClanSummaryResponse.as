package ru.pragmatix.wormix.messages.clans.response
{
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   
   public class ClanSummaryResponse extends CommonClanResponse
   {
      
      public var clan:ClanTO;
      
      public function ClanSummaryResponse()
      {
         super();
      }
   }
}

