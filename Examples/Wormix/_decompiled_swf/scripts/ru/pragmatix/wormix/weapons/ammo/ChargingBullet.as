package ru.pragmatix.wormix.weapons.ammo
{
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-F1P§;
   import §_-L1H§.§_-C3e§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.Physics;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class ChargingBullet extends §_-Qi§
   {
      
      private static const bonusAttack:§_-TA§ = new §_-TA§(20);
      
      private static const DRILLS:uint = 5;
      
      protected var paramSet:§_-C3e§;
      
      protected var source:§_-F3o§;
      
      protected var target:§_-01J§ = null;
      
      protected var fxLifeTime:uint;
      
      private var drills:uint = 5;
      
      public function ChargingBullet(param1:§_-01J§, param2:§_-F3o§, param3:§_-C3e§, param4:Number, param5:uint)
      {
         super(param1,param2.x,param2.y,false,90,DamageType.SHOCK);
         this.paramSet = param3;
         this.source = param2;
         this.fxLifeTime = param5;
         §_-433§ = true;
         §_-62F§ = false;
         §_-92S§ = false;
         initialize(Pool.getPoint(param2.x,param2.y),param3.ammoClassName);
         setSpeedVector(param4,60);
         physParams.removeAllForces();
         if(clip)
         {
            clip.rotation = param4 * 180 / Math.PI;
         }
         Effects.lightningOnObjects(param2,this,30,10,6068463,8);
         addEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         §_-32§ = §_-uN§;
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      private function applyBonusOnTarget() : void
      {
         this.target.attributes.attack.§_-Y2z§(bonusAttack.value);
         var _loc1_:String = §_-F1P§.format(§_-s2a§.§_-K3t§(§_-s2a§.BONUS_ATTACK),{"value":bonusAttack.value});
         Effects.floatingText(_loc1_,Effects.TEXT_COLOR_RED,this.target.x + this.target.width * 0.5,this.target.y + this.target.height * 0.5,20,BitmapFonts.AD_LIB);
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         removeEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         §_-h2B§.play(§_-d27§.§_-Z1d§);
         if(this.target)
         {
            Effects.lightningOnObjects(this.source,this.target as §_-F3o§,this.fxLifeTime,10,6068463,8);
         }
         else
         {
            Effects.lightningOnObject(this.source,x,y,this.fxLifeTime,10,6068463,8);
         }
         this.source = null;
         this.target = null;
         super.defaultOnDisposed(param1);
      }
      
      protected function onCollide(param1:Event) : void
      {
         var _loc2_:Boolean = false;
         if(!disposed)
         {
            if(param1.data.other == null)
            {
               if(!owner.disposed && Physics.isGobjInRadius(owner as §_-F3o§,x,y,this.paramSet.explosionRadius.value,true))
               {
                  this.target = owner;
                  this.applyBonusOnTarget();
                  disposed = true;
               }
               else
               {
                  Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,this.x,this.y,this.paramSet.explosionRadius.value);
                  inflictDamage(param1.data.other,0,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value);
                  if(--this.drills == 0)
                  {
                     disposed = true;
                  }
               }
            }
            else if(param1.data.other is §_-01J§)
            {
               this.target = §_-01J§(param1.data.other);
               _loc2_ = this.target.squad.getTeamId() != owner.squad.getTeamId();
               if(_loc2_)
               {
                  inflictDamage(this.target as §_-F3o§,this.paramSet.damage.value,0,0);
               }
               else
               {
                  this.applyBonusOnTarget();
               }
               disposed = true;
            }
         }
      }
   }
}

