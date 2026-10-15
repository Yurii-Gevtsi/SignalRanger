package ru.pragmatix.wormix.serialization.client
{
   import §_-12W§.*;
   import §_-31W§.§_-aO§;
   import §_-62§.*;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-C3f§.§_-yG§;
   import §_-G16§.*;
   import §_-H16§.*;
   import §_-R2S§.Farmer;
   import §_-R2S§.Hunter;
   import §_-R2S§.Miner;
   import §_-R2S§.§_-P1U§;
   import §_-R2S§.§_-z2x§;
   import §_-RE§.§_-w0§;
   import §_-YF§.§_-q2v§;
   import §_-Z1w§.§_-s1x§;
   import §_-b1F§.§_-v2Q§;
   import §_-j22§.§_-5g§;
   import §_-q26§.*;
   import §_-r1C§.§_-V1R§;
   import §_-r1C§.§_-t1R§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import dragonBones.animation.WorldClock;
   import flash.media.SoundTransform;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.getTimer;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginClanBase;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanEnterAccount;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanErrorResponse;
   import ru.pragmatix.wormix.messages.client.BuyResetBonusItems;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.*;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class BuyResetBonusItemsBinarySerializer implements §_-D2L§
   {
      
      public function BuyResetBonusItemsBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:BuyResetBonusItems = BuyResetBonusItems(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeInt(_loc3_.requestNum);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
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
         return 3010;
      }
   }
}

