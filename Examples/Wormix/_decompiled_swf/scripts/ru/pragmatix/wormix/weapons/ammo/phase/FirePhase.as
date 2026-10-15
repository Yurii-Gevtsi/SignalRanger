package ru.pragmatix.wormix.weapons.ammo.phase
{
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   
   public class FirePhase extends PhaseAmmo
   {
      
      private var damage:uint;
      
      public function FirePhase(param1:§_-01J§, param2:int, param3:int, param4:§_-F3o§, param5:uint, param6:uint = 30, param7:uint = 0)
      {
         super(param1,param2,param3,true,param4,13934615,param6,0.8);
         this.damage = param5;
         this.§_-In§ = DamageType.FIRE;
      }
      
      override protected function doDamage() : void
      {
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,x,y,15,§_-d27§.§_-I1P§);
         inflictDamage(target,this.damage,0,0,false,false);
      }
   }
}

