package ru.pragmatix.wormix.serialization.clans.request.login
{
   import §_-A2U§.§_-A23§;
   import §_-B1y§.*;
   import com.distriqt.extension.inappbilling.events.ApplicationReceiptEvent;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginClanBase;
   import ru.pragmatix.wormix.serialization.BaseCommandSerializer;
   
   public class LoginClanBaseBinarySerializer extends BaseCommandSerializer
   {
      
      public function LoginClanBaseBinarySerializer()
      {
         super();
      }
      
      override protected function §_-41X§(param1:Object) : ByteArray
      {
         var _loc2_:LoginClanBase = LoginClanBase(param1);
         var _loc3_:ByteArray = new ByteArray();
         _loc3_.writeInt(_loc2_.protocolVersion);
         _loc3_.writeShort(_loc2_.socialId);
         _loc3_.writeInt(_loc2_.profileId);
         _loc3_.writeUTF(_loc2_.socialProfileId);
         _loc3_.writeUTF(_loc2_.name);
         _loc3_.writeUTF(_loc2_.sessionKey);
         return _loc3_;
      }
   }
}

