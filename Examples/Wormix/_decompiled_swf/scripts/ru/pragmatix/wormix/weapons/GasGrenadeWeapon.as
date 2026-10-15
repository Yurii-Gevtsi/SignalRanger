package ru.pragmatix.wormix.weapons
{
   import §_-31W§.§_-Kb§;
   import §_-A1K§.§_-TA§;
   import ru.pragmatix.wormix.weapons.ammo.cloud.FreezingCloud;
   import ru.pragmatix.wormix.weapons.ammo.cloud.GasCloud;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.GasGrenade;
   import ru.pragmatix.wormix.weapons.interfaces.§_-O1c§;
   import starling.extensions.ColorArgb;
   
   public class GasGrenadeWeapon extends Grenade implements §_-O1c§
   {
      
      private static const §_-w1V§:Number = 0.5;
      
      private var cloudClipName:String;
      
      private var armorReduction:uint = 0;
      
      public function GasGrenadeWeapon()
      {
         super();
         §_-Y2U§ = null;
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         super.§_-z2Y§(param1);
         if(param1["armorReduction"])
         {
            this.armorReduction = §_-TA§(param1["armorReduction"]).value;
         }
         this.cloudClipName = param1["cloudClipName"];
      }
      
      override protected function shoot() : void
      {
         §_-BB§();
         var _loc1_:Class = property["slowing"] ? FreezingCloud : GasCloud;
         ammo = new GasGrenade(owner,paramSet,clock.value,shotParams.angle,§_-Yz§(shotParams.power),§_-w1V§,"",property["cloudColor"] as ColorArgb,_loc1_);
         if(this.armorReduction > 0)
         {
            GasGrenade(ammo).setArmorReduction(this.armorReduction);
         }
         if(getProperty("explodeOnStopMoving"))
         {
            §_-B2r§ = true;
            ammo.explodeOnStopMoving();
         }
         §_-Ws§();
         nextTurnOnStable();
      }
   }
}

