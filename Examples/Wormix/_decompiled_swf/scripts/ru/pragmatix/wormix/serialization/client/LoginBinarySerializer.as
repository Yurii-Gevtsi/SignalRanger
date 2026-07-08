package ru.pragmatix.wormix.serialization.client
{
   import §_-62§.*;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-DJ§.§_-J2l§;
   import §_-e1e§.§_-CG§;
   import §_-j1w§.§_-lD§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-sQ§.§_-H3D§;
   import feathers.events.FeathersEventType;
   import flash.events.Event;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.logger.§_-11S§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.client.Login;
   import starling.events.Event;
   import starling.utils.SystemUtil;
   
   public class LoginBinarySerializer implements §_-D2L§
   {
      
      public function LoginBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 1;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:Login = Login(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUnsignedInt(_loc3_.id);
         _loc5_.writeUnsignedInt(_loc3_.referrerId);
         _loc5_.writeUTF(_loc3_.authKey);
         var _loc6_:uint = _loc3_.ids.length;
         _loc5_.writeShort(_loc6_);
         var _loc7_:int = 0;
         while(_loc7_ < _loc6_)
         {
            _loc5_.writeUnsignedInt(_loc3_.ids[_loc7_]);
            _loc7_++;
         }
         _loc5_.writeByte(_loc3_.socialCode);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:Login = new Login();
         _loc3_.id = param1.readUnsignedInt();
         _loc3_.referrerId = param1.readUnsignedInt();
         _loc3_.authKey = param1.readUTF();
         _loc3_.ids = [];
         var _loc4_:uint = uint(param1.readShort());
         var _loc5_:int = 0;
         while(_loc5_ < _loc4_)
         {
            _loc3_.ids[_loc5_] = param1.readUnsignedInt();
            _loc5_++;
         }
         _loc3_.socialCode = param1.readByte();
         return _loc3_;
      }
   }
}

