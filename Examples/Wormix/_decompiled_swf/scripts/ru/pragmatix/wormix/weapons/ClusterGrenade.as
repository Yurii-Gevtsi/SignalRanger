package ru.pragmatix.wormix.weapons
{
   import §_-62§.§_-G1p§;
   import §_-62§.§_-W1r§;
   import §_-73d§.§_-w1O§;
   import §_-A1K§.§_-TA§;
   import §_-G2H§.*;
   import §_-q26§.§_-52J§;
   import flash.net.SharedObject;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanInviteStructure;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.cluster.ShellBurst;
   
   public class ClusterGrenade extends Grenade
   {
      
      private static const §_-w1V§:Number = 0.5;
      
      public function ClusterGrenade()
      {
         super();
         §_-Y2U§ = new §_-w1O§(MIN_POWER,MAX_POWER,75);
      }
      
      override protected function shoot() : void
      {
         §_-BB§();
         var _loc1_:Class = getProperty("ammoClass") as Class;
         ammo = new _loc1_(owner,paramSet,new ShellBurst(owner,paramSetEx,§_-TA§(getProperty("quantityShards")),getProperty("classShard") as Class),clock.value,shotParams.angle,§_-Yz§(shotParams.power),§_-w1V§,getProperty("blinkClassName"));
         if(getProperty("explodeOnStopMoving"))
         {
            §_-B2r§ = true;
            ammo.explodeOnStopMoving();
         }
         if(Boolean(getProperty("hasTimer")) && Boolean(!getProperty("hasTimer").value) && !shotParams.§_-R18§)
         {
            ammo.removeTimer();
         }
         §_-Ws§();
         nextTurnOnStable();
      }
   }
}

