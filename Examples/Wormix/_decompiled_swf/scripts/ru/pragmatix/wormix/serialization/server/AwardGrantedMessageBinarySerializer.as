package ru.pragmatix.wormix.serialization.server
{
   import §_-C3f§.§_-4I§;
   import §_-Vg§.*;
   import §_-n1w§.*;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.request.PostToClanChatRequest;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.server.AwardGrantedMessage;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   
   public class AwardGrantedMessageBinarySerializer implements §_-D2L§
   {
      
      public function AwardGrantedMessageBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10094;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:AwardGrantedMessage = new AwardGrantedMessage();
         var _loc4_:int = int(param1.readUnsignedShort());
         _loc3_.awards = [];
         var _loc5_:int = 0;
         while(_loc5_ < _loc4_)
         {
            param1.readShort();
            _loc3_.awards[_loc5_] = §_-616§.§_-fk§(GenericAwardStructure).deserializeStructure(param1,null) as GenericAwardStructure;
            _loc5_++;
         }
         _loc3_.awardType = param1.readShort();
         _loc3_.attach = param1.readUTF();
         return _loc3_;
      }
   }
}

