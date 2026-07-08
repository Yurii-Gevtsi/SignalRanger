package ru.pragmatix.wormix.serialization.structures
{
   import §_-21A§.*;
   import §_-82l§.§_-A2Y§;
   import §_-RE§.§_-52p§;
   import §_-l1K§.§_-d2j§;
   import §_-m2s§.*;
   import §_-o14§.*;
   import §_-rM§.§_-02j§;
   import com.hurlant.crypto.tls.§_-21v§;
   import com.hurlant.util.§_-F2L§;
   import feathers.core.ToggleGroup;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.getDefinitionByName;
   import ru.pragmatix.wormix.messages.structures.ProfileDoubleKeyStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.utils.§_-g20§;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class ProfileDoubleKeyStructureBinarySerializer implements §_-02j§
   {
      
      public function ProfileDoubleKeyStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
         var _loc4_:ProfileDoubleKeyStructure = ProfileDoubleKeyStructure(param1);
         param2.writeUnsignedInt(_loc4_.longId);
         param2.writeUTF(UserProfileStructure.getSocialIdsAsString(_loc4_.socialIds));
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:ProfileDoubleKeyStructure = new ProfileDoubleKeyStructure();
         _loc3_.longId = param1.readUnsignedInt();
         _loc3_.socialIds = UserProfileStructure.parseSocialIdsString(param1.readUTF());
         return _loc3_;
      }
   }
}

