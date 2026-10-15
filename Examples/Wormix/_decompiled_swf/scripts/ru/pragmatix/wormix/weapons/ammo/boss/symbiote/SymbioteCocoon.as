package ru.pragmatix.wormix.weapons.ammo.boss.symbiote
{
   import §_-31N§.§_-42G§;
   import §_-91A§.§_-sH§;
   import §_-D3A§.§_-TZ§;
   import §_-H2g§.§_-di§;
   import §_-H2g§.§_-s1d§;
   import §_-P1B§.§_-EB§;
   import §_-P1B§.§_-b21§;
   import §_-b1F§.§_-v2Q§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-h2B§;
   import dragonBones.animation.WorldClock;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseAmmoWithArmature;
   import starling.display.DisplayObjectContainer;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class SymbioteCocoon extends §_-F3o§ implements §_-sH§
   {
      
      public static const COCOON_HP:uint = 20;
      
      public static const COCOON_RADIUS:uint = 15;
      
      private static const HATCHED_HEAL_RATE:uint = 40;
      
      private static const DESTROYED_HEAL_RATE:uint = HATCHED_HEAL_RATE / 2;
      
      private static const SYMBIOTE_MOSKITO_ARMATURE_NAME:String = "Symbiote_Moskito";
      
      private static const SYMBIOTE_EGG_ARMATURE_NAME:String = "Symbiote_Egg";
      
      public var owner:§_-01J§;
      
      private var isHatched:Boolean = false;
      
      private var lastPosition:Point;
      
      private var cocoonArmature:§_-di§;
      
      public function SymbioteCocoon(param1:§_-01J§, param2:Number, param3:Number)
      {
         super();
         §_-Nt§ = true;
         §_-33R§ = new §_-EB§(this,COCOON_HP,0,0);
         this.owner = param1;
         this.x = param2;
         this.y = param3;
         this.cocoonArmature = §_-K1t§.buildArmature(SYMBIOTE_EGG_ARMATURE_NAME,§_-K1t§.§_-G1E§,§_-K1t§.§_-B1s§);
         WorldClock.clock.add(this.cocoonArmature);
         this.cocoonArmature.display.pivotY = this.cocoonArmature.display.height * 0.4;
         clip = new Sprite();
         (clip as DisplayObjectContainer).addChild(this.cocoonArmature.display);
         this.cocoonArmature.addEventListener(§_-s1d§.COMPLETE,this.onAnimationLoopComplete);
         this.cocoonArmature.playAnimation(§_-K1t§.§_-XN§);
         physParams.addForce(Force.GRAVITY);
         physParams.collidesWith = PhysParams.COLLIDES_WITH_ALL;
         physParams = new PhysParams(CollideType.OTHER);
         §_-Y2I§.addEventListener(§_-42G§.§_-K2J§,this.onPhysicStable);
         _width = this.cocoonArmature.display.width;
         _height = this.cocoonArmature.display.height;
         §_-j2P§ = SortingLayer.UNIT_LAYER;
         §_-TZ§.addListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.EOT,this.hatch);
         §_-TZ§.addListener(this,§_-L3Q§.§_-Qw§,this.onDamaged);
      }
      
      private function onPhysicStable(param1:Event) : void
      {
         §_-Y2I§.removeEventListener(§_-42G§.§_-K2J§,this.onPhysicStable);
         §_-Y2I§.addEventListener(§_-42G§.§_-o1a§,this.makeStable);
      }
      
      private function makeStable(param1:Event) : void
      {
         stable = true;
      }
      
      private function onDamaged(param1:Event) : void
      {
         super.defaultOnDamaged(param1,param1.data as §_-v2Q§);
      }
      
      private function releaseMosquito(param1:§_-01J§, param2:uint) : void
      {
         var paa:PhaseAmmoWithArmature;
         var target:§_-01J§ = param1;
         var healRate:uint = param2;
         if(target.disposed)
         {
            return;
         }
         paa = new PhaseAmmoWithArmature(this.owner,this.lastPosition.x,this.lastPosition.y,target as §_-F3o§,SYMBIOTE_MOSKITO_ARMATURE_NAME,-1,0,function():void
         {
            if(!target.disposed)
            {
               target.attributes.hp.restore(healRate,§_-b21§.§_-uy§,false,false);
               §_-h2B§.play(§_-d27§.§_-j2q§);
            }
         });
         paa.§_-C2P§ = false;
         Pool.putPoint(this.lastPosition);
         this.lastPosition = null;
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         if(this.cocoonArmature)
         {
            this.cocoonArmature.removeEventListener(§_-s1d§.COMPLETE,this.onAnimationLoopComplete);
            WorldClock.clock.remove(this.cocoonArmature);
            this.cocoonArmature.dispose();
            this.cocoonArmature = null;
         }
         §_-Y2I§.removeEventListener(§_-42G§.§_-o1a§,this.makeStable);
         §_-TZ§.removeListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.EOT,this.hatch);
         super.defaultOnDisposed(param1);
      }
      
      private function onAnimationLoopComplete(param1:Event) : void
      {
         if(this.cocoonArmature.animation.lastAnimationName == §_-K1t§.§_-XN§)
         {
            this.cocoonArmature.playAnimation(§_-K1t§.§_-G1E§);
         }
         else if(this.cocoonArmature.animation.lastAnimationName == §_-K1t§.§_-s1g§)
         {
            this.lastPosition = Pool.getPoint(x,y);
            if(this.isHatched)
            {
               this.releaseMosquito(this.owner,HATCHED_HEAL_RATE);
            }
            else
            {
               this.releaseMosquito(lastDamager ? lastDamager.owner : this.owner,DESTROYED_HEAL_RATE);
            }
            super.disposed = true;
         }
      }
      
      public function hatch(param1:Event) : void
      {
         this.isHatched = true;
         §_-TZ§.removeListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.EOT,this.hatch);
         this.cocoonArmature.playAnimation(§_-K1t§.§_-s1g§);
      }
      
      override public function set disposed(param1:Boolean) : void
      {
         if(attributes.hp.value <= 0)
         {
            if(this.cocoonArmature.animation.lastAnimationName == §_-K1t§.§_-s1g§)
            {
               return;
            }
            if(physParams.collideType != CollideType.NONE)
            {
               physParams.collidesWith = PhysParams.COLLIDES_WITH_NOTHING;
               physParams.collideType = CollideType.NONE;
               §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-l2E§(this);
               §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-42N§(this);
            }
            this.cocoonArmature.playAnimation(§_-K1t§.§_-s1g§);
            §_-TZ§.removeListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.EOT,this.hatch);
         }
         else
         {
            super.disposed = param1;
         }
      }
      
      public function getNameHintInfoPanel() : String
      {
         return §_-s2a§.§_-K3t§(§_-s2a§.BOSS_SYMBIOTE_COCOON);
      }
      
      public function getSquadNumber() : uint
      {
         return this.owner.getSquadNumber();
      }
      
      public function get hp() : int
      {
         return attributes.hp.value;
      }
      
      public function get doesntCountTurns() : Boolean
      {
         return true;
      }
   }
}

