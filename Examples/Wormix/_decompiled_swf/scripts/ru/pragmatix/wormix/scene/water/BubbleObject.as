package ru.pragmatix.wormix.scene.water
{
   import §_-G16§.§_-Y2x§;
   import §_-H2g§.§_-di§;
   import §_-l2r§.§_-K1t§;
   import dragonBones.animation.WorldClock;
   
   public class BubbleObject implements §_-Y2x§
   {
      
      private static const ARMATURE_NAME:String = "bubble_emerging_lot";
      
      public var armature:§_-di§;
      
      public function BubbleObject()
      {
         super();
         this.armature = §_-K1t§.buildArmature(ARMATURE_NAME,null,§_-K1t§.§_-B1s§);
      }
      
      public function dispose() : void
      {
         if(this.armature.display.parent)
         {
            this.armature.display.removeFromParent();
         }
         if(WorldClock.clock.contains(this.armature))
         {
            WorldClock.clock.remove(this.armature);
         }
         this.armature.dispose();
         this.armature = null;
      }
      
      public function removeFromPool() : void
      {
         this.dispose();
      }
   }
}

