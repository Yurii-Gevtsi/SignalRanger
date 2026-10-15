package ru.pragmatix.wormix.weapons.ammo.abstracts
{
   import §_-L1H§.§_-C3e§;
   import §_-l1g§.§_-L3Q§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.model.AmmoPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class AbstractStickyAmmo extends §_-Qi§
   {
      
      public function AbstractStickyAmmo(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:uint, param5:int = -1)
      {
         super(param1,param1.x,param1.y,false,param5);
         §_-Nt§ = true;
         var _loc6_:Point = §_-fm§(param3,0);
         initialize(_loc6_,param2.ammoClassName);
         Pool.putPoint(_loc6_);
         setSpeedVector(param3,param4);
         addEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         §_-32§ = §_-h2D§;
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      protected function onCollide(param1:Event) : void
      {
         x = param1.data.x;
         y = param1.data.y;
         §_-Y2I§.stop();
         setSpeedXY(0,0);
         stable = true;
      }
      
      override protected function initPhysicModel() : void
      {
         physParams = new PhysParams(CollideType.AMMO);
         physParams.addForce(Force.GRAVITY);
         physParams.collidesWith = PhysParams.COLLIDES_WITH_GROUND;
         §_-Y2I§ = new AmmoPhysicModel(physParams);
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         removeEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         super.defaultOnDisposed(param1);
      }
   }
}

