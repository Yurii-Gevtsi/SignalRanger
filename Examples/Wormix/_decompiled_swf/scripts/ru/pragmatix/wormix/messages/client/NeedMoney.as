package ru.pragmatix.wormix.messages.client
{
   import §_-12a§.§_-41b§;
   import §_-63U§.ProgressBarBoss;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-H3§.*;
   import §_-Vg§.ClanCreateView;
   import §_-Y1Q§.§_-aG§;
   import §_-Y1Q§.§_-tc§;
   import §_-b1F§.§_-v2Q§;
   import §_-g2r§.§_-03A§;
   import §_-gG§.§_-M2S§;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-J2g§;
   import §_-zI§.§_-8D§;
   import §_-zI§.§_-F3q§;
   import feathers.controls.LayoutGroup;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.ILayoutDisplayObject;
   import feathers.layout.RelativePosition;
   import flash.display.DisplayObject;
   import flash.events.TimerEvent;
   import flash.geom.Rectangle;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.Timer;
   import ru.pragmatix.flox.gui.controls.§_-a1t§;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.controller.ICustomInviter;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.*;
   import ru.pragmatix.wormix.messages.structures.TurnStructure;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import starling.events.Event;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   
   public class NeedMoney
   {
      
      public var value:int;
      
      public var moneyType:int;
      
      public function NeedMoney(param1:int = 0)
      {
         super();
         this.value = param1;
      }
      
      public function toString() : String
      {
         return "NeedMoney{value=" + String(this.value) + ", type=" + String(this.moneyType) + "}";
      }
   }
}

