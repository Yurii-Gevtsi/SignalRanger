package ru.pragmatix.wormix.messages.clans.request
{
   import §_-DJ§.§_-fH§;
   import §_-R2I§.§_-u2V§;
   import §_-f1G§.§_-kf§;
   import §_-z1g§.§_-82X§;
   import com.hurlant.util.*;
   import flash.utils.ByteArray;
   import org.gestouch.core.GestureState;
   import org.gestouch.core.gestouch_internal;
   import ru.pragmatix.flox.social.utils.id.UserIdType;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanServiceResult;
   import ru.pragmatix.wormix.model.items.craft.*;
   import starling.display.Sprite;
   import starling.events.Event;
   
   use namespace gestouch_internal;
   
   public class PostToClanChatRequest
   {
      
      public var text:String;
      
      public function PostToClanChatRequest(param1:String)
      {
         super();
         this.text = param1;
      }
   }
}

