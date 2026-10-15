package ru.pragmatix.wormix.weapons.ammo
{
   import §_-12a§.§_-f1J§;
   import §_-L1H§.§_-C3e§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-sQ§.§_-H3D§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class Boomerang extends §_-Qi§
   {
      
      private const BOUNCE_DAMAGE_COEFF:Number = 0.5;
      
      private var victim:§_-01J§ = null;
      
      private var paramSet:§_-C3e§;
      
      private var bouncing:Boolean;
      
      private var knockHat:Boolean;
      
      private var secondHitBonus:uint;
      
      public function Boomerang(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:uint, param5:Boolean, param6:uint, param7:Boolean)
      {
         super(param1,param1.x,param1.y,false);
         §_-U1o§ = true;
         §_-A3d§ = true;
         this.paramSet = param2;
         this.bouncing = param5;
         this.knockHat = param7;
         this.secondHitBonus = param6;
         §_-h2B§.play(§_-d27§.§_-B3x§);
         var _loc8_:Point = §_-fm§(param3,0);
         initialize(_loc8_,param2.ammoClassName);
         Pool.putPoint(_loc8_);
         this.clip.rotation = param3;
         physParams.addForce(Force.WIND);
         setSpeedVector(param3,param4);
         addEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         §_-32§ = §_-h2D§;
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      private function onCollide(param1:Event) : void
      {
         var _loc2_:uint = 0;
         if(§_-22Y§)
         {
            if(param1.data.other == owner && !§_-X1j§.§_-12n§.gameMaster.§_-U2O§())
            {
               dispatchEvent(new Event(§_-L3Q§.BOOMERANG_CAUGHT));
               disposed = true;
            }
            else if(param1.data.other == null)
            {
               inflictDamage(param1.data.other,this.paramSet.damage.value * this.BOUNCE_DAMAGE_COEFF,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value);
               Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_DRILL,x,y,4 * this.paramSet.explosionRadius.value,§_-d27§.§_-81p§);
               disposed = true;
            }
         }
         else
         {
            if(param1.data.other != null && param1.data.other is §_-01J§)
            {
               this.victim = §_-01J§(param1.data.other);
            }
            _loc2_ = this.paramSet.damage.value + (Boolean(this.victim) && !this.bouncing ? this.secondHitBonus : 0);
            inflictDamage(param1.data.other,_loc2_,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value);
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_SMALL,x,y,4 * this.paramSet.explosionRadius.value,§_-d27§.§_-81p§);
            if(this.bouncing)
            {
               setSpeedVector(-MathUtil.PI_HALF - §_-H3D§.sign(speedX) * speed / 2 * MathUtil.PI_ONE_180,speed / 3 * 2);
               §_-22Y§ = true;
               if(Boolean(this.knockHat) && Boolean(this.victim) && this.victim.attributes.hp.§_-oJ§() > 0)
               {
                  this.victim.buffs.place(new §_-f1J§());
               }
            }
            else
            {
               if(Boolean(this.knockHat) && Boolean(this.victim) && this.victim.buffs.hasBuff(§_-f1J§.NAME))
               {
                  this.victim.§_-G15§(this.victim.direction * -2,-5);
               }
               disposed = true;
            }
            §_-Y2I§.stop();
         }
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         super.defaultOnDisposed(param1);
         removeEventListener(§_-L3Q§.§_-o13§,this.onCollide);
      }
   }
}

