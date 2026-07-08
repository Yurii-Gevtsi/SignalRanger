package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-w1O§;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-D3A§.§_-TZ§;
   import §_-L1H§.§_-b2K§;
   import §_-l1K§.§_-d2j§;
   import §_-l1g§.§_-L3Q§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-4V§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.model.IPhysicModel;
   import ru.pragmatix.wormix.utils.§_-Py§;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.simple.BouncingGrenade;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.simple.StuffedBouncingGrenade;
   import ru.pragmatix.wormix.weapons.interfaces.§_-y2Q§;
   import starling.display.DisplayObjectContainer;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class Grenade extends Weapon
   {
      
      private const §_-w1V§:Number = 0.5;
      
      private const §_-zW§:§_-TA§ = new §_-TA§(30);
      
      private var §_-T1V§:§_-TA§ = new §_-TA§(5);
      
      private var §_-y9§:§_-TA§ = new §_-TA§(1);
      
      private var hasTimer:§_-7u§ = new §_-7u§(true);
      
      protected var clock:§_-TA§ = new §_-TA§(90);
      
      protected var ammo:BouncingGrenade;
      
      public function Grenade()
      {
         super(Weapon.§_-vZ§);
         MIN_POWER = 5;
         MAX_POWER = 45;
         §_-Y2U§ = new §_-w1O§(MIN_POWER,MAX_POWER,50);
         §_-z1W§(new §_-d2j§());
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         super.§_-z2Y§(param1);
         if(param1["hasTimer"])
         {
            this.hasTimer = §_-7u§(param1["hasTimer"]);
         }
      }
      
      override public function attach(param1:§_-01J§) : void
      {
         super.attach(param1);
         this.§_-71w§(this.hasTimer.value ? this.§_-T1V§.value : 0);
      }
      
      override public function §_-X1c§(param1:uint) : void
      {
         this.§_-71w§(this.hasTimer.value ? int(param1) : 0);
      }
      
      public function §_-71w§(param1:int) : void
      {
         this.clock.value = this.§_-zW§.value * param1;
         shotParams.timer = param1;
      }
      
      override protected function shoot() : void
      {
         var _loc1_:Class = getProperty("ammoClass") as Class;
         this.ammo = new _loc1_(owner,paramSet,this.clock.value,shotParams.angle,§_-Yz§(shotParams.power),this.§_-w1V§,getProperty("blink"));
         if(getProperty("multiBurst"))
         {
            StuffedBouncingGrenade(this.ammo).setMiniExplosions(getProperty("multiBurst").value);
         }
         if(getProperty("cripple"))
         {
            StuffedBouncingGrenade(this.ammo).setCripple();
         }
         if(getProperty("explodeOnCollide"))
         {
            §_-B2r§ = true;
            this.ammo.explodeOnCollideWithEnemy();
         }
         if(getProperty("explodeOnStopMoving"))
         {
            §_-B2r§ = true;
            this.ammo.explodeOnStopMoving();
         }
         if(this.clock.value <= 0)
         {
            this.ammo.removeTimer();
         }
         this.detach();
         nextTurnOnStable();
      }
      
      override public function shootWithParams(param1:§_-b2K§) : void
      {
         this.§_-71w§(param1.timer);
         super.shootWithParams(param1);
      }
      
      override public function detach() : void
      {
         if(this.ammo)
         {
            §_-TZ§.removeListener(this.ammo,§_-L3Q§.DISPOSED,this.onAmmoDisposed);
            this.ammo = null;
         }
         if(this is §_-y2Q§)
         {
            ControlSets.WEAPON_CONTROL_SET.deactivate();
            §_-X1j§.§_-12n§.scene.gameInterface.§_-Q2D§();
         }
         super.detach();
      }
      
      protected function §_-BB§() : void
      {
         if(shotParams.§_-R18§)
         {
            this.§_-71w§(this.§_-y9§.value);
         }
      }
      
      protected function §_-Ws§() : void
      {
         §_-YH§ = this is §_-y2Q§;
         if(§_-YH§)
         {
            §_-TZ§.addListener(this.ammo,§_-L3Q§.DISPOSED,this.onAmmoDisposed,§_-4V§.§_-DA§);
            ControlSets.WEAPON_CONTROL_SET.activate();
            §_-X1j§.§_-12n§.scene.gameInterface.§_-n2O§(Weapon.§_-v1e§);
            owner.§_-d2T§(clip);
            shooting = true;
            active = true;
         }
         else
         {
            this.detach();
         }
      }
      
      public function weaponCharge() : void
      {
         if(Boolean(this.ammo) && !this.ammo.disposed)
         {
            this.ammo.explode();
         }
      }
      
      private function onAmmoDisposed(param1:Event) : void
      {
         this.detach();
      }
   }
}

