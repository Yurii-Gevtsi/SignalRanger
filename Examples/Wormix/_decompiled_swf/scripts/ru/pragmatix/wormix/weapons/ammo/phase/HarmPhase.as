package ru.pragmatix.wormix.weapons.ammo.phase
{
   import §_-A1K§.§_-TA§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   
   public class HarmPhase extends PhaseAmmo
   {
      
      private var damage:§_-TA§;
      
      public function HarmPhase(param1:§_-01J§, param2:int, param3:int, param4:§_-01J§, param5:uint, param6:uint = 30, param7:uint = 0, param8:ParticleEffectType = null, param9:Boolean = true)
      {
         super(param1,param2,param3,true,param4 as §_-F3o§,3355443,param6,0.8,param8,param9);
         this.damage = new §_-TA§(param5);
         this.delayBeforeAim = param7;
      }
      
      override protected function doDamage() : void
      {
         inflictDamage(target,this.damage.value,0,0,false,true);
         §_-h2B§.play(§_-d27§.§_-k2k§);
      }
   }
}

