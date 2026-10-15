package ru.pragmatix.wormix.messages.server
{
   import §_-11L§.*;
   import §_-63M§.§_-F3f§;
   import §_-Bv§.§_-G3P§;
   import §_-C2t§.§_-92W§;
   import §_-DJ§.§_-J2l§;
   import §_-Om§.ReagentsPanel;
   import §_-Y1O§.§_-V23§;
   import §_-YF§.§_-q2v§;
   import §_-Z1K§.§_-q24§;
   import §_-aM§.*;
   import §_-dS§.§_-D3a§;
   import §_-e2r§.CraftEquipPanel;
   import §_-l1g§.§_-L3Q§;
   import §_-lh§.*;
   import §_-y1s§.§_-a1i§;
   import com.hurlant.util.der.*;
   import config.§_-p1i§;
   import feathers.controls.AutoSizeMode;
   import feathers.controls.ScreenNavigator;
   import feathers.controls.ScreenNavigatorItem;
   import feathers.controls.ToggleButton;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.VerticalAlign;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.*;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.display.Image;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class GetAchievementsResult
   {
      
      public var profileId:String;
      
      public var achievementsIndex:Array;
      
      public var achievements:Array;
      
      public var boolAchievements:Array;
      
      public var investedAwardPoints:int;
      
      public var timeSequence:int;
      
      public var isSecure:Boolean;
      
      public function GetAchievementsResult()
      {
         super();
      }
   }
}

