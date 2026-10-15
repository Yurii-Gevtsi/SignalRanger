package ru.pragmatix.wormix.weapons.ammo.phase
{
   import §_-A1K§.§_-TA§;
   import §_-gG§.§_-Z2z§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   
   public class HomingFrostPhase extends HomingPhaseAmmo
   {
      
      private static const ROTATION_SPEED:Number = 7 * MathUtil.PI_ONE_180;
      
      private static const SPEED:uint = 20;
      
      private static const DELAY_BEFORE_AIM:uint = 15;
      
      private var damage:§_-TA§;
      
      private var freezeDuration:uint;
      
      public function HomingFrostPhase(param1:§_-01J§, param2:int, param3:int, param4:Boolean, param5:§_-01J§, param6:int, param7:uint = 3)
      {
         this.damage = new §_-TA§(param6);
         this.freezeDuration = param7;
         super(param1,param2,param3,param4,param5 as §_-F3o§,13694463,SPEED);
         setSpeedXY(0,-SPEED);
         homingFeat.rotaSpeed = ROTATION_SPEED;
         homingFeat.delayBeforeAim = DELAY_BEFORE_AIM;
         §_-ni§ = false;
         §_-In§ = DamageType.COLD;
      }
      
      override protected function onTarget() : void
      {
         var _loc1_:§_-01J§ = target as §_-01J§;
         if(Boolean(_loc1_) && !_loc1_.disposed)
         {
            inflictDamage(_loc1_ as §_-F3o§,this.damage.value,0,0,false,true);
            _loc1_.buffs.place(new §_-Z2z§(this.freezeDuration));
            §_-h2B§.play(§_-d27§.§_-Y24§);
         }
      }
   }
}

