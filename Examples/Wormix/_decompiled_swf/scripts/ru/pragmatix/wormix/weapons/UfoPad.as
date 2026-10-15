package ru.pragmatix.wormix.weapons
{
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.weapons.ammo.Ufo;
   import ru.pragmatix.wormix.weapons.interfaces.§_-y2Q§;
   
   public class UfoPad extends §_-Y1z§ implements §_-y2Q§
   {
      
      private const §_-T2B§:uint = 2500;
      
      public function UfoPad()
      {
         super(Ufo,this.§_-T2B§);
      }
      
      override public function attach(param1:§_-01J§) : void
      {
         super.attach(param1);
         §_-X1j§.§_-12n§.scene.gameInterface.§_-n2O§(Weapon.§_-N1r§);
      }
      
      override protected function shoot() : void
      {
         super.shoot();
         if(getProperty("canCarryUnits"))
         {
            Ufo(§_-02Z§).setCanCarryUnit();
         }
         §_-X1j§.§_-12n§.scene.gameInterface.§_-Q2D§();
      }
      
      override protected function §_-n2z§() : void
      {
         if(§_-02Z§)
         {
            Ufo(§_-02Z§).explode();
         }
      }
      
      public function weaponCharge() : void
      {
         if(§_-02Z§)
         {
            Ufo(§_-02Z§).attack();
         }
      }
      
      override public function weaponLeftPressed() : void
      {
         if(§_-02Z§)
         {
            Ufo(§_-02Z§).turnLeft();
         }
         super.weaponLeftPressed();
      }
      
      override public function weaponRightPressed() : void
      {
         if(§_-02Z§)
         {
            Ufo(§_-02Z§).turnRight();
         }
         super.weaponRightPressed();
      }
   }
}

