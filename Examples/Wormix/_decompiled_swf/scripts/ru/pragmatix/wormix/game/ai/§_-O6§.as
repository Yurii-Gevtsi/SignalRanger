package ru.pragmatix.wormix.game.ai
{
   import §_-P1B§.§_-b21§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import starling.events.Event;
   
   public class §_-O6§
   {
      
      public var targetDistance:int = 0;
      
      public var §_-Dz§:int = 0;
      
      public var clearPathPc:Number = 0;
      
      public var §_-v1m§:int = 0;
      
      public function §_-O6§(param1:int, param2:int, param3:Number, param4:int)
      {
         super();
         this.targetDistance = param1;
         this.§_-Dz§ = param2;
         this.clearPathPc = param3;
         this.§_-v1m§ = param4;
      }
      
      public function toString() : String
      {
         return "[TestExplosiveResult] targetDistance = " + this.targetDistance + ", explosionDistance = " + this.§_-Dz§ + ", clearPathPc = " + this.clearPathPc + ", allyDistance = " + this.§_-v1m§;
      }
   }
}

