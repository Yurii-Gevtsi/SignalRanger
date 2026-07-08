package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-pT§;
   import §_-Y1O§.§_-42h§;
   import §_-r1C§.§_-h2B§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.phase.FirePhase;
   import starling.utils.Pool;
   
   public class FireMinigun extends SoulMinigun
   {
      
      private static const §_-V§:uint = 4;
      
      public function FireMinigun()
      {
         super();
         MIN_POWER = MAX_POWER = 30;
         §_-Y2U§ = new §_-pT§(85,MIN_POWER);
         §_-Vd§ = 2000;
      }
      
      override protected function §_-L1U§() : Vector.<§_-F3o§>
      {
         var _loc2_:§_-01J§ = null;
         var _loc1_:Vector.<§_-F3o§> = new Vector.<§_-F3o§>(0);
         for each(_loc2_ in §_-X1j§.§_-12n§.gameMaster.§_-83I§(owner))
         {
            _loc1_.push(_loc2_);
         }
         return _loc1_;
      }
      
      override protected function §_-G11§(param1:Number) : void
      {
         var _loc3_:Point = null;
         §_-83u§();
         var _loc2_:§_-01J§ = targets.length > 0 ? targets[§_-X1j§.randomSeed.§_-Fh§(0,targets.length - 1)] : owner;
         _loc3_ = getBarrelPoint(param1);
         var _temp_1:* = new FirePhase(owner,_loc3_.x,_loc3_.y,_loc2_ as §_-F3o§,§_-V§,MAX_POWER,24);
         var _loc4_:FirePhase = new FirePhase(owner,_loc3_.x,_loc3_.y,_loc2_ as §_-F3o§,§_-V§,MAX_POWER,24);
         _loc4_.setSpeedVector(param1,MAX_POWER);
         _loc4_.setRotationSpeed(4 * MathUtil.PI_ONE_180);
         Pool.putPoint(_loc3_);
         §_-h2B§.play(§_-d27§.§_-tp§);
      }
   }
}

