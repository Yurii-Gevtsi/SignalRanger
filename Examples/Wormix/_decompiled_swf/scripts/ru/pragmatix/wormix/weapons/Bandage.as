package ru.pragmatix.wormix.weapons
{
   import §_-12a§.§_-6p§;
   import §_-F39§.§_-a2W§;
   import §_-P1B§.§_-b21§;
   import §_-d2B§.§_-313§;
   import §_-l1K§.§_-cZ§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-MN§;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.HealBlocker;
   
   public class Bandage extends Weapon
   {
      
      public static const §_-f25§:Number = 0.5;
      
      public static const §_-12U§:uint = 10;
      
      private const COLOR:uint = 15461355;
      
      public function Bandage()
      {
         super();
         §_-a1R§.value = false;
         §_-N1N§ = false;
         §_-z1W§(new §_-cZ§(this.§_-413§));
         §_-Y2U§ = new §_-313§();
      }
      
      override protected function shoot() : void
      {
         var _loc1_:§_-eX§ = §_-X1j§.§_-12n§.gameMaster;
         if(Boolean(_loc1_) && _loc1_.§_-42C§())
         {
            _loc1_.activeUnit.§_-vp§ = true;
         }
         §_-h2B§.play(§_-d27§.BANDAGE);
         owner.buffs.place(new §_-6p§());
         var _loc2_:int = owner.attributes.hp.§_-Z18§() * §_-f25§;
         var _loc3_:int = owner.bonuses.getStatValue(§_-x2M§.HEAL_BONUS);
         if(_loc3_ > 0)
         {
            _loc2_ += MathUtil.§_-na§(_loc3_,_loc2_);
         }
         owner.attributes.hp.restore(_loc2_,§_-b21§.§_-uy§);
         §_-MN§.§_-D9§(owner,this.COLOR);
         detach();
      }
      
      private function §_-413§() : Boolean
      {
         var _loc2_:HealBlocker = null;
         var _loc1_:Boolean = false;
         for each(_loc2_ in §_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-MM§([HealBlocker]))
         {
            if(_loc2_.owner.squad.getTeamId() != owner.squad.getTeamId())
            {
               _loc1_ = true;
               break;
            }
         }
         if(owner.buffs.hasBuff(§_-6p§.NAME))
         {
            if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
            {
               §_-a2W§.§_-1H§(§_-s2a§.§_-K3t§(§_-s2a§.ALREADY_BANDAGED),true);
            }
            return false;
         }
         if(owner.attributes.hp.§_-Z18§() < §_-12U§ || _loc1_)
         {
            if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
            {
               §_-a2W§.§_-1H§(§_-s2a§.§_-K3t§(§_-s2a§.BANDAGE_USELESS),true);
            }
            return false;
         }
         return true;
      }
   }
}

