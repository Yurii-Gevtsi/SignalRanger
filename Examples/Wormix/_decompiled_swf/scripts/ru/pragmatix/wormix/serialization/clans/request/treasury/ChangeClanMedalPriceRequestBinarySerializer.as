package ru.pragmatix.wormix.serialization.clans.request.treasury
{
   import §_-l1K§.§_-W2f§;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.treasury.ChangeClanMedalPriceRequest;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.serialization.BaseCommandSerializer;
   import ru.pragmatix.wormix.weapons.*;
   
   public class ChangeClanMedalPriceRequestBinarySerializer extends BaseCommandSerializer
   {
      
      public function ChangeClanMedalPriceRequestBinarySerializer()
      {
         super();
      }
      
      override protected function §_-41X§(param1:Object) : ByteArray
      {
         var _loc2_:ChangeClanMedalPriceRequest = ChangeClanMedalPriceRequest(param1);
         var _loc3_:ByteArray = new ByteArray();
         _loc3_.writeByte(_loc2_.medalPrice);
         return _loc3_;
      }
      
      override public function getCommandId() : uint
      {
         return ClanMessages.CHANGE_CLAN_MEDAL_PRICE_REQUEST;
      }
   }
}

