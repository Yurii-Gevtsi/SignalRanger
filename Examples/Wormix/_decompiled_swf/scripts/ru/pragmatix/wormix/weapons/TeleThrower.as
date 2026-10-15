package ru.pragmatix.wormix.weapons
{
   import §_-62§.§_-W1r§;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-l1K§.§_-d2j§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.BouncingTeleport;
   import ru.pragmatix.wormix.weapons.ammo.LaserRay;
   import ru.pragmatix.wormix.weapons.interfaces.§_-y2Q§;
   
   public class TeleThrower extends Weapon implements §_-y2Q§
   {
      
      private static const §_-Ts§:§_-TA§ = new §_-TA§(26);
      
      private var §_-R1c§:§_-7u§ = new §_-7u§(false);
      
      private var §_-Wt§:BouncingTeleport;
      
      public function TeleThrower()
      {
         super();
         MIN_POWER = 15;
         MAX_POWER = 45;
         §_-a1R§.value = false;
         §_-N1N§ = false;
         §_-z1W§(new §_-d2j§());
         §_-B2r§ = true;
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         super.§_-z2Y§(param1);
         if(param1["teleportOnButton"])
         {
            this.§_-R1c§ = §_-7u§(param1["teleportOnButton"]);
         }
      }
      
      override protected function shoot() : void
      {
         if(this.§_-h1N§())
         {
            return;
         }
         §_-X1j§.§_-12n§.gameMaster.activeUnit.§_-vp§ = false;
         this.§_-Wt§ = new BouncingTeleport(owner,paramSet,shotParams.angle,§_-Yz§(shotParams.power));
         ControlSets.UNIT_CONTROL_SET.deactivate();
         ControlSets.WEAPON_CONTROL_SET.activate();
         owner.§_-d2T§(clip);
      }
      
      override protected function §_-V2X§(param1:Boolean = true) : void
      {
         super.§_-V2X§(param1);
         if(this.§_-R1c§.value)
         {
            §_-X1j§.§_-12n§.scene.gameInterface.§_-n2O§(Weapon.§_-N1r§);
         }
      }
      
      override public function detach() : void
      {
         super.detach();
         if(this.§_-Wt§)
         {
            this.§_-Wt§ = null;
         }
         ControlSets.WEAPON_CONTROL_SET.deactivate();
         ControlSets.UNIT_CONTROL_SET.activate();
         if(this.§_-R1c§.value && (!this.§_-Wt§ || this.§_-Wt§.disposed))
         {
            §_-X1j§.§_-12n§.scene.gameInterface.§_-Q2D§();
         }
      }
      
      public function weaponCharge() : void
      {
         if(Boolean(this.§_-Wt§) && Boolean(!this.§_-Wt§.disposed) && this.§_-R1c§.value)
         {
            this.§_-Wt§.prepareTeleport(§_-Ts§.value);
            §_-X1j§.§_-12n§.scene.gameInterface.§_-Q2D§();
            §_-X1j§.§_-12n§.gameMaster.activeUnit.§_-vp§ = false;
         }
      }
      
      override public function §_-h1N§() : Boolean
      {
         return Boolean(this.§_-Wt§) && !this.§_-Wt§.disposed;
      }
   }
}

