package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-pT§;
   import §_-l1K§.§_-W2f§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.weapons.ammo.phase.FrostBolt;
   
   public class Blizzard extends Weapon
   {
      
      public function Blizzard()
      {
         super();
         MIN_POWER = MAX_POWER = 20;
         §_-z1W§(new §_-W2f§());
         §_-Y2U§ = new §_-pT§(70,MIN_POWER);
      }
      
      override protected function shoot() : void
      {
         §_-h2B§.play(§_-d27§.§_-Lp§);
         new FrostBolt(owner,paramSet,shotParams.angle,MIN_POWER * 0.87,false);
         var _loc1_:int = 0;
         while(_loc1_ < 4)
         {
            new FrostBolt(owner,paramSet,shotParams.angle,MIN_POWER,true);
            _loc1_++;
         }
         detach();
         nextTurnOnStable();
      }
   }
}

