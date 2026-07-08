package ru.pragmatix.wormix.serialization.wager
{
   import §_-12W§.*;
   import §_-21p§.§_-4w§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-Af§.*;
   import §_-CF§.§_-E19§;
   import §_-DJ§.§_-J2l§;
   import §_-K3r§.§_-E3O§;
   import §_-K3r§.§_-U2b§;
   import §_-b1F§.§_-G4§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-T2N§;
   import §_-q26§.*;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-R1P§;
   import §_-sQ§.§_-V1n§;
   import com.distriqt.extension.inappbilling.InAppBillingAvailability;
   import com.distriqt.extension.inappbilling.events.AvailabilityEvent;
   import feathers.controls.ScreenNavigator;
   import flash.events.Event;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.getTimer;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.§_-3m§;
   import ru.pragmatix.wormix.gui.screenOverlay.§_-O5§;
   import ru.pragmatix.wormix.messages.clans.ClanServiceResult;
   import ru.pragmatix.wormix.messages.clans.request.DeleteClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.QuitClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginCreateClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginJoinClanRequest;
   import ru.pragmatix.wormix.messages.client.EndBattle;
   import ru.pragmatix.wormix.messages.structures.SelectStuffResultStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.RopePhysicModel;
   import ru.pragmatix.wormix.pvp.messages.wager.WagerCancelBattle;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   public class WagerCancelBattleBinarySerializer implements §_-D2L§
   {
      
      public function WagerCancelBattleBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 2007;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:WagerCancelBattle = WagerCancelBattle(param1);
         var _temp_1:* = new §_-5g§();
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         return new WagerCancelBattle();
      }
   }
}

