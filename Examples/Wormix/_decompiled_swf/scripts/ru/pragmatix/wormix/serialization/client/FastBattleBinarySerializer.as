package ru.pragmatix.wormix.serialization.client
{
   import §_-73I§.§_-q15§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-A2U§.*;
   import §_-G2H§.*;
   import §_-a29§.*;
   import §_-fo§.§_-21L§;
   import §_-fo§.§_-r5§;
   import §_-j22§.§_-5g§;
   import §_-k1u§.§_-2m§;
   import §_-l2r§.§_-K1t§;
   import §_-qH§.§_-AC§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-u1T§.*;
   import §_-z1g§.§_-lp§;
   import com.hurlant.crypto.tls.§_-21v§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.LayoutGroup;
   import flash.geom.*;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.FastBattle;
   import ru.pragmatix.wormix.messages.structures.RatingProfileStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.AirBlocker;
   import starling.display.DisplayObject;
   import starling.display.Sprite;
   import starling.utils.Pool;
   
   public class FastBattleBinarySerializer implements §_-D2L§
   {
      
      public function FastBattleBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 142;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:FastBattle = FastBattle(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUTF(_loc3_.adEventId);
         _loc5_.writeUTF(_loc3_.sessionKey);
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

