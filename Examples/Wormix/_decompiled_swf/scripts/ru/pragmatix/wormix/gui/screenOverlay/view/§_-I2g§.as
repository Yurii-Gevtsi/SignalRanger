package ru.pragmatix.wormix.gui.screenOverlay.view
{
   import §_-hO§.§_-03k§;
   import §_-r1C§.§_-h2B§;
   import §_-u1T§.LocalizationService;
   import com.greensock.events.TweenEvent;
   import feathers.controls.LayoutGroup;
   import flash.events.Event;
   import flash.events.TimerEvent;
   import flash.utils.Timer;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-O1f§;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.utils.FrameTimer;
   import ru.pragmatix.wormix.weapons.ammo.VerticalDrillMissile;
   import ru.pragmatix.wormix.weapons.ammo.VerticalDrillStanding;
   import starling.display.Sprite;
   import starling.events.Event;
   import starlingbuilder.extensions.uicomponents.CustomLabel;
   
   public class §_-I2g§ implements §_-O1f§
   {
      
      private var _moneyContainer:Sprite;
      
      private var §_-R4§:LayoutGroup;
      
      private var §_-32b§:LayoutGroup;
      
      private var §_-62f§:CustomLabel;
      
      private var _rubyLabel:CustomLabel;
      
      private var §_-O24§:§_-r1d§;
      
      public function §_-I2g§(param1:Sprite)
      {
         super();
         this._moneyContainer = param1;
         var _loc2_:LocalizationService = §_-X1j§.localizationService;
         this.§_-R4§ = this._moneyContainer.getChildByName("fuzy") as LayoutGroup;
         if(this.§_-R4§)
         {
            _loc2_.§_-I2k§(this.§_-R4§,"toolTip",§_-s2a§.COIN_QTIP);
            this.§_-62f§ = this.§_-R4§.getChildByName("value") as CustomLabel;
         }
         this.§_-32b§ = this._moneyContainer.getChildByName("ruby") as LayoutGroup;
         if(this.§_-32b§)
         {
            _loc2_.§_-I2k§(this.§_-32b§,"toolTip",§_-s2a§.RUBY_QTIP);
            this._rubyLabel = this.§_-32b§.getChildByName("value") as CustomLabel;
         }
      }
      
      public function dispose() : void
      {
         this.§_-Y2v§();
         var _loc1_:LocalizationService = §_-X1j§.localizationService;
         if(this.§_-R4§)
         {
            _loc1_.removeObject(this.§_-R4§);
         }
         if(this.§_-32b§)
         {
            _loc1_.removeObject(this.§_-32b§);
         }
      }
      
      private function §_-53G§(param1:§_-r1d§) : void
      {
         var _loc2_:UserProfile = param1.userProfile;
         if(this.§_-62f§)
         {
            this.§_-62f§.text = _loc2_.§_-51e§().toString();
         }
         if(this._rubyLabel)
         {
            this._rubyLabel.text = _loc2_.§_-o1N§().toString();
         }
      }
      
      public function §_-32X§(param1:§_-r1d§) : void
      {
         this.§_-Y2v§();
         this.§_-O24§ = param1;
         this.§_-O24§.addEventListener(starling.events.Event.CHANGE,this.§_-E28§);
         this.§_-O24§.addEventListener(starling.events.Event.UPDATE,this.§_-Z2B§);
         this.§_-53G§(param1);
      }
      
      private function §_-Y2v§() : void
      {
         if(this.§_-O24§)
         {
            this.§_-O24§.removeEventListener(starling.events.Event.CHANGE,this.§_-E28§);
            this.§_-O24§.removeEventListener(starling.events.Event.UPDATE,this.§_-Z2B§);
            this.§_-O24§ = null;
         }
      }
      
      private function §_-Z2B§(param1:starling.events.Event) : void
      {
         this.§_-53G§(this.§_-O24§);
      }
      
      private function §_-E28§(param1:starling.events.Event) : void
      {
         var _loc2_:§_-r1d§ = param1.target as §_-r1d§;
         this.§_-53G§(_loc2_);
      }
   }
}

