package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-x2C§;
   import §_-bI§.*;
   import §_-l1K§.§_-d2j§;
   import §_-r1C§.§_-h2B§;
   import feathers.layout.RelativePosition;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.weapons.ammo.VilePumpkin;
   
   public class PumpkinWeapon extends Weapon
   {
      
      public function PumpkinWeapon()
      {
         super();
         MIN_POWER = 15;
         MAX_POWER = 45;
         §_-z1W§(new §_-d2j§());
         §_-Y2U§ = new §_-x2C§(90,false,0);
      }
      
      override protected function shoot() : void
      {
         §_-h2B§.play(§_-d27§.§_-G3Z§);
         new VilePumpkin(owner,owner.x,owner.y,paramSet,shotParams.angle,§_-Yz§(shotParams.power));
         detach();
         nextTurnOnStable();
      }
   }
}

