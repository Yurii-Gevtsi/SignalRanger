package ru.pragmatix.wormix.weapons.ammo.type.animal
{
   import §_-31N§.§_-42G§;
   import §_-31Y§.§_-D3B§;
   import §_-L1H§.§_-C3e§;
   import §_-sQ§.§_-H3D§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.model.AnimalPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class RunningObject extends §_-Qi§
   {
      
      protected static const SPEED:int = 5;
      
      protected var direction:int;
      
      protected var paramSet:§_-C3e§;
      
      protected var paramSetEx:§_-C3e§;
      
      public function RunningObject(param1:§_-01J§, param2:§_-C3e§, param3:§_-C3e§)
      {
         super(param1,param1.x,param1.y,true,-1);
         this.paramSet = param2;
         this.paramSetEx = param3;
         tags = new <§_-D3B§>[§_-D3B§.§_-J1i§];
         this.direction = param1.direction;
         initialize(Pool.getPoint(param1.x,param1.y),param2.ammoClassName);
         if(this.direction == §_-8K§.LEFT)
         {
            this.flip();
         }
         this.x = param1.x;
         this.y = param1.y;
         §_-Y2I§.keepUnstable = true;
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      override protected function initPhysicModel() : void
      {
         physParams = new PhysParams(CollideType.AMMO);
         physParams.addForce(Force.GRAVITY);
         physParams.collidesWith = PhysParams.COLLIDES_WITH_GROUND;
         §_-Y2I§ = new AnimalPhysicModel(physParams,30);
         §_-Y2I§.addEventListener(§_-42G§.§_-Lz§,this.onFalling);
         §_-Y2I§.addEventListener(§_-42G§.§_-X1C§,this.onStopMoving);
         §_-Y2I§.addEventListener(§_-42G§.§_-G36§,this.onHitWall);
      }
      
      override public function nextState() : void
      {
         super.nextState();
         if(!this.disposed)
         {
            this.update();
         }
      }
      
      protected function update() : void
      {
      }
      
      public function run(param1:int) : void
      {
         param1 = §_-H3D§.sign(param1);
         if(this.direction != param1)
         {
            if(AnimalPhysicModel(§_-Y2I§).crawling)
            {
               AnimalPhysicModel(§_-Y2I§).crawling = param1 * SPEED;
            }
            this.flip();
            this.direction = param1;
         }
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
         this.run(this.direction * -1);
      }
      
      public function explode() : void
      {
         disposed = true;
         inflictDamage(null,this.paramSet.damage.value,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value);
         Effects.explodeParticles(x,y,12);
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,this.x,this.y,4.5 * this.paramSet.explosionRadius.value,§_-d27§.§_-S14§);
      }
      
      private function flip() : void
      {
         var _loc1_:int = this.clip.rotation;
         this.clip.rotation = 0;
         §_-c1S§.§_-CC§(this.clip);
         this.clip.rotation = _loc1_;
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         §_-Y2I§.removeEventListener(§_-42G§.§_-Lz§,this.onFalling);
         §_-Y2I§.removeEventListener(§_-42G§.§_-X1C§,this.onStopMoving);
         §_-Y2I§.removeEventListener(§_-42G§.§_-G36§,this.onHitWall);
         super.defaultOnDisposed(param1);
      }
   }
}

