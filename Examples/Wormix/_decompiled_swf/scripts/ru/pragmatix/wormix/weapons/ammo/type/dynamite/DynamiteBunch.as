package ru.pragmatix.wormix.weapons.ammo.type.dynamite
{
   import §_-L1H§.§_-C3e§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.DynamiteStick;
   import ru.pragmatix.wormix.weapons.ammo.Shard;
   
   public class DynamiteBunch extends DynamiteStick
   {
      
      private var paramSetEx:§_-C3e§;
      
      public function DynamiteBunch(param1:§_-01J§, param2:int, param3:int, param4:§_-C3e§)
      {
         super(param1,param2,param3,param4);
      }
      
      public function setParamSetEx(param1:§_-C3e§) : void
      {
         this.paramSetEx = param1;
      }
      
      override public function explode() : void
      {
         var _loc2_:Shard = null;
         var _loc1_:int = -1;
         while(_loc1_ <= 1)
         {
            if(_loc1_ != 0)
            {
               _loc2_ = new Shard(owner,x,y,-MathUtil.PI_HALF + _loc1_ * MathUtil.PI_ONE_SIX,8,this.paramSetEx);
            }
            _loc1_++;
         }
         super.explode();
      }
   }
}

