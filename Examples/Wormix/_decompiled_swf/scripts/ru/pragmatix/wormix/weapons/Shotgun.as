package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-v1o§;
   import §_-A1K§.§_-TA§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-l1K§.§_-W2f§;
   import §_-r1C§.§_-h2B§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.weapons.ammo.Bullet;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   
   public class Shotgun extends Weapon
   {
      
      private static const §_-I3r§:Number = Math.PI / 72;
      
      private static const §_-o1Q§:uint = 5;
      
      private var bulletName:String;
      
      private var trailColor:uint;
      
      public function Shotgun()
      {
         super();
         MIN_POWER = MAX_POWER = 80;
         §_-z1W§(new §_-W2f§());
         §_-Y2U§ = new §_-v1o§(90,MIN_POWER);
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         super.§_-z2Y§(param1);
         shots.value = param1["shots"].value;
         this.bulletName = getProperty("bulletName") as String;
         this.trailColor = getProperty("trailColor") ? uint(getProperty("trailColor")) : Bullet.DEFAULT_TRAIL_COLOR;
      }
      
      override protected function shoot() : void
      {
         var _loc4_:Bullet = null;
         var _loc6_:Boolean = false;
         §_-h2B§.play(§_-d27§.§_-vS§);
         var _loc1_:int = §_-TA§(getProperty("quantityBullets")).value;
         var _loc2_:Point = getBarrelPoint(shotParams.angle);
         var _loc3_:Number = shotParams.angle - §_-I3r§ * 0.5 * (_loc1_ - 1);
         if(shotsLeft.value > 2)
         {
            §_-X1j§.§_-12n§.scene.dispatchEvent(new §_-H1K§(§_-A2V§.§_-A2e§,"shotgun shots hack"));
         }
         var _loc5_:int = 0;
         while(_loc5_ < _loc1_)
         {
            if(_loc5_ > 9)
            {
               §_-X1j§.§_-12n§.scene.dispatchEvent(new §_-H1K§(§_-A2V§.§_-A2e§,"shotgun bullets hack"));
            }
            _loc6_ = _loc5_ == 0;
            _loc4_ = AmmosFactory.getInstance().createObject(AmmosFactory.OBJECT_BULLET) as Bullet;
            _loc4_.initAmmoByBulletName(this.bulletName,owner,_loc2_,paramSet,_loc3_,MIN_POWER,0.05,true,false,_loc6_);
            _loc4_.setLifeTime(§_-o1Q§);
            _loc4_.addTrail(this.trailColor);
            _loc3_ += §_-I3r§;
            _loc5_++;
         }
         if(shotsLeft.value == 0)
         {
            detach();
            nextTurnOnStable();
         }
      }
   }
}

