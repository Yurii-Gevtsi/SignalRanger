package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-V19§;
   import §_-l1K§.§_-N1Z§;
   import §_-l1K§.§_-W2f§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Force;
   
   public class ShurikenToss extends Weapon
   {
      
      public function ShurikenToss()
      {
         super();
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         shots.value = param1["quantityShots"].value;
         MIN_POWER = MAX_POWER = param1["power"].value;
         if(param1["trajectoryCrosshair"].value)
         {
            §§push(§§findproperty(§_-z1W§));
            §§push(§§findproperty(§_-N1Z§));
            §§push(MAX_POWER);
            var _temp_2:* = new <Force>[Force.GRAVITY];
            §§pop().§_-z1W§(new §§pop().§_-N1Z§(§§pop(),new <Force>[Force.GRAVITY]));
         }
         else
         {
            §_-z1W§(new §_-W2f§());
         }
         §_-Y2U§ = new §_-V19§(70,MIN_POWER);
         super.§_-z2Y§(param1);
      }
      
      override protected function shoot() : void
      {
         var _loc1_:Class = getProperty("classShuriken") as Class;
         var _loc2_:Boolean = getProperty("noTrail") == true;
         new _loc1_(owner,paramSet,shotParams.angle,MIN_POWER,!_loc2_);
         if(shotsLeft.value == 0)
         {
            this.detach();
            nextTurnOnStable();
         }
      }
      
      override public function attach(param1:§_-01J§) : void
      {
         super.attach(param1);
      }
      
      override public function detach() : void
      {
         super.detach();
      }
   }
}

