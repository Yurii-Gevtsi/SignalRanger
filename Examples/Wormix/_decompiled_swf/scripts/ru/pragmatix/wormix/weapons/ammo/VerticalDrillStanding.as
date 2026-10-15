package ru.pragmatix.wormix.weapons.ammo
{
   import §_-r1C§.§_-V1R§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.model.GenericPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class VerticalDrillStanding extends §_-Qi§
   {
      
      private static const CLIP_NAME:String = "AniFirework";
      
      private var fuseSound:§_-V1R§;
      
      public function VerticalDrillStanding(param1:§_-01J§)
      {
         super(param1,param1.x,param1.y,false);
         this.fuseSound = §_-h2B§.play(§_-d27§.§_-2e§,true);
         initialize(Pool.getPoint(),CLIP_NAME);
         _width = 17;
         _height = 43;
         this.x = param1.x + (param1.direction == §_-8K§.LEFT ? 9 : -9);
         this.y = param1.y;
         stable = false;
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      override protected function initPhysicModel() : void
      {
         physParams = new PhysParams(CollideType.NONE);
         §_-Y2I§ = new GenericPhysicModel(physParams);
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         super.defaultOnDisposed(param1);
         if(this.fuseSound != null)
         {
            this.fuseSound.stop();
         }
      }
   }
}

