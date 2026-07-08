package ru.pragmatix.wormix.weapons
{
   import §_-12a§.§_-41b§;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-P1B§.§_-b21§;
   import §_-l1K§.§_-cZ§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.utils.MathUtil;
   
   public class AntifreezePotion extends Weapon
   {
      
      public function AntifreezePotion()
      {
         super();
         §_-z1W§(new §_-cZ§());
      }
      
      override protected function shoot() : void
      {
         var _loc1_:int = paramSet.damage.value;
         var _loc2_:int = owner.bonuses.getStatValue(§_-x2M§.HEAL_BONUS);
         if(_loc2_ > 0)
         {
            _loc1_ += MathUtil.§_-na§(_loc2_,_loc1_);
         }
         owner.attributes.hp.restore(_loc1_,§_-b21§.§_-uy§,true);
         owner.buffs.place(new §_-41b§(2,§_-7u§(getProperty("protection")).value,§_-TA§(getProperty("burstAttack")).value,§_-TA§(getProperty("burstArmor")).value));
         §_-h2B§.play(§_-d27§.ANTIFREEZE);
         detach();
         nextTurnOnStable();
      }
   }
}

