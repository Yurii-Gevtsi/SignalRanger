package ru.pragmatix.wormix.serialization.client
{
   import §_-62§.§_-O2H§;
   import §_-B1y§.Cookie;
   import §_-B2z§.Server;
   import §_-TF§.§_-y2n§;
   import §_-Tm§.§_-RF§;
   import §_-j22§.§_-5g§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.*;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-6a§;
   import §_-w§.§_-63m§;
   import §_-x2Q§.§_-dU§;
   import dragonBones.animation.WorldClock;
   import flash.events.Event;
   import flash.geom.Point;
   import flash.text.TextField;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.client.GetDailyBonus;
   import ru.pragmatix.wormix.messages.client.RemoveFromGroup;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreated;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   public class GetDailyBonusBinarySerializer implements §_-D2L§
   {
      
      public function GetDailyBonusBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 107;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:GetDailyBonus = GetDailyBonus(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeBoolean(_loc3_.advertisingVew);
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

