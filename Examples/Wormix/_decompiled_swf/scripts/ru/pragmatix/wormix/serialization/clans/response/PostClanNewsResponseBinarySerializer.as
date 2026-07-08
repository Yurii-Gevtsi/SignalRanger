package ru.pragmatix.wormix.serialization.clans.response
{
   import §_-31N§.§_-42G§;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-W1r§;
   import §_-82l§.§_-1I§;
   import §_-A2U§.§_-A23§;
   import §_-B2z§.§_-63i§;
   import §_-C2M§.*;
   import §_-Cl§.*;
   import §_-H3§.*;
   import §_-Tm§.§_-K1I§;
   import §_-U1i§.§_-O2i§;
   import §_-U1i§.§_-hK§;
   import §_-YF§.§_-q2v§;
   import §_-Z1w§.§_-s1x§;
   import §_-p18§.§_-h1b§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-x2Q§.§_-dU§;
   import flash.events.Event;
   import flash.events.NativeWindowDisplayStateEvent;
   import flash.geom.Point;
   import flash.net.URLRequest;
   import flash.net.navigateToURL;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-fT§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.response.PostClanNewsResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.platform.§_-WK§;
   import ru.pragmatix.wormix.pvp.messages.client.PvpActionEx;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.SystemUtil;
   
   public class PostClanNewsResponseBinarySerializer implements §_-D2L§
   {
      
      public function PostClanNewsResponseBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:PostClanNewsResponse = new PostClanNewsResponse();
         _loc3_.serviceResult = param1.readShort();
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return ClanMessages.POST_NEWS_RESPONSE;
      }
   }
}

