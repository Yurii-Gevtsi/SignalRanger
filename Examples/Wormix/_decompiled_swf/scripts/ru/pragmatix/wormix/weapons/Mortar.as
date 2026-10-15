package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-V19§;
   import §_-73d§.§_-u17§;
   import §_-73d§.§_-x2C§;
   import §_-79§.§_-ps§;
   import §_-A1K§.§_-TA§;
   import §_-l1K§.§_-N1Z§;
   import §_-l1K§.§_-W2f§;
   import §_-l1K§.§_-d2j§;
   import §_-w2i§.§_-Ia§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.MortarMissile;
   import starling.events.Event;
   
   public class Mortar extends Weapon
   {
      
      public static const §_-H21§:int = 30;
      
      public static const §_-qy§:int = 30;
      
      private var frames:§_-TA§ = new §_-TA§(0);
      
      private const §_-Z1D§:§_-TA§ = new §_-TA§(10);
      
      private var angle:Number;
      
      private const §_-i23§:Number = 5;
      
      private var §_-Z5§:int = 0;
      
      public function Mortar()
      {
         super();
         MAX_POWER = §_-H21§;
         MIN_POWER = §_-qy§;
         §_-Y2U§ = new §_-u17§();
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         if(param1["trajectoryCrosshair"].value)
         {
            §_-z1W§(new §_-N1Z§(MAX_POWER,new <Force>[Force.GRAVITY]));
         }
         else
         {
            §_-z1W§(new §_-W2f§());
         }
         super.§_-z2Y§(param1);
      }
      
      override protected function shoot() : void
      {
         this.§_-T1Q§(shotParams.angle);
         if(getProperty("additionalMissile") > 0)
         {
            shooting = true;
            ControlSets.UNIT_CONTROL_SET.deactivate();
            §_-K2b§();
            this.angle = shotParams.angle;
            this.frames.value = 0;
         }
         else
         {
            this.nextTurn();
         }
      }
      
      override protected function onFrame(param1:Event) : void
      {
         if(getProperty("additionalMissile") > 0 && owner != null)
         {
            ++this.frames.value;
            if(this.frames.value >= this.§_-Z1D§.value)
            {
               this.angle += (§_-X1j§.randomSeed.getBoolean() ? 1 : -1) * this.§_-i23§ * MathUtil.PI_ONE_180;
               Weapon(owner.weapon).rotateWeaponClip(this.angle);
               this.§_-T1Q§(this.angle);
               this.frames.value = 0;
               if(++this.§_-Z5§ >= getProperty("additionalMissile"))
               {
                  if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
                  {
                     ControlSets.UNIT_CONTROL_SET.activate();
                  }
                  this.nextTurn();
               }
            }
         }
      }
      
      private function §_-T1Q§(param1:Number) : void
      {
         var _loc2_:MortarMissile = new MortarMissile(owner,getBarrelPoint(param1),paramSet,paramSetEx,param1,MAX_POWER);
         if(property["cleverShards"].value)
         {
            _loc2_.enableCleverShards();
         }
      }
      
      private function nextTurn() : void
      {
         detach();
         nextTurnOnStable();
      }
   }
}

