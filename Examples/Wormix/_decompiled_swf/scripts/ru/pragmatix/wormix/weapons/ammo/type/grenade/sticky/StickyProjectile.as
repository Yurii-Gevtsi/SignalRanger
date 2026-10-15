package ru.pragmatix.wormix.weapons.ammo.type.grenade.sticky
{
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Zd§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.abstracts.AbstractStickyAmmo;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class StickyProjectile extends AbstractStickyAmmo
   {
      
      private var blinkClassName:String;
      
      protected var paramSet:§_-C3e§;
      
      private var paramSetEx:§_-C3e§;
      
      private var quantityShards:§_-TA§;
      
      private var classShard:Class;
      
      private var stickedOnce:Boolean = false;
      
      public function StickyProjectile(param1:§_-01J§, param2:§_-C3e§, param3:§_-C3e§, param4:String, param5:§_-TA§, param6:Class, param7:Number, param8:uint)
      {
         super(param1,param2,param7,param8,§_-Qi§.§_-n18§);
         this.paramSet = param2;
         this.paramSetEx = param3;
         this.quantityShards = param5;
         this.classShard = param6;
         this.blinkClassName = param4;
         §_-h2B§.play(§_-d27§.§_-G3Z§);
         §_-Nt§ = false;
      }
      
      override protected function onCollide(param1:Event) : void
      {
         super.onCollide(param1);
         if(Boolean(armature) && armature.animation.lastAnimationName != §_-K1t§.§_-J2e§)
         {
            armature.playAnimation(§_-K1t§.§_-J2e§);
         }
         §_-Zd§.§_-33o§(30,this.explode);
         if(!this.stickedOnce)
         {
            §_-h2B§.play(§_-d27§.§_-h1O§);
            this.stickedOnce = true;
         }
         physParams.removeAllForces();
         stable = false;
      }
      
      protected function explode() : void
      {
         var _loc1_:Array = null;
         var _loc2_:int = 0;
         var _loc3_:Number = NaN;
         var _loc4_:Point = null;
         var _loc5_:Number = NaN;
         var _loc6_:int = 0;
         if(!disposed)
         {
            inflictDamage(null,this.paramSet.damage.value,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value);
            disposed = true;
            Effects.explodeParticles(x,y,50);
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,x,y,4.5 * this.paramSet.explosionRadius.value,§_-d27§.§_-S14§);
            _loc1_ = [-MathUtil.PI_ONE_18,-MathUtil.PI_ONE_40,-MathUtil.PI_ONE_80,MathUtil.PI_ONE_60,MathUtil.PI_ONE_36,MathUtil.PI_ONE_20,-MathUtil.PI_ONE_24,MathUtil.PI_ONE_80];
            if(this.quantityShards.value > 8)
            {
               §_-X1j§.§_-12n§.scene.dispatchEvent(new §_-H1K§(§_-A2V§.§_-A2e§,"sticky projectile hack"));
            }
            _loc2_ = 0;
            while(_loc2_ < this.quantityShards.value)
            {
               _loc3_ = -MathUtil.PI_HALF + _loc1_[_loc2_];
               _loc4_ = this.getStartPositionOnShard(_loc3_);
               _loc5_ = -MathUtil.PI_HALF + MathUtil.PI_ONE_60 * §_-X1j§.randomSeed.§_-Fh§(-10,10);
               _loc6_ = §_-X1j§.randomSeed.§_-Fh§(7,12);
               new this.classShard(owner,_loc4_.x,_loc4_.y,_loc5_,_loc6_,this.paramSetEx,1,false);
               Pool.putPoint(_loc4_);
               _loc2_++;
            }
         }
      }
      
      protected function getStartPositionOnShard(param1:Number) : Point
      {
         return Pool.getPoint(x + Math.cos(param1) * 50,y + Math.sin(param1) * 50);
      }
   }
}

