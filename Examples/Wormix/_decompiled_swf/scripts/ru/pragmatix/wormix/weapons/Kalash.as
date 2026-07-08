package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-V19§;
   import §_-A1K§.§_-TA§;
   import §_-l1K§.§_-W2f§;
   import §_-m0§.§_-X1F§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.IPooledAmmo;
   import starling.events.Event;
   
   public class Kalash extends Weapon
   {
      
      private var frames:§_-TA§ = new §_-TA§(0);
      
      private var §_-a2H§:§_-TA§ = new §_-TA§(0);
      
      private var angle:Number;
      
      private var §_-d18§:Number;
      
      private const §_-Bt§:§_-TA§ = new §_-TA§(4);
      
      private const §_-Z1D§:§_-TA§ = new §_-TA§(4);
      
      private const ANGLE_FLUCT:Number = 0.017453292519943295;
      
      public function Kalash()
      {
         super();
         MIN_POWER = MAX_POWER = 80;
         §_-z1W§(new §_-W2f§());
         §_-Y2U§ = new §_-V19§(130,MIN_POWER,0.2);
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
         super.detach();
         if(shooting)
         {
            this.§_-Ck§();
         }
      }
      
      override public function get bulletCount() : int
      {
         return this.§_-Bt§.value;
      }
      
      override protected function shoot() : void
      {
         if(shooting || !owner)
         {
            return;
         }
         ControlSets.UNIT_CONTROL_SET.deactivate();
         this.angle = this.§_-d18§ = shotParams.angle;
         shooting = true;
         this.frames.value = 0;
         this.§_-a2H§.value = 0;
         §_-K2b§();
      }
      
      override protected function onFrame(param1:Event) : void
      {
         var _loc2_:Number = NaN;
         var _loc3_:IPooledAmmo = null;
         if(owner != null)
         {
            ++this.frames.value;
            owner.graphics.hideHands(renderer.getHideHandsPattern());
            if(this.frames.value % this.§_-Z1D§.value == 0)
            {
               _loc2_ = §_-X1j§.randomSeed.§_-Fh§(-5,5) * this.ANGLE_FLUCT;
               this.angle += _loc2_;
               this.angle = Weapon(owner.weapon).rotateWeaponClip(this.angle);
               _loc3_ = AmmosFactory.getInstance().createObject(AmmosFactory.OBJECT_BULLET);
               _loc3_.initAmmo(AmmosFactory.BULLET_KALASH,owner,getBarrelPoint(this.angle),paramSet,this.angle,MIN_POWER);
               _loc3_.addTrail();
               ++this.§_-a2H§.value;
               §_-h2B§.play(§_-d27§.§_-1g§);
            }
            if(this.§_-a2H§.value == this.§_-Bt§.value)
            {
               rotateWeaponClip(this.§_-d18§);
               this.§_-Ck§();
            }
         }
      }
      
      protected function §_-Ck§() : void
      {
         §_-G1Z§();
         shooting = false;
         ControlSets.UNIT_CONTROL_SET.activate();
         if(shotsLeft.value == 0)
         {
            this.detach();
            nextTurnOnStable();
         }
         else
         {
            deactivate();
            this.activate();
         }
      }
   }
}

