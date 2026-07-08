package ru.pragmatix.wormix.serialization.clans.request
{
   import §_-B1y§.Cookie;
   import §_-Cl§.*;
   import §_-D3A§.§_-TZ§;
   import §_-F39§.*;
   import §_-L1H§.§_-b2K§;
   import §_-P1z§.*;
   import §_-X14§.*;
   import §_-Y1O§.§_-V23§;
   import §_-Y1O§.§_-y1o§;
   import §_-Y1b§.*;
   import §_-j1w§.§_-L1x§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import com.distriqt.extension.inappbilling.events.PurchaseEvent;
   import feathers.controls.Button;
   import feathers.controls.NumericStepper;
   import feathers.controls.ScreenNavigatorItem;
   import feathers.data.ArrayCollection;
   import feathers.skins.ImageSkin;
   import flash.geom.Point;
   import flash.system.ApplicationDomain;
   import flash.system.LoaderContext;
   import flash.system.SecurityDomain;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.InviteToClanRequest;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.model.CatPhysicModel;
   import ru.pragmatix.wormix.serialization.BaseCommandSerializer;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.textures.Texture;
   
   public class InviteToClanRequestBinarySerializer extends BaseCommandSerializer
   {
      
      public function InviteToClanRequestBinarySerializer()
      {
         super();
      }
      
      override protected function §_-41X§(param1:Object) : ByteArray
      {
         var _loc2_:InviteToClanRequest = InviteToClanRequest(param1);
         var _loc3_:ByteArray = new ByteArray();
         _loc3_.writeInt(_loc2_.clanId);
         _loc3_.writeInt(_loc2_.profileId);
         return _loc3_;
      }
      
      override public function getCommandId() : uint
      {
         return ClanMessages.INVITE_TO_CLAN_REQUEST;
      }
   }
}

