package ru.pragmatix.wormix.messages.server
{
   import §_-82l§.§_-A2Y§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-fH§;
   import §_-G16§.*;
   import §_-Ly§.§_-81L§;
   import §_-Tm§.§_-RF§;
   import §_-U1i§.§_-J1§;
   import §_-Y1b§.§_-OP§;
   import §_-fo§.*;
   import §_-l1K§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-m2s§.§_-R11§;
   import §_-o14§.*;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.text.TextFormat;
   import flash.utils.Dictionary;
   import ru.pragmatix.wormix.messages.structures.ChatMessage;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.§_-g20§;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.Pool;
   
   public class ChatMessageEvent
   {
      
      public static const SUCCESS:int = 0;
      
      public static const ILLEGAL_STATE:int = 1;
      
      public static const NOT_ALLOWED:int = 2;
      
      public static const TOO_FAST:int = 3;
      
      public static const RESTRICTED:int = 4;
      
      public var state:int;
      
      public var message:ChatMessage;
      
      public function ChatMessageEvent()
      {
         super();
      }
      
      public function toString() : String
      {
         return "ChatMessageEvent{ " + " state=" + this.state + " message=" + this.message + " }";
      }
   }
}

