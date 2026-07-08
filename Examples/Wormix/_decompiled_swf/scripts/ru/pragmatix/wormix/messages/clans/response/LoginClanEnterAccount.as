package ru.pragmatix.wormix.messages.clans.response
{
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanNewsTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   
   public class LoginClanEnterAccount extends CommonClanResponse
   {
      
      public var sessionKey:String;
      
      public var clan:ClanTO;
      
      public var chat:Vector.<ClanChatMessageTO> = new Vector.<ClanChatMessageTO>();
      
      public var newsBoard:Vector.<ClanNewsTO> = new Vector.<ClanNewsTO>();
      
      public var startSeasonDate:int;
      
      public var finishSeasonDate:int;
      
      public function LoginClanEnterAccount()
      {
         super();
      }
   }
}

