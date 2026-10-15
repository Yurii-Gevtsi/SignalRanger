package ru.pragmatix.wormix.weapons.ammo
{
   import §_-A2F§.§_-4B§;
   import §_-L1H§.§_-C3e§;
   import §_-r1C§.§_-h2B§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.IPooledAmmo;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class BunkerBusterMissile extends §_-Qi§
   {
      
      private static const CRIPPLE_DURATION:uint = 1;
      
      private var counter:uint;
      
      private var paramSet:§_-C3e§;
      
      private var paramSetEx:§_-C3e§;
      
      private var cripple:Boolean = false;
      
      private var spawnShards:Boolean = false;
      
      private var armorReduction:uint;
      
      public function BunkerBusterMissile(param1:§_-01J§, param2:§_-C3e§, param3:§_-C3e§, param4:int, param5:int, param6:uint, param7:uint, param8:Boolean = false)
      {
         this.counter = param6;
         this.paramSet = param2;
         this.paramSetEx = param3;
         this.spawnShards = param8;
         this.armorReduction = param7;
         super(param1,param4,param5,false,§_-Qi§.§_-n18§,DamageType.AIR,false);
         initialize(Pool.getPoint(param4,param5),param2.ammoClassName);
         §_-32§ = §_-uN§;
         Effects.attachParticleTrail(ParticleEffectType.AMMO_TRAIL,this);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      override protected function defaultOnCollide(param1:Event) : void
      {
         var _loc2_:IPooledAmmo = null;
         var _loc6_:Vector.<§_-01J§> = null;
         var _loc7_:uint = 0;
         super.defaultOnCollide(param1);
         if(disposed)
         {
            return;
         }
         var _loc3_:Point = Pool.getPoint();
         --this.counter;
         var _loc4_:§_-01J§ = param1.data.other as §_-01J§;
         if(_loc4_ == null)
         {
            _loc6_ = Physics.getUnitsInCircle(x,y,30);
            if(_loc6_.length > 0)
            {
               _loc4_ = _loc6_[0];
            }
         }
         var _loc5_:int = §_-X1j§.§_-12n§.scene.sceneH - §_-X1j§.§_-12n§.scene.getWaterHeight() - 100;
         if(this.counter == 0 || _loc4_ != null || y > _loc5_)
         {
            if(Boolean(_loc4_) && this.armorReduction > 0)
            {
               _loc7_ = _loc4_ != null ? uint(Math.round(0.01 * this.armorReduction * _loc4_.attributes.armor.value)) : 0;
               if(_loc7_ > 0)
               {
                  _loc4_.attributes.armor.§_-Y2z§(-_loc7_);
               }
               inflictDamage(_loc4_ as §_-F3o§,this.paramSet.damage.value,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value);
               if(_loc7_ > 0)
               {
                  _loc4_.attributes.armor.§_-Y2z§(_loc7_);
               }
            }
            else
            {
               inflictDamage(param1.data.other,this.paramSet.damage.value,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value);
            }
            disposed = true;
            Effects.explodeParticles(x,y,16);
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,x,y,2 * this.paramSet.explosionRadius.value,§_-d27§.§_-33W§);
            if(this.spawnShards)
            {
               this.shards();
            }
            if(this.cripple)
            {
               §_-4B§.§_-x4§(x,y,this.paramSet.damageRadius.value,CRIPPLE_DURATION);
            }
         }
         else
         {
            §_-h2B§.play(§_-d27§.§_-S3§);
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_DRILL,x,y,2 * this.paramSet.explosionRadius.value);
            inflictDamage(param1.data.other,10,30,20,false);
            _loc3_.setTo(x,y);
            _loc2_ = AmmosFactory.getInstance().createObject(AmmosFactory.OBJECT_BURNING_OIL);
            _loc2_.initAmmo(AmmosFactory.BURNING_OIL_SKIN,owner,_loc3_,null,-3 * MathUtil.PI_QUARTER,5);
            _loc2_ = AmmosFactory.getInstance().createObject(AmmosFactory.OBJECT_BURNING_OIL);
            _loc2_.initAmmo(AmmosFactory.BURNING_OIL_SKIN,owner,_loc3_,null,-1 * MathUtil.PI_QUARTER,5);
         }
         Pool.putPoint(_loc3_);
      }
      
      override public function nextState() : void
      {
         super.nextState();
         if(this.spawnShards && §_-X1j§.§_-12n§.isBoss)
         {
            §_-4B§.§_-qn§(owner);
         }
      }
      
      protected function shards() : void
      {
         new BunkerBusterShard(owner,x,y,Math.PI,20,this.paramSetEx);
         new BunkerBusterShard(owner,x,y,0,20,this.paramSetEx);
         new BunkerBusterShard(owner,x,y,MathUtil.PI_QUARTER,20,this.paramSetEx);
         new BunkerBusterShard(owner,x,y,MathUtil.PI_QUARTER * 3,20,this.paramSetEx);
         new BunkerBusterShard(owner,x,y,-MathUtil.PI_QUARTER * 3,20,this.paramSetEx);
         new BunkerBusterShard(owner,x,y,-MathUtil.PI_QUARTER,20,this.paramSetEx);
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         super.defaultOnDisposed(param1);
         this.paramSetEx = null;
         this.paramSet = null;
      }
      
      public function setCripple() : void
      {
         this.cripple = true;
      }
   }
}

