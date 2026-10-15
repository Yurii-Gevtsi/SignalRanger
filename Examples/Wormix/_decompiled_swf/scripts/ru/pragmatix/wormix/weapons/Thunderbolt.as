package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-x2C§;
   import §_-l1K§.§_-d2j§;
   import ru.pragmatix.wormix.physics.model.UnitPhysicModel;
   import ru.pragmatix.wormix.weapons.ammo.ThunderboltMissile;
   
   public class Thunderbolt extends Weapon
   {
      
      public function Thunderbolt()
      {
         super();
         MIN_POWER = 15;
         MAX_POWER = 45;
         §_-Y2U§ = new §_-x2C§(100,true);
         §_-z1W§(new §_-d2j§());
      }
      
      override protected function shoot() : void
      {
         new ThunderboltMissile(owner,paramSet,paramSetEx,shotParams.angle,§_-Yz§(shotParams.power));
         UnitPhysicModel(owner.getPhysicModel()).rolling = -owner.direction * 25;
         detach();
         nextTurnOnStable();
      }
   }
}

