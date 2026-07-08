package ru.pragmatix.wormix.serialization.server
{
   import §_-B1y§.§_-83E§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.*;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-d21§;
   import §_-zI§.*;
   import flash.events.Event;
   import flash.events.IOErrorEvent;
   import flash.events.SecurityErrorEvent;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.messages.client.SteamTransactionInitRequest;
   import ru.pragmatix.wormix.messages.server.ArenaLocked;
   import ru.pragmatix.wormix.messages.server.SteamTransactionInitResponse;
   import ru.pragmatix.wormix.objects.§_-01J§;
   
   public class ArenaLockedBinarySerializer implements §_-D2L§
   {
      
      public function ArenaLockedBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10007;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:ArenaLocked = ArenaLocked(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUnsignedInt(_loc3_.delay);
         _loc5_.writeShort(_loc3_.currentMission);
         _loc5_.writeShort(_loc3_.errorCode);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ArenaLocked = null;
         _loc3_ = new ArenaLocked();
         _loc3_.delay = param1.readUnsignedInt();
         _loc3_.currentMission = param1.readShort();
         _loc3_.errorCode = param1.readShort();
         return _loc3_;
      }
   }
}

