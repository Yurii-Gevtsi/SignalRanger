package ru.pragmatix.wormix.weapons.ammo
{
   import §_-L1H§.§_-C3e§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Zd§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class HoppingMissile extends §_-Qi§
   {
      
      private const DAMAGE_BOOST:uint = 30;
      
      private const HOP_INTERVAL:uint = 12;
      
      private const HOP_POWER:uint = 16;
      
      private var damage:uint;
      
      private var hopsLeft:uint = 3;
      
      private var lastHopFrame:uint = 0;
      
      private var hopAngle:Number;
      
      public function HoppingMissile(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:uint)
      {
         var base:Point;
         var self:HoppingMissile = null;
         var owner:§_-01J§ = param1;
         var paramSet:§_-C3e§ = param2;
         var angle:Number = param3;
         var power:uint = param4;
         super(owner,owner.x,owner.y,false);
         §_-U1o§ = true;
         this.damage = paramSet.damage.value;
         this.hopAngle = -MathUtil.PI_HALF + owner.direction * MathUtil.PI_ONE_SIX;
         base = §_-fm§(angle);
         initialize(base,paramSet.ammoClassName);
         Pool.putPoint(base);
         §_-h2B§.play(§_-d27§.§_-I1P§);
         physParams.collidesWith |= PhysParams.COLLIDES_WITH_AMMO;
         setSpeedVector(angle,power);
         self = this;
         §_-Zd§.once(this,§_-L3Q§.§_-o13§,function(param1:Event):void
         {
            self.inflictDamage(param1.data.other,self.damage,paramSet.damageRadius.value,paramSet.explosionRadius.value);
            self.disposed = true;
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,self.x,self.y,3 * paramSet.explosionRadius.value,§_-d27§.§_-33W§);
         });
         §_-32§ = §_-uN§;
         Effects.attachParticleTrail(ParticleEffectType.AMMO_TRAIL,this);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      public function hop() : void
      {
         var _loc1_:uint = uint(§_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-O1C§());
         if(this.hopsLeft > 0 && _loc1_ > this.lastHopFrame + this.HOP_INTERVAL)
         {
            this.lastHopFrame = _loc1_;
            --this.hopsLeft;
            this.damage += this.DAMAGE_BOOST;
            setSpeedVector(this.hopAngle,this.HOP_POWER);
            §_-h2B§.play(§_-d27§.§_-q5§);
         }
      }
      
      public function getHopsLeft() : int
      {
         return this.hopsLeft;
      }
   }
}

