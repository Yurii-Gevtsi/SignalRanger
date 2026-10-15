package ru.pragmatix.wormix.messages.clans.request.edit
{
   import §_-12W§.*;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-bs§;
   import §_-L1H§.§_-C3e§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1b§.§_-81l§;
   import §_-aM§.*;
   import §_-j1w§.§_-L1x§;
   import §_-qH§.§_-S25§;
   import §_-r1C§.*;
   import flash.errors.IllegalOperationError;
   import flash.events.Event;
   import flash.system.LoaderContext;
   import flash.utils.ByteArray;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.request.InviteToClanRequest;
   import ru.pragmatix.wormix.messages.clans.response.InviteToClanResponse;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.social.§_-N1V§;
   
   public class ChangeClanEmblemRequest
   {
      
      public var emblem:Vector.<uint>;
      
      public var fromTreas:Boolean;
      
      public function ChangeClanEmblemRequest(param1:Vector.<uint>, param2:Boolean)
      {
         super();
         this.emblem = param1;
         this.fromTreas = param2;
      }
   }
}

