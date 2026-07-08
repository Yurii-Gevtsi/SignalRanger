package ru.pragmatix.wormix.gui.menu.shop
{
   import §_-03T§.§_-2p§;
   import §_-12W§.*;
   import §_-62§.*;
   import §_-91A§.*;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-F1P§;
   import §_-C3f§.§_-1Y§;
   import §_-C3f§.§_-Z1Y§;
   import §_-CR§.§_-72p§;
   import §_-DJ§.§_-fH§;
   import §_-K35§.§_-99§;
   import §_-L2F§.§_-D2p§;
   import §_-RE§.§_-r2q§;
   import §_-T2h§.*;
   import §_-TF§.§_-q1j§;
   import §_-Vg§.*;
   import §_-Y1Q§.§_-61Z§;
   import §_-b1F§.§_-G4§;
   import §_-d26§.§_-d1H§;
   import §_-j1w§.*;
   import §_-jF§.ClanMemberItemRenderer;
   import §_-jF§.§_-U17§;
   import §_-qH§.§_-S25§;
   import §_-r1C§.§_-h2B§;
   import com.hurlant.util.§_-a2P§;
   import feathers.controls.Label;
   import feathers.controls.LayoutGroup;
   import feathers.controls.List;
   import feathers.controls.Panel;
   import feathers.controls.ScrollBarDisplayMode;
   import feathers.controls.ScrollInteractionMode;
   import feathers.controls.ScrollPolicy;
   import feathers.data.ArrayCollection;
   import feathers.layout.AnchorLayout;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.RelativePosition;
   import feathers.layout.TiledColumnsLayout;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.themes.styles.AlternateButtonsStyles;
   import feathers.utils.touch.TapToTrigger;
   import flash.events.Event;
   import flash.net.Socket;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.gui.controls.§_-NY§;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-eP§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.desktop.§_-An§;
   import ru.pragmatix.wormix.messages.clans.request.PostClanNewsRequest;
   import ru.pragmatix.wormix.messages.clans.response.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanInviteTO;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.IPhysicModel;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.Ufo;
   import starling.display.Canvas;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.utils.MathUtil;
   import starling.utils.Pool;
   
   public class §_-6s§ implements §_-An§
   {
      
      protected var §_-W14§:Object;
      
      private var panel:DisplayObject;
      
      public function §_-6s§()
      {
         super();
         this.panel = this.§_-l1P§();
         var _loc1_:Panel = this.§_-F2I§();
         _loc1_.verticalScrollStep = 2;
         _loc1_.horizontalScrollPolicy = ScrollPolicy.OFF;
         _loc1_.verticalScrollPolicy = ScrollPolicy.AUTO;
         _loc1_.scrollBarDisplayMode = ScrollBarDisplayMode.FIXED_FLOAT;
         _loc1_.interactionMode = ScrollInteractionMode.TOUCH_AND_SCROLL_BARS;
         _loc1_.verticalScrollBarFactory = §_-eP§.§_-G3p§;
      }
      
      protected function §_-l1P§() : DisplayObjectContainer
      {
         throw new Error("Display creation method is not implemented");
      }
      
      private function §_-N1l§(param1:DisplayObjectContainer, param2:LayoutGroup) : void
      {
         var _loc3_:int = param1.numChildren;
         var _loc4_:int = 0;
         while(_loc4_ < _loc3_)
         {
            param2.addChild(param1.getChildAt(0));
            param2.invalidate();
            _loc4_++;
         }
      }
      
      public function setItem(param1:Object) : void
      {
         if(param1 == null)
         {
            this.§_-W14§ = null;
            this.clearAll();
            return;
         }
         this.§_-X1S§(param1);
         this.§_-F2I§().verticalScrollPosition = 0;
         this.§_-F2I§().invalidate();
      }
      
      protected function clearAll() : void
      {
         this.§_-pn§().text = "";
         this.§_-V2e§().text = "";
      }
      
      protected function §_-X1S§(param1:Object) : void
      {
         this.§_-W14§ = param1;
         (this.§_-pn§().parent as LayoutGroup).invalidate();
         §_-h2B§.play(§_-d27§.§_-S29§);
      }
      
      protected function §_-F2I§() : Panel
      {
         throw new Error("getItemInfoPanel method is not implemented");
      }
      
      protected function §_-V2e§() : Label
      {
         throw new Error("getItemInfoPanel method is not implemented");
      }
      
      protected function §_-pn§() : Label
      {
         throw new Error("getItemInfoPanel method is not implemented");
      }
      
      public function §_-Bh§() : DisplayObject
      {
         return this.panel;
      }
      
      public function destroy() : void
      {
         this.panel.removeFromParent(true);
         this.§_-W14§ = null;
      }
   }
}

