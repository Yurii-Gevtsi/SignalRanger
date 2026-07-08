package ru.pragmatix.wormix.serialization.client
{
   import §_-43V§.Random;
   import §_-5Y§.§_-h2c§;
   import §_-62§.§_-T1y§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.§_-F1P§;
   import §_-RE§.§_-r2q§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-O2i§;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-z1g§.§_-F4§;
   import §_-z1g§.§_-y1S§;
   import §_-z20§.§_-pl§;
   import com.hurlant.crypto.tls.*;
   import flash.events.TimerEvent;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.Timer;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.client.RepeatPvpBattleAward;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.social.common.post.*;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import starling.display.Sprite;
   import starling.events.Event;
   
   public class RepeatPvpBattleAwardBinarySerializer implements §_-D2L§
   {
      
      public function RepeatPvpBattleAwardBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 144;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:RepeatPvpBattleAward = RepeatPvpBattleAward(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _temp_2:* = new ByteArray();
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUTF(_loc3_.adEventId);
         _loc5_.writeUTF(_loc3_.sessionKey);
         _loc5_.writeInt(_loc3_.battleId);
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

