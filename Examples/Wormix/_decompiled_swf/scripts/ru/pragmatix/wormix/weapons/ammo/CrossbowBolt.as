package ru.pragmatix.wormix.weapons.ammo
{
   import §_-L1H§.§_-C3e§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import starling.events.Event;
   
   public class CrossbowBolt extends Bolt
   {
      
      public function CrossbowBolt(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:uint, param5:Boolean = true)
      {
         super(param1,param2,param3,param4,param5);
      }
      
      override protected function onCollide(param1:Event) : void
      {
         super.onCollide(param1);
         Effects.createParticleEffect(ParticleEffectType.POISON,0.5,x,y);
      }
   }
}

