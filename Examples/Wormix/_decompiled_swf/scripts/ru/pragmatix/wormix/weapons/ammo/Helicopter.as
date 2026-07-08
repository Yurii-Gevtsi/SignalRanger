package ru.pragmatix.wormix.weapons.ammo
{
   import §_-31Y§.§_-D3B§;
   import §_-L1H§.§_-C3e§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-V1R§;
   import §_-r1C§.§_-h2B§;
   import §_-su§.§_-Tu§;
   import §_-su§.§_-Vz§;
   import §_-w2i§.§_-Zd§;
   import dragonBones.animation.WorldClock;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.model.AnimalPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   public class Helicopter extends §_-Qi§ implements §_-Tu§, §_-Vz§
   {
      
      private var paramSet:§_-C3e§;
      
      private var paramSetEx:§_-C3e§;
      
      private var direction:int = 0;
      
      private var flightSound:§_-V1R§;
      
      public function Helicopter(param1:§_-01J§)
      {
         super(param1,param1.x,param1.y,false,800);
      }
      
      private static function collideSound(param1:Event) : void
      {
         §_-h2B§.play(§_-d27§.§_-w2d§);
      }
      
      public function init(param1:§_-C3e§, param2:§_-C3e§) : void
      {
         this.paramSet = param1;
         this.paramSetEx = param2;
         tags = new <§_-D3B§>[§_-D3B§.§_-J1i§];
         this.direction = owner.direction;
         armature = §_-K1t§.buildArmature(param1.ammoClassName,§_-K1t§.§_-Y1c§,§_-K1t§.§_-t1V§);
         WorldClock.clock.add(armature);
         armature.playAnimation(§_-K1t§.§_-Y1c§);
         armature.advanceTime(1);
         armature.display.scaleX = armature.display.scaleY = §_-K1t§.§_-31c§;
         §_-z1H§ = armature.display;
         if(this.direction == §_-8K§.RIGHT)
         {
            §_-c1S§.§_-CC§(this.clip);
         }
         this.x = owner.x;
         this.y = owner.y;
         §_-32§ = §_-8G§;
         setSpeedVector(-MathUtil.PI_HALF,5);
         this.flightSound = §_-h2B§.play(§_-d27§.§_-51t§,true);
         addEventListener(§_-L3Q§.§_-o13§,collideSound);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
         §_-92S§ = true;
      }
      
      override protected function initPhysicModel() : void
      {
         physParams = new PhysParams(CollideType.AMMO);
         physParams.addForce(Force.GRAVITY);
         physParams.collidesWith = PhysParams.COLLIDES_WITH_GROUND;
         §_-Y2I§ = new AnimalPhysicModel(physParams);
      }
      
      public function attack() : void
      {
         §_-h2B§.play(§_-d27§.§_-QH§);
         new Shard(owner,x + this.direction * 20,y + 10,MathUtil.PI_HALF - this.direction * MathUtil.§_-y2w§,8,this.paramSetEx,0.5);
      }
      
      public function explode() : void
      {
         if(tags.indexOf(§_-D3B§.§_-f1U§) > -1)
         {
            tags.splice(tags.indexOf(§_-D3B§.§_-f1U§),1);
         }
         disposed = true;
         inflictDamage(null,this.paramSet.damage.value,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value);
         Effects.explodeParticles(x,y,12);
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_LARGE,this.x,this.y,3 * this.paramSet.explosionRadius.value,§_-d27§.§_-S14§);
      }
      
      public function explodeOnCollide() : void
      {
         if(tags.indexOf(§_-D3B§.§_-f1U§) == -1)
         {
            tags.push(§_-D3B§.§_-f1U§);
         }
         Effects.attachParticleTrail(ParticleEffectType.AMMO_TRAIL,this);
         §_-Zd§.once(this,§_-L3Q§.§_-o13§,function(param1:Event):void
         {
            explode();
         });
      }
      
      public function turnLeft() : void
      {
         if(this.direction != §_-8K§.LEFT)
         {
            this.direction = §_-8K§.LEFT;
            §_-c1S§.§_-CC§(this.clip);
         }
      }
      
      public function turnRight() : void
      {
         if(this.direction != §_-8K§.RIGHT)
         {
            this.direction = §_-8K§.RIGHT;
            §_-c1S§.§_-CC§(this.clip);
         }
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         super.defaultOnDisposed(param1);
         if(this.flightSound != null)
         {
            this.flightSound.stop();
            this.flightSound = null;
         }
         removeEventListener(§_-L3Q§.§_-o13§,collideSound);
      }
      
      override public function get clip() : DisplayObject
      {
         return super.clip;
      }
   }
}

