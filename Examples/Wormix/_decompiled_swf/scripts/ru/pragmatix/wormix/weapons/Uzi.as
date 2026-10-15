package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-V19§;
   import §_-A1K§.§_-TA§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-l1K§.§_-W2f§;
   import §_-r1C§.§_-h2B§;
   import §_-u1m§.§_-e1l§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.weapons.ammo.Bullet;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.IPooledAmmo;
   import starling.events.Event;
   
   public class Uzi extends §_-e1l§
   {
      
      private static const §_-Bt§:§_-TA§ = new §_-TA§(16);
      
      private static const §_-A2j§:§_-TA§ = new §_-TA§(4);
      
      private static const SHOT_DELAY:§_-TA§ = new §_-TA§(3);
      
      private static const §_-D3§:§_-TA§ = new §_-TA§(4);
      
      private static const ANGLE_FLUCT:Number = Math.PI / 1480;
      
      private static const §_-03M§:int = 100;
      
      private var §_-k28§:§_-TA§ = new §_-TA§(0);
      
      private var trailColor:uint;
      
      private var §_-lZ§:Number;
      
      public function Uzi()
      {
         super();
         MIN_POWER = MAX_POWER = 150;
         §_-z1W§(new §_-W2f§());
      }
      
      override protected function shoot() : void
      {
         this.§_-k28§.value = 1;
         super.shoot();
         this.§_-lZ§ = §_-03M§ + owner.weapon.§_-R1d§.length;
         owner.graphics.hideHands(renderer.getHideHandsPattern());
         §_-Y2U§ = new §_-V19§(130,MIN_POWER,0.2);
      }
      
      override public function get bulletCount() : int
      {
         return §_-Bt§.value;
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         super.§_-z2Y§(param1);
         this.trailColor = getProperty("trailColor") ? uint(getProperty("trailColor")) : Bullet.DEFAULT_TRAIL_COLOR;
      }
      
      override protected function onFrame(param1:Event) : void
      {
         var _loc2_:Number = NaN;
         var _loc3_:IPooledAmmo = null;
         if(owner != null)
         {
            ++frames.value;
            if(frames.value == this.§_-k28§.value)
            {
               _loc2_ = §_-X1j§.randomSeed.§_-Fh§(-5,5) * ANGLE_FLUCT;
               angle += _loc2_;
               angle = Weapon(owner.weapon).rotateWeaponClip(angle);
               _loc3_ = AmmosFactory.getInstance().createObject(AmmosFactory.OBJECT_BULLET);
               _loc3_.initAmmo(AmmosFactory.BULLET_SIMPLE,owner,getBarrelPoint(angle),paramSet,angle,MIN_POWER);
               _loc3_.addTrail(this.trailColor);
               ++§_-a2H§.value;
               §_-h2B§.play(§_-d27§.§_-1g§);
               this.§_-k28§.value = frames.value + SHOT_DELAY.value;
               if(§_-a2H§.value % §_-A2j§.value == 0)
               {
                  this.§_-k28§.value += §_-D3§.value;
               }
            }
            super.onFrame(param1);
            if(§_-Bt§.value > 16)
            {
               §_-X1j§.§_-12n§.scene.dispatchEvent(new §_-H1K§(§_-A2V§.§_-A2e§,"uzi hack"));
            }
            if(§_-a2H§.value == §_-Bt§.value)
            {
               detach();
            }
         }
      }
      
      override public function activate() : void
      {
         if(!shooting)
         {
            super.activate();
         }
         else
         {
            §_-s2E§();
         }
      }
      
      override protected function updateAngle(param1:Number) : void
      {
         if(param1 != 0)
         {
            super.updateAngle(param1);
         }
         else
         {
            §_-s2E§();
         }
      }
   }
}

