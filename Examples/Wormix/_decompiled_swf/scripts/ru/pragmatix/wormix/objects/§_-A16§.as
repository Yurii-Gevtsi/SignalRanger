package ru.pragmatix.wormix.objects
{
   import §_-31W§.§_-aO§;
   import §_-31W§.§_-g16§;
   import §_-31W§.§_-r26§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-W1r§;
   import §_-D3A§.§_-TZ§;
   import §_-H2g§.§_-di§;
   import §_-L1H§.§_-C3e§;
   import §_-P2i§.§_-b7§;
   import §_-R2S§.Farmer;
   import §_-R2S§.Hunter;
   import §_-R2S§.Miner;
   import §_-R2S§.§_-P1U§;
   import §_-R2S§.§_-z2x§;
   import §_-RE§.§_-w0§;
   import §_-Y1O§.*;
   import §_-YF§.§_-q2v§;
   import §_-Z1w§.§_-s1x§;
   import §_-b1F§.§_-v2Q§;
   import §_-l1g§.§_-L3Q§;
   import §_-l1g§.§_-T2N§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-h2B§;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.LoaderMax;
   import com.greensock.loading.LoaderStatus;
   import com.greensock.loading.core.*;
   import dragonBones.animation.WorldClock;
   import flash.events.Event;
   import flash.events.ProgressEvent;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.utils.getTimer;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.IPhysicModel;
   import ru.pragmatix.wormix.physics.model.NotStuckingPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.cluster.ShellBurst;
   import starling.display.DisplayObject;
   import starling.display.Quad;
   import starling.events.Event;
   
   public class §_-A16§ extends §_-F3o§
   {
      
      public static const §_-g21§:Rectangle = new Rectangle(-150,-180,260,180);
      
      public static const §_-hd§:String = "graves";
      
      protected static const §_-32P§:String = "Zombie";
      
      protected var armature:§_-di§;
      
      protected var owner:§_-01J§;
      
      public function §_-A16§(param1:String, param2:int, param3:int, param4:§_-01J§, param5:Number = 1)
      {
         super();
         this.owner = param4;
         §_-Nt§ = true;
         this.§_-Z2i§(param1);
         §_-z1H§.scaleX = §_-z1H§.scaleY = §_-z1H§.scaleY * (param5 * this.§_-gf§());
         this.x = param2;
         this.y = param3;
         §_-TZ§.addListener(this,§_-L3Q§.§_-Qw§,defaultOnDamaged);
         §_-TZ§.addListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.§_-Gn§,this.§_-fi§);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this,true);
      }
      
      private function §_-fi§(param1:starling.events.Event, param2:§_-v2Q§ = null) : void
      {
         if(Physics.isGobjInRadius(this,param2.source.x,param2.source.y,param2.power + clip.width,false))
         {
            addSpeedVector(0,0);
            §_-Y2I§.stable = false;
         }
      }
      
      override protected function initPhysicModel() : void
      {
         physParams = new PhysParams(CollideType.OTHER);
         physParams.addForce(Force.GRAVITY);
         physParams.collidesWith = PhysParams.COLLIDES_WITH_GROUND;
         §_-Y2I§ = this.§_-s25§(physParams);
      }
      
      protected function §_-s25§(param1:PhysParams) : IPhysicModel
      {
         return new NotStuckingPhysicModel(param1);
      }
      
      protected function §_-gf§() : Number
      {
         return §_-K1t§.§_-Q2C§;
      }
      
      protected function §_-Z2i§(param1:String) : void
      {
         §_-j2P§ = SortingLayer.GRAVE_LAYER;
         this.armature = §_-K1t§.buildArmature(§_-hd§ + "/" + param1,null,§_-K1t§.§_-IP§);
         WorldClock.clock.add(this.armature);
         this.armature.playAnimation("play",0,-1,0);
         this.armature.getSlot(§_-32P§).display.visible = false;
         this.armature.display.mask = new Quad(§_-g21§.width,§_-g21§.height);
         this.armature.display.mask.x = §_-g21§.x;
         this.armature.display.mask.y = §_-g21§.y;
         §_-z1H§ = this.armature.display;
      }
      
      override protected function defaultOnSink(param1:starling.events.Event) : void
      {
         super.defaultOnSink(param1);
         this.§_-a2v§();
      }
      
      protected function §_-a2v§() : void
      {
         if(§_-KX§ != null && §_-KX§.owner != null && §_-KX§.owner.squad.isLocal())
         {
            §_-X1j§.§_-12n§.battleStats.increment(§_-r26§.GRAVES_SANK);
            §_-X1j§.§_-12n§.battleStats.increment(§_-g16§.GRAVES_SANK);
         }
      }
      
      public function §_-l2Z§() : §_-01J§
      {
         return this.owner;
      }
      
      override public function defaultOnDisposed(param1:starling.events.Event = null) : void
      {
         this.removeArmature();
         super.defaultOnDisposed(param1);
         §_-TZ§.removeListener(this,§_-L3Q§.§_-Qw§,defaultOnDamaged);
         §_-TZ§.removeListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.§_-Gn§,this.§_-fi§);
         this.owner = null;
      }
      
      protected function removeArmature() : void
      {
         if(this.armature)
         {
            WorldClock.clock.remove(this.armature);
            this.armature.dispose();
            this.armature = null;
         }
      }
      
      override protected function defaultOnCollide(param1:starling.events.Event) : void
      {
         super.defaultOnCollide(param1);
         if(speed > 3)
         {
            §_-h2B§.play(§_-d27§.BONUS);
         }
      }
   }
}

