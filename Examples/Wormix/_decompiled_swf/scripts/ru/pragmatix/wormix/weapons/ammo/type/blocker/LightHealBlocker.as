package ru.pragmatix.wormix.weapons.ammo.type.blocker
{
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.weapons.ammo.phase.HealPhase;
   import starling.events.Event;
   
   public class LightHealBlocker extends HealBlocker
   {
      
      private static const HEAL_HP_THRESHOLDS:Array = [0.4,0.8];
      
      private static const HEAL_RATES:Array = [15,10];
      
      private static const THRESHOLDS_COUNT:uint = HEAL_HP_THRESHOLDS.length;
      
      public function LightHealBlocker(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:uint, param5:§_-TA§)
      {
         super(param1,param2,param3,param4,param5);
      }
      
      override protected function checkDuration(param1:Event) : void
      {
         var _loc2_:Vector.<§_-01J§> = null;
         var _loc3_:§_-01J§ = null;
         super.checkDuration(param1);
         if(!disposed)
         {
            _loc2_ = §_-X1j§.§_-12n§.gameMaster.§_-b2W§(owner,false,true);
            for each(_loc3_ in _loc2_)
            {
               this.checkAndHealUnit(_loc3_);
            }
         }
      }
      
      private function checkAndHealUnit(param1:§_-01J§) : void
      {
         var _loc2_:int = 0;
         while(_loc2_ < THRESHOLDS_COUNT)
         {
            if(param1.attributes.hp.§_-uk§() < HEAL_HP_THRESHOLDS[_loc2_])
            {
               new HealPhase(owner,x,y,param1,HEAL_RATES[_loc2_],true);
               break;
            }
            _loc2_++;
         }
      }
   }
}

