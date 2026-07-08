package ru.pragmatix.wormix.serialization.pvp.ex
{
   import §_-5Y§.§_-h2c§;
   import §_-62§.§_-W1r§;
   import §_-63U§.§_-NV§;
   import §_-82l§.§_-A2Y§;
   import §_-9§.§_-52A§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-CF§.§_-p2U§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-fH§;
   import §_-J31§.§_-R1h§;
   import §_-J31§.§_-e2J§;
   import §_-X14§.*;
   import §_-Z1w§.§_-s1x§;
   import §_-aM§.*;
   import §_-b1F§.§_-v2Q§;
   import §_-hO§.§_-v24§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-l1g§.§_-Rs§;
   import §_-lh§.§_-z2A§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-w2i§.§_-Ia§;
   import §_-x2Q§.§_-dU§;
   import §_-z1g§.*;
   import §_-zI§.§_-A1s§;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.plugins.*;
   import com.hurlant.crypto.tls.§_-21v§;
   import feathers.controls.ScrollPolicy;
   import feathers.data.VectorCollection;
   import flash.display.*;
   import flash.errors.IllegalOperationError;
   import flash.events.*;
   import flash.system.LoaderContext;
   import flash.utils.*;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.clans.chat.*;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanInviteStructure;
   import ru.pragmatix.wormix.messages.server.ChatMessageEvent;
   import ru.pragmatix.wormix.messages.structures.ChatMessage;
   import ru.pragmatix.wormix.messages.structures.CostStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.rewards.§_-qU§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.pvp.messages.ex.*;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.phase.HarmPhase;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   
   public class JoinToBattleBinarySerializer implements §_-D2L§
   {
      
      public function JoinToBattleBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return JoinToBattle.ID;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:JoinToBattle = JoinToBattle(param1);
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

