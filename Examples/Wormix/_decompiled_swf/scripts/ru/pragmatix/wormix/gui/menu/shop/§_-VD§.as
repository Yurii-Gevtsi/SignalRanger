package ru.pragmatix.wormix.gui.menu.shop
{
   import §_-31W§.§_-Kb§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-A2U§.§_-A23§;
   import §_-B1y§.§_-83E§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.*;
   import §_-C3f§.*;
   import §_-Cl§.*;
   import §_-DJ§.§_-fH§;
   import §_-F39§.*;
   import §_-N8§.*;
   import §_-YF§.§_-q2v§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-l1g§.§_-T2N§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-h2B§;
   import §_-r1C§.§_-t1R§;
   import §_-s20§.§_-J2g§;
   import §_-w2i§.§_-52k§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Button;
   import feathers.controls.IScrollBar;
   import feathers.controls.List;
   import feathers.controls.ScrollBarDisplayMode;
   import feathers.controls.ScrollInteractionMode;
   import feathers.controls.ScrollPolicy;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.events.FeathersEventType;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.TiledColumnsLayout;
   import feathers.layout.VerticalAlign;
   import flash.display.Shape;
   import flash.events.Event;
   import flash.events.IOErrorEvent;
   import flash.events.SecurityErrorEvent;
   import flash.media.Sound;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.gui.controls.MessageBox;
   import ru.pragmatix.wormix.controller.§_-C2c§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.system.ControlSystems;
   import ru.pragmatix.wormix.gui.§_-j2C§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanServiceResult;
   import ru.pragmatix.wormix.messages.clans.request.edit.ChangeClanDescriptionRequest;
   import ru.pragmatix.wormix.messages.clans.response.edit.ChangeClanDescriptionResponse;
   import ru.pragmatix.wormix.messages.clans.response.treasury.DonateClanResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.server.IncomingPayment;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.rewards.§_-P2q§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-6A§;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import ru.pragmatix.wormix.weapons.Weapon;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   public class §_-VD§ extends List
   {
      
      public static const §_-UX§:Number = 50;
      
      public static const §_-n13§:Number = -20;
      
      public static const §_-R2m§:Number = 10;
      
      public function §_-VD§()
      {
         super();
         addEventListener(FeathersEventType.CREATION_COMPLETE,this.§_-vD§);
      }
      
      private function §_-vD§(param1:starling.events.Event) : void
      {
         removeEventListener(FeathersEventType.CREATION_COMPLETE,this.§_-vD§);
         hasElasticEdges = viewPort.width > width;
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         var _loc1_:TiledColumnsLayout = new TiledColumnsLayout();
         _loc1_.horizontalGap = §_-R2m§;
         _loc1_.verticalGap = §_-n13§;
         _loc1_.verticalAlign = VerticalAlign.MIDDLE;
         _loc1_.padding = 0;
         _loc1_.paddingLeft = _loc1_.paddingRight = §_-UX§;
         layout = _loc1_;
         itemRendererType = §_-nx§;
         interactionMode = ScrollInteractionMode.TOUCH_AND_SCROLL_BARS;
         horizontalScrollPolicy = ScrollPolicy.ON;
         verticalScrollPolicy = ScrollPolicy.OFF;
         scrollBarDisplayMode = ScrollBarDisplayMode.FIXED_FLOAT;
         horizontalScrollStep = 1;
         itemRendererFactory = this.§_-g1M§;
         horizontalScrollBarFactory = this.§_-d5§;
      }
      
      override protected function draw() : void
      {
         var _loc1_:TiledColumnsLayout = null;
         var _loc2_:String = null;
         var _loc3_:int = 0;
         var _loc4_:int = 0;
         if(Boolean(width) && Boolean(height))
         {
            _loc1_ = _layout as TiledColumnsLayout;
            _loc2_ = HorizontalAlign.LEFT;
            _loc3_ = Math.floor(width / this.§_-M27§(this.§_-kV§()));
            _loc4_ = Math.floor((height + _loc1_.verticalGap) / (this.§_-kV§() + _loc1_.verticalGap));
            if(Boolean(dataProvider) && Math.ceil(dataProvider.length / _loc4_) <= _loc3_)
            {
               _loc2_ = HorizontalAlign.CENTER;
            }
            _loc1_.horizontalAlign = _loc2_;
         }
         super.draw();
      }
      
      private function §_-d5§() : IScrollBar
      {
         return new §_-d1b§(this);
      }
      
      private function §_-g1M§() : DefaultListItemRenderer
      {
         var _loc1_:§_-nx§ = new §_-nx§();
         if(height)
         {
            _loc1_.setSize(this.§_-M27§(this.§_-kV§()),this.§_-kV§());
         }
         return _loc1_;
      }
      
      private function §_-kV§() : Number
      {
         return 0.8 * (height - §_-VD§.§_-n13§) / 2;
      }
      
      private function §_-M27§(param1:Number) : Number
      {
         return §_-nx§.§_-72A§ * param1;
      }
      
      override public function dispose() : void
      {
         if(hasEventListener(FeathersEventType.CREATION_COMPLETE))
         {
            removeEventListener(FeathersEventType.CREATION_COMPLETE,this.§_-vD§);
         }
         super.dispose();
      }
   }
}

