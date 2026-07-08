package ru.pragmatix.wormix.messages.server
{
   import §_-A1K§.§_-TA§;
   import §_-C3f§.§_-1Y§;
   import §_-DJ§.§_-fH§;
   import §_-Ub§.*;
   import §_-YF§.§_-N3§;
   import §_-aM§.*;
   import dragonBones.Slot;
   import feathers.controls.TabBar;
   import flash.net.URLRequest;
   import flash.net.navigateToURL;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import org.gestouch.core.§_-H3d§;
   import org.gestouch.core.gestouch_internal;
   import ru.pragmatix.wormix.controller.§_-I3A§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.SecureMessage;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.events.Event;
   import starling.textures.Texture;
   
   public class IncomingPayment extends SecureMessage
   {
      
      public var moneyType:int;
      
      public var item:String;
      
      public var count:int;
      
      public var awards:Array;
      
      public var note:String;
      
      public var sessionKey:String;
      
      public function IncomingPayment()
      {
         super();
      }
      
      public function toString() : String
      {
         return "IncomingPayment{" + "moneyType=" + String(this.moneyType) + ",item=" + String(this.item) + ",count=" + String(this.count) + ",sessionKey=" + String(this.sessionKey) + "}";
      }
   }
}

