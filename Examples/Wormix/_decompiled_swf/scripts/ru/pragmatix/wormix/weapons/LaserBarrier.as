package ru.pragmatix.wormix.weapons
{
   import §_-Vg§.*;
   import §_-l1K§.§_-d2j§;
   import §_-l1g§.§_-L3Q§;
   import §_-m0§.§_-X1F§;
   import §_-x2Q§.§_-dU§;
   import §_-zI§.§_-X1q§;
   import feathers.themes.FontColor;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import starling.events.Event;
   import starling.utils.Color;
   
   public class LaserBarrier extends Weapon
   {
      
      public function LaserBarrier()
      {
         super();
         §_-a1R§.value = false;
         §_-N1N§ = false;
         MIN_POWER = 15;
         MAX_POWER = 45;
         §_-z1W§(new §_-d2j§());
      }
      
      override protected function shoot() : void
      {
         var _loc1_:uint = property["radiusIncreasing"] ? uint(property["radiusIncreasing"]) : 0;
         var _loc2_:§_-5e§ = new §_-5e§(this.owner,paramSet.clone(),shotParams.angle,§_-Yz§(shotParams.power),_loc1_);
         _loc2_.addEventListener(§_-L3Q§.MINE_SET_UP,this.§_-J3A§);
         §_-X1j§.§_-12n§.gameMaster.§_-c2w§();
         detach();
      }
      
      protected function §_-J3A§(param1:Event) : void
      {
         if(Boolean(§_-X1j§.§_-12n§) && Boolean(§_-X1j§.§_-12n§.gameMaster))
         {
            §_-X1j§.§_-12n§.gameMaster.§_-c2w§(false);
            if(§_-X1j§.§_-12n§.gameMaster.§_-42C§())
            {
               §_-X1j§.§_-12n§.gameMaster.activeUnit.§_-vp§ = true;
               §_-X1F§(param1.data).mine.owner.reattachWeapon();
            }
         }
      }
   }
}

