package ru.pragmatix.wormix.objects
{
   import starling.events.Event;
   
   public class Direction
   {
      
      public static const LEFT:Direction = new Direction(-1);
      
      public static const RIGHT:Direction = new Direction(1);
      
      private var §_-mf§:int;
      
      public function Direction(param1:int)
      {
         super();
         this.§_-mf§ = param1;
      }
      
      public function get value() : int
      {
         return this.§_-mf§;
      }
   }
}

