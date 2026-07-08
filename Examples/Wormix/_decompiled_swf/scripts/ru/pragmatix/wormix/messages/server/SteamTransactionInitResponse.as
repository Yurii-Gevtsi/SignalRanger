package ru.pragmatix.wormix.messages.server
{
   import §_-A1K§.§_-TA§;
   import §_-D3A§.§_-DI§;
   import §_-N2D§.§_-e2b§;
   import §_-N2D§.§_-o2y§;
   import §_-Y1b§.§_-OP§;
   import §_-j22§.§_-5g§;
   import §_-k1u§.§_-2m§;
   import §_-z1g§.§_-lp§;
   import feathers.controls.LayoutGroup;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import flash.utils.clearTimeout;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.messages.structures.RatingProfileStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseAmmo;
   import starling.display.DisplayObject;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.Pool;
   
   public class SteamTransactionInitResponse
   {
      
      public var purchaseOrderId:int;
      
      public var purchaseTransactionId:String;
      
      public var countryCode:String;
      
      public var currencyCode:String;
      
      public var socialParams:String;
      
      public var errorCode:int;
      
      public var errorMessage:String;
      
      public function SteamTransactionInitResponse()
      {
         super();
      }
      
      public function toString() : String
      {
         return "SteamTransactionInitResponse{" + "purchaseOrderId=" + String(this.purchaseOrderId) + ",purchaseTransactionId=" + String(this.purchaseTransactionId) + ",countryCode=" + String(this.countryCode) + ",currencyCode=" + String(this.currencyCode) + ",socialParams=" + String(this.socialParams) + ",errorCode=" + String(this.errorCode) + ",errorMessage=" + String(this.errorMessage) + "}";
      }
   }
}

