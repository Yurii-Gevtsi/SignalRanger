package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-V19§;
   import §_-A1K§.§_-TA§;
   import §_-l1K§.§_-W2f§;
   import §_-m0§.§_-X1F§;
   import §_-r1C§.§_-h2B§;
   import §_-u1m§.§_-e1l§;
   import dragonBones.animation.WorldClock;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.IPooledAmmo;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class FireThrower extends §_-e1l§
   {
      
      private const §_-A2j§:§_-TA§ = new §_-TA§(4);
      
      private const SHOT_DELAY:§_-TA§ = new §_-TA§(3);
      
      private const §_-D3§:§_-TA§ = new §_-TA§(4);
      
      private const ANGLE_FLUCT:§_-TA§ = new §_-TA§(1480);
      
      private var §_-k28§:§_-TA§ = new §_-TA§(0);
      
      private var bullets:§_-TA§ = new §_-TA§(12);
      
      private var §_-o2H§:Point;
      
      public function FireThrower()
      {
         super();
         §_-z1W§(new §_-W2f§());
         §_-Y2U§ = new §_-V19§(140,25,0.1,true);
      }
      
      override public function get bulletCount() : int
      {
         return this.bullets.value;
      }
      
      override public function attach(param1:§_-01J§) : void
      {
         this.§_-o2H§ = Pool.getPoint();
         super.attach(param1);
      }
      
      override public function detach() : void
      {
         if(owner != null)
         {
            owner.graphics.§_-i§();
         }
         super.detach();
         Pool.putPoint(this.§_-o2H§);
         this.§_-o2H§ = null;
      }
      
      override protected function onFrame(param1:Event) : void
      {
         var _loc2_:IPooledAmmo = null;
         var _loc3_:Number = NaN;
         if(owner != null)
         {
            ++frames.value;
            owner.graphics.hideHands(renderer.getHideHandsPattern());
            this.§_-o2H§.setTo(owner.x,owner.y);
            if(frames.value == this.§_-k28§.value)
            {
               _loc3_ = §_-X1j§.randomSeed.§_-Fh§(-5,5) * Math.PI / this.ANGLE_FLUCT.value;
               angle += _loc3_;
               this.§_-k28§.value = frames.value + this.SHOT_DELAY.value;
               if(§_-a2H§.value % this.§_-A2j§.value == 0)
               {
                  this.§_-k28§.value += this.§_-D3§.value;
               }
            }
            if(frames.value % 3 == 0)
            {
               _loc2_ = AmmosFactory.getInstance().createObject(AmmosFactory.OBJECT_BULLET);
               _loc2_.initAmmo(AmmosFactory.BULLET_FIRE,owner,this.§_-o2H§,paramSet,angle,15 + §_-a2H§.value * 2,paramSet.pushForce.value,false,false,false);
               ++§_-a2H§.value;
            }
            if(frames.value % 6 == 0)
            {
               §_-h2B§.play(§_-d27§.§_-11Y§);
            }
            if(§_-a2H§.value == this.bullets.value)
            {
               this.detach();
            }
         }
      }
      
      override public function weaponUpPressed() : void
      {
      }
      
      override public function weaponUpReleased() : void
      {
      }
      
      override public function weaponDownPressed() : void
      {
      }
      
      override public function weaponDownReleased() : void
      {
      }
   }
}

