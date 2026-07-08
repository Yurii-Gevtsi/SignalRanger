package ru.pragmatix.wormix.serialization.server
{
   import §_-T2V§.§_-13F§;
   import §_-b1F§.§_-v2Q§;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.messages.server.ProfilesResult;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.events.Event;
   
   public class ProfilesResultBinarySerializer implements §_-D2L§
   {
      
      public function ProfilesResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10005;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc8_:ByteArray = null;
         var _loc3_:ProfilesResult = ProfilesResult(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         var _loc6_:uint = 0;
         var _loc7_:uint = _loc3_.userProfileStructures.length;
         _loc5_.writeShort(_loc7_);
         _loc6_ = 0;
         while(_loc6_ < _loc7_)
         {
            _loc8_ = new ByteArray();
            §_-616§.§_-fk§(UserProfileStructure).serializeStructure(_loc3_.userProfileStructures[_loc6_],_loc8_,null);
            _loc5_.writeShort(_loc8_.length);
            _loc5_.writeBytes(_loc8_);
            _loc6_++;
         }
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ProfilesResult = new ProfilesResult();
         var _loc4_:uint = 0;
         var _loc5_:uint = param1.readUnsignedShort();
         _loc3_.userProfileStructures = new Array();
         _loc4_ = 0;
         while(_loc4_ < _loc5_)
         {
            param1.readShort();
            _loc3_.userProfileStructures[_loc4_] = §_-616§.§_-fk§(UserProfileStructure).deserializeStructure(param1,null);
            _loc4_++;
         }
         return _loc3_;
      }
   }
}

