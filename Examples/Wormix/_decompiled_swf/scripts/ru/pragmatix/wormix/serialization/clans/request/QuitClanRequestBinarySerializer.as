package ru.pragmatix.wormix.serialization.clans.request
{
   import §_-31W§.§_-E1E§;
   import §_-62§.§_-W1r§;
   import §_-B1y§.§_-83E§;
   import §_-B1y§.§_-F1P§;
   import §_-DJ§.§_-73y§;
   import §_-Ly§.§_-81L§;
   import §_-Y1O§.§_-V23§;
   import §_-k1r§.*;
   import §_-l1g§.§_-T2N§;
   import §_-r1C§.§_-h2B§;
   import flash.events.EventDispatcher;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.utils.id.GetAuthKeyEvent;
   import ru.pragmatix.flox.social.utils.id.IPlatformAccountController;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.serialization.BaseCommandSerializer;
   import starling.events.Event;
   
   public class QuitClanRequestBinarySerializer extends BaseCommandSerializer
   {
      
      public function QuitClanRequestBinarySerializer()
      {
         super();
      }
      
      override public function getCommandId() : uint
      {
         return ClanMessages.QUIT_CLAN_REQUEST;
      }
   }
}

