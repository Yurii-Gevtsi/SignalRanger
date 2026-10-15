package ru.pragmatix.wormix.serialization.clans.request.edit
{
   import §_-12a§.§_-lc§;
   import §_-31W§.*;
   import §_-931§.§_-sz§;
   import §_-A1K§.§_-TA§;
   import §_-S§.*;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-ZP§.§_-H1H§;
   import §_-gG§.§_-8u§;
   import §_-j1w§.*;
   import §_-rM§.§_-61j§;
   import §_-zI§.§_-8D§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.edit.ChangeClanEmblemRequest;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.server.BuyVipResult;
   import ru.pragmatix.wormix.messages.structures.RentedItemsStructure;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.serialization.BaseCommandSerializer;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.weapons.ammo.cloud.GasCloud;
   import starling.events.Event;
   
   public class ChangeClanEmblemRequestBinarySerializer extends BaseCommandSerializer
   {
      
      public function ChangeClanEmblemRequestBinarySerializer()
      {
         super();
      }
      
      override protected function §_-41X§(param1:Object) : ByteArray
      {
         var _loc2_:ChangeClanEmblemRequest = ChangeClanEmblemRequest(param1);
         var _loc3_:ByteArray = new ByteArray();
         var _loc4_:ByteArray = new ByteArray();
         SerializationUtils.writeUintVector(_loc4_,_loc2_.emblem);
         _loc3_.writeShort(_loc4_.length);
         _loc3_.writeBytes(_loc4_,0,_loc4_.length);
         _loc3_.writeBoolean(_loc2_.fromTreas);
         return _loc3_;
      }
      
      override public function getCommandId() : uint
      {
         return ClanMessages.CHANGE_CLAN_EMBLEM_REQUEST;
      }
   }
}

