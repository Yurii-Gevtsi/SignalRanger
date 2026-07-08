package ru.pragmatix.wormix.serialization.server
{
   import §_-D3A§.§_-TZ§;
   import §_-L1H§.§_-b2K§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.server.ClientSettings;
   
   public class ClientSettingsBinarySerializer implements §_-D2L§
   {
      
      public function ClientSettingsBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10095;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ByteArray = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         var _loc4_:ClientSettings = new ClientSettings();
         var _loc5_:uint = _loc3_.readUnsignedShort() / 2;
         var _loc6_:int = 0;
         while(_loc6_ < _loc5_)
         {
            _loc4_.setValue(_loc3_.readUTF(),_loc3_.readUTF());
            _loc6_++;
         }
         return _loc4_;
      }
   }
}

