package ru.pragmatix.wormix.messages.server
{
   import §_-62§.§_-zF§;
   import §_-YF§.*;
   import §_-b1F§.§_-v2Q§;
   import §_-rM§.§_-61j§;
   import feathers.controls.Button;
   import flash.errors.IllegalOperationError;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.getQualifiedClassName;
   import org.swiftsuspenders.§_-71M§;
   import org.swiftsuspenders.§_-T1p§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.client.AchieveLogin;
   import ru.pragmatix.wormix.messages.structures.HeroicMissionStructure;
   import starling.events.Event;
   
   public class ArenaResult
   {
      
      public var battlesCount:int;
      
      public var curSoloMissionId:int;
      
      public var curCooperativeMissionId:int;
      
      public var bossAvailable:Boolean;
      
      public var superBossAvailable:Boolean;
      
      public var heroicStructures:Vector.<HeroicMissionStructure>;
      
      public var delay:int;
      
      public var restoreBattlesDelay:uint;
      
      public var restrictedWagers:Array = [];
      
      public var restrictedWagersLeftTime:Array = [];
      
      public var heroicMissionDailyProgresses:Array = [];
      
      public var defeatContributionMoney:int;
      
      public var extraBattlesTimetable:String;
      
      public var wagerWinAwardToken:int;
      
      public var bossWinAwardToken:int;
      
      public var advPurchases:Array;
      
      public function ArenaResult()
      {
         super();
      }
   }
}

