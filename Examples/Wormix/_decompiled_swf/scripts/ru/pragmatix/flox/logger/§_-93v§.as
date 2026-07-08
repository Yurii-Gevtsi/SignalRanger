package ru.pragmatix.flox.logger
{
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.model.§_-d27§;
   
   public class §_-93v§
   {
      
      public static var LOG:§_-93v§ = new §_-93v§("LOG",0,"#2485CA");
      
      public static var DEBUG:§_-93v§ = new §_-93v§("DEBUG",1,"#2485CA");
      
      public static var INFO:§_-93v§ = new §_-93v§("INFO",2,"#2AAFB3");
      
      public static var WARNING:§_-93v§ = new §_-93v§("WARNING",3,"#9C8427");
      
      public static var ERROR:§_-93v§ = new §_-93v§("ERROR",4,"#9C5527");
      
      public static var FATAL:§_-93v§ = new §_-93v§("FATAL",5,"#9C2827");
      
      private var name:String;
      
      private var value:int;
      
      private var color:String;
      
      public function §_-93v§(param1:String, param2:int, param3:String)
      {
         super();
         this.name = param1;
         this.color = param3;
         this.value = param2;
      }
      
      public function getName() : String
      {
         return this.name;
      }
      
      public function getValue() : int
      {
         return this.value;
      }
      
      public function getColor() : String
      {
         return this.color;
      }
      
      public function toString() : String
      {
         return this.getName();
      }
   }
}

