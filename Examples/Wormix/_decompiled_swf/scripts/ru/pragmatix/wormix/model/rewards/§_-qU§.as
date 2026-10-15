package ru.pragmatix.wormix.model.rewards
{
   import §_-Y1O§.*;
   import feathers.controls.ImageLoader;
   import flash.geom.Point;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionData;
   import starling.display.Image;
   
   public class §_-qU§
   {
      
      private static var list:Vector.<§_-qU§> = new Vector.<§_-qU§>(0);
      
      public static const §_-q12§:§_-qU§ = new §_-qU§(1);
      
      public static const §_-r1n§:§_-qU§ = new §_-qU§(2);
      
      public static const §_-8f§:§_-qU§ = new §_-qU§(3);
      
      public static const §_-a2a§:§_-qU§ = new §_-qU§(4);
      
      public static const §_-F17§:§_-qU§ = new §_-qU§(5);
      
      public static const ACHIEVE:§_-qU§ = new §_-qU§(6);
      
      public static const §_-v2j§:§_-qU§ = new §_-qU§(7);
      
      public static const §_-C39§:§_-qU§ = new §_-qU§(8);
      
      public static const §_-q2d§:§_-qU§ = new §_-qU§(10);
      
      public static const §_-Q23§:§_-qU§ = new §_-qU§(28);
      
      public static const §_-21o§:§_-qU§ = new §_-qU§(29);
      
      public static const §_-KS§:§_-qU§ = new §_-qU§(31);
      
      public static const §_-g2x§:§_-qU§ = new §_-qU§(32);
      
      private var id:int;
      
      public function §_-qU§(param1:int)
      {
         super();
         this.id = param1;
         list.push(this);
      }
      
      public static function valueOf(param1:int) : §_-qU§
      {
         var _loc2_:§_-qU§ = null;
         for each(_loc2_ in list)
         {
            if(param1 == _loc2_.id)
            {
               return _loc2_;
            }
         }
         return null;
      }
      
      public function getId() : int
      {
         return this.id;
      }
   }
}

