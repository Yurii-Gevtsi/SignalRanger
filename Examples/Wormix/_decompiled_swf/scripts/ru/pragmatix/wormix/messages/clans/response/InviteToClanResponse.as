package ru.pragmatix.wormix.messages.clans.response
{
   import ru.pragmatix.wormix.messages.clans.structures.ClanInviteTO;
   
   public class InviteToClanResponse extends CommonClanResponse
   {
      
      public var interopCode:int;
      
      public var invites:Vector.<ClanInviteTO> = new Vector.<ClanInviteTO>();
      
      public function InviteToClanResponse()
      {
         super();
      }
   }
}

