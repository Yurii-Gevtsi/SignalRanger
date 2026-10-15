package ru.pragmatix.wormix.weapons
{
   import §_-21p§.§_-4w§;
   import §_-51M§.Dragon;
   import §_-51p§.§_-W2e§;
   import §_-A1K§.§_-TA§;
   import §_-CF§.§_-x2t§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-hO§.SupplyPickedUpEvent;
   import §_-l1g§.§_-L3Q§;
   import §_-zI§.*;
   import feathers.core.PopUpManager;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.ammo.GroundBreakerMissile;
   import ru.pragmatix.wormix.weapons.ammo.Helicopter;
   import ru.pragmatix.wormix.weapons.interfaces.§_-y2Q§;
   import starling.display.DisplayObject;
   
   public class Helipad extends §_-Y1z§ implements §_-y2Q§
   {
      
      private static const §_-p2§:uint = 1;
      
      private static const §_-oD§:uint = 200;
      
      private const §_-T2B§:uint = 3000;
      
      private const §_-l0§:§_-TA§ = new §_-TA§(4);
      
      private var §_-01m§:§_-TA§ = new §_-TA§(0);
      
      public function Helipad()
      {
         super(Helicopter,this.§_-T2B§);
         §_-YH§ = true;
      }
      
      override protected function shoot() : void
      {
         super.shoot();
         this.§_-01m§.value = 0;
         §_-X1j§.§_-12n§.scene.gameInterface.§_-n2O§(Weapon.§_-8r§);
      }
      
      override protected function §_-n2z§() : void
      {
         §_-X1j§.§_-12n§.scene.gameInterface.§_-Q2D§();
         §_-02Z§.physParams.removeForce(§_-m2y§);
         Helicopter(§_-02Z§).explodeOnCollide();
      }
      
      public function weaponCharge() : void
      {
         if(this.§_-01m§.value >= this.§_-l0§.value)
         {
            §_-A35§(true);
            §_-X1j§.§_-12n§.scene.gameInterface.§_-Q2D§();
         }
         else
         {
            var _loc1_:§_-TA§ = this.§_-01m§;
            var _loc2_:Number = _loc1_.value + 1;
            _loc1_.value = _loc2_;
            Helicopter(§_-02Z§).attack();
            if(this.§_-01m§.value >= this.§_-l0§.value)
            {
               §_-X1j§.§_-12n§.scene.gameInterface.§_-n2O§(Weapon.§_-v1e§);
            }
            if(this.§_-l0§.value > 4)
            {
               §_-X1j§.§_-12n§.scene.dispatchEvent(new §_-H1K§(§_-A2V§.§_-A2e§,"нelipad hack"));
            }
         }
      }
      
      override public function weaponLeftPressed() : void
      {
         Helicopter(§_-02Z§).turnLeft();
         super.weaponLeftPressed();
      }
      
      override public function weaponRightPressed() : void
      {
         Helicopter(§_-02Z§).turnRight();
         super.weaponRightPressed();
      }
      
      private function supplyPicked(param1:SupplyPickedUpEvent) : void
      {
         if(param1.§_-i1P§ == §_-02Z§ && param1.§_-m1G§ is §_-W2e§)
         {
            §_-l2D§ += §_-oD§;
            §_-X1j§.§_-12n§.scene.gameInterface.§_-n2O§(Weapon.§_-8r§);
         }
      }
      
      override public function attach(param1:§_-01J§) : void
      {
         §_-X1j§.§_-12n§.scene.addEventListener(§_-L3Q§.SUPPLY_PICKED_UP,this.supplyPicked);
         super.attach(param1);
         §_-X1j§.§_-12n§.scene.gameInterface.§_-n2O§(Weapon.§_-N1r§);
      }
      
      override public function detach() : void
      {
         §_-X1j§.§_-12n§.scene.gameInterface.§_-Q2D§();
         §_-X1j§.§_-12n§.scene.removeEventListener(§_-L3Q§.SUPPLY_PICKED_UP,this.supplyPicked);
         super.detach();
      }
   }
}

