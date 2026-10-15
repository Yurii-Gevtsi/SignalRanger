package ru.pragmatix.wormix.serialization.clans.response
{
   import §_-CF§.§_-E19§;
   import §_-Cl§.*;
   import §_-DJ§.§_-fH§;
   import §_-I1l§.*;
   import §_-TF§.§_-Z1U§;
   import §_-b1F§.§_-v2Q§;
   import §_-j22§.§_-5g§;
   import §_-jF§.*;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-z1g§.§_-y1S§;
   import flash.events.Event;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.response.LowerRankInClanResponse;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.pvp.messages.client.BattleReconnect;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreationFailure;
   import ru.pragmatix.wormix.pvp.messages.ex.StartPvpBattle;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   public class LowerRankInClanResponseBinarySerializer implements §_-D2L§
   {
      
      public function LowerRankInClanResponseBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:LowerRankInClanResponse = new LowerRankInClanResponse();
         _loc3_.serviceResult = param1.readShort();
         _loc3_.profileId = param1.readInt();
         _loc3_.rank = param1.readShort();
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return ClanMessages.LOWER_IN_RANK_RESPONSE;
      }
   }
}

