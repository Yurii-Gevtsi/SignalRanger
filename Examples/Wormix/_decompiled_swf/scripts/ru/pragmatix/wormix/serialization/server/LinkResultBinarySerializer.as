package ru.pragmatix.wormix.serialization.server
{
   import §_-p24§.*;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.messages.server.LinkResult;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import starling.display.DisplayObjectContainer;
   
   public class LinkResultBinarySerializer implements §_-D2L§
   {
      
      public function LinkResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10030;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:LinkResult = new LinkResult();
         var _loc4_:ByteArray = new ByteArray();
         param1.readBytes(_loc4_,0,param1.bytesAvailable);
         _loc3_.resultEnum = _loc4_.readShort();
         _loc3_.userProfileStructure = UserProfileStructure(SerializationUtils.readStructure(_loc4_,UserProfileStructure));
         return _loc3_;
      }
   }
}

