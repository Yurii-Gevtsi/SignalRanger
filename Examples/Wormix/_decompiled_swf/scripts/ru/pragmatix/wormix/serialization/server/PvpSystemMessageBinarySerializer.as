package ru.pragmatix.wormix.serialization.server
{
   import §_-31N§.§_-42G§;
   import §_-j22§.§_-5g§;
   import §_-kP§.UpgradeWeaponCell;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-zI§.§_-sI§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.messages.server.RemoveFromGroupResult;
   import ru.pragmatix.wormix.pvp.messages.server.PvpSystemMessage;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import starling.animation.Tween;
   import starling.core.Starling;
   import starling.events.Event;
   
   public class PvpSystemMessageBinarySerializer implements §_-D2L§
   {
      
      public function PvpSystemMessageBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 2021;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:PvpSystemMessage = new PvpSystemMessage();
         _loc3_.type = param1.readShort();
         _loc3_.playerNum = param1.readByte();
         return _loc3_;
      }
   }
}

