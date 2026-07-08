package ru.pragmatix.wormix.serialization.client
{
   import §_-931§.§_-02O§;
   import §_-F39§.§_-a2W§;
   import §_-G16§.*;
   import §_-Y1Q§.§_-61Z§;
   import §_-Y1b§.§_-OP§;
   import §_-j22§.§_-5g§;
   import §_-r1C§.§_-V1R§;
   import §_-r1C§.§_-t1R§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-x2Q§.§_-dU§;
   import com.greensock.*;
   import com.greensock.core.*;
   import feathers.core.ITextRenderer;
   import flash.media.SoundTransform;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.PostToChat;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.HealBlocker;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class PostToChatBinarySerializer implements §_-D2L§
   {
      
      public function PostToChatBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 302;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:PostToChat = PostToChat(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeShort(_loc3_.action);
         _loc5_.writeUTF(_loc3_.profileName);
         _loc5_.writeUTF(_loc3_.message);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         return new PostToChat();
      }
   }
}

