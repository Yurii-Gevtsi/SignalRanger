package ru.pragmatix.wormix.weapons.ammo.phase
{
   import §_-31Y§.§_-D3B§;
   import §_-l1g§.§_-L3Q§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.model.AmmoPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.feats.HomingFeat;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class HomingPhaseAmmo extends §_-Qi§
   {
      
      private static const ROTATION_SPEED:Number = Math.PI;
      
      private static const ROTATION_SPEED_PERIOD:uint = 50;
      
      private static const ROTATION_SPEED_GAIN:Number = MathUtil.PI_ONE_180;
      
      private static const SUPER_HOMING_DELAY:int = 100;
      
      private static const SUPER_HOMING_DISTANCE:int = 100;
      
      protected var target:§_-F3o§;
      
      private var _homingFeat:HomingFeat;
      
      protected var callback:Function;
      
      public function HomingPhaseAmmo(param1:§_-01J§, param2:int, param3:int, param4:Boolean, param5:§_-F3o§, param6:uint, param7:uint)
      {
         super(param1,param2,param3,param4,1000);
         §_-92S§ = false;
         this.target = param5;
         initialize(Pool.getPoint(param2,param3));
         this.initTrail(param6);
         §_-32§ = §_-uN§;
         physParams.ignoresTerrain = true;
         physParams.collidesWith = PhysParams.COLLIDES_WITH_UNIT;
         tags = new <§_-D3B§>[§_-D3B§.§_-f1U§];
         addEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         §_-ni§ = false;
         this._homingFeat = new HomingFeat(this,param7,ROTATION_SPEED);
         this._homingFeat.superHomingDelay = SUPER_HOMING_DELAY;
         this._homingFeat.superHomingDistance = SUPER_HOMING_DISTANCE;
         this._homingFeat.rotaSpeedGain = ROTATION_SPEED_GAIN;
         this._homingFeat.rotaSpeedGainPeriod = ROTATION_SPEED_PERIOD;
         §_-P1P§.push(this._homingFeat.withTarget(param5));
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      public static function fromParams(param1:§_-01J§, param2:int, param3:int, param4:§_-F3o§, param5:Function, param6:uint = 11289911) : HomingPhaseAmmo
      {
         var _loc7_:HomingPhaseAmmo = new HomingPhaseAmmo(param1,param2,param3,true,param4,param6,80);
         _loc7_.callback = param5;
         return _loc7_;
      }
      
      override protected function initPhysicModel() : void
      {
         physParams = new PhysParams(CollideType.AMMO);
         physParams.collidesWith = PhysParams.COLLIDES_WITH_UNIT;
         §_-Y2I§ = new AmmoPhysicModel(physParams);
         AmmoPhysicModel(§_-Y2I§).checkEnemiesOnStart();
      }
      
      protected function initTrail(param1:uint) : void
      {
         Effects.attachParticleTrailWithColor(ParticleEffectType.REVENGE,this,param1,param1,1);
      }
      
      protected function onCollide(param1:Event) : void
      {
         if(param1.data.other == this.target)
         {
            this.onTarget();
            this.clear();
         }
      }
      
      override public function nextState() : void
      {
         super.nextState();
         if(AmmoPhysicModel(§_-Y2I§).isObjectIgnored(this.target))
         {
            disposed = true;
         }
      }
      
      protected function onTarget() : void
      {
         if(this.callback != null)
         {
            this.callback();
         }
      }
      
      protected function clear() : void
      {
         disposed = true;
         removeEventListener(§_-L3Q§.§_-o13§,this.onCollide);
      }
      
      public function get homingFeat() : HomingFeat
      {
         return this._homingFeat;
      }
   }
}

