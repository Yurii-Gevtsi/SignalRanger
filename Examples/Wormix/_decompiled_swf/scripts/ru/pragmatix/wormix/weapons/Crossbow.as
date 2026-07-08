package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-v1o§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-T2N§;
   import §_-r1C§.§_-h2B§;
   import §_-u1P§.ShopResultEvent;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.CrossbowBolt;
   
   public class Crossbow extends Weapon
   {
      
      public function Crossbow()
      {
         super();
         MIN_POWER = MAX_POWER = 50;
         §_-z1W§(new §_-W2f§());
         §_-Y2U§ = new §_-v1o§(110,MIN_POWER,true,220,100);
      }
      
      override protected function shoot() : void
      {
         §_-h2B§.play(§_-d27§.§_-21e§);
         var _loc1_:Number = MathUtil.PI_ONE_60;
         var _loc2_:int = -2;
         while(_loc2_ <= 2)
         {
            new CrossbowBolt(owner,paramSet,shotParams.angle + _loc2_ * _loc1_,MAX_POWER + §_-X1j§.randomSeed.§_-Fh§(-4,4));
            _loc2_++;
         }
         if(shotsLeft.value == 0)
         {
            detach();
            nextTurnOnStable();
         }
      }
   }
}

