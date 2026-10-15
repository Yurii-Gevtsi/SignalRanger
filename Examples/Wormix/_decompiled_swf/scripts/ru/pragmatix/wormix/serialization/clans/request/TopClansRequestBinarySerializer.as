package ru.pragmatix.wormix.serialization.clans.request
{
   import §_-31N§.§_-42G§;
   import §_-31Y§.§_-D3B§;
   import §_-62§.§_-zF§;
   import §_-D3A§.§_-TZ§;
   import §_-F39§.*;
   import §_-L1H§.§_-C3e§;
   import §_-l1g§.§_-L3Q§;
   import §_-z1g§.§_-M2v§;
   import §_-z1g§.§_-lp§;
   import flash.errors.IllegalOperationError;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.*;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.TopClansRequest;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.serialization.BaseCommandSerializer;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.LaserRay;
   import starling.display.DisplayObject;
   
   public class TopClansRequestBinarySerializer extends BaseCommandSerializer
   {
      
      public function TopClansRequestBinarySerializer()
      {
         super();
      }
      
      override protected function §_-41X§(param1:Object) : ByteArray
      {
         var _loc2_:TopClansRequest = TopClansRequest(param1);
         var _loc3_:ByteArray = new ByteArray();
         _loc3_.writeBoolean(_loc2_.season);
         return _loc3_;
      }
      
      override public function getCommandId() : uint
      {
         return ClanMessages.TOP_CLANS_REQUEST;
      }
   }
}

