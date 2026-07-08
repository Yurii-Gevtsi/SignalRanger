package ru.pragmatix.wormix.weapons
{
   import §_-F39§.§_-a2W§;
   import §_-l1K§.§_-cZ§;
   import §_-u1m§.§_-51A§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.weapons.ammo.Antimagnet;
   
   public class ForceField extends §_-51A§
   {
      
      public function ForceField()
      {
         super(Antimagnet);
      }
      
      override protected function §_-D2a§() : void
      {
         §_-z1W§(new §_-cZ§(this.§_-sc§));
      }
      
      private function §_-sc§() : Boolean
      {
         var _loc1_:Boolean = false;
         var _loc3_:§_-F3o§ = null;
         var _loc2_:Vector.<§_-F3o§> = §_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-MM§([Antimagnet]);
         for each(_loc3_ in _loc2_)
         {
            if(Antimagnet(_loc3_).getSquadNumber() == owner.getSquadNumber())
            {
               _loc1_ = true;
               break;
            }
         }
         if(_loc1_)
         {
            if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
            {
               §_-a2W§.§_-1H§(§_-s2a§.§_-K3t§(§_-s2a§.FORCE_FIELD_ALREADY_EXIST),true);
            }
            return false;
         }
         return true;
      }
   }
}

