package ru.pragmatix.wormix.messages.client
{
   import §_-62§.*;
   import §_-A1K§.§_-TA§;
   import §_-TF§.§_-q1j§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-zI§.§_-X1q§;
   import com.greensock.*;
   import com.greensock.core.*;
   import feathers.core.FeathersControl;
   import flash.display.DisplayObject;
   import flash.text.TextField;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.controller.SocialApiSettings;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.utils.SystemUtil;
   
   public class GetProfiles
   {
      
      public var sessionKey:String;
      
      public var ids:Vector.<String>;
      
      public function GetProfiles()
      {
         super();
      }
      
      public function toString() : String
      {
         return "GetProfiles{" + "ids= " + this.ids + "}";
      }
   }
}

