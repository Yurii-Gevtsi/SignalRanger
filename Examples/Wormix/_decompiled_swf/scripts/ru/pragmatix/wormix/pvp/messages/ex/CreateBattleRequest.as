package ru.pragmatix.wormix.pvp.messages.ex
{
   import §_-z1g§.§_-82X§;
   
   public class CreateBattleRequest extends PvpLogin
   {
      
      public static const ID:int = 1001;
      
      public var mapId:int;
      
      public var wager:§_-82X§;
      
      public var missionIds:Vector.<int>;
      
      public var playerIds:Vector.<uint>;
      
      public var teamIds:Vector.<int>;
      
      public var clientParams:Object;
      
      public var friendSocialNetId:uint = 0;
      
      public function CreateBattleRequest()
      {
         super();
      }
   }
}

