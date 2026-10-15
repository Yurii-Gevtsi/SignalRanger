package ru.pragmatix.wormix.serialization.server
{
   import §_-L1H§.§_-b2K§;
   import §_-gG§.§_-73L§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import dragonBones.animation.WorldClock;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.server.ChatMessageEvent;
   import ru.pragmatix.wormix.messages.structures.ChatMessage;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   
   public class ChatMessageEventBinarySerializer implements §_-D2L§
   {
      
      public function ChatMessageEventBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10302;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ByteArray = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         var _loc4_:ChatMessageEvent = new ChatMessageEvent();
         _loc4_.state = _loc3_.readShort();
         _loc4_.message = SerializationUtils.readStructure(_loc3_,ChatMessage) as ChatMessage;
         return _loc4_;
      }
   }
}

