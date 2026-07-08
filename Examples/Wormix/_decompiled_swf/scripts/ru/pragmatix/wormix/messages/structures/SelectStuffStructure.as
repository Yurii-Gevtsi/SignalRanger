package ru.pragmatix.wormix.messages.structures
{
   import §_-F39§.§_-a2W§;
   import §_-L1H§.§_-b2K§;
   import §_-Tm§.§_-RF§;
   import §_-Y1b§.§_-OP§;
   import §_-l1g§.§_-L3Q§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.HealBlocker;
   import starling.utils.Pool;
   
   public class SelectStuffStructure
   {
      
      public var profileId:int;
      
      public var hatId:int;
      
      public var artifactId:int;
      
      public function SelectStuffStructure(param1:int, param2:int, param3:int)
      {
         super();
         this.profileId = param1;
         this.hatId = param2;
         this.artifactId = param3;
      }
      
      public function toString() : String
      {
         return this.profileId + " " + this.hatId + " " + this.artifactId;
      }
   }
}

