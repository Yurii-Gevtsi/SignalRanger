package ru.pragmatix.wormix.serialization.client
{
   import §_-02q§.§_-4s§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-C3f§.§_-1Y§;
   import §_-F2Q§.GestureEvent;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-E1n§;
   import §_-l1K§.§_-m2§;
   import §_-l1K§.§_-n2T§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-sQ§.§_-H3D§;
   import §_-zI§.§_-X1q§;
   import feathers.controls.Label;
   import flash.events.Event;
   import flash.geom.Point;
   import flash.text.TextField;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.gui.controls.§_-zi§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.client.AppleVerifyReceipt;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.model.GenericPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   public class AppleVerifyReceiptBinarySerializer implements §_-D2L§
   {
      
      public function AppleVerifyReceiptBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:AppleVerifyReceipt = AppleVerifyReceipt(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUTF(_loc3_.productId);
         var _loc6_:uint = _loc3_.receiptData.length;
         _loc3_.receiptData.position = 0;
         _loc5_.writeShort(_loc6_);
         var _loc7_:int = 0;
         while(_loc7_ < _loc6_)
         {
            _loc5_.writeByte(_loc3_.receiptData.readByte());
            _loc7_++;
         }
         _loc5_.writeBoolean(_loc3_.isSubscription);
         _loc5_.writeUTF(_loc3_.sessionKey);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:AppleVerifyReceipt = new AppleVerifyReceipt();
         _loc3_.productId = param1.readUTF();
         var _loc4_:uint = uint(param1.readShort());
         param1.readBytes(_loc3_.receiptData,0,_loc4_);
         _loc3_.isSubscription = param1.readBoolean();
         _loc3_.sessionKey = param1.readUTF();
         _loc3_.secureResult = param1 is ByteArray && §_-U11§.isSecure(param1 as ByteArray,§_-6L§.§_-95§);
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return 7;
      }
   }
}

