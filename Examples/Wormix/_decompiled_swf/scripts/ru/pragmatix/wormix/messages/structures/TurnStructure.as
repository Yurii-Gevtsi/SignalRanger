package ru.pragmatix.wormix.messages.structures
{
   import §_-B1y§.§_-F1P§;
   import §_-N8§.*;
   import feathers.controls.List;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import starling.display.Quad;
   
   public class TurnStructure
   {
      
      public var turnNum:int;
      
      public var isPlayerTurn:Boolean;
      
      public var endTime:int;
      
      public var damageToPlayer:int;
      
      public var damageToBoss:int;
      
      public var items:Array;
      
      public var deaths:Array;
      
      public var births:Array;
      
      public function TurnStructure()
      {
         super();
      }
      
      public function toString() : String
      {
         return "TurnStructure{" + " turnNum=" + this.turnNum + ",isPlayerTurn=" + this.isPlayerTurn + ",damageToPlayer=" + this.damageToPlayer + ",damageToBoss=" + this.damageToBoss + ",items=" + this.items + ",deaths=" + this.deaths + ",births=" + this.births + " }";
      }
   }
}

