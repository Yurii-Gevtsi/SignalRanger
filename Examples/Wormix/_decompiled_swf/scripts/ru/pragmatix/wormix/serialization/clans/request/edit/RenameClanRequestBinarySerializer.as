package ru.pragmatix.wormix.serialization.clans.request.edit
{
   import §_-62§.§_-L1r§;
   import §_-B1y§.§_-bs§;
   import §_-C3f§.§_-1Y§;
   import §_-CF§.§_-x2t§;
   import §_-DJ§.§_-fH§;
   import §_-E1I§.*;
   import §_-F39§.*;
   import §_-HD§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-zI§.§_-8D§;
   import feathers.controls.NumericStepper;
   import feathers.layout.RelativePosition;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.AnimationStatesRenderer;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.§_-s1V§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanInviteStructure;
   import ru.pragmatix.wormix.messages.clans.request.edit.RenameClanRequest;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.serialization.BaseCommandSerializer;
   import ru.pragmatix.wormix.utils.StringUtil;
   import starling.display.DisplayObjectContainer;
   import starling.display.Sprite;
   import starling.events.Event;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   public class RenameClanRequestBinarySerializer extends BaseCommandSerializer
   {
      
      public function RenameClanRequestBinarySerializer()
      {
         super();
      }
      
      override protected function §_-41X§(param1:Object) : ByteArray
      {
         var _loc2_:RenameClanRequest = RenameClanRequest(param1);
         var _loc3_:ByteArray = new ByteArray();
         _loc3_.writeUTF(_loc2_.name);
         _loc3_.writeBoolean(_loc2_.fromTreas);
         return _loc3_;
      }
      
      override public function getCommandId() : uint
      {
         return ClanMessages.RENAME_CLAN_REQUEST;
      }
   }
}

