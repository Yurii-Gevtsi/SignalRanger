package ru.pragmatix.wormix.messages.client
{
   import §_-21p§.§_-4w§;
   import §_-5P§.*;
   import §_-C2t§.§_-92W§;
   import §_-C2t§.§_-Ui§;
   import §_-D3A§.§_-TZ§;
   import §_-S1N§.§_-R1U§;
   import §_-Vu§.MD5;
   import §_-W§.§_-Ap§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1b§.§_-OP§;
   import §_-Z1K§.§_-q24§;
   import §_-aM§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-s20§.§_-J2g§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.events.ApplicationReceiptEvent;
   import com.distriqt.extension.inappbilling.events.AvailabilityEvent;
   import com.distriqt.extension.inappbilling.events.InAppBillingEvent;
   import com.distriqt.extension.inappbilling.events.ProductEvent;
   import com.distriqt.extension.inappbilling.events.PurchaseEvent;
   import com.greensock.loading.LoaderMax;
   import com.greensock.loading.core.*;
   import config.§_-vR§;
   import flash.system.ApplicationDomain;
   import flash.system.Capabilities;
   import flash.system.LoaderContext;
   import flash.system.Security;
   import flash.system.SecurityDomain;
   import ru.pragmatix.flox.logger.§_-11S§;
   import ru.pragmatix.flox.logger.§_-93v§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.messages.SecureMessage;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.scene.Weather;
   import ru.pragmatix.wormix.weapons.§_-73v§;
   import ru.pragmatix.wormix.weapons.ammo.GuardingBot;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.textures.Texture;
   
   public class SteamTransactionCommitRequest extends SecureMessage
   {
      
      public var purchaseOrderId:int;
      
      public var purchaseTransactionId:String;
      
      public var sessionId:String;
      
      public function SteamTransactionCommitRequest()
      {
         super();
      }
      
      public function toString() : String
      {
         return "SteamTransactionCommitRequest{purchaseOrderId=" + String(this.purchaseOrderId) + ",purchaseTransactionId=" + String(this.purchaseTransactionId) + ",sessionId=" + String(this.sessionId) + "}";
      }
   }
}

