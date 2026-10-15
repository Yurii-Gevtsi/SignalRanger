package ru.pragmatix.wormix.controller.analytics
{
   import §_-ZB§.§_-W2Q§;
   import flash.events.Event;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import starling.events.Event;
   
   public class DummyAnalyticsController implements §_-W2Q§
   {
      
      protected var _isEnabled:Boolean = true;
      
      private var tracking:Boolean;
      
      private var currency:String;
      
      private var userId:String;
      
      public function DummyAnalyticsController()
      {
         super();
      }
      
      protected function init() : void
      {
         if(!this.userId || !this.currency)
         {
            return;
         }
         Application.§_-i1N§.addEventListener(flash.events.Event.ACTIVATE,this.onStageActivated);
         Application.§_-i1N§.addEventListener(flash.events.Event.DEACTIVATE,this.onStageDeactivated);
         Application.§_-i1N§.addEventListener(flash.events.Event.SUSPEND,this.onStageSuspended);
      }
      
      public function dispose() : void
      {
         Application.§_-i1N§.removeEventListener(flash.events.Event.ACTIVATE,this.onStageActivated);
         Application.§_-i1N§.removeEventListener(flash.events.Event.DEACTIVATE,this.onStageDeactivated);
         Application.§_-i1N§.removeEventListener(flash.events.Event.SUSPEND,this.onStageSuspended);
         this.stopTracking();
      }
      
      private function onStageSuspended(param1:starling.events.Event) : void
      {
         this.stopTracking();
      }
      
      private function onStageDeactivated(param1:starling.events.Event) : void
      {
         this.stopTracking();
      }
      
      private function onStageActivated(param1:starling.events.Event) : void
      {
         this.startTracking();
      }
      
      public function startTracking() : void
      {
         if(!this._isEnabled)
         {
            return;
         }
         if(this.tracking)
         {
            return;
         }
         this.tracking = true;
      }
      
      public function stopTracking() : void
      {
         if(!this.tracking)
         {
            return;
         }
         this.tracking = false;
      }
      
      private function eventHandler(param1:flash.events.Event) : void
      {
      }
      
      public function track(param1:String, param2:Object) : void
      {
      }
      
      public function trackPurchased(param1:PaymentOption) : void
      {
      }
      
      public function trackSubscribe(param1:PaymentOption, param2:Boolean = false) : void
      {
      }
      
      public function trackLevelup() : void
      {
      }
      
      public function trackMoney(param1:int, param2:int, param3:String, param4:String = "", param5:String = "") : void
      {
      }
      
      public function trackAchieve(param1:int, param2:String) : void
      {
      }
      
      public function getUID() : String
      {
         return "";
      }
      
      public function setUserId(param1:String) : void
      {
      }
      
      public function setCurrency(param1:String) : void
      {
      }
      
      public function trackEvent(param1:String, param2:String) : void
      {
      }
      
      public function set isEnabled(param1:Boolean) : void
      {
         this._isEnabled = param1;
      }
   }
}

