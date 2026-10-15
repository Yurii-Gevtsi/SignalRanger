package ru.pragmatix.wormix.serialization.server
{
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-DJ§.§_-fH§;
   import §_-L1H§.§_-b2K§;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.server.ShopResult;
   import ru.pragmatix.wormix.messages.structures.ShopItemStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.weapons.*;
   
   public class ShopResultBinarySerializer implements §_-D2L§
   {
      
      public function ShopResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10003;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:ShopResult = ShopResult(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         var _loc6_:ByteArray = new ByteArray();
         _loc5_.writeShort(_loc3_.result);
         var _loc7_:uint = _loc3_.weapons.length;
         _loc5_.writeShort(_loc7_);
         var _loc8_:int = 0;
         while(_loc8_ < _loc7_)
         {
            _loc6_ = new ByteArray();
            §_-616§.§_-fk§(ShopItemStructure).serializeStructure(_loc3_.weapons[_loc8_],_loc6_,null);
            _loc5_.writeShort(_loc6_.length);
            _loc5_.writeBytes(_loc6_);
            _loc8_++;
         }
         _loc7_ = _loc3_.stuff.length;
         _loc5_.writeShort(_loc7_);
         _loc7_ = _loc3_.temporalStuff.length;
         _loc5_.writeShort(_loc7_);
         var _loc9_:int = 0;
         while(_loc9_ < _loc7_)
         {
            _loc6_ = new ByteArray();
            §_-616§.§_-fk§(ShopItemStructure).serializeStructure(_loc3_.stuff[_loc9_],_loc6_,null);
            _loc5_.writeShort(_loc6_.length);
            _loc5_.writeBytes(_loc6_);
            _loc9_++;
         }
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ByteArray = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         var _loc4_:ShopResult = new ShopResult();
         _loc4_.result = _loc3_.readShort();
         var _loc5_:uint = uint(_loc3_.readShort());
         var _loc6_:uint = 0;
         while(_loc6_ < _loc5_)
         {
            _loc3_.readShort();
            _loc4_.weapons[_loc6_] = §_-616§.§_-fk§(WeaponStructure).deserializeStructure(_loc3_,null);
            _loc6_++;
         }
         var _loc7_:int = _loc3_.readShort();
         var _loc8_:uint = 0;
         while(_loc8_ < _loc7_)
         {
            _loc4_.stuff[_loc8_] = _loc3_.readShort();
            _loc8_++;
         }
         var _loc9_:int = _loc3_.readShort();
         var _loc10_:uint = 0;
         while(_loc10_ < _loc9_)
         {
            _loc4_.temporalStuff[_loc10_] = _loc3_.readShort();
            _loc10_++;
         }
         var _loc11_:Boolean = §_-U11§.isSecure(_loc3_,§_-6L§.§_-95§);
         if(!_loc11_)
         {
            §_-J2g§.§_-i1N§.dispose();
            §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.PROTOCOL_CHEATER),§_-fH§.§_-L2I§);
         }
         return _loc4_;
      }
   }
}

