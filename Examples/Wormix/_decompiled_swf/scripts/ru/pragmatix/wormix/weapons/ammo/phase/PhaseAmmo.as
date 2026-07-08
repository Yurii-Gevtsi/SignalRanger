package ru.pragmatix.wormix.weapons.ammo.phase
{
   import §_-31N§.§_-Yy§;
   import §_-31Y§.§_-D3B§;
   import §_-l1g§.§_-L3Q§;
   import §_-sQ§.§_-H3D§;
   import §_-w2i§.§_-Ia§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.fx.trail.ParticleTrail;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.model.AmmoPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class PhaseAmmo extends §_-Qi§
   {
      
      private static const MAX_COLLIDE_DIST:int = 5;
      
      protected var target:§_-F3o§;
      
      protected var ammoSpeed:uint;
      
      protected var callback:Function;
      
      protected var alpha:Number;
      
      protected var color:uint;
      
      protected var counter:uint = 0;
      
      protected var delayBeforeAim:uint = 0;
      
      protected var rotationSpeed:Number = 3.141592653589793;
      
      protected var rotationSpeedGain:Number = 0.017453292519943295;
      
      protected var superHomingDelay:uint = 100;
      
      protected var superHomingDistance:uint = 100;
      
      protected var particleEffectType:ParticleEffectType;
      
      protected var changeColor:Boolean;
      
      protected var trail:ParticleTrail;
      
      protected var isCleared:Boolean;
      
      private var isTouchingTarget:Boolean;
      
      public function PhaseAmmo(param1:§_-01J§, param2:int, param3:int, param4:Boolean, param5:§_-F3o§, param6:uint, param7:uint, param8:Number = 0.8, param9:ParticleEffectType = null, param10:Boolean = true)
      {
         super(param1,param2,param3,param4,10000);
         §_-92S§ = false;
         this.target = param5;
         this.ammoSpeed = param7;
         this.changeColor = param10;
         this.color = param6;
         this.alpha = param8;
         if(param9 == null)
         {
            this.particleEffectType = ParticleEffectType.REVENGE;
         }
         else
         {
            this.particleEffectType = param9;
         }
         tags = new <§_-D3B§>[§_-D3B§.§_-f1U§];
         initialize(Pool.getPoint(param2,param3));
         this.initTrail();
         §_-32§ = §_-uN§;
         physParams.collidesWith = PhysParams.COLLIDES_WITH_UNIT;
         physParams.ignoresTerrain = true;
         if(param5)
         {
            param5.addEventListener(§_-L3Q§.§_-U2j§,this.beforeHit);
         }
         this.isCleared = false;
         this.isTouchingTarget = false;
         addEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      public function setRotationSpeed(param1:Number) : void
      {
         this.rotationSpeed = param1;
      }
      
      public function setRotationSpeedGain(param1:Number) : void
      {
         this.rotationSpeedGain = param1;
      }
      
      public function setSuperHomingDelay(param1:uint) : void
      {
         this.superHomingDelay = param1;
      }
      
      public function setSuperHomingDistance(param1:uint) : void
      {
         this.superHomingDistance = param1;
      }
      
      override protected function initPhysicModel() : void
      {
         physParams = new PhysParams(CollideType.NONE);
         physParams.collidesWith = PhysParams.COLLIDES_WITH_UNIT;
         §_-Y2I§ = new AmmoPhysicModel(physParams);
      }
      
      override public function nextState() : void
      {
         super.nextState();
         this.processMovement();
      }
      
      protected function initTrail() : void
      {
         if(this.changeColor)
         {
            this.trail = Effects.attachParticleTrailWithColor(this.particleEffectType,this,this.color,this.color,this.alpha);
         }
         else
         {
            this.trail = Effects.attachParticleTrail(this.particleEffectType,this);
         }
      }
      
      protected function doDamage() : void
      {
         if(this.isCleared)
         {
            return;
         }
         if(this.callback != null)
         {
            this.callback();
            this.callback = null;
         }
      }
      
      protected function clear() : void
      {
         if(this.isCleared)
         {
            return;
         }
         this.isCleared = true;
         §_-Ia§.§_-33o§(10,this.remove);
         if(hasEventListener(§_-L3Q§.§_-o13§))
         {
            if(this.target)
            {
               this.target.removeEventListener(§_-L3Q§.§_-U2j§,this.beforeHit);
            }
            removeEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         }
      }
      
      protected function processMovement() : void
      {
         var _loc1_:Number = NaN;
         var _loc2_:Number = NaN;
         var _loc3_:Number = NaN;
         var _loc4_:Number = NaN;
         var _loc5_:Number = NaN;
         var _loc6_:Number = NaN;
         ++this.counter;
         if(this.counter >= this.delayBeforeAim)
         {
            if(!disposed && !this.target.disposed)
            {
               _loc1_ = this.angle;
               _loc2_ = this.target.x - x;
               _loc3_ = this.target.y - y;
               _loc4_ = §_-H3D§.distance(_loc2_,_loc3_);
               _loc5_ = Math.atan2(_loc3_,_loc2_);
               _loc6_ = this.superHomingDelay > 0 && this.counter > this.superHomingDelay || _loc4_ < this.superHomingDistance ? _loc5_ : §_-H3D§.§_-r2H§(_loc1_,_loc5_,this.rotationSpeed);
               if(_loc4_ < MAX_COLLIDE_DIST || this.isTouchingTarget)
               {
                  this.onCollideWithOther(this.target);
               }
               else if(_loc4_ < this.ammoSpeed)
               {
                  x = this.target.x;
                  y = this.target.y;
                  setSpeedXY(0,0);
                  this.isTouchingTarget = true;
               }
               else
               {
                  setSpeedVector(_loc6_,this.ammoSpeed);
               }
            }
            else
            {
               this.clear();
            }
         }
         if(this.counter % 50 == 0)
         {
            this.rotationSpeed += this.rotationSpeedGain;
         }
      }
      
      private function beforeHit(param1:§_-Yy§) : void
      {
         if(Boolean(param1.§_-33i§ && this.id == param1.§_-33i§.getId()) && Boolean(§_-ni§) && param1.§_-K1l§)
         {
            this.clear();
         }
      }
      
      private function onCollide(param1:Event) : void
      {
         this.onCollideWithOther(param1.data.other);
      }
      
      protected function onCollideWithOther(param1:§_-F3o§) : void
      {
         if(this.isCleared)
         {
            return;
         }
         if(param1 == this.target)
         {
            this.doDamage();
            this.clear();
         }
      }
      
      private function remove() : void
      {
         if(this.trail)
         {
            this.trail.removeFromParent(true);
            this.trail = null;
         }
         if(physParams)
         {
            disposed = true;
         }
      }
   }
}

