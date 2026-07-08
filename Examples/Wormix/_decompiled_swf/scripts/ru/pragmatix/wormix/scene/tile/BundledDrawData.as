package ru.pragmatix.wormix.scene.tile
{
   import §_-G16§.§_-33h§;
   
   public class BundledDrawData
   {
      
      private static var pool:§_-33h§ = new §_-33h§(BundledDrawData);
      
      public var posX:Number;
      
      public var posY:Number;
      
      public var scX:Number;
      
      public var scY:Number;
      
      public function BundledDrawData()
      {
         super();
      }
      
      public static function create(param1:Number, param2:Number, param3:Number, param4:Number) : BundledDrawData
      {
         var _loc5_:BundledDrawData = pool.get() as BundledDrawData;
         _loc5_.init(param1,param2,param3,param4);
         return _loc5_;
      }
      
      public function init(param1:Number, param2:Number, param3:Number, param4:Number) : void
      {
         this.posX = param1;
         this.posY = param2;
         this.scX = param3;
         this.scY = param4;
      }
   }
}

