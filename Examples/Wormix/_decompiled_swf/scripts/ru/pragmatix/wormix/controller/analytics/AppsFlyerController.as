package ru.pragmatix.wormix.controller.analytics
{
   import config.§_-vR§;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.platform.§_-WK§;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-c2§;
   import starling.utils.SystemUtil;
   
   public class AppsFlyerController extends DummyAnalyticsController
   {
      
      private static const DEVKEY:String = "PPNfDWwQpiuBzXTgwR6Gm3";
      
      private static var _isEnabled:Boolean = false;
      
      private var tracking:Boolean;
      
      private var currency:String;
      
      private var userId:String;
      
      private var _appsFlyer:§_-96§;
      
      public function AppsFlyerController()
      {
         super();
         if(!§_-vR§.§_-42H§)
         {
            return;
         }
      }
      
      override protected function init() : void
      {
         super.init();
         if(this._appsFlyer)
         {
            return;
         }
         if(!this.userId || !this.currency)
         {
            return;
         }
         this._appsFlyer = new §_-96§();
         this._appsFlyer.§_-a2F§();
         this._appsFlyer.addEventListener(§_-z10§.§_-S2Z§,this.eventHandler);
         this._appsFlyer.addEventListener(§_-z10§.§_-C1F§,this.eventHandler);
         this._appsFlyer.addEventListener(§_-z10§.§_-3x§,this.eventHandler);
         this._appsFlyer.addEventListener(§_-z10§.§_-V2I§,this.eventHandler);
         this._appsFlyer.addEventListener(§_-z10§.§_-x1o§,this.eventHandler);
         this._appsFlyer.addEventListener(§_-z10§.§_-e2x§,this.eventHandler);
         this._appsFlyer.§_-s1G§(this.userId);
         switch(SystemUtil.platform)
         {
            case §_-WK§.IOS:
               this._appsFlyer.init(DEVKEY,§_-vR§.§_-Sp§);
               break;
            case §_-WK§.§_-c1l§:
               this._appsFlyer.init(DEVKEY,null);
         }
         this._appsFlyer.setCurrency(this.currency);
         this._appsFlyer.§_-D1V§();
         this._appsFlyer.§_-N2X§();
      }
      
      override public function dispose() : void
      {
         super.dispose();
         if(this._appsFlyer)
         {
            this._appsFlyer.removeEventListener(§_-z10§.§_-S2Z§,this.eventHandler);
            this._appsFlyer.removeEventListener(§_-z10§.§_-C1F§,this.eventHandler);
            this._appsFlyer.removeEventListener(§_-z10§.§_-3x§,this.eventHandler);
            this._appsFlyer.removeEventListener(§_-z10§.§_-V2I§,this.eventHandler);
            this._appsFlyer.removeEventListener(§_-z10§.§_-x1o§,this.eventHandler);
            this._appsFlyer.removeEventListener(§_-z10§.§_-e2x§,this.eventHandler);
            this._appsFlyer = null;
         }
      }
      
      override public function startTracking() : void
      {
         if(!_isEnabled)
         {
            return;
         }
         if(this.tracking)
         {
            return;
         }
         super.startTracking();
         if(!this._appsFlyer)
         {
            return;
         }
         this._appsFlyer.§_-D1V§();
      }
      
      override public function stopTracking() : void
      {
         if(!this.tracking)
         {
            return;
         }
         super.stopTracking();
         if(!this._appsFlyer)
         {
            return;
         }
         this._appsFlyer.stopTracking(true);
      }
      
      override public function track(param1:String, param2:Object) : void
      {
         if(!_isEnabled || !this._appsFlyer || !§_-vR§.§_-42H§)
         {
            return;
         }
         var _loc3_:String = JSON.stringify(param2);
         if(this._appsFlyer)
         {
            this._appsFlyer.trackEvent(param1,_loc3_);
         }
      }
      
      override public function trackPurchased(param1:PaymentOption) : void
      {
         var _loc2_:PaymentOption = §_-N1V§.§_-42k§.§_-B12§(param1.name);
         var _loc3_:Number = §_-c2§.§_-JT§(_loc2_);
         var _loc4_:Object = {};
         _loc4_[AppsFlyerParams.CONTENT_ID] = param1.code.toString();
         _loc4_[AppsFlyerParams.CONTENT_TYPE] = §_-c2§.§_-X2h§(param1);
         _loc4_[AppsFlyerParams.REVENUE] = _loc3_.toFixed(2);
         _loc4_[AppsFlyerParams.CURRENCY] = "USD";
         this.track(Analytics.PURCHASE,_loc4_);
      }
      
      override public function trackSubscribe(param1:PaymentOption, param2:Boolean = false) : void
      {
         var _loc3_:PaymentOption = §_-X1j§.§_-42k§.§_-B12§(param1.name);
         var _loc4_:Number = §_-c2§.§_-JT§(_loc3_);
         var _loc5_:Object = {};
         _loc5_[AppsFlyerParams.CONTENT_ID] = param1.code.toString();
         _loc5_[AppsFlyerParams.CONTENT_TYPE] = §_-c2§.§_-X2h§(param1);
         _loc5_[AppsFlyerParams.REVENUE] = _loc4_.toFixed(2);
         _loc5_[AppsFlyerParams.CURRENCY] = "USD";
         this.track(AppsFlyerParams.AF_SUBSCRIBE,_loc5_);
      }
      
      override public function trackLevelup() : void
      {
         var _loc1_:Object = {};
         _loc1_[Analytics.CHARACTER] = Application.userProfile.§_-P2e§().getName();
         _loc1_[Analytics.LEVEL] = Application.userProfile.getLevel();
         this.track(Analytics.LEVEL_UP,_loc1_);
      }
      
      override public function trackMoney(param1:int, param2:int, param3:String, param4:String = "", param5:String = "") : void
      {
         var _loc6_:Object = {};
         if(param4)
         {
            _loc6_[param4] = param5;
         }
         if(param1 != 0)
         {
            _loc6_[Analytics.VIRTUAL_CURRENCY_NAME] = "ingameMoney";
            _loc6_[Analytics.VALUE] = param1;
            this.track(param3,_loc6_);
         }
         if(param2 != 0)
         {
            _loc6_[Analytics.VIRTUAL_CURRENCY_NAME] = "realMoney";
            _loc6_[Analytics.VALUE] = param2;
            this.track(param3,_loc6_);
         }
      }
      
      override public function trackAchieve(param1:int, param2:String) : void
      {
         var _loc3_:Object = {};
         _loc3_[Analytics.ACHIEVEMENT_ID] = param1;
         this.track(param2,_loc3_);
      }
      
      override public function getUID() : String
      {
         return this._appsFlyer ? this._appsFlyer.§_-A1S§() : "";
      }
      
      override public function setUserId(param1:String) : void
      {
         this.userId = param1;
         this.init();
      }
      
      override public function trackEvent(param1:String, param2:String) : void
      {
         if(this._appsFlyer)
         {
            this._appsFlyer.trackEvent(Analytics.TUTORIAL_BEGIN,"");
         }
      }
      
      override public function setCurrency(param1:String) : void
      {
         if(param1.indexOf("₽") > -1)
         {
            param1 = "RUB";
         }
         this.currency = param1;
         this.init();
      }
      
      private function eventHandler(param1:§_-z10§) : void
      {
      }
   }
}

