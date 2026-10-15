package ru.pragmatix.wormix.serialization.server
{
   import §_-Y1O§.§_-42h§;
   import §_-Y1O§.§_-V23§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.messages.server.ProfileRestrictions;
   import ru.pragmatix.wormix.messages.structures.RestrictionItemStructure;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import starling.events.Event;
   
   public class ProfileRestrictionsBinarySerializer implements §_-D2L§
   {
      
      public function ProfileRestrictionsBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10138;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ProfileRestrictions = new ProfileRestrictions();
         _loc3_.restrictions = SerializationUtils.readStructureArray(param1 as ByteArray,RestrictionItemStructure);
         return _loc3_;
      }
   }
}

