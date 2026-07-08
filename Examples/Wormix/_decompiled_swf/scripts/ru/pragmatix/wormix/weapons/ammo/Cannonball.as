package ru.pragmatix.wormix.weapons.ammo
{
   import §_-L1H§.§_-C3e§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.ScreenShake;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import starling.events.Event;
   
   public class Cannonball extends GenericGrenade
   {
      
      public function Cannonball(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:uint)
      {
         super(param1,param1.x,param1.y,param2,param3,param4,0.3,180);
         §_-h2B§.play(§_-d27§.§_-o2v§);
         bounceSounds = [§_-d27§.VINTAGE_CANNONBALL_HIT_01,§_-d27§.VINTAGE_CANNONBALL_HIT_02,§_-d27§.VINTAGE_CANNONBALL_HIT_03];
      }
      
      override protected function onStopMoving(param1:Event) : void
      {
         super.onStopMoving(param1);
         inflictDamage(null,paramSet.damage.value,paramSet.damageRadius.value,paramSet.explosionRadius.value);
         disposed = true;
         Effects.explodeParticles(x,y,30);
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_LARGE,x,y,paramSet.damageRadius.value,§_-d27§.§_-m2k§);
         ScreenShake.go(paramSet.damage.value);
      }
   }
}

