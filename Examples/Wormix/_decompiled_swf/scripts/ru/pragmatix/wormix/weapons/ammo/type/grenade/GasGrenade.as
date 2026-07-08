package ru.pragmatix.wormix.weapons.ammo.type.grenade
{
   import §_-L1H§.§_-C3e§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.simple.BouncingGrenade;
   import starling.extensions.ColorArgb;
   
   public class GasGrenade extends BouncingGrenade
   {
      
      private var color:ColorArgb;
      
      private var armorReduction:uint = 0;
      
      private var cloudClass:Class;
      
      public function GasGrenade(param1:§_-01J§, param2:§_-C3e§, param3:uint, param4:Number, param5:uint, param6:Number, param7:String, param8:ColorArgb, param9:Class)
      {
         super(param1,param2,param3,param4,param5,param6,param7);
         this.color = param8;
         this.cloudClass = param9;
      }
      
      public function setArmorReduction(param1:uint) : void
      {
         this.armorReduction = param1;
      }
      
      override public function explode() : void
      {
         disposed = true;
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_SMOKE,x,y,100,§_-d27§.§_-S14§);
         new this.cloudClass(owner,x,y,paramSet,this.color,this.armorReduction);
      }
   }
}

