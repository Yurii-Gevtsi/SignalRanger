package ru.pragmatix.wormix.messages.server
{
   import §_-DJ§.§_-J2l§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-g2r§.§_-x8§;
   import §_-j1w§.*;
   import flash.display.DisplayObject;
   import flash.display.Shape;
   import flash.events.TimerEvent;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import starling.events.Event;
   
   public class GetDailyBonusResult
   {
      
      public var result:uint;
      
      public var awards:Array;
      
      public function GetDailyBonusResult()
      {
         super();
      }
      
      public function toString() : String
      {
         return "GetDailyBonusResult{" + "result=" + String(this.result) + ",awards=" + this.awards + "}";
      }
   }
}

