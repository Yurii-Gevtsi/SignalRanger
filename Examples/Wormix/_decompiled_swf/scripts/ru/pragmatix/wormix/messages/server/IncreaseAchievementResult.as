package ru.pragmatix.wormix.messages.server
{
   import §_-51p§.§_-W2e§;
   import §_-52V§.§_-031§;
   import §_-5P§.§_-h2i§;
   import §_-5P§.§_-qg§;
   import §_-62§.§_-G1p§;
   import §_-A1K§.§_-TA§;
   import §_-DJ§.§_-J2l§;
   import §_-L1H§.§_-C3e§;
   import §_-P1z§.*;
   import §_-Tm§.§_-RF§;
   import §_-Z1K§.§_-q24§;
   import §_-hO§.§_-v24§;
   import §_-l1g§.§_-L3Q§;
   import feathers.controls.LayoutGroup;
   import feathers.controls.List;
   import feathers.layout.HorizontalLayout;
   import feathers.layout.VerticalLayout;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.model.rewards.§_-qU§;
   import ru.pragmatix.wormix.objects.Waypoint;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.§_-73v§;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class IncreaseAchievementResult
   {
      
      public var achievementsIndex:Array;
      
      public var achievementsValues:Array;
      
      public var thresholdAchievmensIndex:Array;
      
      public var thresholdAchievmensOldValues:Array;
      
      public var boolAchievements:Array;
      
      public var timeSequence:uint;
      
      public var investedAwardPoints:uint;
      
      public function IncreaseAchievementResult()
      {
         super();
      }
      
      public function toString() : String
      {
         return "IncreaseAchievementResult{" + "ids=" + this.achievementsIndex + ",values=" + this.achievementsValues + ",bools=" + this.boolAchievements + ",thresholdIds=" + this.thresholdAchievmensIndex + ",thresholdValues=" + this.thresholdAchievmensOldValues + ",investedAwardPoints=" + this.investedAwardPoints + ",timeSequence=" + this.timeSequence + "}";
      }
   }
}

