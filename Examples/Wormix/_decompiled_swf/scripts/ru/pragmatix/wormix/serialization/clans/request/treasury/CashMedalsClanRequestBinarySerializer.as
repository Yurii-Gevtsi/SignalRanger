package ru.pragmatix.wormix.serialization.clans.request.treasury
{
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.treasury.CashMedalsClanRequest;
   import ru.pragmatix.wormix.serialization.BaseCommandSerializer;
   import ru.pragmatix.wormix.weapons.*;
   
   public class CashMedalsClanRequestBinarySerializer extends BaseCommandSerializer
   {
      
      public function CashMedalsClanRequestBinarySerializer()
      {
         super();
      }
      
      override protected function §_-41X§(param1:Object) : ByteArray
      {
         var _loc2_:CashMedalsClanRequest = CashMedalsClanRequest(param1);
         var _loc3_:ByteArray = new ByteArray();
         _loc3_.writeInt(_loc2_.medals);
         return _loc3_;
      }
      
      override public function getCommandId() : uint
      {
         return ClanMessages.CASH_MEDALS_REQUEST;
      }
   }
}

