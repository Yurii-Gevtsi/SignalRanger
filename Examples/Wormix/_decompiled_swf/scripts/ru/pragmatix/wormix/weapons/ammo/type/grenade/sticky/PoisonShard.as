package ru.pragmatix.wormix.weapons.ammo.type.grenade.sticky
{
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import §_-gG§.§_-8u§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.ammo.Shard;
   import starling.events.Event;
   
   public class PoisonShard extends Shard
   {
      
      private const POISON_DURATION:§_-TA§ = new §_-TA§(4);
      
      public function PoisonShard(param1:§_-01J§, param2:int, param3:int, param4:Number, param5:uint, param6:§_-C3e§, param7:Number = 1, param8:Boolean = true)
      {
         super(param1,param2,param3,param4,param5,param6,param7,param8);
      }
      
      override protected function onCollision(param1:Event) : void
      {
         var _loc3_:§_-01J§ = null;
         var _loc2_:Vector.<§_-01J§> = Physics.getUnitsInCircle(x,y,paramSet.damageRadius.value);
         for each(_loc3_ in _loc2_)
         {
            if(!_loc3_.disposed && !_loc3_.buffs.hasBuff(§_-8u§.NAME))
            {
               _loc3_.buffs.place(new §_-8u§(_loc3_,this.POISON_DURATION.value));
            }
         }
         super.onCollision(param1);
      }
   }
}

