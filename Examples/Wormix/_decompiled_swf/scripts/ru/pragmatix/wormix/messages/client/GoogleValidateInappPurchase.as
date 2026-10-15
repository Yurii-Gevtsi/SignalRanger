package ru.pragmatix.wormix.messages.client
{
   import §_-91A§.*;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-DJ§.§_-fH§;
   import §_-U1i§.§_-I3t§;
   import §_-Y1Q§.§_-61Z§;
   import §_-q11§.RaceRoomDescriptionRenderer;
   import §_-q11§.§_-e1u§;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-J2g§;
   import feathers.controls.ScrollBarDisplayMode;
   import feathers.controls.ScrollPolicy;
   import feathers.data.ArrayCollection;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.VerticalLayout;
   import feathers.themes.styles.AlternateScrollBarStyles;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.server.ShopResult;
   import ru.pragmatix.wormix.messages.structures.ProfileDoubleKeyStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.serialization.server.*;
   import ru.pragmatix.wormix.utils.§_-MN§;
   import starling.display.DisplayObject;
   
   public class GoogleValidateInappPurchase
   {
      
      public var productId:String;
      
      public var isSubscription:Boolean;
      
      public var sessionKey:String;
      
      public var purchaseToken:String;
      
      public var isSecure:Boolean;
      
      public function GoogleValidateInappPurchase()
      {
         super();
      }
      
      public function toString() : String
      {
         return "GoogleValidateInappPurchase(" + "productId=" + this.productId + ",isSubscription=" + this.isSubscription + ",sessionKey=" + this.sessionKey + ",purchaseToken=" + this.purchaseToken + ")";
      }
   }
}

