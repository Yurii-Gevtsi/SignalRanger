package ru.pragmatix.wormix.weapons
{
   import §_-12a§.§_-lc§;
   import §_-A1K§.§_-TA§;
   import §_-F39§.§_-a2W§;
   import §_-d2B§.§_-pY§;
   import §_-l1K§.§_-cZ§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   
   public class Antitoxin extends Weapon
   {
      
      private const DURATION:§_-TA§ = new §_-TA§(6);
      
      public function Antitoxin()
      {
         super();
         §_-a1R§.value = false;
         §_-N1N§ = false;
         §_-z1W§(new §_-cZ§(this.§_-413§));
         §_-Y2U§ = new §_-pY§();
      }
      
      override protected function shoot() : void
      {
         var _loc1_:§_-eX§ = §_-X1j§.§_-12n§.gameMaster;
         if(Boolean(_loc1_) && _loc1_.§_-42C§())
         {
            _loc1_.activeUnit.§_-vp§ = true;
         }
         §_-h2B§.play(§_-d27§.ANTITOXIN);
         owner.buffs.place(new §_-lc§(paramSet.damage.value,this.DURATION.value));
         detach();
      }
      
      private function §_-413§() : Boolean
      {
         if(owner.buffs.hasBuff(§_-lc§.NAME))
         {
            if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
            {
               §_-a2W§.§_-1H§(§_-s2a§.§_-K3t§(§_-s2a§.ALREADY_ANTITOXIN),true);
            }
            return false;
         }
         return true;
      }
   }
}

