package ru.pragmatix.wormix.weapons.ammo.type.grenade.cluster
{
   import §_-L1H§.§_-C3e§;
   import §_-sQ§.§_-H3D§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Physics;
   
   public class SeekingShard extends HomingShard
   {
      
      private var searchAngle:Number;
      
      public function SeekingShard(param1:§_-01J§, param2:int, param3:int, param4:Number, param5:uint, param6:§_-C3e§, param7:Number, param8:uint = 350, param9:Number = 1, param10:Boolean = true)
      {
         super(param1,param2,param3,param4,param5,param6,param8,param9,param10);
         this.searchAngle = param7;
         _speed = 40;
         _searchDelay = 2;
      }
      
      override protected function findTarget(param1:int) : void
      {
         var _loc3_:§_-01J§ = null;
         var _loc4_:Number = NaN;
         var _loc2_:§_-01J§ = null;
         for each(_loc3_ in Physics.getUnitsInCircle(x,y,param1))
         {
            _loc4_ = Math.atan2(_loc3_.y - y,_loc3_.x - x);
            if(!_loc3_.disposed && _loc3_ != owner && _loc3_.squad.getTeamId() != owner.squad.getTeamId() && Math.abs(_loc4_ - angle) < this.searchAngle && (_loc2_ == null || §_-H3D§.distance(x - _loc3_.x,y - _loc3_.y) < §_-H3D§.distance(x - _loc2_.x,y - _loc2_.y)))
            {
               _loc2_ = _loc3_;
            }
         }
         target = _loc2_;
      }
   }
}

