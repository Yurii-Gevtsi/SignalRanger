package ru.pragmatix.wormix.weapons.ammo.type.grenade.cluster
{
   import §_-L1H§.§_-C3e§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.ammo.Shard;
   
   public class SensoryShard extends Shard
   {
      
      private static const DISTANCE:uint = 100;
      
      private static const POWER:uint = 5;
      
      private static const OFFSET_Y:int = -4;
      
      public function SensoryShard(param1:§_-01J§, param2:int, param3:int, param4:Number, param5:uint, param6:§_-C3e§, param7:Number = 1, param8:Boolean = true)
      {
         super(param1,param2,param3,param4,param5,param6,param7,param8);
      }
      
      override public function nextState() : void
      {
         super.nextState();
         this.checkTargets();
      }
      
      private function checkTargets() : void
      {
         var _loc2_:§_-01J§ = null;
         var _loc3_:Number = NaN;
         var _loc1_:Vector.<§_-01J§> = Physics.getUnitsInCircle(x,y,DISTANCE);
         for each(_loc2_ in _loc1_)
         {
            if(!_loc2_.disposed && _loc2_ != owner && (!_loc2_.stable || _loc2_.crawling != 0))
            {
               _loc3_ = Math.atan2(_loc2_.y - y,_loc2_.x - x);
               setSpeedVector(_loc3_,POWER);
               addSpeedXY(0,OFFSET_Y);
               break;
            }
         }
      }
   }
}

