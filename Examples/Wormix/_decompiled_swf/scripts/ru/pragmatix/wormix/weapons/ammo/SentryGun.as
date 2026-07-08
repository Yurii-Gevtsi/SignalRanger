package ru.pragmatix.wormix.weapons.ammo
{
   import §_-31N§.§_-42G§;
   import §_-91A§.§_-sH§;
   import §_-A1K§.§_-TA§;
   import §_-D3A§.§_-TZ§;
   import §_-L1H§.§_-C3e§;
   import §_-b1F§.§_-v2Q§;
   import §_-fo§.§_-r5§;
   import §_-hO§.§_-y2Z§;
   import §_-l1g§.§_-L3Q§;
   import §_-sQ§.§_-H3D§;
   import flash.display.DisplayObjectContainer;
   import flash.display.MovieClip;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.IPooledAmmo;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class SentryGun extends Mine implements §_-sH§
   {
      
      private static const TOTAL_HP:uint = 50;
      
      private static const MAX_BULLET:uint = 8;
      
      private static const ANGLE_FLUCT:Number = MathUtil.PI_ONE_180;
      
      private static const GUN_STEP_Y:int = -20;
      
      private var currentHp:§_-TA§;
      
      private var countBullet:uint = 0;
      
      private var gun:MovieClip;
      
      private var bulletParamSet:§_-C3e§;
      
      private var downAngleVal:Number = 1.0471975511965976;
      
      private var upAngleVal:Number = 5.235987755982988;
      
      public function SentryGun(param1:§_-01J§, param2:int, param3:int, param4:§_-C3e§)
      {
         super(param1,param2,param3,param4);
         mineDistance.value = 200;
         this.currentHp = new §_-TA§(TOTAL_HP);
         physParams.collidesWith = PhysParams.COLLIDES_WITH_ALL;
         physParams.collideType = CollideType.OTHER;
         §_-32§ = §_-8G§;
         §_-z1H§.rotation = 0;
         this.gun = §_-r5§.§_-tF§("Minigun");
         this.gun.y = GUN_STEP_Y;
         if(§_-z1H§ is DisplayObjectContainer)
         {
            DisplayObjectContainer(§_-z1H§).addChild(this.gun);
         }
         this.bulletParamSet = param4.clone();
         this.bulletParamSet.ammoClassName = "";
         §_-Y2I§.addEventListener(§_-42G§.§_-K2J§,this.onStable);
         §_-TZ§.addListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.SOT,this.nextTurn);
      }
      
      private function nextTurn(param1:Event) : void
      {
         this.countBullet = 0;
      }
      
      private function onStable(param1:Event) : void
      {
         §_-z1H§.rotation = 0;
      }
      
      override protected function findTarget() : §_-01J§
      {
         var _loc1_:IPooledAmmo = null;
         var _loc3_:int = 0;
         var _loc4_:int = 0;
         var _loc5_:Number = NaN;
         var _loc6_:§_-01J§ = null;
         if(owner.disposed)
         {
            disposed = true;
         }
         var _loc2_:int = y + GUN_STEP_Y;
         if(this.countBullet <= MAX_BULLET)
         {
            for each(_loc6_ in §_-X1j§.§_-12n§.gameMaster.§_-83I§(owner))
            {
               if(!_loc6_.disposed && §_-H3D§.distance(_loc6_.x - x,_loc6_.y - _loc2_) < mineDistance.value)
               {
                  _loc3_ = _loc6_.x;
                  _loc4_ = _loc6_.y;
                  _loc5_ = MathUtil.§_-02X§(x,_loc2_,_loc3_,_loc4_);
                  if(_loc5_ >= this.upAngleVal && _loc5_ <= this.downAngleVal)
                  {
                     this.gun.rotation = MathUtil.§_-02X§(x,_loc2_,_loc3_,_loc4_);
                     _loc1_ = AmmosFactory.getInstance().createObject(AmmosFactory.OBJECT_BULLET);
                     _loc1_.initAmmo(AmmosFactory.BULLET_SIMPLE,owner,Pool.getPoint(§_-z1H§.x,§_-z1H§.y + GUN_STEP_Y),this.bulletParamSet,Math.atan2(_loc4_ - _loc2_,_loc3_ - x) + §_-X1j§.randomSeed.§_-Fh§(-5,5) * ANGLE_FLUCT,100,1,false,false,false);
                     ++this.countBullet;
                  }
               }
            }
         }
         return null;
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_DRILL,x,y,55);
         §_-Y2I§.removeEventListener(§_-42G§.§_-K2J§,this.onStable);
         if(clip is DisplayObjectContainer)
         {
            DisplayObjectContainer(clip).removeChild(this.gun);
         }
         this.gun = null;
         §_-TZ§.removeListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.SOT,this.nextTurn);
         super.defaultOnDisposed(param1);
      }
      
      override public function defaultOnDamaged(param1:Event, param2:§_-v2Q§) : void
      {
         var _loc3_:uint = Math.ceil(param2.power);
         this.currentHp.value -= _loc3_;
         dispatchEvent(new §_-y2Z§(this,_loc3_));
         super.defaultOnDamaged(param1,param2);
         if(this.currentHp.value <= 0)
         {
            disposed = true;
         }
      }
      
      public function get hp() : int
      {
         return this.currentHp.value;
      }
      
      public function getNameHintInfoPanel() : String
      {
         return "sentry gun";
      }
      
      public function getSquadNumber() : uint
      {
         return owner.squad.getNumber();
      }
      
      public function get doesntCountTurns() : Boolean
      {
         return true;
      }
   }
}

