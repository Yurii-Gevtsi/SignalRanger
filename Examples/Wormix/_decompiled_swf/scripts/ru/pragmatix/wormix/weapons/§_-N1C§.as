package ru.pragmatix.wormix.weapons
{
   import §_-A1K§.§_-TA§;
   import §_-D3A§.§_-TZ§;
   import §_-L1H§.§_-C3e§;
   import §_-l1g§.§_-L3Q§;
   import flash.display.BitmapData;
   import flash.display.Shape;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.model.IPhysicModel;
   import starling.display.Image;
   import starling.events.Event;
   import starling.textures.Texture;
   
   public class §_-N1C§
   {
      
      private static var storage:Vector.<§_-N1C§>;
      
      private static var §_-E2d§:Vector.<§_-N1C§>;
      
      public var recordId:int;
      
      public var §_-E2Y§:Boolean;
      
      public var §_-P2Q§:Boolean;
      
      public function §_-N1C§()
      {
         super();
      }
      
      public static function create(param1:int, param2:Boolean, param3:Boolean) : §_-N1C§
      {
         if(storage == null)
         {
            storage = new Vector.<§_-N1C§>(0);
            §_-E2d§ = new Vector.<§_-N1C§>(0);
            §_-TZ§.addListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.EOT,§_-r2k§);
            §_-X1j§.§_-12n§.scene.addEventListener(Event.REMOVED_FROM_STAGE,§_-D22§);
         }
         var _loc4_:§_-N1C§ = storage.pop();
         if(_loc4_ == null)
         {
            _loc4_ = new §_-N1C§();
         }
         _loc4_.setData(param1,param2,param3);
         §_-E2d§.push(_loc4_);
         return _loc4_;
      }
      
      private static function §_-r2k§(param1:Event) : void
      {
         var _loc2_:§_-N1C§ = §_-E2d§.pop();
         while(_loc2_)
         {
            _loc2_.§_-bV§();
            storage.push(_loc2_);
            _loc2_ = §_-E2d§.pop();
         }
      }
      
      private static function §_-D22§(param1:Event) : void
      {
         §_-TZ§.removeListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.EOT,§_-r2k§);
         §_-X1j§.§_-12n§.scene.removeEventListener(Event.REMOVED_FROM_STAGE,§_-D22§);
         storage = §_-E2d§ = null;
      }
      
      private function setData(param1:int, param2:Boolean, param3:Boolean) : void
      {
         this.recordId = param1;
         this.§_-E2Y§ = param2;
         this.§_-P2Q§ = param3;
      }
      
      private function §_-bV§() : void
      {
         this.recordId = 0;
         this.§_-E2Y§ = false;
         this.§_-P2Q§ = false;
      }
   }
}

