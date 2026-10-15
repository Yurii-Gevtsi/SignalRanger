package ru.pragmatix.flox.social.email
{
   import §_-12W§.*;
   import §_-62§.*;
   import §_-B1y§.Cookie;
   import §_-C3f§.§_-yG§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-RE§.§_-D3v§;
   import §_-SP§.§_-M4§;
   import §_-b1F§.§_-G4§;
   import §_-k13§.*;
   import §_-q26§.*;
   import §_-sQ§.§_-f1z§;
   import §_-u1F§.§_-SE§;
   import config.§_-vR§;
   import feathers.themes.styles.AlternateTextStyles;
   import flash.utils.ByteArray;
   import flash.utils.getTimer;
   import org.swiftsuspenders.*;
   import ru.pragmatix.flox.social.response.email.EmailSendResponse;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.arena.boss.BossTypesTab;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginClanBase;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanEnterAccount;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanErrorResponse;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.events.Event;
   
   public class MobileEmailSender implements IEmailSender
   {
      
      public function MobileEmailSender()
      {
         super();
      }
      
      public function isAvailable() : Boolean
      {
         return false;
      }
      
      public function sendEmail(param1:String, param2:String, param3:String, param4:Function) : void
      {
         param4(new EmailSendResponse(false));
      }
   }
}

