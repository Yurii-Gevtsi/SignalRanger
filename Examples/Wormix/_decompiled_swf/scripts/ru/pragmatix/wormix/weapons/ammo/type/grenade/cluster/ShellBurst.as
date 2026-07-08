package ru.pragmatix.wormix.weapons.ammo.type.grenade.cluster
{
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-sQ§.§_-V1n§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.MathUtil;
   
   public class ShellBurst
   {
      
      private static const PI_HALF:Number = MathUtil.PI_HALF;
      
      public var paramSetEx:§_-C3e§;
      
      public var quantityShards:§_-TA§;
      
      private var classShard:Class;
      
      private var owner:§_-01J§;
      
      public function ShellBurst(param1:§_-01J§, param2:§_-C3e§, param3:§_-TA§, param4:Class)
      {
         super();
         this.owner = param1;
         this.paramSetEx = param2;
         this.quantityShards = param3;
         this.classShard = param4;
      }
      
      public function explode(param1:Point) : void
      {
         var _loc3_:Number = NaN;
         var _loc4_:int = 0;
         var _loc5_:int = 0;
         var _loc2_:§_-V1n§ = §_-X1j§.randomSeed;
         if(this.quantityShards.value > 9)
         {
            §_-X1j§.§_-12n§.scene.dispatchEvent(new §_-H1K§(§_-A2V§.§_-A2e§,"shell burst hack"));
         }
         var _loc6_:int = 0;
         while(_loc6_ < this.quantityShards.value)
         {
            _loc3_ = -PI_HALF + Math.PI / _loc2_.§_-Fh§(8,24) * (_loc6_ % 2 == 0 ? 1 : -1);
            _loc4_ = param1.x + 20 * Math.cos(_loc3_);
            _loc5_ = param1.y + 20 * Math.sin(_loc3_);
            new this.classShard(this.owner,_loc4_,_loc5_,_loc3_,_loc2_.§_-Fh§(4,9),this.paramSetEx);
            _loc6_++;
         }
      }
   }
}

