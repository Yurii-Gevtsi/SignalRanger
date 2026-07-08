package ru.pragmatix.wormix.pvp.messages.client
{
   import §_-A1K§.§_-TA§;
   import §_-YF§.§_-q2v§;
   import flash.geom.ColorTransform;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.utils.§_-MN§;
   import starling.display.DisplayObject;
   
   public class BattleReconnect
   {
      
      public var id:uint;
      
      public var authKey:String;
      
      public var mainHost:String;
      
      public var mainPort:int;
      
      public var battleId:uint;
      
      public var turnNum:uint;
      
      public var playerNum:uint;
      
      public var lastCommandNum:uint;
      
      public var socialId:uint = 1;
      
      public function BattleReconnect()
      {
         super();
      }
   }
}

