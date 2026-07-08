package ru.pragmatix.wormix.weapons.ammo
{
   import §_-L1H§.§_-C3e§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.Physics;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class RicochetingDisc extends §_-Qi§
   {
      
      private const DAMAGE_BOOST:uint = 20;
      
      private var hits:uint = 5;
      
      private var paramSet:§_-C3e§;
      
      private var power:uint;
      
      private var damage:uint;
      
      public function RicochetingDisc(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:uint)
      {
         super(param1,param1.x,param1.y);
         §_-U1o§ = true;
         §_-A3d§ = true;
         this.paramSet = param2;
         this.power = param4;
         this.damage = param2.damage.value;
         var _loc5_:Point = §_-fm§(param3,0);
         initialize(_loc5_,param2.ammoClassName);
         Pool.putPoint(_loc5_);
         this.§_-z1H§.rotation = param3;
         setSpeedVector(param3,param4);
         addEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         §_-32§ = §_-uN§;
         §_-h2B§.play(§_-d27§.§_-D1M§);
         Effects.attachTrail(this,7,6,14606046,16777215,1);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      private function onCollide(param1:Event) : void
      {
         var _loc2_:Number = NaN;
         if(param1.data.other != null || --this.hits <= 0)
         {
            this.explode(param1.data.other);
         }
         else
         {
            this.damage += this.DAMAGE_BOOST;
            _loc2_ = Physics.getNormal(x,y,angle) * 2 - angle;
            x = param1.data.x;
            y = param1.data.y;
            setSpeedVector(_loc2_,-speed);
            §_-Y2I§.stop();
            §_-h2B§.play(§_-d27§.§_-E2s§);
         }
      }
      
      private function explode(param1:§_-F3o§) : void
      {
         inflictDamage(param1,this.damage,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value);
         disposed = true;
         Effects.explodeParticles(x,y,16);
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,x,y,3 * this.paramSet.damageRadius.value,§_-d27§.§_-33W§);
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         removeEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         super.defaultOnDisposed(param1);
      }
   }
}

