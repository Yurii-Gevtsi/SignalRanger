package ru.pragmatix.wormix.serialization.server
{
   import §_-DJ§.§_-J2l§;
   import §_-j22§.§_-5g§;
   import §_-p24§.*;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.messages.server.LinkResult;
   import ru.pragmatix.wormix.messages.server.WhoPumpedReactionResult;
   import ru.pragmatix.wormix.messages.structures.ProfileDoubleKeyStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import starling.display.DisplayObjectContainer;
   
   public class WhoPumpedReactionResultBinarySerializer implements §_-D2L§
   {
      
      public function WhoPumpedReactionResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10034;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:WhoPumpedReactionResult = WhoPumpedReactionResult(param1);
         var _loc4_:ByteArray = new ByteArray();
         var _loc5_:ByteArray = new ByteArray();
         var _loc6_:§_-5g§ = new §_-5g§();
         _loc6_.§_-c2D§(this.getCommandId());
         var _loc7_:uint = 0;
         var _loc8_:uint = _loc3_.todayPumped.length;
         _loc4_.writeShort(_loc8_);
         _loc7_ = 0;
         while(_loc7_ < _loc8_)
         {
            _loc5_ = new ByteArray();
            §_-616§.§_-fk§(ProfileDoubleKeyStructure).serializeStructure(_loc3_.todayPumped[_loc7_],_loc5_,null);
            _loc4_.writeShort(_loc5_.length);
            _loc4_.writeBytes(_loc5_);
            _loc7_++;
         }
         _loc6_.setLength(_loc4_.length);
         _loc6_.write(param2);
         param2.writeBytes(_loc4_);
         var _loc9_:§_-5g§ = new §_-5g§();
         _loc9_.§_-c2D§(this.getCommandId());
         _loc7_ = 0;
         _loc8_ = _loc3_.yesterdayPumped.length;
         _loc4_.writeShort(_loc8_);
         _loc7_ = 0;
         while(_loc7_ < _loc8_)
         {
            _loc5_ = new ByteArray();
            §_-616§.§_-fk§(ProfileDoubleKeyStructure).serializeStructure(_loc3_.yesterdayPumped[_loc7_],_loc5_,null);
            _loc4_.writeShort(_loc5_.length);
            _loc4_.writeBytes(_loc5_);
            _loc7_++;
         }
         _loc9_.setLength(_loc4_.length);
         _loc9_.write(param2);
         param2.writeBytes(_loc4_);
         var _loc10_:§_-5g§ = new §_-5g§();
         _loc10_.§_-c2D§(this.getCommandId());
         _loc7_ = 0;
         _loc8_ = _loc3_.twoDaysAgoPumped.length;
         _loc4_.writeShort(_loc8_);
         _loc7_ = 0;
         while(_loc7_ < _loc8_)
         {
            _loc5_ = new ByteArray();
            §_-616§.§_-fk§(ProfileDoubleKeyStructure).serializeStructure(_loc3_.twoDaysAgoPumped[_loc7_],_loc5_,null);
            _loc4_.writeShort(_loc5_.length);
            _loc4_.writeBytes(_loc5_);
            _loc7_++;
         }
         _loc10_.setLength(_loc4_.length);
         _loc10_.write(param2);
         param2.writeBytes(_loc4_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:WhoPumpedReactionResult = new WhoPumpedReactionResult();
         var _loc4_:uint = 0;
         _loc3_.todayPumped = new Array();
         var _loc5_:uint = uint(param1.readShort());
         _loc4_ = 0;
         while(_loc4_ < _loc5_)
         {
            param1.readShort();
            _loc3_.todayPumped[_loc4_] = §_-616§.§_-fk§(ProfileDoubleKeyStructure).deserializeStructure(param1,null);
            _loc4_++;
         }
         _loc4_ = 0;
         _loc3_.yesterdayPumped = new Array();
         _loc5_ = uint(param1.readShort());
         _loc4_ = 0;
         while(_loc4_ < _loc5_)
         {
            param1.readShort();
            _loc3_.yesterdayPumped[_loc4_] = §_-616§.§_-fk§(ProfileDoubleKeyStructure).deserializeStructure(param1,null);
            _loc4_++;
         }
         _loc4_ = 0;
         _loc3_.twoDaysAgoPumped = new Array();
         _loc5_ = uint(param1.readShort());
         _loc4_ = 0;
         while(_loc4_ < _loc5_)
         {
            param1.readShort();
            _loc3_.twoDaysAgoPumped[_loc4_] = §_-616§.§_-fk§(ProfileDoubleKeyStructure).deserializeStructure(param1,null);
            _loc4_++;
         }
         return _loc3_;
      }
   }
}

