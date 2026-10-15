package ru.pragmatix.wormix.serialization.pvp.ex
{
   import §_-12W§.*;
   import §_-62§.§_-zF§;
   import §_-79§.*;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-A2U§.*;
   import §_-CF§.§_-p2U§;
   import §_-Cl§.*;
   import §_-DJ§.§_-fH§;
   import §_-F2Q§.GestureEvent;
   import §_-Y1b§.§_-OP§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-E1n§;
   import §_-l1K§.§_-m2§;
   import §_-l1K§.§_-n2T§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-u1m§.*;
   import §_-y1s§.§_-p6§;
   import feathers.controls.List;
   import feathers.controls.ScrollPolicy;
   import feathers.controls.TabBar;
   import feathers.data.ListCollection;
   import feathers.layout.HorizontalLayout;
   import flash.errors.IllegalOperationError;
   import flash.events.TimerEvent;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.screenOverlay.view.§_-B1m§;
   import ru.pragmatix.wormix.messages.clans.ClanServiceResult;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.pvp.messages.ex.ReconnectToBattle;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.Bullet;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseAmmo;
   import starling.events.Event;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   public class ReconnectToBattleBinarySerializer implements §_-D2L§
   {
      
      public function ReconnectToBattleBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return ReconnectToBattle.ID;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:ReconnectToBattle = ReconnectToBattle(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUnsignedInt(_loc3_.profileId);
         _loc5_.writeUTF(_loc3_.profileSocialId);
         _loc5_.writeByte(_loc3_.socialNetworkId);
         _loc5_.writeUTF(_loc3_.authKey);
         _loc5_.writeUTF(_loc3_.playerName);
         _loc5_.writeShort(_loc3_.mainServerId);
         _loc5_.writeUnsignedInt(_loc3_.battleId);
         _loc5_.writeShort(_loc3_.turnNum);
         _loc5_.writeByte(_loc3_.playerNum);
         _loc5_.writeShort(_loc3_.lastCommandNum);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         return {};
      }
   }
}

