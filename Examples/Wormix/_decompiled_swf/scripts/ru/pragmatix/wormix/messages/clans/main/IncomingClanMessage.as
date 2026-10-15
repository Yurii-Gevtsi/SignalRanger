package ru.pragmatix.wormix.messages.clans.main
{
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.objects.Direction;
   
   public class IncomingClanMessage
   {
      
      public var message:ClanChatMessageTO;
      
      public function IncomingClanMessage()
      {
         super();
      }
   }
}

