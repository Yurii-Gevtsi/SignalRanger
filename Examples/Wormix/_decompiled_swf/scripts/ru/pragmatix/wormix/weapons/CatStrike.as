package ru.pragmatix.wormix.weapons
{
   import §_-L1H§.§_-C3e§;
   import §_-r1C§.§_-h2B§;
   import §_-u1m§.§_-61A§;
   import ru.pragmatix.wormix.controller.§_-J2d§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.weapons.ammo.AirCatAmmo;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import ru.pragmatix.wormix.weapons.interfaces.§_-jm§;
   
   public class CatStrike extends §_-61A§ implements §_-jm§
   {
      
      private var targets:Array;
      
      public function CatStrike()
      {
         super();
      }
      
      override public function attach(param1:§_-01J§) : void
      {
         super.attach(param1);
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         var _loc2_:uint = param1["targets"] ? uint(param1["targets"].value) : 1;
         §_-UP§ = _loc2_;
         var _loc3_:uint = 1;
         while(_loc3_ < _loc2_)
         {
            §_-D2a§(§_-V1B§);
            _loc3_++;
         }
         super.§_-z2Y§(param1);
      }
      
      override protected function §_-Q2r§(param1:Number, param2:Number, param3:int) : void
      {
         var _loc4_:Object = {
            "x":param1,
            "y":param2,
            "dir":param3
         };
         this.targets.push(_loc4_);
         super.§_-Q2r§(param1,param2,param3);
      }
      
      override protected function §_-83o§() : void
      {
         var _loc1_:Object = null;
         §_-h2B§.play(§_-d27§.§_-h1g§);
         if(this.targets.length > 1)
         {
            for each(_loc1_ in this.targets)
            {
               this.§_-w1B§(_loc1_.x,_loc1_.y,_loc1_.dir);
            }
         }
         else
         {
            super.§_-83o§();
         }
      }
      
      private function §_-w1B§(param1:int, param2:int, param3:int) : void
      {
         var _loc4_:int = 40;
         new AirCatAmmo(owner,paramSet,§_-22V§(param3,param1 - _loc4_,param2),§_-039§,param3 * §_-AX§,0,DamageType.AIR);
         new AirCatAmmo(owner,paramSet,§_-22V§(param3,param1 + _loc4_,param2),§_-039§,param3 * §_-AX§,0,DamageType.AIR);
      }
      
      override protected function §_-B1T§(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:Number, param5:Number, param6:Number) : void
      {
         var _loc7_:int = -1;
         while(_loc7_ <= 1)
         {
            new AirCatAmmo(param1,param2,param3 + 60 * _loc7_,param4,param5,param6,DamageType.AIR);
            _loc7_++;
         }
         super.§_-B1T§(param1,param2,param3,param4,param5,param6);
      }
      
      override protected function §_-kn§() : void
      {
         super.§_-kn§();
         this.targets = [];
      }
   }
}

