package ru.pragmatix.wormix.controller
{
   import §_-5Y§.§_-h2c§;
   import §_-63U§.ArchidemonCage;
   import §_-931§.§_-02O§;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-LE§;
   import §_-A1K§.§_-TA§;
   import §_-CF§.§_-x2t§;
   import §_-H2g§.§_-di§;
   import §_-Ly§.§_-81L§;
   import §_-S§.*;
   import §_-X14§.§_-03Y§;
   import §_-Y1O§.*;
   import §_-Y1b§.*;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-ZB§.§_-A10§;
   import §_-ZP§.§_-H1H§;
   import §_-aM§.*;
   import §_-eW§.§_-S21§;
   import §_-eW§.§_-V2n§;
   import §_-eW§.§_-g2b§;
   import §_-j22§.§_-5g§;
   import §_-k1r§.*;
   import §_-k1u§.§_-Vr§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-h2B§;
   import §_-u13§.MAC;
   import §_-w2W§.*;
   import §_-w2i§.§_-52k§;
   import §_-z20§.*;
   import §_-zI§.§_-8D§;
   import com.hurlant.crypto.*;
   import com.hurlant.math.*;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Button;
   import feathers.controls.ButtonState;
   import feathers.controls.Label;
   import feathers.controls.List;
   import feathers.core.ITextRenderer;
   import flash.events.IOErrorEvent;
   import flash.events.TimerEvent;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataOutput;
   import flash.utils.Timer;
   import flash.utils.clearTimeout;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.controller.ISocialController;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.flox.social.utils.id.IPlatformAccountController;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanNewsTO;
   import ru.pragmatix.wormix.messages.client.BuyShopItems;
   import ru.pragmatix.wormix.messages.structures.RentedItemsStructure;
   import ru.pragmatix.wormix.messages.structures.ShopItemStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.rewards.§_-P2q§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.serialization.*;
   import ru.pragmatix.wormix.serialization.client.*;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.ammo.phase.HomingPhaseAmmo;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Quad;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starlingbuilder.engine.tween.WormixTweenBuilder;
   
   use namespace bi_internal;
   
   public class §_-C2c§ extends EventDispatcher
   {
      
      public static const CHEAT:String = "Time has been cheated";
      
      public static const §_-yU§:String = "Time difference from last activation";
      
      private static const §_-a1F§:§_-TA§ = new §_-TA§(2000);
      
      private static const §_-J2W§:§_-TA§ = new §_-TA§(100);
      
      private var timer:Timer;
      
      private var §_-43e§:§_-LE§ = new §_-LE§(0);
      
      public var paused:§_-7u§ = new §_-7u§(true);
      
      private var §_-B1q§:Number;
      
      private var §_-E3G§:§_-TA§ = new §_-TA§(0);
      
      private var §_-312§:Boolean;
      
      public function §_-C2c§()
      {
         super();
         this.§_-312§ = true;
      }
      
      public function run(param1:Number) : void
      {
         var _loc2_:uint = 0;
         this.stop();
         if(param1 > 0)
         {
            _loc2_ = uint(§_-a1F§.value / §_-J2W§.value);
            this.§_-43e§.value = Math.ceil(param1) * _loc2_ * §_-J2W§.value;
            this.§_-E3G§.value = Math.ceil(param1) * _loc2_;
            this.createTimer(§_-J2W§.value,Math.ceil(param1) * _loc2_);
            dispatchEventWith(Event.TRIGGERED);
         }
      }
      
      private function createTimer(param1:Number, param2:int) : void
      {
         this.timer = new Timer(param1,param2);
         this.timer.addEventListener(TimerEvent.TIMER,this.tick);
         this.timer.addEventListener(TimerEvent.TIMER_COMPLETE,this.§_-kJ§);
         this.§_-B1q§ = new Date().getTime();
         this.paused.value = false;
         this.timer.start();
      }
      
      private function §_-kJ§(param1:TimerEvent = null) : void
      {
         this.stop();
         this.§_-43e§.value = 0;
         dispatchEventWith(Event.COMPLETE);
      }
      
      private function tick(param1:TimerEvent) : void
      {
         var _loc2_:§_-TA§ = this.§_-E3G§;
         var _loc3_:Number = _loc2_.value - 1;
         _loc2_.value = _loc3_;
         this.§_-E3G§.value.toString();
         this.§_-12O§();
         dispatchEventWith(Event.CHANGE,false,this.§_-H3K§);
      }
      
      private function §_-12O§() : void
      {
         if(this.§_-E3G§.value < -3)
         {
            dispatchEventWith(CHEAT);
         }
         var _loc1_:Number = 0.9;
         var _loc2_:Number = new Date().getTime();
         var _loc3_:int = int(_loc2_ - this.§_-B1q§);
         if(_loc3_ < §_-J2W§.value * _loc1_)
         {
            _loc3_ = §_-J2W§.value;
         }
         this.§_-B1q§ = _loc2_;
         this.§_-43e§.value -= _loc3_ * §_-a1F§.value / §_-72u§.§_-E1O§;
         if(this.§_-43e§.value < 0)
         {
            this.§_-43e§.value = 0;
            this.§_-kJ§();
         }
      }
      
      public function pause() : void
      {
         if(!this.paused.value && Boolean(this.timer))
         {
            this.timer.stop();
            this.paused.value = true;
         }
      }
      
      public function resume() : void
      {
         if(this.paused.value && Boolean(this.timer))
         {
            this.§_-B1q§ = new Date().getTime();
            this.timer.start();
            this.paused.value = false;
         }
      }
      
      public function stop() : void
      {
         if(this.timer)
         {
            this.timer.stop();
            this.timer.removeEventListener(TimerEvent.TIMER,this.tick);
            this.timer.removeEventListener(TimerEvent.TIMER_COMPLETE,this.§_-kJ§);
         }
      }
      
      public function get §_-H3K§() : int
      {
         return Math.floor(this.§_-43e§.value / §_-a1F§.value);
      }
      
      public function set §_-T1a§(param1:Boolean) : void
      {
         this.§_-312§ = param1;
      }
      
      public function get running() : Boolean
      {
         return this.timer ? this.timer.running : false;
      }
      
      public function §_-k22§(param1:int) : void
      {
         var _loc2_:uint = 5 * §_-a1F§.value;
         if(this.§_-43e§.value > _loc2_)
         {
            this.§_-43e§.value = Math.max(this.§_-43e§.value - param1 * §_-a1F§.value,_loc2_);
         }
      }
   }
}

