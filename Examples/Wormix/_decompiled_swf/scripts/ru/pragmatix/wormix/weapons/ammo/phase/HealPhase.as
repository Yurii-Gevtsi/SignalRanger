package ru.pragmatix.wormix.weapons.ammo.phase
{
   import §_-A1K§.§_-TA§;
   import §_-P1B§.§_-b21§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.utils.MathUtil;
   
   public class HealPhase extends PhaseAmmo
   {
      
      private var heal:§_-TA§;
      
      private var blocks:Boolean;
      
      public function HealPhase(param1:§_-01J§, param2:int, param3:int, param4:§_-01J§, param5:uint, param6:Boolean = false, param7:uint = 13934615)
      {
         super(param1,param2,param3,true,param4 as §_-F3o§,param7,40);
         this.heal = new §_-TA§(param5);
         this.blocks = param6;
         §_-ni§ = false;
      }
      
      protected function onTarget() : void
      {
         var _loc1_:int = 0;
         if(target is §_-01J§)
         {
            _loc1_ = §_-01J§(target).bonuses.getStatValue(§_-x2M§.HEAL_BONUS);
            if(_loc1_ > 0)
            {
               this.heal.value += MathUtil.§_-na§(_loc1_,this.heal.value);
            }
         }
         target.attributes.hp.restore(this.heal.value,§_-b21§.§_-uy§,false,this.blocks);
      }
      
      override protected function doDamage() : void
      {
         super.doDamage();
         if(target)
         {
            this.onTarget();
         }
      }
   }
}

