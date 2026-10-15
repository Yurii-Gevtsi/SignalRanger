package ru.pragmatix.wormix.pvp.messages.server
{
   import §_-31W§.*;
   import §_-jF§.*;
   import feathers.core.ITextRenderer;
   import flash.events.Event;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreationFailure;
   import ru.pragmatix.wormix.pvp.messages.ex.StartPvpBattle;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   
   public class PvpRetryCommandRequestServer
   {
      
      public var battleId:uint;
      
      public var commandNums:Array;
      
      public var turnNum:uint;
      
      public function PvpRetryCommandRequestServer()
      {
         super();
      }
   }
}

