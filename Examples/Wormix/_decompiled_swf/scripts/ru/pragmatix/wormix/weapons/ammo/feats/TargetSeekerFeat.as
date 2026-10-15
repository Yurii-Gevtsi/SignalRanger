package ru.pragmatix.wormix.weapons.ammo.feats
{
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   
   public class TargetSeekerFeat implements IFrameFeat, ITargetSource
   {
      
      private var ammo:§_-F3o§;
      
      private var seekRadius:uint;
      
      private var seekPeriod:uint;
      
      private var target:§_-01J§;
      
      private var enemies:Vector.<§_-01J§>;
      
      public function TargetSeekerFeat(param1:§_-F3o§, param2:uint, param3:uint = 1)
      {
         super();
         this.ammo = param1;
         this.seekRadius = param2;
         this.seekPeriod = param3 > 0 ? param3 : 1;
         this.enemies = §_-X1j§.§_-12n§.gameMaster.§_-83I§();
      }
      
      public function nextState(param1:uint) : void
      {
         var _loc2_:Vector.<§_-01J§> = null;
         if(Boolean(this.ammo && !this.ammo.disposed) && Boolean(!this.target) && param1 % this.seekPeriod == 0)
         {
            _loc2_ = Physics.getUnitsInCircle(this.ammo.x,this.ammo.y,this.seekRadius,this.enemies.concat(new Vector.<§_-01J§>()));
            if(_loc2_.length > 0)
            {
               this.target = _loc2_[0];
            }
         }
      }
      
      public function getTarget() : §_-01J§
      {
         return this.target;
      }
   }
}

