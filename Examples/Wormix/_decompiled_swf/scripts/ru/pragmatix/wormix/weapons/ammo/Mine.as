package ru.pragmatix.wormix.weapons.ammo
{
   import §_-31N§.§_-42G§;
   import §_-31Y§.§_-D3B§;
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import §_-b1F§.§_-v2Q§;
   import §_-hO§.§_-03k§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-m0§.§_-X1F§;
   import §_-r1C§.§_-V1R§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Zd§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.game.ai.§_-Q2U§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.FrameTimer;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-F2L§;
   import starling.animation.Tween;
   import starling.core.Starling;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.Color;
   
   public class Mine extends GenericGrenade implements §_-Q2U§
   {
      
      protected static const DEFAULT_ROAMING_DELAY:uint = 55;
      
      protected var enemyUnitsVec:Vector.<§_-01J§> = new Vector.<§_-01J§>(0);
      
      protected var explosionDelay:§_-TA§ = new §_-TA§(20);
      
      protected var mineDistance:§_-TA§ = new §_-TA§(0);
      
      protected var activator:§_-01J§;
      
      protected var roaming:Boolean = false;
      
      private var exploding:Boolean = false;
      
      private var explosionCounter:int = 0;
      
      private var setupTimer:FrameTimer;
      
      private var setupEventFired:Boolean = false;
      
      private var beepSound:§_-V1R§;
      
      protected var setupTimerCompleted:Boolean = false;
      
      protected var mineStopped:Boolean = false;
      
      protected var friendlyFire:Boolean;
      
      private var maxTurn:§_-TA§ = new §_-TA§(-1);
      
      private var curTurn:§_-TA§ = new §_-TA§(0);
      
      public function Mine(param1:§_-01J§, param2:int, param3:int, param4:§_-C3e§, param5:Boolean = true, param6:Boolean = true, param7:uint = 55)
      {
         this.friendlyFire = param6;
         var _loc8_:Number = -MathUtil.PI_HALF + (param1 ? param1.direction : 1) * MathUtil.PI_QUARTER;
         super(param1,param2,param3,param4,_loc8_,2,0.2,0,-1);
         tags = new <§_-D3B§>[§_-D3B§.§_-f1U§];
         §_-32§ = §_-h2D§;
         this.mineDistance.value = param4.damageRadius.value + 1;
         §_-Y2I§.keepUnstable = true;
         if(param5)
         {
            this.setupTimer = new FrameTimer(param7,1);
            this.setupTimer.addEventListener(§_-03k§.§_-cE§,this.onSetupTimer);
            this.setupTimer.start();
            §_-Zd§.once(§_-Y2I§ as EventDispatcher,§_-42G§.§_-X1C§,this.onStopWhileSetup);
         }
         else
         {
            this.roaming = true;
         }
         clip.rotation = Math.random() * 360;
         §_-Y2I§.addEventListener(§_-42G§.§_-K2J§,this.onStopMoving);
         §_-X1j§.§_-12n§.scene.addEventListener(§_-L3Q§.SOT,this.nextTurn);
         this.enemyUnitsVec = new Vector.<§_-01J§>(0);
         this.updateEnemiesVec();
      }
      
      protected function onSetupTimer(param1:Event) : void
      {
         this.setupTimerCompleted = true;
         this.checkSetup();
      }
      
      protected function updateEnemiesVec() : void
      {
         var _loc1_:§_-eX§ = §_-X1j§.§_-12n§.gameMaster;
         if(_loc1_)
         {
            this.enemyUnitsVec = this.friendlyFire ? _loc1_.§_-62V§() : _loc1_.§_-D2A§(owner);
         }
      }
      
      private function onStopWhileSetup(param1:Event) : void
      {
         this.mineStopped = true;
         this.checkSetup();
      }
      
      protected function checkSetup() : void
      {
         if(!disposed && this.setupTimerCompleted && this.mineStopped)
         {
            this.roaming = true;
         }
      }
      
      private function nextTurn(param1:Event) : void
      {
         this.updateEnemiesVec();
         if(this.maxTurn.value != -1 && ++this.curTurn.value >= this.maxTurn.value)
         {
            this.explode();
         }
      }
      
      override protected function onStopMoving(param1:Event) : void
      {
         §_-32§ = §_-8G§;
      }
      
      override public function nextState() : void
      {
         super.nextState();
         if(!§_-53T§)
         {
            this.tick();
         }
      }
      
      protected function tick() : void
      {
         if(this.roaming)
         {
            this.activator = this.findTarget();
            if(this.activator)
            {
               this.exploding = true;
               this.roaming = false;
               this.beforeTrigger();
               if(!this.setupEventFired)
               {
                  this.setupEventFired = true;
                  dispatchEventWith(§_-X1F§.§_-C3Z§(),false,§_-X1F§.create(this,true));
               }
               return;
            }
            if(!this.setupEventFired)
            {
               this.setupEventFired = true;
               §_-Y2I§.keepUnstable = false;
               dispatchEventWith(§_-X1F§.§_-C3Z§(),false,§_-X1F§.create(this,false));
            }
         }
         if(this.exploding)
         {
            if(this.explosionCounter == 0)
            {
               this.beforeExplosion();
            }
            ++this.explosionCounter;
            this.onExplosionCounter(this.explosionCounter);
         }
         if(this.explosionCounter >= this.explosionDelay.value)
         {
            this.explode();
         }
      }
      
      protected function findTarget() : §_-01J§
      {
         var posX:int = 0;
         var posY:int = 0;
         var sqDist:int = 0;
         var ownerTeamId:int = 0;
         var unit:§_-01J§ = null;
         posX = this.x;
         posY = this.y;
         sqDist = this.mineDistance.value * this.mineDistance.value;
         ownerTeamId = owner ? int(owner.squad.getTeamId()) : -1;
         §_-F2L§.§_-x1v§(this.enemyUnitsVec,"disposed",false,function(param1:§_-01J§):Boolean
         {
            if(Boolean(!param1.disposed && (!friendlyFire && Boolean(owner) ? param1.squad.getTeamId() != ownerTeamId : true)) && Boolean(precheckTarget(param1)) && MathUtil.§_-Ih§(param1.x,param1.y,posX,posY) < sqDist)
            {
               unit = param1;
               return true;
            }
            return false;
         });
         return unit;
      }
      
      protected function precheckTarget(param1:§_-01J§) : Boolean
      {
         return true;
      }
      
      protected function beforeTrigger() : void
      {
         addSpeedXY(0,1);
         §_-Y2I§.keepUnstable = true;
      }
      
      protected function beforeExplosion() : void
      {
         var _loc1_:Tween = null;
         this.beepSound = §_-h2B§.play(§_-d27§.§_-27§,true);
         if(armature)
         {
            armature.animation.gotoAndPlay(§_-K1t§.§_-J2e§);
         }
         else
         {
            _loc1_ = new Tween(§_-z1H§,0.07);
            _loc1_.repeatCount = 20;
            _loc1_.reverse = true;
            _loc1_.animate("color",Color.RED);
            _loc1_.scaleTo(§_-z1H§.scale * 1.1);
            Starling.juggler.add(_loc1_);
         }
         dispatchEventWith(§_-L3Q§.MINE_ACTIVATE,false,§_-X1F§.create(this,this.activator != null));
      }
      
      protected function onExplosionCounter(param1:uint) : void
      {
      }
      
      protected function explode() : void
      {
         inflictDamage(null,paramSet.damage.value,paramSet.damageRadius.value,paramSet.explosionRadius.value);
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,this.x,this.y,3 * paramSet.explosionRadius.value,§_-d27§.§_-a2n§);
         disposed = true;
      }
      
      override public function defaultOnDamaged(param1:Event, param2:§_-v2Q§) : void
      {
         super.defaultOnDamaged(param1,param2);
         §_-32§ = §_-h2D§;
         addSpeedVector(-MathUtil.PI_HALF,5);
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         this.enemyUnitsVec = null;
         if(this.beepSound)
         {
            this.beepSound.stop();
         }
         this.activator = null;
         if(Boolean(this.setupTimer) && this.setupTimer.running)
         {
            this.setupTimer.stop();
         }
         if(!this.setupEventFired)
         {
            dispatchEventWith(§_-X1F§.§_-C3Z§(),false,§_-X1F§.create(this,false));
         }
         §_-Y2I§.removeEventListener(§_-42G§.§_-K2J§,this.onStopMoving);
         §_-X1j§.§_-12n§.scene.removeEventListener(§_-L3Q§.SOT,this.nextTurn);
         super.defaultOnDisposed(param1);
      }
      
      public function setExplosionDelay(param1:§_-TA§) : void
      {
         this.explosionDelay.value = param1.value;
      }
      
      public function setMaxTurn(param1:§_-TA§) : void
      {
         this.maxTurn.value = param1.value;
      }
      
      public function getDangerRadius() : int
      {
         return this.mineDistance.value;
      }
      
      public function get isExploding() : Boolean
      {
         return this.exploding;
      }
      
      override public function set disabled(param1:Boolean) : void
      {
         if(!this.exploding)
         {
            super.disabled = param1;
         }
      }
   }
}

