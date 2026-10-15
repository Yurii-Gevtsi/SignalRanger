package ru.pragmatix.wormix.messages.structures
{
   import §_-L1H§.§_-C3e§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.cluster.ShellBurst;
   
   public class HeroicMissionStructure
   {
      
      public var bossIds:Vector.<int> = new Vector.<int>();
      
      public var mapId:uint;
      
      public function HeroicMissionStructure()
      {
         super();
      }
      
      public function get bossId1() : int
      {
         return this.bossIds[0];
      }
      
      public function get bossId2() : int
      {
         return this.bossIds[1];
      }
   }
}

