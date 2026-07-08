package ru.pragmatix.wormix.weapons.ammo.type.missile.homing
{
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import §_-hO§.§_-03k§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-V1R§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Zd§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.utils.FrameTimer;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.feats.HomingFeat;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class HomingMissile extends §_-Qi§
   {
      
      protected static const DEFAULT_HOMING_DELAY:int = 12;
      
      private static const DETONATE_TIME:int = 150;
      
      private static const SPEED:int = 25;
      
      private static const ROTATION_SPEED:Number = 7 * MathUtil.PI_ONE_180;
      
      protected var point:Point;
      
      private var freeTimer:FrameTimer;
      
      private var detonateTimer:FrameTimer;
      
      private var beepSound:§_-V1R§;
      
      protected var paramSet:§_-C3e§;
      
      protected var damage:§_-TA§;
      
      protected var homingFeat:HomingFeat;
      
      public function HomingMissile(param1:§_-01J§, param2:Point, param3:§_-C3e§, param4:Number, param5:uint, param6:Point, param7:int = 12)
      {
         var base:Point;
         var owner:§_-01J§ = param1;
         var position:Point = param2;
         var paramSet:§_-C3e§ = param3;
         var angle:Number = param4;
         var power:uint = param5;
         var target:Point = param6;
         var homingDelay:int = param7;
         super(owner,position.x,position.y,false);
         this.damage = new §_-TA§(paramSet.damage.value);
         §_-h2B§.play(§_-d27§.§_-I1P§);
         base = §_-fm§(angle,0,position);
         initialize(base,paramSet.ammoClassName);
         Pool.putPoint(base);
         this.clip.rotation = angle * 180 / Math.PI;
         this.point = target;
         this.paramSet = paramSet;
         setSpeedVector(angle,power);
         §_-Zd§.once(this,§_-L3Q§.§_-o13§,function(param1:Event):void
         {
            explode(param1.data.other);
         });
         this.homingFeat = new HomingFeat(this,SPEED,ROTATION_SPEED);
         §_-P1P§.push(this.homingFeat);
         if(homingDelay > 0)
         {
            this.startTimers(homingDelay);
            §_-32§ = §_-h2D§;
         }
         else
         {
            this.startHoming();
         }
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
         Effects.attachParticleTrail(ParticleEffectType.AMMO_TRAIL,this);
      }
      
      private function startTimers(param1:uint) : void
      {
         var homingDelay:uint = param1;
         this.detonateTimer = new FrameTimer(DETONATE_TIME,1);
         this.detonateTimer.addEventListener(§_-03k§.§_-cE§,function(param1:Event):void
         {
            explode(null);
         });
         this.freeTimer = new FrameTimer(homingDelay,1);
         this.freeTimer.addEventListener(§_-03k§.§_-cE§,function(param1:Event):void
         {
            physParams.removeForce(Force.GRAVITY_NAME);
            detonateTimer.start();
            startHoming();
            beepSound = §_-h2B§.play(§_-d27§.§_-s24§,true);
         });
         this.freeTimer.start();
      }
      
      private function startHoming() : void
      {
         §_-32§ = §_-uN§;
         this.homingFeat.setTarget(this.point);
      }
      
      protected function explode(param1:§_-F3o§) : void
      {
         inflictDamage(param1,this.damage.value,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value);
         disposed = true;
         Effects.explodeParticles(x,y,16);
         if(this.beepSound != null)
         {
            this.beepSound.stop();
         }
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,x,y,this.paramSet.explosionRadius.value,§_-d27§.§_-33W§);
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         if(Boolean(this.freeTimer) && this.freeTimer.running)
         {
            this.freeTimer.stop();
         }
         if(Boolean(this.detonateTimer) && this.detonateTimer.running)
         {
            this.detonateTimer.stop();
         }
         if(this.beepSound != null)
         {
            this.beepSound.stop();
         }
         super.defaultOnDisposed(param1);
      }
   }
}

