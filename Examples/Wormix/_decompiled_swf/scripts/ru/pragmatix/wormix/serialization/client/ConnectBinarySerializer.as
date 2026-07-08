package ru.pragmatix.wormix.serialization.client
{
   import §_-5Y§.§_-h2c§;
   import §_-931§.*;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.§_-F1P§;
   import §_-D3A§.§_-TZ§;
   import §_-G16§.*;
   import §_-b1§.*;
   import §_-d26§.§_-d1H§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-z1g§.§_-F4§;
   import flash.display.DisplayObject;
   import flash.display.DisplayObjectContainer;
   import flash.display.InteractiveObject;
   import flash.display.Stage;
   import flash.geom.Point;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.client.Connect;
   import ru.pragmatix.wormix.weapons.Girder;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class ConnectBinarySerializer implements §_-D2L§
   {
      
      public function ConnectBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 27;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc7_:uint = 0;
         var _loc3_:Connect = Connect(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUTF(_loc3_.socialProfileId);
         _loc5_.writeUTF(_loc3_.authKey);
         var _loc6_:uint = 0;
         if(!_loc3_.ids)
         {
            _loc5_.writeShort(0);
         }
         else
         {
            _loc7_ = _loc3_.ids.length;
            _loc5_.writeShort(_loc7_);
            _loc6_ = 0;
            while(_loc6_ < _loc7_)
            {
               _loc5_.writeUTF(_loc3_.ids[_loc6_]);
               _loc6_++;
            }
         }
         _loc5_.writeShort(_loc3_.socialNetId);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:Connect = new Connect();
         _loc3_.socialProfileId = param1.readUTF();
         _loc3_.authKey = param1.readUTF();
         var _loc4_:uint = 0;
         _loc3_.ids = new Array();
         var _loc5_:uint = uint(param1.readShort());
         _loc4_ = 0;
         while(_loc4_ < _loc5_)
         {
            _loc3_.ids[_loc4_] = param1.readUTF();
            _loc4_++;
         }
         _loc3_.socialNetId = param1.readByte();
         _loc3_.isSecure = param1 is ByteArray && §_-U11§.isSecure(param1 as ByteArray,§_-6L§.§_-95§);
         return _loc3_;
      }
   }
}

