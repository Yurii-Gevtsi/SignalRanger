package ru.pragmatix.wormix.messages.structures
{
   import §_-CF§.§_-E19§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.weapons.Weapon;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class EndBattleStructure
   {
      
      public var startBattleTime:int;
      
      public var finishBattleTime:int;
      
      public var result:int;
      
      public var type:int;
      
      public var expBonus:int;
      
      public var missionId:int;
      
      public var items:Array;
      
      public var banType:int;
      
      public var banNote:String;
      
      public function EndBattleStructure()
      {
         super();
      }
   }
}

