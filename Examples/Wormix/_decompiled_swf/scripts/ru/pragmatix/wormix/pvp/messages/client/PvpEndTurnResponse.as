package ru.pragmatix.wormix.pvp.messages.client
{
   import flash.geom.Point;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import starling.events.Event;
   
   public class PvpEndTurnResponse extends PvpCommand
   {
      
      public var eliminated:Array = [];
      
      public var collectedReagents:Array = [];
      
      public var items:Array = [];
      
      public var battleState:int = 0;
      
      public var droppedUnits:Array = [];
      
      public var participantsHealthInPercent:Array = [];
      
      public function PvpEndTurnResponse()
      {
         super();
      }
   }
}

