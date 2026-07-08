package ru.pragmatix.wormix.weapons
{
   import ru.pragmatix.wormix.weapons.ammo.BunkerBusterMissile;
   
   public class SuperBunkerBuster extends BunkerBuster
   {
      
      public function SuperBunkerBuster()
      {
         super();
      }
      
      override protected function §_-RT§(param1:int, param2:int) : void
      {
         new BunkerBusterMissile(owner,paramSet,paramSetEx,param1,param2,§_-T2g§,0,true);
      }
   }
}

