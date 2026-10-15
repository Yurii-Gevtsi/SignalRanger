package ru.pragmatix.wormix.serialization.server
{
   import §_-21p§.§_-N1H§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-gG§.§_-73L§;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-w2W§.§_-21w§;
   import dragonBones.animation.WorldClock;
   import feathers.core.ToggleGroup;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.screenOverlay.view.*;
   import ru.pragmatix.wormix.messages.server.ChooseBonusItemResult;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   public class ChooseBonusItemResultBinarySerializer implements §_-D2L§
   {
      
      public function ChooseBonusItemResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 13006;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:ChooseBonusItemResult = ChooseBonusItemResult(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeShort(_loc3_.result);
         _loc5_.writeInt(_loc3_.itemId);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ChooseBonusItemResult = new ChooseBonusItemResult();
         var _loc4_:ByteArray = new ByteArray();
         param1.readBytes(_loc4_,0,param1.bytesAvailable);
         _loc3_.result = _loc4_.readShort();
         _loc3_.itemId = _loc4_.readInt();
         _loc3_.isSecure = §_-U11§.isSecure(_loc4_,§_-6L§.§_-95§);
         return _loc3_;
      }
   }
}

