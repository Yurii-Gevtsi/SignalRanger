package ru.pragmatix.wormix.serialization.server
{
   import §_-A2U§.§_-A23§;
   import §_-DJ§.§_-fH§;
   import §_-L1w§.§_-CD§;
   import §_-l1K§.§_-W2f§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-sQ§.§_-H3D§;
   import flash.geom.ColorTransform;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.gui.controls.MessageBox;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanServiceResult;
   import ru.pragmatix.wormix.messages.clans.response.treasury.DonateClanResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.server.RepeatLevelUpAwardResponse;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.§_-6A§;
   import ru.pragmatix.wormix.utils.§_-MN§;
   import starling.core.Starling;
   import starling.display.Canvas;
   import starling.events.Event;
   import starling.utils.Align;
   
   public class RepeatLevelUpAwardResponseBinarySerializer implements §_-D2L§
   {
      
      public function RepeatLevelUpAwardResponseBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10141;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:RepeatLevelUpAwardResponse = new RepeatLevelUpAwardResponse();
         _loc3_.status = param1.readShort();
         _loc3_.level = param1.readInt();
         _loc3_.awards = SerializationUtils.readStructureArray(param1 as ByteArray,GenericAwardStructure);
         return _loc3_;
      }
   }
}

