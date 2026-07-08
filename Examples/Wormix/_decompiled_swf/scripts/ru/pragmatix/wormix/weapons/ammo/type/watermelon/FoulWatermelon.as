package ru.pragmatix.wormix.weapons.ammo.type.watermelon
{
   import §_-L1H§.§_-C3e§;
   import §_-gG§.§_-8u§;
   import §_-l1g§.§_-L3Q§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Physics;
   import starling.events.Event;
   
   public class FoulWatermelon extends Watermelon
   {
      
      protected var collideCount:uint = 2;
      
      public function FoulWatermelon(param1:§_-01J§, param2:§_-C3e§, param3:§_-C3e§, param4:uint, param5:Number, param6:uint, param7:Number)
      {
         super(param1,param2,param3,param4,param5,param6,param7);
         pieceClass = FoulWatermelonPiece;
         explosionEffectType = EffectType.EXPLOSION_POISON_LARGE;
         addEventListener(§_-L3Q§.§_-o13§,this.onCollide);
      }
      
      override protected function initialize(param1:Point, param2:String = null, param3:Boolean = false, param4:String = "resurces/_ammo/") : void
      {
         super.initialize(param1,param2,param3,param4);
         Effects.attachParticleTrail(ParticleEffectType.AMMO_POISON_TRAIL,this);
      }
      
      private function onCollide(param1:Event) : void
      {
         --this.collideCount;
         if(this.collideCount == 0)
         {
            this.explode();
         }
      }
      
      override public function explode() : void
      {
         var _loc1_:§_-01J§ = null;
         if(!disposed)
         {
            super.explode();
            for each(_loc1_ in Physics.getUnitsInCircle(x,y,paramSet.damageRadius.value))
            {
               if(!_loc1_.disposed && !_loc1_.buffs.hasBuff(§_-8u§.NAME))
               {
                  _loc1_.buffs.place(new §_-8u§(owner));
               }
            }
         }
      }
   }
}

