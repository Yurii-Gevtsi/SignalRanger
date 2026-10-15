package ru.pragmatix.wormix.serialization.server
{
   import §_-72j§.§_-N19§;
   import §_-SP§.§_-M4§;
   import §_-ZP§.§_-YG§;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.core.*;
   import flash.display.DisplayObject;
   import flash.events.Event;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.server.Pong;
   import ru.pragmatix.wormix.messages.server.VerifyPaymentResult;
   import starling.core.Starling;
   import starling.events.Event;
   
   public class PongBinarySerializer implements §_-D2L§
   {
      
      public function PongBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10017;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:Pong = Pong(param1);
         var _temp_1:* = new §_-5g§();
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         return new Pong();
      }
   }
}

