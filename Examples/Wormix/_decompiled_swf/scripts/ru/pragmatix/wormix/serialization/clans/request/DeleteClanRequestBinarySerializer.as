package ru.pragmatix.wormix.serialization.clans.request
{
   import §_-zI§.§_-sI§;
   import dragonBones.animation.WorldClock;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.model.rewards.§_-P2q§;
   import ru.pragmatix.wormix.serialization.BaseCommandSerializer;
   import starling.animation.Tween;
   import starling.core.Starling;
   import starling.events.Event;
   
   public class DeleteClanRequestBinarySerializer extends BaseCommandSerializer
   {
      
      public function DeleteClanRequestBinarySerializer()
      {
         super();
      }
      
      override protected function §_-41X§(param1:Object) : ByteArray
      {
         return new ByteArray();
      }
      
      override public function getCommandId() : uint
      {
         return ClanMessages.DELETE_CLAN_REQUEST;
      }
   }
}

