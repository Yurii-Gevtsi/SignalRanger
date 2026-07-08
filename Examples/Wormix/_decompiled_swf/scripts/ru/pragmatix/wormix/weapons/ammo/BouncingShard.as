package ru.pragmatix.wormix.weapons.ammo
{
   import §_-31N§.§_-42G§;
   import §_-A1K§.§_-TA§;
   import §_-b1F§.§_-v2Q§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.ScreenShake;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.model.GrenadePhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class BouncingShard extends §_-Qi§
   {
      
      private static const ammoClassName:String = "DefaultShard";
      
      private static const DAMAGE:uint = 10;
      
      private static const RADIUS:uint = 55;
      
      private static const DAMAGE_RADIUS:uint = 75;
      
      private static const bounceCoeff:Number = 0.5;
      
      private static const bounceSounds:Array = [§_-d27§.§_-x14§];
      
      private var clock:uint;
      
      public function BouncingShard(param1:§_-01J§, param2:int, param3:int, param4:Number, param5:Number, param6:uint = 60)
      {
         super(param1,param2,param3,false);
         this.clock = param6;
         §_-D2j§ = new §_-TA§(4);
         initialize(Pool.getPoint(param2,param3),ammoClassName);
         setSpeedVector(param4,param5);
         §_-32§ = §_-h2D§;
         Effects.attachTrail(this,10,3,16777215,16777215,1);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
         §_-X1j§.§_-12n§.scene.addEventListener(§_-L3Q§.FRAME,this.onFrame);
      }
      
      override protected function initPhysicModel() : void
      {
         physParams = new PhysParams(CollideType.AMMO);
         physParams.addForce(Force.GRAVITY);
         physParams.collidesWith = PhysParams.COLLIDES_WITH_GROUND;
         §_-Y2I§ = new GrenadePhysicModel(physParams,bounceCoeff,0);
         §_-Y2I§.addEventListener(§_-42G§.§_-X1C§,this.onStopMoving);
         §_-Y2I§.addEventListener(§_-42G§.§_-XQ§,function(param1:Event):void
         {
            if(bounceSounds != null && bounceSounds.length > 0)
            {
               §_-h2B§.play(§_-d27§.§_-k4§(bounceSounds));
            }
         });
      }
      
      protected function onStopMoving(param1:Event) : void
      {
         §_-32§ = §_-8G§;
         §_-Y2I§.removeEventListener(§_-42G§.§_-X1C§,this.onStopMoving);
      }
      
      protected function onFrame(param1:Event) : void
      {
         if(!this.disposed)
         {
            --this.clock;
            if(0 == this.clock)
            {
               inflictDamage(null,DAMAGE,DAMAGE_RADIUS,RADIUS);
               disposed = true;
               Effects.explodeParticles(x,y,12);
               Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,this.x,this.y,3 * DAMAGE_RADIUS,§_-d27§.§_-k4§([§_-d27§.CASSETE_EXPLOSION1,§_-d27§.CASSETE_EXPLOSION2,§_-d27§.CASSETE_EXPLOSION3]));
               ScreenShake.go(DAMAGE);
               §_-X1j§.§_-12n§.scene.removeEventListener(§_-L3Q§.FRAME,this.onFrame);
            }
         }
      }
      
      override public function defaultOnDamaged(param1:Event, param2:§_-v2Q§) : void
      {
         super.defaultOnDamaged(param1,param2);
         if(speedY > -3)
         {
            setSpeedXY(speedX,-3);
         }
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         §_-X1j§.§_-12n§.scene.removeEventListener(§_-L3Q§.FRAME,this.onFrame);
         super.defaultOnDisposed(param1);
      }
   }
}

