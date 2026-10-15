package ru.pragmatix.wormix.serialization.clans.request.login
{
   import §_-L1H§.§_-b2K§;
   import §_-Y1O§.§_-42h§;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import starling.events.Event;
   
   public class LoginClanRequestBinarySerializer extends LoginClanBaseBinarySerializer
   {
      
      public function LoginClanRequestBinarySerializer()
      {
         super();
      }
      
      override protected function §_-41X§(param1:Object) : ByteArray
      {
         return new ByteArray();
      }
      
      override public function getCommandId() : uint
      {
         return ClanMessages.LOGIN_REQUEST;
      }
   }
}

