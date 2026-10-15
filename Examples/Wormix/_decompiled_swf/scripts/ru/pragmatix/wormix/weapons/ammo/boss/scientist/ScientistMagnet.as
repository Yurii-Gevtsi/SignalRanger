package ru.pragmatix.wormix.weapons.ammo.boss.scientist
{
   import §_-31N§.§_-42G§;
   import §_-H2g§.§_-di§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import dragonBones.animation.WorldClock;
   import flash.geom.Point;
   import flash.utils.Dictionary;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.weapons.ammo.Bullet;
   import ru.pragmatix.wormix.weapons.ammo.BurningOil;
   import ru.pragmatix.wormix.weapons.ammo.GenericGrenade;
   import ru.pragmatix.wormix.weapons.ammo.Rope;
   import ru.pragmatix.wormix.weapons.ammo.RopeHarpoon;
   import ru.pragmatix.wormix.weapons.ammo.phase.BlockHealPhase;
   import ru.pragmatix.wormix.weapons.ammo.phase.PoisonPhase;
   import ru.pragmatix.wormix.weapons.ammo.type.mine.SpiderMineSimple;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class ScientistMagnet extends §_-F3o§
   {
      
      private static const CAPTURED_AMMO_LIFE_TIME:int = 60;
      
      private static const GRENADE_LIFETIME:uint = 5;
      
      private static const DEFAULT_ATTRACTION_POWER:int = 25;
      
      private static const BULLET_ATTRACTION_POWER:int = 250;
      
      private static const MAGNET_ARMATURE_NAME:String = "Scientist_Magnet";
      
      private const maxX:int = §_-X1j§.§_-12n§.scene.sceneW;
      
      private const maxY:int = §_-X1j§.§_-12n§.scene.sceneH;
      
      public var generator:ScientistGenerator;
      
      private var capturedAmmosLifetime:Dictionary = new Dictionary();
      
      private var owner:§_-01J§;
      
      private var armature:§_-di§;
      
      private var ignoreList:Vector.<Class> = new <Class>[Rope,PoisonPhase,BlockHealPhase];
      
      private var laggingList:Vector.<Class> = new <Class>[BurningOil,SpiderMineSimple,RopeHarpoon];
      
      public function ScientistMagnet(param1:§_-01J§, param2:Number, param3:Number)
      {
         super();
         this.owner = param1;
         this.generator = new ScientistGenerator(param1,600,995);
         this.armature = §_-K1t§.buildArmature(MAGNET_ARMATURE_NAME,§_-K1t§.§_-G1E§,§_-K1t§.§_-B1s§);
         WorldClock.clock.add(this.armature);
         this.armature.playAnimation(§_-K1t§.§_-G1E§);
         clip = this.armature.display;
         physParams.collidesWith = PhysParams.COLLIDES_WITH_AMMO;
         physParams.collideType = CollideType.OTHER;
         stable = true;
         this.x = param2;
         this.y = param3;
         §_-Y2I§.addEventListener(§_-42G§.§_-o1a§,this.onUnstable);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      override public function nextState() : void
      {
         super.nextState();
         this.checkState();
         if(this.generator.isOn)
         {
            ++frames;
            this.findTarget();
         }
      }
      
      override protected function increaseFrames() : void
      {
         if(this.generator.isOn)
         {
            ++frames;
         }
      }
      
      private function isRelatedTo(param1:§_-F3o§, param2:Vector.<Class>) : Boolean
      {
         var _loc3_:Class = null;
         for each(_loc3_ in param2)
         {
            if(param1 is _loc3_)
            {
               return true;
            }
         }
         return false;
      }
      
      private function findTarget() : void
      {
         var _loc1_:Bullet = null;
         var _loc3_:§_-F3o§ = null;
         var _loc4_:§_-F3o§ = null;
         var _loc5_:uint = 0;
         var _loc2_:Vector.<§_-F3o§> = §_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-MM§([§_-Qi§]);
         for each(_loc3_ in Physics.getGameObjectsInZone(0,0,this.maxX,this.maxY,_loc2_))
         {
            if(§_-Qi§(_loc3_).owner != this.owner)
            {
               if(this.isRelatedTo(_loc3_,this.laggingList))
               {
                  this.disposeObject(_loc3_,false);
               }
               else
               {
                  _loc4_ = _loc3_;
               }
            }
            if(Boolean(_loc4_) && !this.isRelatedTo(_loc4_,this.ignoreList))
            {
               if(!this.capturedAmmosLifetime[_loc4_])
               {
                  this.capturedAmmosLifetime[_loc4_] = frames + (_loc4_ is GenericGrenade ? GRENADE_LIFETIME : CAPTURED_AMMO_LIFE_TIME);
                  _loc4_.addEventListener(§_-L3Q§.DISPOSED,this.onAmmoDisposed);
               }
               _loc1_ = _loc4_ as Bullet;
               if(Boolean(_loc1_) && _loc1_.createEndParticles)
               {
                  _loc1_.createEndParticles = false;
               }
               _loc5_ = _loc4_ is Bullet && _loc1_.isSimpleBullet() ? uint(BULLET_ATTRACTION_POWER) : uint(DEFAULT_ATTRACTION_POWER);
               _loc4_.stable = true;
               _loc4_.addSpeedVector(Math.atan2(y - _loc4_.y,x - _loc4_.x),this.attractionPower(_loc5_));
               if(this.capturedAmmosLifetime[_loc4_] <= frames || _loc4_.y < 0)
               {
                  this.disposeObject(_loc4_);
               }
            }
         }
      }
      
      private function disposeObject(param1:§_-F3o§, param2:Boolean = true) : void
      {
         var _loc3_:Point = null;
         _loc3_ = param1 is Rope ? Pool.getPoint(§_-Qi§(param1).owner.x + 30 * §_-Qi§(param1).owner.direction,§_-Qi§(param1).owner.y) : Pool.getPoint(param1.x,param1.y);
         if(param2)
         {
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_ELECTRIC,_loc3_.x,_loc3_.y,10);
         }
         param1.disposed = true;
         Pool.putPoint(_loc3_);
      }
      
      private function onAmmoDisposed(param1:Event) : void
      {
         var _loc2_:§_-F3o§ = param1.target as §_-F3o§;
         if(Boolean(_loc2_) && Boolean(this.capturedAmmosLifetime))
         {
            _loc2_.removeEventListener(§_-L3Q§.DISPOSED,this.onAmmoDisposed);
            delete this.capturedAmmosLifetime[_loc2_];
         }
      }
      
      private function attractionPower(param1:Number) : Number
      {
         return param1 * this.generator.charge / 100;
      }
      
      private function checkState() : void
      {
         if(this.generator.isOn)
         {
            if(!this.armature.animation.isPlaying)
            {
               this.armature.playAnimation(§_-K1t§.§_-G1E§);
            }
         }
         else if(this.armature.animation.isPlaying)
         {
            this.armature.stopAnimation();
         }
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         if(this.armature)
         {
            WorldClock.clock.remove(this.armature);
            this.armature.dispose();
            this.armature = null;
         }
         §_-Y2I§.removeEventListener(§_-42G§.§_-o1a§,this.onUnstable);
         super.defaultOnDisposed(param1);
      }
      
      private function onUnstable(param1:Event) : void
      {
         stable = true;
      }
   }
}

