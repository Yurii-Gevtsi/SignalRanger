package ru.pragmatix.flox.social.model
{
   import §_-12W§.*;
   import §_-62§.*;
   import §_-73d§.§_-g1J§;
   import §_-B1y§.*;
   import §_-C3f§.§_-yG§;
   import §_-RE§.§_-qW§;
   import §_-T2h§.*;
   import §_-k1u§.§_-Oi§;
   import §_-l1K§.§_-d2j§;
   import §_-q26§.*;
   import §_-w2W§.WardrobeRoomScreen;
   import feathers.controls.List;
   import flash.events.ErrorEvent;
   import flash.utils.ByteArray;
   import flash.utils.getTimer;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.menu.arena.boss.*;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginClanBase;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanEnterAccount;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanErrorResponse;
   import ru.pragmatix.wormix.messages.structures.HeroicMissionStructure;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionData;
   import starling.events.Event;
   
   public class SocialErrorCode
   {
      
      public var code:uint;
      
      public var msg:String;
      
      public function SocialErrorCode(param1:uint, param2:String)
      {
         super();
         this.code = param1;
         this.msg = param2;
      }
      
      public static function createError(param1:ErrorEvent) : SocialErrorCode
      {
         return new SocialErrorCode(0,param1.type + ": " + param1.text);
      }
      
      public function toString() : String
      {
         return "SocialErrorCode{code=" + String(this.code) + ",msg=" + String(this.msg) + "}";
      }
   }
}

