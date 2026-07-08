package ru.pragmatix.wormix.weapons.ammo
{
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-r1C§.§_-t1R§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.Force;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class DrillMissile extends §_-Qi§
   {
      
      private static const DEFAULT_DRILL_LENGTH:§_-TA§ = new §_-TA§(20);
      
      private static const DEFAULT_DRILL_RADIUS:§_-TA§ = new §_-TA§(10);
      
      private var counter:uint = 0;
      
      private var drillParamSet:§_-C3e§;
      
      private var drillLength:uint = DEFAULT_DRILL_LENGTH.value;
      
      private var drillRadius:uint = DEFAULT_DRILL_RADIUS.value;
      
      private var groundHitSound:§_-t1R§ = §_-d27§.§_-S3§;
      
      private var explosionSound:§_-t1R§ = §_-d27§.§_-33W§;
      
      public function DrillMissile(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:uint)
      {
         super(param1,param1.x,param1.y,false);
         §_-U1o§ = true;
         §_-A3d§ = true;
         §_-h2B§.play(§_-d27§.§_-I1P§);
         var _loc5_:Point = §_-fm§(param3,0);
         initialize(_loc5_,param2.ammoClassName);
         Pool.putPoint(_loc5_);
         physParams.addForce(Force.WIND);
         physParams.optimizedMovement = false;
         physParams.ignoresTerrain = true;
         this.drillParamSet = param2;
         setSpeedVector(param3,param4);
         addEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         §_-32§ = §_-uN§;
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         super.defaultOnDisposed(param1);
         removeEventListener(§_-L3Q§.§_-o13§,this.onCollide);
      }
      
      private function onCollide(param1:Event) : void
      {
         var _loc3_:int = 0;
         ++this.counter;
         if(this.counter == 1 && !owner.disposed)
         {
            owner.stable = false;
         }
         var _loc2_:§_-F3o§ = param1.data.other;
         _loc3_ = this.drillParamSet.explosionRadius.value;
         if(this.counter == this.drillLength || _loc2_ is §_-01J§ || §_-X1j§.§_-12n§.scene.hitTestPermanent(param1.data.x,param1.data.y))
         {
            inflictDamage(_loc2_,this.drillParamSet.damage.value,this.drillParamSet.damageRadius.value,_loc3_);
            disposed = true;
            Effects.explodeParticles(x,y,16);
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_SMALL,x,y,_loc3_,this.explosionSound);
         }
         else
         {
            §_-h2B§.play(this.groundHitSound);
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_DRILL,x,y,_loc3_ * 2);
            inflictDamage(_loc2_,10,this.drillRadius + 12,this.drillRadius,false);
            Effects.explodeParticles(x,y,4);
         }
      }
      
      public function setDrillParams(param1:uint, param2:uint) : void
      {
         this.drillLength = param1;
         this.drillRadius = param2;
      }
      
      public function setSounds(param1:§_-t1R§, param2:§_-t1R§) : void
      {
         this.groundHitSound = param1;
         this.explosionSound = param2;
      }
   }
}

