package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-V19§;
   import §_-73d§.§_-w1O§;
   import §_-l1K§.§_-W2f§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.Cannonball;
   
   public class Cannon extends Weapon
   {
      
      public function Cannon()
      {
         super();
         MIN_POWER = MAX_POWER = 35;
         §_-z1W§(new §_-W2f§(§_-W2f§.§_-G3n§,MathUtil.PI_HALF - §_-W2f§.§_-G3n§,MathUtil.PI_HALF - §_-W2f§.§_-G3n§));
         §_-Y2U§ = new §_-w1O§(MIN_POWER - 5,MAX_POWER + 5,130,false);
      }
      
      override protected function shoot() : void
      {
         new Cannonball(owner,paramSet,shotParams.angle,MAX_POWER);
         ControlSets.UNIT_CONTROL_SET.deactivate();
         detach();
         nextTurnOnStable();
      }
   }
}

