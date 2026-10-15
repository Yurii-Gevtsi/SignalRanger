package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-x2C§;
   import §_-l1K§.§_-d2j§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.Parasite;
   
   public class ParasiteWeapon extends Weapon
   {
      
      public function ParasiteWeapon()
      {
         super();
         MIN_POWER = 15;
         MAX_POWER = 45;
         §_-z1W§(new §_-d2j§());
         §_-Y2U§ = new §_-x2C§(90,false,0);
      }
      
      override protected function shoot() : void
      {
         §_-h2B§.play(§_-d27§.§_-p20§);
         var _loc1_:Class = getProperty("ammoClass") as Class;
         var _loc2_:Parasite = new _loc1_(owner,owner.x,owner.y,paramSet,shotParams.angle,§_-Yz§(shotParams.power));
         _loc2_.setBlinkingClip(getProperty("attachedClip") as String);
         detach();
         nextTurnOnStable();
      }
   }
}

