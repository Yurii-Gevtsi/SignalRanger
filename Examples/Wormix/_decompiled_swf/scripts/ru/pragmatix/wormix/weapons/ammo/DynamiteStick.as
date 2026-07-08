package ru.pragmatix.wormix.weapons.ammo
{
   import §_-L1H§.§_-C3e§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-V1R§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.simple.BouncingGrenade;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class DynamiteStick extends BouncingGrenade
   {
      
      private static const CLOCK_TIME:uint = 90;
      
      private var fuseSound:§_-V1R§;
      
      public function DynamiteStick(param1:§_-01J§, param2:int, param3:int, param4:§_-C3e§, param5:uint = 1, param6:Number = NaN)
      {
         if(isNaN(param6))
         {
            param6 = -MathUtil.PI_HALF + param1.direction * MathUtil.PI_QUARTER;
         }
         super(param1,param4,CLOCK_TIME,param6,param5,0.2,null);
         counter.§_-u2c§(true);
         bounceSounds = null;
         stopSound = null;
         if(armature)
         {
            armature.playAnimation(§_-K1t§.§_-G1E§);
         }
         §_-32§ = §_-8G§;
         §_-z1H§.rotation = 0;
         §_-z1H§.pivotY += 5;
         initialize(Pool.getPoint(param2,param3));
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         super.defaultOnDisposed(param1);
         if(this.fuseSound != null)
         {
            this.fuseSound.stop();
         }
      }
   }
}

