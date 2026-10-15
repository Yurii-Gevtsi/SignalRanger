package ru.pragmatix.wormix.serialization.clans.response
{
   import §_-02q§.§_-4s§;
   import §_-21p§.§_-4w§;
   import §_-62§.*;
   import §_-73d§.§_-pT§;
   import §_-A1K§.§_-7u§;
   import §_-DJ§.§_-J2l§;
   import §_-Ly§.§_-n1J§;
   import §_-hv§.*;
   import §_-l1K§.§_-W2f§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-w2W§.§_-21w§;
   import config.§_-B1K§;
   import flash.display.DisplayObject;
   import flash.display.DisplayObjectContainer;
   import flash.text.TextField;
   import flash.text.TextFormat;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.describeType;
   import ru.pragmatix.flox.gui.controls.*;
   import ru.pragmatix.wormix.gui.screenOverlay.view.§_-in§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.response.SetExpelPermitResponse;
   import ru.pragmatix.wormix.messages.structures.ChatMessage;
   import starling.display.Sprite;
   import starling.events.Event;
   
   public class SetExpelPermitResponseBinarySerializer implements §_-D2L§
   {
      
      public function SetExpelPermitResponseBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:SetExpelPermitResponse = new SetExpelPermitResponse();
         _loc3_.serviceResult = param1.readShort();
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return ClanMessages.SET_EXPEL_PERMIT_RESPONSE;
      }
   }
}

