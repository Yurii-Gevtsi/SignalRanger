package ru.pragmatix.wormix.weapons.ammo
{
   import §_-31Y§.§_-D3B§;
   import §_-L1H§.§_-C3e§;
   import §_-l1g§.§_-L3Q§;
   import §_-m0§.§_-a1O§;
   import §_-r1C§.§_-V1R§;
   import §_-r1C§.§_-h2B§;
   import §_-su§.§_-21h§;
   import §_-su§.§_-Vz§;
   import §_-w2i§.§_-Zd§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.AnimalPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.§_-5e§;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.cluster.BouncingClusterGrenadeMine;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.sticky.ShardMine;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class Ufo extends §_-Qi§ implements §_-21h§, §_-Vz§
   {
      
      private static const ROTATION_COEFF:Number = 3 * MathUtil.PI_ONE_180;
      
      private static const CAPTURE_ZONE_WIDTH:uint = 30;
      
      private static const CAPTURE_ZONE_HEIGHT:uint = 60;
      
      private static const FIND_TARGET_FREQUENCY:int = 10;
      
      private var paramSet:§_-C3e§;
      
      private var direction:int = 0;
      
      private var carrying:Boolean = false;
      
      private var cargo:§_-F3o§;
      
      private var cargoLayerIndex:int;
      
      private var canCarryUnit:Boolean = false;
      
      private var flightSound:§_-V1R§;
      
      private var frameTargetCounter:int = 0;
      
      private var attacking:Boolean;
      
      private var target:§_-F3o§;
      
      public function Ufo(param1:§_-01J§)
      {
         super(param1,param1.x,param1.y,false,-1);
         §_-92S§ = true;
      }
      
      public function init(param1:§_-C3e§) : void
      {
         this.paramSet = param1;
         tags = new <§_-D3B§>[§_-D3B§.§_-J1i§,§_-D3B§.§_-f1U§,§_-D3B§.§_-B3u§];
         initialize(Pool.getPoint(owner.x,owner.y),param1.ammoClassName);
         armature.animation.play();
         §_-32§ = §_-8G§;
         this.attacking = false;
         §_-ni§ = false;
         setSpeedVector(-MathUtil.PI_HALF,5);
         this.flightSound = §_-h2B§.play(§_-d27§.§_-m1L§,true);
         addEventListener(§_-L3Q§.§_-o13§,this.collideSound);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      override public function nextState() : void
      {
         var _loc1_:§_-F3o§ = null;
         super.nextState();
         if(this.attacking)
         {
            return;
         }
         if(!disposed)
         {
            §_-z1H§.rotation = speedX * ROTATION_COEFF;
         }
         if(this.carrying)
         {
            §_-X1j§.§_-12n§.scene.gameInterface.§_-n2O§(Weapon.§_-mv§);
            this.updateCargoPosition();
         }
         else
         {
            ++this.frameTargetCounter;
            if(this.frameTargetCounter % FIND_TARGET_FREQUENCY == 0)
            {
               _loc1_ = this.findTarget();
               if(_loc1_)
               {
                  if(_loc1_ is §_-01J§)
                  {
                     §_-X1j§.§_-12n§.scene.gameInterface.§_-n2O§(this.canCarry(_loc1_) ? Weapon.§_-mv§ : Weapon.§_-v1e§);
                  }
                  else
                  {
                     §_-X1j§.§_-12n§.scene.gameInterface.§_-n2O§(Weapon.§_-mv§);
                  }
                  this.changeTarget(_loc1_);
               }
               else
               {
                  §_-X1j§.§_-12n§.scene.gameInterface.§_-Q2D§();
               }
            }
         }
      }
      
      override protected function initPhysicModel() : void
      {
         physParams = new PhysParams(CollideType.AMMO);
         physParams.addForce(Force.GRAVITY);
         physParams.collidesWith = PhysParams.COLLIDES_WITH_UNIT;
         §_-Y2I§ = new AnimalPhysicModel(physParams);
      }
      
      public function attack() : void
      {
         var cargo:§_-F3o§ = null;
         if(!this.carrying)
         {
            this.changeTarget(null);
            cargo = this.findTarget();
            if(cargo != null)
            {
               if(this.canCarry(cargo))
               {
                  §_-h2B§.play(§_-d27§.§_-zr§);
                  this.startCarrying(cargo);
               }
               else if(!this.isFrendlyUnit(cargo))
               {
                  this.attacking = true;
                  §_-X1j§.§_-12n§.scene.gameInterface.§_-Q2D§();
                  setSpeedVector(Math.atan2(cargo.y - y,cargo.x - x),10);
                  §_-Zd§.once(this,§_-L3Q§.§_-o13§,function(param1:Event):void
                  {
                     explode();
                  });
               }
            }
         }
         else
         {
            this.stopCarrying();
         }
      }
      
      private function canCarry(param1:§_-F3o§) : Boolean
      {
         return param1 is §_-8K§ ? this.canCarryUnit && this.isFrendlyUnit(param1) : true;
      }
      
      private function isFrendlyUnit(param1:§_-F3o§) : Boolean
      {
         return owner.squad.getTeamId() == §_-8K§(param1).squad.getTeamId();
      }
      
      private function startCarrying(param1:§_-F3o§) : void
      {
         this.carrying = true;
         this.cargo = param1;
         if(this.target is §_-8K§)
         {
            this.cargoLayerIndex = §_-X1j§.§_-12n§.scene.getLayerIndex(this.cargo,SortingLayer.UNIT_LAYER);
            §_-X1j§.§_-12n§.scene.bringToTop(this.cargo);
         }
         this.updateCargoPosition();
         param1.addEventListener(§_-L3Q§.DISPOSED,this.onCargoDisposed);
      }
      
      private function onCargoDisposed(param1:Event) : void
      {
         this.stopCarrying();
      }
      
      private function stopCarrying() : void
      {
         this.carrying = false;
         this.cargo.removeEventListener(§_-L3Q§.DISPOSED,this.onCargoDisposed);
         if(this.target is §_-8K§)
         {
            §_-X1j§.§_-12n§.scene.setLayerIndex(this.cargo,SortingLayer.UNIT_LAYER,this.cargoLayerIndex);
         }
         if(this.cargo != null && !this.cargo.disposed)
         {
            this.cargo.x = x;
            this.cargo.y = y;
            this.cargo.setSpeedXY(0,-3);
         }
         this.cargo = null;
      }
      
      private function updateCargoPosition() : void
      {
         if(!disposed && this.cargo != null && !this.cargo.disposed)
         {
            this.cargo.x = x;
            this.cargo.y = y + 20;
            this.cargo.setSpeedXY(0,0);
            this.cargo.stable = false;
         }
      }
      
      private function findTarget() : §_-F3o§
      {
         var _loc2_:Array = null;
         var _loc4_:Vector.<§_-F3o§> = null;
         var _loc5_:int = 0;
         var _loc6_:§_-F3o§ = null;
         var _loc7_:int = 0;
         var _loc1_:§_-F3o§ = null;
         _loc2_ = [Mine,§_-5e§,BouncingClusterGrenadeMine,ShardMine];
         var _loc3_:Vector.<§_-F3o§> = Physics.getGameObjectsInZone(x - CAPTURE_ZONE_WIDTH * 0.5,y,x + CAPTURE_ZONE_WIDTH,y + CAPTURE_ZONE_HEIGHT,§_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-MM§(_loc2_));
         if(_loc3_.length == 0)
         {
            _loc4_ = Physics.getGameObjectsInZone(x - CAPTURE_ZONE_WIDTH * 0.5,y,x + CAPTURE_ZONE_WIDTH,y + CAPTURE_ZONE_HEIGHT,§_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-434§(CollideType.UNIT));
            _loc5_ = int(_loc4_.length);
            if(_loc5_ > 0)
            {
               _loc7_ = 0;
               while(_loc7_ < _loc5_)
               {
                  _loc6_ = _loc4_[_loc7_];
                  if(!this.isFrendlyUnit(_loc6_))
                  {
                     _loc1_ = _loc6_;
                     break;
                  }
                  if(this.canCarryUnit)
                  {
                     _loc1_ = _loc6_;
                     break;
                  }
                  _loc7_++;
               }
            }
         }
         else
         {
            _loc1_ = _loc3_[0];
         }
         return _loc1_;
      }
      
      private function changeTarget(param1:§_-F3o§) : void
      {
         if(this.target != param1)
         {
            this.target = param1;
            dispatchEventWith(§_-a1O§.§_-g1§,false,[this.target,this.canCarry(this.target)]);
         }
      }
      
      public function explode() : void
      {
         if(!disposed)
         {
            §_-ni§ = true;
            if(this.carrying)
            {
               this.stopCarrying();
            }
            disposed = true;
            inflictDamage(null,this.paramSet.damage.value,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value);
            Effects.explodeParticles(x,y,12);
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_LARGE,this.x,this.y,3 * this.paramSet.explosionRadius.value,§_-d27§.§_-S14§);
         }
      }
      
      public function turnLeft() : void
      {
         this.direction = §_-8K§.LEFT;
      }
      
      public function turnRight() : void
      {
         this.direction = §_-8K§.RIGHT;
      }
      
      private function collideSound(param1:Event) : void
      {
         if(!param1.data.other || param1.data.other != this.cargo)
         {
            §_-h2B§.play(§_-d27§.§_-w2d§);
         }
      }
      
      public function setCanCarryUnit() : void
      {
         this.canCarryUnit = true;
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         super.defaultOnDisposed(param1);
         if(this.flightSound != null)
         {
            this.flightSound.stop();
            this.flightSound = null;
         }
         removeEventListener(§_-L3Q§.§_-o13§,this.collideSound);
         §_-X1j§.§_-12n§.scene.gameInterface.§_-Q2D§();
      }
   }
}

