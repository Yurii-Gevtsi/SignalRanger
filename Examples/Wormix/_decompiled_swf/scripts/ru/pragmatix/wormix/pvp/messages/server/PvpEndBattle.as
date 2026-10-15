package ru.pragmatix.wormix.pvp.messages.server
{
   import §_-K3r§.§_-E3O§;
   import §_-TF§.§_-q1j§;
   import §_-U1i§.§_-O2i§;
   import §_-U1i§.§_-hK§;
   import §_-Z1K§.§_-q24§;
   import §_-k2s§.§_-e1U§;
   import §_-l1g§.§_-L3Q§;
   import §_-zI§.§_-X1q§;
   import flash.geom.Point;
   import flash.text.TextField;
   import ru.pragmatix.flox.social.utils.storage.StorageType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   public class PvpEndBattle
   {
      
      public var battleId:uint;
      
      public var turnNum:uint;
      
      public var commandNum:uint;
      
      public var battleResult:int;
      
      public var ratingPoints:int;
      
      public var rankPoints:int;
      
      public var awards:Array;
      
      public var advPurchases:Array;
      
      public var confirmed:Boolean;
      
      public function PvpEndBattle()
      {
         super();
      }
      
      public function toString() : String
      {
         return "PvpEndBattle{" + "battleId=" + this.battleId + "turnNum=" + this.turnNum + "commandNum=" + this.commandNum + "battleResult=" + this.battleResult + "ratingPoints=" + this.ratingPoints + "rankPoints=" + this.rankPoints + "confirmed=" + this.confirmed + "awards=" + this.awards + "advPurchases=" + this.advPurchases + "}";
      }
   }
}

