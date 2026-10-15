package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-V19§;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import §_-l1K§.§_-sV§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.weapons.ammo.Bullet;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import starling.utils.Pool;
   
   public class SniperRifle extends Weapon
   {
      
      public static const §_-A34§:uint = 10;
      
      public static const §_-12§:uint = 10;
      
      private var armorReduction:uint = 0;
      
      private var antidodge:Boolean = false;
      
      public function SniperRifle()
      {
         super();
         MIN_POWER = MAX_POWER = 100;
         §_-Y2U§ = new §_-V19§(110,MIN_POWER,0.8,false,0,false,true);
         §_-V19§(§_-Y2U§).minDistance = 400;
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         §_-z1W§(new §_-sV§(§_-TA§(param1["rayLength"]).value,param1["angleFluct"],§_-A34§,§_-12§,param1["colorLaser"]));
         if(param1["armorReduction"])
         {
            this.armorReduction = §_-TA§(param1["armorReduction"]).value;
         }
         if(param1["antidodge"])
         {
            this.antidodge = §_-7u§(param1["antidodge"]).value;
         }
         super.§_-z2Y§(param1);
      }
      
      override public function §_-d1P§(param1:§_-C3e§) : void
      {
         super.§_-d1P§(param1);
      }
      
      override public function attach(param1:§_-01J§) : void
      {
         super.attach(param1);
      }
      
      override protected function shoot() : void
      {
         var _loc1_:Bullet = AmmosFactory.getInstance().createObject(AmmosFactory.OBJECT_BULLET) as Bullet;
         _loc1_.initAmmo(AmmosFactory.BULLET_PIERCING,owner,Pool.getPoint(owner.x,owner.y),paramSet.clone(),shotParams.angle,MIN_POWER);
         _loc1_.addTrail();
         if(this.armorReduction > 0)
         {
            _loc1_.setArmorReductionPc(this.armorReduction);
         }
         if(this.antidodge)
         {
            _loc1_.setAntidodge();
         }
         detach();
         nextTurnOnStable();
         §_-h2B§.play(§_-d27§.§_-D1A§);
      }
   }
}

