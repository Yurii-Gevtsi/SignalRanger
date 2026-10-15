package ru.pragmatix.wormix.weapons.ammo.phase
{
   import §_-A1K§.§_-TA§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.HealBlocker;
   
   public class BlockHealPhase extends PhaseAmmo
   {
      
      private var heal:§_-TA§;
      
      public function BlockHealPhase(param1:§_-01J§, param2:int, param3:int, param4:HealBlocker, param5:uint)
      {
         this.heal = new §_-TA§(param5);
         §_-ni§ = false;
         super(param1,param2,param3,true,param4,16711680,20,1);
      }
      
      override protected function doDamage() : void
      {
      }
      
      override public function nextState() : void
      {
         var _loc1_:Number = NaN;
         super.nextState();
         if(!disposed && !target.disposed)
         {
            _loc1_ = MathUtil.distance(x,y,target.x,target.y);
            if(_loc1_ <= ammoSpeed)
            {
               this.doDamage();
               clear();
            }
         }
      }
   }
}

