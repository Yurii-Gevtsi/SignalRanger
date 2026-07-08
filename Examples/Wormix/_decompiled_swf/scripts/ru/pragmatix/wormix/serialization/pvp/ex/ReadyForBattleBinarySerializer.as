package ru.pragmatix.wormix.serialization.pvp.ex
{
   import §_-2U§.*;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-H2g§.§_-di§;
   import §_-J31§.§_-e2J§;
   import §_-RE§.§_-52p§;
   import §_-U1i§.*;
   import §_-Vg§.*;
   import §_-Y1O§.*;
   import §_-Z1w§.§_-s1x§;
   import §_-j1w§.§_-B2i§;
   import §_-j22§.§_-5g§;
   import §_-k2s§.§_-e1k§;
   import §_-l2r§.§_-K1t§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-z1g§.§_-lp§;
   import com.greensock.events.TweenEvent;
   import com.hurlant.util.der.*;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Button;
   import feathers.events.FeathersEventType;
   import feathers.themes.styles.AlternateTextStyles;
   import flash.errors.IllegalOperationError;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.request.PostToClanChatRequest;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.client.GetDividend;
   import ru.pragmatix.wormix.messages.server.BuyRaceResult;
   import ru.pragmatix.wormix.messages.server.RepeatPvpBattleAwardResult;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.CostStructure;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.ex.ReadyForBattle;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.weapons.ammo.LaserLineAmmoController;
   import starling.events.Event;
   
   public class ReadyForBattleBinarySerializer implements §_-D2L§
   {
      
      public function ReadyForBattleBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return ReadyForBattle.ID;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:ReadyForBattle = ReadyForBattle(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
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

