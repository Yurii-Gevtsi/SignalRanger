package ru.pragmatix.wormix.weapons.ammo
{
   import §_-31N§.§_-42G§;
   import §_-L1H§.§_-C3e§;
   import §_-r1C§.§_-h2B§;
   import §_-r1C§.§_-t1R§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.model.GrenadePhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class GenericGrenade extends §_-Qi§
   {
      
      protected var paramSet:§_-C3e§;
      
      private var bounceCoeff:Number;
      
      private var lifeTime:int;
      
      protected var bounceSounds:Array = [§_-d27§.§_-x14§];
      
      protected var stopSound:§_-t1R§ = §_-d27§.§_-32Z§;
      
      public function GenericGrenade(param1:§_-01J§, param2:int, param3:int, param4:§_-C3e§, param5:Number, param6:uint, param7:Number, param8:int = 0, param9:int = 300)
      {
         this.bounceCoeff = param7;
         this.lifeTime = param8;
         super(param1,param2,param3,false,param9);
         §_-Nt§ = true;
         this.paramSet = param4;
         initialize(Pool.getPoint(param2,param3),param4.ammoClassName);
         this.§_-z1H§.rotation = 200 * MathUtil.PI_ONE_180;
         setSpeedVector(param5,param6);
         §_-32§ = §_-h2D§;
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      override protected function initPhysicModel() : void
      {
         physParams = new PhysParams(CollideType.AMMO);
         physParams.addForce(Force.GRAVITY);
         physParams.collidesWith = PhysParams.COLLIDES_WITH_GROUND;
         §_-Y2I§ = new GrenadePhysicModel(physParams,this.bounceCoeff,this.lifeTime);
         §_-Y2I§.addEventListener(§_-42G§.§_-X1C§,this.onStopMoving);
         §_-Y2I§.addEventListener(§_-42G§.§_-XQ§,function(param1:Event):void
         {
            if(bounceSounds != null && bounceSounds.length > 0 && Boolean(§_-Y2I§.lastTravelledDistance))
            {
               §_-h2B§.play(§_-d27§.§_-k4§(bounceSounds));
            }
         });
      }
      
      protected function onStopMoving(param1:Event) : void
      {
         physParams.removeAllForces();
         physParams.addForce(Force.GRAVITY);
         §_-32§ = §_-8G§;
         if(this.stopSound != null)
         {
            §_-h2B§.play(this.stopSound);
         }
         §_-Y2I§.removeEventListener(§_-42G§.§_-X1C§,this.onStopMoving);
      }
   }
}

