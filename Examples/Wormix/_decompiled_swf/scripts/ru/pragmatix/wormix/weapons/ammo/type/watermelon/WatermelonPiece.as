package ru.pragmatix.wormix.weapons.ammo.type.watermelon
{
   import §_-L1H§.§_-C3e§;
   import §_-l1g§.§_-L3Q§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class WatermelonPiece extends §_-Qi§
   {
      
      protected var paramSet:§_-C3e§;
      
      public function WatermelonPiece(param1:§_-01J§, param2:int, param3:int, param4:Number, param5:uint, param6:§_-C3e§)
      {
         super(param1,param2,param3,true);
         this.paramSet = param6;
         initialize(Pool.getPoint(param2,param3),param6.ammoClassName,true);
         §_-z1H§.scaleX = §_-z1H§.scaleY = 1;
         setSpeedVector(param4,param5 * 2);
         physParams.collidesWith = PhysParams.COLLIDES_WITH_GROUND;
         addEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         §_-32§ = §_-h2D§;
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      protected function onCollide(param1:Event) : void
      {
         removeEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         this.inflictDamage(param1.data.other,this.paramSet.damage.value,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value);
         this.disposed = true;
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,this.x,this.y,3 * this.paramSet.damageRadius.value,§_-d27§.§_-S14§);
      }
   }
}

