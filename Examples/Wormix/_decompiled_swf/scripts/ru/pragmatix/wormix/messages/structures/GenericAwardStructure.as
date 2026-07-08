package ru.pragmatix.wormix.messages.structures
{
   import §_-C2t§.§_-L3H§;
   import §_-W§.*;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.Grenade;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import starling.events.Event;
   
   public class GenericAwardStructure
   {
      
      public var awardKind:int;
      
      public var count:int;
      
      public var itemId:int;
      
      public function GenericAwardStructure()
      {
         super();
      }
      
      public function toString() : String
      {
         return "GenericAwardStructure{" + "awardKind=" + this.awardKind + ",itemId=" + this.itemId + ",count=" + this.count + "}";
      }
   }
}

