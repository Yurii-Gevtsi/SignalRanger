package ru.pragmatix.wormix.messages.clans.request.login
{
   import §_-73d§.§_-x2C§;
   import §_-A1K§.§_-TA§;
   import §_-U1i§.§_-O2i§;
   import §_-Y1O§.*;
   import §_-b1F§.§_-v2Q§;
   import §_-g2r§.§_-03A§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-d2j§;
   import flash.events.Event;
   import flash.ui.Keyboard;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.gui.*;
   import ru.pragmatix.wormix.messages.client.AchieveLogin;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import starling.events.Event;
   
   public class LoginCreateClanRequest extends LoginClanBase
   {
      
      public var clanName:String;
      
      public var clanEmblem:Vector.<uint>;
      
      public var clanDescription:String;
      
      public function LoginCreateClanRequest()
      {
         super();
      }
      
      override public function toString() : String
      {
         return "LoginCreateClanRequest{" + "protocolVersion=" + String(protocolVersion) + ",socialId=" + String(socialId) + ",profileId=" + String(profileId) + ",socialProfileId=" + String(socialProfileId) + ",name=" + String(name) + ",sessionKey=" + String(sessionKey) + ",clanName=" + String(this.clanName) + ",clanEmblem=" + String(this.clanEmblem) + ",clanDescription=" + String(this.clanDescription) + "}";
      }
   }
}

