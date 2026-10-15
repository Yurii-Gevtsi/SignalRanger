package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-tf§;
   import §_-l1K§.§_-d2j§;
   import ru.pragmatix.wormix.weapons.ammo.DrillMissile;
   
   public class DrillGun extends Weapon
   {
      
      public static const §_-k1c§:uint = 45;
      
      public static const §_-Y7§:uint = 15;
      
      protected var §_-l1w§:int;
      
      public function DrillGun()
      {
         super();
         MIN_POWER = 15;
         MAX_POWER = 45;
         this.§_-l1w§ = §_-z1W§(new §_-d2j§());
         §_-Y2U§ = new §_-tf§();
      }
      
      override protected function shoot() : void
      {
         new DrillMissile(owner,paramSet,shotParams.angle,§_-Yz§(shotParams.power));
         detach();
         nextTurnOnStable();
      }
   }
}

