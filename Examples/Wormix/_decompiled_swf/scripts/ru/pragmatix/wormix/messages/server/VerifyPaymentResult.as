package ru.pragmatix.wormix.messages.server
{
   import §_-21p§.§_-4w§;
   import §_-91B§.§_-QK§;
   import §_-91B§.§_-z2l§;
   import §_-B2z§.§_-63i§;
   import §_-Cl§.*;
   import §_-DJ§.§_-J2l§;
   import §_-e2r§.*;
   import §_-l1g§.§_-F1X§;
   import §_-l1g§.§_-L3Q§;
   import §_-wD§.*;
   import dragonBones.animation.WorldClock;
   import flash.display.DisplayObject;
   import flash.display.Shape;
   import flash.geom.Rectangle;
   import flash.utils.Dictionary;
   import ru.pragmatix.flox.gui.controls.*;
   import ru.pragmatix.wormix.controller.§_-S2Y§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import starling.core.Starling;
   import starling.events.Event;
   
   public class VerifyPaymentResult extends IncomingPayment
   {
      
      public var result:int;
      
      public var subscriptionExpiryTimeSeconds:int;
      
      public var transactionId:String;
      
      public function VerifyPaymentResult()
      {
         super();
      }
      
      override public function toString() : String
      {
         return super.toString().replace("IncomingPayment{","VerifyPaymentResult{" + "result=" + this.result + "\n,subscriptionExpiryTimeSeconds=" + this.subscriptionExpiryTimeSeconds + ",transactionId=" + this.transactionId.substr(0,10) + (this.transactionId.length > 10 ? "***(" + this.transactionId.length + ")" : "") + "\n,");
      }
   }
}

