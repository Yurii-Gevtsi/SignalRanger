package ru.pragmatix.flox.logger
{
   import §_-62§.§_-O2H§;
   import §_-62§.§_-W1r§;
   import flash.utils.getQualifiedClassName;
   import starling.events.Event;
   
   public class §_-11S§ extends §_-31f§
   {
      
      public function §_-11S§(param1:Class)
      {
         super(getQualifiedClassName(param1));
         §_-43B§(new ConsolePublisher());
         §_-N1n§(§_-93v§.LOG);
      }
      
      public static function §_-b1a§(param1:Class) : §_-11S§
      {
         return new §_-11S§(param1);
      }
      
      public static function §_-c1Q§(param1:Class) : §_-11S§
      {
         var _loc2_:§_-11S§ = new §_-11S§(param1);
         _loc2_.addPublisher(new TracePublisher());
         return _loc2_;
      }
   }
}

