package ru.pragmatix.wormix.weapons.ammo.type.air.cat
{
   import §_-L1H§.§_-C3e§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.AirCatAmmo;
   import ru.pragmatix.wormix.weapons.ammo.BurningOil;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class AirFireCatAmmo extends AirCatAmmo
   {
      
      private static const FIRE_DROPS_COUNT:uint = 5;
      
      public function AirFireCatAmmo(param1:§_-01J§, param2:§_-C3e§, param3:int, param4:int, param5:int, param6:int, param7:DamageType, param8:int, param9:int)
      {
         super(param1,param2,param3,param4,param5,param6,param7,param8,param9);
      }
      
      override protected function defaultOnCollide(param1:Event) : void
      {
         var _loc2_:int = 0;
         var _loc3_:BurningOil = null;
         if(!disposed)
         {
            _loc2_ = int(-FIRE_DROPS_COUNT / 2);
            while(_loc2_ <= FIRE_DROPS_COUNT / 2)
            {
               _loc3_ = AmmosFactory.getInstance().createObject(AmmosFactory.OBJECT_BURNING_OIL) as BurningOil;
               _loc3_.initAmmo(AmmosFactory.BURNING_OIL_SKIN,owner,Pool.getPoint(x + _loc2_ * 6,y - 5),null,-MathUtil.PI_HALF + _loc2_ * MathUtil.§_-4S§,7);
               _loc3_.setDuration(60);
               _loc3_.rotate = true;
               _loc2_++;
            }
         }
         super.defaultOnCollide(param1);
      }
   }
}

