package ru.pragmatix.wormix.messages.client
{
   import §_-D3A§.§_-TZ§;
   import §_-H2g§.§_-di§;
   import §_-J3L§.§_-h1D§;
   import §_-RE§.§_-52p§;
   import §_-Vg§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import com.greensock.events.TweenEvent;
   import com.hurlant.util.der.*;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Button;
   import feathers.controls.ButtonState;
   import feathers.controls.ToggleButton;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.messages.clans.request.PostToClanChatRequest;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.events.Event;
   
   public class AppleVerifyReceipt
   {
      
      public var productId:String;
      
      public var isSubscription:Boolean;
      
      public var sessionKey:String = "";
      
      public var receiptData:ByteArray = new ByteArray();
      
      public var secureResult:Boolean = false;
      
      public function AppleVerifyReceipt()
      {
         super();
      }
      
      public function toString() : String
      {
         return "AppleVerifyReceipt(" + "productId=" + this.productId + ",isSubscription=" + this.isSubscription + ",sessionKey=" + this.sessionKey + ",receiptData.length=" + this.receiptData.length + ")";
      }
   }
}

