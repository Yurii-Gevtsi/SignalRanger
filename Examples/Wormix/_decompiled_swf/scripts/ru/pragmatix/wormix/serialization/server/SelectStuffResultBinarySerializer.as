package ru.pragmatix.wormix.serialization.server
{
   import §_-62§.§_-G1p§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-rM§.§_-02j§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.messages.server.SelectStuffResult;
   import ru.pragmatix.wormix.messages.structures.SelectStuffResultStructure;
   
   public class SelectStuffResultBinarySerializer implements §_-D2L§
   {
      
      public function SelectStuffResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10022;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc5_:uint = 0;
         var _loc6_:uint = 0;
         var _loc7_:§_-02j§ = null;
         var _loc3_:ByteArray = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         var _loc4_:SelectStuffResult = new SelectStuffResult();
         _loc6_ = uint(_loc3_.readShort());
         _loc4_.selectStuffResults = [];
         _loc5_ = 0;
         while(_loc5_ < _loc6_)
         {
            _loc3_.readShort();
            _loc7_ = §_-616§.§_-fk§(SelectStuffResultStructure);
            _loc4_.selectStuffResults[_loc5_] = _loc7_.deserializeStructure(_loc3_,null);
            _loc5_++;
         }
         _loc4_.isSecure = §_-U11§.isSecure(_loc3_,§_-6L§.§_-95§);
         return _loc4_;
      }
   }
}

