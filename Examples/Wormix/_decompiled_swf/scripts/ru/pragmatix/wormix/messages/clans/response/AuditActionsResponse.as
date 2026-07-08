package ru.pragmatix.wormix.messages.clans.response
{
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanAuditActionTO;
   import ru.pragmatix.wormix.messages.structures.ChatMessage;
   import ru.pragmatix.wormix.utils.StringUtil;
   
   public class AuditActionsResponse extends CommonClanResponse
   {
      
      public var actions:Vector.<ClanAuditActionTO> = new Vector.<ClanAuditActionTO>();
      
      public function AuditActionsResponse()
      {
         super();
      }
   }
}

