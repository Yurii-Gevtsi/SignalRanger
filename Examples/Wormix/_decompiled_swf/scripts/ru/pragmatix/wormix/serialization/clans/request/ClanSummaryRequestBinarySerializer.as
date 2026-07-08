package ru.pragmatix.wormix.serialization.clans.request
{
   import §_-51W§.§_-SN§;
   import §_-5Y§.StickerPanel;
   import §_-5Y§.§_-t2O§;
   import §_-B1y§.§_-bs§;
   import §_-C3f§.§_-1Y§;
   import §_-C3f§.§_-yG§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-H16§.*;
   import §_-SP§.§_-M4§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-XA§;
   import §_-Y1b§.§_-jH§;
   import §_-b1R§.§_-Bx§;
   import §_-l1K§.§_-cZ§;
   import feathers.controls.LayoutGroup;
   import feathers.events.FeathersEventType;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.RelativePosition;
   import feathers.layout.VerticalAlign;
   import flash.utils.ByteArray;
   import org.swiftsuspenders.Injector;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.ClanSummaryRequest;
   import ru.pragmatix.wormix.messages.clans.request.edit.ChangeClanDescriptionRequest;
   import ru.pragmatix.wormix.messages.clans.response.edit.ChangeClanDescriptionResponse;
   import ru.pragmatix.wormix.messages.structures.HeroicMissionStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.serialization.BaseCommandSerializer;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.events.Event;
   import starling.utils.MathUtil;
   
   public class ClanSummaryRequestBinarySerializer extends BaseCommandSerializer
   {
      
      public function ClanSummaryRequestBinarySerializer()
      {
         super();
      }
      
      override protected function §_-41X§(param1:Object) : ByteArray
      {
         var _loc2_:ClanSummaryRequest = ClanSummaryRequest(param1);
         var _loc3_:ByteArray = new ByteArray();
         _loc3_.writeInt(_loc2_.clanId);
         _loc3_.writeInt(_loc2_.scope);
         return _loc3_;
      }
      
      override public function getCommandId() : uint
      {
         return ClanMessages.CLAN_SUMMARY_REQUEST;
      }
   }
}

