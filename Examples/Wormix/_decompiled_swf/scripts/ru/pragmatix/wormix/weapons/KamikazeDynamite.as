package ru.pragmatix.wormix.weapons
{
   import §_-12a§.KamikazeBuff;
   import §_-F39§.§_-a2W§;
   import §_-l1K§.§_-cZ§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   
   public class KamikazeDynamite extends Weapon
   {
      
      public function KamikazeDynamite()
      {
         super();
         §_-a1R§.value = false;
         §_-N1N§ = false;
         §_-z1W§(new §_-cZ§(this.§_-413§));
      }
      
      override protected function shoot() : void
      {
         owner.buffs.place(new KamikazeBuff(paramSet));
         detach();
         §_-X1j§.§_-12n§.gameMaster.activeUnit.§_-vp§ = true;
      }
      
      private function §_-413§() : Boolean
      {
         if(owner.buffs.hasBuff(KamikazeBuff.NAME))
         {
            if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
            {
               §_-a2W§.§_-1H§(§_-s2a§.§_-K3t§(§_-s2a§.KAMIKAZE_ALREADY_ACTIVATED),true);
            }
            return false;
         }
         return true;
      }
   }
}

