package ru.pragmatix.wormix.objects
{
   import §_-A1K§.§_-TA§;
   import §_-H2g§.§_-di§;
   import §_-b1F§.§_-v2Q§;
   import §_-l2r§.§_-K1t§;
   import §_-z2S§.*;
   import dragonBones.animation.WorldClock;
   import flash.system.Capabilities;
   import flash.utils.clearTimeout;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.pvp.messages.ex.*;
   import ru.pragmatix.wormix.weapons.*;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class §_-F1r§ extends §_-F3o§
   {
      
      private static const §_-t1F§:§_-TA§ = new §_-TA§(30);
      
      private var armature:§_-di§;
      
      public function §_-F1r§()
      {
         super();
         §_-Nt§ = false;
         this.armature = §_-K1t§.buildArmature("LaserActivatorIndestructible",§_-K1t§.§_-G1E§,§_-K1t§.§_-t1V§);
         WorldClock.clock.add(this.armature);
         this.armature.animation.play();
         this.armature.display.scaleX = this.armature.display.scaleY = §_-K1t§.§_-31c§;
         §_-z1H§ = this.armature.display;
         stable = true;
         _width = _height = §_-t1F§.value;
         physParams.collidesWith = PhysParams.COLLIDES_WITH_AMMO;
         physParams.collideType = CollideType.NONE;
      }
      
      override public function defaultOnDamaged(param1:Event, param2:§_-v2Q§) : void
      {
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         if(this.armature)
         {
            WorldClock.clock.remove(this.armature);
            (this.armature.display.parent as DisplayObjectContainer).removeChild(this.armature.display);
            this.armature.dispose();
            this.armature = null;
         }
      }
   }
}

