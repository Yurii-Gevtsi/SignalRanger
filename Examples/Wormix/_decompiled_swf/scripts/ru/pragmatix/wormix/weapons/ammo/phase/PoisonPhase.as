package ru.pragmatix.wormix.weapons.ammo.phase
{
   import §_-31N§.§_-Yy§;
   import §_-gG§.§_-8u§;
   import §_-r1C§.§_-h2B§;
   import §_-sQ§.§_-H3D§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   
   public class PoisonPhase extends PhaseAmmo
   {
      
      private const MAX_DODGES:int = 5;
      
      private const DAMAGE_DEFAULT:int = 30;
      
      private const HP_LIMIT_DEFAULT:int = 20;
      
      private const ROTATION_SPEED:Number = 7;
      
      private const SPEED:uint = 15;
      
      private var damage:int;
      
      private var unit:§_-01J§;
      
      private var hpLimit:int;
      
      public function PoisonPhase(param1:§_-01J§, param2:int, param3:int, param4:Boolean, param5:§_-01J§, param6:int = 30, param7:int = 20)
      {
         this.damage = param6;
         this.hpLimit = param7;
         this.unit = param5;
         super(param1,param2,param3,param4,param5 as §_-F3o§,0,this.SPEED,1);
         setSpeedXY(0,-this.SPEED);
      }
      
      override public function nextState() : void
      {
         ++counter;
         super.nextState();
      }
      
      override protected function processMovement() : void
      {
         var _loc1_:Number = NaN;
         var _loc2_:Number = NaN;
         var _loc3_:Number = NaN;
         if(counter > 15)
         {
            if(!disposed && !target.disposed)
            {
               _loc1_ = target.x - x;
               _loc2_ = target.y - y;
               _loc3_ = §_-H3D§.§_-r2H§(angle,Math.atan2(_loc2_,_loc1_),this.ROTATION_SPEED);
               setSpeedVector(_loc3_,this.SPEED);
            }
            else
            {
               clear();
            }
         }
      }
      
      override protected function defaultOnBeforeHit(param1:§_-Yy§) : void
      {
         super.defaultOnBeforeHit(param1);
         if(param1.§_-K1l§)
         {
            clear();
         }
      }
      
      override protected function doDamage() : void
      {
         if(!target.disposed)
         {
            if(this.unit.hp > this.hpLimit)
            {
               inflictDamage(target,Math.min(this.damage,this.unit.hp - this.hpLimit),0,0,false,true);
            }
            this.unit.buffs.place(new §_-8u§(owner));
            §_-h2B§.play(§_-d27§.§_-h1O§);
         }
      }
   }
}

