package ru.pragmatix.wormix.weapons.ammo.type.grenade.sticky
{
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import §_-gG§.§_-8u§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Physics;
   
   public class PoisonStickyProjectile extends StickyProjectile
   {
      
      private const POISON_DURATION:§_-TA§ = new §_-TA§(8);
      
      public function PoisonStickyProjectile(param1:§_-01J§, param2:§_-C3e§, param3:§_-C3e§, param4:String, param5:§_-TA§, param6:Class, param7:Number, param8:uint)
      {
         super(param1,param2,param3,param4,param5,param6,param7,param8);
      }
      
      override protected function explode() : void
      {
         var _loc1_:Vector.<§_-01J§> = null;
         var _loc2_:§_-01J§ = null;
         if(!disposed)
         {
            _loc1_ = Physics.getUnitsInCircle(x,y,paramSet.damageRadius.value);
            for each(_loc2_ in _loc1_)
            {
               if(!_loc2_.disposed && !_loc2_.buffs.hasBuff(§_-8u§.NAME))
               {
                  _loc2_.buffs.place(new §_-8u§(owner,this.POISON_DURATION.value));
               }
            }
         }
         super.explode();
      }
   }
}

