package ru.pragmatix.wormix.gui.menu.shop
{
   import §_-31W§.§_-Kb§;
   import §_-52L§.§_-u2x§;
   import §_-5Y§.*;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-T1y§;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-a3§;
   import §_-62§.§_-ou§;
   import §_-62§.§_-zF§;
   import §_-91B§.§_-z2l§;
   import §_-9e§.§_-73e§;
   import §_-9e§.§_-8O§;
   import §_-9e§.§_-A18§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-A2U§.*;
   import §_-CF§.§_-E19§;
   import §_-DJ§.§_-J2l§;
   import §_-G2H§.*;
   import §_-Ly§.§_-81L§;
   import §_-N8§.*;
   import §_-d26§.§_-d1H§;
   import §_-j1w§.§_-B2i§;
   import §_-j1y§.§_-i1b§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.*;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-h2B§;
   import §_-r1C§.§_-t1R§;
   import §_-s20§.§_-J2g§;
   import §_-sQ§.§_-H3D§;
   import §_-x2Q§.§_-dU§;
   import com.distriqt.extension.inappbilling.Product;
   import com.distriqt.extension.inappbilling.SubscriptionOffer;
   import com.distriqt.extension.inappbilling.SubscriptionPhase;
   import com.distriqt.extension.inappbilling.events.ProductEvent;
   import dragonBones.objects.DragonBonesData;
   import feathers.controls.Button;
   import feathers.controls.LayoutGroup;
   import feathers.controls.ProgressBar;
   import feathers.controls.Scroller;
   import feathers.controls.SimpleScrollBar;
   import feathers.core.ITextRenderer;
   import feathers.data.ListCollection;
   import feathers.layout.Direction;
   import feathers.utils.math.roundToNearest;
   import flash.geom.Point;
   import flash.media.Sound;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.server.IncomingPayment;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.UnitPhysicModel;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-F2L§;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import ru.pragmatix.wormix.weapons.*;
   import starling.animation.Transitions;
   import starling.animation.Tween;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.Image;
   import starling.display.Quad;
   import starling.events.Event;
   import starling.events.TouchEvent;
   import starling.events.TouchPhase;
   import starling.textures.Texture;
   import starling.utils.Color;
   import starling.utils.MathUtil;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   public class §_-d1b§ extends SimpleScrollBar
   {
      
      private const §_-31C§:String = "scrollbar_btn_default";
      
      private var §_-2§:DisplayObject;
      
      private var §_-U1p§:Scroller;
      
      private var §_-VO§:Number = 0;
      
      private var §_-13U§:Tween;
      
      private var §_-yP§:Boolean = false;
      
      public function §_-d1b§(param1:Scroller)
      {
         super();
         this.§_-2§ = this.§_-g1t§();
         addChild(this.§_-2§);
         this.§_-U1p§ = param1;
         _direction = Direction.HORIZONTAL;
         _thumbFactory = this.§_-pS§;
         liveDragging = true;
         this.§_-13U§ = new Tween(this,1,Transitions.EASE_OUT);
      }
      
      public static function §_-43p§() : int
      {
         return SystemUtil.isDesktop ? 20 : 10;
      }
      
      private function §_-g1t§() : DisplayObject
      {
         var _loc1_:Quad = null;
         if(SystemUtil.isDesktop)
         {
            _loc1_ = new Quad(10,10,Color.BLACK);
            _loc1_.alpha = 0.3;
            return _loc1_;
         }
         return new LayoutGroup();
      }
      
      override public function set maximum(param1:Number) : void
      {
         super.maximum = param1;
         this.§_-2§.visible = param1 > 0;
      }
      
      override protected function adjustPage() : void
      {
         var _loc3_:Number = NaN;
         var _loc1_:Number = this._maximum - this._minimum;
         var _loc2_:Number = this._page;
         if(_loc2_ === 0)
         {
            _loc2_ = this._step;
         }
         if(_loc2_ > _loc1_)
         {
            _loc2_ = _loc1_;
         }
         if(this._touchValue < this._value)
         {
            _loc3_ = Math.max(this._touchValue,this._value - _loc2_);
            if(this._step != 0 && _loc3_ != this._maximum && _loc3_ != this._minimum)
            {
               _loc3_ = roundToNearest(_loc3_,this._step);
            }
            this.§_-U1p§.scrollToPosition(_loc3_,this.§_-U1p§.verticalScrollPosition,2 * (Math.abs(_loc3_ - _value) / _maximum));
         }
         else if(this._touchValue > this._value)
         {
            _loc3_ = Math.min(this._touchValue,this._value + _loc2_);
            if(this._step != 0 && _loc3_ != this._maximum && _loc3_ != this._minimum)
            {
               _loc3_ = roundToNearest(_loc3_,this._step);
            }
            this.§_-U1p§.scrollToPosition(_loc3_,this.§_-U1p§.verticalScrollPosition,2 * (Math.abs(_loc3_ - _value) / _maximum));
         }
      }
      
      private function §_-pS§() : Button
      {
         var _loc1_:Button = new Button();
         var _loc2_:Texture = this.§_-I17§();
         var _loc3_:Image = new Image(_loc2_);
         _loc3_.scale9Grid = Pool.getRectangle(16,Math.floor((_loc2_.height - 1) * 0.5),1,1);
         _loc1_.defaultSkin = _loc3_;
         return _loc1_;
      }
      
      private function §_-I17§() : Texture
      {
         return Application.assetManager.getTexture(this.§_-31C§);
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         this.§_-2§.width = this.§_-U1p§.width;
         this.§_-2§.height = §_-43p§();
         this.§_-2§.x = (this.§_-U1p§.width - this.§_-2§.width) * 0.5;
         _paddingLeft = _paddingRight = this.§_-2§.x;
         width = this.§_-2§.width;
         height = this.§_-2§.height;
      }
      
      override public function dispose() : void
      {
         if(this.§_-yP§)
         {
            this.§_-O1k§();
         }
         this.§_-2§ = null;
         this.§_-U1p§ = null;
         super.dispose();
      }
      
      override public function set value(param1:Number) : void
      {
         this.§_-VO§ = param1 - _value;
         super.value = param1;
      }
      
      override protected function locationToValue(param1:Point) : Number
      {
         if(this.§_-yP§)
         {
            this.§_-O1k§();
         }
         return super.locationToValue(param1);
      }
      
      override protected function thumb_touchHandler(param1:TouchEvent) : void
      {
         if(param1.getTouch(thumb,TouchPhase.ENDED))
         {
            this.§_-017§(_value + this.§_-VO§ * 2);
         }
         super.thumb_touchHandler(param1);
      }
      
      private function §_-017§(param1:Number) : void
      {
         this.§_-13U§.reset(this,0.5,Transitions.EASE_OUT);
         this.§_-13U§.animate("value",starling.utils.MathUtil.clamp(param1,_minimum,_maximum));
         Starling.juggler.add(this.§_-13U§);
         this.§_-yP§ = true;
      }
      
      private function §_-O1k§() : void
      {
         Starling.juggler.remove(this.§_-13U§);
         this.§_-yP§ = false;
      }
   }
}

