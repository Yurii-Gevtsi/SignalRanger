package ru.pragmatix.wormix.weapons.ammo.type.animal
{
   import §_-L1H§.§_-C3e§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.weapons.ammo.phase.HomingFrostPhase;
   import ru.pragmatix.wormix.weapons.ammo.type.mine.SpiderMineSimple;
   
   public class SpiderMineFrostBolt extends SpiderMineSimple
   {
      
      private static const DAMAGE:uint = 25;
      
      private static const FREEZE_DURATION:uint = 1;
      
      public function SpiderMineFrostBolt(param1:§_-01J§, param2:§_-C3e§)
      {
         super(param1,param2);
      }
      
      override protected function addPoisonDebuffToTarget() : void
      {
         new HomingFrostPhase(owner,this.x,this.y,false,target,DAMAGE,FREEZE_DURATION);
      }
   }
}

