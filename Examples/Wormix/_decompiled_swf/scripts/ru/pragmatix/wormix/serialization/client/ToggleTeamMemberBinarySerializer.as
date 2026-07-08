package ru.pragmatix.wormix.serialization.client
{
   import §_-23P§.*;
   import §_-82a§.*;
   import §_-Cl§.§_-L2z§;
   import §_-Cl§.§_-li§;
   import §_-DJ§.§_-J2l§;
   import §_-YF§.§_-q2v§;
   import §_-j1w§.§_-L1x§;
   import §_-j1y§.§_-i1b§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.*;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-sQ§.§_-H3D§;
   import §_-t1J§.FastMissionCard;
   import §_-t1J§.HotSeatCard;
   import §_-t1J§.§_-B2R§;
   import com.hurlant.math.BigInteger;
   import com.hurlant.math.bi_internal;
   import feathers.controls.Button;
   import flash.display.DisplayObject;
   import flash.display.DisplayObjectContainer;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.clearInterval;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.client.ToggleTeamMember;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-i14§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.§_-6A§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.Girder;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseAmmo;
   import starling.display.DisplayObject;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.utils.MathUtil;
   import starling.utils.Pool;
   
   use namespace bi_internal;
   
   public class ToggleTeamMemberBinarySerializer implements §_-D2L§
   {
      
      public function ToggleTeamMemberBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:ToggleTeamMember = ToggleTeamMember(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeInt(_loc3_.teamMemberId);
         _loc5_.writeBoolean(_loc3_.active);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         return null;
      }
      
      public function getCommandId() : uint
      {
         return 106;
      }
   }
}

