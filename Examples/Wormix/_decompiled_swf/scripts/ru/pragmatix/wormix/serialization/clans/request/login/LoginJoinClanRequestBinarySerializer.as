package ru.pragmatix.wormix.serialization.clans.request.login
{
   import §_-B1y§.*;
   import §_-B2z§.§_-82u§;
   import com.hurlant.crypto.tls.*;
   import feathers.controls.Button;
   import flash.errors.IllegalOperationError;
   import flash.utils.ByteArray;
   import flash.utils.getQualifiedClassName;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginJoinClanRequest;
   import starling.display.Image;
   
   public class LoginJoinClanRequestBinarySerializer extends LoginClanBaseBinarySerializer
   {
      
      public function LoginJoinClanRequestBinarySerializer()
      {
         super();
      }
      
      override protected function §_-41X§(param1:Object) : ByteArray
      {
         var _loc2_:ByteArray = new ByteArray();
         var _loc3_:LoginJoinClanRequest = LoginJoinClanRequest(param1);
         _loc2_.writeInt(_loc3_.clanId);
         _loc2_.writeInt(_loc3_.hostProfileId);
         return _loc2_;
      }
      
      override public function getCommandId() : uint
      {
         return ClanMessages.LOGIN_JOIN_REQUEST;
      }
   }
}

