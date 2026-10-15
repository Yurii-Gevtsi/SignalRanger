package ru.pragmatix.wormix.weapons.ammo
{
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-l1g§.§_-L3Q§;
   import §_-sQ§.§_-H3D§;
   import §_-w2i§.§_-Zd§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.simple.BouncingGrenade;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class MolotovProjectile extends BouncingGrenade
   {
      
      private static const BULLETS_IN_BATCH:§_-TA§ = new §_-TA§(6);
      
      private static const SHOT_DELAY:int = 1;
      
      private var fireCount:§_-TA§;
      
      private var _exploding:Boolean;
      
      private var speedBonus:Number;
      
      private var angleStep:Number;
      
      private var batchCheck:uint = 0;
      
      private var batch:§_-TA§;
      
      private var batchesLimit:§_-TA§;
      
      public function MolotovProjectile(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:uint, param5:§_-TA§)
      {
         var owner:§_-01J§ = param1;
         var paramSet:§_-C3e§ = param2;
         var angle:Number = param3;
         var power:uint = param4;
         var fireCount:§_-TA§ = param5;
         super(owner,paramSet,300,angle,power,0,"");
         this.fireCount = fireCount;
         this.angleStep = MathUtil.§_-M2N§ / fireCount.value;
         this.batchesLimit = new §_-TA§(fireCount.value / BULLETS_IN_BATCH.value);
         this.batch = new §_-TA§();
         §_-U1o§ = true;
         §_-Zd§.once(this,§_-L3Q§.§_-o13§,function(param1:Event):void
         {
            explode();
         });
         counter.detach();
         explodeOnCollideWithEnemy();
      }
      
      override public function explode() : void
      {
         if(this._exploding)
         {
            return;
         }
         this.speedBonus = Math.min(3,0.2 * Math.abs(speedX)) * §_-H3D§.sign(speedX);
         inflictDamage(null,paramSet.damage.value,paramSet.damageRadius.value,paramSet.explosionRadius.value);
         physParams.removeAllForces();
         setSpeedXY(0,0);
         this._exploding = true;
         frames = 0;
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,x,y,4.5 * paramSet.explosionRadius.value,§_-d27§.§_-Q2F§);
      }
      
      override protected function grenadeTick() : void
      {
         if(!disposed)
         {
            if(this._exploding)
            {
               if(frames > 0 && frames % SHOT_DELAY == 0)
               {
                  if(this.batch.value < this.batchesLimit.value)
                  {
                     this.shootBatch();
                     ++this.batch.value;
                  }
               }
               if(this.batch.value >= this.batchesLimit.value)
               {
                  disposed = true;
               }
            }
         }
      }
      
      private function shootBatch() : void
      {
         var _loc1_:BurningOil = null;
         ++this.batchCheck;
         if(this.batchCheck > 4)
         {
            §_-X1j§.§_-12n§.scene.dispatchEvent(new §_-H1K§(§_-A2V§.§_-A2e§,"molotov grenade hack"));
         }
         var _loc2_:Point = Pool.getPoint(x,y);
         var _loc3_:int = this.batch.value * BULLETS_IN_BATCH.value;
         while(_loc3_ < (this.batch.value + 1) * BULLETS_IN_BATCH.value && _loc3_ < this.fireCount.value)
         {
            _loc1_ = AmmosFactory.getInstance().createObject(AmmosFactory.OBJECT_BURNING_OIL) as BurningOil;
            _loc1_.initAmmo(AmmosFactory.BURNING_OIL_SKIN,owner,_loc2_,null,_loc3_ * this.angleStep,8);
            _loc1_.addSpeedXY(this.speedBonus,-2);
            _loc3_++;
         }
         Pool.putPoint(_loc2_);
      }
      
      public function get exploding() : Boolean
      {
         return this._exploding;
      }
   }
}

