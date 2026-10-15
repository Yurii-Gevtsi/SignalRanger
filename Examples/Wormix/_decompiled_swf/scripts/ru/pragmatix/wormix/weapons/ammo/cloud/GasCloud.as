package ru.pragmatix.wormix.weapons.ammo.cloud
{
   import §_-12a§.§_-E1Q§;
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import §_-b1F§.§_-G4§;
   import §_-gG§.§_-5R§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.extensions.ColorArgb;
   
   public class GasCloud extends BasicCloud
   {
      
      private static const DURATION:§_-TA§ = new §_-TA§(6);
      
      private var armorReduction:uint = 0;
      
      public function GasCloud(param1:§_-01J§, param2:int, param3:int, param4:§_-C3e§, param5:ColorArgb, param6:uint)
      {
         super(param1,param2,param3,param4,param5,DURATION.value);
         this.armorReduction = param6;
      }
      
      override protected function isUnitAffected(param1:§_-01J§) : Boolean
      {
         return Boolean(super.isUnitAffected(param1)) && !param1.buffs.hasBuff(§_-E1Q§.NAME);
      }
      
      override protected function applyOnUnit(param1:§_-01J§) : void
      {
         var _loc2_:uint = 0;
         super.applyOnUnit(param1);
         if(this.armorReduction > 0)
         {
            _loc2_ = param1 != null ? uint(Math.round(0.01 * this.armorReduction * param1.attributes.armor.value)) : 0;
            if(_loc2_ > 0)
            {
               param1.buffs.place(new §_-5R§(_loc2_,2));
            }
         }
         §_-G4§.§_-Ee§(owner,param1.x,param1.y,param1 as §_-F3o§,paramSet.damage.value,0,0,true,true,DamageType.POISON);
      }
   }
}

