package ru.pragmatix.wormix.weapons.ammo
{
   import §_-31Y§.§_-D3B§;
   import §_-73I§.§_-Tl§;
   import §_-A2F§.§_-4B§;
   import §_-L1H§.§_-C3e§;
   import §_-hO§.SupplyPickedUpEvent;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-V1R§;
   import §_-r1C§.§_-h2B§;
   import §_-su§.§_-Vz§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.model.AmmoPhysicModel;
   import ru.pragmatix.wormix.physics.model.IPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.events.Event;
   
   public class SuperBoar extends §_-Qi§ implements §_-Vz§
   {
      
      private static const ROTATION_SPEED:Number = Math.PI / 24;
      
      private static const UP_ANGLE_MIN:Number = 2 * Math.PI / 6;
      
      private static const UP_ANGLE_MAX:Number = 4 * Math.PI / 6;
      
      private static const CRIPPLE_DURATION:uint = 1;
      
      private static const DEFAULT_DRILL_RADIUS:uint = 20;
      
      private static const DRILL_DAMAGE:uint = 10;
      
      private static const MAX_BONUS_DAMAGE_STACKS:uint = 5;
      
      private var paramSet:§_-C3e§;
      
      private var graphics:§_-Tl§;
      
      private var sound:§_-V1R§;
      
      private var groundBreaks:uint = 0;
      
      private var damagePerSupply:uint = 0;
      
      private var cripple:Boolean = false;
      
      private var flyingSpeed:int;
      
      private var damage:int;
      
      public function SuperBoar(param1:§_-01J§, param2:§_-C3e§, param3:String)
      {
         var _loc4_:IPhysicModel = null;
         _loc4_ = param1.getPhysicModel();
         super(param1,_loc4_.x,_loc4_.y,false,-1);
         var _loc5_:IPhysicModel = getPhysicModel();
         this.paramSet = param2;
         tags = new <§_-D3B§>[§_-D3B§.§_-J1i§];
         this.graphics = new §_-Tl§(param3);
         _loc5_.x = _loc4_.x;
         _loc5_.y = _loc4_.y;
         §_-z1H§ = this.graphics.getContainer();
         this.graphics.§_-Dp§();
         §_-32§ = §_-uN§;
         §_-h2B§.play(§_-d27§.SWINE);
         Effects.attachParticleTrail(ParticleEffectType.FLYING_OBJECT_TRAIL,this);
         this.sound = §_-h2B§.play(§_-d27§.§_-p1D§,true);
         this.damage = param2.damage.value;
         addEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
         §_-X1j§.§_-12n§.scene.addEventListener(§_-L3Q§.SUPPLY_PICKED_UP,this.supplyPicked);
         §_-92S§ = true;
      }
      
      override protected function initPhysicModel() : void
      {
         physParams = new PhysParams(CollideType.AMMO);
         physParams.collidesWith = PhysParams.COLLIDES_WITH_GROUND + PhysParams.COLLIDES_WITH_UNIT;
         §_-Y2I§ = new AmmoPhysicModel(physParams);
      }
      
      public function rollLeft() : void
      {
         var _loc1_:IPhysicModel = getPhysicModel();
         var _loc2_:Number = _loc1_.angle - ROTATION_SPEED;
         if(_loc2_ < -Math.PI)
         {
            _loc2_ += MathUtil.§_-M2N§;
         }
         _loc1_.setSpeedVector(_loc2_,this.flyingSpeed);
         this.updateAnimation(_loc2_);
      }
      
      public function rollRight() : void
      {
         var _loc1_:IPhysicModel = getPhysicModel();
         var _loc2_:Number = _loc1_.angle + ROTATION_SPEED;
         if(_loc2_ > Math.PI)
         {
            _loc2_ -= MathUtil.§_-M2N§;
         }
         _loc1_.setSpeedVector(_loc2_,this.flyingSpeed);
         this.updateAnimation(_loc2_);
      }
      
      private function updateAnimation(param1:Number) : void
      {
         if(Math.abs(param1) > UP_ANGLE_MIN && Math.abs(param1) < UP_ANGLE_MAX)
         {
            this.graphics.§_-Dp§();
         }
         else if(Math.abs(param1) < UP_ANGLE_MIN)
         {
            this.graphics.§_-9U§();
         }
         else
         {
            this.graphics.§_-s10§();
         }
      }
      
      protected function onCollide(param1:Event) : void
      {
         if(this.groundBreaks <= 0 || param1.data.other is §_-01J§)
         {
            this.explode();
         }
         else
         {
            §_-h2B§.play(§_-d27§.§_-S3§);
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_DRILL,x,y,this.paramSet.explosionRadius.value);
            inflictDamage(param1.data.other,DRILL_DAMAGE,DEFAULT_DRILL_RADIUS + 15,DEFAULT_DRILL_RADIUS,false);
            Effects.explodeParticles(x,y,4);
            --this.groundBreaks;
         }
      }
      
      public function explode() : void
      {
         var _loc1_:IPhysicModel = getPhysicModel();
         disposed = true;
         inflictDamage(null,this.damage,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value);
         Effects.explodeParticles(_loc1_.x,_loc1_.y,12);
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,_loc1_.x,_loc1_.y,2 * this.paramSet.explosionRadius.value,§_-d27§.§_-S14§);
         if(this.cripple)
         {
            §_-4B§.§_-x4§(x,y,this.paramSet.damageRadius.value,CRIPPLE_DURATION);
         }
         if(Boolean(owner) && owner.stable)
         {
            owner.stable = false;
         }
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         if(this.sound)
         {
            this.sound.stop();
            this.sound = null;
         }
         this.graphics.dispose();
         super.defaultOnDisposed(param1);
      }
      
      public function setFlyingSpeed(param1:Number) : void
      {
         this.flyingSpeed = param1;
         §_-Y2I§.setSpeedVector(-MathUtil.PI_HALF,this.flyingSpeed);
      }
      
      public function setGroundBreaks(param1:uint) : void
      {
         this.groundBreaks = param1;
         AmmoPhysicModel(§_-Y2I§).stopOnCollision = param1 == 0;
      }
      
      public function setDamageBonus(param1:uint) : void
      {
         this.damagePerSupply = param1;
      }
      
      private function supplyPicked(param1:SupplyPickedUpEvent) : void
      {
         if(param1.§_-i1P§ == this && !this.isMaxStacks)
         {
            this.damage += this.damagePerSupply;
            --this.flyingSpeed;
            §_-Y2I§.setSpeedVector(§_-Y2I§.angle,this.flyingSpeed);
         }
      }
      
      private function get isMaxStacks() : Boolean
      {
         return this.damage - this.paramSet.damage.value >= this.damagePerSupply * MAX_BONUS_DAMAGE_STACKS;
      }
      
      override public function get disposed() : Boolean
      {
         return super.disposed;
      }
      
      public function setCripple() : void
      {
         this.cripple = true;
      }
      
      public function getFlyingSpeed() : uint
      {
         return this.flyingSpeed;
      }
   }
}

