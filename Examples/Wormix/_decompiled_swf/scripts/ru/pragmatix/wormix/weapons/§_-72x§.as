package ru.pragmatix.wormix.weapons
{
   import §_-b1F§.§_-G4§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.IPhysicModel;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.type.missile.homing.HomingMissile;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class §_-72x§ extends Weapon
   {
      
      private var targets:Vector.<§_-01J§> = new Vector.<§_-01J§>(0);
      
      public function §_-72x§()
      {
         super();
      }
      
      private function §_-u2W§() : void
      {
         var _loc1_:§_-01J§ = null;
         this.targets = this.§_-P1y§();
         for each(_loc1_ in this.targets)
         {
            _loc1_.graphics.colorTransform(40,-30,-20);
         }
      }
      
      private function §_-e6§() : void
      {
         var _loc1_:§_-01J§ = null;
         for each(_loc1_ in this.targets)
         {
            _loc1_.graphics.colorTransform(-40,30,20);
         }
         this.targets = new Vector.<§_-01J§>(0);
      }
      
      protected function §_-P1y§() : Vector.<§_-01J§>
      {
         var _loc1_:int = owner.x + owner.direction * 40;
         var _loc2_:int = owner.y;
         return Physics.getUnitsInCircle(_loc1_,_loc2_,paramSet.damageRadius.value);
      }
      
      override protected function onFrame(param1:Event) : void
      {
         this.§_-e6§();
         if(owner != null)
         {
            this.§_-u2W§();
         }
      }
   }
}

