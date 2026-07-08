package ru.pragmatix.wormix.gui.menu
{
   import §_-62§.*;
   import §_-63U§.ProgressBarBoss;
   import §_-63U§.§_-NV§;
   import §_-73I§.*;
   import §_-82l§.§_-1I§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.§_-F1P§;
   import §_-B1y§.§_-ts§;
   import §_-B2z§.§_-63i§;
   import §_-C2t§.§_-G2m§;
   import §_-CR§.§_-S2o§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-H3§.*;
   import §_-Ly§.§_-81L§;
   import §_-P1B§.§_-b21§;
   import §_-P2i§.§_-G3J§;
   import §_-TF§.§_-q1j§;
   import §_-d2p§.*;
   import §_-g2r§.§_-03A§;
   import §_-hO§.§_-v24§;
   import §_-j1w§.§_-B2i§;
   import §_-j1w§.§_-lD§;
   import §_-j2R§.§_-73G§;
   import §_-jF§.*;
   import §_-lh§.§_-z2A§;
   import §_-o14§.*;
   import §_-p18§.§_-83X§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-J2g§;
   import §_-t2x§.§_-53b§;
   import §_-t2x§.§_-H12§;
   import §_-t2x§.§_-nH§;
   import §_-t2x§.§_-v2c§;
   import §_-t2x§.§_-w1o§;
   import §_-zI§.§_-91X§;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.plugins.*;
   import config.§_-vR§;
   import feathers.controls.LayoutGroup;
   import feathers.controls.List;
   import feathers.controls.ScrollPolicy;
   import feathers.controls.text.TextFieldTextRenderer;
   import feathers.controls.text.TextInstanceCreator;
   import feathers.core.ITextRenderer;
   import feathers.data.ListCollection;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.HorizontalLayout;
   import feathers.layout.ILayout;
   import feathers.layout.ILayoutDisplayObject;
   import feathers.layout.RelativePosition;
   import flash.display.*;
   import flash.events.*;
   import flash.geom.ColorTransform;
   import flash.geom.Rectangle;
   import flash.text.TextFormat;
   import flash.text.TextFormatAlign;
   import flash.utils.*;
   import org.as3commons.zip.*;
   import ru.pragmatix.flox.gui.controls.§_-NY§;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.*;
   import ru.pragmatix.wormix.gui.menu.shop.mobile.*;
   import ru.pragmatix.wormix.messages.client.EndTurn;
   import ru.pragmatix.wormix.messages.client.StartBattle;
   import ru.pragmatix.wormix.messages.structures.TurnStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.platform.§_-WK§;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurn;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurnResponse;
   import starling.display.Button;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.extensions.ParticleAura;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   import starling.utils.SystemUtil;
   
   public class §_-k25§ extends List
   {
      
      public function §_-k25§(param1:starling.display.DisplayObjectContainer)
      {
         super();
         param1.addChild(this);
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         _layout = this.§_-N1g§();
         _horizontalScrollPolicy = ScrollPolicy.OFF;
         _verticalScrollPolicy = ScrollPolicy.OFF;
      }
      
      public function §_-b2U§(param1:Button, param2:Button) : void
      {
         param1.alphaWhenDisabled = 0.1;
         param2.alphaWhenDisabled = 0.1;
         param1.scaleWhenDown = 0.8;
         param2.scaleWhenDown = 0.8;
         param1.addEventListener(starling.events.Event.TRIGGERED,this.§_-b14§);
         param2.addEventListener(starling.events.Event.TRIGGERED,this.§_-525§);
      }
      
      private function §_-b14§(param1:starling.events.Event) : void
      {
         if(_selectedIndex == 0)
         {
            §_-h2B§.play(§_-d27§.§_-S29§);
            return;
         }
         --selectedIndex;
      }
      
      private function §_-525§(param1:starling.events.Event) : void
      {
         if(_selectedIndex == _dataProvider.length - 1)
         {
            §_-h2B§.play(§_-d27§.§_-S29§);
            return;
         }
         ++selectedIndex;
      }
      
      protected function §_-N1g§() : ILayout
      {
         var _loc1_:HorizontalLayout = new HorizontalLayout();
         _loc1_.padding = 10;
         return _loc1_;
      }
      
      public function §_-03o§(param1:int) : void
      {
         _dataProvider = new ListCollection(this.§_-n1r§(param1));
         visible = param1 > 1;
      }
      
      protected function §_-n1r§(param1:int) : Array
      {
         var _loc2_:Array = [];
         var _loc3_:int = 1;
         while(_loc3_ <= param1)
         {
            _loc2_.push(_loc3_);
            _loc3_++;
         }
         return _loc2_;
      }
      
      public function §_-01o§() : int
      {
         if(_dataProvider)
         {
            return _dataProvider.length;
         }
         return 0;
      }
   }
}

