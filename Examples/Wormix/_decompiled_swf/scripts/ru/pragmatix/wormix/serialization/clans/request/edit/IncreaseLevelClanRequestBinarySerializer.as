package ru.pragmatix.wormix.serialization.clans.request.edit
{
   import §_-D3A§.§_-TZ§;
   import §_-L1H§.§_-b2K§;
   import §_-Tm§.§_-RF§;
   import §_-l1K§.*;
   import §_-l1g§.§_-L3Q§;
   import com.hurlant.math.*;
   import dragonBones.animation.WorldClock;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.edit.IncreaseLevelClanRequest;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.serialization.BaseCommandSerializer;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.Pool;
   
   use namespace bi_internal;
   
   public class IncreaseLevelClanRequestBinarySerializer extends BaseCommandSerializer
   {
      
      public function IncreaseLevelClanRequestBinarySerializer()
      {
         super();
      }
      
      override protected function §_-41X§(param1:Object) : ByteArray
      {
         var _loc2_:IncreaseLevelClanRequest = IncreaseLevelClanRequest(param1);
         var _loc3_:ByteArray = new ByteArray();
         _loc3_.writeInt(_loc2_.level);
         _loc3_.writeBoolean(_loc2_.fromTreas);
         return _loc3_;
      }
      
      override public function getCommandId() : uint
      {
         return ClanMessages.INCREASE_LEVEL_CLAN_REQUEST;
      }
   }
}

