package ru.pragmatix.wormix.weapons
{
   import §_-A1K§.§_-7u§;
   import §_-l1K§.§_-N1Z§;
   import §_-l1K§.§_-W2f§;
   import §_-r1C§.§_-h2B§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.GravityAmmo;
   
   public class GravityGun extends Weapon
   {
      
      public function GravityGun()
      {
         super();
         MIN_POWER = MAX_POWER = 35;
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         var _loc2_:§_-7u§ = param1["trajectoryCrosshair"] as §_-7u§;
         if(Boolean(_loc2_) && _loc2_.value)
         {
            §_-z1W§(new §_-N1Z§(MIN_POWER,null,§_-N1Z§.§_-q17§,MathUtil.PI_HALF,MathUtil.PI_HALF - §_-W2f§.§_-G3n§));
         }
         else
         {
            §_-z1W§(new §_-W2f§(§_-W2f§.§_-G3n§,MathUtil.PI_HALF,MathUtil.PI_HALF - §_-W2f§.§_-G3n§));
         }
      }
      
      override protected function shoot() : void
      {
         this.§_-13v§();
         this.§_-5s§();
      }
      
      protected function §_-5s§() : void
      {
         if(shotsLeft.value == 0)
         {
            detach();
            nextTurnOnStable();
         }
      }
      
      protected function §_-13v§() : void
      {
         var _loc1_:Point = getBarrelPoint(shotParams.angle);
         new GravityAmmo(owner,_loc1_.x,_loc1_.y,paramSet,shotParams.angle,MAX_POWER);
         §_-h2B§.play(§_-d27§.§_-Lp§);
      }
   }
}

