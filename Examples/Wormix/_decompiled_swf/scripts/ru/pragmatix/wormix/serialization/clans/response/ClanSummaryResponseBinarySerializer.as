package ru.pragmatix.wormix.serialization.clans.response
{
   import §_-23P§.§_-V2T§;
   import §_-23P§.§_-u1z§;
   import §_-31N§.§_-42G§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-O2H§;
   import §_-82l§.§_-A2Y§;
   import §_-9§.§_-52A§;
   import §_-A1K§.§_-TA§;
   import §_-D3A§.§_-TZ§;
   import §_-J31§.§_-e2J§;
   import §_-L1H§.§_-C3e§;
   import §_-P2i§.§_-G3J§;
   import §_-RE§.§_-qW§;
   import §_-Tm§.§_-RF§;
   import §_-YF§.*;
   import §_-Z1K§.§_-q24§;
   import §_-Z1w§.§_-s1x§;
   import §_-aM§.*;
   import §_-fo§.*;
   import §_-jF§.*;
   import §_-k1u§.§_-Oi§;
   import §_-l1K§.§_-d2j§;
   import §_-l1g§.§_-L3Q§;
   import §_-m2s§.§_-R11§;
   import §_-o14§.*;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-sK§.*;
   import §_-w2i§.§_-Zd§;
   import §_-z20§.§_-V7§;
   import §_-z20§.§_-p4§;
   import feathers.controls.List;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import flash.events.Event;
   import flash.geom.Point;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.logger.§_-93v§;
   import ru.pragmatix.wormix.controller.§_-I3A§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.ClanServiceResult;
   import ru.pragmatix.wormix.messages.clans.response.ClanSummaryResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-g20§;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.ammo.ChargingBullet;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import ru.pragmatix.wormix.weapons.interfaces.§_-O1c§;
   import starling.animation.Tween;
   import starling.core.Starling;
   import starling.display.Image;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.events.Touch;
   import starling.events.TouchEvent;
   import starling.events.TouchPhase;
   import starling.utils.Pool;
   
   public class ClanSummaryResponseBinarySerializer implements §_-D2L§
   {
      
      public function ClanSummaryResponseBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ClanSummaryResponse = new ClanSummaryResponse();
         _loc3_.serviceResult = param1.readShort();
         if(_loc3_.serviceResult != ClanServiceResult.ERR_NOT_FOUND.code)
         {
            _loc3_.clan = SerializationUtils.readStructure(param1 as ByteArray,ClanTO) as ClanTO;
         }
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return ClanMessages.CLAN_SUMMARY_RESPONSE;
      }
   }
}

