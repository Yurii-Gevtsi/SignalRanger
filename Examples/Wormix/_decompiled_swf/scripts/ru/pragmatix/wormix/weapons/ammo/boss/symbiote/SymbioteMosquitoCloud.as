package ru.pragmatix.wormix.weapons.ammo.boss.symbiote
{
   import §_-31N§.§_-42G§;
   import §_-D3A§.§_-TZ§;
   import §_-H2g§.§_-di§;
   import §_-H2g§.§_-s1d§;
   import §_-b1F§.§_-G4§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-h2B§;
   import dragonBones.animation.WorldClock;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.§_-F2L§;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseAmmoWithArmature;
   import starling.animation.IAnimatable;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   public class SymbioteMosquitoCloud extends §_-Qi§ implements IAnimatable
   {
      
      private static const DAMAGE:uint = 100;
      
      private static const ATTACK_RADIUS:uint = 150;
      
      private static const EXPLOSION_RADIUS:uint = 150;
      
      private static const EXPLOSION_EFFECT_RADIUS:uint = EXPLOSION_RADIUS * 2;
      
      private static const SYMBIOTE_EGG_ARMATURE_NAME:String = "Symbiote_Egg";
      
      private static const MOSQUITO_CLOUD_ARMATURE_NAME:String = "Symbiote_MoskitoCloud";
      
      private var skippedFirstTurn:Boolean;
      
      public function SymbioteMosquitoCloud(param1:§_-01J§, param2:Number, param3:Number)
      {
         super(param1,param2,param3,false,0);
         §_-Nt§ = false;
         this.x = param2;
         this.y = param3;
         armature = this.createArmature(SYMBIOTE_EGG_ARMATURE_NAME);
         armature.addEventListener(§_-s1d§.COMPLETE,this.onAnimationLoopComplete);
         armature.playAnimation(§_-K1t§.§_-XN§);
         stable = false;
         this.skippedFirstTurn = false;
         Starling.juggler.add(this);
         §_-TZ§.addListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.SOT,this.suicide);
         §_-Y2I§.addEventListener(§_-42G§.§_-o1a§,this.suicide);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      private function createArmature(param1:String) : §_-di§
      {
         var _loc2_:§_-di§ = §_-K1t§.buildArmature(param1,§_-K1t§.§_-G1E§,§_-K1t§.§_-B1s§);
         WorldClock.clock.add(_loc2_);
         clip = _loc2_.display;
         return _loc2_;
      }
      
      private function findTarget() : void
      {
         var posX:int = 0;
         var posY:int = 0;
         posX = x;
         posY = y;
         if(!§_-X1j§.§_-12n§.gameMaster || !owner || owner.disposed || !this.skippedFirstTurn)
         {
            return;
         }
         §_-F2L§.§_-13w§(§_-X1j§.§_-12n§.gameMaster.§_-D2A§(owner),function(param1:§_-01J§):Boolean
         {
            if(Physics.isGobjInRadius(param1 as §_-F3o§,posX,posY,ATTACK_RADIUS,true))
            {
               attack(param1 as §_-F3o§);
               return true;
            }
            return false;
         });
      }
      
      private function attack(param1:§_-F3o§) : void
      {
         Starling.juggler.remove(this);
         new PhaseAmmoWithArmature(owner,x,y,param1,MOSQUITO_CLOUD_ARMATURE_NAME,-1,DAMAGE);
         disposed = true;
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         §_-TZ§.removeListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.SOT,this.suicide);
         §_-Y2I§.removeEventListener(§_-42G§.§_-o1a§,this.suicide);
         super.defaultOnDisposed(param1);
      }
      
      private function onAnimationLoopComplete(param1:Event) : void
      {
         var _loc2_:§_-di§ = null;
         if(armature.animation.lastAnimationName == §_-K1t§.§_-XN§)
         {
            armature.playAnimation(§_-K1t§.§_-G1E§,0,-1,1);
         }
         else if(armature.animation.lastAnimationName == §_-K1t§.§_-G1E§)
         {
            armature.playAnimation(§_-K1t§.§_-s1g§);
         }
         else if(armature.animation.lastAnimationName == §_-K1t§.§_-s1g§)
         {
            armature.removeEventListener(§_-s1d§.COMPLETE,this.onAnimationLoopComplete);
            _loc2_ = this.createArmature(MOSQUITO_CLOUD_ARMATURE_NAME);
            this.swapClips(_loc2_.display,armature.display);
            this.removeArmature();
            armature = _loc2_;
            this.findTarget();
            stable = true;
         }
      }
      
      private function swapClips(param1:DisplayObject, param2:DisplayObject) : void
      {
         if(param2.parent)
         {
            param2.parent.addChild(param1);
            param2.parent.removeChild(param2);
         }
         param1.x = param2.x;
         param1.y = param2.y;
      }
      
      private function removeArmature() : void
      {
         armature.stopAnimation();
         WorldClock.clock.remove(armature);
         var _loc1_:DisplayObject = armature.display;
         if(Boolean(_loc1_) && Boolean(_loc1_.parent))
         {
            _loc1_.parent.removeChild(_loc1_);
            _loc1_.dispose();
         }
         armature = null;
      }
      
      public function advanceTime(param1:Number) : void
      {
         if(!this.skippedFirstTurn)
         {
            return;
         }
         if(!disposed && !stable)
         {
            this.suicide(null);
         }
         if(!disposed)
         {
            this.findTarget();
         }
      }
      
      public function suicide(param1:Event) : void
      {
         if(Boolean(param1) && Boolean(param1.type == §_-L3Q§.SOT) && !this.skippedFirstTurn)
         {
            this.skippedFirstTurn = true;
            return;
         }
         if(!owner || owner.disposed || disposed)
         {
            return;
         }
         Starling.juggler.remove(this);
         §_-G4§.§_-Ee§(owner,x,y,null,DAMAGE,EXPLOSION_RADIUS,EXPLOSION_RADIUS + 10,false);
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_POISON_LARGE,x,y,EXPLOSION_EFFECT_RADIUS,§_-d27§.§_-S14§);
         §_-h2B§.play(§_-d27§.§_-I1v§);
         this.removeArmature();
         clip = null;
         disposed = true;
      }
   }
}

