package ru.pragmatix.flox.social.controller
{
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-x2t§;
   import §_-z1g§.§_-5c§;
   import com.greensock.*;
   import com.greensock.plugins.*;
   import flash.filters.BlurFilter;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   public class SocialApiSettings
   {
      
      public static const PUBLISH_TO_STREAM:SocialApiSettings = new SocialApiSettings(0,"PUBLISH TO STREAM");
      
      public static const NOTIFICATION:SocialApiSettings = new SocialApiSettings(1,"MESSAGING");
      
      public static const FRIENDS_ACCESS:SocialApiSettings = new SocialApiSettings(2);
      
      public static const PHOTO_ACCESS:SocialApiSettings = new SocialApiSettings(4,"PHOTO CONTENT");
      
      public static const AUDIO_ACCESS:SocialApiSettings = new SocialApiSettings(8);
      
      public static const VIDEO_ACCESS:SocialApiSettings = new SocialApiSettings(16);
      
      public static const WALL_POST:SocialApiSettings = new SocialApiSettings(512 + 8192);
      
      public static const STATUS_ACCESS:SocialApiSettings = new SocialApiSettings(1024,"SET STATUS");
      
      public static const NOTE_ACCESS:SocialApiSettings = new SocialApiSettings(2048);
      
      public var vkontakteCode:uint;
      
      public var odnoklassnikiSettingName:String;
      
      public function SocialApiSettings(param1:uint, param2:String = null)
      {
         super();
         this.vkontakteCode = param1;
         this.odnoklassnikiSettingName = param2;
      }
   }
}

