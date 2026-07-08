package ru.pragmatix.wormix.weapons
{
   import §_-62§.*;
   import §_-73d§.§_-w1O§;
   import §_-91B§.§_-z2l§;
   import §_-A1K§.§_-TA§;
   import §_-z20§.§_-Tn§;
   import ru.pragmatix.wormix.utils.§_-2t§;
   import ru.pragmatix.wormix.weapons.ammo.MolotovProjectile;
   
   public class Molotov extends Grenade
   {
      
      private static const §_-R1J§:§_-TA§ = new §_-TA§(36);
      
      public function Molotov()
      {
         super();
         MIN_POWER = 15;
         MAX_POWER = 45;
         §_-Y2U§ = new §_-w1O§(MIN_POWER,MAX_POWER,120,false);
      }
      
      override protected function shoot() : void
      {
         var _loc1_:§_-TA§ = new §_-TA§(property["fireCount"] ? int(§_-TA§(property["fireCount"]).value / 2) : int(§_-R1J§.value / 2));
         ammo = new MolotovProjectile(owner,paramSet,shotParams.angle,§_-Yz§(shotParams.power),_loc1_);
         §_-Ws§();
         nextTurnOnStable();
      }
   }
}

