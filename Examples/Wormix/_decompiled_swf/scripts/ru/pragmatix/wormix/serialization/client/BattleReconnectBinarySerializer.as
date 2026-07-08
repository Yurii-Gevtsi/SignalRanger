package ru.pragmatix.wormix.serialization.client
{
   import §_-12W§.*;
   import §_-73I§.§_-Y1l§;
   import §_-A2U§.*;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-fo§.§_-r5§;
   import §_-j22§.§_-5g§;
   import §_-nE§.SelectCraftWeaponScreen;
   import §_-nE§.§_-p16§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-y1s§.§_-p6§;
   import §_-zI§.§_-X1q§;
   import feathers.controls.List;
   import feathers.controls.ScrollPolicy;
   import feathers.data.ListCollection;
   import feathers.layout.HorizontalLayout;
   import flash.errors.IllegalOperationError;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.gui.controls.§_-zi§;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.controller.mobile.BaseMobileSocialController;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.screenOverlay.view.§_-B1m§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.client.BattleReconnect;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.Bullet;
   import starling.events.Event;
   
   public class BattleReconnectBinarySerializer implements §_-D2L§
   {
      
      public function BattleReconnectBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 1015;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:BattleReconnect = BattleReconnect(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUnsignedInt(_loc3_.id);
         _loc5_.writeUTF(_loc3_.authKey);
         _loc5_.writeUTF(_loc3_.mainHost);
         _loc5_.writeInt(_loc3_.mainPort);
         _loc5_.writeUnsignedInt(_loc3_.battleId);
         _loc5_.writeShort(_loc3_.turnNum);
         _loc5_.writeByte(_loc3_.playerNum);
         _loc5_.writeShort(_loc3_.lastCommandNum);
         _loc5_.writeByte(_loc3_.socialId);
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

