package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-g1J§;
   import §_-82l§.*;
   import §_-P2i§.§_-b7§;
   import §_-Y1O§.§_-42h§;
   import §_-Y1O§.§_-V23§;
   import §_-l1K§.§_-d2j§;
   import §_-l1g§.§_-L3Q§;
   import §_-zI§.*;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.weapons.ammo.HoppingMissile;
   import ru.pragmatix.wormix.weapons.interfaces.§_-c2a§;
   import ru.pragmatix.wormix.weapons.interfaces.§_-jm§;
   import ru.pragmatix.wormix.weapons.interfaces.§_-y2Q§;
   import starling.events.Event;
   
   public class HoppingWeapon extends Weapon implements §_-c2a§, §_-jm§, §_-y2Q§
   {
      
      private var missile:HoppingMissile;
      
      public function HoppingWeapon(param1:uint = 0)
      {
         super(param1);
         MIN_POWER = 15;
         MAX_POWER = 45;
         §_-z1W§(new §_-d2j§());
         §_-YH§ = true;
      }
      
      override public function activate() : void
      {
         if(!shooting)
         {
            super.activate();
         }
      }
      
      override public function detach() : void
      {
         shooting = false;
         super.detach();
         §_-X1j§.§_-12n§.scene.gameInterface.§_-Q2D§();
         if(this.missile)
         {
            this.missile.removeEventListener(§_-L3Q§.DISPOSED,this.§_-92O§);
            this.missile = null;
            ControlSets.WEAPON_CONTROL_SET.deactivate();
            ControlSets.UNIT_CONTROL_SET.activate();
         }
      }
      
      override protected function shoot() : void
      {
         owner.§_-d2T§(clip);
         this.missile = new HoppingMissile(owner,paramSet,shotParams.angle,§_-Yz§(shotParams.power));
         this.missile.addEventListener(§_-L3Q§.DISPOSED,this.§_-92O§);
         shooting = true;
         ControlSets.WEAPON_CONTROL_SET.activate();
         ControlSets.UNIT_CONTROL_SET.deactivate();
         §_-b7§.§_-I21§(owner);
         nextTurnOnStable();
         §_-X1j§.§_-12n§.scene.gameInterface.§_-n2O§(Weapon.§_-N1r§);
      }
      
      public function weaponCharge() : void
      {
         if(Boolean(this.missile) && !this.missile.disposed)
         {
            this.missile.hop();
            if(this.missile.getHopsLeft() <= 0)
            {
               §_-X1j§.§_-12n§.scene.gameInterface.§_-Q2D§();
            }
         }
      }
      
      public function §_-92O§(param1:Event) : void
      {
         this.detach();
         §_-X1j§.§_-12n§.scene.gameInterface.§_-Q2D§();
      }
      
      public function weaponLeftPressed() : void
      {
      }
      
      public function weaponLeftReleased() : void
      {
      }
      
      public function weaponRightPressed() : void
      {
      }
      
      public function weaponRightReleased() : void
      {
      }
   }
}

