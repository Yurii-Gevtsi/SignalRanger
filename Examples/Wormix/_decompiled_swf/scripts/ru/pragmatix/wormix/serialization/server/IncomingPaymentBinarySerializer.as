package ru.pragmatix.wormix.serialization.server
{
   import §_-31W§.§_-Kb§;
   import §_-3D§.§_-a2b§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-fH§;
   import §_-K3q§.*;
   import §_-N8§.*;
   import §_-Vg§.§_-X7§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import §_-zI§.§_-A1s§;
   import feathers.controls.Button;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.server.IncomingPayment;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import starling.display.Sprite;
   import starling.events.Event;
   
   public class IncomingPaymentBinarySerializer implements §_-D2L§
   {
      
      public function IncomingPaymentBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10031;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:IncomingPayment = IncomingPayment(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeShort(_loc3_.moneyType);
         _loc5_.writeUTF(_loc3_.item);
         _loc5_.writeInt(_loc3_.count);
         SerializationUtils.writeStructureArray(_loc5_,GenericAwardStructure,_loc3_.awards);
         _loc5_.writeUTF(_loc3_.note);
         _loc5_.writeUTF(§_-J2g§.§_-i1N§.§_-yt§());
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ByteArray = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         var _loc4_:IncomingPayment = new IncomingPayment();
         _loc4_.moneyType = _loc3_.readShort();
         _loc4_.item = _loc3_.readUTF();
         _loc4_.count = _loc3_.readInt();
         _loc4_.awards = SerializationUtils.readStructureArray(_loc3_,GenericAwardStructure);
         _loc4_.note = _loc3_.readUTF();
         _loc4_.sessionKey = _loc3_.readUTF();
         var _loc5_:Boolean = §_-U11§.isSecure(_loc3_,§_-6L§.§_-95§);
         if(!_loc5_)
         {
            §_-J2g§.§_-i1N§.dispose();
            §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.PROTOCOL_CHEATER) + " IncomingPayment",§_-fH§.§_-L2I§);
            return null;
         }
         return _loc4_;
      }
   }
}

