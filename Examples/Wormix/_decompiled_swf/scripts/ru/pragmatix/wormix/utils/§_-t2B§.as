package ru.pragmatix.wormix.utils
{
   import §_-12a§.KamikazeBuff;
   import §_-B1y§.Cookie;
   import §_-B1y§.§_-F1P§;
   import §_-DJ§.§_-J2l§;
   import config.§_-vR§;
   import flash.net.URLRequest;
   import flash.net.navigateToURL;
   import flash.system.Capabilities;
   import flash.utils.Dictionary;
   import ru.pragmatix.flox.social.response.email.EmailSendResponse;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-i14§;
   
   public class §_-t2B§
   {
      
      public static const §_-33x§:String = "support@pragmatixgames.com";
      
      private static const §_-22A§:String = "{app_name} {platform} Problem Report";
      
      private static const §_-me§:String = "WormixMobileLog";
      
      public function §_-t2B§()
      {
         super();
      }
      
      public static function §_-j2Z§() : void
      {
         Cookie.setValue(§_-me§,null);
      }
      
      public static function §_-y2l§() : void
      {
         var messageBody:String = null;
         var subject:String = null;
         var time:String = new Date().toUTCString();
         var id:String = UserProfileStructure.getSocialIdsAsString(Application.userProfile.getSocialIds());
         var version:String = §_-vR§.§_-w2u§ + "/" + §_-vR§.protocolVersion;
         var language:String = §_-X1j§.§_-82T§.§_-I9§().§_-b2§();
         var screenDetails:String = " (" + Application.§_-i1N§.stageWidth + "x" + Application.§_-i1N§.stageHeight + ", dpi = " + Capabilities.screenDPI + ")";
         messageBody = §_-F1P§.format(§_-s2a§.§_-K3t§(§_-s2a§.REPORT_BODY_TEMPLATE),{
            "time":time,
            "version":version,
            "id":id,
            "language":language,
            "os":Capabilities.os + screenDetails
         });
         subject = §_-F1P§.format(§_-22A§,{
            "app_name":§_-vR§.§_-s2R§,
            "platform":§_-vR§.§_-I1d§
         });
         if(§_-YD§.§_-t1O§)
         {
            messageBody = messageBody.replace("<br>","\n");
            messageBody = encodeURI(messageBody);
         }
         try
         {
            if(§_-X1j§.§_-31p§.isAvailable())
            {
               §_-X1j§.§_-31p§.sendEmail(subject,§_-33x§,messageBody,§_-23§);
            }
            else
            {
               §_-hS§(subject,messageBody);
            }
         }
         catch(e:Error)
         {
            §_-hS§(subject,messageBody);
         }
      }
      
      private static function §_-hS§(param1:String, param2:String) : void
      {
         var _loc3_:String = "mailto:" + §_-33x§;
         _loc3_ += "?subject=" + param1;
         _loc3_ += "&body=" + param2;
         navigateToURL(new URLRequest(_loc3_));
      }
      
      private static function §_-23§(param1:EmailSendResponse) : void
      {
      }
   }
}

