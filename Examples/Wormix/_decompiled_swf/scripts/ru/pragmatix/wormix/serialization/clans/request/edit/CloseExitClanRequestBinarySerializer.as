package ru.pragmatix.wormix.serialization.clans.request.edit
{
   import §_-21p§.§_-4w§;
   import §_-31N§.§_-42G§;
   import §_-31Y§.§_-D3B§;
   import §_-A1K§.§_-TA§;
   import §_-F39§.§_-i1i§;
   import §_-Z1K§.§_-q24§;
   import §_-b1F§.§_-H3Z§;
   import §_-fo§.*;
   import §_-hO§.§_-729§;
   import §_-l1g§.§_-L3Q§;
   import §_-p24§.*;
   import com.hurlant.math.*;
   import feathers.controls.Button;
   import flash.events.MouseEvent;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-E2w§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.edit.CloseExitClanRequest;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.scene.ObjectType;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.serialization.BaseCommandSerializer;
   import starling.display.Image;
   import starling.utils.Pool;
   
   use namespace bi_internal;
   
   public class CloseExitClanRequestBinarySerializer extends BaseCommandSerializer
   {
      
      public function CloseExitClanRequestBinarySerializer()
      {
         super();
      }
      
      override protected function §_-41X§(param1:Object) : ByteArray
      {
         var _loc2_:CloseExitClanRequest = CloseExitClanRequest(param1);
         var _loc3_:ByteArray = new ByteArray();
         _loc3_.writeBoolean(_loc2_.closed);
         return _loc3_;
      }
      
      override public function getCommandId() : uint
      {
         return ClanMessages.CHANGE_CLAN_CLOSE_EXIT_REQUEST;
      }
   }
}

