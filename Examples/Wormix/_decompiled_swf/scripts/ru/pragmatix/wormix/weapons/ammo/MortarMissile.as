package ru.pragmatix.wormix.weapons.ammo
{
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
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
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.events.Event;
   
   public class MortarMissile extends §_-Qi§
   {
      
      private static const SHARD_COUNT:§_-TA§ = new §_-TA§(5);
      
      private static const CLEVER_SHARD_POWER:§_-TA§ = new §_-TA§(15);
      
      private static const NORMAL_SHARD_POWER:§_-TA§ = new §_-TA§(5);
      
      private var cleverShards:Boolean = false;
      
      public function MortarMissile(param1:§_-01J§, param2:Point, param3:§_-C3e§, param4:§_-C3e§, param5:Number, param6:uint)
      {
         var self:MortarMissile = null;
         var owner:§_-01J§ = param1;
         var initialPoint:Point = param2;
         var paramSet:§_-C3e§ = param3;
         var paramSetEx:§_-C3e§ = param4;
         var angle:Number = param5;
         var power:uint = param6;
         super(owner,initialPoint.x,initialPoint.y,false);
         §_-U1o§ = true;
         initialize(initialPoint,paramSet.ammoClassName);
         §_-h2B§.play(§_-d27§.§_-I1P§);
         setSpeedVector(angle,power);
         self = this;
         §_-Zd§.once(this,§_-L3Q§.§_-o13§,function(param1:Event):void
         {
            var _loc3_:Number = NaN;
            var _loc4_:uint = 0;
            var _loc6_:Vector.<§_-01J§> = null;
            var _loc7_:Number = NaN;
            var _loc8_:int = 0;
            var _loc9_:int = 0;
            var _loc10_:Shard = null;
            self.inflictDamage(param1.data.other,paramSet.damage.value,paramSet.damageRadius.value,paramSet.explosionRadius.value);
            self.disposed = true;
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,self.x,self.y,3 * paramSet.explosionRadius.value,§_-d27§.§_-33W§);
            var _loc2_:Number = cleverShards ? MathUtil.PI_ONE_48 : MathUtil.PI_QUARTER;
            if(cleverShards)
            {
               _loc6_ = Physics.getUnitsInCircle(self.x,self.y,paramSet.damageRadius.value);
               _loc3_ = _loc6_.length > 0 ? Math.atan2(_loc6_[0].y - self.y,_loc6_[0].x - self.x) : Math.PI + self.angle;
               _loc4_ = uint(CLEVER_SHARD_POWER.value);
            }
            else
            {
               _loc3_ = Math.PI + self.angle;
               _loc4_ = uint(NORMAL_SHARD_POWER.value);
            }
            var _loc5_:int = 0;
            while(_loc5_ < SHARD_COUNT.value)
            {
               _loc7_ = §_-X1j§.randomSeed.§_-H3r§(_loc3_ - _loc2_,_loc3_ + _loc2_);
               _loc8_ = self.x + Math.cos(_loc7_) * 10;
               _loc9_ = self.y + Math.sin(_loc7_) * 10;
               _loc10_ = new Shard(owner,_loc8_,_loc9_,_loc7_,_loc4_,paramSetEx,0.6);
               _loc5_++;
            }
         });
         §_-32§ = §_-uN§;
         Effects.attachParticleTrail(ParticleEffectType.AMMO_TRAIL,this);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      public function enableCleverShards() : void
      {
         this.cleverShards = true;
      }
   }
}

