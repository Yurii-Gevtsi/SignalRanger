package ru.pragmatix.wormix.weapons
{
   import §_-A2U§.§_-A23§;
   import §_-Y1O§.*;
   import §_-l1K§.§_-d2j§;
   import §_-x2Q§.§_-dU§;
   import §_-zI§.§_-8D§;
   import §_-zI§.§_-X1q§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.StringUtil;
   import ru.pragmatix.wormix.utils.§_-6A§;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import starling.events.Event;
   
   public class ThrowableMineWeapon extends GuardingBotWeapon
   {
      
      public function ThrowableMineWeapon(param1:Class = null)
      {
         super(param1);
         §_-C3j§ = false;
         §_-r2C§ = [];
         MIN_POWER = 15;
         MAX_POWER = 45;
         §_-z1W§(new §_-d2j§());
      }
      
      override public function jumpShoot(param1:Boolean) : void
      {
      }
      
      override protected function §_-H1g§() : Mine
      {
         var _loc1_:Mine = super.§_-H1g§();
         _loc1_.setSpeedVector(shotParams.angle,§_-Yz§(shotParams.power));
         return _loc1_;
      }
   }
}

