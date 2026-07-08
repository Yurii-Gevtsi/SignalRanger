package ru.pragmatix.wormix.weapons.ammo.type.grenade.cluster
{
   import §_-L1H§.§_-C3e§;
   import §_-l1g§.§_-L3Q§;
   import §_-sQ§.§_-H3D§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.*;
   import starling.events.Event;
   
   public class HomingShard extends Shard
   {
      
      protected static var _speed:int;
      
      protected static var _searchDelay:int;
      
      protected static const ROTATION_SPEED:Number = 30 * MathUtil.PI_ONE_180;
      
      protected static const DEFAULT_SPEED:int = 15;
      
      protected static const DEFAULT_DELAY:int = 10;
      
      protected var counter:int = 0;
      
      protected var searchRadius:uint;
      
      protected var target:§_-01J§;
      
      public function HomingShard(param1:§_-01J§, param2:int, param3:int, param4:Number, param5:uint, param6:§_-C3e§, param7:uint = 350, param8:Number = 1, param9:Boolean = true)
      {
         _speed = DEFAULT_SPEED;
         _searchDelay = DEFAULT_DELAY;
         super(param1,param2,param3,param4,param5,param6,param8,param9);
         §_-X1j§.§_-12n§.scene.addEventListener(§_-L3Q§.FRAME,this.onFrame);
         this.searchRadius = param7;
      }
      
      private function onFrame(param1:Event) : void
      {
         if(this.counter >= _searchDelay)
         {
            if(!this.target)
            {
               this.findTarget(this.searchRadius);
            }
            else if(!this.target.disposed)
            {
               setSpeedVector(§_-H3D§.§_-r2H§(angle,Math.atan2(this.target.y - y,this.target.x - x),ROTATION_SPEED),_speed);
            }
            else
            {
               this.target = null;
               this.findTarget(this.searchRadius / 2);
            }
         }
         ++this.counter;
      }
      
      protected function findTarget(param1:int) : void
      {
         var _loc2_:Array = null;
         var _loc3_:§_-01J§ = null;
         var _loc4_:uint = 0;
         if(this.counter % 3 == 0)
         {
            _loc2_ = [];
            for each(_loc3_ in Physics.getUnitsInCircle(x,y,param1))
            {
               if(!_loc3_.disposed && _loc3_ != owner && _loc3_.squad.getTeamId() != owner.squad.getTeamId())
               {
                  _loc2_.push(_loc3_);
               }
            }
            _loc4_ = uint(§_-X1j§.randomSeed.§_-Fh§(0,_loc2_.length));
            this.target = _loc2_[_loc4_];
         }
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         §_-X1j§.§_-12n§.scene.removeEventListener(§_-L3Q§.FRAME,this.onFrame);
         super.defaultOnDisposed(param1);
      }
   }
}

