package ru.pragmatix.wormix.messages.server
{
   import §_-C2t§.§_-92W§;
   import §_-F39§.*;
   import §_-HD§.*;
   import §_-T2h§.*;
   import §_-Y1O§.§_-V23§;
   import §_-Z1K§.§_-q24§;
   import §_-aM§.*;
   import §_-dS§.§_-D3a§;
   import §_-fo§.*;
   import §_-l1K§.§_-d2j§;
   import §_-l1g§.§_-L3Q§;
   import §_-o14§.§_-Dd§;
   import config.§_-p1i§;
   import feathers.controls.LayoutGroup;
   import feathers.controls.NumericStepper;
   import feathers.events.FeathersEventType;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.HorizontalLayout;
   import feathers.layout.RelativePosition;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import flash.display.MovieClip;
   import flash.net.Socket;
   import flash.text.TextFormat;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.*;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.gui.menu.shop.§_-s1V§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   public class EndTurnResponse
   {
      
      public static const SUCCESS:int = 0;
      
      public static const ERROR:int = 1;
      
      public var result:int;
      
      public var battleId:int;
      
      public var turnNum:int;
      
      public var lastTurnNum:int;
      
      public function EndTurnResponse()
      {
         super();
      }
      
      public function toString() : String
      {
         return "EndTurnResponse{" + "result=" + this.result + ",battleId=" + this.battleId + ",turnNum=" + this.turnNum + ",lastTurnNum=" + this.lastTurnNum + "}";
      }
   }
}

