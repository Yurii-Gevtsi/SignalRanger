package ru.pragmatix.wormix.messages.structures
{
   import §_-GQ§.§_-31v§;
   import §_-l1g§.§_-L3Q§;
   import §_-w2i§.§_-Ia§;
   import §_-z1g§.*;
   import flash.geom.Point;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   public class ReconnectToSimpleBattleResultStructure
   {
      
      public var battleId:int;
      
      public var originalBattleId:int;
      
      public var missionId:int;
      
      public var lastTurnNum:int;
      
      public function ReconnectToSimpleBattleResultStructure()
      {
         super();
      }
      
      public function toString() : String
      {
         return "ReconnectToSimpleBattleResultStructure{" + "battleId=" + this.battleId + ",originalBattleId=" + this.originalBattleId + ",missionId=" + this.missionId + ",lastTurnNum=" + this.lastTurnNum + "}";
      }
   }
}

