package ru.pragmatix.wormix.messages.client
{
   import §_-A1K§.§_-TA§;
   import §_-B1y§.*;
   import §_-D3A§.§_-TZ§;
   import §_-E1I§.§_-b2n§;
   import §_-H3§.*;
   import §_-RE§.§_-qW§;
   import §_-T2h§.*;
   import §_-g2r§.§_-03A§;
   import §_-k1u§.§_-Oi§;
   import §_-l1g§.§_-L3Q§;
   import §_-s20§.§_-J2g§;
   import §_-w2W§.WardrobeRoomScreen;
   import feathers.controls.List;
   import flash.events.ErrorEvent;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.model.SocialErrorCode;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.menu.arena.boss.*;
   import ru.pragmatix.wormix.messages.clans.response.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.structures.HeroicMissionStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionData;
   import ru.pragmatix.wormix.social.*;
   import starling.events.Event;
   import starling.utils.ScaleMode;
   
   public class Login
   {
      
      public var id:uint;
      
      public var referrerId:uint;
      
      public var authKey:String;
      
      public var ids:Array;
      
      public var socialCode:uint;
      
      public function Login()
      {
         super();
      }
      
      public function toString() : String
      {
         return "LoginBy{socialUserId=" + String(this.id) + ", auth_key=" + String(this.authKey) + ", socialId=" + String(this.socialCode) + "}";
      }
   }
}

