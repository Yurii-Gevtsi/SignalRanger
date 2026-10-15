package ru.pragmatix.wormix.serialization.server
{
   import §_-62§.*;
   import §_-73d§.§_-x2C§;
   import §_-Y1O§.*;
   import §_-l1K§.§_-d2j§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.messages.server.GetFriendsForBossResult;
   import ru.pragmatix.wormix.messages.structures.SimpleProfileStructure;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   
   public class GetFriendsForBossResultBinarySerializer implements §_-D2L§
   {
      
      public function GetFriendsForBossResultBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:GetFriendsForBossResult = new GetFriendsForBossResult();
         var _loc4_:ByteArray = new ByteArray();
         param1.readBytes(_loc4_,0,param1.bytesAvailable);
         _loc3_.profiles = SerializationUtils.readStructureArray(_loc4_,SimpleProfileStructure);
         _loc3_.states = SerializationUtils.readShortVector(_loc4_);
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return 1093;
      }
   }
}

