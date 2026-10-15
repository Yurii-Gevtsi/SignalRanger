package ru.pragmatix.wormix.weapons.ammo
{
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.model.GenericPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class Shell extends §_-Qi§
   {
      
      private const DEFAULT_SHELL_CLIP:String = "AmGun";
      
      private const DEFAULT_SHELL_SCALE:Number = 0.5;
      
      public function Shell(param1:int, param2:int, param3:String = "AmGun", param4:Number = 0.5, param5:String = "resurces/_ammo/")
      {
         super(null,param1,param2,true);
         §_-92S§ = false;
         initialize(Pool.getPoint(param1,param2),param3,true,param5);
         §_-z1H§.scaleX *= param4;
         §_-z1H§.scaleY *= param4;
         §_-32§ = §_-h2D§;
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this,true);
      }
      
      override protected function initPhysicModel() : void
      {
         physParams = new PhysParams(CollideType.NONE);
         physParams.addForce(Force.GRAVITY);
         §_-Y2I§ = new GenericPhysicModel(physParams);
      }
      
      override protected function defaultOnSink(param1:Event) : void
      {
         disposed = true;
      }
   }
}

