package ru.pragmatix.wormix.weapons.ammo.type.shuriken
{
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import §_-b1F§.§_-G4§;
   import §_-sQ§.§_-H3D§;
   import flash.utils.Dictionary;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import starling.events.Event;
   
   public class ShurikenSaw extends Shuriken
   {
      
      private static const MAX_HITS_PER_UNIT:uint = 3;
      
      private static const HIT_FORCE:Number = -12;
      
      private const RADIUS_SEARCH:§_-TA§ = new §_-TA§(30);
      
      private var victims:Dictionary = new Dictionary(true);
      
      public function ShurikenSaw(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:uint, param5:Boolean = true)
      {
         super(param1,param2,param3,param4,param5);
      }
      
      override protected function onCollide(param1:Event) : void
      {
         if(param1.data.other is §_-01J§)
         {
            return;
         }
         super.onCollide(param1);
      }
      
      override protected function bonusOnFirstHit(param1:§_-F3o§, param2:uint) : uint
      {
         return param1 == null ? 1 : param2;
      }
      
      override public function nextState() : void
      {
         var _loc1_:§_-01J§ = null;
         super.nextState();
         for each(_loc1_ in Physics.getUnitsInCircle(x,y,this.RADIUS_SEARCH.value))
         {
            if(this.victims[_loc1_] == null)
            {
               this.victims[_loc1_] = 0;
            }
            if(!_loc1_.disposed && _loc1_ != owner && this.victims[_loc1_] < MAX_HITS_PER_UNIT)
            {
               ++this.victims[_loc1_];
               toHit(_loc1_ as §_-F3o§);
               §_-G4§.§_-u1v§(_loc1_ as §_-F3o§,§_-H3D§.sign(speedX),HIT_FORCE,false);
            }
         }
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         super.defaultOnDisposed(param1);
         this.victims = null;
      }
   }
}

