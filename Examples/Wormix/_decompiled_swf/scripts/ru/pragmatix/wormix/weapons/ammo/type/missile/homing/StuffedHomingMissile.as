package ru.pragmatix.wormix.weapons.ammo.type.missile.homing
{
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.objects.§_-01J§;
   
   public class StuffedHomingMissile extends HomingMissile
   {
      
      private static const BUFF_PERIOD:uint = 3;
      
      private var damagePerFrame:§_-TA§ = new §_-TA§(0);
      
      private var damageBonusLimit:§_-TA§ = new §_-TA§(0);
      
      private var bonusDamage:§_-TA§ = new §_-TA§(0);
      
      public function StuffedHomingMissile(param1:§_-01J§, param2:Point, param3:§_-C3e§, param4:Number, param5:uint, param6:Point, param7:int = 12)
      {
         damage = new §_-TA§(param3.damage.value);
         super(param1,param2,param3,param4,param5,param6,param7);
      }
      
      override public function nextState() : void
      {
         super.nextState();
         if(homingFeat.isTargetValid() && §_-22h§ % BUFF_PERIOD == 0)
         {
            if(this.bonusDamage.value < this.damageBonusLimit.value)
            {
               this.bonusDamage.value += this.damagePerFrame.value;
               damage.value += this.damagePerFrame.value;
            }
         }
      }
      
      public function setDamagePerSecond(param1:§_-TA§) : void
      {
         this.damagePerFrame.value = param1.value;
      }
      
      public function setDamageBonusLimit(param1:§_-TA§) : void
      {
         this.damageBonusLimit.value = param1.value;
      }
   }
}

