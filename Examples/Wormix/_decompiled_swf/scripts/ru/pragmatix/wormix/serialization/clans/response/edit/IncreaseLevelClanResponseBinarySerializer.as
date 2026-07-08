package ru.pragmatix.wormix.serialization.clans.response.edit
{
   import §_-62§.§_-O2H§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.Cookie;
   import §_-D3A§.§_-TZ§;
   import §_-Ly§.§_-81L§;
   import §_-Y1Q§.§_-61Z§;
   import §_-Y1b§.§_-OP§;
   import §_-b1F§.§_-G4§;
   import §_-jF§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-o14§.*;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-t2x§.§_-53b§;
   import §_-t2x§.§_-H12§;
   import §_-t2x§.§_-nH§;
   import §_-t2x§.§_-v2c§;
   import §_-t2x§.§_-w1o§;
   import §_-x22§.§_-xY§;
   import §_-zI§.§_-91X§;
   import config.§_-vR§;
   import flash.desktop.NativeApplication;
   import flash.geom.Point;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.logger.§_-11S§;
   import ru.pragmatix.flox.social.response.LoadUserProfilesResponse;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.response.edit.IncreaseLevelClanResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.RopePhysicModel;
   import ru.pragmatix.wormix.physics.model.UnitPhysicModel;
   import ru.pragmatix.wormix.platform.§_-WK§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.text.TextField;
   import starling.text.TextFieldAutoSize;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   public class IncreaseLevelClanResponseBinarySerializer implements §_-D2L§
   {
      
      public function IncreaseLevelClanResponseBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:IncreaseLevelClanResponse = new IncreaseLevelClanResponse();
         _loc3_.serviceResult = param1.readShort();
         _loc3_.level = param1.readInt();
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return ClanMessages.INCREASE_LEVEL_CLAN_RESPONSE;
      }
   }
}

