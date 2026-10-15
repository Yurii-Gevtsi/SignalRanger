package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-V19§;
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-b2K§;
   import §_-Vg§.*;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-l1K§.§_-W2f§;
   import §_-m0§.§_-X1F§;
   import §_-r1C§.§_-h2B§;
   import §_-x2Q§.§_-dU§;
   import §_-zI§.§_-X1q§;
   import feathers.themes.FontColor;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.weapons.ammo.Bullet;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.IPooledAmmo;
   import starling.events.Event;
   import starling.utils.Color;
   
   public class Minigun extends Weapon
   {
      
      protected var §_-A1Q§:§_-TA§ = new §_-TA§(2);
      
      private var frames:§_-TA§ = new §_-TA§(0);
      
      private var §_-a2H§:§_-TA§ = new §_-TA§(0);
      
      private var angle:Number;
      
      private var §_-k2J§:§_-TA§ = new §_-TA§(0);
      
      private var angleFluct:Number = 0.017453292519943295;
      
      private var bulletName:String;
      
      private var trailColor:uint;
      
      public function Minigun()
      {
         super();
         MIN_POWER = MAX_POWER = 100;
         §_-z1W§(new §_-W2f§());
         §_-Y2U§ = new §_-V19§(120,MIN_POWER);
         §_-V19§(§_-Y2U§).maxDistance = 400;
      }
      
      override public function get bulletCount() : int
      {
         return this.§_-k2J§.value;
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         super.§_-z2Y§(param1);
         this.§_-k2J§.value = param1["quantityBullets"].value;
         this.angleFluct = param1["angleFluct"];
         this.bulletName = param1["bulletName"];
         this.trailColor = param1["trailColor"] ? uint(param1["trailColor"]) : Bullet.DEFAULT_TRAIL_COLOR;
      }
      
      override public function detach() : void
      {
         if(Boolean(owner) && Boolean(owner.graphics))
         {
            owner.graphics.§_-i§();
         }
         super.detach();
         if(shooting)
         {
            this.§_-Ck§();
         }
      }
      
      override protected function shoot() : void
      {
         shooting = true;
         ControlSets.UNIT_CONTROL_SET.deactivate();
         §_-K2b§();
         this.angle = shotParams.angle;
         this.frames.value = 0;
         this.§_-a2H§.value = 0;
      }
      
      override public function shootWithParams(param1:§_-b2K§) : void
      {
         super.shootWithParams(param1);
         deactivate();
         if(owner)
         {
            owner.graphics.§_-m14§(false);
         }
      }
      
      override protected function onFrame(param1:Event) : void
      {
         if(owner != null)
         {
            ++this.frames.value;
            owner.graphics.hideHands(renderer.getHideHandsPattern());
            if(this.§_-a2H§.value == this.§_-k2J§.value)
            {
               this.detach();
            }
            else if(this.frames.value % this.§_-A1Q§.value == 0)
            {
               this.angle += §_-X1j§.randomSeed.§_-Fh§(-5,5) * this.angleFluct;
               Weapon(owner.weapon).rotateWeaponClip(this.angle);
               this.§_-G11§(this.angle);
               var _loc2_:§_-TA§ = this.§_-a2H§;
               var _loc3_:Number = _loc2_.value + 1;
               _loc2_.value = _loc3_;
               if(this.§_-a2H§.value > 20)
               {
                  §_-X1j§.§_-12n§.scene.dispatchEvent(new §_-H1K§(§_-A2V§.§_-A2e§,"minigun hack"));
               }
            }
         }
      }
      
      protected function §_-Ck§() : void
      {
         shooting = false;
         ControlSets.UNIT_CONTROL_SET.activate();
         §_-G1Z§();
         nextTurnOnStable();
      }
      
      protected function §_-G11§(param1:Number) : void
      {
         var _loc2_:IPooledAmmo = AmmosFactory.getInstance().createObject(AmmosFactory.OBJECT_BULLET);
         _loc2_.initAmmoByBulletName(this.bulletName,owner,getBarrelPoint(param1),paramSet,param1,MIN_POWER,1);
         _loc2_.addTrail(this.trailColor);
         §_-h2B§.play(§_-d27§.§_-73O§);
      }
   }
}

