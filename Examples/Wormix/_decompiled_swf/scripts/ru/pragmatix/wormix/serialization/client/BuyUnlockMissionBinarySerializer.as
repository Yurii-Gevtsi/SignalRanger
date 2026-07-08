package ru.pragmatix.wormix.serialization.client
{
   import §_-A1K§.§_-TA§;
   import §_-B2z§.§_-63i§;
   import §_-P2i§.§_-G3J§;
   import §_-Tm§.§_-RF§;
   import §_-X14§.*;
   import §_-j22§.§_-5g§;
   import §_-jF§.*;
   import §_-l1K§.§_-N1Z§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import feathers.controls.List;
   import feathers.controls.TabBar;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import flash.events.Event;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.messages.client.BuyUnlockMission;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.objects.§_-V1L§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.pvp.messages.client.PvpActionEx;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurn;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreationFailure;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattle;
   import ru.pragmatix.wormix.pvp.messages.server.BattleLoginError;
   import ru.pragmatix.wormix.pvp.messages.server.PvpEndBattle;
   import ru.pragmatix.wormix.pvp.messages.server.PvpRetryCommandRequestServer;
   import ru.pragmatix.wormix.pvp.messages.server.PvpStartTurn;
   import ru.pragmatix.wormix.pvp.messages.server.PvpSystemMessage;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.interfaces.§_-O1c§;
   import starling.animation.Tween;
   import starling.core.Starling;
   import starling.display.Image;
   import starling.events.Event;
   import starling.events.Touch;
   import starling.events.TouchEvent;
   import starling.events.TouchPhase;
   
   public class BuyUnlockMissionBinarySerializer implements §_-D2L§
   {
      
      public function BuyUnlockMissionBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 58;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:BuyUnlockMission = BuyUnlockMission(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeShort(_loc3_.missionId);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:BuyUnlockMission = new BuyUnlockMission();
         _loc3_.missionId = param1.readShort();
         return _loc3_;
      }
   }
}

