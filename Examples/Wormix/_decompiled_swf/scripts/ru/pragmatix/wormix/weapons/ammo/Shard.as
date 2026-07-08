package ru.pragmatix.wormix.weapons.ammo
{
   import §_-L1H§.§_-C3e§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-w2i§.§_-Zd§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class Shard extends §_-Qi§
   {
      
      private const DEFAULT_AMMO_CLASS_NAME:String = "DefaultShard";
      
      protected var paramSet:§_-C3e§;
      
      public function Shard(param1:§_-01J§, param2:int, param3:int, param4:Number, param5:uint, param6:§_-C3e§, param7:Number = 1, param8:Boolean = true)
      {
         this.paramSet = param6;
         super(param1,param2,param3,true);
         initialize(Pool.getPoint(param2,param3),param6.ammoClassName ? param6.ammoClassName : this.DEFAULT_AMMO_CLASS_NAME);
         this.§_-z1H§.scaleX = this.§_-z1H§.scaleY = param7 * §_-K1t§.§_-31c§;
         setSpeedVector(param4,param5);
         §_-Zd§.once(this,§_-L3Q§.§_-o13§,this.onCollision);
         §_-32§ = §_-uN§;
         if(param8)
         {
            Effects.attachParticleTrail(ParticleEffectType.AMMO_TRAIL,this);
         }
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      protected function onCollision(param1:Event) : void
      {
         this.explode(param1.data.other);
      }
      
      protected function explode(param1:§_-F3o§) : void
      {
         inflictDamage(param1,this.paramSet.damage.value,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value);
         var _loc2_:EffectType = this.paramSet.damageRadius.value > 75 ? EffectType.EXPLOSION_MEDIUM : EffectType.EXPLOSION_SMALL;
         Effects.createDragonbonesAnimationEffect(_loc2_,x,y,this.paramSet.damageRadius.value * 1.2,§_-d27§.§_-k4§([§_-d27§.CASSETE_EXPLOSION1,§_-d27§.CASSETE_EXPLOSION2,§_-d27§.CASSETE_EXPLOSION3]));
         disposed = true;
      }
   }
}

