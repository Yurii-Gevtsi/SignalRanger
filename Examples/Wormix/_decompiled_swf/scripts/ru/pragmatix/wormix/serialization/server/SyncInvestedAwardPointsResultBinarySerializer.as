package ru.pragmatix.wormix.serialization.server
{
   import §_-Y1O§.*;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-x2Q§.§_-dU§;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.server.SyncInvestedAwardPointsResult;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import starling.events.Event;
   
   public class SyncInvestedAwardPointsResultBinarySerializer implements §_-D2L§
   {
      
      public function SyncInvestedAwardPointsResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10041;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:SyncInvestedAwardPointsResult = new SyncInvestedAwardPointsResult();
         _loc3_.points = param1.readByte();
         return _loc3_;
      }
   }
}

