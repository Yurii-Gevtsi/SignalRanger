package ru.pragmatix.wormix.weapons.ammo.cloud
{
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import §_-b1F§.§_-G4§;
   import §_-gG§.§_-M2S§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.extensions.ColorArgb;
   
   public class FreezingCloud extends BasicCloud
   {
      
      private static const DURATION:§_-TA§ = new §_-TA§(5);
      
      public function FreezingCloud(param1:§_-01J§, param2:int, param3:int, param4:§_-C3e§, param5:ColorArgb, param6:uint = 0)
      {
         super(param1,param2,param3,param4,param5,DURATION.value);
      }
      
      override protected function applyOnUnit(param1:§_-01J§) : void
      {
         super.applyOnUnit(param1);
         param1.buffs.place(new §_-M2S§(1));
         §_-G4§.§_-Ee§(owner,param1.x,param1.y,param1 as §_-F3o§,paramSet.damage.value,0,0,true,false,DamageType.COLD,true,false);
      }
   }
}

