package ru.pragmatix.wormix.weapons
{
   import §_-31W§.§_-r26§;
   import §_-L1H§.§_-b2K§;
   import §_-P2i§.§_-G3J§;
   import §_-l1K§.§_-cZ§;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.ammo.SuperBoar;
   import ru.pragmatix.wormix.weapons.interfaces.§_-b29§;
   import ru.pragmatix.wormix.weapons.interfaces.§_-jm§;
   import starling.events.Event;
   
   public class SuperBoarWeapon extends Weapon implements §_-jm§, §_-b29§
   {
      
      private var counter:§_-93W§;
      
      private var §_-A2X§:SuperBoar;
      
      private var §_-32z§:Boolean = true;
      
      private var §_-B2c§:Boolean;
      
      private var duration:int;
      
      private var §_-o1m§:int;
      
      private var §_-n1h§:int;
      
      private var frames:int;
      
      private var distance:uint;
      
      public function SuperBoarWeapon()
      {
         super();
         §_-z1W§(new §_-cZ§());
         this.§_-B2c§ = §_-X1j§.§_-12n§.battleStats.§_-B2c§();
      }
      
      public function jumpShoot(param1:Boolean) : void
      {
         var _loc2_:§_-b2K§ = null;
         if(!§_-X1j§.§_-12n§.gameMaster.§_-Rk§())
         {
            _loc2_ = new §_-b2K§();
            shootWithParams(_loc2_);
         }
      }
      
      override public function activate() : void
      {
         if(this.§_-A2X§ == null)
         {
            super.activate();
         }
         §_-mG§ = true;
      }
      
      override public function detach() : void
      {
         if(owner == null)
         {
            return;
         }
         owner.controlsMap.isActive = false;
         owner.controlsMap.§_-01B§();
         this.§_-o1m§ = 0;
         this.§_-n1h§ = 0;
         if(this.§_-A2X§ != null && !this.§_-32z§)
         {
            this.§_-p2V§(true);
         }
         super.detach();
      }
      
      override protected function shoot() : void
      {
         owner.§_-d2T§(clip);
         var _loc1_:§_-G3J§ = owner.controlsMap;
         _loc1_.isActive = true;
         _loc1_.§_-A2D§(true);
         _loc1_.§_-DV§(Command.Up.name,false);
         _loc1_.§_-DV§(Command.Down.name,false);
         _loc1_.§_-DV§(Command.Jump.name,false);
         _loc1_.§_-DV§(Command.BackJump.name,false);
         _loc1_.§_-DV§(Command.Shift.name,false);
         _loc1_.§_-DV§(Command.Bag.name,false);
         _loc1_.§_-DV§(Command.Alt.name,false);
         _loc1_.§_-DV§(Command.Shoot.name,false);
         §_-mG§ = false;
         this.frames = 0;
         this.distance = 0;
         this.duration = getProperty("duration").value;
         this.§_-A2X§ = new SuperBoar(owner,paramSet,String(getProperty("clipClassName")));
         this.§_-A2X§.setGroundBreaks(getProperty("groundBreaks") ? uint(getProperty("groundBreaks").value) : 0);
         this.§_-A2X§.setDamageBonus(getProperty("damagePerSupply") ? uint(getProperty("damagePerSupply").value) : 0);
         this.§_-A2X§.setFlyingSpeed(getProperty("flyingSpeed").value);
         this.counter = new §_-93W§(this.duration / 30,Effects.FUEL_COLOR,Effects.FUEL_SHADOW,-1,Effects.CRITICAL_TIME_COLOR,Effects.CRITICAL_TIME_SHADOW);
         this.counter.attach(this.§_-A2X§);
         this.§_-32z§ = false;
         ControlSets.UNIT_CONTROL_SET.deactivate();
         ControlSets.WEAPON_CONTROL_SET.activate();
         if(getProperty("cripple"))
         {
            this.§_-A2X§.setCripple();
         }
         §_-K2b§();
      }
      
      override protected function §_-GV§(param1:§_-b2K§) : void
      {
         super.§_-GV§(param1);
         renderer.deactivate(owner);
      }
      
      override protected function onFrame(param1:Event) : void
      {
         var _loc2_:uint = 0;
         var _loc3_:int = 0;
         if(this.§_-A2X§ != null && owner != null)
         {
            ++this.frames;
            this.counter.tick(1 / 30);
            if(this.§_-A2X§.disposed)
            {
               this.§_-p2V§(false);
            }
            else
            {
               _loc2_ = this.distance;
               this.distance += this.§_-A2X§.getFlyingSpeed();
               if(this.§_-B2c§ && uint(this.distance / Physics.METER / 5) > uint(_loc2_ / Physics.METER / 5))
               {
                  Effects.floatingText("5",Effects.MISC_COLOR,this.§_-A2X§.x,this.§_-A2X§.y);
               }
               _loc3_ = this.§_-o1m§ + this.§_-n1h§;
               if(_loc3_ < 0)
               {
                  this.§_-A2X§.rollLeft();
               }
               else if(_loc3_ > 0)
               {
                  this.§_-A2X§.rollRight();
               }
               if(this.frames == this.duration)
               {
                  this.§_-p2V§(true);
               }
            }
         }
      }
      
      private function §_-p2V§(param1:Boolean) : void
      {
         §_-G1Z§();
         ControlSets.WEAPON_CONTROL_SET.deactivate();
         ControlSets.UNIT_CONTROL_SET.activate();
         if(param1 && !this.§_-A2X§.disposed)
         {
            this.§_-A2X§.explode();
         }
         if(§_-X1j§.§_-12n§.gameMaster.activeSquad.isLocal())
         {
            §_-X1j§.§_-12n§.battleStats.increment(§_-r26§.§_-AH§,this.distance / Physics.METER);
         }
         §_-X1j§.§_-12n§.scene.sceneCamera.followObject(§_-X1j§.§_-12n§.gameMaster.activeUnit as §_-F3o§);
         if(!§_-X1j§.§_-12n§.gameMaster.§_-U2O§())
         {
            §_-X1j§.§_-12n§.scene.showArrowOnUnit(§_-X1j§.§_-12n§.gameMaster.activeUnit);
         }
         nextTurnOnStable();
         this.§_-A2X§ = null;
         this.§_-32z§ = true;
         §_-mG§ = true;
         this.detach();
      }
      
      public function weaponLeftPressed() : void
      {
         this.§_-o1m§ = -1;
      }
      
      public function weaponLeftReleased() : void
      {
         this.§_-o1m§ = 0;
      }
      
      public function weaponRightPressed() : void
      {
         this.§_-n1h§ = 1;
      }
      
      public function weaponRightReleased() : void
      {
         this.§_-n1h§ = 0;
      }
   }
}

