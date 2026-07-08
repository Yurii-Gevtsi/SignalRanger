package ru.pragmatix.wormix.weapons.ammo
{
   import §_-L1H§.§_-C3e§;
   import §_-b1F§.§_-G4§;
   import §_-gG§.§_-Z2z§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Zd§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.feats.HomingFeat;
   import ru.pragmatix.wormix.weapons.ammo.feats.TargetSeekerFeat;
   import starling.events.Event;
   
   public class FreezingMissile extends §_-Qi§
   {
      
      private static const DEFAULT_FREEZE_DURATION:int = 2;
      
      protected var paramSet:§_-C3e§;
      
      private var modifier:Number = 1;
      
      private var freezeDuration:uint = 2;
      
      private var directHitDuration:uint = 0;
      
      private var bonusDebuffs:Array = [];
      
      public function FreezingMissile(param1:§_-01J§, param2:Point, param3:§_-C3e§, param4:Number, param5:uint)
      {
         super(param1,param1.x,param1.y,false,§_-n18§,DamageType.COLD);
         this.paramSet = param3;
         §_-U1o§ = true;
         §_-A3d§ = true;
         initialize(param2,param3.ammoClassName);
         this.clip.rotation = param4 * 180 / Math.PI;
         physParams.addForce(Force.WIND);
         setSpeedVector(param4,param5);
         §_-Zd§.once(this,§_-L3Q§.§_-o13§,this.onCollide);
         §_-32§ = §_-uN§;
         Effects.attachParticleTrail(ParticleEffectType.AMMO_TRAIL,this);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
         §_-h2B§.play(§_-d27§.§_-w2g§);
      }
      
      protected function onCollide(param1:Event) : void
      {
         var _loc4_:§_-01J§ = null;
         var _loc5_:Number = NaN;
         var _loc6_:uint = 0;
         var _loc7_:Boolean = false;
         var _loc8_:String = null;
         disposed = true;
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_SMOKE,x,y,3 * this.paramSet.explosionRadius.value,§_-d27§.§_-Y24§);
         var _loc2_:§_-01J§ = Boolean(param1.data.other) && param1.data.other is §_-01J§ ? §_-01J§(param1.data.other) : null;
         var _loc3_:Vector.<§_-01J§> = Physics.getUnitsInCircle(x,y,this.paramSet.damageRadius.value);
         for each(_loc4_ in _loc3_)
         {
            if(!_loc4_.disposed)
            {
               _loc5_ = 1 * this.paramSet.damage.value;
               _loc6_ = this.freezeDuration;
               if(_loc4_ == _loc2_)
               {
                  _loc7_ = false;
                  for each(_loc8_ in this.bonusDebuffs)
                  {
                     if(_loc4_.buffs.hasBuff(_loc8_))
                     {
                        _loc7_ = true;
                     }
                  }
                  if(_loc7_)
                  {
                     _loc5_ *= this.modifier;
                     _loc6_ += this.directHitDuration;
                     if(_loc6_ > DEFAULT_FREEZE_DURATION)
                     {
                        _loc4_.buffs.§_-e1z§(§_-Z2z§.NAME);
                     }
                  }
               }
               §_-G4§.§_-Ee§(owner,_loc4_.x,_loc4_.y,_loc4_ as §_-F3o§,_loc5_,0,0,false,false,§_-In§);
               _loc4_.buffs.place(new §_-Z2z§(_loc6_));
            }
         }
      }
      
      public function addBonus(param1:Number) : void
      {
         this.modifier = 1 + param1;
      }
      
      public function setDirectHitDuration(param1:int) : void
      {
         this.directHitDuration = param1;
      }
      
      public function setFreezeDuration(param1:int) : void
      {
         this.freezeDuration = param1;
      }
      
      public function applyBonusOnDebuffs(param1:Array) : void
      {
         this.bonusDebuffs = param1;
      }
      
      public function enableTargetSearch() : void
      {
         var _loc1_:TargetSeekerFeat = new TargetSeekerFeat(this,200,3);
         var _loc2_:HomingFeat = new HomingFeat(this,20,MathUtil.PI_ONE_30);
         _loc2_.setTargetSource(_loc1_);
         §_-P1P§.push(_loc1_,_loc2_);
      }
   }
}

