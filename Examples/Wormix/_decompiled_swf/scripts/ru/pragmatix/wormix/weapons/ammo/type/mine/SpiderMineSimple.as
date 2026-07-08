package ru.pragmatix.wormix.weapons.ammo.type.mine
{
   import §_-31N§.§_-42G§;
   import §_-31Y§.§_-D3B§;
   import §_-L1H§.§_-C3e§;
   import §_-gG§.§_-8u§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-m0§.§_-v21§;
   import §_-r1C§.§_-V1R§;
   import §_-r1C§.§_-h2B§;
   import dragonBones.animation.WorldClock;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.ai.§_-Q2U§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.model.AnimalPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-F2L§;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class SpiderMineSimple extends §_-Qi§ implements §_-Q2U§
   {
      
      private static const Y_SHIFT:int = -10;
      
      private static const SPEED:int = 7;
      
      private static const LIFETIME:int = 180;
      
      protected var direction:int;
      
      protected var paramSet:§_-C3e§;
      
      protected var target:§_-01J§;
      
      private var walkSound:§_-V1R§;
      
      public function SpiderMineSimple(param1:§_-01J§, param2:§_-C3e§)
      {
         super(param1,param1.x,param1.y,true,-1);
         this.paramSet = param2;
         tags = new <§_-D3B§>[§_-D3B§.§_-f1U§];
         initialize(Pool.getPoint(param1.x,param1.y),param2.ammoClassName);
         armature.playAnimation(§_-K1t§.§_-Y1c§);
         §_-Vw§(0,Y_SHIFT);
         §_-Y2I§.keepUnstable = true;
         this.direction = param1.direction;
         if(this.direction == §_-8K§.LEFT)
         {
            §_-c1S§.§_-CC§(this.clip);
         }
         AnimalPhysicModel(§_-Y2I§).crawling = this.direction * SPEED;
         §_-h2B§.play(§_-d27§.§_-12k§);
         this.walkSound = §_-h2B§.play(§_-d27§.§_-13O§,true);
         §_-X1j§.§_-12n§.scene.addEventListener(§_-L3Q§.FRAME,this.onFrame);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      override protected function initPhysicModel() : void
      {
         physParams = new PhysParams(CollideType.AMMO);
         physParams.addForce(Force.GRAVITY);
         physParams.collidesWith = PhysParams.COLLIDES_WITH_GROUND;
         §_-Y2I§ = new AnimalPhysicModel(physParams,20,5);
         §_-Y2I§.addEventListener(§_-42G§.§_-Lz§,this.onFalling);
         §_-Y2I§.addEventListener(§_-42G§.§_-X1C§,this.onStopMoving);
         §_-Y2I§.addEventListener(§_-42G§.§_-G36§,this.onHitWall);
      }
      
      protected function onFalling(param1:Event) : void
      {
         AnimalPhysicModel(§_-Y2I§).crawling = 0;
         addSpeedXY(this.direction * 3,0);
      }
      
      protected function onStopMoving(param1:Event) : void
      {
         AnimalPhysicModel(§_-Y2I§).crawling = this.direction * SPEED;
      }
      
      protected function onHitWall(param1:Event) : void
      {
         this.direction *= -1;
         §_-c1S§.§_-CC§(this.clip);
         AnimalPhysicModel(§_-Y2I§).crawling = this.direction * SPEED;
      }
      
      protected function onFrame(param1:Event) : void
      {
         var _loc2_:§_-01J§ = null;
         if(!this.disposed)
         {
            if(this.target == null)
            {
               if(frames >= LIFETIME)
               {
                  this.explode();
               }
               _loc2_ = this.nearbyTarget();
               if(_loc2_ != null)
               {
                  this.target = _loc2_;
                  if(§_-Y2I§)
                  {
                     AnimalPhysicModel(§_-Y2I§).crawling = 0;
                     §_-Y2I§.keepUnstable = false;
                     this.target.addEventListener(§_-L3Q§.TELEPORT,this.onTeleport);
                     this.walkSound.stop();
                     §_-h2B§.play(§_-d27§.§_-V1M§);
                     WorldClock.clock.remove(armature);
                     armature.stopAnimation();
                     §_-92S§ = false;
                  }
               }
            }
            else if(frames % 5 == 0)
            {
               if(this.target.disposed || MathUtil.§_-Ih§(x,y,this.target.x,this.target.y) >= 400)
               {
                  §_-X1j§.§_-12n§.scene.removeEventListener(§_-L3Q§.FRAME,this.onFrame);
                  this.addPoisonDebuffToTarget();
                  this.explode();
               }
            }
         }
      }
      
      protected function addPoisonDebuffToTarget() : void
      {
         this.target.buffs.place(new §_-8u§(owner));
      }
      
      private function nearbyTarget() : §_-01J§
      {
         var target:§_-01J§ = null;
         §_-F2L§.§_-13w§(§_-X1j§.§_-12n§.gameMaster.§_-D2A§(owner),function(param1:§_-01J§):Boolean
         {
            if(!param1.disposed && param1.stable && Math.abs(x - param1.x) < 5 && Math.abs(y + Y_SHIFT - param1.y) < 10)
            {
               target = param1;
               return true;
            }
            return false;
         });
         return target;
      }
      
      private function onTeleport(param1:§_-v21§) : void
      {
         if(!this.disposed && Boolean(§_-Y2I§))
         {
            this.stable = false;
            this.x = param1.§_-Y2Q§.x;
            this.y = param1.§_-Y2Q§.y;
            updateClipPos();
         }
      }
      
      public function explode() : void
      {
         disposed = true;
         inflictDamage(null,this.paramSet.damage.value,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value);
         Effects.explodeParticles(x,y,12);
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,this.x,this.y,2 * this.paramSet.explosionRadius.value,§_-d27§.§_-S14§);
         if(this.target != null)
         {
            this.target.removeEventListener(§_-L3Q§.TELEPORT,this.onTeleport);
         }
         §_-X1j§.§_-12n§.scene.removeEventListener(§_-L3Q§.FRAME,this.onFrame);
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         super.defaultOnDisposed(param1);
         this.walkSound.stop();
      }
      
      public function getDangerRadius() : int
      {
         return 30;
      }
   }
}

