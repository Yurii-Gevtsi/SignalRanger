package ru.pragmatix.wormix.messages.client
{
   import §_-62§.§_-G1p§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-F1P§;
   import §_-B1y§.§_-bs§;
   import §_-L1H§.§_-C3e§;
   import §_-U1i§.*;
   import §_-Y1O§.§_-yA§;
   import §_-a19§.*;
   import §_-fo§.*;
   import §_-j1w§.§_-B2i§;
   import §_-j1w§.§_-L1x§;
   import §_-k1u§.§_-2m§;
   import §_-k1u§.§_-52D§;
   import §_-k1u§.§_-F3H§;
   import §_-k1u§.§_-H2M§;
   import §_-k1u§.§_-Vr§;
   import §_-k1u§.§_-by§;
   import §_-k2s§.§_-e1k§;
   import §_-z1g§.§_-lp§;
   import com.greensock.loading.ImageLoader;
   import com.greensock.loading.LoaderMax;
   import feathers.data.IListCollection;
   import flash.events.TimerEvent;
   import flash.geom.Point;
   import flash.utils.Dictionary;
   import org.gestouch.core.GestureState;
   import ru.pragmatix.wormix.model.user.*;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   
   public class BuyBattle
   {
      
      public var moneyType:int;
      
      public var bulk:Boolean;
      
      public var adEventId:String;
      
      public function BuyBattle(param1:int, param2:Boolean, param3:String = "")
      {
         super();
         this.moneyType = param1;
         this.bulk = param2;
         this.adEventId = param3;
      }
      
      public function toString() : String
      {
         return "BuyBattle{" + "moneyType=" + this.moneyType + ",bulk=" + this.bulk + ",adEventId=" + this.adEventId + "}";
      }
   }
}

