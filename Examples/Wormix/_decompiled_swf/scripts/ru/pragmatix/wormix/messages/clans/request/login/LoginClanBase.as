package ru.pragmatix.wormix.messages.clans.request.login
{
   import §_-43V§.Random;
   import §_-62§.§_-W1r§;
   import §_-Y1b§.§_-81l§;
   import §_-b1F§.§_-v2Q§;
   import §_-j1w§.§_-L1x§;
   import §_-jF§.*;
   import §_-qH§.§_-S25§;
   import §_-sQ§.§_-V1n§;
   import com.hurlant.math.*;
   import com.hurlant.util.§_-a2P§;
   import feathers.controls.ToggleButton;
   import flash.events.Event;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreationFailure;
   import ru.pragmatix.wormix.pvp.messages.ex.StartPvpBattle;
   import ru.pragmatix.wormix.utils.§_-F2L§;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   use namespace bi_internal;
   
   public class LoginClanBase
   {
      
      public var protocolVersion:int;
      
      public var socialId:int;
      
      public var profileId:int;
      
      public var socialProfileId:String;
      
      public var name:String;
      
      public var sessionKey:String;
      
      public function LoginClanBase()
      {
         super();
      }
      
      public function toString() : String
      {
         return "LoginClanBase{protocolVersion=" + String(this.protocolVersion) + ",socialId=" + String(this.socialId) + ",profileId=" + String(this.profileId) + ",socialProfileId=" + String(this.socialProfileId) + ",name=" + String(this.name) + ",sessionKey=" + String(this.sessionKey) + "}";
      }
   }
}

