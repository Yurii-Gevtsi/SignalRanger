package ru.pragmatix.wormix.weapons.ammo
{
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import §_-b1F§.§_-G4§;
   import §_-g1P§.§_-L38§;
   import §_-gG§.§_-8u§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class AtomBomb extends §_-Qi§
   {
      
      private static const EXPLOSIONS_COUNT:uint = 3;
      
      private static const SIDE_EXPLOSION_DAMAGE:§_-TA§ = new §_-TA§(50);
      
      private static const STEP_FRAMES:uint = 8;
      
      private var paramSet:§_-C3e§;
      
      private var countFrame:uint = 0;
      
      private var countExplode:uint = 0;
      
      private var stepExplode:Number;
      
      private var posExplode:Point;
      
      private var unstableObject:§_-F3o§;
      
      public function AtomBomb(param1:§_-01J§, param2:§_-C3e§, param3:int, param4:int, param5:int, param6:int)
      {
         this.paramSet = param2;
         super(param1,param3,param4,false,§_-n18§,null,false);
         initialize(Pool.getPoint(param3,param4),param2.ammoClassName);
         this.§_-j2P§ = SortingLayer.AMMO_LAYER;
         §_-z1H§.scaleX = §_-z1H§.scaleY = §_-z1H§.scaleY * 0.74;
         setSpeedXY(param5,param6);
         Effects.attachParticleTrail(ParticleEffectType.AMMO_TRAIL,this);
         this.stepExplode = param2.explosionRadius.value / 2;
         §_-32§ = §_-uN§;
         §_-h2B§.play(§_-d27§.§_-D24§);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
         this.unstableObject = §_-X1j§.§_-12n§.scene.makeUnstabilizer();
      }
      
      override protected function defaultOnCollide(param1:Event) : void
      {
         var _loc3_:§_-01J§ = null;
         if(param1.data.other is §_-L38§)
         {
            return;
         }
         var _loc2_:AtomBomb = this;
         this.posExplode = Pool.getPoint(_loc2_.x,_loc2_.y);
         this.createExplosion(_loc2_.x,_loc2_.y,this.paramSet.damage.value);
         for each(_loc3_ in Physics.getUnitsInCircle(_loc2_.x,_loc2_.y,3 * this.paramSet.damageRadius.value))
         {
            _loc3_.buffs.place(new §_-8u§(owner));
         }
         §_-X1j§.§_-12n§.scene.addEventListener(§_-L3Q§.FRAME,this.onFrame);
         super.disposed = true;
      }
      
      override protected function defaultOnSink(param1:Event) : void
      {
         super.defaultOnSink(param1);
         this.disposeUnstableObject();
      }
      
      private function onFrame(param1:Event) : void
      {
         if(++this.countFrame >= STEP_FRAMES)
         {
            this.countFrame = 0;
            this.createExplosion(this.posExplode.x + this.stepExplode,this.posExplode.y,SIDE_EXPLOSION_DAMAGE.value);
            this.createExplosion(this.posExplode.x - this.stepExplode,this.posExplode.y,SIDE_EXPLOSION_DAMAGE.value);
            this.stepExplode += this.paramSet.explosionRadius.value / 2;
            if(++this.countExplode >= EXPLOSIONS_COUNT)
            {
               §_-X1j§.§_-12n§.scene.removeEventListener(§_-L3Q§.FRAME,this.onFrame);
               this.disposeUnstableObject();
               Pool.putPoint(this.posExplode);
               this.posExplode = null;
            }
         }
      }
      
      private function createExplosion(param1:int, param2:int, param3:uint) : void
      {
         if(!this.paramSet || !this.paramSet.damageRadius)
         {
            return;
         }
         §_-G4§.§_-Ee§(owner,param1,param2,null,param3,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value,true,false,DamageType.AIR);
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_LARGE,param1,param2,2 * this.paramSet.explosionRadius.value);
         §_-h2B§.play(§_-d27§.§_-k4§([§_-d27§.§_-33W§,§_-d27§.CASSETE_EXPLOSION1,§_-d27§.CASSETE_EXPLOSION2,§_-d27§.CASSETE_EXPLOSION3]));
      }
      
      private function disposeUnstableObject() : void
      {
         if(Boolean(this.unstableObject) && !this.unstableObject.disposed)
         {
            this.unstableObject.disposed = true;
            this.unstableObject = null;
         }
      }
      
      override public function set disposed(param1:Boolean) : void
      {
         physParams.disposed = param1;
         if(param1)
         {
            this.disposeUnstableObject();
         }
      }
   }
}

