package ru.pragmatix.wormix.serialization.clans.response.edit
{
   import §_-91B§.§_-QK§;
   import §_-A1K§.§_-TA§;
   import §_-CF§.§_-p2U§;
   import §_-H2g§.§_-di§;
   import §_-T1o§.§_-n1S§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-b1k§;
   import §_-l1K§.§_-cZ§;
   import §_-l2r§.§_-K1t§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-z1g§.*;
   import §_-zI§.*;
   import dragonBones.animation.WorldClock;
   import flash.geom.Point;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.response.edit.RenameClanResponse;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   public class RenameClanResponseBinarySerializer implements §_-D2L§
   {
      
      public function RenameClanResponseBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:RenameClanResponse = new RenameClanResponse();
         _loc3_.serviceResult = param1.readShort();
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return ClanMessages.RENAME_CLAN_RESPONSE;
      }
   }
}

