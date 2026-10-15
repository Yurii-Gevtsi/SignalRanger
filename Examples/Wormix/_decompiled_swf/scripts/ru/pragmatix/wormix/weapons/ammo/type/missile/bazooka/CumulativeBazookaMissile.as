package ru.pragmatix.wormix.weapons.ammo.type.missile.bazooka
{
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import starling.events.Event;
   
   public class CumulativeBazookaMissile extends BazookaMissile
   {
      
      private const MAX_COLLISION:§_-TA§ = new §_-TA§(2);
      
      private var curCollision:§_-TA§ = new §_-TA§(0);
      
      public function CumulativeBazookaMissile(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:uint)
      {
         super(param1,param2,param3,param4);
      }
      
      override protected function onCollisionEvent(param1:Event) : void
      {
         explode(param1.data.other);
         if(++this.curCollision.value >= this.MAX_COLLISION.value)
         {
            disposed = true;
         }
      }
   }
}

