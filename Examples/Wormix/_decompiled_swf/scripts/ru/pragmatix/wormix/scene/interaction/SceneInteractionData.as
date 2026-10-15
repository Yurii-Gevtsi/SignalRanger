package ru.pragmatix.wormix.scene.interaction
{
   import flash.geom.Point;
   import starling.utils.Pool;
   
   public class SceneInteractionData
   {
      
      public var interactionType:int;
      
      public var location:Point;
      
      public var isTouched:Boolean;
      
      public function SceneInteractionData()
      {
         super();
      }
      
      public function setData(param1:int, param2:Point, param3:Boolean) : void
      {
         this.interactionType = param1;
         this.location = param2;
         this.isTouched = param3;
      }
      
      public function dispose() : void
      {
         Pool.putPoint(this.location);
         this.location = null;
      }
   }
}

