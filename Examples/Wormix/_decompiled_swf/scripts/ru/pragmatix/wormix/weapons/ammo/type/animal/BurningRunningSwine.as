package ru.pragmatix.wormix.weapons.ammo.type.animal
{
   import §_-L1H§.§_-C3e§;
   import §_-b1F§.§_-G4§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.BurningOil;
   import ru.pragmatix.wormix.weapons.ammo.RunningSwine;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import starling.utils.Pool;
   
   public class BurningRunningSwine extends RunningSwine
   {
      
      private static const PI_HALF:Number = MathUtil.PI_HALF;
      
      private static const PI_ONE_SIX:Number = Math.PI / 6;
      
      private static const FIRE_EJECTION_PERIOD:uint = 15;
      
      private static const FIRE_AMOUNT:uint = 15;
      
      private var fireCounter:uint = 0;
      
      public function BurningRunningSwine(param1:§_-01J§, param2:§_-C3e§, param3:§_-C3e§)
      {
         super(param1,param2,param3);
         _width = _height = 41;
      }
      
      override protected function update() : void
      {
         var _loc1_:uint = 0;
         var _loc2_:uint = 0;
         var _loc3_:uint = 0;
         var _loc4_:uint = 0;
         var _loc5_:int = 0;
         var _loc6_:Number = NaN;
         var _loc7_:int = 0;
         var _loc8_:int = 0;
         var _loc9_:BurningOil = null;
         super.update();
         if(!this.disposed && this.fireCounter < FIRE_AMOUNT && frames % FIRE_EJECTION_PERIOD == 0)
         {
            ++this.fireCounter;
            _loc1_ = y - (_width >> 1);
            _loc2_ = 17;
            §_-G4§.§_-Ee§(owner,x,_loc1_,null,10,_loc2_ * 2,_loc2_,false);
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_DRILL,x,_loc1_,_loc2_);
            _loc3_ = 4;
            _loc4_ = 60;
            _loc5_ = -1;
            while(_loc5_ <= 1)
            {
               if(_loc5_ != 0)
               {
                  _loc6_ = -PI_HALF + _loc5_ * PI_ONE_SIX;
                  _loc7_ = x + _loc5_ * 6;
                  _loc8_ = y - 5;
                  _loc9_ = AmmosFactory.getInstance().createObject(AmmosFactory.OBJECT_BURNING_OIL) as BurningOil;
                  _loc9_.initAmmo(AmmosFactory.BURNING_OIL_SKIN,owner,Pool.getPoint(_loc7_,_loc8_),null,_loc6_,_loc3_,paramSet.pushForce.value);
                  _loc9_.setDuration(_loc4_);
                  _loc9_.rotate = true;
               }
               _loc5_++;
            }
         }
      }
   }
}

