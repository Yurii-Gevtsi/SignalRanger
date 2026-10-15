package ru.pragmatix.wormix.weapons.ammo
{
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import §_-gG§.§_-uZ§;
   import §_-r1C§.§_-V1R§;
   import §_-r1C§.§_-h2B§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.trail.ITrail;
   import ru.pragmatix.wormix.game.ai.§_-Q2U§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.objects.§_-X1i§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.IPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.utils.§_-F2L§;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.IPooledAmmo;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class BurningOil extends §_-Qi§ implements IPooledAmmo, §_-Q2U§
   {
      
      private static var globalCounter:uint = 0;
      
      private static var burnSounds:Array = [];
      
      private static const DEFAULT_DAMAGE:§_-TA§ = new §_-TA§(8);
      
      private static const DEFAULT_DAMAGE_INTERVAL:uint = 8;
      
      private static const DEFAULT_GROUND_DAMAGE_INTERVAL:uint = 40;
      
      private static const DEFAULT_DURATION:uint = 90;
      
      private static const BURNING_OIL_LIFE_TME:int = 600;
      
      private static const OBJECT_TYPE:int = AmmosFactory.OBJECT_BURNING_OIL;
      
      private static var ammoObjectsFactory:AmmosFactory = AmmosFactory.getInstance();
      
      private static const DAMAGE_RADIUS:uint = 20;
      
      private static const DAMAGE_RADIUS_SQUARED:uint = DAMAGE_RADIUS * DAMAGE_RADIUS;
      
      private static const EXPL_RADIUS:uint = 7;
      
      private var burningStartFrame:int = -1;
      
      private var duration:uint;
      
      private var damageInterval:uint = 8;
      
      private var _damage:§_-TA§;
      
      private var trail:ITrail;
      
      public function BurningOil()
      {
         super(null,0,0,false,BURNING_OIL_LIFE_TME,DamageType.FIRE);
         §_-92S§ = false;
         this.duration = DEFAULT_DURATION;
         §_-z1H§ = new Sprite();
         _width = 4;
         _height = 8;
         §_-ni§ = false;
         physParams.collidesWith = PhysParams.COLLIDES_WITH_GROUND;
      }
      
      private static function addSoundCounter() : void
      {
         if(globalCounter % 15 == 0)
         {
            burnSounds.push(§_-h2B§.play(§_-d27§.§_-F1A§,true));
         }
         ++globalCounter;
      }
      
      private static function subtractSoundCounter() : void
      {
         --globalCounter;
         if(globalCounter % 15 == 0)
         {
            §_-V1R§(burnSounds.shift()).stop();
         }
      }
      
      override protected function initPhysicModel() : void
      {
         super.initPhysicModel();
         physParams.sinkable = true;
      }
      
      override public function nextState() : void
      {
         super.nextState();
         if(this.burningStartFrame >= 0)
         {
            this.nextBurnState();
         }
      }
      
      private function nextBurnState() : void
      {
         var _loc1_:int = frames - this.burningStartFrame;
         if(_loc1_ % this.damageInterval == 0)
         {
            this.damageUnits();
            this.damageObjects();
         }
         if((_loc1_ + 1) % DEFAULT_GROUND_DAMAGE_INTERVAL == 0)
         {
            this.damageGround();
         }
         if(_loc1_ == this.duration)
         {
            disposed = true;
         }
      }
      
      private function damageUnits() : void
      {
         var _loc1_:Vector.<§_-01J§> = §_-X1j§.§_-12n§.gameMaster.§_-62V§();
         §_-F2L§.§_-x1v§(_loc1_,"disposed",false,this.enemyDamageFunction);
      }
      
      private function enemyDamageFunction(param1:§_-01J§) : Boolean
      {
         var _loc2_:§_-uZ§ = null;
         if(Physics.isGobjInRadiusFast(param1 as §_-F3o§,x,y,DAMAGE_RADIUS,DAMAGE_RADIUS_SQUARED,true))
         {
            _loc2_ = param1.buffs.getBuff(§_-uZ§.NAME) as §_-uZ§;
            if(_loc2_)
            {
               _loc2_.§_-t21§(this as §_-F3o§);
            }
            else
            {
               param1.buffs.place(new §_-uZ§(this.damageInterval));
               param1.setSpeedXY(param1.speedX,0);
               inflictDamage(param1 as §_-F3o§,this._damage.value,0,0);
            }
         }
         return false;
      }
      
      private function damageObjects() : void
      {
         var _loc1_:§_-X1i§ = null;
         _loc1_ = §_-X1j§.§_-12n§.scene.gameObjectProcessor.cache;
         var _loc2_:Vector.<§_-F3o§> = _loc1_.§_-434§(CollideType.AMMO);
         var _loc3_:Vector.<§_-F3o§> = _loc1_.§_-434§(CollideType.OTHER);
         §_-F2L§.§_-x1v§(_loc2_,"disposed",false,this.gameObjectDamageFunction);
         §_-F2L§.§_-x1v§(_loc3_,"disposed",false,this.gameObjectDamageFunction);
      }
      
      private function gameObjectDamageFunction(param1:§_-F3o§) : Boolean
      {
         if(Physics.isGobjInRadiusFast(param1,x,y,DAMAGE_RADIUS,DAMAGE_RADIUS_SQUARED,false))
         {
            inflictDamage(param1,this._damage.value,0,0);
         }
         return false;
      }
      
      private function damageGround() : void
      {
         inflictDamage(null,0,0,EXPL_RADIUS);
         setSpeedXY(§_-X1j§.randomSeed.§_-Fh§(-2,2),2);
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         subtractSoundCounter();
         var _loc2_:IPhysicModel = this.getPhysicModel();
         this.trail.detach();
         if(Boolean(§_-z1H§) && Boolean(§_-z1H§.parent))
         {
            §_-z1H§.parent.removeChild(§_-z1H§);
         }
         this.setDefault();
         this.remove();
      }
      
      override protected function defaultOnCollide(param1:Event) : void
      {
         if(this.burningStartFrame < 0)
         {
            §_-32§ = §_-8G§;
            clip.rotation = 0;
            this.burningStartFrame = frames;
         }
         x = param1.data.x;
         y = param1.data.y;
         §_-Y2I§.stop();
      }
      
      override protected function defaultOnSink(param1:Event) : void
      {
         disposed = true;
      }
      
      public function setDuration(param1:uint) : void
      {
         this.duration = param1;
      }
      
      public function set rotate(param1:Boolean) : void
      {
         if(param1)
         {
            §_-32§ = §_-h2D§;
         }
         else
         {
            §_-32§ = §_-8G§;
         }
      }
      
      public function initAmmo(param1:int, param2:§_-01J§, param3:Point, param4:§_-C3e§, param5:Number, param6:uint, param7:Number = 2, param8:Boolean = false, param9:Boolean = false, param10:Boolean = true) : void
      {
         this.disposed = false;
         this.owner = param2;
         this.setDefault();
         var _loc11_:IPhysicModel = getPhysicModel();
         _loc11_.x = param3.x;
         _loc11_.y = param3.y;
         _loc11_.setSpeedVector(param5,param6);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
         addSoundCounter();
         this.trail = Effects.attachStardustParticle(this,EffectType.FIRE);
      }
      
      public function initAmmoByBulletName(param1:String, param2:§_-01J§, param3:Point, param4:§_-C3e§, param5:Number, param6:uint, param7:Number = 2, param8:Boolean = false, param9:Boolean = false, param10:Boolean = true) : void
      {
      }
      
      public function addTrail(param1:uint = 16762931, param2:uint = 16777215, param3:uint = 6, param4:uint = 2, param5:Number = 0.996) : void
      {
      }
      
      public function setDefault() : void
      {
         this._damage = new §_-TA§(DEFAULT_DAMAGE.value);
         this.burningStartFrame = -1;
         this.damageInterval = DEFAULT_DAMAGE_INTERVAL;
         frames = -1;
         this.duration = DEFAULT_DURATION;
         §_-iN§ = BURNING_OIL_LIFE_TME;
         §_-32§ = §_-8G§;
         clip.rotation = 0;
         if(§_-53z§)
         {
            §_-53z§.setTo(0,0);
         }
         else
         {
            §_-53z§ = Pool.getPoint(0,0);
         }
         §_-ni§ = false;
      }
      
      public function getDangerRadius() : int
      {
         return DAMAGE_RADIUS + 5;
      }
      
      public function remove() : void
      {
         ammoObjectsFactory.removeObject(this,OBJECT_TYPE);
      }
      
      public function dispose() : void
      {
      }
   }
}

