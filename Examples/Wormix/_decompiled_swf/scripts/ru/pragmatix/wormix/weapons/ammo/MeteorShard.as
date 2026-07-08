package ru.pragmatix.wormix.weapons.ammo
{
   import §_-A1K§.§_-TA§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class MeteorShard extends §_-Qi§
   {
      
      private static var DAMAGE:§_-TA§ = new §_-TA§(35);
      
      private static var RADIUS:§_-TA§ = new §_-TA§(50);
      
      private static var DAMAGE_RADIUS:§_-TA§ = new §_-TA§(70);
      
      private static var DAMAGE_RADIUS_INC:Number = 2.5;
      
      private var ammoClassName:String = "Meteor";
      
      public function MeteorShard(param1:§_-01J§, param2:int, param3:int, param4:Number, param5:uint)
      {
         super(param1,param2,param3,true,§_-Qi§.§_-n18§,DamageType.AIR);
         initialize(Pool.getPoint(param2,param3),this.ammoClassName);
         this.§_-z1H§.scaleX = this.§_-z1H§.scaleY = this.§_-z1H§.scaleY * 0.5;
         setSpeedVector(param4,param5);
         physParams.collidesWith = PhysParams.COLLIDES_WITH_GROUND;
         §_-32§ = §_-h2D§;
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      override protected function defaultOnCollide(param1:Event) : void
      {
         if(!disposed)
         {
            super.defaultOnCollide(param1);
            inflictDamage(param1.data.other,DAMAGE.value,DAMAGE_RADIUS.value,RADIUS.value);
            disposed = true;
            Effects.explodeParticles(x,y,6);
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,x,y,DAMAGE_RADIUS_INC * DAMAGE_RADIUS.value,§_-d27§.§_-S14§);
         }
      }
   }
}

