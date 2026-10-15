package ru.pragmatix.wormix.serialization.server
{
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import flash.events.TimerEvent;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.Timer;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.pvp.messages.server.BattleLoginError;
   import ru.pragmatix.wormix.pvp.messages.server.BattleLoginErrorType;
   
   public class BattleLoginErrorBinarySerializer implements §_-D2L§
   {
      
      public function BattleLoginErrorBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 2004;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:BattleLoginError = new BattleLoginError();
         _loc3_.error = BattleLoginErrorType.valueOf(param1.readShort());
         return _loc3_;
      }
   }
}

