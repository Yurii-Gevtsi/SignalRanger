package ru.pragmatix.wormix.weapons.ammo.type.grenade.cluster
{
   import §_-L1H§.§_-C3e§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.simple.BouncingGrenade;
   import starling.utils.Pool;
   
   public class BouncingClusterGrenade extends BouncingGrenade
   {
      
      protected var shellBurst:ShellBurst;
      
      public function BouncingClusterGrenade(param1:§_-01J§, param2:§_-C3e§, param3:ShellBurst, param4:uint, param5:Number, param6:uint, param7:Number, param8:String)
      {
         this.shellBurst = param3;
         super(param1,param2,param4,param5,param6,param7,param8);
      }
      
      override public function explode() : void
      {
         super.explode();
         if(this.shellBurst)
         {
            this.shellBurst.explode(Pool.getPoint(x,y));
         }
      }
   }
}

