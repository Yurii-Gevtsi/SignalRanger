package ru.pragmatix.wormix.weapons
{
   import §_-A1K§.§_-TA§;
   import §_-F39§.§_-a2W§;
   import §_-P1B§.§_-b21§;
   import §_-Y1b§.§_-OP§;
   import §_-d2B§.§_-w2U§;
   import §_-l1K§.§_-cZ§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.HealBlocker;
   
   public class DoctorBag extends Weapon
   {
      
      private static const §_-Cy§:§_-TA§ = new §_-TA§(75);
      
      private static const RADIUS:uint = 150;
      
      public function DoctorBag()
      {
         super();
         §_-a1R§.value = false;
         §_-N1N§ = false;
         §_-z1W§(new §_-cZ§(this.§_-413§));
         §_-Y2U§ = new §_-w2U§();
      }
      
      override protected function shoot() : void
      {
         var _loc3_:int = 0;
         var _loc4_:int = 0;
         var _loc5_:§_-01J§ = null;
         var _loc1_:§_-eX§ = §_-X1j§.§_-12n§.gameMaster;
         if(Boolean(_loc1_) && _loc1_.§_-42C§())
         {
            _loc1_.activeUnit.§_-vp§ = true;
         }
         §_-h2B§.play(§_-d27§.BANDAGE);
         var _loc2_:Vector.<§_-01J§> = Physics.getUnitsInCircle(owner.x,owner.y,RADIUS,§_-X1j§.§_-12n§.gameMaster.§_-b2W§(owner));
         for each(_loc5_ in _loc2_)
         {
            _loc3_ = _loc5_.bonuses.getStatValue(§_-x2M§.HEAL_BONUS);
            _loc4_ = §_-Cy§.value + (_loc3_ > 0 ? MathUtil.§_-na§(_loc3_,§_-Cy§.value) : 0);
            _loc5_.attributes.hp.restore(_loc4_,§_-b21§.§_-uy§);
         }
         detach();
      }
      
      public function §_-413§(param1:§_-01J§ = null) : Boolean
      {
         var _loc3_:Vector.<§_-01J§> = null;
         var _loc7_:§_-01J§ = null;
         param1 ||= owner;
         var _loc2_:Boolean = §_-OP§.§_-Ri§(HealBlocker,param1,false) != null;
         if(_loc2_)
         {
            if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
            {
               §_-a2W§.§_-1H§(§_-s2a§.§_-K3t§(§_-s2a§.DOCTOR_BAG_USELESS),true);
            }
            return false;
         }
         _loc3_ = §_-X1j§.§_-12n§.gameMaster.§_-b2W§(param1);
         var _loc4_:Vector.<§_-01J§> = Physics.getUnitsInCircle(param1.x,param1.y,RADIUS,_loc3_);
         var _loc5_:uint = _loc4_.length;
         var _loc6_:uint = 0;
         if(_loc5_ > 0)
         {
            for each(_loc7_ in _loc4_)
            {
               if(_loc7_.attributes.hp.value >= _loc7_.attributes.hp.§_-33H§())
               {
                  _loc6_++;
               }
            }
            if(_loc5_ == _loc6_)
            {
               if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
               {
                  §_-a2W§.§_-1H§(§_-s2a§.§_-K3t§(§_-s2a§.DOCTOR_BAG_USELESS_MAX_HP),true);
               }
               return false;
            }
            return true;
         }
         if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
         {
            §_-a2W§.§_-1H§(§_-s2a§.§_-K3t§(§_-s2a§.DOCTOR_BAG_USELESS_NO_ALLIES),true);
         }
         return false;
      }
   }
}

