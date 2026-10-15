package ru.pragmatix.wormix.pvp.messages.wager
{
   import §_-73d§.§_-a2s§;
   import §_-82a§.§_-pp§;
   import §_-DJ§.§_-J2l§;
   import com.hurlant.util.der.§_-UO§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.physics.Force;
   import starling.events.Event;
   
   public class WagerBattleRequestResult
   {
      
      public var host:String;
      
      public var port:int;
      
      public var mapId:uint;
      
      public var enemyProfile:UserProfileStructure;
      
      public var userProfile:UserProfileStructure;
      
      public var firstPlayerId:uint;
      
      public var seed:int;
      
      public var battleId:uint;
      
      public var reagentsForBattle:Vector.<int> = new Vector.<int>();
      
      public function WagerBattleRequestResult()
      {
         super();
      }
   }
}

