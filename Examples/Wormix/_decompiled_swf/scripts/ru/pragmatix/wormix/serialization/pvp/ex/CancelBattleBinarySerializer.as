package ru.pragmatix.wormix.serialization.pvp.ex
{
   import §_-11L§.*;
   import §_-63U§.§_-y2O§;
   import §_-931§.§_-sz§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-C2M§.*;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-fH§;
   import §_-Y1O§.§_-yA§;
   import §_-Y1b§.§_-OP§;
   import §_-YF§.§_-q2v§;
   import §_-Z1K§.§_-q24§;
   import §_-aM§.*;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-p2E§.*;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-u1m§.*;
   import feathers.core.ITextRenderer;
   import flash.geom.Point;
   import flash.net.URLRequest;
   import flash.net.navigateToURL;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.DailyBonusStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.pvp.messages.ex.CancelBattle;
   import ru.pragmatix.wormix.utils.§_-MN§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseAmmo;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   public class CancelBattleBinarySerializer implements §_-D2L§
   {
      
      public function CancelBattleBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return CancelBattle.ID;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:CancelBattle = CancelBattle(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUnsignedInt(_loc3_.battleId);
         _loc5_.writeShort(_loc3_.error);
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

