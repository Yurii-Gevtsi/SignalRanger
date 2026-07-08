package ru.pragmatix.wormix.weapons.ammo.type.grenade.simple
{
   import §_-A1K§.§_-TA§;
   import §_-A2F§.§_-4B§;
   import §_-L1H§.§_-C3e§;
   import §_-b1F§.§_-G4§;
   import §_-w2i§.§_-Ia§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   
   public class StuffedBouncingGrenade extends BouncingGrenade
   {
      
      private static const EXPLOSION_DELAY:§_-TA§ = new §_-TA§(5);
      
      private static const MIN_DISTANCE_MINI_EXPLODE:§_-TA§ = new §_-TA§(20);
      
      private static const MAX_DISTANCE_MINI_EXPLODE:§_-TA§ = new §_-TA§(60);
      
      private var cripple:Boolean = false;
      
      private var miniExplosions:§_-TA§ = new §_-TA§(2);
      
      private var paramSetEx:§_-C3e§;
      
      private var unstabilizer:§_-F3o§;
      
      public function StuffedBouncingGrenade(param1:§_-01J§, param2:§_-C3e§, param3:uint, param4:Number, param5:uint, param6:Number, param7:String)
      {
         super(param1,param2,param3,param4,param5,param6,param7);
         this.paramSetEx = new §_-C3e§();
         this.paramSetEx.damage = new §_-TA§(25);
         this.paramSetEx.damageRadius = new §_-TA§(35);
         this.paramSetEx.explosionRadius = new §_-TA§(22);
      }
      
      override public function explode() : void
      {
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         var _loc4_:int = 0;
         var _loc5_:int = 0;
         var _loc1_:int = 0;
         while(_loc1_ < this.miniExplosions.value)
         {
            _loc2_ = §_-X1j§.randomSeed.§_-Fh§(MIN_DISTANCE_MINI_EXPLODE.value,MAX_DISTANCE_MINI_EXPLODE.value);
            _loc3_ = §_-X1j§.randomSeed.§_-Fh§(0,360);
            _loc4_ = x + _loc2_ * Math.cos(_loc3_);
            _loc5_ = y + _loc2_ * Math.sin(_loc3_);
            §_-Ia§.§_-33o§(EXPLOSION_DELAY.value * (_loc1_ + 1),this.miniExplode,_loc4_,_loc5_,owner);
            _loc1_++;
         }
         this.unstabilizer = §_-X1j§.§_-12n§.scene.makeUnstabilizer();
         §_-Ia§.§_-33o§(EXPLOSION_DELAY.value * this.miniExplosions.value,this.destroyUnstabilizer);
         if(this.cripple)
         {
            §_-4B§.§_-x4§(x,y,paramSet.damageRadius.value);
         }
         super.explode();
      }
      
      private function miniExplode(param1:int, param2:int, param3:§_-01J§) : void
      {
         §_-G4§.§_-Ee§(param3,param1,param2,null,this.paramSetEx.damage.value,this.paramSetEx.damageRadius.value,this.paramSetEx.explosionRadius.value,true);
         Effects.explodeParticles(param1,param2,12);
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_DRILL,param1,param2,3 * this.paramSetEx.damageRadius.value,§_-d27§.§_-S14§);
         if(this.cripple)
         {
            §_-4B§.§_-x4§(param1,param2,this.paramSetEx.damageRadius.value);
         }
      }
      
      private function destroyUnstabilizer() : void
      {
         if(Boolean(this.unstabilizer) && Boolean(this.unstabilizer.physParams))
         {
            this.unstabilizer.disposed = true;
            this.unstabilizer = null;
         }
      }
      
      public function setMiniExplosions(param1:int) : void
      {
         this.miniExplosions.value = param1;
      }
      
      public function setCripple() : void
      {
         this.cripple = true;
      }
   }
}

