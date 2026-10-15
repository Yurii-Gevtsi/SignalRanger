package ru.pragmatix.wormix.messages.clans.request.edit
{
   import §_-B1y§.*;
   import §_-B2z§.§_-63i§;
   import §_-B2z§.§_-82u§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-Tm§.Action;
   import §_-Tm§.§_-K1I§;
   import §_-Tm§.§_-RF§;
   import §_-Tm§.§_-W1Y§;
   import §_-b1F§.§_-G4§;
   import §_-l1g§.§_-L3Q§;
   import §_-x2Q§.§_-dU§;
   import com.greensock.*;
   import com.greensock.plugins.*;
   import flash.errors.IllegalOperationError;
   import flash.utils.ByteArray;
   import flash.utils.getQualifiedClassName;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginJoinClanRequest;
   import ru.pragmatix.wormix.messages.client.PvpChatMessage;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.*;
   import starling.events.Event;
   
   public class RenameClanRequest
   {
      
      public var name:String;
      
      public var fromTreas:Boolean;
      
      public function RenameClanRequest(param1:String, param2:Boolean)
      {
         super();
         this.name = param1;
         this.fromTreas = param2;
      }
   }
}

