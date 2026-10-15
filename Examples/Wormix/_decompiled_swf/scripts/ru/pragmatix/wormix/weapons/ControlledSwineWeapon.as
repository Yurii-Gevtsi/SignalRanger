package ru.pragmatix.wormix.weapons
{
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.weapons.ammo.RunningSwine;
   import ru.pragmatix.wormix.weapons.interfaces.§_-jm§;
   import ru.pragmatix.wormix.weapons.interfaces.§_-o2d§;
   import starling.events.Event;
   
   public class ControlledSwineWeapon extends SwineWeapon implements §_-jm§, §_-o2d§
   {
      
      public function ControlledSwineWeapon()
      {
         super();
      }
      
      override protected function shoot() : void
      {
         super.shoot();
         ControlSets.UNIT_CONTROL_SET.deactivate();
      }
      
      override protected function §_-a1c§(param1:Boolean) : void
      {
         super.§_-a1c§(param1);
         ControlSets.UNIT_CONTROL_SET.activate();
      }
      
      public function weaponLeftPressed() : void
      {
         RunningSwine(swine).run(-1);
      }
      
      public function weaponLeftReleased() : void
      {
      }
      
      public function weaponRightPressed() : void
      {
         RunningSwine(swine).run(1);
      }
      
      public function weaponRightReleased() : void
      {
      }
      
      public function weaponUpPressed() : void
      {
         RunningSwine(swine).jump();
      }
      
      public function weaponUpReleased() : void
      {
      }
      
      public function weaponDownPressed() : void
      {
      }
      
      public function weaponDownReleased() : void
      {
      }
   }
}

