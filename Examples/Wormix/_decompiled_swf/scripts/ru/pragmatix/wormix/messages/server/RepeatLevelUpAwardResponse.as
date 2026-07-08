package ru.pragmatix.wormix.messages.server
{
   import §_-62§.§_-O2H§;
   import §_-Bv§.§_-G3P§;
   import §_-YF§.§_-q2v§;
   import §_-Z1w§.§_-s1x§;
   import §_-eO§.*;
   import §_-x2Q§.§_-dU§;
   import flash.display.BitmapData;
   import flash.geom.Point;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.§_-k1h§;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.events.Event;
   
   public class RepeatLevelUpAwardResponse
   {
      
      public var status:int;
      
      public var level:int;
      
      public var awards:Array;
      
      public function RepeatLevelUpAwardResponse()
      {
         super();
      }
      
      public function toString() : String
      {
         return "RepeatLevelUpAwardResponse{" + "status=" + this.status + ",level=" + this.level + ",awards=" + this.awards + "}";
      }
   }
}

