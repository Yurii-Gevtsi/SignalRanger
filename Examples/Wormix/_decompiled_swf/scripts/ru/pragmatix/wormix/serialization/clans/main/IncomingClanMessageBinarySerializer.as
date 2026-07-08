package ru.pragmatix.wormix.serialization.clans.main
{
   import §_-A1K§.§_-TA§;
   import §_-CF§.§_-x2t§;
   import §_-N8§.§_-o2w§;
   import §_-Vg§.*;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import flash.geom.Point;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.main.IncomingClanMessage;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionData;
   
   public class IncomingClanMessageBinarySerializer implements §_-D2L§
   {
      
      public function IncomingClanMessageBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:IncomingClanMessage = new IncomingClanMessage();
         param1.readUnsignedShort();
         _loc3_.message = ClanChatMessageTO(§_-616§.§_-fk§(ClanChatMessageTO).deserializeStructure(param1,null));
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return ClanMessages.CHAT_MESSAGE_EVENT;
      }
   }
}

