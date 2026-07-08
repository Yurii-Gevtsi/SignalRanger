package ru.pragmatix.wormix.weapons
{
   import §_-A1K§.§_-TA§;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.cluster.SeekingShard;
   
   public class HomingMinigun extends Minigun
   {
      
      private static const SEARCH_RADIUS:uint = 450;
      
      private static const SEARCH_ANGLE:Number = Math.PI / 6;
      
      public function HomingMinigun()
      {
         super();
         MIN_POWER = MAX_POWER = 40;
         §_-A1Q§ = new §_-TA§(4);
      }
      
      override protected function §_-G11§(param1:Number) : void
      {
         new SeekingShard(owner,getBarrelPoint(param1).x,getBarrelPoint(param1).y,param1,MIN_POWER,paramSet.clone(),SEARCH_ANGLE,SEARCH_RADIUS,0.5,true);
      }
   }
}

