package ru.pragmatix.wormix.serialization.client
{
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-z1g§.§_-4l§;
   import §_-z1g§.§_-522§;
   import dragonBones.Slot;
   import feathers.controls.ImageLoader;
   import feathers.controls.TabBar;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import org.gestouch.core.§_-H3d§;
   import org.gestouch.core.gestouch_internal;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.client.CrashLog;
   import ru.pragmatix.wormix.weapons.Girder;
   import starling.display.DisplayObject;
   import starling.display.Image;
   import starling.events.Event;
   import starling.textures.Texture;
   
   public class CrashLogBinarySerializer implements §_-D2L§
   {
      
      public function CrashLogBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 138;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:CrashLog = CrashLog(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUTF(_loc3_.platform);
         _loc5_.writeUTF(_loc3_.userId);
         _loc5_.writeUTF(_loc3_.log);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         return null;
      }
   }
}

