package ru.pragmatix.wormix.serialization.server
{
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.events.InAppBillingEvent;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.messages.server.OpenDepositResult;
   import ru.pragmatix.wormix.messages.structures.DepositStructure;
   import starling.display.DisplayObject;
   
   public class OpenDepositResultBinarySerializer implements §_-D2L§
   {
      
      public function OpenDepositResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10132;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:OpenDepositResult = new OpenDepositResult();
         param1.readShort();
         _loc3_.deposit = DepositStructure(§_-616§.§_-fk§(DepositStructure).deserializeStructure(param1,null));
         _loc3_.firstPart = param1.readInt();
         _loc3_.sessionKey = param1.readUTF();
         return _loc3_;
      }
   }
}

