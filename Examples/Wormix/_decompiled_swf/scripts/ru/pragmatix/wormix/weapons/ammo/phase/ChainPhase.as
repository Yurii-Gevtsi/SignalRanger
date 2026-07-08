package ru.pragmatix.wormix.weapons.ammo.phase
{
   import §_-A1K§.§_-TA§;
   import §_-l1g§.§_-L3Q§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.AmmoPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class ChainPhase extends §_-Qi§
   {
      
      private static const DEFAULT_BOUNCE_DISTANCE:uint = 700;
      
      private var target:§_-01J§;
      
      private var ammoSpeed:uint;
      
      private var damage:§_-TA§;
      
      private var bounces:uint = 2;
      
      private var bounceDistance:uint = 700;
      
      public function ChainPhase(param1:§_-01J§, param2:int, param3:int, param4:§_-01J§, param5:uint, param6:uint)
      {
         super(param1,param2,param3,true,1000,DamageType.SHOCK);
         §_-92S§ = false;
         this.target = param4;
         this.ammoSpeed = param6;
         this.damage = new §_-TA§(param5);
         initialize(Pool.getPoint(param2,param3),"AmThunderBolt");
         §_-32§ = §_-uN§;
         physParams.ignoresTerrain = true;
         physParams.collidesWith = PhysParams.COLLIDES_WITH_UNIT;
         addEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      override protected function initPhysicModel() : void
      {
         physParams = new PhysParams(CollideType.AMMO);
         physParams.collidesWith = PhysParams.COLLIDES_WITH_UNIT;
         §_-Y2I§ = new AmmoPhysicModel(physParams);
      }
      
      override public function nextState() : void
      {
         super.nextState();
         this.aimOnTarget();
         if(AmmoPhysicModel(§_-Y2I§).isObjectIgnored(this.target as §_-F3o§))
         {
            disposed = true;
         }
         if(disposed)
         {
            this.clear();
         }
      }
      
      public function setBounceDistance(param1:uint) : void
      {
         this.bounceDistance = param1;
      }
      
      protected function aimOnTarget() : void
      {
         var _loc1_:Number = NaN;
         if(!disposed)
         {
            if(Boolean(this.target) && !this.target.disposed)
            {
               _loc1_ = Math.atan2(this.target.y - y,this.target.x - x);
               setSpeedVector(_loc1_,this.ammoSpeed);
            }
         }
         else
         {
            this.clear();
         }
      }
      
      protected function clear() : void
      {
         disposed = true;
         removeEventListener(§_-L3Q§.§_-o13§,this.onCollide);
      }
      
      protected function onCollide(param1:Event) : void
      {
         var _loc2_:§_-01J§ = null;
         var _loc5_:§_-01J§ = null;
         _loc2_ = param1.data.other as §_-01J§;
         var _loc3_:Boolean = Boolean(this.target) && _loc2_ == this.target;
         var _loc4_:Boolean = Boolean(_loc2_) && _loc2_.squad.getTeamId() != owner.squad.getTeamId();
         if(_loc3_ || _loc4_)
         {
            inflictDamage(_loc2_ as §_-F3o§,this.damage.value,0,0,false,true);
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_ELECTRIC,_loc2_.x,_loc2_.y,30,§_-d27§.§_-H2w§);
            _loc5_ = null;
            if(this.bounces > 0)
            {
               _loc5_ = Physics.nearestObject(_loc2_ as §_-F3o§,§_-X1j§.§_-12n§.gameMaster.§_-939§(_loc2_),this.bounceDistance) as §_-01J§;
            }
            if(_loc5_)
            {
               this.target = _loc5_;
               --this.bounces;
            }
            else
            {
               this.clear();
            }
         }
      }
   }
}

