package ru.pragmatix.wormix.weapons.ammo.abstracts
{
   import §_-L1H§.§_-C3e§;
   import §_-l1g§.§_-L3Q§;
   import §_-w2i§.§_-Zd§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class AbstractAirMissile extends §_-Qi§
   {
      
      protected var paramSet:§_-C3e§;
      
      public function AbstractAirMissile(param1:§_-01J§, param2:§_-C3e§, param3:int, param4:int, param5:int, param6:int)
      {
         super(param1,param3,param4,false,§_-Qi§.§_-n18§,DamageType.AIR,false);
         initialize(Pool.getPoint(param3,param4),param2.ammoClassName);
         this.paramSet = param2;
         setSpeedXY(param5,param6);
         §_-Zd§.once(this,§_-L3Q§.§_-o13§,this.onCollide);
         §_-32§ = §_-uN§;
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      private function onCollide(param1:Event) : void
      {
         if(!disposed)
         {
            this.explode(param1.data.other);
         }
      }
      
      protected function explode(param1:§_-F3o§) : void
      {
         disposed = true;
         inflictDamage(param1,this.paramSet.damage.value,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value);
      }
   }
}

