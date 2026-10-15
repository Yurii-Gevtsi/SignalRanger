package ru.pragmatix.wormix.weapons
{
   import §_-A1K§.§_-TA§;
   import §_-DJ§.§_-J2l§;
   import §_-L1H§.§_-b2K§;
   import §_-l1K§.§_-cZ§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.weapons.ammo.RunningSwine;
   import ru.pragmatix.wormix.weapons.interfaces.§_-b29§;
   import ru.pragmatix.wormix.weapons.interfaces.§_-y2Q§;
   import starling.events.Event;
   
   public class SwineWeapon extends Weapon implements §_-y2Q§, §_-b29§
   {
      
      protected static const §_-T2B§:§_-TA§ = new §_-TA§(300);
      
      protected var swine:RunningSwine;
      
      protected var frames:§_-TA§;
      
      private var counter:§_-93W§;
      
      protected var maxFuel:int;
      
      public function SwineWeapon()
      {
         super();
         §_-YH§ = true;
         §_-z1W§(new §_-cZ§());
      }
      
      public function jumpShoot(param1:Boolean) : void
      {
         if(!§_-X1j§.§_-12n§.gameMaster.§_-Rk§() && this.swine == null)
         {
            shootWithParams(new §_-b2K§());
         }
      }
      
      override public function activate() : void
      {
         if(this.swine == null)
         {
            super.activate();
         }
      }
      
      override public function attach(param1:§_-01J§) : void
      {
         super.attach(param1);
         this.maxFuel = getProperty("maxFuel") ? int(getProperty("maxFuel").value) : §_-T2B§.value;
         §_-X1j§.§_-12n§.scene.gameInterface.§_-n2O§(Weapon.§_-N1r§);
      }
      
      override public function detach() : void
      {
         if(owner == null)
         {
            return;
         }
         if(this.swine != null && !this.swine.disposed)
         {
            this.§_-a1c§(true);
         }
         super.detach();
      }
      
      override protected function shoot() : void
      {
         owner.§_-d2T§(clip);
         this.frames = new §_-TA§(0);
         var _loc1_:Class = getProperty("ammoClassName") as Class;
         this.swine = new _loc1_(owner,paramSet,paramSetEx);
         shooting = true;
         this.counter = new §_-93W§(this.maxFuel / 30,Effects.FUEL_COLOR,Effects.FUEL_SHADOW,-1,Effects.CRITICAL_TIME_COLOR,Effects.CRITICAL_TIME_SHADOW);
         this.counter.attach(this.swine);
         ControlSets.WEAPON_CONTROL_SET.activate();
         §_-K2b§();
         §_-X1j§.§_-12n§.scene.gameInterface.§_-n2O§(Weapon.§_-v1e§);
      }
      
      override protected function §_-GV§(param1:§_-b2K§) : void
      {
         super.§_-GV§(param1);
         renderer.deactivate(owner);
      }
      
      override protected function onFrame(param1:Event) : void
      {
         if(this.swine != null && owner != null)
         {
            var _loc2_:§_-TA§ = this.frames;
            var _loc3_:Number = _loc2_.value + 1;
            _loc2_.value = _loc3_;
            this.counter.tick(1 / 30);
            if(this.swine.disposed)
            {
               this.§_-a1c§(false);
               this.detach();
            }
            else if(this.frames.value == this.maxFuel)
            {
               this.§_-a1c§(true);
               this.detach();
            }
         }
      }
      
      public function weaponCharge() : void
      {
         this.§_-a1c§(true);
         this.detach();
      }
      
      protected function §_-a1c§(param1:Boolean) : void
      {
         shooting = false;
         §_-G1Z§();
         ControlSets.WEAPON_CONTROL_SET.deactivate();
         if(param1)
         {
            this.swine.explode();
         }
         this.swine = null;
         nextTurnOnStable();
         §_-X1j§.§_-12n§.scene.gameInterface.§_-Q2D§();
      }
   }
}

