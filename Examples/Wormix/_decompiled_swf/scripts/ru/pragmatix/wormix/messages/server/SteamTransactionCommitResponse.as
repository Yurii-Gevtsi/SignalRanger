package ru.pragmatix.wormix.messages.server
{
   import §_-21p§.§_-4w§;
   import §_-21p§.§_-N1H§;
   import §_-62§.§_-gr§;
   import §_-73I§.§_-q15§;
   import §_-DJ§.§_-fH§;
   import §_-G16§.*;
   import §_-Jd§.§_-11K§;
   import §_-Ly§.§_-81L§;
   import §_-Nq§.*;
   import §_-TF§.§_-q1j§;
   import §_-YF§.§_-q2v§;
   import §_-b1F§.§_-v2Q§;
   import §_-fo§.§_-r5§;
   import §_-m2s§.§_-R11§;
   import feathers.core.ITextRenderer;
   import feathers.events.FeathersEventType;
   import feathers.layout.RelativePosition;
   import feathers.layout.VerticalLayout;
   import flash.events.ErrorEvent;
   import flash.geom.Rectangle;
   import flash.text.TextFieldAutoSize;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.bonus.*;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-R2g§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.display.DisplayObject;
   import starling.display.Quad;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   
   public class SteamTransactionCommitResponse extends IncomingPayment
   {
      
      public var purchaseOrderId:int;
      
      public var purchaseTransactionId:String;
      
      public var result:int;
      
      public function SteamTransactionCommitResponse()
      {
         super();
      }
      
      override public function toString() : String
      {
         return "SteamTransactionCommitResponse{moneyType=" + String(moneyType) + ",item=" + String(item) + ",count=" + String(count) + ",sessionKey=" + String(sessionKey) + "purchaseOrderId=" + String(this.purchaseOrderId) + ",purchaseTransactionId=" + String(this.purchaseTransactionId) + ",result=" + String(this.result) + "}";
      }
   }
}

