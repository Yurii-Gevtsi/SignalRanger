package ru.pragmatix.wormix.weapons
{
   import §_-l1K§.§_-N1Z§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.weapons.ammo.type.bullet.ElectroBullet;
   
   public class ElectroGun extends Weapon
   {
      
      public function ElectroGun()
      {
         super();
         MIN_POWER = MAX_POWER = 50;
         §_-z1W§(new §_-N1Z§(MIN_POWER,new <Force>[Force.GRAVITY],§_-N1Z§.§_-q17§));
      }
      
      override protected function shoot() : void
      {
         new ElectroBullet(owner,getBarrelPoint(shotParams.angle),paramSet,shotParams.angle,MIN_POWER);
         if(shotsLeft.value == 0)
         {
            detach();
            nextTurnOnStable();
         }
      }
   }
}

