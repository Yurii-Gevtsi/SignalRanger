package ru.pragmatix.wormix.weapons.ammo.boss.scientist
{
   import §_-H2g§.§_-di§;
   import §_-l2r§.§_-K1t§;
   import dragonBones.animation.WorldClock;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import starling.events.Event;
   
   public class ScientistLightningRod extends §_-F3o§
   {
      
      private static const ARMATURE_NAME:String = "Scientist_LightingRod";
      
      public const SAFE_AREA_RADIUS:uint = 200;
      
      private var armature:§_-di§;
      
      public function ScientistLightningRod(param1:Number, param2:Number)
      {
         super();
         this.armature = §_-K1t§.buildArmature(ARMATURE_NAME,§_-K1t§.§_-G1E§,§_-K1t§.§_-B1s§);
         WorldClock.clock.add(this.armature);
         this.armature.animation.play();
         clip = this.armature.display;
         clip.touchable = false;
         physParams.collidesWith = PhysParams.COLLIDES_WITH_NOTHING;
         physParams.collideType = CollideType.NONE;
         stable = true;
         this.x = param1;
         this.y = param2;
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this,true);
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         if(this.armature)
         {
            WorldClock.clock.remove(this.armature);
            this.armature.dispose();
            this.armature = null;
         }
         super.defaultOnDisposed(param1);
      }
   }
}

