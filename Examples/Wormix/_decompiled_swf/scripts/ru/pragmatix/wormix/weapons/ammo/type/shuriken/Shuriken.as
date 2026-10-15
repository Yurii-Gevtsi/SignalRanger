package ru.pragmatix.wormix.weapons.ammo.type.shuriken
{
   import §_-L1H§.§_-C3e§;
   import §_-gG§.§_-i1B§;
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
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class Shuriken extends §_-Qi§
   {
      
      public static const attackSounds:Array = [§_-d27§.SHURIKEN_ATTACK_01,§_-d27§.SHURIKEN_ATTACK_02,§_-d27§.SHURIKEN_ATTACK_03,§_-d27§.SHURIKEN_ATTACK_04];
      
      public static const groundHitSounds:Array = [§_-d27§.SHURIKEN_HITGROUND_01,§_-d27§.SHURIKEN_HITGROUND_02,§_-d27§.SHURIKEN_HITGROUND_03,§_-d27§.SHURIKEN_HITGROUND_04];
      
      public static const wormHitSounds:Array = [§_-d27§.SHURIKEN_HITWORM_01,§_-d27§.SHURIKEN_HITWORM_02,§_-d27§.SHURIKEN_HITWORM_03,§_-d27§.SHURIKEN_HITWORM_04];
      
      protected var paramSet:§_-C3e§;
      
      public var selfHit:Boolean = true;
      
      public function Shuriken(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:uint, param5:Boolean = true)
      {
         super(param1,param1.x,param1.y,false);
         this.paramSet = param2;
         var _loc6_:Point = §_-fm§(param3,5);
         initialize(_loc6_,param2.ammoClassName);
         Pool.putPoint(_loc6_);
         if(armature)
         {
            armature.playAnimation("play");
         }
         setSpeedVector(param3,param4);
         §_-z1H§.rotation = param3;
         §_-h2B§.play(§_-d27§.§_-k4§(attackSounds));
         addEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         §_-32§ = §_-8G§;
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
         if(param5)
         {
            Effects.attachShadowTrail(this,param2.ammoClassName);
         }
      }
      
      protected function onCollide(param1:Event) : void
      {
         if(!§_-433§ && param1.data.other == owner)
         {
            return;
         }
         this.toHit(param1.data.other);
         disposed = true;
      }
      
      protected function toHit(param1:§_-F3o§) : void
      {
         var _loc2_:uint = uint(this.paramSet.damage.value);
         _loc2_ = this.bonusOnFirstHit(param1,_loc2_);
         var _loc3_:uint = param1 != null ? 0 : uint(this.paramSet.damageRadius.value);
         var _loc4_:uint = param1 != null ? 0 : uint(this.paramSet.explosionRadius.value);
         inflictDamage(param1,_loc2_,_loc3_,_loc4_,this.selfHit);
         §_-h2B§.play(param1 == null ? §_-d27§.§_-k4§(groundHitSounds) : §_-d27§.§_-k4§(wormHitSounds));
      }
      
      protected function bonusOnFirstHit(param1:§_-F3o§, param2:uint) : uint
      {
         if(param1 != null && param1 is §_-01J§ && !§_-01J§(param1).buffs.hasBuff(§_-i1B§.NAME))
         {
            §_-01J§(param1).buffs.place(new §_-i1B§());
            return param2 * 2;
         }
         return param2;
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_DRILL,x,y,3 * this.paramSet.explosionRadius.value,null);
         removeEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         super.defaultOnDisposed(param1);
      }
   }
}

