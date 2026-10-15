package ru.pragmatix.wormix.serialization.clans.response
{
   import §_-12W§.*;
   import §_-2U§.*;
   import §_-62§.*;
   import §_-73d§.§_-pT§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-C3f§.§_-1Y§;
   import §_-C3f§.§_-yG§;
   import §_-DJ§.§_-fH§;
   import §_-L1H§.§_-b2K§;
   import §_-TF§.§_-y2n§;
   import §_-Tm§.Action;
   import §_-Tm§.§_-RF§;
   import §_-YF§.§_-N3§;
   import §_-aM§.§_-P1N§;
   import §_-aM§.§_-y1L§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-p14§.§_-U1D§;
   import §_-q26§.*;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.*;
   import com.greensock.loading.core.LoaderCore;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.ILayout;
   import feathers.layout.VerticalLayout;
   import flash.display.SimpleButton;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.getTimer;
   import ru.pragmatix.wormix.controller.§_-I3A§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginClanBase;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanEnterAccount;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanErrorResponse;
   import ru.pragmatix.wormix.messages.clans.response.PromoteRankInClanResponse;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.client.PvpActionEx;
   import ru.pragmatix.wormix.serialization.client.*;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-o1q§;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class PromoteRankInClanResponseBinarySerializer implements §_-D2L§
   {
      
      public function PromoteRankInClanResponseBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:PromoteRankInClanResponse = new PromoteRankInClanResponse();
         _loc3_.serviceResult = param1.readShort();
         _loc3_.profileId = param1.readInt();
         _loc3_.rank = param1.readShort();
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return ClanMessages.PROMOTE_IN_RANK_RESPONSE;
      }
   }
}

