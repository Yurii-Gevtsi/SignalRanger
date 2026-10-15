package ru.pragmatix.wormix.serialization.clans.request
{
   import §_-31W§.*;
   import §_-D3A§.§_-TZ§;
   import §_-Z1w§.§_-w1D§;
   import §_-l1K§.*;
   import §_-l1g§.§_-L3Q§;
   import com.greensock.loading.LoaderMax;
   import com.greensock.loading.LoaderStatus;
   import com.greensock.loading.core.*;
   import com.hurlant.util.§_-a2P§;
   import flash.display.DisplayObject;
   import flash.net.LocalConnection;
   import flash.system.Capabilities;
   import flash.text.TextFormat;
   import ru.pragmatix.wormix.controller.§_-S2Y§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.serialization.BaseCommandSerializer;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   public class AuditActionsRequestBinarySerializer extends BaseCommandSerializer
   {
      
      public function AuditActionsRequestBinarySerializer()
      {
         super();
      }
      
      override public function getCommandId() : uint
      {
         return ClanMessages.AUDIT_ACTIONS_REQUEST;
      }
   }
}

