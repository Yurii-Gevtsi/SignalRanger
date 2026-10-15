package ru.pragmatix.wormix.weapons.ammo.type.grenade.simple
{
   import §_-L1H§.§_-C3e§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.weapons.§_-93W§;
   import ru.pragmatix.wormix.weapons.ammo.*;
   import starling.events.Event;
   
   public class BouncingGrenade extends GenericGrenade
   {
      
      protected var clock:uint;
      
      protected var blinkClassName:String = null;
      
      protected var counter:§_-93W§;
      
      protected var explosionEffectType:EffectType = EffectType.EXPLOSION_MEDIUM;
      
      protected var explodeOnStop:Boolean = false;
      
      public function BouncingGrenade(param1:§_-01J§, param2:§_-C3e§, param3:uint, param4:Number, param5:uint, param6:Number, param7:String)
      {
         super(param1,param1.x,param1.y,param2,param4,param5,param6,0);
         §_-h2B§.play(§_-d27§.§_-G3Z§);
         this.counter = new §_-93W§(param3 / 30,Effects.MISC_COLOR,Effects.MISC_SHADOW,2,Effects.CRITICAL_TIME_COLOR,Effects.CRITICAL_TIME_SHADOW);
         this.counter.attach(this,0,-40);
         this.blinkClassName = param7;
         this.clock = param3;
      }
      
      public function explodeOnCollideWithEnemy() : void
      {
         physParams.collidesWith |= PhysParams.COLLIDES_WITH_UNIT;
         addEventListener(§_-L3Q§.§_-o13§,this.onGrenadeCollide);
      }
      
      private function onGrenadeCollide(param1:Event) : void
      {
         if(param1.data.other != null && param1.data.other is §_-01J§ && owner.squad.getTeamId() != §_-01J§(param1.data.other).squad.getTeamId())
         {
            this.explode();
         }
      }
      
      override public function nextState() : void
      {
         super.nextState();
         this.grenadeTick();
      }
      
      protected function grenadeTick() : void
      {
         if(!this.disposed && Boolean(this.counter))
         {
            this.counter.tick(1 / 30);
            if(frames == this.clock - 29 && Boolean(this.blinkClassName))
            {
               if(armature)
               {
                  armature.playAnimation(§_-K1t§.§_-J2e§);
               }
            }
            if(frames == this.clock)
            {
               this.explode();
            }
         }
      }
      
      public function explode() : void
      {
         inflictDamage(null,paramSet.damage.value,paramSet.damageRadius.value,paramSet.explosionRadius.value);
         disposed = true;
         Effects.explodeParticles(x,y,12);
         Effects.createDragonbonesAnimationEffect(this.explosionEffectType,this.x,this.y,3 * paramSet.damageRadius.value,§_-d27§.§_-S14§);
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         super.defaultOnDisposed(param1);
         removeEventListener(§_-L3Q§.§_-o13§,this.onGrenadeCollide);
      }
      
      public function removeTimer() : void
      {
         if(this.counter)
         {
            this.counter.detach();
         }
         this.counter = null;
      }
      
      public function explodeOnStopMoving() : void
      {
         this.explodeOnStop = true;
      }
      
      override protected function onStopMoving(param1:Event) : void
      {
         super.onStopMoving(param1);
         if(this.explodeOnStop && !disposed)
         {
            this.explode();
         }
      }
   }
}

