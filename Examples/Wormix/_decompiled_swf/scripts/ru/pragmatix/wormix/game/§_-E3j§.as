package ru.pragmatix.wormix.game
{
   import §_-11L§.*;
   import §_-12W§.*;
   import §_-21p§.§_-4w§;
   import §_-62§.§_-W1r§;
   import §_-82a§.*;
   import §_-931§.§_-02O§;
   import §_-9e§.§_-8O§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.§_-63i§;
   import §_-C2t§.§_-92W§;
   import §_-C3f§.§_-yG§;
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-p2U§;
   import §_-CF§.§_-x2t§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-F39§.§_-93t§;
   import §_-H2g§.§_-di§;
   import §_-L1H§.§_-C3e§;
   import §_-L1H§.§_-b2K§;
   import §_-Nq§.*;
   import §_-P1z§.*;
   import §_-TF§.§_-q1j§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-XA§;
   import §_-Y1O§.*;
   import §_-YF§.§_-Zi§;
   import §_-Z1K§.§_-q24§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-aM§.*;
   import §_-j1w§.§_-L1x§;
   import §_-j22§.§_-5g§;
   import §_-k2s§.§_-8c§;
   import §_-k2s§.§_-C3A§;
   import §_-k2s§.§_-fG§;
   import §_-l1K§.§_-F3m§;
   import §_-l1K§.§_-I7§;
   import §_-l1K§.§_-m2§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-s20§.§_-J2g§;
   import §_-w2i§.§_-Ia§;
   import com.greensock.loading.core.LoaderCore;
   import com.hurlant.math.*;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Button;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.core.ITextRenderer;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.RelativePosition;
   import flash.display.DisplayObject;
   import flash.events.Event;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.gui.controls.§_-NY§;
   import ru.pragmatix.wormix.controller.§_-C2c§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.request.edit.RenameClanRequest;
   import ru.pragmatix.wormix.messages.clans.response.edit.RenameClanResponse;
   import ru.pragmatix.wormix.messages.client.PumpReactionRate;
   import ru.pragmatix.wormix.messages.client.WipeProfile;
   import ru.pragmatix.wormix.messages.structures.CostStructure;
   import ru.pragmatix.wormix.model.§_-4V§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.model.rewards.§_-qU§;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattle;
   import ru.pragmatix.wormix.pvp.messages.server.PvpEndBattle;
   import ru.pragmatix.wormix.pvp.messages.server.PvpRetryCommandRequestServer;
   import ru.pragmatix.wormix.pvp.messages.server.PvpStartTurn;
   import ru.pragmatix.wormix.pvp.messages.server.PvpSystemMessage;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-R2g§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.utils.§_-k1h§;
   import ru.pragmatix.wormix.weapons.ammo.JumpingMine;
   import ru.pragmatix.wormix.weapons.ammo.Ufo;
   import ru.pragmatix.wormix.weapons.ammo.feats.IFrameFeat;
   import ru.pragmatix.wormix.weapons.ammo.phase.HarmPhase;
   import starling.display.DisplayObject;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.Color;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   import starling.utils.SystemUtil;
   
   use namespace bi_internal;
   
   public class §_-E3j§
   {
      
      private static const §_-lP§:int = 20000;
      
      public var timer:§_-C2c§;
      
      private var display:§_-93t§;
      
      private var settings:§_-8O§;
      
      private var onComplete:Function;
      
      private var §_-y22§:§_-TA§;
      
      public function §_-E3j§(param1:§_-93t§, param2:§_-8O§)
      {
         super();
         this.display = param1;
         this.settings = param2;
         this.§_-y22§ = new §_-TA§(param2.totalTime);
      }
      
      public function start(param1:Function) : void
      {
         this.stop();
         if(!this.timer)
         {
            this.createTimer();
         }
         if(this.§_-y22§.value != 0)
         {
            if(this.display)
            {
               this.display.show(this.§_-y22§.value);
            }
            this.onComplete = param1;
            this.timer.run(this.§_-y22§.value);
            this.timer.§_-T1a§ = this.§_-n2s§;
         }
      }
      
      private function createTimer() : void
      {
         this.timer = new §_-C2c§();
         this.timer.addEventListener(§_-C2c§.CHEAT,this.§_-OK§);
         this.timer.addEventListener(§_-C2c§.§_-yU§,this.§_-I3j§);
         this.timer.addEventListener(starling.events.Event.COMPLETE,this.§_-kJ§);
         this.timer.addEventListener(starling.events.Event.CHANGE,this.onTimer);
      }
      
      private function §_-I3j§(param1:starling.events.Event) : void
      {
         var _loc2_:Number = Number(param1.data);
         if(_loc2_ < 0)
         {
            §_-X1j§.§_-12n§.§_-Q2l§(§_-J1§.§_-23m§,§_-XA§.SPEED_HACK_CHEATER);
         }
         else if(_loc2_ > §_-lP§ && this.§_-n2s§)
         {
            §_-X1j§.§_-12n§.§_-Q2l§(§_-J1§.§_-23m§,§_-XA§.SPEED_HACK_CHEATER);
         }
      }
      
      private function §_-OK§(param1:starling.events.Event) : void
      {
         §_-X1j§.§_-12n§.scene.dispatchEvent(new §_-H1K§(§_-A2V§.§_-A3T§,"negative quants"));
      }
      
      public function pause() : void
      {
         var _loc1_:§_-eX§ = §_-X1j§.§_-12n§.gameMaster;
         if(Boolean(_loc1_) && Boolean(!_loc1_.§_-W2j§().online) || !this.display)
         {
            if(this.timer)
            {
               this.timer.pause();
            }
         }
      }
      
      public function resume() : void
      {
         var _loc1_:§_-eX§ = §_-X1j§.§_-12n§.gameMaster;
         if(Boolean(_loc1_) && !_loc1_.§_-W2j§().online)
         {
            if(this.timer)
            {
               this.timer.resume();
            }
         }
      }
      
      public function stop(param1:Boolean = true) : void
      {
         if(param1 && Boolean(this.display))
         {
            this.display.hide();
         }
         if(this.timer)
         {
            this.timer.stop();
            this.onComplete = null;
         }
      }
      
      public function §_-01q§(param1:int) : void
      {
         if(this.settings.§_-z2t§ > 0 && param1 >= this.settings.§_-z2t§ && (param1 - this.settings.§_-z2t§) % this.settings.§_-R1x§ == 0 && this.§_-y22§.value > this.settings.§_-82Q§)
         {
            this.§_-y22§.value = Math.max(this.settings.§_-82Q§,this.§_-y22§.value - this.settings.§_-J1e§);
         }
      }
      
      private function onTimer(param1:starling.events.Event) : void
      {
         if(!§_-X1j§.§_-12n§.scene)
         {
            return;
         }
         var _loc2_:int = int(param1.data);
         if(Boolean(this.display) && _loc2_ != this.display.time)
         {
            this.display.time = _loc2_;
            if(this.display.time <= 5)
            {
               §_-h2B§.play(§_-d27§.§_-27§);
            }
         }
      }
      
      public function §_-kJ§(param1:starling.events.Event = null) : void
      {
         var _loc2_:Function = this.onComplete;
         this.stop();
         if(_loc2_ != null)
         {
            _loc2_();
         }
      }
      
      public function get isStopped() : Boolean
      {
         return !this.timer || !this.timer.running;
      }
      
      public function §_-k22§(param1:int) : void
      {
         if(this.timer)
         {
            this.timer.§_-k22§(param1);
         }
      }
      
      private function get §_-n2s§() : Boolean
      {
         return Boolean(§_-X1j§.§_-12n§.gameMaster) && Boolean(§_-X1j§.§_-12n§.gameMaster.§_-W2j§()) ? §_-X1j§.§_-12n§.gameMaster.§_-W2j§().online : false;
      }
      
      public function dispose() : void
      {
         if(this.timer)
         {
            this.timer.removeEventListener(§_-C2c§.CHEAT,this.§_-OK§);
            this.timer.removeEventListener(§_-C2c§.§_-yU§,this.§_-I3j§);
            this.timer.removeEventListener(starling.events.Event.COMPLETE,this.§_-kJ§);
            this.timer.removeEventListener(starling.events.Event.CHANGE,this.onTimer);
            this.stop();
         }
         this.timer = null;
      }
   }
}

