package ru.pragmatix.wormix.pvp.messages.client
{
   import §_-D3A§.§_-TZ§;
   import §_-l1g§.§_-L3Q§;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.LoaderStatus;
   import flash.events.Event;
   import flash.events.ProgressEvent;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import starling.events.Event;
   
   public class PvpEndTurn extends PvpCommand
   {
      
      public var eliminated:Array = [];
      
      public var collectedReagents:Array = [];
      
      public var items:Array = [];
      
      public var battleState:int = 0;
      
      public var forced:Boolean = false;
      
      public var banType:int = 0;
      
      public var banNote:String = "";
      
      public var droppedUnits:Array = [];
      
      public var participantsHealthInPercent:Array = [];
      
      public function PvpEndTurn()
      {
         super();
      }
   }
}

