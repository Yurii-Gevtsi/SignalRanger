package ru.pragmatix.wormix.weapons.ammo
{
   import §_-L1H§.§_-C3e§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.§_-F2L§;
   
   public class JumpingMine extends Mine
   {
      
      private const JUMPING_DISTANCE:uint = 75;
      
      private const JUMPING_DISTANCE_SQUARED:uint = 5625;
      
      private const JUMP_POWER:uint = 6;
      
      private const JUMP_OFFSET_Y:int = -4;
      
      public var turnLive:int = 0;
      
      public function JumpingMine(param1:§_-01J§, param2:int, param3:int, param4:§_-C3e§, param5:uint, param6:Boolean)
      {
         super(param1,param2,param3,param4,true,param6);
         this.explosionDelay.value = param5;
      }
      
      override protected function initialize(param1:Point, param2:String = null, param3:Boolean = false, param4:String = "resurces/_ammo/") : void
      {
         super.initialize(param1,param2,param3,param4);
         §_-92S§ = false;
      }
      
      override public function nextState() : void
      {
         if(stable && !§_-X1j§.§_-12n§.gameMaster.§_-y2v§())
         {
            §_-F2L§.§_-x1v§(enemyUnitsVec,"disposed",false,this.jumpToNearestUnit);
         }
         super.nextState();
      }
      
      override protected function findTarget() : §_-01J§
      {
         var _loc1_:§_-01J§ = super.findTarget();
         if(Boolean(!friendlyFire) && Boolean(_loc1_) && _loc1_.squad.getTeamId() == owner.squad.getTeamId())
         {
            _loc1_ = null;
         }
         return _loc1_;
      }
      
      public function destroy() : void
      {
         explode();
      }
      
      private function jumpToNearestUnit(param1:§_-01J§) : Boolean
      {
         var _loc2_:Number = NaN;
         if(Physics.isGobjInRadiusFast(param1 as §_-F3o§,x,y,this.JUMPING_DISTANCE,this.JUMPING_DISTANCE_SQUARED,true))
         {
            if(param1 != owner && (!param1.stable || param1.crawling != 0))
            {
               _loc2_ = Math.atan2(param1.y - y,param1.x - x);
               setSpeedVector(_loc2_,this.JUMP_POWER);
               addSpeedXY(0,this.JUMP_OFFSET_Y);
               return true;
            }
         }
         return false;
      }
   }
}

