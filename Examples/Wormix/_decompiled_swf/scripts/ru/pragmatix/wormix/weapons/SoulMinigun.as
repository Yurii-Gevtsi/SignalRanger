package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-pT§;
   import §_-A1K§.§_-TA§;
   import §_-r1C§.§_-V1R§;
   import §_-r1C§.§_-h2B§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.phase.HarmPhase;
   import starling.utils.Pool;
   
   public class SoulMinigun extends Minigun
   {
      
      private static const SEARCH_ANGLE:Number = MathUtil.PI_QUARTER;
      
      private static const §_-o1Y§:uint = 700;
      
      private static const MIN_DAMAGE:§_-TA§ = new §_-TA§(4);
      
      private static const §_-bl§:§_-TA§ = new §_-TA§(8);
      
      private static const ROTATION_SPEED:Number = 4 * MathUtil.PI_ONE_180;
      
      protected var §_-Vd§:uint = 700;
      
      protected var targets:Vector.<§_-01J§>;
      
      protected var §_-z2z§:§_-V1R§;
      
      public function SoulMinigun()
      {
         super();
         MIN_POWER = MAX_POWER = 15;
         §_-Y2U§ = new §_-pT§(75,MIN_POWER);
         §_-A1Q§ = new §_-TA§(4);
      }
      
      private function §_-Ha§(param1:Number) : Vector.<§_-F3o§>
      {
         return Physics.getGameObjectsInCone(owner.x,owner.y,this.§_-Vd§,param1,SEARCH_ANGLE,this.§_-L1U§());
      }
      
      protected function §_-L1U§() : Vector.<§_-F3o§>
      {
         return §_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-434§(CollideType.UNIT);
      }
      
      override protected function shoot() : void
      {
         var _loc2_:§_-F3o§ = null;
         var _loc3_:§_-01J§ = null;
         this.targets = new Vector.<§_-01J§>(0);
         var _loc1_:Vector.<§_-F3o§> = this.§_-Ha§(shotParams.angle);
         for each(_loc2_ in _loc1_)
         {
            _loc3_ = §_-01J§(_loc2_);
            if(!_loc3_.disposed && _loc3_ != owner)
            {
               this.targets.push(_loc3_);
            }
         }
         super.shoot();
      }
      
      override protected function §_-G11§(param1:Number) : void
      {
         var _loc4_:uint = 0;
         var _loc5_:Point = null;
         this.§_-83u§();
         var _loc2_:§_-01J§ = this.targets.length > 0 ? this.targets[§_-X1j§.randomSeed.§_-Fh§(0,this.targets.length - 1)] : owner;
         var _loc3_:int = owner.attributes.hp.§_-33H§();
         _loc4_ = MIN_DAMAGE.value + uint(Math.max(0,_loc3_ - owner.hp) / _loc3_ * 5);
         _loc4_ = Math.min(_loc4_,§_-bl§.value);
         _loc5_ = getBarrelPoint(param1);
         var _loc6_:HarmPhase = new HarmPhase(owner,_loc5_.x,_loc5_.y,_loc2_,_loc4_,MAX_POWER,4);
         _loc6_.setSpeedVector(param1,MAX_POWER);
         _loc6_.setRotationSpeed(ROTATION_SPEED);
         Pool.putPoint(_loc5_);
         if(this.§_-z2z§)
         {
            this.§_-z2z§.stop();
         }
         this.§_-z2z§ = §_-h2B§.play(§_-d27§.§_-k1s§);
      }
      
      protected function §_-83u§() : void
      {
         var _loc2_:§_-01J§ = null;
         var _loc1_:Vector.<§_-01J§> = new Vector.<§_-01J§>(0);
         for each(_loc2_ in this.targets)
         {
            if(!_loc2_.disposed)
            {
               _loc1_.push(_loc2_);
            }
         }
         this.targets = _loc1_;
      }
   }
}

