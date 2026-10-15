package ru.pragmatix.wormix.messages.server
{
   import §_-31W§.*;
   import §_-62§.§_-W1r§;
   import §_-A1K§.§_-TA§;
   import §_-TF§.§_-13z§;
   import §_-TF§.§_-J3K§;
   import §_-TF§.§_-Sq§;
   import §_-TF§.§_-t2r§;
   import §_-fo§.§_-r5§;
   import §_-s20§.§_-J2g§;
   import com.distriqt.extension.inappbilling.Purchase;
   import feathers.controls.SimpleScrollBar;
   import feathers.layout.Direction;
   import flash.display.MovieClip;
   import flash.events.TimerEvent;
   import flash.utils.Timer;
   import flash.utils.clearTimeout;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.AuditActionsRequest;
   import ru.pragmatix.wormix.messages.clans.response.AuditActionsResponse;
   import ru.pragmatix.wormix.messages.client.AppleVerifyReceipt;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.utils.Pool;
   
   public class LinkResult
   {
      
      public static const SUCCESS:uint = 0;
      
      public static const ERROR:uint = 1;
      
      public static const INVALID_PLATFORM_ID:uint = 2;
      
      public static const INVALID_AUTH_KEY:uint = 3;
      
      public static const LINK_ALREADY_EXISTS:uint = 4;
      
      public var resultEnum:int;
      
      public var userProfileStructure:UserProfileStructure;
      
      public var isSecure:Boolean;
      
      public function LinkResult()
      {
         super();
      }
      
      public function toString() : String
      {
         return "LinkResult{resultEnum = " + this.resultEnum + ", userProfileStructure = " + this.userProfileStructure + ", isSecure = " + this.isSecure + "}";
      }
   }
}

