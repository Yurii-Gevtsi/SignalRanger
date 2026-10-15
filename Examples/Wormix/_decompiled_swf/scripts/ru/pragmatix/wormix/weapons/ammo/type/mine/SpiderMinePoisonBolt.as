package ru.pragmatix.wormix.weapons.ammo.type.mine
{
   import §_-L1H§.§_-C3e§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.weapons.ammo.phase.PoisonPhase;
   
   public class SpiderMinePoisonBolt extends SpiderMineSimple
   {
      
      public function SpiderMinePoisonBolt(param1:§_-01J§, param2:§_-C3e§)
      {
         super(param1,param2);
      }
      
      override protected function addPoisonDebuffToTarget() : void
      {
         new PoisonPhase(owner,this.x,this.y,false,target,10,0);
      }
   }
}

