package ru.pragmatix.wormix.serialization.client
{
   import §_-12W§.§_-221§;
   import §_-31W§.*;
   import §_-931§.§_-02O§;
   import §_-B2z§.§_-63i§;
   import §_-W§.§_-ix§;
   import §_-f1G§.§_-kf§;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-zI§.§_-sI§;
   import com.hurlant.crypto.tls.§_-Tx§;
   import com.hurlant.crypto.tls.§_-a2D§;
   import feathers.controls.Button;
   import feathers.core.ITextRenderer;
   import flash.events.Event;
   import flash.events.IEventDispatcher;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.getTimer;
   import org.gestouch.core.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.JoinToChat;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.server.PvpSystemMessage;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   use namespace gestouch_internal;
   
   public class JoinToChatBinarySerializer implements §_-D2L§
   {
      
      public function JoinToChatBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 301;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:JoinToChat = JoinToChat(param1);
         var _temp_1:* = new §_-5g§();
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         return new JoinToChat();
      }
   }
}

