package ru.pragmatix.wormix.serialization.clans.response.edit
{
   import §_-B1y§.§_-MQ§;
   import §_-B1y§.§_-bs§;
   import §_-H2g§.§_-di§;
   import §_-RE§.§_-D3v§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.LoaderStatus;
   import feathers.data.HierarchicalCollection;
   import flash.events.Event;
   import flash.events.ProgressEvent;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.logger.§_-11S§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.main.server.GetJoinClansMainRequest;
   import ru.pragmatix.wormix.messages.clans.response.GetJoinClansMainResponse;
   import ru.pragmatix.wormix.messages.clans.response.edit.ChangeClanJoinRatingResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanAuditActionTO;
   import starling.events.Event;
   
   public class ChangeClanJoinRatingResponseBinarySerializer implements §_-D2L§
   {
      
      public function ChangeClanJoinRatingResponseBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ChangeClanJoinRatingResponse = new ChangeClanJoinRatingResponse();
         _loc3_.serviceResult = param1.readShort();
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return ClanMessages.CHANGE_CLAN_JOIN_RATING_RESPONSE;
      }
   }
}

