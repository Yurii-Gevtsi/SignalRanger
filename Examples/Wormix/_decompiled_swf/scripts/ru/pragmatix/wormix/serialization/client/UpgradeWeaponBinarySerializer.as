package ru.pragmatix.wormix.serialization.client
{
   import §_-12a§.KamikazeBuff;
   import §_-21p§.§_-4w§;
   import §_-A2U§.§_-A23§;
   import §_-B1y§.§_-F1P§;
   import §_-CF§.§_-x2t§;
   import §_-DJ§.§_-fH§;
   import §_-F39§.§_-a2W§;
   import §_-P1B§.§_-b21§;
   import §_-P1B§.§_-bb§;
   import §_-YF§.§_-q2v§;
   import §_-j22§.§_-5g§;
   import §_-r1C§.*;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-zI§.§_-8D§;
   import §_-zI§.§_-X1q§;
   import feathers.data.ListCollection;
   import flash.events.Event;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.logger.§_-93v§;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-G1F§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanMemberStructure;
   import ru.pragmatix.wormix.messages.clans.request.TopClansRequest;
   import ru.pragmatix.wormix.messages.clans.response.DeleteClanResponse;
   import ru.pragmatix.wormix.messages.clans.response.TopClansResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.UpgradeWeapon;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-72u§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.utils.Pool;
   
   public class UpgradeWeaponBinarySerializer implements §_-D2L§
   {
      
      public function UpgradeWeaponBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:UpgradeWeapon = UpgradeWeapon(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeShort(_loc3_.recipeId);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:UpgradeWeapon = new UpgradeWeapon();
         _loc3_.recipeId = param1.readShort();
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return 86;
      }
   }
}

