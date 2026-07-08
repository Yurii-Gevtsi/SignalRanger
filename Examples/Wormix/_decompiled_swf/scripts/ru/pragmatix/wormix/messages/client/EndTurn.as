package ru.pragmatix.wormix.messages.client
{
   import §_-52L§.§_-u2x§;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-T1y§;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-ou§;
   import §_-62§.§_-zF§;
   import §_-U1i§.§_-J1§;
   import §_-s20§.§_-J2g§;
   import com.distriqt.extension.inappbilling.PurchaseRequest;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.messages.structures.TurnStructure;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.StringUtil;
   import starling.display.DisplayObject;
   import starling.display.Quad;
   import starling.display.Sprite;
   import starling.events.Event;
   
   public class EndTurn
   {
      
      public var missionId:int;
      
      public var battleId:int;
      
      public var turn:TurnStructure;
      
      public var banType:int;
      
      public var banNote:String = "";
      
      public var sessionKey:String = "";
      
      public function EndTurn(param1:int = 0, param2:int = 0)
      {
         super();
      }
      
      public function toString() : String
      {
         return "EndTurn{" + "battleId=" + this.battleId + ",missionId=" + this.missionId + ",banType=" + this.banType + ",banNote=" + this.banNote + ",sessionKey=" + this.sessionKey + ",turn=" + this.turn + "}";
      }
   }
}

