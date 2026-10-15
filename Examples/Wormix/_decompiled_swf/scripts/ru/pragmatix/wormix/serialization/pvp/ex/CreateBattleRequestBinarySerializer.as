package ru.pragmatix.wormix.serialization.pvp.ex
{
   import §_-21p§.§_-4w§;
   import §_-31W§.§_-r26§;
   import §_-63U§.§_-412§;
   import §_-91B§.§_-z2l§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-Bv§.§_-G3P§;
   import §_-DJ§.§_-fH§;
   import §_-Ly§.§_-81L§;
   import §_-Vg§.ClanCreateView;
   import §_-Y1Q§.§_-aG§;
   import §_-Y1Q§.§_-tc§;
   import §_-fo§.§_-r5§;
   import §_-j1w§.§_-B2i§;
   import §_-j1w§.§_-L1x§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-W2f§;
   import §_-l1K§.§_-m2§;
   import §_-l1g§.§_-L3Q§;
   import §_-n1w§.*;
   import §_-nE§.SelectCraftWeaponScreen;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-u13§.MD2;
   import §_-u13§.MD5;
   import §_-u13§.SHA1;
   import §_-u13§.§_-F3§;
   import §_-w2i§.§_-Zd§;
   import §_-zI§.§_-8D§;
   import §_-zI§.§_-F3q§;
   import §_-zI§.§_-g1a§;
   import com.distriqt.extension.pushnotifications.PushNotifications;
   import com.hurlant.crypto.tls.§_-21v§;
   import com.hurlant.math.*;
   import com.hurlant.util.der.Sequence;
   import com.hurlant.util.der.§_-cz§;
   import com.hurlant.util.der.§_-l1b§;
   import flash.display.MovieClip;
   import flash.geom.Point;
   import flash.system.ApplicationDomain;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.getQualifiedClassName;
   import org.swiftsuspenders.Injector;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.*;
   import ru.pragmatix.wormix.messages.structures.PumpReactionRateStructure;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.pvp.messages.ex.CreateBattleRequest;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseAmmo;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.AirBlocker;
   import ru.pragmatix.wormix.weapons.interfaces.§_-J3w§;
   import starling.events.Event;
   
   use namespace bi_internal;
   
   public class CreateBattleRequestBinarySerializer implements §_-D2L§
   {
      
      public function CreateBattleRequestBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return CreateBattleRequest.ID;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:CreateBattleRequest = CreateBattleRequest(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUnsignedInt(_loc3_.profileId);
         _loc5_.writeUTF(_loc3_.profileSocialId);
         _loc5_.writeByte(_loc3_.socialNetworkId);
         _loc5_.writeUTF(_loc3_.authKey);
         _loc5_.writeUTF(_loc3_.playerName);
         _loc5_.writeShort(_loc3_.mainServerId);
         _loc5_.writeUnsignedInt(_loc3_.mapId);
         _loc5_.writeShort(_loc3_.wager.id);
         SerializationUtils.writeShortVector(_loc5_,_loc3_.missionIds);
         SerializationUtils.writeUintVector(_loc5_,_loc3_.playerIds);
         SerializationUtils.writeByteVector(_loc5_,_loc3_.teamIds);
         _loc5_.writeUTF(JSON.stringify(_loc3_.clientParams));
         _loc5_.writeByte(_loc3_.friendSocialNetId);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         return null;
      }
   }
}

