package ru.pragmatix.wormix.gui.menu.shop.mobile
{
   import §_-62§.§_-W1r§;
   import §_-A1K§.§_-TA§;
   import §_-C3f§.§_-1Y§;
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-x2t§;
   import §_-l1g§.§_-T2N§;
   import §_-r1C§.§_-h2B§;
   import §_-z1g§.§_-F4§;
   import feathers.controls.Button;
   import feathers.controls.NumericStepper;
   import feathers.controls.Panel;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.events.Event;
   
   public class MobilePriceInfoPanel extends Panel
   {
      
      private var fuzyButton:Button;
      
      private var rubyButton:Button;
      
      private var adsButton:Button;
      
      private var §_-F1i§:NumericStepper;
      
      private var §_-52H§:§_-1Y§;
      
      private var _count:int;
      
      private var §_-xl§:int;
      
      private var §_-o2t§:Function;
      
      private var §_-k1L§:Function;
      
      private var _enabled:Boolean = true;
      
      public function MobilePriceInfoPanel()
      {
         super();
         this.§_-o2t§ = this.§_-Q1O§;
         this.§_-k1L§ = this.§_-Sv§;
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         this.§_-F1i§ = §_-YQ§.getClip("counter",this) as NumericStepper;
         if(!this.§_-F1i§)
         {
            this.§_-F1i§ = new NumericStepper();
         }
         this.fuzyButton = §_-YQ§.getClip("fuzyButton",this) as Button;
         this.rubyButton = §_-YQ§.getClip("rubyButton",this) as Button;
         this.adsButton = §_-YQ§.getClip("adsButton",this) as Button;
         if(this.adsButton)
         {
            this.adsButton.visible = this.adsButton.includeInLayout = false;
         }
         if(this.adsButton)
         {
            this.adsButton.label = §_-s2a§.§_-K3t§(§_-s2a§.FREE_LABEL);
         }
         this.§_-F1i§.minimum = 1;
         this.§_-F1i§.maximum = 99;
         this.§_-F1i§.textInputProperties.isEditable = false;
         this.§_-F1i§.textInputProperties.isSelectable = false;
         var _loc1_:Number = this.§_-F1i§.repeatDelay;
         this.fuzyButton.addEventListener(Event.TRIGGERED,this.§_-90§);
         this.rubyButton.addEventListener(Event.TRIGGERED,this.§_-90§);
         this.§_-F1i§.addEventListener(Event.CHANGE,this.§_-r1m§);
      }
      
      override public function dispose() : void
      {
         super.dispose();
         this.fuzyButton.removeEventListener(Event.TRIGGERED,this.§_-90§);
         this.rubyButton.removeEventListener(Event.TRIGGERED,this.§_-90§);
         this.§_-F1i§.removeEventListener(Event.CHANGE,this.§_-r1m§);
      }
      
      public function setItem(param1:§_-1Y§, param2:int = 1, param3:int = 1) : void
      {
         this.§_-52H§ = param1;
         this._count = param2;
         this.§_-xl§ = param3;
         invalidate(INVALIDATION_FLAG_DATA);
      }
      
      override protected function draw() : void
      {
         var _loc1_:§_-x2t§ = null;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         var _loc4_:§_-W1r§ = null;
         super.draw();
         if(isInvalid(INVALIDATION_FLAG_DATA) && Boolean(this.§_-52H§))
         {
            _loc1_ = this.§_-52H§.getPrice();
            _loc2_ = _loc1_.§_-R1T§ * this._count;
            _loc3_ = _loc1_.realPrice * this._count;
            _loc4_ = this.§_-52H§ as §_-W1r§;
            this.fuzyButton.label = _loc2_.toString();
            this.rubyButton.label = _loc3_.toString();
            this.§_-F1i§.value = this._count * this.§_-xl§;
            this.§_-F1i§.minimum = this.§_-xl§;
            this.§_-F1i§.step = this.§_-xl§;
            §_-YQ§.§_-Xv§(this.fuzyButton,_loc2_ > 0);
            §_-YQ§.§_-Xv§(this.rubyButton,_loc3_ > 0);
            §_-YQ§.§_-Xv§(this.§_-F1i§,Boolean(_loc4_) && _loc4_.§_-K2N§());
            this.fuzyButton.isEnabled = this._enabled && Boolean(this.checkFuzyButtonEnabledFunction());
            this.rubyButton.isEnabled = this._enabled && Boolean(this.checkRubyButtonEnabledFunction());
         }
      }
      
      private function §_-90§(param1:Event) : void
      {
         var _loc2_:§_-E19§ = null;
         §_-h2B§.play(§_-d27§.§_-sg§);
         if(param1.currentTarget == this.fuzyButton)
         {
            _loc2_ = §_-E19§.§_-i2d§;
         }
         else if(param1.currentTarget == this.rubyButton)
         {
            _loc2_ = §_-E19§.§_-51D§;
         }
         if(_loc2_)
         {
            dispatchEvent(new §_-T2N§(this.§_-52H§,_loc2_,this._count * this.§_-xl§));
         }
      }
      
      private function §_-r1m§(param1:Event) : void
      {
         var _loc2_:int = (param1.currentTarget as NumericStepper).value / this.§_-xl§;
         if(_loc2_ != this._count)
         {
            this._count = _loc2_;
            §_-h2B§.play(§_-d27§.§_-S29§);
            invalidate(INVALIDATION_FLAG_DATA);
         }
      }
      
      private function §_-Q1O§() : Boolean
      {
         var _loc1_:Boolean = false;
         var _loc2_:Boolean = false;
         if(this.§_-52H§)
         {
            _loc1_ = §_-F4§.§_-EY§(this.§_-52H§);
            _loc2_ = §_-F4§.§_-f16§(this.§_-52H§);
            return _loc1_ && _loc2_;
         }
         return false;
      }
      
      private function §_-Sv§() : Boolean
      {
         var _loc1_:Boolean = false;
         if(this.§_-52H§)
         {
            return §_-F4§.§_-EY§(this.§_-52H§);
         }
         return false;
      }
      
      public function get checkFuzyButtonEnabledFunction() : Function
      {
         return this.§_-o2t§;
      }
      
      public function set checkFuzyButtonEnabledFunction(param1:Function) : void
      {
         this.§_-o2t§ = param1;
      }
      
      public function get checkRubyButtonEnabledFunction() : Function
      {
         return this.§_-k1L§;
      }
      
      public function set checkRubyButtonEnabledFunction(param1:Function) : void
      {
         this.§_-k1L§ = param1;
      }
      
      public function set enabled(param1:Boolean) : void
      {
         this._enabled = param1;
         invalidate(INVALIDATION_FLAG_DATA);
      }
   }
}

