package ru.pragmatix.wormix.serialization.clans.response
{
   import §_-31N§.§_-42G§;
   import §_-31W§.*;
   import §_-62§.§_-G1p§;
   import §_-F3k§.*;
   import §_-J31§.§_-e2J§;
   import §_-U1i§.§_-J1§;
   import §_-Y1O§.§_-42h§;
   import §_-Z1w§.§_-s1x§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import feathers.core.ITextRenderer;
   import flash.events.Event;
   import flash.events.NativeWindowDisplayStateEvent;
   import flash.events.TimerEvent;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.Timer;
   import ru.pragmatix.flox.social.controller.mobile.*;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.response.SetMuteModeResponse;
   import ru.pragmatix.wormix.pvp.messages.ex.PvpBattleType;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class SetMuteModeResponseBinarySerializer implements §_-D2L§
   {
      
      public function SetMuteModeResponseBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:SetMuteModeResponse = new SetMuteModeResponse();
         _loc3_.serviceResult = param1.readShort();
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return ClanMessages.SET_MUTE_MODE_RESPONSE;
      }
   }
}

