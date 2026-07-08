package ru.pragmatix.wormix.pvp.messages.ex
{
   import §_-z1g§.§_-82X§;
   import ru.pragmatix.wormix.messages.structures.PvpProfileStructure;
   import ru.pragmatix.wormix.objects.§_-01J§;
   
   public class BattleCreated
   {
      
      public static const ID:int = 2006;
      
      public var battleType:PvpBattleType;
      
      public var battleId:uint;
      
      public var wager:§_-82X§;
      
      public var mapId:uint;
      
      public var missionIds:Vector.<int> = new Vector.<int>();
      
      public var playerStructs:Vector.<PvpProfileStructure>;
      
      public var seed:int;
      
      public var reagentsForBattle:Vector.<int>;
      
      public var questId:int;
      
      public var preCalculatedPoints:Array = [];
      
      public function BattleCreated()
      {
         super();
      }
   }
}

