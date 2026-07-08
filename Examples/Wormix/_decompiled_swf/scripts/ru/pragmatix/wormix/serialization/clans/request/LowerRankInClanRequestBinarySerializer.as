package ru.pragmatix.wormix.serialization.clans.request
{
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-bs§;
   import §_-C3f§.§_-4I§;
   import §_-Cl§.*;
   import §_-DJ§.§_-J2l§;
   import §_-YF§.§_-q2v§;
   import feathers.controls.Button;
   import feathers.core.ITextRenderer;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.LowerRankInClanRequest;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.serialization.BaseCommandSerializer;
   import ru.pragmatix.wormix.utils.StringUtil;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.Weapon;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class LowerRankInClanRequestBinarySerializer extends BaseCommandSerializer
   {
      
      public function LowerRankInClanRequestBinarySerializer()
      {
         super();
      }
      
      override protected function §_-41X§(param1:Object) : ByteArray
      {
         var _loc2_:LowerRankInClanRequest = LowerRankInClanRequest(param1);
         var _loc3_:ByteArray = new ByteArray();
         _loc3_.writeInt(_loc2_.profileId);
         _loc3_.writeShort(_loc2_.targetRank);
         return _loc3_;
      }
      
      override public function getCommandId() : uint
      {
         return ClanMessages.LOWER_IN_RANK_REQUEST;
      }
   }
}

