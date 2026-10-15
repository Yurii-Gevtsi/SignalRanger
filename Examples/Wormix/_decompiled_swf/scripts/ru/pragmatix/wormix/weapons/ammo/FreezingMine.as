package ru.pragmatix.wormix.weapons.ammo
{
   import §_-L1H§.§_-C3e§;
   import §_-b1F§.§_-G4§;
   import §_-gG§.§_-Z2z§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   
   public class FreezingMine extends Mine
   {
      
      public function FreezingMine(param1:§_-01J§, param2:int, param3:int, param4:§_-C3e§, param5:Boolean = true)
      {
         super(param1,param2,param3,param4,param5);
         §_-In§ = DamageType.COLD;
         explosionDelay.value = 1;
         mineDistance.value = param4.explosionRadius.value;
      }
      
      override protected function explode() : void
      {
         var _loc2_:§_-01J§ = null;
         var _loc1_:Vector.<§_-01J§> = Physics.getUnitsInCircle(x,y,paramSet.damageRadius.value);
         for each(_loc2_ in _loc1_)
         {
            §_-G4§.§_-Ee§(owner,_loc2_.x,_loc2_.y,_loc2_ as §_-F3o§,paramSet.damage.value,0,0,false,false,§_-In§);
            _loc2_.buffs.place(new §_-Z2z§(2));
         }
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_SMOKE,this.x,this.y,1 * paramSet.explosionRadius.value,§_-d27§.§_-Y24§);
         disposed = true;
      }
   }
}

