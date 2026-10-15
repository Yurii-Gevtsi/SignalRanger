package ru.pragmatix.wormix.weapons.ammo.phase
{
   import §_-A1K§.§_-TA§;
   import §_-P1B§.§_-b21§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   
   public class HealBolt extends PhaseAmmo
   {
      
      private var heal:§_-TA§;
      
      private var unit:§_-01J§;
      
      public function HealBolt(param1:§_-01J§, param2:int, param3:int, param4:§_-01J§, param5:uint)
      {
         super(param1,param2,param3,true,param4 as §_-F3o§,13934615,40);
         this.heal = new §_-TA§(param5);
         §_-ni§ = false;
         this.unit = param4;
      }
      
      override protected function doDamage() : void
      {
         var _loc1_:uint = this.unit.attributes.hp.§_-33H§() >= this.unit.hp + this.heal.value ? uint(this.heal.value) : uint(Math.max(this.unit.attributes.hp.§_-33H§() - this.unit.hp,0));
         target.attributes.hp.restore(this.heal.value,§_-b21§.§_-uy§,false,false);
      }
   }
}

